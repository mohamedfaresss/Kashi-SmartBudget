using Kashi_SmartBudget.Models.DTOs.AI;
using Kashi_SmartBudget.Persistence;
using Kashi_SmartBudget.Services.Reports;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Eventing.Reader;
using System.Text.RegularExpressions;

namespace Kashi_SmartBudget.Services.AiSe
{
    public class AIService : IAIService
    {
        private readonly ApplicationDbContext _db;
        private readonly IReportService _reports;

        public AIService(ApplicationDbContext db, IReportService reports)
        {
            _db = db;
            _reports = reports;
        }

        public async Task<AIResultDto> ForecastMonthlyExpenseAsync(string userId, ExpenseForecastDto dto)
        {
            var monthstart = new DateTime(dto.Year, dto.Month, 1);
            var monthend = monthstart.AddMonths(1);
            var today = DateTime.UtcNow.Date;
            if (today < monthstart)
                today = monthstart;

            var daysPassed = (today - monthstart).Days + 1;
            var dayinmonth = DateTime.DaysInMonth(dto.Year, dto.Month);
            var daysRemaining = Math.Max(dayinmonth - daysPassed, 0);
            //1
            var spentSoFar = await _db.Transactions
                .Where(t => t.UserId == userId &&
                t.Type == "Expense" &&
                t.TransactionDate >= monthstart &&
                t.TransactionDate < today).
                SumAsync(t => (decimal)t.Amount);
            //2
            var dailyAverage = daysPassed > 0 ? spentSoFar / daysPassed : 0m;
            var expectedRemaining = dailyAverage * daysRemaining;
            var expectedTotal = spentSoFar + expectedRemaining;
            var result = new ForecastResultDto
            {
                SpentSoFar = Math.Round(spentSoFar, 2),
                DailyAverage = Math.Round(dailyAverage, 2),
                DaysPassed = daysPassed,
                DaysRemaining = daysRemaining,
                ExpectedRemainingExpense = Math.Round(expectedRemaining, 2),
                ExpectedTotalExpense = Math.Round(expectedTotal, 2)
            };

            return new AIResultDto
            {
                Title = "Monthly Expense Forecast",
                Message =
                   $"Based on your current spending pattern, you are expected to spend approximately {result.ExpectedTotalExpense} by the end of this month.",
                Data = result
            };


        }
        public async Task<AIResultDto> AnalyzeBudgetRiskAsync(string userId, BudgetRiskDto dto)
        {
            var budget = await _db.Budgets
                .FirstOrDefaultAsync(b => b.Id == dto.BudgetId && b.UserId == userId);

            if (budget == null)
            {
                return new AIResultDto
                {
                    Title = "Budget Risk Analysis",
                    Message = "Budget not found or does not belong to the current user.",
                    Data = null
                };
            }

            string categoryName;
            if (budget.CategoryId.HasValue)
            {
                categoryName = await _db.Categories
                    .Where(c => c.Id == budget.CategoryId.Value)
                    .Select(c => c.Name)
                    .FirstOrDefaultAsync() ?? "Unknown Category";
            }
            else
            {
                categoryName = "All Expenses";
            }

            var year = budget.CreatedAt.Year;
            var month = budget.CreatedAt.Month;

            var monthStart = new DateTime(year, month, 1);
            var daysInMonth = DateTime.DaysInMonth(year, month);
            var monthEnd = monthStart.AddMonths(1);

            var today = DateTime.UtcNow.Date;
            if (today < monthStart) today = monthStart;
            if (today > monthEnd) today = monthEnd.AddDays(-1);

            var daysPassed = (today - monthStart).Days + 1;
            var daysRemaining = Math.Max(daysInMonth - daysPassed, 0);

            var q = _db.Transactions
                .Where(t =>
                    t.UserId == userId &&
                    t.Type == "Expense" &&
                    t.TransactionDate >= monthStart &&
                    t.TransactionDate <= today);

            if (budget.CategoryId.HasValue)
            {
                q = q.Where(t => t.CategoryId == budget.CategoryId);
            }

            var spentSoFar = await q.SumAsync(t => (decimal?)t.Amount) ?? 0m;

            var dailyAverage = daysPassed > 0
                ? spentSoFar / daysPassed
                : 0m;

            var expectedEndOfMonth = spentSoFar + (dailyAverage * daysRemaining);

            var willExceed = expectedEndOfMonth > budget.Amount;
            var exceededAmount = willExceed
                ? expectedEndOfMonth - budget.Amount
                : 0m;

            var detail = new BudgetRiskResultDto
            {
                BudgetId = budget.Id,
                CategoryName = categoryName,
                BudgetAmount = Math.Round(budget.Amount, 2),
                SpentSoFar = Math.Round(spentSoFar, 2),
                Year = year,
                Month = month,
                DaysPassed = daysPassed,
                DaysRemaining = daysRemaining,
                DailyAverage = Math.Round(dailyAverage, 2),
                ExpectedEndOfMonth = Math.Round(expectedEndOfMonth, 2),
                WillExceed = willExceed,
                ExpectedExceededAmount = Math.Round(exceededAmount, 2)
            };

            string msg;

            if (willExceed)
            {
                msg =
                    $"You are on track to exceed your \"{detail.CategoryName}\" budget by approximately {detail.ExpectedExceededAmount} by the end of {year}-{month:D2}.";
            }
            else if (detail.SpentPercentage >= 80)
            {
                msg =
                    $"You have already used {detail.SpentPercentage}% of your \"{detail.CategoryName}\" budget. If you keep your current pace, you will stay within the limit, but you are very close.";
            }
            else
            {
                msg =
                    $"Your \"{detail.CategoryName}\" budget looks safe so far. You are expected to stay within the limit if you keep the same spending pattern.";
            }

            return new AIResultDto
            {
                Title = "Budget Risk Analysis",
                Message = msg,
                Data = detail
            };
        }
        public async Task<AIResultDto> SummarizeMonthlyAsync(string userId, MonthlySummaryRequestDto dto)
        {
            var report = await _reports.GetMonthlyReportAsync(userId, dto.Year, dto.Month);

            var summary = new MonthlySummaryDto
            {
                Year = dto.Year,
                Month = dto.Month,
                TotalIncome = report.TotalIncome,
                TotalExpense = report.TotalExpense
            };

            // أعلى تصنيف صرف
            var topCat = report.SpendingByCategory
                .OrderByDescending(c => c.TotalExpense)
                .FirstOrDefault();

            if (topCat != null)
            {
                summary.TopSpendingCategory = topCat.CategoryName;
                summary.TopSpendingAmount = topCat.TotalExpense;
            }

            // أكتر Budget مضغوطة
            var stressed = report.Budgets
                .OrderByDescending(b =>
                    b.BudgetAmount <= 0 ? 0 :
                    (b.Spent / b.BudgetAmount))
                .FirstOrDefault();

            if (stressed != null && stressed.BudgetAmount > 0)
            {
                summary.MostStressedBudgetCategory = stressed.CategoryName ?? "Overall";
                summary.MostStressedBudgetSpentPercentage =
                    Math.Round((stressed.Spent / stressed.BudgetAmount) * 100, 2);
            }

            string monthName = new DateTime(dto.Year, dto.Month, 1)
                .ToString("MMMM");

            string direction;
            if (summary.Net > 0)
                direction = "You are ending the month with a surplus.";
            else if (summary.Net < 0)
                direction = "You are spending more than you earn this month.";
            else
                direction = "Your income and expenses are almost equal this month.";

            string catPart = summary.TopSpendingCategory is null
                ? "We couldn't detect a dominant spending category."
                : $"Your highest spending category is \"{summary.TopSpendingCategory}\" with about {summary.TopSpendingAmount}.";

            string budgetPart;
            if (summary.MostStressedBudgetCategory is null)
            {
                budgetPart = "No budget stress detected yet.";
            }
            else if (summary.MostStressedBudgetSpentPercentage >= 100)
            {
                budgetPart =
                    $"The most stressed budget is \"{summary.MostStressedBudgetCategory}\", which has already reached or exceeded 100% of its limit.";
            }
            else if (summary.MostStressedBudgetSpentPercentage >= 80)
            {
                budgetPart =
                    $"The most stressed budget is \"{summary.MostStressedBudgetCategory}\", at about {summary.MostStressedBudgetSpentPercentage}% of its limit.";
            }
            else
            {
                budgetPart =
                    $"Your budgets look generally safe. The most used one is \"{summary.MostStressedBudgetCategory}\" at about {summary.MostStressedBudgetSpentPercentage}% of its limit.";
            }

            var message =
                $"Here is your financial summary for {monthName} {dto.Year}: " +
                $"You earned a total of {summary.TotalIncome} and spent {summary.TotalExpense}, " +
                $"resulting in a net of {summary.Net}. " +
                $"{direction} {catPart} {budgetPart}";

            return new AIResultDto
            {
                Title = "Monthly Financial Summary",
                Message = message,
                Data = summary
            };
        }

        public async Task<AIResultDto> ExtractTransactionFromTextAsync(
     string userId,
     TextToTransactionDto dto)
        {
            var text = dto.Text?.Trim();
            if (string.IsNullOrWhiteSpace(text))
            {
                return new AIResultDto
                {
                    Title = "Text → Transaction",
                    Message = "Text is empty.",
                    Data = null
                };
            }

            var lowered = text.ToLower();

            // 1) استخرج أول رقم في الجملة (بالإنجليزي)
            var match = Regex.Match(lowered, @"\d+(\.\d+)?");
            if (!match.Success)
            {
                return new AIResultDto
                {
                    Title = "Text → Transaction",
                    Message = "Could not detect any numeric amount in the text.",
                    Data = null
                };
            }

            decimal amount = decimal.Parse(match.Value);

            // 2) حدّد النوع (Income / Expense) بكلمتين بسيطين كبداية
            string type = "Expense";
            string[] incomeKeywords =
            {
        "salary", "bonus", "income", "reward", "transfer",
        "مرتب", "راتب", "قبضت", "دخل", "حوّل", "حوالي"
    };

            if (incomeKeywords.Any(k => lowered.Contains(k)))
            {
                type = "Income";
            }

            // 3) حاول تطابق الكاتيجوري من الـ DB
            // نجيب Global + User Categories
            var cats = await _db.Categories
                .Where(c => c.UserId == null || c.UserId == userId)
                .ToListAsync();

            Guid? categoryId = null;
            string? categoryName = null;

            foreach (var c in cats)
            {
                if (string.IsNullOrWhiteSpace(c.Name))
                    continue;

                // match case-insensitive
                if (lowered.Contains(c.Name.ToLower()))
                {
                    categoryId = c.Id;
                    categoryName = c.Name;
                    break;
                }
            }

            if (categoryName == null)
            {
                categoryName = "Uncategorized";
            }

            var extracted = new ExtractedTransactionDto
            {
                Amount = amount,
                CategoryId = categoryId,
                CategoryName = categoryName,
                Type = type,
                TransactionDate = DateTime.UtcNow,
                Description = text
            };

            var msg =
                $"Detected a {type} transaction of about {amount}. " +
                $"Category: {categoryName}.";

            return new AIResultDto
            {
                Title = "Text → Transaction",
                Message = msg,
                Data = extracted
            };
        }
    }
}
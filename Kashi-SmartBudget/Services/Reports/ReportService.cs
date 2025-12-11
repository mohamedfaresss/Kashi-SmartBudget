using Kashi_SmartBudget.Models.DTOs.Reports;
using Kashi_SmartBudget.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Kashi_SmartBudget.Services.Reports
{
    public class ReportService : IReportService
    {
        private readonly ApplicationDbContext _db;
        public ReportService(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<MonthlyReportDto> GetMonthlyReportAsync(string userId, int year, int month)
        {
            var start = new DateTime(year, month, 1);
            var end = start.AddMonths(1);

            var monthTransactions = await _db.Transactions
                .Where(t => t.UserId == userId &&
                            t.TransactionDate >= start &&
                            t.TransactionDate < end)
                .ToListAsync();

            var totalIncome = monthTransactions
                .Where(t => t.Type == "Income")
                .Sum(t => t.Amount);

            var totalExpense = monthTransactions
                .Where(t => t.Type == "Expense")
                .Sum(t => t.Amount);

            var categories = await _db.Categories.ToListAsync();
            var categoriesDict = categories.ToDictionary(c => c.Id, c => c.Name);

            var spendingByCategory = monthTransactions
                .Where(t => t.Type == "Expense")
                .GroupBy(t => t.CategoryId)
                .Select(g =>
                {
                    Guid? catId = g.Key;
                    string name = "Uncategorized";

                    if (catId.HasValue && categoriesDict.TryGetValue(catId.Value, out var n))
                        name = n;

                    return new CategorySpendingDto
                    {
                        CategoryId = catId,
                        CategoryName = name,
                        TotalExpense = g.Sum(x => x.Amount)
                    };
                })
                .OrderByDescending(x => x.TotalExpense)
                .ToList();

            var monthBudgets = await _db.Budgets
                .Where(b => b.UserId == userId &&
                            b.CreatedAt.Year == year &&
                            b.CreatedAt.Month == month)
                .ToListAsync();

            var budgetsOverview = new List<BudgetOverviewDto>();

            foreach (var b in monthBudgets)
            {
                decimal spent;

                if (b.CategoryId.HasValue)
                {
                    spent = monthTransactions
                        .Where(t => t.Type == "Expense" &&
                                    t.CategoryId == b.CategoryId)
                        .Sum(t => t.Amount);
                }
                else
                {
                    spent = totalExpense;
                }

                string? categoryName = null;
                if (b.CategoryId.HasValue && categoriesDict.TryGetValue(b.CategoryId.Value, out var n))
                    categoryName = n;

                budgetsOverview.Add(new BudgetOverviewDto
                {
                    Id = b.Id,
                    CategoryId = b.CategoryId,
                    CategoryName = categoryName,
                    BudgetAmount = b.Amount,
                    Period = b.Period,
                    Spent = spent
                });
            }

            return new MonthlyReportDto
            {
                Year = year,
                Month = month,
                TotalIncome = totalIncome,
                TotalExpense = totalExpense,
                SpendingByCategory = spendingByCategory,
                Budgets = budgetsOverview
            };
        }
        public async Task<List<DailySpendingDto>> GetDailySpendingAsync(string userId, int year, int month)
        {
            var start = new DateTime(year, month, 1);
            var end = start.AddMonths(1);

            return await _db.Transactions
                .Where(t => t.UserId == userId &&
                            t.Type == "Expense" &&
                            t.TransactionDate >= start &&
                            t.TransactionDate < end)
                .GroupBy(t => t.TransactionDate.Date)
                .Select(g => new DailySpendingDto
                {
                    Date = g.Key,
                    TotalExpense = g.Sum(x => x.Amount)
                })
                .OrderBy(d => d.Date)
                .ToListAsync();
        }

        public async Task<YearlyReportDto> GetYearlyReportAsync(string userId, int year)
        {
            var yearStart = new DateTime(year, 1, 1);
            var yearEnd = yearStart.AddYears(1);

            var yearTransactions = await _db.Transactions
                .Where(t => t.UserId == userId &&
                            t.TransactionDate >= yearStart &&
                            t.TransactionDate < yearEnd)
                .ToListAsync();

            var totalIncome = yearTransactions
                .Where(t => t.Type == "Income")
                .Sum(t => t.Amount);

            var totalExpense = yearTransactions
                .Where(t => t.Type == "Expense")
                .Sum(t => t.Amount);

            // Monthly breakdown
            var monthly = new List<MonthlyAggregateDto>();

            for (int month = 1; month <= 12; month++)
            {
                var monthStart = new DateTime(year, month, 1);
                var monthEnd = monthStart.AddMonths(1);

                var monthTx = yearTransactions
                    .Where(t => t.TransactionDate >= monthStart &&
                                t.TransactionDate < monthEnd)
                    .ToList();

                var mIncome = monthTx
                    .Where(t => t.Type == "Income")
                    .Sum(t => t.Amount);

                var mExpense = monthTx
                    .Where(t => t.Type == "Expense")
                    .Sum(t => t.Amount);

                monthly.Add(new MonthlyAggregateDto
                {
                    Month = month,
                    TotalIncome = mIncome,
                    TotalExpense = mExpense
                });
            }

            // Top categories (expenses فقط خلال السنة)
            var categories = await _db.Categories.ToListAsync();
            var categoriesDict = categories.ToDictionary(c => c.Id, c => c.Name);

            var topCategories = yearTransactions
                .Where(t => t.Type == "Expense")
                .GroupBy(t => t.CategoryId)
                .Select(g =>
                {
                    Guid? catId = g.Key;
                    string name = "Uncategorized";

                    if (catId.HasValue && categoriesDict.TryGetValue(catId.Value, out var n))
                        name = n;

                    return new CategorySpendingDto
                    {
                        CategoryId = catId,
                        CategoryName = name,
                        TotalExpense = g.Sum(x => x.Amount)
                    };
                })
                .OrderByDescending(x => x.TotalExpense)
                .Take(10) // Top 10
                .ToList();

            return new YearlyReportDto
            {
                Year = year,
                TotalIncome = totalIncome,
                TotalExpense = totalExpense,
                MonthlyBreakdown = monthly,
                TopCategories = topCategories
            };
        }

    }
}
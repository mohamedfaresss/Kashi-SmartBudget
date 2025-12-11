namespace Kashi_SmartBudget.Models.DTOs.AI
{
    public class BudgetRiskResultDto
    {
       
    
            public Guid BudgetId { get; set; }
            public string? CategoryName { get; set; }

            public decimal BudgetAmount { get; set; }
            public decimal SpentSoFar { get; set; }

            public int Year { get; set; }
            public int Month { get; set; }
            public int DaysPassed { get; set; }
            public int DaysRemaining { get; set; }

            public decimal DailyAverage { get; set; }
            public decimal ExpectedEndOfMonth { get; set; }

            public bool WillExceed { get; set; }
            public decimal ExpectedExceededAmount { get; set; }

            public decimal SpentPercentage =>
                BudgetAmount <= 0 ? 0 : Math.Round((SpentSoFar / BudgetAmount) * 100, 2);
        }
    }


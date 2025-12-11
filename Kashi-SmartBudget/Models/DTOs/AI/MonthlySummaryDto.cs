namespace Kashi_SmartBudget.Models.DTOs.AI
{
    public class MonthlySummaryDto
    {
        public int Year { get; set; }
        public int Month { get; set; }

        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal Net => TotalIncome - TotalExpense;

        public string? TopSpendingCategory { get; set; }
        public decimal TopSpendingAmount { get; set; }

        public string? MostStressedBudgetCategory { get; set; }
        public decimal? MostStressedBudgetSpentPercentage { get; set; }
    }
}

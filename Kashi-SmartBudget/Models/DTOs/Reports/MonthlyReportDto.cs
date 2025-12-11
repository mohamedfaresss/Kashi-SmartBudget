namespace Kashi_SmartBudget.Models.DTOs.Reports
{
    public class MonthlyReportDto
    {
        public int Year { get; set; }
        public int Month { get; set; }

        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal Net => TotalIncome - TotalExpense;

        public List<CategorySpendingDto> SpendingByCategory { get; set; } = new();
        public List<BudgetOverviewDto> Budgets { get; set; } = new();
    }
}

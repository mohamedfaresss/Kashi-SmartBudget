namespace Kashi_SmartBudget.Models.DTOs.Reports
{
    public class YearlyReportDto
    {
        public int Year { get; set; }

        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal Net => TotalIncome - TotalExpense;

        public List<MonthlyAggregateDto> MonthlyBreakdown { get; set; } = new();
        public List<CategorySpendingDto> TopCategories { get; set; } = new();
    }
}

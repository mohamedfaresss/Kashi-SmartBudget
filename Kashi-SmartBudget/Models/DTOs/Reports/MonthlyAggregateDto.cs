namespace Kashi_SmartBudget.Models.DTOs.Reports
{
    public class MonthlyAggregateDto
    {
        public int Month { get; set; }          // 1..12
        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal Net => TotalIncome - TotalExpense;
    }
}

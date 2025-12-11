namespace Kashi_SmartBudget.Models.DTOs.Reports
{
    public class CategorySpendingDto
    {
        public Guid? CategoryId { get; set; }
        public string CategoryName { get; set; } = "Uncategorized";
        public decimal TotalExpense { get; set; }
    }

}

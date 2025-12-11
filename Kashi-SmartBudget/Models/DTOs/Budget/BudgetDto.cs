namespace Kashi_SmartBudget.Models
{
    public class BudgetDto
    {
        public Guid Id { get; set; }
        public Guid? CategoryId { get; set; }
        public decimal Amount { get; set; }
        public string Period { get; set; } = "Monthly";
        public DateTime CreatedAt { get; set; }
    }
}

namespace Kashi_SmartBudget.Models
{
    public class TransactionDto
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public Guid? CategoryId { get; set; }

        public decimal Amount { get; set; }
        public string Type { get; set; } = default!;
        public DateTime TransactionDate { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

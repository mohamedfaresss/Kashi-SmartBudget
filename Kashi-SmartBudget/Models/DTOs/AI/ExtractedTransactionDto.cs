namespace Kashi_SmartBudget.Models.DTOs.AI
{
    public class ExtractedTransactionDto
    {
        public decimal Amount { get; set; }
        public Guid? CategoryId { get; set; }
        public string? CategoryName { get; set; }

        public string Type { get; set; } = "Expense"; // Expense / Income
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
        public string? Description { get; set; }
    }
}

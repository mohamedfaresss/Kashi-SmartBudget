using System;

namespace Kashi.Domain
{
    public class Transaction
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid AccountId { get; set; }
        public string UserId { get; set; } = default!;
        public Guid? CategoryId { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; } = "Expense"; // Expense | Income | Transfer
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

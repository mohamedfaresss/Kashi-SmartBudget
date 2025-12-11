using System;

namespace Kashi.Domain
{
    public class Budget
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string UserId { get; set; } = default!;
        public Guid? CategoryId { get; set; }
        public decimal Amount { get; set; }
        public string Period { get; set; } = "Monthly";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

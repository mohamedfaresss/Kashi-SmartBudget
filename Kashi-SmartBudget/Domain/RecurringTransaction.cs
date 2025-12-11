using System;

namespace Kashi.Domain
{
    public class RecurringTransaction
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string UserId { get; set; } = default!;
        public Guid AccountId { get; set; }
        public Guid? CategoryId { get; set; }
        public decimal Amount { get; set; }
        public string CronExpression { get; set; } = default!;
        public DateTime? NextRun { get; set; }
        public bool Active { get; set; } = true;
    }
}

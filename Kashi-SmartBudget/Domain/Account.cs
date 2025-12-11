using System;

namespace Kashi.Domain
{
    public class Account
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string UserId { get; set; } = default!;
        public string Name { get; set; } = default!;
        public decimal Balance { get; set; } = 0;
        public string Currency { get; set; } = "EGP";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kashi.Domain
{
    public class RefreshToken
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string TokenHash { get; set; } = default!;
        public string UserId { get; set; } = default!;
        public DateTime ExpiresAt { get; set; }
        public bool Revoked { get; set; } = false;
        public string CreatedByIp { get; set; } = default!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // 👇 raw token اللي يرجع للعميل ولا يتخزن في DB
        [NotMapped]
        public string? Token { get; set; }
    }
}

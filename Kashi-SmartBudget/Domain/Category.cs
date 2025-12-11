using System;

namespace Kashi.Domain
{
    public class Category
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? UserId { get; set; } // null => global category
        public string Name { get; set; } = default!;
        public string? Icon { get; set; }
    }
}

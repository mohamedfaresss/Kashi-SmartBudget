using System.ComponentModel.DataAnnotations;

namespace Kashi_SmartBudget.Models
{
    public class CreateCategoryDto
    {
        [Required]
        public string Name { get; set; } = default!;

        public string? Icon { get; set; }

        // إذا true → category global (UserId = null)
        public bool IsGlobal { get; set; } = false;
    }
}

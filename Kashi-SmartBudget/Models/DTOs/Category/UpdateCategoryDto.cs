using System.ComponentModel.DataAnnotations;

namespace Kashi_SmartBudget.Models
{
    public class UpdateCategoryDto
    {
        [Required]
        public string Name { get; set; } = default!;

        public string? Icon { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Kashi_SmartBudget.Models
{
    public class UpdateBudgetDto
    {
        public Guid? CategoryId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        public string Period { get; set; } = "Monthly";
    }
}

using System.ComponentModel.DataAnnotations;

namespace Kashi_SmartBudget.Models
{
    public class CreateBudgetDto
    {
        public Guid? CategoryId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        // e.g. "Monthly", "Weekly", "Yearly" — default "Monthly"
        public string Period { get; set; } = "Monthly";
    }
}

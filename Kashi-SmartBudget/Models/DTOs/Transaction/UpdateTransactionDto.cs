using System.ComponentModel.DataAnnotations;

namespace Kashi_SmartBudget.Models
{
    public class UpdateTransactionDto
    {
        [Required]
        public Guid AccountId { get; set; }

        public Guid? CategoryId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }

        [Required]
        [RegularExpression("Expense|Income|Transfer")]
        public string Type { get; set; } = "Expense";

        public DateTime? TransactionDate { get; set; }

        public string? Description { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Kashi_SmartBudget.Models
{
    public class CreateTransactionDto
    {
        [Required]
        public Guid AccountId { get; set; }

        public Guid? CategoryId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }

        // Expense | Income | Transfer (Transfer لسه مش هنتعامل معاها كتحويل حقيقي دلوقتي)
        [Required]
        [RegularExpression("Expense|Income|Transfer")]
        public string Type { get; set; } = "Expense";

        public DateTime? TransactionDate { get; set; }

        public string? Description { get; set; }
    }
}

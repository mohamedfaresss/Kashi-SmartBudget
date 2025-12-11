using System.ComponentModel.DataAnnotations;

namespace Kashi_SmartBudget.Models.DTOs.Account
{
    public class CreateAccountDto
    {
        [Required]
        public string Name { get; set; } = default!;

        public decimal? InitialBalance { get; set; } = 0m;

        [Required]
        public string Currency { get; set; } = "EGP";
    }
}

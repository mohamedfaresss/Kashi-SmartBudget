using System.ComponentModel.DataAnnotations;

namespace Kashi_SmartBudget.Models.DTOs.Account
{
    public class UpdateAccountDto
    {
        [Required]
        public string Name { get; set; } = default!;

        [Required]
        public string Currency { get; set; } = default!;
    }
}

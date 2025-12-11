using Microsoft.AspNetCore.Identity;

namespace Kashi_SmartBudget.Domain
{
    public class ApplicationUser:IdentityUser 
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string FullName { get; set; } = default!;
        public string Country { get; set; } = default!;
        public string PhoneNumberCustom { get; set; } = default!;
        public string PreferredCurrency { get; set; } = "EGP";


    }
}

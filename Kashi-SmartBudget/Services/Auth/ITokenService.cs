using Kashi.Domain;
using Kashi_SmartBudget.Domain;

namespace Kashi_SmartBudget.Services.Auth
{
    public interface ITokenService
    {
        string CreateAccessToken(ApplicationUser user);
        RefreshToken CreateRefreshToken(string userId, string createdByIp);
    }
}

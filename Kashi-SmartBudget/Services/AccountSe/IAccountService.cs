using Kashi.Domain;
using Kashi_SmartBudget.Models.DTOs.Account;

namespace Kashi_SmartBudget.Services.Accountse
{
    public interface IAccountService
    {
        Task<AccountDto> CreateAsync(string userId, CreateAccountDto dto);
        Task<IEnumerable<AccountDto>> GetAllAsync(string userId);
        Task<AccountDto> GetByIdAsync(string userId, Guid id);
        Task<bool> UpdateAsync(string userId, Guid id, UpdateAccountDto dto);
        Task<bool> DeleteAsync(string userId, Guid id);
        Task<decimal?> GetBalanceAsync(string userId, Guid id);





    }
}

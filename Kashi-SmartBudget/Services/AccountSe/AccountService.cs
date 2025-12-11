using Kashi.Domain;
using Kashi_SmartBudget.Models.DTOs.Account;
using Kashi_SmartBudget.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Kashi_SmartBudget.Services.Accountse
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationDbContext _db;

        public AccountService(ApplicationDbContext  db)
        {
           _db = db;
        }


        public async Task<AccountDto> CreateAsync(string userId, CreateAccountDto dto)
        {
            var acc = new Account ();
            {
                acc.UserId = userId;
                acc.Name = dto.Name;
                acc.Balance = dto.InitialBalance ?? 0m;
                acc.Currency = dto.Currency;
            }
            _db.Accounts.Add(acc);
            await _db.SaveChangesAsync();
            return new AccountDto(acc.Id, acc.Name, acc.Balance, acc.Currency, acc.CreatedAt);

        }
        public async Task<IEnumerable<AccountDto>> GetAllAsync(string userId)
        {
            return await _db.Accounts
                .AsNoTracking()
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.CreatedAt)
                .Select(a => new AccountDto(a.Id, a.Name, a.Balance, a.Currency, a.CreatedAt))
                .ToListAsync();
        }

        public async Task<AccountDto> GetByIdAsync(string userId, Guid id)
        {
            var a=await _db.Accounts
                .AsNoTracking()
                .FirstOrDefaultAsync(x=>x.Id == id && x.UserId==userId);

            if (a==null)
            {
                return null;
            }

            return new AccountDto(a.Id, a.Name, a.Balance, a.Currency, a.CreatedAt);

        }

        public async Task<bool> UpdateAsync(string userId, Guid id, UpdateAccountDto dto)
        {
            var a = await _db.Accounts.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
            if (a == null) return false;
            a.Name=dto.Name;
            a.Currency=dto.Currency;
            await _db.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteAsync(string userId, Guid id)
        {
            var a = await _db.Accounts.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
            if (a == null) return false;
            if (a.Balance != 0m) return false; // prevent deletion if non-zero balance
            _db.Accounts.Remove(a);
            return true;


        }
        public async Task<decimal?> GetBalanceAsync(string userId, Guid id)
        {
            var a = await _db.Accounts
                .AsNoTracking().
                FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
            if (a == null) return null;
            return a.Balance;

        }













    }
}

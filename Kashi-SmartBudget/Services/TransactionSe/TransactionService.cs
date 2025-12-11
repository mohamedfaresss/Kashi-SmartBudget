using Kashi.Domain; // entity Budget lives here
using ApiDtos = Kashi_SmartBudget.Models; // alias to avoid ambiguous reference
using Kashi_SmartBudget.Persistence;
using Microsoft.EntityFrameworkCore;
using Kashi_SmartBudget.Models;

namespace Kashi_SmartBudget.Services.TransactionSe
{
    public class TransactionService : ITransactionService
    {
        private readonly ApplicationDbContext _db;

        public TransactionService(ApplicationDbContext db)
        {
            _db = db;
        }

        // تحديد الإشارة حسب نوع العملية
        private static decimal GetSignedAmount(string type, decimal amount)
        {
            return type switch
            {
                "Income" => amount,
                "Expense" => -amount,
                // Transfer حالياً مش بنغيّر بيها الرصيد (لحد ما نبني منطق تحويل بين حسابين)
                _ => 0m
            };
        }

        public async Task<TransactionDto?> CreateAsync(string userId, CreateTransactionDto dto)
        {
            var account = await _db.Accounts
                .FirstOrDefaultAsync(a => a.Id == dto.AccountId && a.UserId == userId);

            if (account == null)
                return null;

            var trx = new Transaction
            {
                UserId = userId,
                AccountId = dto.AccountId,
                CategoryId = dto.CategoryId,
                Amount = dto.Amount,
                Type = dto.Type,
                TransactionDate = dto.TransactionDate ?? DateTime.UtcNow,
                Description = dto.Description,
                CreatedAt = DateTime.UtcNow
            };

            var change = GetSignedAmount(trx.Type, trx.Amount);
            account.Balance += change;

            _db.Transactions.Add(trx);
            await _db.SaveChangesAsync();

            return new TransactionDto
            {
                Id = trx.Id,
                AccountId = trx.AccountId,
                CategoryId = trx.CategoryId,
                Amount = trx.Amount,
                Type = trx.Type,
                TransactionDate = trx.TransactionDate,
                Description = trx.Description,
                CreatedAt = trx.CreatedAt
            };
        }

        public async Task<IEnumerable<TransactionDto>> GetAllAsync(
            string userId,
            DateTime? from = null,
            DateTime? to = null,
            Guid? accountId = null,
            Guid? categoryId = null,
            string? type = null)
        {
            var q = _db.Transactions.AsQueryable();

            q = q.Where(t => t.UserId == userId);

            if (from.HasValue) q = q.Where(t => t.TransactionDate >= from.Value);
            if (to.HasValue) q = q.Where(t => t.TransactionDate <= to.Value);
            if (accountId.HasValue) q = q.Where(t => t.AccountId == accountId.Value);
            if (categoryId.HasValue) q = q.Where(t => t.CategoryId == categoryId.Value);
            if (!string.IsNullOrWhiteSpace(type)) q = q.Where(t => t.Type == type);

            var list = await q
                .OrderByDescending(t => t.TransactionDate)
                .Select(t => new TransactionDto
                {
                    Id = t.Id,
                    AccountId = t.AccountId,
                    CategoryId = t.CategoryId,
                    Amount = t.Amount,
                    Type = t.Type,
                    TransactionDate = t.TransactionDate,
                    Description = t.Description,
                    CreatedAt = t.CreatedAt
                })
                .ToListAsync();

            return list;
        }

        public async Task<TransactionDto?> GetByIdAsync(string userId, Guid id)
        {
            var t = await _db.Transactions
                .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            if (t == null)
                return null;

            return new TransactionDto
            {
                Id = t.Id,
                AccountId = t.AccountId,
                CategoryId = t.CategoryId,
                Amount = t.Amount,
                Type = t.Type,
                TransactionDate = t.TransactionDate,
                Description = t.Description,
                CreatedAt = t.CreatedAt
            };
        }

        public async Task<bool> UpdateAsync(string userId, Guid id, UpdateTransactionDto dto)
        {
            var t = await _db.Transactions
                .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            if (t == null)
                return false;

            var account = await _db.Accounts
                .FirstOrDefaultAsync(a => a.Id == t.AccountId && a.UserId == userId);

            if (account == null)
                return false;

            var oldChange = GetSignedAmount(t.Type, t.Amount);

            // هنا حافظنا على نفس الـ AccountId، لو حابب تغيره نوسع المنطق بعدين
            t.AccountId = dto.AccountId;
            t.CategoryId = dto.CategoryId;
            t.Amount = dto.Amount;
            t.Type = dto.Type;
            t.TransactionDate = dto.TransactionDate ?? t.TransactionDate;
            t.Description = dto.Description;

            var newChange = GetSignedAmount(t.Type, t.Amount);
            var delta = newChange - oldChange;

            account.Balance += delta;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(string userId, Guid id)
        {
            var t = await _db.Transactions
                .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            if (t == null)
                return false;

            var account = await _db.Accounts
                .FirstOrDefaultAsync(a => a.Id == t.AccountId && a.UserId == userId);

            if (account == null)
                return false;

            var change = GetSignedAmount(t.Type, t.Amount);
            account.Balance -= change;

            _db.Transactions.Remove(t);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
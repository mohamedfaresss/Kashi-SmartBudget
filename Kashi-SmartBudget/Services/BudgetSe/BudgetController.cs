using Kashi.Domain; // entity Budget lives here
using ApiDtos = Kashi_SmartBudget.Models; // alias to avoid ambiguous reference
using Kashi_SmartBudget.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Kashi_SmartBudget.Services.BudgetSe
{
    public class BudgetService : IBudgetService
    {
        private readonly ApplicationDbContext _db;

        public BudgetService(ApplicationDbContext db) => _db = db;

        public async Task<ApiDtos.BudgetDto> CreateAsync(string userId, ApiDtos.CreateBudgetDto dto)
        {
            var budget = new Budget
            {
                UserId = userId,
                CategoryId = dto.CategoryId,
                Amount = dto.Amount,
                Period = string.IsNullOrWhiteSpace(dto.Period) ? "Monthly" : dto.Period,
                CreatedAt = DateTime.UtcNow
            };

            _db.Budgets.Add(budget);
            await _db.SaveChangesAsync();

            return new ApiDtos.BudgetDto
            {
                Id = budget.Id,
                CategoryId = budget.CategoryId,
                Amount = budget.Amount,
                Period = budget.Period,
                CreatedAt = budget.CreatedAt
            };
        }

        public async Task<IEnumerable<ApiDtos.BudgetDto>> GetAllAsync(string userId)
        {
            return await _db.Budgets
                .AsNoTracking()
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.CreatedAt)
                .Select(b => new ApiDtos.BudgetDto
                {
                    Id = b.Id,
                    CategoryId = b.CategoryId,
                    Amount = b.Amount,
                    Period = b.Period,
                    CreatedAt = b.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<ApiDtos.BudgetDto?> GetByIdAsync(string userId, Guid id)
        {
            var b = await _db.Budgets
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            if (b == null) return null;

            return new ApiDtos.BudgetDto
            {
                Id = b.Id,
                CategoryId = b.CategoryId,
                Amount = b.Amount,
                Period = b.Period,
                CreatedAt = b.CreatedAt
            };
        }

        public async Task<IEnumerable<ApiDtos.BudgetDto>> GetByMonthYearAsync(string userId, int month, int year)
        {
            return await _db.Budgets
                .AsNoTracking()
                .Where(b => b.UserId == userId && b.CreatedAt.Month == month && b.CreatedAt.Year == year)
                .OrderByDescending(b => b.CreatedAt)
                .Select(b => new ApiDtos.BudgetDto
                {
                    Id = b.Id,
                    CategoryId = b.CategoryId,
                    Amount = b.Amount,
                    Period = b.Period,
                    CreatedAt = b.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<bool> UpdateAsync(string userId, Guid id, ApiDtos.UpdateBudgetDto dto)
        {
            var b = await _db.Budgets.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
            if (b == null) return false;

            b.CategoryId = dto.CategoryId;
            b.Amount = dto.Amount;
            b.Period = string.IsNullOrWhiteSpace(dto.Period) ? b.Period : dto.Period;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(string userId, Guid id)
        {
            var b = await _db.Budgets.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
            if (b == null) return false;

            _db.Budgets.Remove(b);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}

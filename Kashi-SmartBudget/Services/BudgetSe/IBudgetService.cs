using Kashi_SmartBudget.Models;

namespace Kashi_SmartBudget.Services.BudgetSe
{
    public interface IBudgetService
    {
        Task<BudgetDto> CreateAsync(string userId, CreateBudgetDto dto);
        Task<IEnumerable<BudgetDto>> GetAllAsync(string userId);
        Task<BudgetDto?> GetByIdAsync(string userId, Guid id);
        Task<IEnumerable<BudgetDto>> GetByMonthYearAsync(string userId, int month, int year); // helper
        Task<bool> UpdateAsync(string userId, Guid id, UpdateBudgetDto dto);
        Task<bool> DeleteAsync(string userId, Guid id);
    }
}

using Kashi_SmartBudget.Models;

namespace Kashi_SmartBudget.Services.CategorySe
{
    public interface ICategoryService
    {
        Task<CategoryDto> CreateAsync(string? userId, CreateCategoryDto dto);
        Task<IEnumerable<CategoryDto>> GetAllAsync(string? userId);
        Task<CategoryDto?> GetByIdAsync(string? userId, Guid id);
        Task<bool> UpdateAsync(string? userId, Guid id, UpdateCategoryDto dto);
        Task<bool> DeleteAsync(string? userId, Guid id);


    }
}

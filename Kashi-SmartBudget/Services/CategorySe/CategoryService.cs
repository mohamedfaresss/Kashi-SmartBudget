using Kashi.Domain;
using Kashi_SmartBudget.Models;
using Kashi_SmartBudget.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Kashi_SmartBudget.Services.CategorySe
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _db;
        public CategoryService(ApplicationDbContext db) => _db = db;

        public async Task<CategoryDto> CreateAsync(string? userId, CreateCategoryDto dto)
        {
            var cat = new Category
            {
                Name = dto.Name,
                Icon = dto.Icon,
                UserId = dto.IsGlobal ? null : userId

            };
            _db.Categories.Add(cat);
            await _db.SaveChangesAsync();

            return new CategoryDto
            {
                Id = cat.Id,
                Name = cat.Name,
                Icon = cat.Icon,
                UserId = cat.UserId
            };


        }
        public async Task<IEnumerable<CategoryDto>> GetAllAsync(string? userId)
        {
           return await _db.Categories
                .Where(c => c.UserId == null || c.UserId == userId)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Icon = c.Icon,
                    UserId = c.UserId
                }).ToListAsync();
        }
        public async Task<CategoryDto?> GetByIdAsync(string? userId, Guid id)
        {
            var c = await _db.Categories
                .FirstOrDefaultAsync(x => x.Id == id && (x.UserId == null || x.UserId == userId));

            if (c == null) return null;

            return new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Icon = c.Icon,
                UserId = c.UserId
            };
        }
        public async Task<bool> UpdateAsync(string? userId, Guid id, UpdateCategoryDto dto)
        {
            var c=await _db.Categories
                .FirstOrDefaultAsync(x => x.Id == id && (x.UserId == null || x.UserId == userId));
            if (c == null)
            
                return false;
            
            c.Name = dto.Name;
            c.Icon = dto.Icon;

            await _db.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteAsync(string? userId, Guid id)
        {
            var c = await _db.Categories
                            .FirstOrDefaultAsync(x => x.Id == id && (x.UserId == null || x.UserId == userId));

            if (c == null) return false;

            _db.Categories.Remove(c);
            await _db.SaveChangesAsync();
            return true;

        }
    }
}

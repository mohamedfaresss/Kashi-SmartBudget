using Kashi_SmartBudget.Models;
namespace Kashi_SmartBudget.Services.TransactionSe
{
    public interface ITransactionService
    {
        Task<TransactionDto?> CreateAsync(string userId, CreateTransactionDto dto);

        Task<IEnumerable<TransactionDto>> GetAllAsync(
            string userId,
            DateTime? from = null,
            DateTime? to = null,
            Guid? accountId = null,
            Guid? categoryId = null,
            string? type = null
        );

        Task<TransactionDto?> GetByIdAsync(string userId, Guid id);

        Task<bool> UpdateAsync(string userId, Guid id, UpdateTransactionDto dto);

        Task<bool> DeleteAsync(string userId, Guid id);
    }
}

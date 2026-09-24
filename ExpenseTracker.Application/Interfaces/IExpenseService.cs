using ExpenseTracker.Application.Common;
using ExpenseTracker.Application.DTOs.Expenses;

namespace ExpenseTracker.Application.Interfaces
{
    public interface IExpenseService
    {
        Task<Result<IEnumerable<ExpenseDto>>> GetAllAsync(string userId, int pageNumber = 1, int pageSize = 10);
        Task<Result<ExpenseDto>> GetByIdAsync(int id, string userId);
        Task<Result<IEnumerable<ExpenseDto>>> GetByCategoryIdAsync(int categoryId, string userId);
        Task<Result<ExpenseDto>> CreateAsync(CreateExpenseDto createExpenseDto, string userId);
        Task<Result<ExpenseDto>> UpdateAsync(UpdateExpenseDto updateExpenseDto, string userId);
        Task<Result<bool>> RemoveAsync(int id, string userId);
    }
}

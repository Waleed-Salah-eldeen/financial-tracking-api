using ExpenseTracker.Application.Common;
using ExpenseTracker.Application.DTOs.Categories;

namespace ExpenseTracker.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<Result<CategoryDto>> CreateAsync(CreateCategoryDto categoryDto, string userId);
        Task<Result<IEnumerable<CategoryDto>>> GetAllAsync(string userId, int pageNumber = 1, int pageSize = 10);
        Task<Result<CategoryDto>> GetByIdAsync(int id, string userId);
        Task<Result<IEnumerable<CategoryDetailsDto>>> GetAllWithExpensesAsync(string userId, int pageNumber = 1, int pageSize = 10);
        Task<Result<CategoryDetailsDto>> GetWithExpensesAsync(int id, string userId);
        Task<Result<CategoryDto>> UpdateAsync(UpdateCategoryDto categoryDto, string userId);
        Task<Result<bool>> RemoveAsync(int id, string userId);
    }
}

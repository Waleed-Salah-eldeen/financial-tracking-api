using ExpenseTracker.Application.Common;
using ExpenseTracker.Application.DTOs.Categories;

namespace ExpenseTracker.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<Result<CategoryDto>> Create(CreateCategoryDto categoryDto, string userId);
        Task<Result<IEnumerable<CategoryDto>>> GetAll(string userId);
        Task<Result<CategoryDto>> GetById(int id, string userId);
        Task<Result<IEnumerable<CategoryDetailsDto>>> GetAllWithExpenses(string userId);
        Task<Result<CategoryDetailsDto>> GetWithExpenses(int id, string userId);
        Task<Result<CategoryDto>> Update(UpdateCategoryDto categoryDto, string userId);
        Task<Result<bool>> Remove(int id, string userId);
    }
}

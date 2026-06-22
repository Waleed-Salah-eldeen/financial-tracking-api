using ExpenseTracker.Application.Common;
using ExpenseTracker.Application.DTOs.Categories;
using ExpenseTracker.Application.DTOs.Expenses;
using ExpenseTracker.Application.Interfaces;
using ExpenseTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
namespace ExpenseTracker.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IApplicationDbContext _context;


        public CategoryService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<CategoryDto>> CreateAsync(CreateCategoryDto createCategoryDto, string userId)
        {
            var categories = _context.Set<Category>();
            var isExists = await categories.AnyAsync(c => c.Name == createCategoryDto.Name 
                                                          && c.ApplicationUserId == userId
                                                          && !c.IsDeleted);
            if (isExists)
                return Result<CategoryDto>.Failure("Category with the same name already existes", ErrorType.Validation);

            var newCategory = new Category 
            { 
                Name = createCategoryDto.Name, 
                ApplicationUserId = userId 
            };
            
            await categories.AddAsync(newCategory);
            await _context.SaveChangesAsync();

            var categoryDto = ToCategoryDto.Compile().Invoke(newCategory);

            return Result<CategoryDto>.Success(categoryDto);
        }

        public async Task<Result<IEnumerable<CategoryDto>>> GetAllAsync(string userId)
        {
            var categories = await _context.Set<Category>().AsNoTracking().Where(c => c.ApplicationUserId == userId && !c.IsDeleted)
                                                           .Select(ToCategoryDto).ToListAsync();

            if (!categories.Any())
                return Result<IEnumerable<CategoryDto>>.Failure("This user has no categories", ErrorType.NotFound);

            return Result<IEnumerable<CategoryDto>>.Success(categories);

        }

        public async Task<Result<IEnumerable<CategoryDetailsDto>>> GetAllWithExpensesAsync(string userId)
        {
            var result = await _context.Set<Category>().AsNoTracking()
                                                     .Where(c => c.ApplicationUserId == userId && !c.IsDeleted)
                                                     .Select(ToCategoryDetails).ToListAsync();

            if (!result.Any())
                return Result<IEnumerable<CategoryDetailsDto>>.Failure("This user has no categories", ErrorType.NotFound);

            return Result<IEnumerable<CategoryDetailsDto>>.Success(result);
        }

        public async Task<Result<CategoryDto>> GetByIdAsync(int id, string userId)
        {
            var categoryDto = await _context.Set<Category>().AsNoTracking()
                                                         .Where(c => 
                                                                c.ApplicationUserId == userId
                                                                && c.Id == id
                                                                && !c.IsDeleted)
                                                         .Select(ToCategoryDto).FirstOrDefaultAsync();
            if (categoryDto is null)
                return Result<CategoryDto>.Failure("Category not found", ErrorType.NotFound);
            return Result<CategoryDto>.Success(categoryDto);
        }

        public async Task<Result<CategoryDetailsDto>> GetWithExpensesAsync(int id, string userId)
        {
            var categoryDetailsDto = await _context.Set<Category>().AsNoTracking()
                                                                    .Where(c => c.Id == id 
                                                                                && c.ApplicationUserId == userId 
                                                                                && !c.IsDeleted)
                                                                    .Select(ToCategoryDetails)
                                                                    .FirstOrDefaultAsync();
            if (categoryDetailsDto is null)
                return Result<CategoryDetailsDto>.Failure("Category not found", ErrorType.NotFound);
            
            return Result<CategoryDetailsDto>.Success(categoryDetailsDto);
        }

        public async Task<Result<bool>> RemoveAsync(int id, string userId)
        {
            var category = await _context.Set<Category>().FirstOrDefaultAsync(c =>
                                                         c.ApplicationUserId == userId
                                                         && c.Id == id
                                                         && !c.IsDeleted);

            if (category is null)
                return Result<bool>.Failure("Category not found", ErrorType.NotFound);

            category.IsDeleted = true;
            category.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Result<bool>.Success(true);
        }

        public async Task<Result<CategoryDto>> UpdateAsync(UpdateCategoryDto newCategoryDto, string userId)
        {
            var newCategoryDtoId = newCategoryDto.Id!.Value;
            var category = await _context.Set<Category>().FirstOrDefaultAsync(c =>
                                                         c.ApplicationUserId == userId
                                                         && c.Id == newCategoryDtoId
                                                         && !c.IsDeleted);

            if (category is null)
                return Result<CategoryDto>.Failure("Category not found", ErrorType.NotFound);

            var isNameExists = await _context.Set<Category>().AnyAsync(c => c.Name == newCategoryDto.Name 
                                                                            && !c.IsDeleted 
                                                                            && c.ApplicationUserId == userId
                                                                            && c.Id != newCategoryDto.Id);

            if(isNameExists)
                return Result<CategoryDto>.Failure("You have category with the same name", ErrorType.Validation);

            category.Name = newCategoryDto.Name;
            category.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            var categoryDto = ToCategoryDto.Compile().Invoke(category);
            return Result<CategoryDto>.Success(categoryDto);
        }


        private static readonly Expression<Func<Category, CategoryDto>> ToCategoryDto = c => new CategoryDto
        {
            Id = c.Id,
            Name = c.Name,
            CreatedAt = c.CreatedAt,
            UpdatedAt = c.UpdatedAt
        };


        private static readonly Expression<Func<Category, CategoryDetailsDto>> ToCategoryDetails = c => new CategoryDetailsDto
        {
            Id = c.Id,
            Name = c.Name,
            CreatedAt = c.CreatedAt,
            UpdatedAt = c.UpdatedAt,
            Expenses = c.Expenses.Where(e => !e.IsDeleted).Select(e => new ExpenseDto
            {
                Id = e.Id,
                Description = e.Description,
                Amount = e.Amount,
                CategoryName = c.Name,
                CreatedAt = e.CreatedAt,
                UpdatedAt = e.UpdatedAt
            }).ToList()
        };


    }
}

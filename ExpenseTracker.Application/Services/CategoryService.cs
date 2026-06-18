using ExpenseTracker.Application.Common;
using ExpenseTracker.Application.DTOs.Categories;
using ExpenseTracker.Application.DTOs.Expenses;
using ExpenseTracker.Application.Interfaces;
using ExpenseTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace ExpenseTracker.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IApplicationDbContext _context;


        public CategoryService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<CategoryDto>> Create(CreateCategoryDto createCategoryDto, string userId)
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

            var categoryDto = new CategoryDto
            {
                Name = newCategory.Name,
                Id = newCategory.Id,
                CreatedAt = newCategory.CreatedAt
            };

            return Result<CategoryDto>.Success(categoryDto);
        }

        public async Task<Result<IEnumerable<CategoryDto>>> GetAll(string userId)
        {
            var categories = await _context.Set<Category>().AsNoTracking().Where(c => c.ApplicationUserId == userId && !c.IsDeleted)
                                                           .Select(c => 
                                                           new CategoryDto 
                                                           { 
                                                               Id = c.Id, 
                                                               Name = c.Name, 
                                                               CreatedAt = c.CreatedAt, 
                                                               UpdatedAt = c.UpdatedAt
                                                           }).ToListAsync();

            if (!categories.Any())
                return Result<IEnumerable<CategoryDto>>.Failure("This user has no categories", ErrorType.NotFound);

            return Result<IEnumerable<CategoryDto>>.Success(categories);

        }

        public async Task<Result<IEnumerable<CategoryDetailsDto>>> GetAllWithExpenses(string userId)
        {
            var result = await _context.Set<Category>().AsNoTracking()
                                                     .Where(c => c.ApplicationUserId == userId && !c.IsDeleted)
                                                     .Select(c => new CategoryDetailsDto
                                                     {
                                                         Id = c.Id,
                                                         Name = c.Name,
                                                         CreatedAt = c.CreatedAt,
                                                         UpdatedAt = c.UpdatedAt,
                                                         Expenses = c.Expenses.Where(e => !e.IsDeleted && e.ApplicationUserId == userId)
                                                                              .Select(e => new ExpenseDto
                                                                              {
                                                                                  Id = e.Id,
                                                                                  Description = e.Description,
                                                                                  Amount = e.Amount,
                                                                                  CategoryName = c.Name,
                                                                                  CreatedAt = e.CreatedAt,
                                                                                  UpdatedAt = e.UpdatedAt
                                                                              }).ToList()
                                                     }).ToListAsync();

            

            if (!result.Any())
                return Result<IEnumerable<CategoryDetailsDto>>.Failure("This user has no categories", ErrorType.NotFound);

            return Result<IEnumerable<CategoryDetailsDto>>.Success(result);
        }

        public async Task<Result<CategoryDto>> GetById(int id, string userId)
        {
            var category = await _context.Set<Category>().AsNoTracking().FirstOrDefaultAsync(c => 
                                                         c.ApplicationUserId == userId
                                                         && c.Id == id
                                                         && !c.IsDeleted);
            if (category is null)
                return Result<CategoryDto>.Failure("Category not found", ErrorType.NotFound);
            var categoryDto = new CategoryDto 
            { 
                Id  = id, 
                Name = category.Name, 
                CreatedAt = category.CreatedAt, 
                UpdatedAt = category.UpdatedAt
            };
            return Result<CategoryDto>.Success(categoryDto);
        }

        public async Task<Result<CategoryDetailsDto>> GetWithExpenses(int id, string userId)
        {
            var category = await _context.Set<Category>().AsNoTracking().Include(c => c.Expenses.Where(e => !e.IsDeleted))
                                                         .FirstOrDefaultAsync(c =>
                                                         c.ApplicationUserId == userId
                                                         && c.Id == id
                                                         && !c.IsDeleted);
            if (category is null)
                return Result<CategoryDetailsDto>.Failure("Category not found", ErrorType.NotFound);
            var categoryDetailsDto = new CategoryDetailsDto
            {
                Id = id,
                Name = category.Name,
                CreatedAt = category.CreatedAt,
                UpdatedAt = category.UpdatedAt,
                Expenses = category.Expenses.Select(e => new ExpenseDto
                {
                    Id = e.Id,
                    Description = e.Description,
                    Amount = e.Amount,
                    CategoryName = category.Name,
                    CreatedAt = e.CreatedAt,
                    UpdatedAt = e.UpdatedAt
                }).ToList()
            };
            return Result<CategoryDetailsDto>.Success(categoryDetailsDto);
        }

        public async Task<Result<bool>> Remove(int id, string userId)
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

        public async Task<Result<CategoryDto>> Update(UpdateCategoryDto newCategoryDto, string userId)
        {
            var category = await _context.Set<Category>().FirstOrDefaultAsync(c =>
                                                         c.ApplicationUserId == userId
                                                         && c.Id == newCategoryDto.Id
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

            var categoryDto = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                CreatedAt = category.CreatedAt,
                UpdatedAt = category.UpdatedAt
            };
            return Result<CategoryDto>.Success(categoryDto);
        }

    }
}

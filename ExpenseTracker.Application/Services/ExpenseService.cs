using ExpenseTracker.Application.Common;
using ExpenseTracker.Application.DTOs.Expenses;
using ExpenseTracker.Application.Interfaces;
using ExpenseTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ExpenseTracker.Application.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IApplicationDbContext _context;

        public ExpenseService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<IEnumerable<ExpenseDto>>> GetAllAsync(string userId)
        {
            var expenses = await _context.Set<Expense>()
                .AsNoTracking()
                .Where(e => e.ApplicationUserId == userId && !e.IsDeleted)
                .Select(ToExpenseDto)
                .ToListAsync();

            return Result<IEnumerable<ExpenseDto>>.Success(expenses);
        }

        public async Task<Result<ExpenseDto>> GetByIdAsync(int id, string userId)
        {
            var expense = await _context.Set<Expense>()
                .AsNoTracking()
                .Where(e => e.Id == id && e.ApplicationUserId == userId && !e.IsDeleted)
                .Select(ToExpenseDto)
                .FirstOrDefaultAsync();

            if (expense is null)
                return Result<ExpenseDto>.Failure("Expense not found", ErrorType.NotFound);

            return Result<ExpenseDto>.Success(expense);
        }

        public async Task<Result<IEnumerable<ExpenseDto>>> GetByCategoryIdAsync(int categoryId, string userId)
        {
            var categoryExists = await _context.Set<Category>()
                                .AnyAsync(c => c.Id == categoryId && c.ApplicationUserId == userId && !c.IsDeleted);

            if (!categoryExists)
                return Result<IEnumerable<ExpenseDto>>.Failure("Category not found", ErrorType.NotFound);

            
            var expenses = await _context.Set<Expense>()
                .AsNoTracking()
                .Where(e => e.CategoryId == categoryId && e.ApplicationUserId == userId && !e.IsDeleted)
                .Select(ToExpenseDto)
                .ToListAsync();

            return Result<IEnumerable<ExpenseDto>>.Success(expenses);
        }

        public async Task<Result<ExpenseDto>> CreateAsync(CreateExpenseDto createExpenseDto, string userId)
        {
            int categoryId = createExpenseDto.CategoryId!.Value;

            var categoryExists = await _context.Set<Category>()
                .AnyAsync(c => c.Id == categoryId && c.ApplicationUserId == userId && !c.IsDeleted);

            if (!categoryExists)
                return Result<ExpenseDto>.Failure("Invalid Category ID or category does not belong to user", ErrorType.Validation);

            var expense = new Expense
            {
                Description = createExpenseDto.Description,
                Amount = createExpenseDto.Amount!.Value,
                CategoryId = categoryId,
                ApplicationUserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Set<Expense>().AddAsync(expense);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(expense.Id, userId);
        }

        public async Task<Result<ExpenseDto>> UpdateAsync(UpdateExpenseDto updateExpenseDto, string userId)
        {
            int expenseId = updateExpenseDto.Id!.Value;
            int categoryId = updateExpenseDto.CategoryId!.Value;

            var expense = await _context.Set<Expense>()
                .FirstOrDefaultAsync(e => e.Id == expenseId && e.ApplicationUserId == userId && !e.IsDeleted);

            if (expense is null)
                return Result<ExpenseDto>.Failure("Expense not found", ErrorType.NotFound);

            var categoryExists = await _context.Set<Category>()
                .AnyAsync(c => c.Id == categoryId && c.ApplicationUserId == userId && !c.IsDeleted);

            if (!categoryExists)
                return Result<ExpenseDto>.Failure("Invalid Category ID", ErrorType.Validation);

            expense.Description = updateExpenseDto.Description;
            expense.Amount = updateExpenseDto.Amount!.Value;
            expense.CategoryId = categoryId;
            expense.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return await GetByIdAsync(expense.Id, userId);
        }

        public async Task<Result<bool>> RemoveAsync(int id, string userId)
        {
            var expense = await _context.Set<Expense>()
                .FirstOrDefaultAsync(e => e.Id == id && e.ApplicationUserId == userId && !e.IsDeleted);

            if (expense is null)
                return Result<bool>.Failure("Expense not found", ErrorType.NotFound);

            expense.IsDeleted = true;
            expense.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Result<bool>.Success(true);
        }



        private static readonly Expression<Func<Expense, ExpenseDto>> ToExpenseDto = expense => new ExpenseDto
        {
            Id = expense.Id,
            Description = expense.Description,
            Amount = expense.Amount,
            CreatedAt = expense.CreatedAt,
            UpdatedAt = expense.UpdatedAt,
            CategoryId = expense.CategoryId,
            CategoryName = expense.Category != null ? expense.Category.Name : null
        };
    }
}

using ExpenseTracker.Application.DTOs.Expenses;
using ExpenseTracker.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExpenseTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ExpensesController : ApiBaseController
    {
        private readonly IExpenseService _expenseService;

        public ExpensesController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllAsync()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var result = await _expenseService.GetAllAsync(userId);

            if (!result.IsSuccess)
                return HandleFailure(result);

            return Ok(result.Data);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var result = await _expenseService.GetByIdAsync(id, userId);

            if (!result.IsSuccess)
                return HandleFailure(result);

            return Ok(result.Data);
        }

        [HttpGet("by-category/{categoryId:int}")]
        public async Task<IActionResult> GetByCategoryIdAsync(int categoryId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var result = await _expenseService.GetByCategoryIdAsync(categoryId, userId);

            if (!result.IsSuccess)
                return HandleFailure(result);

            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateExpenseDto createExpenseDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var result = await _expenseService.CreateAsync(createExpenseDto, userId);

            if (!result.IsSuccess)
                return HandleFailure(result);

            return CreatedAtAction(nameof(GetByIdAsync).Replace("Async", ""), new { id = result.Data!.Id }, result.Data);
        }

        [HttpPut("Edit")]
        public async Task<IActionResult> UpdateAsync(UpdateExpenseDto updateExpenseDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var result = await _expenseService.UpdateAsync(updateExpenseDto, userId);

            if (!result.IsSuccess)
                return HandleFailure(result);

            return Ok(result.Data);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Remove(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var result = await _expenseService.RemoveAsync(id, userId);

            if (!result.IsSuccess)
                return HandleFailure(result);

            return Ok(result.Data);
        }
    }
}

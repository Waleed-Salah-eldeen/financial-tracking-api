using ExpenseTracker.Application.DTOs.Categories;
using ExpenseTracker.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExpenseTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ApiBaseController
    {
        private readonly ICategoryService _categoryService;
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllAsync()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var result = await _categoryService.GetAllAsync(userId);

            if (!result.IsSuccess)
                return HandleFailure(result);

            return Ok(result.Data);
        }

        [HttpGet("allWithExpenses")]
        public async Task<IActionResult> GetAllWithExpensesAsync()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var result = await _categoryService.GetAllWithExpensesAsync(userId);

            if (!result.IsSuccess)
                return HandleFailure(result);

            return Ok(result.Data);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var result = await _categoryService.GetByIdAsync(id, userId);
            if (!result.IsSuccess)
                return HandleFailure(result);
            return Ok(result.Data);
        }

        [HttpGet("CategoryWithExpenses/{id:int}")]
        public async Task<IActionResult> GetWithExpensesAsync(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var result = await _categoryService.GetWithExpensesAsync(id, userId);
            if (!result.IsSuccess)
                return HandleFailure(result);
            return Ok(result.Data);
        }


        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateCategoryDto createCategoryDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var result = await _categoryService.CreateAsync(createCategoryDto, userId);
            if (!result.IsSuccess)
                return HandleFailure(result);
            return CreatedAtAction(nameof(GetByIdAsync).Replace("Async", ""), new { id = result.Data!.Id }, result.Data);
        }

        [HttpPost("Edit")]
        public async Task<IActionResult> UpdateAsync(UpdateCategoryDto updateCategoryDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var result = await _categoryService.UpdateAsync(updateCategoryDto, userId);
            if (!result.IsSuccess)
                return HandleFailure(result);
            return CreatedAtAction(nameof(GetByIdAsync).Replace("Async", ""), new { id = result.Data!.Id }, result.Data);
        }


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Remove(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var result = await _categoryService.RemoveAsync(id, userId);
            if (!result.IsSuccess)
                return HandleFailure(result);
            return Ok(result.Data);
        }
    }
}

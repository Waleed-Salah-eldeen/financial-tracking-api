using System.ComponentModel.DataAnnotations;
namespace ExpenseTracker.Application.DTOs.Expenses
{
    public class CreateExpenseDto
    {
        [Required(ErrorMessage = "Expense description is required")]
        [MinLength(3, ErrorMessage = "Description must be at least 3 characters")]
        [MaxLength(100, ErrorMessage = "Description cannot exceed 100 characters")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, (double)decimal.MaxValue, ErrorMessage = "Amount must be greater than zero")]
        public decimal? Amount { get; set; }

        [Required(ErrorMessage = "Category ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid category")]
        public int? CategoryId { get; set; }
    }
}

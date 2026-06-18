using System.ComponentModel.DataAnnotations;
namespace ExpenseTracker.Application.DTOs.Expenses
{
    public class CreateExpenseDto
    {
        [Required(ErrorMessage = "Expense description is required")]
        [MinLength(3, ErrorMessage = "Description must be at least 3 characters")]
        [MaxLength(300, ErrorMessage = "Description cannot exceed 50 characters")]
        public string Description { get; set; } = null!;

        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero")]
        public decimal Amount { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select a category")]
        public int CategoryId { get; set; }
    }
}

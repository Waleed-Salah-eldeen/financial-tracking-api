using System.ComponentModel.DataAnnotations;
namespace ExpenseTracker.Application.DTOs.Categories
{
    public class UpdateCategoryDto
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Category name is required.")]
        [MinLength(3, ErrorMessage = "Category name must be at least 3 characters.")]
        [MaxLength(50, ErrorMessage = "Category name cannot exceed 50 characters.")]
        public string Name { get; set; } = null!;

    }
}

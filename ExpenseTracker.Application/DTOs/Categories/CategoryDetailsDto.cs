using ExpenseTracker.Application.DTOs.Expenses;

namespace ExpenseTracker.Application.DTOs.Categories
{
    public class CategoryDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public ICollection<ExpenseDto> Expenses { get; set; } = new List<ExpenseDto>();
    }
}

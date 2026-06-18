namespace ExpenseTracker.Application.DTOs.Categories
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime CreatedAt { get; set; } 
        public DateTime? UpdatedAt { get; set; }
    }
}

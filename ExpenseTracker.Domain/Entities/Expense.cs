namespace ExpenseTracker.Domain.Entities
{
    public class Expense
    {
        public int Id { get; set; }
        public string Description { get; set; } = null!;
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;

        public int CategoryId { get; set; }
        public Category? Category { get; set; } 
        public string ApplicationUserId { get; set; } = null!;

    }
}

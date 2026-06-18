namespace ExpenseTracker.Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string ApplicationUserId { get; set; } = null!;
        public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
    }
}

using Microsoft.AspNetCore.Identity;
using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = null!;
        public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
        public ICollection<Category> Categories { get; set; } = new List<Category>();

    }
}

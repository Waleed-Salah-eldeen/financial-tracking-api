using ExpenseTracker.Application.Interfaces;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace ExpenseTracker.Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option) : base(option) { }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Expense>().HasOne(e => e.Category)
                                     .WithMany(c => c.Expenses)
                                     .HasForeignKey(e => e.CategoryId)
                                     .OnDelete(DeleteBehavior.Restrict); ;
            builder.Entity<Expense>().Property(e => e.Amount).HasPrecision(18, 2);

            base.OnModelCreating(builder);
        }

        


        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}

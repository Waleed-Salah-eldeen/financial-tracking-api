using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.Application.DTOs.Expenses
{
    public class ExpenseDto
    {
        public int Id { get; set; }
        public string Description { get; set; } = null!;
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
    }
}

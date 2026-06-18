using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.Application.DTOs.Auth
{
    public class AuthResponseDto
    {
        public bool IsSuccess { get; set; }

        public IEnumerable<string>? Errors { get; set; }

        public string? Token { get; set; }
    }
}

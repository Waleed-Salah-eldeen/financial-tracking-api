
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace ExpenseTracker.Application.DTOs.Auth
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Email is requierd !"), EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is requierd !")]
        public string Password { get; set; } = string.Empty;
    }
}

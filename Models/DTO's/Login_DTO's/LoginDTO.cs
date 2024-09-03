using System.ComponentModel.DataAnnotations;

namespace ShoppingAppAPI.Models.DTO_s
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(4, ErrorMessage = "Password must be at least 4 characters long.")]
        public string Password { get; set; }
    }
}

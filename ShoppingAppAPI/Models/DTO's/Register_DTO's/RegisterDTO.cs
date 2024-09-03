using System;
using System.ComponentModel.DataAnnotations;

namespace ShoppingAppAPI.Models.DTO_s
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Username is required.")]
        [MinLength(3, ErrorMessage = "Username must be at least 3 characters long.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [MinLength(2, ErrorMessage = "Name must be at least 2 characters long.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string Phone_Number { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Date_of_Birth { get; set; }

        [RegularExpression("^(Male|Female|Other|Prefer not to say)$", ErrorMessage = "Invalid gender value.")]
        public string? Gender { get; set; }

        public string? Profile_Picture_URL { get; set; }
    }
}

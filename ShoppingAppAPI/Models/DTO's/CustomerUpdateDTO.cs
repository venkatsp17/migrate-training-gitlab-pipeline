using System.ComponentModel.DataAnnotations;

namespace ShoppingAppAPI.Models.DTO_s
{
    public class CustomerUpdateDTO
    {
        [Required(ErrorMessage = "Customer ID is required.")]
        public int CustomerID { get; set; }

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

        public string? Profile_Picture_URL { get; set; }
    }
}

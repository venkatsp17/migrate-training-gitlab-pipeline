using System.ComponentModel.DataAnnotations;
using static ShoppingAppAPI.Models.Enums;

namespace ShoppingAppAPI.Models.DTO_s
{
    public class UserRegisterDTO
    {
        public string Username { get; set; }
        public byte[] Password { get; set; }
        public byte[] Password_Hashkey { get; set; }
        public bool IsAdmin { get; set; }
        public UserRole Role { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone_Number { get; set; }
        public DateTime? Date_of_Birth { get; set; }
        public string? Gender { get; set; }
        public string? Profile_Picture_URL { get; set; }
    }
}

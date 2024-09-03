using static ShoppingAppAPI.Models.Enums;

namespace ShoppingAppAPI.Models.DTO_s
{
    public class LoginReturnDTO
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public UserRole Role { get; set; }
    }
}

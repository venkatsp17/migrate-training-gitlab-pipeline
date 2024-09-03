using ShoppingAppAPI.Models;

namespace ShoppingAppAPI.Services.Interfaces
{
    public interface ITokenServices
    {
        public string GenerateToken(User user);
    }
}

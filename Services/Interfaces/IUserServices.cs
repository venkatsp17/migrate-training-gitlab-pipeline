using ShoppingAppAPI.Models;
using ShoppingAppAPI.Models.DTO_s;

namespace ShoppingAppAPI.Services.Interfaces
{
    public interface IUserServices
    {
        Task<RegisterReturnDTO> CustomerRegister(RegisterDTO userRegisterDTO);
        Task<LoginReturnDTO> CustomerLogin(LoginDTO userLoginDTO);
        Task<RegisterReturnDTO> SellerRegister(RegisterDTO userRegisterDTO);
        Task<LoginReturnDTO> SellerLogin(LoginDTO userLoginDTO);
    }
}

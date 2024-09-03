using ShoppingAppAPI.Models.DTO_s.Seller_DTO_s;
using ShoppingAppAPI.Models;

namespace ShoppingAppAPI.Mappers
{
    public class SellerMapper
    {
        public static SellerDTO MapToSellerDTO(Seller seller)
        {
            return new SellerDTO
            {
                SellerID = seller.SellerID,
                Email = seller.Email,
                Name = seller.Name,
                Address = seller.Address,
                Phone_Number = seller.Phone_Number,
                Profile_Picture_URL = seller.Profile_Picture_URL,
            };
        }
    }
}

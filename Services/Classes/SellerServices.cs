using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Mappers;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Models.DTO_s.Customer_DTO_s;
using ShoppingAppAPI.Models.DTO_s;
using ShoppingAppAPI.Repositories.Classes;
using ShoppingAppAPI.Repositories.Interfaces;
using ShoppingAppAPI.Services.Interfaces;
using ShoppingAppAPI.Models.DTO_s.Seller_DTO_s;

namespace ShoppingAppAPI.Services.Classes
{
    public class SellerServices : ISellerServices
    {
        private readonly ISellerRepository _sellerRepository;

        /// <summary>
        /// Constructor for SellerServices class.
        /// </summary>
        /// <param name="sellerRepository">Seller repository dependency.</param>
        public SellerServices(ISellerRepository sellerRepository)
        {
            _sellerRepository = sellerRepository;
        }

        /// <summary>
        /// Updates the last login time of a seller.
        /// </summary>
        /// <param name="SellerID">ID of the seller.</param>
        /// <returns>Returns the updated seller entity.</returns>
        public async Task<Seller> UpdateSellerLastLogin(int SellerID)
        {
            try
            {
                Seller seller = await _sellerRepository.Get(SellerID);
                seller.Last_Login = DateTime.Now;
                return await _sellerRepository.Update(seller);
            }
            catch (Exception ex)
            {
                throw new UnableToUpdateItemException("Unable to update seller last login."+ ex);
            }
        }

        /// <summary>
        /// Updates the profile of a seller.
        /// </summary>
        /// <param name="updateDTO">DTO containing seller update information.</param>
        /// <returns>Returns the updated seller DTO.</returns>
        public async Task<SellerDTO> UpdateSeller(SellerUpdateDTO updateDTO)
        {
            try
            {
                Seller seller = await _sellerRepository.Get(updateDTO.SellerID);
                seller.Phone_Number = updateDTO.Phone_Number;
                seller.Address = updateDTO.Address;
                seller.Email = updateDTO.Email;
                seller.Name = updateDTO.Name;
                seller.Profile_Picture_URL = updateDTO.Profile_Picture_URL;
                Seller updatedSeller = await _sellerRepository.Update(seller);
                if (updatedSeller == null)
                {
                    throw new UnableToUpdateItemException("Unable to update seller profile.");
                }
                return SellerMapper.MapToSellerDTO(seller);
            }
            catch (Exception ex)
            {
                throw new UnableToUpdateItemException("Unable to update seller profile."+ ex);
            }
        }

        public async Task<SellerDTO> GetSellerProfile(int UserID)
        {
            try
            {
                Seller seller = await _sellerRepository.GetSellerByUserID(UserID);
                return SellerMapper.MapToSellerDTO(seller);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

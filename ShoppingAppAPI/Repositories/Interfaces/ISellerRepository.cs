using ShoppingAppAPI.Models;

namespace ShoppingAppAPI.Repositories.Interfaces
{
    public interface ISellerRepository : IRepository<int, Seller>
    {
      Task<Seller> GetSellerByEmail(string email);

       Task<Seller> GetSellerByUserID(int userID);
    }
}

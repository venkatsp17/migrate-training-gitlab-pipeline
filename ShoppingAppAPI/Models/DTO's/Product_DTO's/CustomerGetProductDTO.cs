using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ShoppingAppAPI.Models.DTO_s.Seller_DTO_s;
using ShoppingAppAPI.Models.DTO_s.Review_DTO_s;

namespace ShoppingAppAPI.Models.DTO_s
{
    public class CustomerGetProductDTO
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryID { get; set; }
        public string Brand { get; set; }
        public string Image_URL { get; set; }
        public int Stock {  get; set; }
        public SellerDTO Seller { get; set; }
        public ICollection<ReviewReturnDTO> Reviews { get; set; }
    }
}

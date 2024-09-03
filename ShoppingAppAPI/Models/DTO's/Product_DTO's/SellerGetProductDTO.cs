using ShoppingAppAPI.Models.DTO_s.Review_DTO_s;
using System.ComponentModel.DataAnnotations;

namespace ShoppingAppAPI.Models.DTO_s.Product_DTO_s
{
    public class SellerGetProductDTO
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryID { get; set; }
        public string Brand { get; set; }
        public string Image_URL { get; set; }
        public int Stock_Quantity { get; set; }
        public DateTime Creation_Date { get; set; }
        public DateTime Last_Updated { get; set; }
        public ICollection<ReviewReturnDTO> Reviews { get; set; }
    }
}

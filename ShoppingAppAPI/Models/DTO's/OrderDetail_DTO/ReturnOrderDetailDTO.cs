using ShoppingAppAPI.Models.DTO_s.Product_DTO_s;
using System.ComponentModel.DataAnnotations;

namespace ShoppingAppAPI.Models.DTO_s.OrderDetail_DTO
{
    public class ReturnOrderDetailDTO
    {
        public int OrderDetailID { get; set; }
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public string Size { get; set; }

        public SellerGetProductDTO Product { get; set; }

    }

    public class ReturnOrderDetailDTO1
    {
        public int OrderDetailID { get; set; }
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public string Size { get; set; }

        public CustomerGetProductDTO1 Product { get; set; }

    }
}

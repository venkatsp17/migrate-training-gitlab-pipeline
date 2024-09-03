using System.ComponentModel.DataAnnotations;

namespace ShoppingAppAPI.Models.DTO_s
{
    public class AddProductDTO
    {
        [Required(ErrorMessage = "SellerID is required!")]
       
        public int SellerID { get; set; }
        [Required(ErrorMessage = "Name is required!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Description is required!")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Description is required!")]
        [Range(1, int.MaxValue, ErrorMessage = "Price must be greater than zero")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "CategoryID is required!")]
        public int CategoryID { get; set; }
        [Required(ErrorMessage = "Brand is required!")]
        public string Brand { get; set; }
        [Required(ErrorMessage = "Image URL is required!")]
        public string Image_URL { get; set; }
        [Required(ErrorMessage = "Stock Quantity is required!")]
        [Range(1, int.MaxValue, ErrorMessage = "Price must be greater than zero")]
        public int Stock_Quantity { get; set; }
    }
}

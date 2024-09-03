using static ShoppingAppAPI.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ShoppingAppAPI.Models.DTO_s.Payment_DTO_s
{
    public class PaymentGetDTO
    {
        [Required(ErrorMessage ="Order ID is required!")]
        public int OrderID { get; set; }
        [Required(ErrorMessage = "Payment_Method is required!")]
        public string Payment_Method { get; set; }

    }
}

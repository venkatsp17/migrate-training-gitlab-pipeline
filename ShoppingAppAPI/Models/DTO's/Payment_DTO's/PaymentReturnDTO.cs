using static ShoppingAppAPI.Models.Enums;

namespace ShoppingAppAPI.Models.DTO_s.Payment_DTO_s
{
    public class PaymentReturnDTO
    {
        public int PaymentID { get; set; }
        public int OrderID { get; set; }
        public string Payment_Method { get; set; }
        public decimal Amount { get; set; }
        public DateTime Transaction_Date { get; set; }
        public PaymentStatus Payment_Status { get; set; }
    }
}

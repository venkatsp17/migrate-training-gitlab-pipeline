using ShoppingAppAPI.Models;
using ShoppingAppAPI.Models.DTO_s.Payment_DTO_s;

namespace ShoppingAppAPI.Mappers
{
    public class PaymentMapper
    {
        public static PaymentReturnDTO MapToPaymentReturnDTO(Payment payment)
        {
            return new PaymentReturnDTO()
            {
                PaymentID = payment.PaymentID,
                OrderID = payment.OrderID,
                Payment_Method = payment.Payment_Method,
                Amount = payment.Amount,
                Transaction_Date = payment.Transaction_Date,
                Payment_Status = payment.Payment_Status,
            };
        }
    }
}

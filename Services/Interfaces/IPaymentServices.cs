using ShoppingAppAPI.Models.DTO_s.Payment_DTO_s;

namespace ShoppingAppAPI.Services.Interfaces
{
    public interface IPaymentServices
    {
        Task<PaymentReturnDTO> MakePayment(PaymentGetDTO paymentGetDTO);
    }
}

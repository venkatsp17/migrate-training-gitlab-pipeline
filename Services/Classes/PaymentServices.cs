using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Mappers;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Models.DTO_s.Payment_DTO_s;
using ShoppingAppAPI.Repositories.Interfaces;
using ShoppingAppAPI.Services.Interfaces;

namespace ShoppingAppAPI.Services.Classes
{
    /// <summary>
    /// Service class responsible for handling payment-related operations.
    /// </summary>
    public class PaymentServices : IPaymentServices
    {
        private readonly IRepository<int, Payment> _paymentRepository;
        private readonly IOrderRepository _orderRepository;

        /// <summary>
        /// Constructor for PaymentServices class.
        /// </summary>
        /// <param name="paymentRepository">Payment repository dependency.</param>
        /// <param name="orderRepository">Order repository dependency.</param>
        public PaymentServices(IRepository<int, Payment> paymentRepository, IOrderRepository orderRepository)
        {
            _paymentRepository = paymentRepository;
            _orderRepository = orderRepository;
        }

        /// <summary>
        /// Makes a payment for an order.
        /// </summary>
        /// <param name="paymentGetDTO">Payment information DTO.</param>
        /// <returns>Returns the payment details DTO.</returns>
        public async Task<PaymentReturnDTO> MakePayment(PaymentGetDTO paymentGetDTO)
        {
            try
            {
                var order = await _orderRepository.Get(paymentGetDTO.OrderID);
                if (order == null || order.Status == Enums.OrderStatus.Failed || order.Status == Enums.OrderStatus.Canceled || order.Status == Enums.OrderStatus.Refunded)
                {
                    throw new UnableToProcessOrder("Unable to Process Order at this moment!");
                }

                // Create a new payment
                Payment payment = new Payment()
                {
                    OrderID = paymentGetDTO.OrderID,
                    Payment_Method = paymentGetDTO.Payment_Method,
                    Amount = order.Total_Amount,
                    Transaction_Date = DateTime.Now,
                    Payment_Status = Enums.PaymentStatus.Authorized,
                };

                // Add the payment to the repository
                var newPayment = await _paymentRepository.Add(payment);
                if (newPayment == null)
                {
                    throw new UnableToAddItemException("Unable to add payment at this moment!");
                }

                // Update the order with the successful payment ID
                order.Success_PaymentID = newPayment.PaymentID;
                var updatedOrder = await _orderRepository.Update(order);

                // Map the payment to a DTO and return it
                return PaymentMapper.MapToPaymentReturnDTO(newPayment);
            }
            catch (Exception ex)
            {
                throw new UnableToAddItemException("Unable to add payment at this moment: " + ex.Message);
            }
        }
    }
}

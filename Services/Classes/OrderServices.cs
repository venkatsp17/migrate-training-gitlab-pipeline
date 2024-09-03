using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Mappers;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Models.DTO_s.Order_DTO_s;
using ShoppingAppAPI.Models.DTO_s.Product_DTO_s;
using ShoppingAppAPI.Repositories.Interfaces;
using ShoppingAppAPI.Services.Interfaces;
using static ShoppingAppAPI.Models.Enums;

namespace ShoppingAppAPI.Services.Classes
{
    public class OrderServices : IOrderServices
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IRepository<int, Refund> _refundRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICartServices _cartServices;
        private readonly IProductServices _productServices;
        private readonly IProductRepository _productRepository;

        /// <summary>
        /// Constructor for OrderServices class.
        /// </summary>
        public OrderServices(IOrderRepository orderRepository, 
            IUnitOfWork unitOfWork, 
            IOrderDetailRepository orderDetailRepository,
            ICartRepository cartRepository,
            ICartServices cartServices,
            IRepository<int, Refund> refundRepository,
            IProductServices productServices,
            IProductRepository productRepository
            )
        { 
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
            _orderDetailRepository = orderDetailRepository;
            _cartRepository = cartRepository;
            _cartServices = cartServices;
            _refundRepository = refundRepository;
            _productServices = productServices;
            _productRepository = productRepository;
        }
        /// <summary>
        /// Place Order from collecting details from cart.
        /// </summary>
        /// <param name="placeOrderDTO">The PlaceOrderDTO get Order Details</param>
        /// <returns>Return necessary order details for customer after placing the order</returns>
        /// <exception cref="NoAvailableItemException">Thrown when no carts are available.</exception>
        /// <exception cref="EmptyCartException">Thrown when cart is empty.</exception>
        /// <exception cref="UnableToAddItemException">Thrown when unable to add item to database.</exception>
        public async Task<CustomerOrderReturnDTO> PlaceOrder(PlaceOrderDTO placeOrderDTO)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var cart = await _cartRepository.GetCartByCustomerID(placeOrderDTO.CustomerID);
                    if(cart == null || cart.Cart_Status == CartStatus.Closed)
                    {
                        throw new NoAvailableItemException("Cart");
                    }
                    if(cart.CartItems == null || !cart.CartItems.Any())
                    {
                        throw new EmptyCartException();
                    }
                    var totalPrice = cart.CartItems.Sum(od => od.Price);
                    Order order = new Order()
                    {
                        CustomerID = placeOrderDTO.CustomerID,
                        Order_Date = DateTime.Now,
                        Status = OrderStatus.Pending,
                        Address = placeOrderDTO.Address,
                        Total_Amount = (decimal)totalPrice + placeOrderDTO.Shipping_Cost,
                        Shipping_Method = placeOrderDTO.Shipping_Method,
                        Shipping_Cost = placeOrderDTO.Shipping_Cost,
                    };

                    Order newOrder = await _orderRepository.Add(order);
                    if(newOrder == null)
                    {
                        throw new UnableToAddItemException("Unable to Create Order at this moment!");
                    }

                    ICollection<OrderDetail> orderDetails = cart.CartItems.Select(od => new OrderDetail()
                    {
                        ProductID = od.ProductID,
                        Quantity = od.Quantity,
                        OrderID = newOrder.OrderID,
                        Size = od.Size,
                        SellerID = od.Product.SellerID,
                        Price = (decimal)od.Price,

                    }).ToList();

                    ICollection<OrderDetail> newOrderDetails = new List<OrderDetail>(); ;
                    foreach (var orderDetail in orderDetails)
                    {
                         OrderDetail newOrderDetail = await _orderDetailRepository.Add(orderDetail);
                        if (newOrderDetail == null)
                        {
                            throw new UnableToAddItemException("Unable to Create Order at this moment!");
                        }
                        Product product = await _productRepository.Get(orderDetail.ProductID);
                         if(product == null)
                         {
                            throw new UnableToAddItemException("Unable to process order at this moment!");
                         }
                         SellerGetProductDTO product1 = await _productServices.UpdateProductStock(product.Stock_Quantity - orderDetail.Quantity, product.ProductID);
                        if (product1 == null)
                        {
                            throw new UnableToAddItemException("Unable to process order at this moment!");
                        }
                        newOrderDetails.Add(newOrderDetail);
                    }

                    newOrder.OrderDetails = newOrderDetails;

                    var deletedCart = await _cartServices.CloseCart(cart.CartID);
                    if(deletedCart == null)
                    {
                        throw new UnableToAddItemException("Unable to process order at this moment!");
                    }
                    await _unitOfWork.Commit();
                    return OrderMapper.MapToCustomerOrderReturnDTO(newOrder); 
                }
                catch (Exception ex)
                {
                    await _unitOfWork.Rollback();
                    throw new UnableToAddItemException(ex.Message);
                }
            }

        }
        /// <summary>
        /// Update Order status by Order ID
        /// </summary>
        /// <param name="orderStatus">The order status to be updated</param>
        /// <param name="OrderID">The order id of the order to updated</param>
        /// <returns>Return necessary order details for seller after updating the order</returns>
        /// <exception cref="NotFoundException">Thrown when order not found.</exception>
        /// <exception cref="UnableToUpdateItemException">Thrown when unable to update order status.</exception>
        public async Task<SellerOrderReturnDTO> UpdateOrderStatus(OrderStatus orderStatus, int OrderID)
        {
            try
            {
                Order order = await _orderRepository.Get(OrderID);
                if (order == null)
                {
                    throw new NotFoundException("Order");
                }
                order.Status = orderStatus;
                order.Last_Updated = DateTime.Now;
                Order updatedOrder = await _orderRepository.Update(order);
                if(updatedOrder == null)
                {
                    throw new UnableToUpdateItemException("Unable to update order status at this moment!");
                }
                return OrderMapper.MapToSellerOrderReturnDTO(updatedOrder);
            }
            catch (Exception ex)
            {
                throw new UnableToUpdateItemException(ex.Message);
            }
        }
        /// <summary>
        /// View All Active Orders for seller
        /// </summary>
        /// <param name="SellerID">To fetch orders of particular seller using ID</param>
        /// <returns>Return necessary order details for seller</returns>
        /// <exception cref="NoAvailableItemException">Thrown when order not found.</exception>
        public async Task<PaginatedResult<SellerOrderReturnDTO>> ViewAllSellerActiveOrders(int SellerID, int offset, int limit, string searchQuery)
        {
            try
            {
                IEnumerable<OrderDetail> ordersDetails = await _orderDetailRepository.GetSellerOrderDetails(SellerID);
                if (!ordersDetails.Any())
                {
                    throw new NoAvailableItemException("Orders");
                }
                if(searchQuery == "null")
                {
                    searchQuery = null;
                }
                var activeOrders = ordersDetails
                                    .Where(od => (od.Order.Status == OrderStatus.Pending || od.Order.Status == OrderStatus.Processing)
                                    && (string.IsNullOrEmpty(searchQuery) || od.Order.OrderID.ToString().Contains(searchQuery)))
                                    .Select(od => od.Order)
                                    .Distinct();

                var paginatedOrders = activeOrders
                    .Skip(offset)
                    .Take(limit)
                    .ToList();

                var totalCount = activeOrders.Count();

                var result = new PaginatedResult<SellerOrderReturnDTO>
                {
                    Items = paginatedOrders.Select(o => OrderMapper.MapToSellerOrderReturnDTO(o)).ToList(),
                    TotalCount = totalCount
                };

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public async Task<IEnumerable<CustomerOrderReturnDTO>> ViewAllCustomerActiveOrders(int CustomerID)
        {
            try
            {
                IEnumerable<Order> orders = await _orderRepository.GetCustomerOrders(CustomerID);
                if (!orders.Any())
                {
                    throw new NoAvailableItemException("Orders");
                }

                var activeOrders = orders
                    .Where(o => o.Status == OrderStatus.Shipped ||
                                 o.Status == OrderStatus.Pending ||
                                 o.Status == OrderStatus.Processing);
                return activeOrders.Select(o => OrderMapper.MapToCustomerOrderReturnDTO(o));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        /// <summary>
        /// View customer's order history
        /// </summary>
        /// <param name="CustomerID">To fetch orders of particular customer</param>
        /// <returns>Return necessary order details for customers</returns>
        /// <exception cref="NoAvailableItemException">Thrown when order not found.</exception>
        public async Task<IEnumerable<CustomerOrderReturnDTO>> ViewCustomerOrderHistory(int CustomerID)
        {
            try
            {
                IEnumerable<Order> orders = await _orderRepository.GetCustomerOrders(CustomerID);
                if (orders.Count() == 0)
                {
                    throw new NoAvailableItemException("Products");
                }
                return orders.Where(o => o.CustomerID == CustomerID)
                    .Where(o => o.Status == OrderStatus.Delivered || o.Status == OrderStatus.Failed || o.Status == OrderStatus.Canceled || o.Status == OrderStatus.Refunded)
                    .Select(o => OrderMapper.MapToCustomerOrderReturnDTO(o));
            }
            catch (Exception ex)
            {
                throw new NoAvailableItemException(ex.Message);
            }
        }
        /// <summary>
        /// Cancel Order By Customer
        /// </summary>
        /// <param name="OrderID">To cancel particular order of Order ID</param>
        /// <returns>Return necessary order detail of cancelled order</returns>
        /// <exception cref="NotFoundException">Thrown when order not found.</exception>
        /// <exception cref="NotAllowedToCancelOrderException">Thrown when order is shipped, delivered, cancelled</exception>
        /// <exception cref="UnableToUpdateItemException">Thrown when unable to update the order status.</exception>
        public async Task<CustomerOrderReturnDTO> CustomerCancelOrder(int OrderID)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    Order order = await _orderRepository.Get(OrderID);
                    if (order == null)
                    {
                        throw new NotFoundException("Order");
                    }
                    if (order.Status == OrderStatus.Shipped || order.Status == OrderStatus.Delivered || order.Status == OrderStatus.Refunded || order.Status == OrderStatus.Failed || order.Status == OrderStatus.Canceled)
                    {
                        OrderStatus orderStatus = order.Status;
                        throw new NotAllowedToCancelOrderException(orderStatus.ToString());
                    }
                    order.Status = OrderStatus.Canceled;
                    order.Last_Updated = DateTime.Now;
                    Order updatedOrder = await _orderRepository.Update(order);
                    if (updatedOrder == null)
                    {
                        throw new UnableToUpdateItemException("Unable to update order status at this moment!");
                    }
                    if (order.Success_PaymentID == null)
                    {

                    }
                    else
                    {
                        Refund refund = new Refund()
                        {
                            OrderID = order.OrderID,
                            Amount = order.Total_Amount,
                            Reason = "Order Cancelled!",
                            Refund_Method = "Online",
                            Status = RefundStatus.Pending,
                        };
                        var initiateRefund = await _refundRepository.Add(refund);
                        if (initiateRefund == null)
                        {
                            throw new UnableToUpdateItemException("Unable to update order status at this moment!");
                        }
                    }
                    await _unitOfWork.Commit();
                    return OrderMapper.MapToCustomerOrderReturnDTO(updatedOrder);
                }
                catch (Exception ex)
                {
                    await _unitOfWork.Rollback();
                    throw new UnableToUpdateItemException(ex.Message);
                }
            }
        }
        /// <summary>
        /// Update Order specific details By Customer
        /// </summary>
        /// <param name="updateOrderDeliveryDetailsDTO">The details of order to be updated</param>
        /// <returns>Return necessary order detail of updated order</returns>
        /// <exception cref="NotFoundException">Thrown when order not found.</exception>
        /// <exception cref="UnableToUpdateItemException">Thrown when unable to update the order status.</exception>
        public async Task<CustomerOrderReturnDTO> UpdateOrderDeliveryDetails(UpdateOrderDeliveryDetailsDTO updateOrderDeliveryDetailsDTO)
        {
            try
            {
                Order order = await _orderRepository.Get(updateOrderDeliveryDetailsDTO.OrderID);
                if (order == null)
                {
                    throw new NotFoundException("Order");
                }
                order.Address = updateOrderDeliveryDetailsDTO.Address;
                order.Shipping_Method = updateOrderDeliveryDetailsDTO.Shipping_Method;
                order.Last_Updated = DateTime.Now;
                Order updatedOrder = await _orderRepository.Update(order);
                if (updatedOrder == null)
                {
                    throw new UnableToUpdateItemException("Unable to update order status at this moment!");
                }
                return OrderMapper.MapToCustomerOrderReturnDTO(updatedOrder);
            }
            catch (Exception ex)
            {
                throw new UnableToUpdateItemException(ex.Message);
            }
        }
    }
}

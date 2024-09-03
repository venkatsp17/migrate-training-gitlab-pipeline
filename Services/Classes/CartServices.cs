using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Models.DTO_s.Cart_DTO_s;
using ShoppingAppAPI.Models;
using static ShoppingAppAPI.Models.Enums;
using ShoppingAppAPI.Mappers;
using ShoppingAppAPI.Services.Interfaces;
using ShoppingAppAPI.Repositories.Interfaces;


namespace ShoppingAppAPI.Services.Classes
{
    public class CartServices : ICartServices
    {
        private readonly ICartRepository _cartRepository;
        private readonly IRepository<int, CartItem> _cartItemRepository;
        private readonly IProductRepository _productRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CartServices"/> class.
        /// </summary>
        /// <param name="cartRepository">The repository for managing carts.</param>
        /// <param name="cartItemRepository">The repository for managing cart items.</param>
        /// <param name="productRepository">The repository for managing products.</param>
        public CartServices(ICartRepository cartRepository, IRepository<int, CartItem> cartItemRepository, IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
            _productRepository = productRepository;
        }
        /// <summary>
        /// Adds an item to a shopping cart.
        /// </summary>
        /// <param name="cartItem">The item to add to the cart.</param>
        /// <param name="cartID">The ID of the cart.</param>
        /// <param name="customerID">The ID of the customer.</param>
        /// <returns>The updated cart details.</returns>
        public async Task<CartReturnDTO> AddItemToCart(CartItemGetDTO cartItem, int CustomerID)
        {
            try
            {
                var cart = await _cartRepository.GetCartByCustomerID(CustomerID);

                if (cart == null)
                {
                    // Create a new cart if none exists
                    cart = new Cart
                    {
                        CustomerID = CustomerID,
                        Cart_Status = CartStatus.Open,
                        Last_Updated = DateTime.Now,
                    };
                    cart = await _cartRepository.Add(cart);
                    if(cart == null)
                    {
                        throw new UnableToAddItemException("Unable to Add Item to Cart at this moment!");
                    }
                }
                var product = await _productRepository.Get(cartItem.ProductID);
                if(cart.CartItems ==  null || cart.CartItems.Count() == 0)
                {
                    var newItem = new CartItem
                    {
                        CartID = cart.CartID,
                        Size = cartItem.Size,
                        ProductID = cartItem.ProductID,
                        Quantity = cartItem.Quantity,
                        Price = (double)(product.Price*cartItem.Quantity)
                    };
                    var newCartItem = await _cartItemRepository.Add(newItem);
                    if (newCartItem == null)
                    {
                        throw new UnableToAddItemException("Unable to Add Item to Cart at this moment!");
                    }
                }
                else
                {
                    var existingItem = cart.CartItems.FirstOrDefault(ci => ci.ProductID == cartItem.ProductID && ci.Size == cartItem.Size);
                    if (existingItem != null)
                    {
                        existingItem.Quantity += cartItem.Quantity;
                        existingItem.Price = (double)(existingItem.Quantity * product.Price); // Update price based on new quantity
                        var updatedCartItem = await _cartItemRepository.Update(existingItem);
                        if (updatedCartItem == null)
                        {
                            throw new UnableToAddItemException("Unable to Add Item to Cart at this moment!");
                        }
                    }
                    else
                    {
                        var newItem = new CartItem
                        {
                            CartID = cart.CartID,
                            ProductID = cartItem.ProductID,
                            Quantity = cartItem.Quantity,
                            Size = cartItem.Size,
                            Price = (double)(cartItem.Quantity*product.Price)
                        };
                        var newCartItem = await _cartItemRepository.Add(newItem);
                        if (newCartItem == null)
                        {
                            throw new UnableToAddItemException("Unable to Add Item to Cart at this moment!");
                        }
                    }
                }
                cart.Last_Updated = DateTime.Now;
                await _cartRepository.Update(cart);
    

                return CartMapper.MapCartToDTO(cart);
            }
            catch(Exception ex)
            {
                throw new UnableToAddItemException(ex.Message);
            }
        }
        /// <summary>
        /// Removes an item from a shopping cart.
        /// </summary>
        /// <param name="cartItemID">The ID of the cart item to remove.</param>
        /// <returns>The updated cart details.</returns>
        public async Task<CartReturnDTO> RemoveItemFromCart(int cartItemID)
        {
            try
            {
                var cartItem = await _cartItemRepository.Get(cartItemID);
                if (cartItem == null) throw new NotFoundException("CartItem not found.");

                var cart = await _cartRepository.Get(cartItem.CartID);
                if (cart == null) throw new NotFoundException("Cart not found.");

                var product = await _productRepository.Get(cartItem.ProductID);

                if (cartItem.Quantity > 1)
                {
                    cartItem.Quantity--;
                    cartItem.Price = (double)(product.Price * cartItem.Quantity); // Adjust price
                }
                else
                {
                    var deletedItem = await _cartItemRepository.Delete(cartItemID);
                    if (deletedItem == null)
                    {
                        throw new UnableToUpdateItemException("Unable to remove Item from Cart at this moment!");
                    }
                }
                cart.Last_Updated = DateTime.Now;
                try
                {
                    await _cartRepository.Update(cart);
                }
                catch (Exception ex)
                {

                }
                return CartMapper.MapCartToDTO(cart);
            }
           catch(Exception ex)
            {
                throw new UnableToUpdateItemException(ex.Message);
            }
        }
        /// <summary>
        /// Updates the quantity of a cart item.
        /// </summary>
        /// <param name="CartItemID">The ID of the cart item to update.</param>
        /// <param name="Quantity">The new quantity.</param>
        /// <returns>The updated cart details.</returns>
        public async Task<CartReturnDTO> UpdateCartItemQuantity(int CartItemID, int Quantity)
        {
            try
            {
  
                var existingcartItem = await _cartItemRepository.Get(CartItemID);
                if (existingcartItem == null) throw new NotFoundException("CartItem not found.");

                var cart = await _cartRepository.Get(existingcartItem.CartID);
                if (cart == null) throw new NotFoundException("Cart not found.");

                var product = await _productRepository.Get(existingcartItem.ProductID);
                if (Quantity <= 0)
                {
                    var deletedCartItem = await _cartItemRepository.Delete(existingcartItem.CartItemID);
                    cart.Last_Updated = DateTime.Now;
                    if (deletedCartItem == null)
                    {
                        throw new UnableToUpdateItemException("Unable to update Item from Cart at this moment!");
                    }
                    return CartMapper.MapCartToDTO(cart);
                }
                existingcartItem.Quantity = Quantity;
                existingcartItem.Price = (double)(product.Price * existingcartItem.Quantity); // Update price based on quantity
                var updatedCartItem = await _cartItemRepository.Update(existingcartItem);
                if (updatedCartItem == null)
                {
                    throw new UnableToUpdateItemException("Unable to update Item from Cart at this moment!");
                }
                cart.Last_Updated = DateTime.Now;

                try
                {
                    await _cartRepository.Update(cart);
                }
                catch (Exception ex)
                {

                }
                return CartMapper.MapCartToDTO(cart);
            }
            catch (Exception ex)
            {
                throw new UnableToUpdateItemException(ex.Message);
            }
        }
        /// <summary>
        /// Retrieves a cart by its ID.
        /// </summary>
        /// <param name="cartID">The ID of the cart.</param>
        /// <returns>The cart details.</returns>
        public async Task<CartReturnDTO1> GetCart(int customerID)
        {
            try
            {
                var cart = await _cartRepository.GetCartByCustomerID(customerID);
                if (cart == null) throw new NotFoundException("Cart");

                return CartMapper.MapCartToDTO1(cart);
            }
            catch(NotFoundException ex)
            {
                throw new NotFoundException("Cart");
            }
        }
        /// <summary>
        /// Closes a shopping cart.
        /// </summary>
        /// <param name="cartID">The ID of the cart to close.</param>
        /// <returns>The closed cart details.</returns>
        public async Task<CartReturnDTO> CloseCart(int cartID)
        {
            try
            {
                var cart = await _cartRepository.Get(cartID);
                if (cart == null) throw new NotFoundException("Cart");

                var deletedCart = await _cartRepository.Delete(cartID);
                if (deletedCart == null)
                {
                    throw new UnableToUpdateItemException("Unable to close Cart at this moment!");
                }

                return CartMapper.MapCartToDTO(cart);
            }
            catch(Exception ex)
            {
                throw new UnableToUpdateItemException(ex.Message);
            }
        }
        }
    }

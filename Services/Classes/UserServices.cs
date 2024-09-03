using Microsoft.EntityFrameworkCore;
using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Models.DTO_s;
using ShoppingAppAPI.Repositories.Classes;
using ShoppingAppAPI.Repositories.Interfaces;
using ShoppingAppAPI.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;
using static ShoppingAppAPI.Models.Enums;

namespace ShoppingAppAPI.Services.Classes
{
    public class UserServices : IUserServices
    {
 
        private readonly IUserRepository _userRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ISellerRepository _sellerRepository;
        private readonly ITokenServices _tokenService;
        private readonly IRegistrationRepository _registrationRepository;
        private readonly ICustomerServices _customerServices;
        private readonly ISellerServices _sellerServices;
        private readonly ILogger<UserServices> _logger;
        public UserServices(IUserRepository userRepository,
            ICustomerRepository customerRepository,
            ISellerRepository sellerRepository,
            ITokenServices tokenService,
            IRegistrationRepository registrationRepository,
            ICustomerServices customerServices,
            ISellerServices sellerServices,
            ILogger<UserServices> logger
            )
        {
            _customerRepository = customerRepository;
            _userRepository = userRepository;
            _tokenService = tokenService;
            _sellerRepository = sellerRepository;
            _registrationRepository = registrationRepository;
            _customerServices = customerServices;
            _sellerServices = sellerServices;
            _logger = logger;
        }

        private byte[] EncryptPassword(string password, byte[] passwordHash)
        {
            HMACSHA512 hMACSHA = new HMACSHA512(passwordHash);
            var encrypterPass = hMACSHA.ComputeHash(Encoding.UTF8.GetBytes(password));
            return encrypterPass;
        }

        private bool ComparePassword(byte[] encrypterPass, byte[] password)
        {
            for (int i = 0; i < encrypterPass.Length; i++)
            {
                if (encrypterPass[i] != password[i])
                {
                    return false;
                }
            }
            return true;
        }
        private LoginReturnDTO MapUserToLoginReturn(User user)
        {
            LoginReturnDTO returnDTO = new LoginReturnDTO();
            returnDTO.Id = user.UserID;
            returnDTO.Role = user.Role;
            returnDTO.Token = _tokenService.GenerateToken(user);
            return returnDTO;
        }
        /// <summary>
        /// Handles customer login functionality.
        /// </summary>
        /// <param name="loginDTO">LoginDTO containing user credentials.</param>
        /// <returns>LoginReturnDTO with user details and token.</returns>
        /// <exception cref="UnauthorizedUserException">Thrown when user authentication fails.</exception>
        /// <exception cref="AccountInActiveException">Thrown when the user's account is not active.</exception>
        /// <exception cref="UnableToLoginException">Thrown when unable to complete the login process.</exception>
        public async Task<LoginReturnDTO> CustomerLogin(LoginDTO loginDTO)
        {
            try
            {
                var user = await _userRepository.GetCustomerDetailByEmail(loginDTO.Email);
                if (user == null)
                {
                    throw new UnauthorizedUserException();
                }
                var encryptedPassword = EncryptPassword(loginDTO.Password, user.Password_Hashkey);
                bool isPasswordSame = ComparePassword(encryptedPassword, user.Password);
                if (isPasswordSame)
                {
                    if (user.Customer == null)
                    {
                        throw new UnauthorizedUserException();
                    }
                    if (user.Customer.Account_Status != "Active")
                    {
                        throw new AccountInActiveException("Account is not Active!");
                    }
                    LoginReturnDTO loginReturnDTO = MapUserToLoginReturn(user);
                    try
                    {
                        await _customerServices.UpdateCustomerLastLogin(user.Customer.CustomerID);
                    }
                    catch(Exception ex)
                    {
                        _logger.LogWarning(ex.Message);
                    }
                    if (loginReturnDTO == null)
                    {
                        throw new UnableToLoginException("Not able to login at this moment!");
                    }
                    return loginReturnDTO;
                }
                throw new UnauthorizedUserException();
            }
            catch (Exception e)
            {
                throw new UnableToLoginException(e.Message);
            }
        }
        /// <summary>
        /// Handles customer registration functionality.
        /// </summary>
        /// <param name="userRegisterDTO">RegisterDTO containing user registration details.</param>
        /// <returns>RegisterReturnDTO with registered user details.</returns>
        /// <exception cref="UserAlreadyExistsException">Thrown when a user with the provided email already exists.</exception>
        /// <exception cref="UnableToRegisterException">Thrown when unable to complete the registration process.</exception>
        public async Task<RegisterReturnDTO> CustomerRegister(RegisterDTO userRegisterDTO)
        {
            try
            {
                var existingCustomer = await _customerRepository.GetCustomerByEmail(userRegisterDTO.Email);
                if (existingCustomer != null)
                {
                    throw new UserAlreadyExistsException("Email already exists");
                }
                UserRegisterDTO userRepostioryRegisterDTO = MapUserRegisterDTO(userRegisterDTO);
                userRepostioryRegisterDTO.Role = UserRole.Customer;
                var data = await _registrationRepository.AddCustomer_UserTransaction(userRepostioryRegisterDTO);

                if (data.customer == null || data.user == null)
                {
                    throw new UnableToRegisterException("Unable to Register at this moment");
                }
                RegisterReturnDTO registerReturnDTO = MapCustomerToRegisterReturnDTO(data.user, data.customer);
                return registerReturnDTO;
            }
            catch (Exception e)
            {
                throw new UnableToRegisterException(e.Message);
            }
        }

        private RegisterReturnDTO MapCustomerToRegisterReturnDTO(User user, Customer customer)
        {
            RegisterReturnDTO registerReturnDTO = new RegisterReturnDTO();
            registerReturnDTO.Username = user.Username;
            registerReturnDTO.ID = customer.CustomerID;
            registerReturnDTO.Name = customer.Name;
            registerReturnDTO.Email = customer.Email;
            return registerReturnDTO;
        }

        private RegisterReturnDTO MapSellerToRegisterReturnDTO(User user, Seller seller)
        {
            RegisterReturnDTO registerReturnDTO = new RegisterReturnDTO();
            registerReturnDTO.Username = user.Username;
            registerReturnDTO.ID = seller.SellerID;
            registerReturnDTO.Name = seller.Name;
            registerReturnDTO.Email = seller.Email;
            return registerReturnDTO;
        }

        private UserRegisterDTO MapUserRegisterDTO(RegisterDTO registerDTO)
        {
            UserRegisterDTO user = new UserRegisterDTO();
            HMACSHA512 hMACSHA = new HMACSHA512();
            user.Password_Hashkey = hMACSHA.Key;
            user.Username = registerDTO.Username;
            user.IsAdmin = false;
            user.Password = hMACSHA.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password));
            user.Address = registerDTO.Address;
            user.Email = registerDTO.Email;
            user.Date_of_Birth = registerDTO.Date_of_Birth;
            user.Phone_Number = registerDTO.Phone_Number;
            user.Gender = registerDTO.Gender;
            user.Profile_Picture_URL = registerDTO.Profile_Picture_URL;
            user.Name = registerDTO.Name;
            return user;
        }
        /// <summary>
        /// Handles seller login functionality.
        /// </summary>
        /// <param name="loginDTO">LoginDTO containing user credentials.</param>
        /// <returns>LoginReturnDTO with user details and token.</returns>
        /// <exception cref="UnauthorizedUserException">Thrown when user authentication fails.</exception>
        /// <exception cref="AccountInActiveException">Thrown when the user's account is not active.</exception>
        /// <exception cref="UnableToLoginException">Thrown when unable to complete the login process.</exception>
        public async Task<LoginReturnDTO> SellerLogin(LoginDTO loginDTO)
        {
            try
            {
                var user = await _userRepository.GetSellerDetailByEmail(loginDTO.Email);
                if (user == null)
                {
                    throw new UnauthorizedUserException();
                }
                var encryptedPassword = EncryptPassword(loginDTO.Password, user.Password_Hashkey);
                bool isPasswordSame = ComparePassword(encryptedPassword, user.Password);
                if (isPasswordSame)
                {
                    if (user.Seller == null)
                    {
                        throw new UnauthorizedUserException();
                    }
                    if (user.Seller.Account_Status != "Active")
                    {
                        throw new AccountInActiveException("Account is not Active!");
                    }
                    LoginReturnDTO loginReturnDTO = MapUserToLoginReturn(user);
                    try
                    {
                        await _sellerServices.UpdateSellerLastLogin(user.Seller.SellerID);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex.Message);
                    }
                    if (loginReturnDTO == null)
                    {
                        throw new UnableToLoginException("Not able to login at this moment!");
                    }
                    return loginReturnDTO;
                }
                throw new UnauthorizedUserException();
            }
            catch (Exception e)
            {
                throw new UnableToLoginException(e.Message);
            }
        }
        /// <summary>
        /// Handles seller registration functionality.
        /// </summary>
        /// <param name="userRegisterDTO">RegisterDTO containing user registration details.</param>
        /// <returns>RegisterReturnDTO with registered user details.</returns>
        /// <exception cref="UserAlreadyExistsException">Thrown when a user with the provided email already exists.</exception>
        /// <exception cref="UnableToRegisterException">Thrown when unable to complete the registration process.</exception>
        public async Task<RegisterReturnDTO> SellerRegister(RegisterDTO userRegisterDTO)
        {
            try
            {
                var existingSeller = await _sellerRepository.GetSellerByEmail(userRegisterDTO.Email);
                if (existingSeller != null)
                {
                    throw new UserAlreadyExistsException("Email already exists");
                }
                UserRegisterDTO userRepostioryRegisterDTO = MapUserRegisterDTO(userRegisterDTO);
                userRepostioryRegisterDTO.Role = UserRole.Seller;
                var data = await _registrationRepository.AddSeller_UserTransaction(userRepostioryRegisterDTO);

                if (data.seller == null || data.user == null)
                {
                    throw new UnableToRegisterException("Unable to Register at this moment");
                }
                RegisterReturnDTO registerReturnDTO = MapSellerToRegisterReturnDTO(data.user, data.seller);
                return registerReturnDTO;
            }
            catch (Exception e)
            {
                throw new UnableToRegisterException(e.Message);
            }
        }
    }
}

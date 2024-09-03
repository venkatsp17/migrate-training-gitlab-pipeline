using Microsoft.IdentityModel.Tokens;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ShoppingAppAPI.Services.Classes
{
    public class TokenServices : ITokenServices
    {
        private readonly string _secretKey;
        private readonly SymmetricSecurityKey _key;

        /// <summary>
        /// Constructor for TokenServices class.
        /// </summary>
        /// <param name="configuration">Configuration interface for retrieving token secret key.</param>
        public TokenServices(IConfiguration configuration)
        {
            _secretKey = configuration.GetSection("TokenKey").GetSection("JWT").Value.ToString();
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        }

        /// <summary>
        /// Generates a JWT token for the specified user.
        /// </summary>
        /// <param name="user">User entity for which the token is generated.</param>
        /// <returns>Returns the generated JWT token.</returns>
        public string GenerateToken(User user)
        {
            string token = string.Empty;
            var claims = new List<Claim>()
            {
                new Claim("UID", user.UserID.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
            };
            var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);
            var myToken = new JwtSecurityToken(null, null, claims, expires: DateTime.Now.AddHours(2), signingCredentials: credentials);
            token = new JwtSecurityTokenHandler().WriteToken(myToken);
            return token;
        }
    }
}

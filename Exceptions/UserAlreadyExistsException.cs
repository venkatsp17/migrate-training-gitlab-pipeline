using System.Runtime.Serialization;

namespace ShoppingAppAPI.Exceptions
{
    public class UserAlreadyExistsException : Exception
    {
        public UserAlreadyExistsException(string? message) : base(message)
        {
        }
    }
}
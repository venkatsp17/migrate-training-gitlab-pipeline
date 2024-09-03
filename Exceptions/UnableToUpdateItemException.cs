using System.Runtime.Serialization;

namespace ShoppingAppAPI.Exceptions
{

    public class UnableToUpdateItemException : Exception
    {
        public UnableToUpdateItemException(string message) : base(message) { }
    }
}
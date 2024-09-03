using System.Runtime.Serialization;

namespace ShoppingAppAPI.Exceptions
{
    public class UnableToProcessOrder : Exception
    {    
        public UnableToProcessOrder(string? message) : base(message)
        {
        }
    }
}
namespace ShoppingAppAPI.Exceptions
{
    public class NotAllowedToCancelOrderException : Exception
    {
        string message;

        public NotAllowedToCancelOrderException(string status)
        {
            message = $"Order has been {status} alraeady!";
        }
        public override string Message => message;
    }
}

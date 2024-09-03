namespace ShoppingAppAPI.Exceptions
{
    public class UnableToAddItemException : Exception
    {
        string message;

        public UnableToAddItemException(string msg)
        {
            message = msg;
        }

        public override string Message => message;
    }
}

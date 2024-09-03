namespace ShoppingAppAPI.Exceptions
{
    public class UnableToLoginException : Exception
    {
        string message;

        public UnableToLoginException(string msg)
        {
            message = msg;
        }

        public override string Message => message;
    }
}

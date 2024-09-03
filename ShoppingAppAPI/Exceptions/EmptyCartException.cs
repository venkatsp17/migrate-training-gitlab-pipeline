namespace ShoppingAppAPI.Exceptions
{
    public class EmptyCartException : Exception
    {
        string message;
        public EmptyCartException()
        {
            message = "Cart Is Empty!";
        }
        public override string Message => message;
    }
}

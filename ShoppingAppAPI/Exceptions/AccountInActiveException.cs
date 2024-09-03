namespace ShoppingAppAPI.Exceptions
{
    public class AccountInActiveException : Exception
    {
        public AccountInActiveException(string message) : base(message) { }
    }
}

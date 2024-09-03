namespace ShoppingAppAPI.Exceptions
{
    public class NoAvailableItemException : Exception
    {
        string message;
        public NoAvailableItemException(string Name)
        {
            message = $"No {Name} available!";
        }
        public override string Message => message;
    }
}

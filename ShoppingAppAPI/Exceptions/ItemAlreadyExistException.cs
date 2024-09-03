namespace ShoppingAppAPI.Exceptions
{
    public class ItemAlreadyExistException : Exception
    {
        string message;
        public ItemAlreadyExistException(string Name) {
            message = $"{Name} already exist!";
        }
        public override string Message => message;
    }
}

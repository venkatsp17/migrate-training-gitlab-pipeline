namespace ShoppingAppAPI.Exceptions
{
    public class NotFoundException : Exception
    {
        string message;
        public NotFoundException(string Name)
        {
            message = $"{Name} with given ID Not Found!";
        }
        public override string Message => message;
    }
}

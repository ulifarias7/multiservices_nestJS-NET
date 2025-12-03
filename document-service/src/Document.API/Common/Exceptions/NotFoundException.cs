namespace Document.API.Common.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException() : base("NotFoundException")
        {
        }
        public NotFoundException(string message) : base(message)
        {
        }
    }
}

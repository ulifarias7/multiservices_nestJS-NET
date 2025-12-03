namespace Document.API.Common.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException() : base("BadRequestException")
        {
        }
        public BadRequestException(string message) : base(message)
        {
        }
    }
}

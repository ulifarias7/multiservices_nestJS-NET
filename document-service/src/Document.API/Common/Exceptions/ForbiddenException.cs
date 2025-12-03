namespace Document.API.Common.Exceptions
{
    public class ForbiddenException : Exception
    {
        public ForbiddenException() : base("ForbiddenException")
        {
        }
        public ForbiddenException(string message) : base(message)
        {
        }
    }
}

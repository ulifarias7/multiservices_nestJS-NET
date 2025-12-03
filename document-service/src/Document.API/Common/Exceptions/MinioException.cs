namespace Document.API.Common.Exceptions
{
    public class MinioException : Exception
    {
        public MinioException() : base("MinioException")
        {
        }
        public MinioException(string message) : base(message)
        {
        }
    }
}

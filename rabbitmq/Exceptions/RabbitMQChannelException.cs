namespace RabbitMq.Exceptions
{
    public class RabbitMQChannelException : Exception
    {
        public RabbitMQChannelException(string message) : base(message) { }

        public RabbitMQChannelException(string message, Exception innerException)
        : base(message, innerException) { }
    }
}

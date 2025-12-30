namespace RabbitMq.Exceptions
{
    public class RabbitMQPublishException : Exception
    {
        public RabbitMQPublishException(string message) : base(message) { }

        public RabbitMQPublishException(string message, Exception exception)
            : base(message, exception) { }
    }
}

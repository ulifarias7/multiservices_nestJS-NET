namespace RabbitMq.Exceptions
{
    public class RabbitMQConsumerException : Exception
    {
        public RabbitMQConsumerException(string message) : base(message) { }
       
        public RabbitMQConsumerException(string message,Exception exception) 
            : base(message, exception) { }
    }
}

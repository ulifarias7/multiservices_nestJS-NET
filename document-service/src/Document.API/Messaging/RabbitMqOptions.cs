namespace Document.API.Messaging
{
    public sealed class RabbitMqOptions
    {
        public required string Uri { get; init; }
        public string QueueName { get; init; } = "document.created";
    }
}

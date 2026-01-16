namespace Auditory.API.Messaging.Options
{
    public sealed class RabbitMqDocumentServiceOptions
    {
        public required string Uri { get; init; }
        public string QueueName { get; init; } = "document.created";
        public string SourceService { get; init; } = "document-service";
    }
}

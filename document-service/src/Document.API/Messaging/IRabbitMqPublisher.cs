
namespace Document.API.Messaging
{
    public interface IRabbitMqPublisher
    {
        ValueTask PublishAsync<T>(T message, CancellationToken ct = default);
    }
}

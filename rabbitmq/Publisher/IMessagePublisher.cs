using RabbitMq.Models;

namespace RabbitMq.Publisher
{
    public interface IMessagePublisher
    {
        Task DeclareQueueAsync(QueueConfiguration config);

        Task PublishToQueueAsync<T>(
            string queueName,
            T message,
            bool persistent = true) where T : class;
    }
}

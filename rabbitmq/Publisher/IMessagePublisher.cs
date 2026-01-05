using RabbitMq.Models;

namespace RabbitMq.Publisher
{
    public interface IMessagePublisher
    {
        Task DeclareQueueAsync(QueueConfiguration config);
    }
}

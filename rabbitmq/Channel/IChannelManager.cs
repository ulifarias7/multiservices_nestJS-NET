using RabbitMQ.Client;

namespace RabbitMq.Channel
{
    public interface IChannelManager
    {
        Task<IChannel> CreateChannelAsync();
        Task CloseChannelAsync(IChannel channel);
    }
}

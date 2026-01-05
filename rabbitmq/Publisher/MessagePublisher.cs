using RabbitMq.Channel;
using RabbitMq.Exceptions;
using RabbitMq.Models;
using RabbitMq.Serialization;
using RabbitMQ.Client;
using System.Text;

namespace RabbitMq.Publisher
{
    public class MessagePublisher : IMessagePublisher
    {
        private readonly IChannelManager _channelManager;
        private readonly IMessageSerializer _serializer;

        public MessagePublisher(
            IChannelManager channelManager,
            IMessageSerializer serializer)
        {
            _channelManager = channelManager;
            _serializer = serializer;
        }

        public async Task DeclareQueueAsync(QueueConfiguration config)
        {
            IChannel? channel = null;

            try
            {
                channel = await _channelManager.CreateChannelAsync();

                await channel.QueueDeclareAsync(
                    queue: config.QueueName,
                    durable: config.Durable,
                    exclusive: config.Exclusive,
                    autoDelete: config.AutoDelete,
                    arguments: config.Arguments
                );

                Console.WriteLine($"Cola '{config.QueueName}' declarada exitosamente");
            }
            catch (Exception ex)
            {
                throw new RabbitMQPublishException(
                    $"Error al declarar la cola '{config.QueueName}'", ex);
            }
            finally
            {
                if (channel != null)
                {
                    await _channelManager.CloseChannelAsync(channel);
                }
            }
        }

        public async Task PublishToQueueAsync<T>(
            string queueName,
            T message,
            bool persistent = true) where T : class
        {
            await PublishAsync(
                exchange: "",
                routingKey: queueName,
                message: message,
                persistent: persistent
            );
        }

        public async Task PublishAsync<T>(
            string exchange,
            string routingKey,
            T message,
            bool persistent = true) where T : class
        {

            IChannel? channel = null;

            try
            {
                channel = await _channelManager.CreateChannelAsync();

                var messageJson = _serializer.Serialize(message);
                var body = Encoding.UTF8.GetBytes(messageJson);

                var properties = new BasicProperties
                {
                    Persistent = persistent, 
                    ContentType = "application/json",
                    ContentEncoding = "utf-8",
                    Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds()),
                    MessageId = Guid.NewGuid().ToString(),
                    Type = typeof(T).Name
                };

                await channel.BasicPublishAsync(
                    exchange: exchange,
                    routingKey: routingKey,
                    mandatory: false,
                    basicProperties: properties,
                    body: body
                );

                Console.WriteLine($"Mensaje publicado a '{exchange}' con routing key '{routingKey}'");
            }
            catch (Exception ex)
            {
                throw new RabbitMQPublishException(
                    $"Error al publicar mensaje a exchange '{exchange}' con routing key '{routingKey}'",
                    ex);
            }
            finally
            {
                if (channel != null)
                {
                    await _channelManager.CloseChannelAsync(channel);
                }
            }
        }
    }
}

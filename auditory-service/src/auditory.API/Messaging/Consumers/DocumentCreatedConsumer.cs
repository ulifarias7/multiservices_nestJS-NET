using Auditory.API.Database.Entity;
using Auditory.API.Database.Persistence;
using Auditory.API.Messaging.Options;
using AutoMapper;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Auditory.API.Messaging.Consumers
{
    public class DocumentCreatedConsumer : BackgroundService
    {
        private readonly RabbitMqDocumentServiceOptions _rabbitMqOptions;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<DocumentCreatedConsumer> _logger;
        private IConnection? _connection;
        private IChannel? _channel;
        private readonly IMapper _mapper;

        public DocumentCreatedConsumer(
            RabbitMqDocumentServiceOptions rabbitMqOptions,
            IServiceScopeFactory scopeFactory,
            ILogger<DocumentCreatedConsumer> logger,
            IMapper mapper)
        {
            _rabbitMqOptions = rabbitMqOptions;
            _scopeFactory = scopeFactory;
            _logger = logger;
            _mapper = mapper;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri(_rabbitMqOptions.Uri)
            };

            _connection = await factory.CreateConnectionAsync(stoppingToken);
            _logger.LogInformation($"Se genero la conexion con la url : {factory.Uri}");
            _channel = await _connection.CreateChannelAsync();
            _logger.LogInformation($"se genero el canal.");

            await _channel.QueueDeclareAsync(
                queue: _rabbitMqOptions.QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null,
                cancellationToken: stoppingToken
            );

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += OnMessageReceived;

            await _channel.BasicConsumeAsync(
                queue: _rabbitMqOptions.QueueName,
                autoAck: false,
                consumer: consumer,
                cancellationToken: stoppingToken
            );
        }

        private async Task OnMessageReceived(object sender, BasicDeliverEventArgs args)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            try
            {
                var body = Encoding.UTF8.GetString(args.Body.ToArray());
                var payloadJson = JsonDocument.Parse(body);

                var audit = _mapper.Map<AuditEntity>(args);
                audit.SourceService = _rabbitMqOptions.SourceService;
                audit.Payload = payloadJson;
                audit.OccurredAt = ExtractOccurredAt(payloadJson);

                db.Audits.Add(audit);
                await db.SaveChangesAsync();

                await _channel!.BasicAckAsync(args.DeliveryTag, false);
            }
            catch
            {
                await _channel!.BasicNackAsync(args.DeliveryTag, false, true);
            }
        }

        private static DateTime ExtractOccurredAt(JsonDocument payload)
        {
            if (payload.RootElement.TryGetProperty("CreatedAt", out var createdAt))
                return createdAt.GetDateTime();

            return DateTime.UtcNow;
        }

        public override async Task StopAsync(CancellationToken cancellationToken)//se ejecuta cuando la app se esta apagando 
        {
            if (_channel != null) await _channel.CloseAsync();
            if (_connection != null) await _connection.CloseAsync();
        }
    }
}

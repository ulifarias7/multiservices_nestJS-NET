namespace RabbitMq.Models
{
    public class QueueConfiguration
    {
        public string QueueName { get; set; } = string.Empty;
        public bool Durable { get; set; } = true;
        public bool Exclusive { get; set; } = false;
        public bool AutoDelete { get; set; } = false;
        public IDictionary<string, object>? Arguments { get; set; }
    }
}

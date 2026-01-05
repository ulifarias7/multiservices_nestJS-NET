namespace RabbitMq.Configurations
{
    public class ConnectionSettings
    {
        public string HostName { get; set; } = "rabbitmq";
        public int Port { get; set; } = 5672;
        public string UserName { get; set; } = "admin";
        public string Password { get; set; } = "admin123";
        public string VirtualHost { get; set; } = "/";
        public string? Url { get; set; } = null;
    }
}

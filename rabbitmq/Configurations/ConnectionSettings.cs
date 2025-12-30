namespace RabbitMq.Configurations
{
    public class ConnectionSettings
    {
        public string HostName { get; set; } = "localhost";
        public int Port { get; set; } = 15672;
        public string UserName { get; set; } = "admin";
        public string Password { get; set; } = "admin123";
        public string VirtualHost { get; set; } = "/";
        public string? Uri { get; set; } = "rabbitmq";
    }
}

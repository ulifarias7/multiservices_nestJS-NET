namespace RabbitMq.Serialization
{
    public interface IMessageSerializer
    {
        string Serialize<T>(T obj) where T : class;
        T Deserialize<T>(string json) where T : class;
    }
}

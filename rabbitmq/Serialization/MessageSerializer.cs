using System.Runtime.Serialization;
using System.Text.Json;

namespace RabbitMq.Serialization
{
    public class MessageSerializer : IMessageSerializer
    {
        private readonly JsonSerializerOptions _Options;

        public MessageSerializer()
        {
            _Options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };
        }

        public T Deserialize<T>(string json) where T : class
        {
            var result = JsonSerializer.Deserialize<T>(json, _Options);

            if (result == null)
                throw new SerializationException("La deserialización retornó null");

            return result;
        }

        public string Serialize<T>(T obj) where T : class
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            return JsonSerializer.Serialize(obj, _Options);
        }
    }
}

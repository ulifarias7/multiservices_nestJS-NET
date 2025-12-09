using System.Text.Json.Serialization;

namespace Auth.API.Models
{
    public class CreateUserKeycloacDto
    {
        [JsonPropertyName("firstame")]
        public string? FirstName { get; set; }

        [JsonPropertyName("lastname")]
        public string? LastName { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("emailVerified")]
        public bool? EmailVerified { get; set; }

        [JsonPropertyName("enabled")]
        public bool? Enabled { get; set; }

        [JsonPropertyName("credentials")]
        public credentialDto Credentials { get; set; }

    }
    public class credentialDto
    {
        [JsonPropertyName("type")]
        public string? Type { get; set; } = "password";

        [JsonPropertyName("value")]
        public string? Value { get; set; }

        [JsonPropertyName("tempory")]
        public bool? Tempory { get; set; } = false;
    }
}

using System.Text.Json;

namespace Auditory.API.Models.Dtos
{
    public class AuditDto
    {
        public Guid Id { get; set; }
        public string EventName { get; set; } = string.Empty;
        public string RoutingKey { get; set; } = string.Empty;
        public string Exchange { get; set; } = string.Empty;
        public string SourceService { get; set; } = string.Empty;
        public string? SourceHost { get; set; }
        public JsonDocument Payload { get; set; } 
        public JsonDocument? Headers { get; set; }
        public string? CorrelationId { get; set; }
        public string? CausationId { get; set; }
        public string MessageId { get; set; }  = string.Empty;
    }
}

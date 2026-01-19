using System.Text.Json;

namespace Auditory.API.Database.Entity
{
    public class AuditEntity
    {
        public Guid Id { get; set; }
        public string EventName { get; set; } = default!;
        public string RoutingKey { get; set; } = default!;
        public string Exchange { get; set; } = default!;
        public string SourceService { get; set; } = default!;
        public string? SourceHost { get; set; }
        public JsonDocument Payload { get; set; } = default!;
        public JsonDocument? Headers { get; set; }
        public string? CorrelationId { get; set; }
        public string? CausationId { get; set; }
        public string MessageId { get; set; } = default!;
        public DateTime OccurredAt { get; set; }
        public DateTime ReceivedAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

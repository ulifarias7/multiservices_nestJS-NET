namespace Auditory.API.Models.Events
{
    public class DocumentCreatedEvent
    {
        public int DocumentId { get; set; }
        public string Bucket { get; set; } = default!;
        public string Url { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
    }
}

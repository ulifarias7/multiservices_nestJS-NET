namespace Document.API.Models.Events
{
    public sealed class DocumentCreatedEvent
    {
        public int DocumentId { get; set; }
        public string Bucket { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}

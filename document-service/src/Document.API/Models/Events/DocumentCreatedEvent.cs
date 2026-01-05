namespace Document.API.Models.Events
{
    public class DocumentCreatedEvent
    {
        public Guid DocumentId { get; set; }
        public string BucketName { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string FileName { get; set; } = string.Empty;
    }
}

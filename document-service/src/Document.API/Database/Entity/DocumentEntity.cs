namespace Document.API.Database.Entity
{
    public class DocumentEntity
    {
        public int Id { get; set; }
        public int BucketId { get; set; }
        public string Url { get; set; }
        public DateTime CreatAt { get; set; } = DateTime.Now;
        public virtual BucketEntity Bucket { get; set; }
    }
}

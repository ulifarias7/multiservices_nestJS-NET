namespace Document.API.Database.Entity
{
    public class BucketEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<DocumentEntity> Documents { get; set; }
    }
}

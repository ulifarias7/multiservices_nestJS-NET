using Document.API.Database.Entity;
using Document.API.Models.Dtos;

namespace Document.API.Repository
{
    public interface IFileRepository
    {
        Task<bool> AddDocument(DocumentEntity document);
        Task<bool> AddBucket(BucketEntity bucket);
        Task<BucketEntity?> GetBucketByName(string bucketName);
    }
}

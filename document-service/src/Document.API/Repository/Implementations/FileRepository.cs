using Document.API.Database.Entity;
using Document.API.Database.Persistence;
using Document.API.Models.Dtos;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Document.API.Repository.Implementations
{
    public class FileRepository : IFileRepository
    {
        private readonly ApplicationDbContext _context;

        public FileRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddDocument(DocumentEntity document)
        {
            await _context.Documents.AddAsync(document);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddBucket(BucketEntity bucket)
        {
            await _context.Buckets.AddAsync(bucket);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<BucketEntity?> GetBucketByName(string bucketName)
        {
            return _context.Buckets
                .Where(b => b.Name == bucketName)
                .FirstOrDefault();
        }
    }
}

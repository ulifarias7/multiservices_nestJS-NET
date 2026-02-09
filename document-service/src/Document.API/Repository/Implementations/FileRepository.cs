using AutoMapper;
using AutoMapper.QueryableExtensions;
using Document.API.Common.filters;
using Document.API.Database.Entity;
using Document.API.Database.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Document.API.Repository.Implementations
{
    public class FileRepository : IFileRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public FileRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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

        public async Task<List<T>> FindAllByFilters<T>(DocumentFilterModel filter)
        {
            var query = _context.Documents.AsQueryable();

            query = ApplyDocumentFilters(query, filter);

            return await query.ProjectTo<T>(_mapper.ConfigurationProvider).ToListAsync();
        }

        private static IQueryable<DocumentEntity> ApplyDocumentFilters(IQueryable<DocumentEntity> query, DocumentFilterModel filter)
        {
            if (filter.IdList != null && filter.IdList.Any()) 
            {
               query = query.Where(d => filter.IdList.Contains(d.Id));
            }

            if (filter.PageSize > 0)
            {
                var skip = (filter.Page - 1) * filter.PageSize;
                query = query.Skip(skip).Take(filter.PageSize);
            }

            return query;
        }
    }
}

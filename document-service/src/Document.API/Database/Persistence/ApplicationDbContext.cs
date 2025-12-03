using Document.API.Database.Entity;
using Microsoft.EntityFrameworkCore;

namespace Document.API.Database.Persistence
{
    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options) {}

        public virtual DbSet<DocumentEntity> Documents { get; set; }
        public virtual DbSet<BucketEntity> Buckets { get; set; }
    }
}

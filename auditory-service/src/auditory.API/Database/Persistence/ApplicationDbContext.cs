using Auditory.API.Database.Entity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Auditory.API.Database.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        { }
        public virtual DbSet<AuditEntity> Audits { get; set; }
    }
}

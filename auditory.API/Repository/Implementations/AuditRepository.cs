using Auditory.API.Database.Entity;
using Auditory.API.Database.Persistence;

namespace Auditory.API.Repository.Implementations
{
    public class AuditRepository : IAuditRepository
    {
        private readonly ApplicationDbContext _context;

        public AuditRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AuditEntity?> GetRegister(Guid id)
        {
            return _context.Audits
               .Where(b => b.Id == id)
               .FirstOrDefault();
        }
    }
}

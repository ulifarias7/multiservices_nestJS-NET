using Auditory.API.Database.Entity;

namespace Auditory.API.Repository
{
    public interface IAuditRepository
    {
        Task<AuditEntity> GetRegister(int id);
    }
}

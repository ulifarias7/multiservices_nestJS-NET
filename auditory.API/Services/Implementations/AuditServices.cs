using Auditory.API.Models.Dtos;
using Auditory.API.Repository;

namespace Auditory.API.Services.Implementations
{
    public class AuditServices : IAuditServices
    {
        private readonly IAuditRepository _auditRepository;

        public Task<AuditDto> GetRegister(int id)
        {
            throw new NotImplementedException();
        }
    }
}

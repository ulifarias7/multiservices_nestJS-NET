using Auditory.API.Models.Dtos;

namespace Auditory.API.Services
{
    public interface IAuditServices
    {
        public Task<AuditDto> GetRegister(int id);
    }
}

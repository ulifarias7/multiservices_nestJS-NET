using Auditory.API.Models.Dtos;
using Auditory.API.Repository;

namespace Auditory.API.Services.Implementations
{
    public class AuditServices : IAuditServices
    {
        private readonly IAuditRepository _auditRepository;

        public AuditServices(IAuditRepository auditRepository)
        {
            _auditRepository = auditRepository;
        }

        public async Task<AuditDto> GetRegister(int id)
        {
            var register = await _auditRepository.GetRegister(id);

            if (register is null)
                throw new ArgumentException($"no existe registro con ese id: {id}");

            var registerDto = new AuditDto()
            {
                Id = register.Id,
                EventName = register.EventName,
                RoutingKey = register.RoutingKey,
                Exchange = register.Exchange,
                SourceService = register.SourceService,
                SourceHost = register.SourceHost,
                Payload = register.Payload,
                Headers = register.Headers,
                CorrelationId = register.CorrelationId,
                MessageId = register.MessageId,
            };

            return registerDto;
        }
    }
}

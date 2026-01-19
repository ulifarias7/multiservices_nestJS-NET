using Auditory.API.Database.Entity;
using AutoMapper;
using RabbitMQ.Client.Events;
using System.Text.Json;

namespace Auditory.API.MappingConfig
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<BasicDeliverEventArgs, AuditEntity>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.EventName, opt => opt.MapFrom(src => "DocumentCreated"))
                .ForMember(dest => dest.RoutingKey, opt => opt.MapFrom(src => src.RoutingKey))
                .ForMember(dest => dest.Exchange, opt => opt.MapFrom(src => src.Exchange))
                .ForMember(dest => dest.SourceService, opt => opt.Ignore()) 
                .ForMember(dest => dest.SourceHost, opt => opt.MapFrom(src => Environment.MachineName))
                .ForMember(dest => dest.Payload, opt => opt.Ignore()) 
                .ForMember(dest => dest.Headers, opt => opt.MapFrom(src =>
                    src.BasicProperties.Headers != null
                        ? JsonDocument.Parse(JsonSerializer.Serialize(src.BasicProperties.Headers))
                        : null))
                .ForMember(dest => dest.CorrelationId, opt => opt.MapFrom(src => src.BasicProperties.CorrelationId))
                .ForMember(dest => dest.CausationId, opt => opt.MapFrom(src => (string)null))
                .ForMember(dest => dest.MessageId, opt => opt.MapFrom(src =>
                    src.BasicProperties.MessageId ?? Guid.NewGuid().ToString()))
                .ForMember(dest => dest.OccurredAt, opt => opt.Ignore()) 
                .ForMember(dest => dest.ReceivedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
        }
    }
}

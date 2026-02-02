using Auth.API.Models.Dto;
using AutoMapper;
using Keycloak.Net.Models.Users;

namespace Auth.API.MappingConfig
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            // Crear usuario
            CreateMap<CreateUserKeycloacDto, User>()
               .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName ?? src.Email))
               .ForMember(dest => dest.EmailVerified, opt => opt.MapFrom(src => src.EmailVerified ?? false))
               .ForMember(dest => dest.Enabled, opt => opt.MapFrom(src => src.Enabled ?? true))
               .ForMember(dest => dest.Credentials, opt => opt.MapFrom(src =>
                            src.CredentialDto == null
                                ? null
                                : new List<Credentials>
                                {
                                    new Credentials
                                    {
                                        Type = "password",
                                        Value = src.CredentialDto.Value,
                                        Temporary = src.CredentialDto.Temporary ?? false
                                    }
                                }));

            //Get de usuario
            CreateMap<User, UserKeycloackDto>();

            //editar usuario
            CreateMap<UpdateUserKeycloakDto, User>();
        }
    }
}

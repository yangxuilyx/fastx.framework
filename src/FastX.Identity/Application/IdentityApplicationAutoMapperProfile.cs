using AutoMapper;
using FastX.Identity.Application.Identity.Ous.Dtos;
using FastX.Identity.Application.Identity.Roles.Dtos;
using FastX.Identity.Application.Identity.Users.Dtos;
using FastX.Identity.Core.Identity.Ous;
using FastX.Identity.Core.Identity.Roles;
using FastX.Identity.Core.Identity.Users;

namespace FastX.Identity.Application;

/// <summary>
/// XApplicationAutoMapperProfile
/// </summary>
public class IdentityApplicationAutoMapperProfile : Profile
{
    /// <summary>
    /// 
    /// </summary>
    public IdentityApplicationAutoMapperProfile()
    {
        CreateMap<UserDto, User>().ReverseMap();
        CreateMap<CreateUserDto, User>().ReverseMap();

        CreateMap<Ou, OuDto>().ReverseMap();
        CreateMap<Ou, CreateOuDto>().ReverseMap();

        CreateMap<Role, RoleDto>().ReverseMap();
    }
}

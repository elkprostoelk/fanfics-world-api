using AutoMapper;
using FanficsWorld.Common.DTO;
using FanficsWorld.DataAccess.Entities;

namespace FanficsWorld.WebAPI.Configurations;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<RegisterUserDTO, User>();
        CreateMap<User, SimpleUserDTO>();
        
        CreateMap<Fanfic, FanficDTO>()
            .ForMember(dto => dto.Coauthors,
                opts => opts.MapFrom(f => f.Coauthors))
            .ForMember(dto => dto.Author,
                opts => opts.MapFrom(f => f.Author));

        CreateMap<Fandom, FandomDTO>()
            .ForMember(dto => dto.Fanfics,
                opts => opts.MapFrom(fdom => fdom.Fanfics));
        CreateMap<Fandom, SimpleFandomDTO>();
        CreateMap<NewFandomDto, Fandom>();
    }
}
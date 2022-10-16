using AutoMapper;
using FanficsWorld.Common.DTO;
using FanficsWorld.DataAccess.Entities;

namespace FanficsWorld.WebAPI.Configurations;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<RegisterUserDto, User>();
        CreateMap<User, SimpleUserDto>();
        
        CreateMap<Fanfic, FanficDto>()
            .ForMember(dto => dto.Coauthors,
                opts => opts.MapFrom(f => f.Coauthors))
            .ForMember(dto => dto.Author,
                opts => opts.MapFrom(f => f.Author));

        CreateMap<Fandom, FandomDto>()
            .ForMember(dto => dto.Fanfics,
                opts => opts.MapFrom(fdom => fdom.Fanfics));
        CreateMap<Fandom, SimpleFandomDto>();
        CreateMap<NewFandomDto, Fandom>();

        CreateMap<Tag, TagDto>()
            .ReverseMap();
        CreateMap<Tag, TagWithFanficsDto>()
            .ForMember(dto => dto.Fanfics,
                opts => opts.MapFrom(t => t.Fanfics));
    }
}
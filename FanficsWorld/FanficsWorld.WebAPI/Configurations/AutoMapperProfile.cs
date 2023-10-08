using AutoMapper;
using FanficsWorld.Common.DTO;
using FanficsWorld.Common.Extensions;
using FanficsWorld.DataAccess.Entities;
using Microsoft.OpenApi.Extensions;

namespace FanficsWorld.WebAPI.Configurations;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<RegisterUserDto, User>();
        CreateMap<User, SimpleUserDto>();

        CreateMap<Fanfic, MinifiedFanficDto>();
        CreateMap<Fanfic, FanficDto>()
            .ForMember(dto => dto.Coauthors,
                opts => opts.MapFrom(f => f.Coauthors))
            .ForMember(dto => dto.Author,
                opts => opts.MapFrom(f => f.Author))
            .ForMember(dto => dto.Fandoms,
                opts => opts.MapFrom(f => f.Fandoms));

        CreateMap<Fanfic, SimpleFanficDto>()
            .ForMember(dto => dto.Coauthors,
                opts => opts.MapFrom(f => f.Coauthors))
            .ForMember(dto => dto.Author,
                opts => opts.MapFrom(f => f.Author))
            .ForMember(dto => dto.Fandoms,
                opts => opts.MapFrom(f => f.Fandoms))
            .ForMember(dto => dto.Tags,
                opts => opts.MapFrom(f => f.Tags))
            .ForMember(dto => dto.Direction,
                opts => opts.MapFrom(f => f.Direction.GetDisplayAttribute()))
            .ForMember(dto => dto.Origin,
                opts => opts.MapFrom(f => f.Origin.GetDisplayAttribute()))
            .ForMember(dto => dto.Rating,
                opts => opts.MapFrom(f => f.Rating.GetDisplayAttribute()))
            .ForMember(dto => dto.Status,
                opts => opts.MapFrom(f => f.Status.GetDisplayAttribute()));
        
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
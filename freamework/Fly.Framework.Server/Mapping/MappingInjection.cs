using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using Fly.Framework.Server.Persistence.Common;
using Fly.Framework.Shared.Common.Dtos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fly.Framework.Server.Mapping;

internal interface IServerFlyFrameworkMarker { }

public static class MappingInjection
{
    public static IServiceCollection InjectFrameworkMapping(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(typeof(IServerFlyFrameworkMarker));
        services.AddAutoMapper(ConfigAutoMapper);
        return services;
    }

    private static void ConfigAutoMapper(IMapperConfigurationExpression config)
    {
        config.AddExpressionMapping();
        config.CreateMap<TEntity, DtoBase>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(sec => sec.Guid))
        ;
        config.CreateMap<DtoBase, TEntity>()
            .ForMember(dest => dest.Guid, opt => opt.MapFrom(sec => sec.Id))
        ;
    }
}

using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Fly.Framework.Server.Mapping;
using Mapster;
using MapsterMapper;
using Fly.Framework.Shared.Common.Dtos;
using Fly.Framework.Server.Persistence.Common;
using System.Reflection;
using Fly.Core.Common.Attributes;
using Fly.Shared;

namespace Fly.Core.Common.Mapping;

//public class IdentifierFormatter : IValueConverter<Guid, Identifier>
//{
//    public Identifier Convert(Guid source, ResolutionContext context)
//        => Identifier.Create(source);
//}


internal static class CoreMappingInjection
{
    public static IServiceCollection InjectCoreMapping(this IServiceCollection services, IConfiguration configuration)
    {
        services.InjectFrameworkMapping(configuration);

        //  AUTOMAPPER
        services.AddAutoMapper(typeof(IFlyCoreMarker));
        services.AddAutoMapper(ConfigAutoMapper);

        //  MAPSTER
        AddMapster(services);

        return services;
    }

    private static void ConfigAutoMapper(IMapperConfigurationExpression config)
    {



        config.CreateMap<TEntity, DtoBase>()
        ;

        config.CreateMap<DtoBase, TEntity>()
        ;

        //config.CreateMap<TAccount, DtoAccount>()
        //    .IncludeBase<TEntity, DtoBase>()
        //;
        /*
        config.CreateMap<DtoCurrency, TCurrency>()
            .IncludeBase<DtoBase, TEntity>();
        config.CreateMap<TCurrency, DtoCurrency>()
            .IncludeBase<TEntity, DtoBase>();
         */
        InitAutomapper(config);
        // InitCommandsAutomapper(config);
    }

    private static void InitAutomapper(IMapperConfigurationExpression cfg)
    {
        // Get all types in the current assembly that have the MapTo attribute
        var typesWithMapTo = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.GetCustomAttribute<MapToAttribute>() != null);

        // Get the assembly containing the DtoBase type
        var dtoAssembly = typeof(IShareMarker).Assembly;

        var dtos = dtoAssembly.GetTypes().Where(w => w.Name.StartsWith("Dto"));

        // Iterate through each type with the MapTo attribute
        foreach (var type in typesWithMapTo)
        {
            // Get the MapTo attribute
            var mapToAttribute = type.GetCustomAttribute<MapToAttribute>();

            // Get the name of the source type, removing the initial "T"
            var sourceTypeName = type.Name[1..];  // Corrected line
            // If DtoType is not specified, use the convention
            if (mapToAttribute.DtoType == null)
            {

                // Construct the DTO type name using the convention
                var dtoTypeName = $"Dto{sourceTypeName}";

                // Get the DTO type from the DTO assembly
                var dtoType = dtos.FirstOrDefault(t => t.Name == dtoTypeName);

                // If the DTO type is found, create the mapping
                if (dtoType != null)
                {
                    var entityToDtoMapping = cfg.CreateMap(type, dtoType);
                    var dtoToEntityMapping = cfg.CreateMap(dtoType, type);
#if DEBUG
                    Console.WriteLine("{0}", sourceTypeName);
#endif
                }
                else
                {
                    Console.WriteLine("Cannot fine dto type:{0}", dtoTypeName);
                }
            }
            else
            {
                // Create the mapping using the specified DTO type
                var entityToDtoMapping = cfg.CreateMap(type, mapToAttribute.DtoType);
                var dtoToEntityMapping = cfg.CreateMap(mapToAttribute.DtoType, type);
#if DEBUG
                Console.WriteLine("{0}", sourceTypeName);
#endif
            }
        }
    }
    private static void InitCommandsAutomapper(IMapperConfigurationExpression cfg)
    {
        // Get all types in the current assembly that have the MapTo attribute
        var typesWithMapTo = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.GetCustomAttribute<MapToAttribute>() != null);

        // Get the assembly containing the DtoBase type
        var dtoAssembly = typeof(IShareMarker).Assembly;

        var dtos = dtoAssembly.GetTypes().Where(w => w.Name.StartsWith("Dto"));

        // Iterate through each type with the MapTo attribute
        foreach (var type in typesWithMapTo)
        {
            // Get the MapTo attribute
            var mapToAttribute = type.GetCustomAttribute<MapToAttribute>();

            // Get the name of the source type, removing the initial "T"
            var sourceTypeName = type.Name[1..];  // Corrected line
            // If DtoType is not specified, use the convention
            if (mapToAttribute.DtoType == null)
            {

                // Construct the DTO type name using the convention
                var dtoTypeName = $"Dto{sourceTypeName}";

                // Get the DTO type from the DTO assembly
                var dtoType = dtos.FirstOrDefault(t => t.Name == dtoTypeName);

                // If the DTO type is found, create the mapping
                if (dtoType != null)
                {
                    var entityToDtoMapping = cfg.CreateMap(type, dtoType);
                    var dtoToEntityMapping = cfg.CreateMap(dtoType, type);
#if DEBUG
                    Console.WriteLine("{0}", sourceTypeName);
#endif
                }
                else
                {
                    Console.WriteLine("Cannot fine dto type:{0}", dtoTypeName);
                }
            }
            else
            {
                // Create the mapping using the specified DTO type
                var entityToDtoMapping = cfg.CreateMap(type, mapToAttribute.DtoType);
                var dtoToEntityMapping = cfg.CreateMap(mapToAttribute.DtoType, type);
#if DEBUG
                Console.WriteLine("{0}", sourceTypeName);
#endif
            }
        }
    }

    private static void AddMapster(IServiceCollection services)
    {
        var config = new TypeAdapterConfig();
        // Or
        // var config = TypeAdapterConfig.GlobalSettings;
        services.AddSingleton(config);
        services.AddScoped<MapsterMapper.IMapper, ServiceMapper>();
    }
}

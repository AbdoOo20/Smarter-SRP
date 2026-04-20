using AutoMapper.QueryableExtensions;
using Mapster;

namespace Fly.Framework.Server.Persistence.Implementations;

internal static class ServicebaseHelpers
{
    public static IQueryable<TTdo> MapToDto<TTdo>(
        this IQueryable q,
        AutoMapper.IMapper mapper,
        MapsterMapper.IMapper mapsterMapper,
        bool useAutomapper = false)
    {
        if (useAutomapper)
            return q.ProjectTo<TTdo>(mapper.ConfigurationProvider);
        else
            return q.ProjectToType<TTdo>(mapsterMapper.Config);
    }
}
using Microsoft.Extensions.Logging;

namespace Fly.Core.Modules;

public class GlobalModuleService(ILogger<GlobalModuleService> logger,
    AutoMapper.IMapper mapper,
    MapsterMapper.IMapper mapsterMapper,
    IServiceProvider serviceProvider)

{
}
using Fly.Core.Modules.E.Store.Services.InternalServices;
using Fly.Core.Modules.E.Store.Services.ModuleService;
using Fly.Core.Modules.F.Maintenance.Services;
using Fly.Core.Modules.Factoring.FactoringInvoices.Services.InternalServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fly.Core.Modules.E.Store;

public static class StoreInjections
{
    public static IServiceCollection InjectStorelModule
       (this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IItemService, ItemService>();
        services.AddScoped<IFactoringInvoiceService, FactoringInvoiceService>();
        services.AddScoped<IMaintenanceInvoiceService, MaintenanceInvoiceService>();
        services.AddScoped<IItemCompositionService, ItemCompositionService>();
        services.AddScoped<IInventoryService, InventoryService>();

        //  GLOBAL
        services.AddScoped<IStorageModuleService, StorageModuleService>();

        return services;
    }

    public static WebApplication UsePersonnelModule(this WebApplication app)
    {
        return app;
    }
}

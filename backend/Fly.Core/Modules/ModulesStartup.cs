using Fly.Core.Modules.C.Accounting;
using Fly.Core.Modules.D.Personnel;
using Fly.Core.Modules.E.Store;
using Fly.Core.Modules.F.Marketers;
using Fly.Core.Modules.F.PurchaseAndSales;
using Fly.Core.Modules.Global;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fly.Core.Modules;

public static class ModulesStartup
{
    public static IServiceCollection InjectModules
        (this IServiceCollection services, IConfiguration configuration)
    {
        return services
                .InjectGlobalModule(configuration)
                .InjectAccountingModule(configuration)
            .InjectPersonnelModule(configuration)
            .InjectStorelModule(configuration)
            .InjectPurchaseAndSalesModule(configuration)
            .InjectMarketerSModule(configuration)
            ;
    }

    public static WebApplication UseModules(this WebApplication app)
    {
        return app
                .UseAccountingModule()
                .UsePurchaseAndSalesModule()
                .UseMarketerModule()
            ;
    }
}
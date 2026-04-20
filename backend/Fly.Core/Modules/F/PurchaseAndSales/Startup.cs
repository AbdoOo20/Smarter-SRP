using Fly.Core.Modules.F.PurchaseAndSales.Services.GlobalServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fly.Core.Modules.F.PurchaseAndSales;

public static class Startup
{
    public static IServiceCollection InjectPurchaseAndSalesModule
        (this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IPurchaseAndSalesModuleService, PurchaseAndSalesModuleService>();
        services.AddScoped<IDiscountitedItemService, DiscountitedItemService>();
        return services;
    }

    public static WebApplication UsePurchaseAndSalesModule(this WebApplication app)
    {
        app.Use(async (a, b) =>
        {

            //var accService = a.RequestServices.GetRequiredService<IServiceBase<DtoAccount, TAccount>>();
            //var seeded = AccountSeed.Accounts;

            //var ids = seeded.Select(s => s.Id).ToList();

            //var q_exists = await accService.GetAllAsAsync(s => s.Id, w => ids.Contains(w.Id));
            //var exists = q_exists.Items.ToList();

            //var notExists = ids.Except(exists);



            await b.Invoke();
        });
        return app;
    }
}
using FluentValidation;
using Fly.Core.Modules.C.Accounting.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fly.Core.Modules.C.Accounting;

public static class Startup
{
    public static IServiceCollection InjectAccountingModule
        (this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<AccountService>();
        services.AddTransient<EntryService>();
        services.AddTransient<AccountingFinacial>();

        services.AddTransient<IAccountModuleService, AccountService>();
        services.AddTransient<IEntryService, EntryService>();
        services.AddValidatorsFromAssemblyContaining<FlyCoreMarker>();
        return services;
    }

    public static WebApplication UseAccountingModule(this WebApplication app)
    {
        //app.Use(async (a, b) =>
        //{
        //    //var accService = a.RequestServices.GetRequiredService<IServiceBase<DtoAccount, TAccount>>();
        //    //var seeded = AccountSeed.Accounts;
        //    //var ids = seeded.Select(s => s.Id).ToList();
        //    //var q_exists = await accService.GetAllAsAsync(s => s.Id, w => ids.Contains(w.Id));
        //    //var exists = q_exists.Items.ToList();
        //    //var notExists = ids.Except(exists);
        //    await b.Invoke();
        //});
        return app;
    }
}
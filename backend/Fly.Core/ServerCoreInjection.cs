using Fly.Core.Common.Mapping;
using Fly.Core.Common.Swagger;
using Fly.Core.Modules;
using Fly.Core.Modules.C.Accounting;
using Fly.Core.Persistence.Ef.Contexts;
using Fly.Framework.Server;
using Fly.Framework.Server.Persistence.Contexts;
using Fly.Framework.Server.Persistence.Mysql;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using System.Text;
using Fly.Core.Common.Reporting;

namespace Fly.Core;

public static class ServerCoreInjection
{

    public static void StartPoint()
    {
        //var types = new[] { typeof(ReportItemClass1), typeof(ReportItemClass2), typeof(ReportItemClass3) };
        var types = typeof(RTModel).Assembly.GetTypes().Where(w=> w.Name.StartsWith("RT")).ToArray();
        var xri = new System.Xml.Serialization.XmlReflectionImporter();
        var xss = new System.Xml.Serialization.XmlSchemas();
        var xse = new System.Xml.Serialization.XmlSchemaExporter(xss);
        foreach (var type in types)
        {
            var xtm = xri.ImportTypeMapping(type);
            xse.ExportTypeMapping(xtm);
        }
        var dir = Path.Combine(Directory.GetCurrentDirectory(), "..\\Fly.Core", "Datasources");
        Directory.CreateDirectory(dir);
        var file = Path.Combine(dir, "ReportItemSchemas.xsd");
        using var sw = new System.IO.StreamWriter(file, false, Encoding.UTF8);
        for (int i = 0; i < xss.Count; i++)
        {
            var xs = xss[i];
            xs.Id = "ReportItemSchemas";
            xs.Write(sw);
        }
    }

    public static IServiceCollection InjectServerCore(this IServiceCollection services, IConfiguration configuration)
    {
        //  Base
        services.InjectFrameworkServer(configuration);
        services.InjectCoreMapping(configuration);
        services.AddMediatR((config) =>
        {
            config.RegisterServicesFromAssemblyContaining<IFlyCoreMarker>();
            config.RegisterServicesFromAssemblyContaining<IFlyServerFrameworkMarker>();
        });
        services.MySqlInjection<DbApplicationContext>(configuration);
        services.AddScoped<DbCustomeContext, DbApplicationContext>();
        services.AddValidatorsFromAssemblyContaining<IFlyCoreMarker>();

        //  Implementations
        services.InjectSwagger(configuration);

        //  Modules
        services.InjectModules(configuration);

        //  Server Config
        services.AddCors((opt) =>
        {
            opt.AddDefaultPolicy(p =>
            {
                p
                .AllowAnyHeader()
                .AllowAnyOrigin()
                .AllowAnyMethod();
            });
        });
        return services;
    }

    public static WebApplication UseApplication(this WebApplication app)
    {
        //  Base
        app.UseSwaggerTool();
        app.UseBaseFramework();

        //  Modules
        app.UseAccountingModule();

        //  Server Config
        app.UseCors();

        return app;
    }
}
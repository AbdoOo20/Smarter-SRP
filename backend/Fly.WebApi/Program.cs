using System.Net;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using Fly.Core;
using Fly.Core.Common.Reporting;
using Fly.Core.Persistence.Ef.Contexts;
using Fly.WebApi;
using Fly.WebApi.Tools.SaveFiles;
using Fly.WebApi.UdpDiscovery;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Serilog;

#if DEBUG
ServerCoreInjection.StartPoint();
#endif

LoggingServices.SetupLogger();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        var builtInFactory = options.InvalidModelStateResponseFactory;

        options.InvalidModelStateResponseFactory = context =>
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();

            var errors = context.ModelState
                .Where(ms => ms.Value.Errors.Any())
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );

            var errorLog = System.Text.Json.JsonSerializer.Serialize(errors);

            logger.LogWarning("Invalid model state received: {ModelStateErrors}", errorLog);

            return builtInFactory(context);
        };
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
Action<HttpsConnectionAdapterOptions> configureOptions = options =>
{
    options.SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls13;
};
builder.Services.AddEndpointsApiExplorer();
builder.Services.InjectServerCore(builder.Configuration);
builder.Services.AddTransient<ISaveReport, SaveReport>();
builder.Host.UseSerilog();
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    var resourcesLeapayBackendPfx = "Resources/leapay-backend.pfx";
    var pw = "m3w27b8E";
    
    serverOptions.ListenAnyIP(0,
        l => l.UseHttps(resourcesLeapayBackendPfx,
            pw, configureOptions));
    
    /*serverOptions.Listen(IPAddress.Loopback, 0,
        l => l.UseHttps(resourcesLeapayBackendPfx,
            pw, configureOptions));
    serverOptions.Listen(GetPrimaryLanIp(), 0,
        l => l.UseHttps(resourcesLeapayBackendPfx,
            pw, configureOptions));*/

    serverOptions.Limits.MaxRequestHeadersTotalSize = 64768; // default is 32 KB
    serverOptions.Limits.MaxRequestHeaderCount = 200;        // default is 100
});


var app = builder.Build();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DbApplicationContext>();
    
    var migrations = await db.Database.GetAppliedMigrationsAsync(CancellationToken.None);

    if (!migrations.Any())
    {
        // Fresh DB
    }
    
    await db.Database.MigrateAsync(); // Apply pending migrations
}
// add a fileprovider to serve static files for `wwwroot/images` folder
var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/images");

if (Directory.Exists(folder))
{
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/images")),
        RequestPath = "/images"
    });
}

app.UseHttpsRedirection();

app.UseApplication();

app.MapControllers();


_ = app.StartAsync();

Information("Bound to the following addresses: {0}", string.Join(", ", app.Urls));
var address = app.Urls.First();
var port = new Uri(address).Port;

var cts = new CancellationTokenSource();
_ = UdpDiscovery.BroadcastAsync(app.Urls.ToList(), port, cts.Token);

await app.WaitForShutdownAsync();

CloseAndFlush();


static IPAddress GetPrimaryLanIp()
{
    using var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0);

    socket.Connect("8.8.8.8", 65530);

    return ((IPEndPoint)socket.LocalEndPoint!).Address;
}
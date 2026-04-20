using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Threading;
using Fly.Framework.Desktop;
using Fly.Framework.Desktop.LicenseValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SmartErpWin.Core;
using SmartErpWin.Core.Exceptions;
using SmartiErpWin.Client.DtoModels.Reports;
using SmartiErpWin.Client.Reports;
using SmartiErpWin.Client.Reports.Fixed.Models;
using SmartiErpWin.Client.ViewModels.Pages;
using Wpf.Ui;
using Wpf.Ui.DependencyInjection;

namespace SmartiErpWin.Client;

public partial class App
{
    private static readonly IHost _host = Host
        .CreateDefaultBuilder()
        .ConfigureAppConfiguration(c =>
        {
            c.SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location));
        })
        .ConfigureServices((context, services) =>
        {
            _ = services.AddNavigationViewPageProvider();

            InjectionServices.BeginInjection(services);
            services.InjecDesktop(context.Configuration);

            services.AddHostedService<ApplicationHostService>();

            services
                .AddSingleton<INavigationService, NavigationService>()
                .AddSingleton<ISnackbarService, SnackbarService>()
                .AddSingleton<IContentDialogService, ContentDialogService>();
        }).Build();

    public static T? GetService<T>()
        where T : class
    {
        return _host.Services.GetService(typeof(T)) as T;
    }

     private void CreateDatasources()
    {
        var types = new[] {
            typeof(ClientInfo),
            typeof(SalesInvoiceDto),
            typeof(SalesInvoiceDetailDto),
            typeof(PurchaseInvoiceDto) ,
            typeof(PurchaseInvoiceDetailDto) ,
            typeof(RandomDto) ,
            //  new-style
            typeof(AccountMovementsReportModel) ,
            typeof(TheClientDebtionReportModel) ,
        };
        var xri = new System.Xml.Serialization.XmlReflectionImporter();
        var xss = new System.Xml.Serialization.XmlSchemas();
        var xse = new System.Xml.Serialization.XmlSchemaExporter(xss);
        foreach (var type in types)
        {
            var xtm = xri.ImportTypeMapping(type);
            xse.ExportTypeMapping(xtm);
        }
        var proj = Directory.GetCurrentDirectory();
        var pathDir = Path.Combine(proj, "DataSources");
        Directory.CreateDirectory(pathDir);
        var pathFile = Path.Combine(pathDir, "ReportItemSchemas.xsd");
        using var sw = new System.IO.StreamWriter(pathFile, false, Encoding.UTF8);
        for (int i = 0; i < xss.Count; i++)
        {
            var xs = xss[i];
            xs.Id = "ReportItemSchemas";
            xs.Write(sw);
        }
    }

    private void OnStartup(object sender, StartupEventArgs e)
    {
        //var licenseValidation = BasicLicenseValidation.ValidateLicense();
#if DEBUG
        CreateDatasources();
#endif
        ReportRunner.PreoarwReportRunner();
        var licenseValidation = GetService<ISubscriberModule>()!;
        if (!licenseValidation.SyncSubscription())
        {
            MessageBox.Show("Cannot find MMtech2024.DLL");
            Application.Current.Shutdown(-55);
        }
        else
        {
            ReportRunnerHelper.RegisterEvents();
            /*var file = Path.Combine(Directory.GetCurrentDirectory(), "config", "client.json");
            var fileImage = Path.Combine(Directory.GetCurrentDirectory(), "images", "logo.png");
            if (File.Exists(fileImage))
            {
                var @string = File.ReadAllText(file);
                CLientInfo = JsonConvert.DeserializeObject<CLientInfo>(@string);
                CLientInfo = new CLientInfo()
                {
                    Image = "logo.png",
                };
                CLientInfo?.LoadImage();
            }
            else
            {
                var json = JsonConvert.SerializeObject(CLientInfo);
                File.WriteAllText(file, json);
            }*/
            LoggingServices.SetupLogger();
            //new Reports.ReportRunnerHelper();
            _host.Start();
        }
    }

    private async void OnExit(object sender, ExitEventArgs e)
    {
        AuthenticationStorageService.DeleteToken();
        await _host.StopAsync();

        await CloseAndFlushAsync();
        _host.Dispose();
    }

    private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        if (e.Exception is not { } exc)
        {
            _ = DialogueServices.ShowMessageAsync("Error",
                "An unknown error occurred.",
                "Ok");
            return;
        }

        Error(exc, "Unhandled error occurred");

        var locale = GetService<LocalisationService>();
        e.Handled = true;

        if (e.Exception is LoggedException logEx)
        {
            if (SkippedErrors.Any(m => logEx.Message.Contains(m)))
            {
                Information("Silencing error: {0} from user interface", logEx.Message);
                return;
            }
            var msg = logEx.ErrorCode switch
            {
                //400 => locale["SystemErrorContactTechSupport"],
                401 => Logout(),
                500 => string.Format(locale["SystemErrorContactTechSupportWithErrorMessage"], logEx.Message),
                400 => string.Format(locale["SupportWithErrorMessage"], logEx.Message),
                _ => logEx.Message
            };

            _ = DialogueServices.ShowMessageAsync("Error", msg, locale["OK"]);
            return;

            string Logout()
            {
                var settings = GetService<SettingsPageViewModel>();
                settings?.LogoutCommand.Execute(null);

                return "You are unauthorised to do this action. You will be logged out, and" +
                       "your session deemed expired.";
            }
        }

        _ = DialogueServices.ShowMessageAsync(locale["Error"], exc.Message, locale["OK"]);
    }

    public List<string> SkippedErrors =
    [
        "لم يتم العثور على الوحدة"
    ];
}
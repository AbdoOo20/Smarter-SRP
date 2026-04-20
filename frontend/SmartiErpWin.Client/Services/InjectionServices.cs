using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using SmartiErpWin.Client.Profiles;
using SmartiErpWin.Client.UserInterface.Accounting.AccountingTree;
using SmartiErpWin.Client.UserInterface.Accounting.Assets;
using SmartiErpWin.Client.UserInterface.Accounting.Entries;
using SmartiErpWin.Client.UserInterface.Accounting.ExpenseTypes;
using SmartiErpWin.Client.UserInterface.Financial.DailyEntries;
using SmartiErpWin.Client.UserInterface.Financial.FinancingOperations;
using SmartiErpWin.Client.UserInterface.Financial.StatementsLookup;
using SmartiErpWin.Client.UserInterface.Management.Groups;
using SmartiErpWin.Client.UserInterface.Management.Users;
using SmartiErpWin.Client.UserInterface.Marketing.Marketer;
using SmartiErpWin.Client.UserInterface.Marketing.MarketerEntry;
using SmartiErpWin.Client.UserInterface.Misc.AccountSettings;
using SmartiErpWin.Client.UserInterface.Misc.MedianPage;
using SmartiErpWin.Client.UserInterface.Personnel.AttendanceAndDeparture;
using SmartiErpWin.Client.UserInterface.Personnel.Departments;
using SmartiErpWin.Client.UserInterface.Personnel.DiscountsAndIncreases;
using SmartiErpWin.Client.UserInterface.Personnel.EmployeeAssignments;
using SmartiErpWin.Client.UserInterface.Personnel.Employees;
using SmartiErpWin.Client.UserInterface.Purchases.PurchaseInvoices;
using SmartiErpWin.Client.UserInterface.Purchases.PurchaseReturnInvoice;
using SmartiErpWin.Client.UserInterface.Reports.AccountStatementReport;
using SmartiErpWin.Client.UserInterface.Reports.GeneralReport;
using SmartiErpWin.Client.UserInterface.Reports.Navigator;
using SmartiErpWin.Client.UserInterface.Sales.BankBranches;
using SmartiErpWin.Client.UserInterface.Sales.Banks;
using SmartiErpWin.Client.UserInterface.Sales.Clients;
using SmartiErpWin.Client.UserInterface.Sales.QuantityCrediting;
using SmartiErpWin.Client.UserInterface.Sales.Safes;
using SmartiErpWin.Client.UserInterface.Sales.SaleInvoices;
using SmartiErpWin.Client.UserInterface.Sales.SalesCheckout;
using SmartiErpWin.Client.UserInterface.Sales.SellReturnInvoice;
using SmartiErpWin.Client.UserInterface.Stores.Categories;
using SmartiErpWin.Client.UserInterface.Stores.CollectionPrice;
using SmartiErpWin.Client.UserInterface.Stores.CostCentres;
using SmartiErpWin.Client.UserInterface.Stores.DisbursementPermits;
using SmartiErpWin.Client.UserInterface.Stores.Items;
using SmartiErpWin.Client.UserInterface.Stores.Stores;
using SmartiErpWin.Client.UserInterface.Stores.Suppliers;
using SmartiErpWin.Client.UserInterface.Stores.SupplyPermits;
using SmartiErpWin.Client.UserInterface.Stores.Units;
using SmartiErpWin.Client.ViewModels.Pages;
using SmartiErpWin.Client.ViewModels.Windows;
using SmartiErpWin.Client.Views.Pages;
using SmartiErpWin.Client.Views.Windows;
using Wpf.Ui;

namespace SmartiErpWin.Client.Services;

public static class InjectionServices
{
    public static IServiceProvider? ServiceProvider { get; set; }
    
    public static void BeginInjection(IServiceCollection services)
    {
        InjectServices(services);
        InjectWindows(services);
        InjectWindowViewModels(services);
        InjectPages(services);
        InjectPageViewModels(services);

        ServiceProvider = services.BuildServiceProvider();
    }

    #region Services

    private static void InjectServices(IServiceCollection services)
    {
        var config = new MapperConfiguration((cfg) =>
            cfg.AddProfile<MappingProfile>());

        var mapper = config.CreateMapper();

        services.AddSingleton(mapper)
            .AddSingleton<BackendService>()
            .AddSingleton<LocalisationService>()
            .AddSingleton<AuthenticationStorageService>()
            .AddSingleton<SessionService>()
            .AddSingleton<HttpClient>()
            .AddSingleton<INavigationWindow, MainWindow>()
            .AddSingleton<INavigationService, NavigationService>()
            .AddSingleton<ITaskBarService, TaskBarService>()
            .AddSingleton<IThemeService, ThemeService>();
    }

    #endregion
    
    #region Windows

    private static void InjectWindows(IServiceCollection services)
    {
        services.AddSingleton<MainWindow>()
            .AddSingleton<LoginWindow>();
    }
    
    private static void InjectWindowViewModels(IServiceCollection services)
    {
        services.AddSingleton<MainWindowViewModel>()
            .AddSingleton<LoginWindowViewModel>();
    }

    #endregion

    #region Pages

    private static void InjectPages(IServiceCollection services)
    {
        services.AddSingleton<DashboardPage>()
            .AddSingleton<SettingsPage>()
            .AddSingleton<AccountingTreePage>()
            .AddSingleton<DashboardPage>()
            .AddSingleton<BanksPage>()
            .AddSingleton<EntryPage>()
            .AddSingleton<VaultsPage>()
            .AddSingleton<BankBranchesPage>()
            .AddSingleton<ExpenseTypesPage>()
            .AddSingleton<AssetsPage>()
            .AddSingleton<EmployeesPage>()
            .AddSingleton<AssignmentsPage>()
            .AddSingleton<DepartmentsPage>()
            .AddSingleton<AttendanceDeparturePage>()
            .AddSingleton<DiscountIncreasePage>()
            .AddSingleton<EmployeesPage>()
            .AddSingleton<CategoriesPage>()
            .AddSingleton<CostCentresPage>()
            .AddSingleton<StoresPage>()
            .AddSingleton<SuppliersPage>()
            .AddSingleton<UnitsPage>()
            .AddSingleton<ItemsPage>()
            .AddSingleton<SupplyPermitsPage>()
            .AddSingleton<DisbursementsPermitsPage>()
            .AddSingleton<SalesInvoicePage>()
            .AddSingleton<SellReturnInvoicePage>()
            .AddSingleton<PurchasesInvoicePage>()
            .AddSingleton<PurchaseReturnInvoicePage>()
            .AddSingleton<DailyEntriesPage>()
            .AddSingleton<FinancingOperationsPage>()
            .AddSingleton<MarketerDisbursementPermitPage>()
            .AddSingleton<ClientsPage>()
            .AddSingleton<AccountSettingsPage>()
            .AddSingleton<MarketerInfoPage>()
            .AddSingleton<AccountStatementReportPage>()
            .AddSingleton<StatementsNavigatorPage>()
            .AddSingleton<StatementsLookupPage>()
            .AddSingleton<GeneralReportPage>()
            .AddSingleton<SalesCheckoutEntryWindow>()
            .AddSingleton<GroupPage>()
            .AddSingleton<UserPage>()
            .AddSingleton<MedianPage>()
            .AddSingleton<QuantityCreditingPage>()
            .AddSingleton<CollectionPricesPage>()
            
            ;
    }
    
    private static void InjectPageViewModels(IServiceCollection services)
    {
        services.AddSingleton<DashboardPageViewModel>()
            .AddSingleton<SettingsPageViewModel>()
            .AddSingleton<AccountingTreePageViewModel>()
            .AddSingleton<DashboardPageViewModel>()
            .AddSingleton<EntryPageViewModel>()
            .AddSingleton<BanksPageViewModel>()
            .AddSingleton<VaultsPageViewModel>()
            .AddSingleton<BankBranchesPageViewModel>()
            .AddSingleton<ExpenseTypesPageViewModel>()
            .AddSingleton<AssetsPageViewModel>()
            .AddSingleton<EmployeesPageViewModel>()
            .AddSingleton<AssignmentsPageViewModel>()
            .AddSingleton<DepartmentsPageViewModel>()
            .AddSingleton<DiscountIncreasePageViewModel>()
            .AddSingleton<AttendanceDeparturePageViewModel>()
            .AddSingleton<EmployeesPageViewModel>()
            .AddSingleton<CategoriesPageViewModel>()
            .AddSingleton<CostCentresPageViewModel>()
            .AddSingleton<StoresPageViewModel>()
            .AddSingleton<SuppliersPageViewModel>()
            .AddSingleton<UnitsPageViewModel>()
            .AddSingleton<ItemsPageViewModel>()
            .AddSingleton<SupplyPermitsPageViewModel>()
            .AddSingleton<DisbursementsPermitsPageViewModel>()
            .AddSingleton<SalesInvoicePageViewModel>()
            .AddSingleton<SellReturnInvoicePageViewModel>()
            .AddSingleton<PurchaseReturnInvoicePageViewModel>()
            .AddSingleton<PurchasesInvoicePageViewModel>()
            .AddSingleton<DailyEntriesPageViewModel>()
            .AddSingleton<FinancingOperationsPageViewModel>()
            .AddSingleton<MarketerDisbursementPermitPageViewModel>()
            .AddSingleton<ClientsPageViewModel>()
            .AddSingleton<AccountSettingsPageViewModel>()
            .AddSingleton<MarketerInfoPageViewModel>()
            .AddSingleton<AccountStatementReportPageViewModel>()
            .AddSingleton<StatementsNavigatorPageViewModel>()
            .AddSingleton<StatementsLookupPageViewModel>()
            .AddSingleton<GeneralReportPageViewModel>()
            .AddSingleton<SalesCheckoutEntryViewModel>()
            .AddSingleton<GroupPageViewModel>()
            .AddSingleton<UserPageViewModel>()
            .AddSingleton<MedianPageViewModel>()
            .AddSingleton<QuantityCreditingPageViewModel>()

            .AddSingleton<CollectionPricesPageViewModel>()

            ;
    }

    #endregion
}
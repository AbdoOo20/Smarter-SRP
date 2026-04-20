using Avalonia.Preferences;
using Fly.Framework.Desktop.LicenseValidation;
using Fly.Framework.Desktop.Toos;
using Leapay.Client.Features.Theming;
using Leapay.Client.UserInterface.Accounting.AccountingTree;
using Leapay.Client.UserInterface.Accounting.Assets;
using Leapay.Client.UserInterface.Accounting.Entries;
using Leapay.Client.UserInterface.Accounting.ExpenseTypes;
using Leapay.Client.UserInterface.Authentication;
using Leapay.Client.UserInterface.Common.HomePage;
using Leapay.Client.UserInterface.Factoring.FactoringInvoice;
using Leapay.Client.UserInterface.Factoring.ManufacturingFacility;
using Leapay.Client.UserInterface.FinancingOperations;
using Leapay.Client.UserInterface.Maintenance.MaintenanceSector;
using Leapay.Client.UserInterface.Management.Groups;
using Leapay.Client.UserInterface.Management.Users;
using Leapay.Client.UserInterface.Marketer.MarketerDisbursementPermits;
using Leapay.Client.UserInterface.Marketer.MarketerEntry;
using Leapay.Client.UserInterface.Misc.Configuration;
using Leapay.Client.UserInterface.Personnel.Assignments;
using Leapay.Client.UserInterface.Personnel.Departments;
using Leapay.Client.UserInterface.Personnel.DiscountIncrease;
using Leapay.Client.UserInterface.Personnel.Employees;
using Leapay.Client.UserInterface.Purchases.PurchaseReturnInvoice;
using Leapay.Client.UserInterface.Purchases.PurchasesInvoice;
using Leapay.Client.UserInterface.Reports.GeneralReport;
using Leapay.Client.UserInterface.Reports.Navigator;
using Leapay.Client.UserInterface.Routing;
using Leapay.Client.UserInterface.Sales.BankBranches;
using Leapay.Client.UserInterface.Sales.Banks;
using Leapay.Client.UserInterface.Sales.Clients;
using Leapay.Client.UserInterface.Sales.QuantityCrediting;
using Leapay.Client.UserInterface.Sales.Safes;
using Leapay.Client.UserInterface.Sales.SalesInvoice;
using Leapay.Client.UserInterface.Sales.SalesPoint;
using Leapay.Client.UserInterface.Sales.SalesReturnInvoice;
using Leapay.Client.UserInterface.StoresAndCategories.Categories;
using Leapay.Client.UserInterface.StoresAndCategories.CostCentres;
using Leapay.Client.UserInterface.StoresAndCategories.DisbursementPermits;
using Leapay.Client.UserInterface.StoresAndCategories.ItemCompositions;
using Leapay.Client.UserInterface.StoresAndCategories.Items;
using Leapay.Client.UserInterface.StoresAndCategories.ItemsTransferInvoice;
using Leapay.Client.UserInterface.StoresAndCategories.ItemVariance;
using Leapay.Client.UserInterface.StoresAndCategories.Stores;
using Leapay.Client.UserInterface.StoresAndCategories.Suppliers;
using Leapay.Client.UserInterface.StoresAndCategories.SupplyPermits;
using Leapay.Client.UserInterface.StoresAndCategories.Units;
using Microsoft.Extensions.DependencyInjection;
using SukiUI.Dialogs;
using SukiUI.Toasts;
using MaintenanceInvoicesPage = Leapay.Client.UserInterface.Maintenance.MaintenanceInvoice.MaintenanceInvoicesPage;
using MaintenanceInvoicesPageViewModel = Leapay.Client.UserInterface.Maintenance.MaintenanceInvoice.MaintenanceInvoicesPageViewModel;

namespace Leapay.Client.Services;

public static class InjectionServices
{
    public static void BeginInjection(IServiceCollection services)
    {
        InjectServices(services);
        InjectWindows(services);
        InjectWindowViewModels(services);
        InjectPages(services);
    }

    #region Services

    private static void InjectServices(IServiceCollection services)
    {
        services.AddSingleton<ClipboardService>();
        services.AddSingleton<PageNavigationService>();
        services.AddSingleton<ISukiToastManager, SukiToastManager>();
        services.AddSingleton<ISukiDialogManager, SukiDialogManager>()
            .AddSingleton<Preferences>()
            .AddSingleton(new HttpClient(BackendService.CreatePinnedHandler()))
            .AddSingleton<LocalisationService>()
            .AddSingleton<AuthenticationStorageService>()
            .AddSingleton<SessionService>()
            .AddSingleton<LoginWindow>()
            .AddSingleton<LoginWindowViewModel>()
            .AddSingleton<BackendService>()
            .AddSingleton<TabbedNavigationalPage>()
            .AddSingleton<TabbedNavigationalPageViewModel>()
            .AddSingleton<BasicSubscriber>()
            .AddSingleton<ISubscriberModule, SubscriberModule>();
    }

    #endregion
    
    #region Windows

    private static void InjectWindows(IServiceCollection services)
    {
        services.AddSingleton<MainWindow>();
    }
    
    private static void InjectWindowViewModels(IServiceCollection services)
    {
        services.AddSingleton<MainWindowViewModel>();
    }

    #endregion

    #region Pages

    private static void InjectPages(IServiceCollection services)
    {
        services.AddSingleton<AccountingTreePage>()
            .AddSingleton<AccountingTreePageViewModel>()
        
            .AddSingleton<HomePage>()
            .AddSingleton<HomePageViewModel>()
            
            .AddSingleton<BanksPage>()
            .AddSingleton<BanksPageViewModel>()
            
            .AddSingleton<ThemingView>()
            .AddSingleton<ThemingViewModel>()
            
            .AddSingleton<EntryPage>()
            .AddSingleton<EntryPageViewModel>()
            
            .AddSingleton<SafesPage>()
            .AddSingleton<SafesPageViewModel>()
            
            .AddSingleton<BankBranchesPage>()
            .AddSingleton<BankBranchesPageViewModel>()
            
            .AddSingleton<MarketerPage>()
            .AddSingleton<MarketerPageViewModel>()
            
            .AddSingleton<ExpensesPage>()
            .AddSingleton<ExpensesPageViewModel>()
            
            .AddSingleton<PurchaseInvoicesPage>()
            .AddSingleton<PurchaseInvoicesPageViewModel>()
            
            .AddSingleton<ItemsPage>()
            .AddSingleton<ItemsPageViewModel>()
            
            .AddSingleton<AssetsPage>()
            .AddSingleton<AssetsPageViewModel>()
            
            .AddSingleton<EmployeesPage>()
            .AddSingleton<EmployeesPageViewModel>()
            
            .AddSingleton<AssignmentsPage>()
            .AddSingleton<AssignmentsPageViewModel>()
            
            .AddSingleton<ItemCompositionsPage>()
            .AddSingleton<ItemCompositionsPageViewModel>()
            
            .AddSingleton<DepartmentsPage>()
            .AddSingleton<DepartmentsPageViewModel>()
            
            .AddSingleton<DiscountIncreasePage>()
            .AddSingleton<DiscountIncreasePageViewModel>()
            
            .AddSingleton<EmployeesPage>()
            .AddSingleton<EmployeesPageViewModel>()
            
            .AddSingleton<CategoriesPage>()
            .AddSingleton<CategoriesPageViewModel>()
            
            .AddSingleton<ItemsTransferInvoicesPage>()
            .AddSingleton<ItemsTransferInvoicesPageViewModel>()
            
            .AddSingleton<CostCentresPage>()
            .AddSingleton<CostCentresPageViewModel>()
            
            .AddSingleton<StoresPage>()
            .AddSingleton<StoresPageViewModel>()
            
            .AddSingleton<SuppliersPage>()
            .AddSingleton<SuppliersPageViewModel>()
            
            .AddSingleton<UnitsPage>()
            .AddSingleton<UnitsPageViewModel>()
            
            .AddSingleton<FinancingOperationsPage>()
            .AddSingleton<FinancingOperationsPageViewModel>()
            
            .AddSingleton<ClientsPage>()
            .AddSingleton<ClientsPageViewModel>()
            
            .AddSingleton<FactoringInvoicesPage>()
            .AddSingleton<FactoringInvoicesPageViewModel>()
            
            .AddSingleton<ItemVariancesPage>()
            .AddSingleton<ItemVariancesPageViewModel>()
            
            .AddSingleton<GeneralReportPage>()
            .AddSingleton<GeneralReportPageViewModel>()
            
            .AddSingleton<GroupsPage>()
            .AddSingleton<GroupsPageViewModel>()
            
            .AddSingleton<UsersPage>()
            .AddSingleton<UsersPageViewModel>()
            
            .AddSingleton<MedianPage>()
            .AddSingleton<MedianPageViewModel>()
            
            .AddSingleton<SalesPointPage>()
            .AddSingleton<SalesPointPageViewModel>()
            
            .AddSingleton<ReportsNavigatorPage>()
            .AddSingleton<ReportsNavigatorPageViewModel>()
            
            .AddSingleton<MaintenanceInvoicesPage>()
            .AddSingleton<MaintenanceInvoicesPageViewModel>()
            
            .AddSingleton<MaintenanceSectorsPage>()
            .AddSingleton<MaintenanceSectorsPageViewModel>()
            
            .AddSingleton<ManufacturingFacilitiesPage>()
            .AddSingleton<ManufacturingFacilitiesPageViewModel>()
            
            .AddSingleton<SupplyPermitsPage>()
            .AddSingleton<SupplyPermitsPageViewModel>()
            
            .AddSingleton<DisbursementPermitsPage>()
            .AddSingleton<DisbursementPermitsPageViewModel>()
            
            .AddSingleton<SellReturnInvoicesPage>()
            .AddSingleton<SellReturnInvoicesPageViewModel>()
            
            .AddSingleton<PurchaseReturnInvoicesPage>()
            .AddSingleton<PurchaseReturnInvoicesPageViewModel>()
            
            .AddSingleton<QuantityCreditingsPage>()
            .AddSingleton<QuantityCreditingsPageViewModel>()
            
            .AddSingleton<MarketerDisbursementPermitsPage>()
            .AddSingleton<MarketerDisbursementPermitsPageViewModel>()
            
            .AddSingleton<SalesInvoicePage>()
            .AddSingleton<SalesInvoicePageViewModel>()
        
            .AddSingleton<CompanyInfoPage>()
            .AddSingleton<CompanyInfoPageViewModel>();
    }
    #endregion
}
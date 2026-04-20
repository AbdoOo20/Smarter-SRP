using Avalonia.Data;
using Avalonia.Media;
using Avalonia.Threading;
using Leapay.Client.Features.Theming;
using Leapay.Client.Helpers;
using Leapay.Client.Services;
using Leapay.Client.UserInterface.Accounting.AccountingTree;
using Leapay.Client.UserInterface.Accounting.Assets;
using Leapay.Client.UserInterface.Accounting.Entries;
using Leapay.Client.UserInterface.Accounting.ExpenseTypes;
using Leapay.Client.UserInterface.Common.HomePage;
using Leapay.Client.UserInterface.Factoring.FactoringInvoice;
using Leapay.Client.UserInterface.Factoring.ManufacturingFacility;
using Leapay.Client.UserInterface.FinancingOperations;
using Leapay.Client.UserInterface.Maintenance.MaintenanceInvoice;
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
using Leapay.Client.Wrappers;
using Material.Icons.Avalonia;
using SmartErpWin.Core;
using SukiUI.Controls;

namespace Leapay.Client;

public partial class MainWindowViewModel
{
    
    private SukiSideMenu? _sideMenu;

    private bool _watchForNextSelectionToCollapseMenu;
    
    public void SetMenuItems(SukiSideMenu sideMenu)
    {
        var tabVm = App.GetService<TabbedNavigationalPageViewModel>();
        tabVm.OnSynchronisationRequestedAsync(CodeBook.RefCom, PreferencesService.Get(s => s.LastOpenView));
        
        _sideMenu = sideMenu;
        
        _sideMenu.Items.Clear();

        List<SukiSideMenuItem> items =
        [
            CreateNav<HomePage>("HomeTab", "Home", MaterialIconKind.Home).Item,
            CreateNav("AccountingTab", MaterialIconKind.Account,
                CreateNav<AccountingTreePage>("AccountingTreeTab", "AccountingTree", MaterialIconKind.Tree),
                CreateNav<AssetsPage>("AssetsTab", "Assets", MaterialIconKind.Package),
                CreateNav<ExpensesPage>("ExpensesTab", "ExpenseTypes", MaterialIconKind.InvoiceLineItems),
                CreateNav<EntryPage>("EntryTab", "Entry", MaterialIconKind.Billboard)).Item,
            CreateNav("PersonnelTab", MaterialIconKind.People,
                CreateNav<DepartmentsPage>("DepartmentsTab", "Department", MaterialIconKind.OfficeBuilding),
                CreateNav<EmployeesPage>("EmployeesTab", "Employee", MaterialIconKind.Person),
            CreateNav<AssignmentsPage>("AssignmentsTab", "EmployeeAssignment", MaterialIconKind.Assignment),
            CreateNav<DiscountIncreasePage>("DiscountIncreaseTab", "DiscountIncrease", MaterialIconKind.Increment)).Item,
            CreateNav("StoresCategoriesTab", MaterialIconKind.Store,
                CreateNav<CategoriesPage>("CategoriesTab", "Category", MaterialIconKind.Category),
                CreateNav<ItemsPage>("ItemsTab", "Item", MaterialIconKind.Package),
                CreateNav<UnitsPage>("UnitsTab", "Unit", MaterialIconKind.CurrencyBtc),
                CreateNav<ItemVariancesPage>("ItemVariancesTab", "ItemVariance", MaterialIconKind.TrackChanges),
                CreateNav<ItemCompositionsPage>("ItemCompositionsTab", "ItemComposition", MaterialIconKind.PackageOpen),
                CreateNav<CostCentresPage>("CostCentresTab", "CostCenter", MaterialIconKind.FitnessCenter),
                CreateNav<SuppliersPage>("SuppliersTab", "Supplier", MaterialIconKind.Hand),
                CreateNav<StoresPage>("StoresTab", "Store", MaterialIconKind.ShopCheck),
                CreateNav<ItemsTransferInvoicesPage>("ItemsTransferInvoicesTab", "ItemsTransferInvoice", MaterialIconKind.TruckDelivery),
                CreateNav<SupplyPermitsPage>("SupplyPermitsTab", "SupplyPermit", MaterialIconKind.Handshake),
                CreateNav<DisbursementPermitsPage>("DisbursementPermitTab", "DisbursementPermit", MaterialIconKind.PackageOff)).Item,
            CreateNav("SalesTab", MaterialIconKind.Sale,
                CreateNav<ClientsPage>("ClientsTab", "Client", MaterialIconKind.PersonCheck),
                CreateNav<SalesInvoicePage>("SalesInvoiceTab", "Sales", MaterialIconKind.Sale),
                CreateNav<SellReturnInvoicesPage>("SellReturnInvoiceTab", "SellReturn", MaterialIconKind.CashChargeback),
                CreateNav<QuantityCreditingsPage>("QuantityCreditingTab", "QuantityCrediting", MaterialIconKind.PackageVariantAdd),
                CreateNav<SalesPointPage>("SalesCheckoutTab", "FastSales", MaterialIconKind.ShopCheck)).Item,
            CreateNav("BanksSafesTab", MaterialIconKind.Safe, CreateNav<SafesPage>("SafesTab", "Safe", MaterialIconKind.Safe),
                CreateNav<BanksPage>("BankTab", "Bank", MaterialIconKind.Bank),
                CreateNav<BankBranchesPage>("BankBranchTab", "BankBranch", MaterialIconKind.BankTransfer)).Item,
            CreateNav("PurchasesTab", MaterialIconKind.Purse,
                CreateNav<PurchaseInvoicesPage>("PurchasesInvoicesTab", "Purchase", MaterialIconKind.Cart),
                CreateNav<PurchaseReturnInvoicesPage>("PurchasesReturnInvoicesTab", "PurchaseReturnInvoice", MaterialIconKind.Feedback)).Item,
            CreateNav("MaintenanceTab", MaterialIconKind.ScheduledMaintenance, 
                CreateNav<MaintenanceSectorsPage>("MaintenanceSectorsTab", "MaintenanceSector", MaterialIconKind.Worker),
                CreateNav<MaintenanceInvoicesPage>("MaintenanceInvoiceTab", "MaintenanceInvoice", MaterialIconKind.Wrench)).Item,
            CreateNav("ManufacturingTab", MaterialIconKind.Factory, 
                CreateNav<ManufacturingFacilitiesPage>("ManufacturingFacilitiesTab", "ManufacturingFacility", MaterialIconKind.Factory),
                CreateNav<FactoringInvoicesPage>("FactoringInvoiceTab", "FactoringInvoice", MaterialIconKind.Hammer)).Item,
            CreateNav("MarketingTab", MaterialIconKind.Stocking,
                CreateNav<MarketerPage>("MarketerInfoTab", "Marketer", MaterialIconKind.PersonBadge), 
                CreateNav<MarketerDisbursementPermitsPage>("MarketerDisbursementPermitTab", "MarketerDisbursementPermit", MaterialIconKind.PackageDelivered)).Item,
            CreateNav("ManagementTab", MaterialIconKind.Administrator,
                CreateNav<UsersPage>("UsersTab", "User", MaterialIconKind.User),
                CreateNav<GroupsPage>("GroupTab", "Group", MaterialIconKind.Group)).Item,
            CreateNav("MiscTab", MaterialIconKind.StackOverflow,
                CreateNav<CompanyInfoPage>("AccountsConfigTab", "Config", MaterialIconKind.Account)).Item,
            CreateNav<ReportsNavigatorPage>("ReportsTab", "Reports", MaterialIconKind.DriveDocument).Item,
            CreateNav<FinancingOperationsPage>("FinancingOperationsTab", "Financial", MaterialIconKind.Money).Item,
            CreateNav<ThemingView>("SettingsTab", "Settings", MaterialIconKind.Settings).Item,
            CreateNav<GeneralReportPage>("GeneralReportsTab", "Reports", MaterialIconKind.Apple, true).Item,
        ];

        foreach (var item in items)
            _sideMenu.Items.Add(item);

        _sideMenu.UpdateMenuExpanded(PreferencesService.Get(s => s.IsMenuExpanded));
        _sideMenu.SelectionChanged += (_, args) =>
        {
            var selectedItem = args.AddedItems.OfType<SukiSideMenuItem>().FirstOrDefault();

            if (selectedItem is null)
                return;
            
            if (selectedItem.InnerContent is null)
            {
                // This is most likely a parent menu item.

                if (!_sideMenu.IsMenuExpanded)
                {
                    _sideMenu.UpdateMenuExpanded(true);
                    selectedItem.IsExpanded = true;
                    Task.Delay(300).ContinueWith(_ =>
                    {
                        Dispatcher.UIThread.Post(() =>
                        {
                            _sideMenu.ItemsPanelRoot?.BringIntoView();

                            selectedItem.BringIntoView();
                            (selectedItem.Items.LastOrDefault() as SukiSideMenuItem)?.BringIntoView();
                        }, DispatcherPriority.Background);
                    });
                    
                    _watchForNextSelectionToCollapseMenu = true;
                }
                
                return;
            }
            
            if (_watchForNextSelectionToCollapseMenu)
            {
                _sideMenu.UpdateMenuExpanded(false);
                _watchForNextSelectionToCollapseMenu = false;
            }
            
            var view = ContentHelper.GetViewByType(selectedItem.InnerContent.GetType());

            if (view is null)
            {
                Warning("No registered view was found while trying to navigate to this navigation item");
                return;
            }
            // TODO memory allocation issue
            tabVm.OnSynchronisationRequestedAsync(view.GetTabCardModel());

            _sideMenu.UnselectAll();
            /*if (selectedItem is null)
            {
                return;
            }

            if ((selectedItem.PageContent as UserControlBase)?.SynchronisationModel is not { } synchronisationModel)
            {
                Warning("Selected item does not have a synchronisation model in side menu selection changed event.");
                return;
            }

            await synchronisationModel.OnSynchronisationRequestedAsync(selectedItem.Header, selectedItem.Name);*/
        };

        tabVm.OnSynchronisationRequestedAsync(CodeBook.AddNew);
        ContentHelper.InvokeMenuItemsLoaded();
    }
    
    private NavigationViewItemWrapper CreateNav<TPage>(
        string name,
        string endpoint,
        MaterialIconKind symbol,
        bool isHiddenPage = false) where TPage: class
    {
        bool isVisible;
        
        if (name is "FinancingOperationsTab")
        {
            isVisible = Session.UserKeyContains("FinancialOperation");
        }
        else if (name is "ReportsTab")
        {
            isVisible = Session.UserKeyContains("StatementsNavigator");
        }
        else
        {
            isVisible = isHiddenPage ? false
                : Task.Run(() => Session.JudgeAccessAsync(endpoint, ShowPermission))
                    .GetAwaiter().GetResult();
        }
        
        var page = new NavigationViewItemWrapper
        {
            Symbol = symbol,
            PageType = typeof(TPage),
            Item = new SukiSideMenuItem
            {
                IsVisible = isVisible,
                Header = "•  " +  this[name],
                FontWeight = FontWeight.Normal,
                PageContent = App.GetService<TabbedNavigationalPage>(),
                InnerContent = App.GetService<TPage>(),
                Icon = new MaterialIcon
                {
                    Kind = symbol
                },
                Name = name,
            },
            CardModel = new()
            {
                CardName = this[name],
                CardSymbol = symbol,
                OnClick = () => PageNavigationService.RequestNavigation(typeof(TPage))
            }
        };
            
        ContentHelper.AddView(name, page);

        return page;
    }
    
    private NavigationViewItemWrapper CreateNav(string name,
        MaterialIconKind symbol,
        params NavigationViewItemWrapper[] children)
    {
        var ico = new MaterialIcon { Kind = symbol, Foreground = BackgroundTheme };
        ico.Bind(MaterialIcon.ForegroundProperty, new Binding("ViewModel.BackgroundTheme"));

        bool isVisible;
        
        isVisible = children.Any(c => c.Item.IsVisible);
        
        var nav = new SukiSideMenuItem
        {
            Header = this[name],
            Icon = ico,
            FontWeight = FontWeight.ExtraBold,
            IsVisible = isVisible,
            PageContent = App.GetService<TabbedNavigationalPage>()
        };
        
        nav.Tapped += (_, _) =>
        {
            nav.IsExpanded = true;
            
            /*var cardModels = children.Select(c =>
            {
                var model = new CardModel
                {
                    CardName = this[c.Item.Header],
                    CardSymbol = c.Symbol,
                    IsVisible = c.Item.IsVisible,
                    OnClick = delegate
                    {
                        PageNavigationService.RequestNavigation(c.PageType);
                    }
                };

                return model;
            }).Where(s => s.IsVisible);

            //nav.IsExpanded = !nav.IsExpanded;
            
            SynchroniseTypeBase<MedianPageViewModel>(cardModels, this[name]);*/
        };

        if (PreferencesService.Get(s => s.DroplistMenu))
        {
            foreach (var child in children)
            {
                nav.Items.Add(child.Item);
            }
        }

        return new NavigationViewItemWrapper
        {
            Item = nav
        };
    }

    // To make sure we navigate to these pages, even if they're not visible, because they should be
    // navigated to regardless of permissions.
    private static readonly List<string> _autoHiddenPages = [ "GeneralReportsTab" ];
}
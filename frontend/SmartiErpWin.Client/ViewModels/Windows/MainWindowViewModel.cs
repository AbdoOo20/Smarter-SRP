using System.Collections.ObjectModel;
using Fly.Framework.Shared.Common.Dtos;
using SmartErpWin.Core;
using SmartiErpWin.Client.DtoModels.UiModels;
using SmartiErpWin.Client.Helpers;
using SmartiErpWin.Client.UserInterface.Accounting.AccountingTree;
using SmartiErpWin.Client.UserInterface.Accounting.Assets;
using SmartiErpWin.Client.UserInterface.Accounting.Entries;
using SmartiErpWin.Client.UserInterface.Accounting.ExpenseTypes;
using SmartiErpWin.Client.UserInterface.Financial.FinancingOperations;
using SmartiErpWin.Client.UserInterface.Management.Groups;
using SmartiErpWin.Client.UserInterface.Management.Users;
using SmartiErpWin.Client.UserInterface.Marketing.Marketer;
using SmartiErpWin.Client.UserInterface.Marketing.MarketerEntry;
using SmartiErpWin.Client.UserInterface.Misc.AccountSettings;
using SmartiErpWin.Client.UserInterface.Misc.MedianPage;
using SmartiErpWin.Client.UserInterface.Personnel.Departments;
using SmartiErpWin.Client.UserInterface.Personnel.DiscountsAndIncreases;
using SmartiErpWin.Client.UserInterface.Personnel.EmployeeAssignments;
using SmartiErpWin.Client.UserInterface.Personnel.Employees;
using SmartiErpWin.Client.UserInterface.Purchases.PurchaseInvoices;
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
using SmartiErpWin.Client.UserInterface.Stores.Items;
using SmartiErpWin.Client.UserInterface.Stores.Stores;
using SmartiErpWin.Client.UserInterface.Stores.Suppliers;
using SmartiErpWin.Client.UserInterface.Stores.Units;
using SmartiErpWin.Client.Views.Pages;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace SmartiErpWin.Client.ViewModels.Windows;

public sealed partial class MainWindowViewModel : BaseRefreshableViewModel<DtoBase>
{
    private NavigationView? _navView;

    [ObservableProperty]
    private NavigationViewPaneDisplayMode _navigationViewPaneDisplay;

    [ObservableProperty]
    private double _navHeight;

    public MainWindowViewModel(LocalisationService locale,
        HttpClient client,
        SessionService sessionService) : base(client)
    {
        CheckTheme();

        InitBaseModel(locale, sessionService, null);
    }

    private static void CheckTheme()
    {
        var theme = Config.Get?.Theme == "dark" ? ApplicationTheme.Dark : ApplicationTheme.Light;
        ApplicationThemeManager.Apply(theme);
    }

    public void Navigate(Type? type)
    {
        if (type is null)
            return;

        _navView?.Navigate(type);
    }

    public void GoBack()
    {
        _navView?.GoBack();
    }

    public override void SetRefreshState(bool isRefreshing)
    {
        base.SetRefreshState(isRefreshing);
        NavHeight = IsRefreshing ? 0 : double.NaN;
    }

    public void ShowMenu(NavigationView navView)
    {
        _navView = navView;

        SetDisplayType();
        SetMenuItems();
        SetFooterItems();
    }

    public void AuthenticateRequestsForFeatureBase(string token)
    {
        AuthenticateRequests(token);
    }

    private void SetDisplayType()
    {
        try
        {
            var config = Config.Get;
            NavigationViewPaneDisplay = config.PaneDisplayType == "left"
                ? NavigationViewPaneDisplayMode.Left
                : NavigationViewPaneDisplayMode.Top;
        }
        catch
        {

        }
    }

    public void WindowSizeChanged(Size newSize)
    {
        if (_navView is null)
            return;

        _navView.IsPaneOpen = newSize.Width > 900;
    }

    private void SetFooterItems()
    {
        FooterMenuItems =
        [
            new NavigationViewItem
            {
                Content = LocaleService["SettingsTab"],
                Icon = new SymbolIcon { Symbol = SymbolRegular.Settings24 },
                TargetPageType = typeof(SettingsPage)
            }
        ];
    }

    private void SetMenuItems()
    {
        MenuItems = new ObservableCollection<object>
        {
            //  https://github.com/microsoft/fluentui-system-icons/blob/main/icons_regular.md

            CreateNav<DashboardPage>("HomeTab", "Home", SymbolRegular.Home12).Item,
            CreateNav("AccountingTab", SymbolRegular.DataHistogram24,
                CreateNav<AccountingTreePage>("AccountingTreeTab", "Account", SymbolRegular.TreeDeciduous20),
                CreateNav<ExpenseTypesPage>("ExpensesTab", "ExpenseTypes", SymbolRegular.Money16),
                CreateNav<EntryPage>("EntryTab", "Entry", SymbolRegular.Record12),
                CreateNav<AssetsPage>("AssetsTab", "Assets", SymbolRegular.People12)
            ),
            CreateNav("PersonnelTab", SymbolRegular.People48,
                CreateNav<EmployeesPage>("EmployeesTab", "Employee", SymbolRegular.Person32),
                CreateNav<AssignmentsPage>("AssignmentsTab", "EmployeeAssignment", SymbolRegular.TasksApp20),
                CreateNav<DepartmentsPage>("DepartmentsTab", "Department", SymbolRegular.NotebookSection20),
                CreateNav<DiscountIncreasePage>("DiscountIncreaseTab", "DiscountIncrease", SymbolRegular.Money16)
            ),
            CreateNav("StoresCategoriesTab", SymbolRegular.People48,
                CreateNav<CategoriesPage>("CategoriesTab", "Category", SymbolRegular.Diversity24),
                CreateNav<StoresPage>("StoresTab", "Store", SymbolRegular.StoreMicrosoft16),
                CreateNav<CostCentresPage>("CostCentresTab", "CostCenter", SymbolRegular.CenterHorizontal20),
                CreateNav<UnitsPage>("UnitsTab", "Unit", SymbolRegular.CurrencyDollarEuro16),
                CreateNav<SuppliersPage>("SuppliersTab", "Supplier", SymbolRegular.People24),
                CreateNav<ItemsPage>("ItemsTab", "Item", SymbolRegular.People24),
                CreateNav<CollectionPricesPage>("PriceSettingsTab", "Item", SymbolRegular.People24)
                //CreateNav<SupplyPermitsPage>("SupplyPermitsTab", "SupplyPermit", SymbolRegular.PersonSupport16),
                //CreateNav<DisbursementsPermitsPage>("DisbursementsPermitsTab", "DisbursementPermit",
                //    SymbolRegular.Receipt28)
            ),
            CreateNav("SalesTab", SymbolRegular.People48,
                CreateNav<SalesInvoicePage>("SalesInvoiceTab", "Sales", SymbolRegular.Storage16),
                CreateNav<SellReturnInvoicePage>("SellReturnInvoiceTab", "SellReturnInvoice", SymbolRegular.Storage16),
                CreateNav<QuantityCreditingPage>("QuantityCreditingTab", "QuantityCrediting", SymbolRegular.WalletCreditCard16),
                CreateNav<VaultsPage>("VaultsTab", "Safe", SymbolRegular.Storage16),
                CreateNav<BanksPage>("BankTab", "Bank", SymbolRegular.BuildingBank16),
                CreateNav<BankBranchesPage>("BankBranchTab", "BankBranch", SymbolRegular.BuildingBankLink16),
                CreateNav<ClientsPage>("ClientsTab", "Client", SymbolRegular.BuildingBankLink16),
                CreateNav<SalesCheckoutEntryWindow>("SalesCheckoutTab", "FastSales",SymbolRegular.BuildingBankLink16)
                //    SymbolRegular.ShoppingBagArrowLeft20)
            ),
            CreateNav("PurchasesTab", SymbolRegular.People48,
                CreateNav<PurchasesInvoicePage>("PurchasesInvoicesTab", "Purchase", SymbolRegular.Storage16),
                CreateNav<PurchaseReturnInvoicePage>("PurchasesReturnInvoicesTab", "PurchaseReturn", SymbolRegular.ArrowAutofitContent24)
            ),
            CreateNav("MarketingTab", SymbolRegular.People48,
                CreateNav<MarketerInfoPage>("MarketerInfoTab", "Marketer", SymbolRegular.Record48),
                CreateNav<MarketerDisbursementPermitPage>("MarketerDisbursementPermitTab", "MarketerDisbursementPermit",
                    SymbolRegular.Record48)
            ),
            CreateNav<FinancingOperationsPage>("FinancingOperationsTab", "Financial", SymbolRegular.MoneyHand24).Item,
            CreateNav<StatementsNavigatorPage>("StatementsNavigator", "StatementsNavigator", SymbolRegular.People48).Item,
            //CreateNav<StatementsLookupPage>("StatementsLookupTab", "StatementsLookup", SymbolRegular.MoneyHand24),
            CreateNav("MiscTab", SymbolRegular.People48,
                CreateNav<AccountSettingsPage>("AccountsConfigTab", "Config", SymbolRegular.Record48)
            ),
            CreateNav("ManagementTab", SymbolRegular.KeyCommand16,
                CreateNav<GroupPage>("GroupTab", "Group", SymbolRegular.Group20),
                CreateNav<UserPage>("UsersTab", "User", SymbolRegular.People12)),
                new NavigationViewItem
                {
                    Visibility = Visibility.Hidden,
                    Content = LocaleService["GeneralReport"],
                    TargetPageType = typeof(GeneralReportPage)
                },
                new NavigationViewItem
                {
                    Visibility = Visibility.Hidden,
                    Content = LocaleService["AccountStatements"],
                    TargetPageType = typeof(AccountStatementReportPage)
                }
            };
    }

    [ObservableProperty]
    private ObservableCollection<object>? _menuItems;

    [ObservableProperty]
    private ObservableCollection<object>? _footerMenuItems;

    [ObservableProperty]
    private ObservableCollection<MenuItem>? _trayMenuItems = new()
    {
        new MenuItem { Header = "Home", Tag = "tray_home" }
    };

    public override async Task OnSynchronisationRequestedAsync(params object[] args)
    {
        SetDisplayType();
        await base.OnSynchronisationRequestedAsync(args);
    }

    private NavigationViewItemWrapper CreateNav<TPage>(
        string name,
        string endpoint,
        SymbolRegular symbol)
    {
        return new NavigationViewItemWrapper
        {
            Item = new NavigationViewItem(LocaleService?[name] ?? name, symbol, typeof(TPage))
            {
                Visibility = Task.Run(() => Session.JudgeAccessAsync(endpoint, ShowPermission))
                    .GetAwaiter().GetResult()
                    ? Visibility.Visible
                    : Visibility.Collapsed
            },
            Symbol = symbol
        };
    }

    private NavigationViewItem CreateNav(string name,
        SymbolRegular symbol,
        params NavigationViewItemWrapper[] children)
    {
        var nav = new NavigationViewItem
        {
            Content = LocaleService?[name],
            Icon = new SymbolIcon { Symbol = symbol },
            Visibility = children.Any(c => c.Item.Visibility is Visibility.Visible).ApplyVisibility(),
            TargetPageType = typeof(MedianPage)
        };

        nav.Click += (_, _) =>
        {
            var cardModels = children.Select(c =>
            {
                var model = new CardModel
                {
                    CardName = c.Item.Content.ToString(),
                    CardSymbol = c.Symbol,
                    IsVisible = c.Item.Visibility,
                    OnClick = delegate
                    {
                        Navigate(c.Item.TargetPageType);
                    }
                };

                return model;
            });

            SynchroniseTypeBase<MedianPageViewModel>(cardModels);
        };

        if (Config.Get.DroplistMenu)
        {
            foreach (var child in children)
            {
                nav.MenuItems.Add(child.Item);
            }
        }

        return nav;
    }
}

public class NavigationViewItemWrapper
{
    public required NavigationViewItem Item { get; set; }

    public CardModel? CardModel { get; set; }

    public SymbolRegular Symbol { get; set; }
}
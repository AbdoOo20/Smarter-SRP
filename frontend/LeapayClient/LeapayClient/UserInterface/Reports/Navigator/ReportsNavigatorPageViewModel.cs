using System.Collections.ObjectModel;
using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Dtos;
using Fly.Shared.Modules.Accounting.Enums;
using Fly.Shared.Modules.Financial.Enums;
using Fly.Shared.Modules.Financial.Requests;
using Leapay.Client.ClientModels.Dto.DtoModels.UiModels;
using Leapay.Client.Helpers;
using Leapay.Client.Options;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.Services;
using Leapay.Client.UserInterface.Factoring.FactoringInvoice;
using Leapay.Client.UserInterface.Maintenance.MaintenanceInvoice;
using Leapay.Client.UserInterface.Purchases.PurchaseReturnInvoice;
using Leapay.Client.UserInterface.Purchases.PurchasesInvoice;
using Leapay.Client.UserInterface.Reports.GeneralReport;
using Leapay.Client.UserInterface.Sales.SalesInvoice;
using Leapay.Client.UserInterface.Sales.SalesReturnInvoice;
using Material.Icons.Avalonia;
using SmartErpWin.Core;
using SmartErpWin.Core.Enums;
using SmartErpWin.Core.Extensions;

namespace Leapay.Client.UserInterface.Reports.Navigator;

public partial class ReportsNavigatorPageViewModel : BasePageViewModel<DtoBase>
{
    private readonly MainWindowViewModel _mainWindowViewModel;

    [ObservableProperty]
    public partial ObservableCollection<ExpanderModel> Expanders { get; set; } = [];

    public ReportsNavigatorPageViewModel(MainWindowViewModel mainWindowViewModel)
    {
        _mainWindowViewModel = mainWindowViewModel;
        
        LoadInterface();
    }
    
        private void LoadInterface()
    {
        BulkCreateExpanders(
        [
            Expander("ACCOUNTS",
            // ------- ACCOUNTS -------
                //  001 -   التقرير لشامل
                NewCard("AccountMovements", new StatementQueryRequest(){
                    //DynamicCombos = [
                    //    new ("accounts", "accounts", "accountid", true),
                    //    ]
                } , "accounts", accountType: null,
                buttons:
                [
                    new ButtonColumnSingular("InvoiceInfo", this["InvoiceDetails"], (a, olderVm) =>
                    {
                        if (a is not DtoNamesBase namesBase || olderVm is not GeneralReportPageViewModel oldVm)
                            return;
                        
                        var type = namesBase.Description;
                        if(!Enum.TryParse(type, out OprationTypeEnum enumType)
                           || !int.TryParse(namesBase.Name, out var invoiceNumber))
                            return;
                        
                        var endpoint = "";
                        UserControlBase toShow;
                        
                        switch (enumType)
                        {
                            case OprationTypeEnum.Sales:
                            {
                                endpoint = "Sales";
                                var vm = new SalesInvoiceEntryViewModel(null, EntryType.Edit,
                                    typeof(SalesInvoicePageViewModel), invoiceNumber);
                                toShow = new SalesInvoiceEntryWindow(vm);
                                break;
                            }
                            case OprationTypeEnum.Purchases:
                            {
                                endpoint = "Purchase";
                                var vm = new PurchaseInvoiceEntryViewModel(null, EntryType.Edit,
                                    typeof(PurchaseInvoicesPageViewModel), invoiceNumber);
                                toShow = new PurchaseInvoiceEntryWindow(vm);
                                break;
                            }
                            case OprationTypeEnum.Maintenance:
                            {
                                endpoint = "Maintenance";
                                var vm = new MaintenanceInvoiceEntryViewModel(null, EntryType.Edit,
                                    typeof(MaintenanceInvoicesPageViewModel), invoiceNumber);
                                toShow = new MaintenanceInvoiceEntryWindow(vm);
                                break;
                            }
                            case OprationTypeEnum.Factoring:
                            {
                                endpoint = "FactoringInvoice";
                                var vm = new FactoringInvoiceEntryViewModel(null, EntryType.Edit,
                                    typeof(FactoringInvoicesPageViewModel), invoiceNumber);
                                toShow = new FactoringInvoiceEntryWindow(vm);
                                break;
                            }
                            case OprationTypeEnum.Sales_Return:
                            {
                                endpoint = "SellReturnInvoice";
                                var vm = new SellReturnInvoiceEntryViewModel(null, EntryType.Edit,
                                    typeof(SellReturnInvoicesPageViewModel), invoiceNumber);
                                toShow = new SellReturnInvoiceEntryWindow(vm);
                                break;
                            }
                            case OprationTypeEnum.Purchases_Return:
                            {
                                endpoint = "PurchaseReturnInvoice";
                                var vm = new PurchaseReturnInvoiceEntryViewModel(null, EntryType.Edit,
                                    typeof(PurchaseReturnInvoicesPageViewModel), invoiceNumber);
                                toShow = new PurchaseReturnInvoiceEntryWindow(vm);
                                break;
                            }
                            default:
                            {
                                true.Stop("NoInvoice", "ThisItemHasNoSupportedInvoice");
                                return;
                            }
                        }

                        if ((!Session.JudgeSpecificAccess($"{endpoint}.Update") || !Session.JudgeSpecificAccess($"{endpoint}.Show"))
                            .Stop("NoPermissions", "NoPermissionsToSeeThisInvoice"))
                            return;

                        _ = ShowContentAsync(toShow, EntryType.Edit,
                            options: new WindowOptions(this["ViewInvoice"] + " - " + this[endpoint],
                            IsDialogue: false, CanResize: false,
                            CanMaximize: true, CanMinimize: true,
                            IsMaximized: true));
                        
                    }, () => new MaterialIcon { Kind = MaterialIconKind.Eye }, IncrementForName: 2)
                ]),

                //*************
                //  002 -   ديون العملاء
                NewCard("TheClientDebtion", new(){
                    DynamicCombos = [
                        new ("العملاء", "Representatives", "RepresentativeId", true),
                    ],
                }, null, accountType: StatementAccountType.Clients, true),
                //  003 -   نسبة التحصيل من العملاء
                NewCard("TheClientDebtionDetails", new(){
                    DynamicCombos = [
                        new  ("العملاء", "Representatives", "RepresentativeId", true),
                    ],
                }, null, accountType: StatementAccountType.Clients, true),
                //  004  -   ديون متأخرة | عملاء
                NewCard("ClientsOverdueDebts", new StatementQueryRequest(){
                     DynamicFields = [
                        new ("الأيام", "days")
                    ]
                }, null, accountType: StatementAccountType.Clients),
                //*************
                //  005 -   ديون الموردين
                NewCard("TheCSupplierDebtion", new(), "accounts", accountType: StatementAccountType.Suppliers, false),
                //*************
                //  006 -   ديون المندوبين
                NewCard("TheRepresentativeDebtionDetails", new(), "Representatives", accountType: StatementAccountType.Suppliers, false),
                //*************
                //  006 -  مقبوضات مندوب
                NewCard("MarketerCredtions", new(), "Representatives", accountType: StatementAccountType.Suppliers, true),
                // ---------------------
                //*************
                //  007 -  حركة المصروفات
                NewCard("ExpencesHestory", new(), null, accountType: StatementAccountType.Suppliers, true)
                // ---------------------
            ),

            Expander("Items",
                // ------- ITEMS -------
                //  005 -   أرصدة المخازن
                NewCard("ItemsStoragesQuantities", new(){
                    DynamicCombos = [
                        new ("المخزن", "StoragesAndMarketers", "storageId", true),
                        new ("الفئة", "Categories", "CategoryId", true),
                    ],
                }, "items", accountType: StatementAccountType.Safes),
                NewCard("DamageItems", new(){
                    DynamicCombos = [
                        new ("المخزن", "StoragesAndMarketers", "storageId", true),
                        new ("الفئة", "Categories", "CategoryId", true),
                    ],
                }, "items", accountType: StatementAccountType.Safes),
                NewCard("MaintenanceInvoicesByStatus", new() {
                    DynamicCombos = [
                        new ("العملاء", "Clients", "ClientId", true),
                        new ("حالة الدفع", "MaintenanceStatuses", "StatusId"),
                        new ("حالة الأصناف", "MaintenanceItemsStatuses", "ItemsStatusId"),
                    ],
                }, null, accountType: StatementAccountType.Clients),
                NewCard("FactoringInvoicesByStatus", new() {
                    DynamicCombos = [
                        new ("العملاء", "Clients", "ClientId", true),
                        new ("حالة الدفع", "FactoringStatuses", "ItemsStatusId"),
                        new ("حالة الأصناف", "FactoringItemsStatuses", "ItemsStatusId"),
                    ],
                }, null, accountType: StatementAccountType.Clients),
                //  005 -   أرصدة المسوقين
                NewCard("ItemsMarkersQuantities", new(){
                    DynamicCombos = [
                        new ("المسوقين", "Representatives", "RepresentativeId", true),
                    ]
                }, null, StatementAccountType.Marketers),

                //  007 -   حركة الأصناف
                NewCard("ItemsMovements", new(){
                    DynamicCombos = [
                        new ("المخزن", "StoragesAndMarketers", "storageId", true),
                    ]
                }, "items", accountType: StatementAccountType.Clients,
                    buttons:
                    [
                        new ButtonColumnSingular("ItemId", this["DetailsString"], (a, olderVm) =>
                        {
                            if (a is not DtoNamesBase namesBase ||
                                !Identifier.TryParse(namesBase.Description, null, out var id) ||
                                olderVm is not GeneralReportPageViewModel oldVm)
                                return;
                            var vm = new GeneralReportPageViewModel(_mainWindowViewModel)
                            {
                                IsBackVisible = false
                            };
                            
                            var page = new GeneralReportPage(vm);
                            var rq = oldVm.QueryRequest.DeepCopy() ?? new();
                            rq.AccountId = id;
                            
                            vm.SetNewAsync(rq, "ItemsMovements",
                                accountType: StatementAccountType.Clients);
                            vm.Title = this["ItemsMovements"] + " - " + namesBase.Name;

                            _ = ContentHelper.ShowCrossPlatformContentAsync(page,
                                new WindowOptions(this["ItemsMovements"], true, true, true, false, false));
                        }, () => new MaterialIcon { Kind = MaterialIconKind.Eye }, IncrementForName: 3)
                    ]),

                //  007-1 -   حركة الأصناف مع المندوبين
                NewCard("RepresentativeItemsMovements", new StatementQueryRequest {
                    DynamicCombos = [
                        new ("المندوبين", "Representatives", "RepresentativeId", IsSearchable: false),
                    ],
                }, "items", accountType: StatementAccountType.Clients)
                // ---------------------
                // 009  -   أرباح مبيعات 2
                , NewCard("SalesItemsProfit3", new StatementQueryRequest(), "items", accountType: StatementAccountType.Clients)
            ),

            Expander("OTHERS",

                // ------- OTHERS ------
                //  008 -   الأيصالات الضائعة
                NewCard("MissedSerial", new StatementQueryRequest(){
                     DynamicFields = [
                        new ("أول رقم", "StartPoint"),
                        new ("عدد اوراق الدفتر", "EndPoint", 50)
                    ],DynamicCombos = [
                        new ("النوع", "LostTypes", "LostTypeId"),
                    ],

                }, "Accounts", accountType: StatementAccountType.Marketers),
                // 009  -   أرباح مبيعات 2
                NewCard("SalesItemsProfit2", new StatementQueryRequest(), "items", accountType: StatementAccountType.Clients),
                //  012 -   تفاصيل المبيعات
                NewCard("SalesCollectionInsights", new StatementQueryRequest(), null, accountType: StatementAccountType.All),

                // 
                //  010 -   عدد مرات تخفيض صنف
                NewCard("MaxDiscountItemsBased", new StatementQueryRequest(), null, accountType: StatementAccountType.All),
                //  011 -   الأصناف المُخفضة
                NewCard("DiscountItemsBased", new StatementQueryRequest(), "items", accountType: StatementAccountType.All),

                //  012 -   ارصدة المندوبين
                NewCard("RepresentativeAmounts", new StatementQueryRequest(), null, accountType: StatementAccountType.Marketers, false),

                //  012 -   ارصدة المندوبين
                NewCard("FinancialHestory", new StatementQueryRequest(), null, accountType: StatementAccountType.Marketers)
            ),
        ]);
    }

    private void BulkCreateExpanders(params ExpanderModel[] expanders)
    {
        expanders.Observe(Expanders);
    }

    private ExpanderModel Expander(string name, params CardModel[] cards)
    {
        var expander = new ExpanderModel
        {
            Name = this[name]
        };

        cards.Observe(expander.Cards);

        expander.Visibility = expander.Cards
            .Any(c => c.IsVisible)
            .ApplyVisibility();

        return expander;
    }

    private CardModel NewCard(
        string name,
        StatementQueryRequest request,
        string? customComboEndpoint = "",
        StatementAccountType? accountType = null,
        bool areDatesVisible = true,
        MaterialIconKind symbol = MaterialIconKind.Report,
        List<ButtonColumnSingular> buttons = null)
    {
        const string parent = "StatementsNavigator.";
        const string action = "-Show";
        var isVisible = Session.JudgeSpecificAccess(parent + name + action);

        return new CardModel
        {
            CardName = name,
            CardSymbol = symbol,
            IsVisible = isVisible,
            OnClick = delegate
            {
                OpenNewQuery(async vm => vm.SetNewAsync(
                    await CommonHelpers.DeepCopyValueOfReferenceTypeAsync(request),
                    name,
                    customComboEndpoint,
                    accountType,
                    areDatesVisible,
                    buttons: buttons), name, isVisible);
            }
        };
    }

    private void OpenNewQuery(Action<GeneralReportPageViewModel> ac, string name, bool isVisible)
    {
        var vm = new GeneralReportPageViewModel(_mainWindowViewModel);
        ac.Invoke(vm);

        _mainWindowViewModel.PageNavigationService.RequestStandaloneNavigation(new TabbedViewCardModel
        {
            ViewName = name,
            View = new GeneralReportPage(vm),
            CardGroup = "GeneralReports",
            CardName = this[name],
            IsVisible = isVisible
        });
    }
}
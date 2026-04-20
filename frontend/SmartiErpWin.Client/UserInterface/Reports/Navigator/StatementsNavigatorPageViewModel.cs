using System.Collections.ObjectModel;
using Fly.Framework.Shared.Common.Dtos;
using Fly.Shared.Modules.Financial.Enums;
using Fly.Shared.Modules.Financial.Requests;
using SmartErpWin.Core.Extensions;
using SmartiErpWin.Client.DtoModels.UiModels;
using SmartiErpWin.Client.Helpers;
using SmartiErpWin.Client.UserInterface.Reports.GeneralReport;
using SmartiErpWin.Client.ViewModels.Windows;
using SmartErpWin.Core;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace SmartiErpWin.Client.UserInterface.Reports.Navigator;

public partial class StatementsNavigatorPageViewModel : BaseViewModel<DtoBase>
{
    private readonly MainWindowViewModel _mainViewModel;

    [ObservableProperty]
    private ObservableCollection<ExpanderModel> _expanders = [];

    public StatementsNavigatorPageViewModel(HttpClient client,
        IContentDialogService contentDialogService,
        SessionService sessionService,
        LocalisationService localisationService,
        MainWindowViewModel mainViewModel) : base(client)
    {
        _mainViewModel = mainViewModel;
        InitBaseModel(localisationService, sessionService, contentDialogService);

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
                } , "accounts", accountType: null),

                //*************
                //  002 -   ديون العملاء
                NewCard("TheClientDebtion", new(){
                    DynamicCombos = [
                        new ("العملاء", "Representatives", "RepresentativeId", true),
                    ],
                }, "accounts", accountType: StatementAccountType.Clients, false),
                //  003 -   نسبة التحصيل من العملاء
                NewCard("TheClientDebtionDetails", new(){
                    DynamicCombos = [
                        new    ("العملاء", "Representatives", "RepresentativeId", true),
                    ],
                }, "accounts", accountType: StatementAccountType.Clients, false),
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
                        new ("الفئة", "Categories", "CategoryId", true),
                        //new ("العرض", "Views", "ViewId", true),
                        //new ("المسوقين", "Representatives", "RepresentativeId", true),
                    ],
                }, "items", accountType: StatementAccountType.Safes),

                //  005 -   أرصدة المسوقين
                NewCard("ItemsMarkersQuantities", new(){
                    DynamicCombos = [
                        new ("المسوقين", "Representatives", "RepresentativeId", true),
                    ]
                }, null, StatementAccountType.Marketers),

                //  007 -   حركة الأصناف
                NewCard("ItemsMovements", new(){
                    DynamicCombos = [
                        new ("المخازن", "StoragesAndMarketers", "storageId", true),
                        //new ("المندوبين", "Representatives", "RepresentativeId", true),
                    ]
                }, "items", accountType: StatementAccountType.Clients),

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
            .Any(c => c.IsVisible is Visibility.Visible)
            .ApplyVisibility();

        return expander;
    }

    private CardModel NewCard(
        string name,
        StatementQueryRequest request,
        string? customComboEndpoint = "",
        StatementAccountType? accountType = null,
        bool areDatesVisible = true,
        SymbolRegular symbol = SymbolRegular.CardUi20)
        => new()
        {
            CardName = name,
            CardSymbol = symbol,
            IsVisible = Visibility.Visible,
            /*(App
                .GetService<SessionService>()?
                .JudgeAccessAsync("report", $"{name}.Show report")
                .GetAwaiter()
                .GetResult())
                .GetValueOrDefault()
                .ApplyVisibility(),*/
            OnClick = delegate
            {
                OpenNewQuery(async vm => vm.SetNewAsync(
                    await CommonHelpers.DeepCopyValueOfReferenceTypeAsync(request),
                    name,
                    customComboEndpoint,
                    accountType,
                    areDatesVisible));
            }
        };

    private void OpenNewQuery(Action<GeneralReportPageViewModel> ac)
    {
        var vm = App.GetService<GeneralReportPageViewModel>();
        ac.Invoke(vm);

        _mainViewModel.Navigate(typeof(GeneralReportPage));
    }
}
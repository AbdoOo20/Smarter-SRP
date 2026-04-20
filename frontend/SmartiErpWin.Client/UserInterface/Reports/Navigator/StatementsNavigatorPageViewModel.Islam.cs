using Fly.Shared.Modules.Financial.Enums;
using Fly.Shared.Modules.Financial.Requests;

namespace SmartiErpWin.Client.UserInterface.Reports.Navigator;

public partial class StatementsNavigatorPageViewModel
{

    #region not-used

    [RelayCommand]
    private void OnNavigateToAccountMovementsReport()
    {
        OpenNewQuery(vm => vm.SetNewAsync(new StatementQueryRequest
            {
                DynamicFields =
                [
                    new("AccountName", "AccountQuery" ),
                    new("AccountDate", "AccountDate")
                ]
            },
            "AccountMovements",
            "accounts",
            accountType: null));
    }

    [RelayCommand]
    private void OnNavigateToSaleComplexPercentReport()
    {
        OpenNewQuery(vm => vm.SetNewAsync(
            new StatementQueryRequest(),
            "SaleComplexPercentReport",
            "accounts",
            accountType: StatementAccountType.Clients));
    }

    [RelayCommand]
    private void OnNavigateToSaleComplexReport()
    {
        OpenNewQuery(vm => vm.SetNewAsync(
            new StatementQueryRequest(),
            "SaleComplexReport",
            "accounts",
            accountType: StatementAccountType.Clients));
    }

    [RelayCommand]
    private void OnNavigateToSaleReport()
    {
        OpenNewQuery(vm => vm.SetNewAsync(
            new StatementQueryRequest(),
            "SaleReport",
            "accounts",
            accountType: StatementAccountType.Clients));
    }

    #endregion


    //  002 -   التقرير لشامل
    [RelayCommand]
    private void OnNavigateToTestReport()
    {
        OpenNewQuery(vm => vm.SetNewAsync(new StatementQueryRequest(),
            "AccountMovements",
            "Accounts",
            accountType: null, width: 100));
    }


    //  003 -   ديون العملاء
    [RelayCommand]
    private void OnNavigateToTheClientDebtionReport()
    {
        OpenNewQuery(vm => vm.SetNewAsync(
            new StatementQueryRequest()
            {
                //DynamicFields = [
                //    new ("ClientId", "qqq")
                //]
            },
            "TheClientDebtion",
            "accounts",
            accountType: StatementAccountType.Clients, false));
    }


    //  004 -   ديون الموردين
    [RelayCommand]
    private void OnNavigateToTheSupplierDebtionReport()
    {
        OpenNewQuery(vm => vm.SetNewAsync(
            new StatementQueryRequest(),
            "TheCSupplierDebtion",
            "accounts",
            accountType: StatementAccountType.Suppliers, false));
    }


    //  004 -   ديون المندوبين
    [RelayCommand]
    private void OnNavigateTTheRepresentativeDebtionDetailsReport()
    {
        OpenNewQuery(vm => vm.SetNewAsync(
            new StatementQueryRequest(),
            "TheRepresentativeDebtionDetails",
            "Representatives",
            accountType: StatementAccountType.Suppliers, false));
    }


    //  005 -   أرصدة المخازن
    [RelayCommand]
    private void OnNavigateToItemReport()
    {
        OpenNewQuery(vm => vm.SetNewAsync(new StatementQueryRequest(),
            "ItemsStoragesQuantities",
            "items",
            accountType: StatementAccountType.All));
    }


    //  006 -   نسبة التحصيل من العملاء
    [RelayCommand]
    private void OnNavigateToTheClientDebtionDetailsReport()
    {
        OpenNewQuery(vm => vm.SetNewAsync(new StatementQueryRequest(),
            "TheClientDebtionDetails",
            "accounts",
            accountType: StatementAccountType.Clients, false));
    }


    //  007 -   حركة الأصناف
    [RelayCommand]
    private void OnNavigateToItemsMovementsReport()
    {
        //var isSupportRepresentatives = true;
        //OpenNewQuery(vm => vm.SetNewAsync(new StatementQueryRequest(),
        //"ItemsMovements",
        //"items",
        //accountType: StatementAccountType.Clients));
        var isSupportRepresentatives = true;
        OpenNewQuery(vm => vm.SetNewAsync(new StatementQueryRequest()
                { },
            "ItemsMovements",
            "items",
            accountType: StatementAccountType.Clients));
    }

    //  007-1 -   حركة الأصناف
    [RelayCommand]
    private void OnNavigateToItemsMovementsWithRepresentativesReport()
    {
        var isSupportRepresentatives = true;
        OpenNewQuery(vm => vm.SetNewAsync(new StatementQueryRequest()
            {
                DynamicCombos = [
                    new ("المندوبين", "Representatives", "RepresentativeId", ComboWidth: 500),
                ],
            },
            "RepresentativeItemsMovements",
            "items",
            accountType: StatementAccountType.Clients));
    }


    //  008 -   الأيصالات الضائعة
    [RelayCommand]
    private void NavigateToMissedSerialReport()
    {
        OpenNewQuery(vm => vm.SetNewAsync(new StatementQueryRequest()
            {
                DynamicFields = [
                    new ("أول رقم", "StartPoint"),
                    new ("عدد اوراق الدفتر", "EndPoint", 50)
                ]
            },
            "MissedSerial",
            "Accounts",
            accountType: StatementAccountType.Marketers));
    }


    // 009  -   أرباح مبيعات 2
    [RelayCommand]
    private void NavigateToSalesItemsProfit2Report()
    {
        OpenNewQuery(vm => vm.SetNewAsync(new StatementQueryRequest(),
            "SalesItemsProfit2",
            "items",
            accountType: StatementAccountType.Clients));
    }


    //  010 -   عدد مرات تخفيض صنف
    [RelayCommand]
    private void NavigateToMaxDiscountItemsBasedReport()
    {
        OpenNewQuery(vm => vm.SetNewAsync(new StatementQueryRequest(),
            "MaxDiscountItemsBased",
            accountType: StatementAccountType.All));
    }

        
    //  011 -   الأصناف المُخفضة
    [RelayCommand]
    private void NavigateToDiscountItemsBasedReport()
    {
        OpenNewQuery(vm => vm.SetNewAsync(new StatementQueryRequest()
            {
                //DynamicFields = [
                //    new ("الأيام", "DaysNumber")
                //]
            },
            "DiscountItemsBased",
            "items",
            accountType: StatementAccountType.All));
    }


    //  012 -   تفاصيل المبيعات
    [RelayCommand]
    private void NavigateToSalesCollectionInsightsReport()
    {
        OpenNewQuery(vm => vm.SetNewAsync(new StatementQueryRequest(),
            "SalesCollectionInsights",
            accountType: StatementAccountType.All));
    }


    // 013  -   ديون متأخرة | عملاء
    [RelayCommand]
    private void OnNavigateToClientsOverdueDebtsReport()
    {
        OpenNewQuery(vm => vm.SetNewAsync(new StatementQueryRequest()
            {
                DynamicFields = [
                    new ("الأيام", "days")
                ]
            },
            "ClientsOverdueDebts",
            accountType: StatementAccountType.Clients));
    }
        

    // 013  -   ديون متأخرة | عملاء
    [RelayCommand]
    private void OnNavigateToRepresentativeAmountsReport()
    {
        OpenNewQuery(vm => vm.SetNewAsync(new StatementQueryRequest()
            {
            },
            "RepresentativeAmounts",
            accountType: StatementAccountType.Marketers, areDatesVisible: false));
    }

}
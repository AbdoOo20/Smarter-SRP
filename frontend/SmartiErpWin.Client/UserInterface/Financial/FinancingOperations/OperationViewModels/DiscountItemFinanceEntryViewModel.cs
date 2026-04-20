using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.Financial.Requests;
using Fly.Shared.Modules.PurchaseAndSales.Requests;
using SmartiErpWin.Client.Reports;
using SmartiErpWin.Client.UserInterface.Financial.FinancingOperations.ModelBase;

namespace SmartiErpWin.Client.UserInterface.Financial.FinancingOperations.OperationViewModels;

public partial class DiscountItemFinanceEntryViewModel(
    Window window,
    LocalisationService locale,
    DiscountItemBasedRequest? model,
    EntryType entryType,
    HttpClient client,
    Type pageType)
    : FinanceOperationViewModelBase<DiscountItemBasedRequest>(
        window,
        locale,
        model,
        entryType,
        client,
        pageType,
        "Marketers",
        "DiscountItemPaymentReceipt")
{

    [ObservableProperty]
    private Identifier _clientPercentage;

    protected override OperationType? FinanceType => OperationType.Withdraw;

    protected override async Task PrepareAdditionalAsync()
    {
        Model.Date = DateTime.Now;
        Model.ExpireDate = DateTime.Now;
        await PrepareComboBoxAsync("Items", Model.ItemId, 2);
        //await PrepareComboBoxAsync("Units", Model.ItemId, 3);
    }

    protected override void PreSaveActions()
    {
        base.PreSaveActions();

        Model.ItemId = ComboModelBase.GetSelectedItemId(2);
        Model.ClientId = ComboModelBase.GetSelectedItemId(5);
        Model.UnitId = ComboModelBase.GetSelectedItemId(3);
    }

    public async void SetUnits()
    {
        var selectedItem = ComboModelBase.GetSelectedItemId(2);

        if (selectedItem == Identifier.Empty)
            return;
        
        await RequestApiComboItemsFromInnerEndpointAsync("Units", 3, selectedItem.ToString());
        ComboModelBase.SetComboModelOrDefault(Model?.UnitId, 3);
    }
    
    protected override void LoadReport(bool isDeposit, string? others, string? data = null)
    {
        ReportRunner.PrintDiscountItem(isDeposit, this);
        base.LoadReport(isDeposit, others, data);
    }

    public async void SetMarketerInfo()
    {
        var id = ComboModelBase.GetSelectedItemId();

        if (id == Identifier.Empty)
        {
            ComboModelBase.Clear(5);
            return;
        }


        await RequestCombosUsingQueryRequestAsync("Clients", 5, new ClientQueryRequest
        {
            MarketerId = id
        });
        ComboModelBase.SetComboModelOrDefault(null, 5);
    }
}
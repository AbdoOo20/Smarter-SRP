using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.ValueTypes;
using Fly.Shared.Modules.Financial.Requests;
using Fly.Shared.Modules.PurchaseAndSales.Requests;
using SmartiErpWin.Client.Reports;
using SmartiErpWin.Client.UserInterface.Financial.FinancingOperations.ModelBase;

namespace SmartiErpWin.Client.UserInterface.Financial.FinancingOperations.OperationViewModels;

public sealed partial class ClientRepresentativeReceiptEntryViewModel(
    Window window,
    LocalisationService locale,
    RepresentativeFinancialRequest? model,
    EntryType entryType,
    HttpClient client,
    Type pageType)
    : FinanceOperationViewModelBase<RepresentativeFinancialRequest>(
        window,
        locale,
        model,
        entryType,
        client,
        pageType,
        "Marketers",
        "RepresentativePaymentReceipt")
{
    [ObservableProperty]
    private decimal _clientPercentage;
    
    protected override OperationType? FinanceType => OperationType.Withdraw;

    protected override void LoadReport(bool isDeposit, string? others, string? data = null)
    {
        ReportRunner.PrintClientRepresentativeReceipt(isDeposit, this);
        //base.LoadReport(isDeposit, others, data);
    }

    protected override async Task PrepareAdditionalAsync()
    {
        Model.Date = DateTime.Now;
    }

    protected override void PreSaveActions()
    {
        base.PreSaveActions();

        Model.MarketerId = ComboModelBase.GetSelectedItemId();
        Model.SafeId = ComboModelBase.GetSelectedItemId(1);
        Model.ClientId = ComboModelBase.GetSelectedItemId(2);
        Model.MarketerShare = ClientPercentage;
    }

    public async void SetMarketerInfo()
    {
        var id = ComboModelBase.GetSelectedItemId();

        if (id == Identifier.Empty)
            return;
        
        ClientPercentage = (await RequestOneUsingQueryRequestAsync<TypeOf<decimal>>("MarketerPercentage", 
            new StatementQueryRequest
            {
                AccountId = id
            })).Data!.Value;

        await RequestCombosUsingQueryRequestAsync("Clients", 2, new ClientQueryRequest
        {
            MarketerId = id,
        });
        
        ComboModelBase.SetComboModelOrDefault(null, 2);
    }
}
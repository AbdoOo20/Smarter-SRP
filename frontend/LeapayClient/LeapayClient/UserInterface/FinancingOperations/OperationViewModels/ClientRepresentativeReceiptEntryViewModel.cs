using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.ValueTypes;
using Fly.Shared.Modules.Financial.Requests;
using Fly.Shared.Modules.PurchaseAndSales.Requests;
using Leapay.Client.ClientModels.Dto.DtoModels;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.Reports;
using SmartErpWin.Core.Enums;

namespace Leapay.Client.UserInterface.FinancingOperations.OperationViewModels;

public sealed partial class ClientRepresentativeReceiptEntryViewModel : ModelBase.FinanceOperationViewModelBase<RepresentativeFinancialRequest>
{
    [ObservableProperty]
    public partial SearchableComboVm<RepresentativeFinancialRequest> Clients { get; set; }
        = new("ClientName", SearchableComboEntryWidth);
    
    /// <inheritdoc/>
    public ClientRepresentativeReceiptEntryViewModel(RepresentativeFinancialRequest? model,
        EntryType entryType,
        Type pageType) : base(model,
        entryType,
        pageType,
        "Marketers",
        "RepresentativePaymentReceipt",
        "MarketerName")
    {
        Party.Combo.SelectionChanged += (_, comboModel) => SetMarketerInfo(comboModel);

        AddValidation(() => Model.ClientId)
            .AddValidation(() => Model.MarketerId);
    }

    [ObservableProperty]
    public partial decimal ClientPercentage { get; set; }

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

        if (Model is null || Party.Combo.SelectedItem is null || Safes.SelectedItem is null)
        {
            // As usual, let the validation system inform the user about this.
            return;
        }
        
        Model.MarketerId = Party.Combo.SelectedItem.Id;
        Model.SafeId = Safes.SelectedItem.Id;
        Model.MarketerShare = ClientPercentage;
    }

    public async void SetMarketerInfo(ComboModel? model)
    {
        var id = model?.Id;
        
        if (id == Identifier.Empty)
        { 
            Warning("Marketer ID was null or empty while trying to set marketer info in client " +
                    "representative receipt creator page. Resetting clients list...");
            Clients.Combo.Reset();

            return;
        }
        
        ClientPercentage = (await RequestOneUsingQueryRequestAsync<TypeOf<decimal>>("MarketerPercentage", 
            new StatementQueryRequest
            {
                AccountId = id
            })).Data!.Value;

        var data = await RequestCombosUsingQueryRequestAndReturnAsync("Clients", new ClientQueryRequest
        {
            MarketerId = id,
        });
        
        Clients.Load(this, data, onSelectionChanged: s => Model.ClientId = s.Id);
    }
}
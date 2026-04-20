using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.Financial.Requests;
using Fly.Shared.Modules.PurchaseAndSales.Requests;
using Leapay.Client.ClientModels.Dto.DtoModels;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.Polymorphism.Interfaces;
using Leapay.Client.Reports;
using SmartErpWin.Core.Enums;

namespace Leapay.Client.UserInterface.FinancingOperations.OperationViewModels;

public partial class DiscountItemFinanceEntryViewModel : ModelBase.FinanceOperationViewModelBase<DiscountItemBasedRequest>
{
    [ObservableProperty]
    public partial SearchableComboVm<DiscountItemBasedRequest> Clients { get; set; }
        = new("ClientName", SplitSearchableComboEntryWidth);
    
    [ObservableProperty]
    public partial SearchableComboVm<DiscountItemBasedRequest> Items { get; set; }
        = new("ItemName", SearchableComboEntryWidth);

    [ObservableProperty]
    public partial SuperComboVm<DiscountItemBasedRequest> Units { get; set; } = new();
    
    public DiscountItemFinanceEntryViewModel(DiscountItemBasedRequest? model,
        EntryType entryType,
        Type pageType) : base(model,
        entryType,
        pageType,
        "Marketers",
        "DiscountItemPaymentReceipt",
        "MarketerName")
    {
        Party = new SearchableComboVm<DiscountItemBasedRequest>("MarketerName", SplitSearchableComboEntryWidth);
        Party.Combo.SelectionChanged += (_, comboModel) => SetMarketerInfo(comboModel);
        
        As<IValidationEnforcer>().ClearAll();
        
        AddValidation(() => Model.ItemId);
        AddValidation(() => Model.UnitId);
        AddValidation(() => Model.ClientId);
        AddValidation(() => Model.MarketerId)
            .AddValidation(() => Model.Amount, [ new(r => r != 0, "TypeAnAmount") ])
            .AddValidation(() => Model.Quantity, [ new(r => r > 0, "QuantityMustBeGreaterThanZero") ]);
    }
    
    protected override OperationType? FinanceType => OperationType.Withdraw;

    protected override async Task PrepareAdditionalAsync()
    {
        Model.Date = DateTime.Now;
        Model.ExpireDate = DateTime.Now;

        await Items.LoadAsync(this, "Items", new Identifier(), m =>
        {
            Model.ItemId = m.Id;
            SetUnits(m);
        });
    }
    
    public async void SetUnits(ComboModel model)
    {
        if (string.IsNullOrEmpty(model.Value))
        {
            Units.Reset();
            return;
        }

        await Units.LoadAsync(this, "Units", new Identifier(),
            m => Model.UnitId = m.Id,
            header: model.Value);
    }

    protected override void PreSaveActions()
    {
        if (Model is null || Clients.Combo.SelectedItem is null || Party.Combo.SelectedItem is null)
            return;
        
        // The name swapping is intentional. Read why below and don't change unless the
        // backend adjusts accordingly.
        
        Model.FromAccount = Clients.Combo.SelectedItem.Id;
        Model.ToAccount = Party.Combo.SelectedItem.Id;
        
        Model.ToName = Clients.Combo.SelectedItem.Display;
        Model.FromName = Party.Combo.SelectedItem.Display;
        
        // There's no safe in this case. The client is
        // the one who is going to receive the money from the representative
        // as part of the item's discount.
        // Apparently, this is reversed in the backend for some reason, 
        // but the operation still correctly adds the discounted amount to the client's
        // account (as evident in the receipt created after saving and in the entries page).
        // So I am not going to swap the account IDs here as to not
        // break any functionality. I'll only swap account names so that the user knows
        // what's actually going on with the accounts before commiting the transaction.
    }

    protected override void LoadReport(bool isDeposit, string? others, string? data = null)
    {
        ReportRunner.PrintDiscountItem(isDeposit, this);
        base.LoadReport(isDeposit, others, data);
    }

    private async void SetMarketerInfo(ComboModel? model)
    {
        var id = model?.Id;
        
        if (id == Identifier.Empty)
        { 
            Warning("Marketer ID was empty while trying to set marketer info in discount " +
                    "item receipt page. Resetting clients list...");
            Clients.Combo.Reset();
            
            return;
        }
        
        var data = await RequestCombosUsingQueryRequestAndReturnAsync("Clients", new ClientQueryRequest
        {
            MarketerId = id,
        });
        
        Clients.Load(this, data, onSelectionChanged: s => Model.ClientId = s.Id);

        Model.MarketerId = model.Id;
    }
}
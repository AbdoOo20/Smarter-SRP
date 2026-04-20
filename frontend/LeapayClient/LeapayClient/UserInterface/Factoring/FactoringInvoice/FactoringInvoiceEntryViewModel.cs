using Avalonia.Threading;
using Fly.Shared.Modules.Factoring.Dtos;
using Fly.Shared.Modules.Factoring.Enums;
using Fly.Shared.Modules.PurchaseAndSales.Enums;
using Leapay.Client.ClientModels.Dto.DtoModels;
using Leapay.Client.Helpers;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.UserInterface.Sales.Clients;
using SmartErpWin.Core.Enums;
using static Fly.Shared.Modules.Factoring.Enums.FactoringStatus;

namespace Leapay.Client.UserInterface.Factoring.FactoringInvoice;

public partial class FactoringInvoiceEntryViewModel : BaseItemPageEntryViewModel<DtoFactoringInvoice, DtoFactoringInvoiceDetail>
{
    protected override string Endpoint => "FactoringInvoice";
    
    [ObservableProperty]
    public partial bool UpfrontCostVisibility { get; set; }

    [ObservableProperty]
    public partial bool FactoringSectorVisibility { get; set; }

    [ObservableProperty]
    public partial long InvoiceNumber { get; set; }

    [ObservableProperty]
    public partial bool SafeVisibility { get; set; }
    
    protected override double DataGridHeight => 470;

    public StaticComboVm<PeymentType> PaymentTypes { get; } = new();
    
    public StaticComboVm<FactoringStatus> FactoringStatus { get; } = new();
    
    public StaticComboVm<FactoringItemsStatus> FactoringItemsStatus { get; } = new();
    
    [ObservableProperty]
    public partial bool BankVisibility { get; set; } = true;
    
    [ObservableProperty]
    public partial SuperComboVm<DtoFactoringInvoiceDetail> Safes { get; set; } = new();

    [ObservableProperty]
    public partial SuperComboVm<DtoFactoringInvoiceDetail> Stores { get; set; } = new();
    
    [ObservableProperty]
    public partial SearchableComboVm<DtoFactoringInvoiceDetail> Clients { get; set; } = new("Client", SplitSearchableComboEntryWidth);

    [ObservableProperty]
    public partial SuperComboVm<DtoFactoringInvoiceDetail> Banks { get; set; } = new();
    
    [ObservableProperty]
    public partial SearchableComboVm<DtoFactoringInvoiceDetail> FactoringSectors { get; set; } = new("ManufacturingFacility", SplitSearchableComboEntryWidth);
    
    [ObservableProperty]
    public partial decimal TotalPrice { get; set; }

    [ObservableProperty]
    public partial decimal TotalExtraCost { get; set; }
    
    [ObservableProperty]
    public partial BasePageEntryViewModel<DtoFactoringInvoice, DtoFactoringInvoiceCost> Costs { get; set; } = new();
    
    [ObservableProperty]
    public partial BasePageEntryViewModel<DtoFactoringInvoice, DtoFactoringInvoiceComposition> Compositions { get; set; } = new();
    
    public FactoringInvoiceEntryViewModel(
        DtoFactoringInvoice? model,
        EntryType entryType,
        Type pageType,
        long? invoiceNumber = null)
    {
        SetDetailsByInvoiceNumberOrModel(model, entryType, pageType, false, invoiceNumber)
            .ContinueWith(_ =>
            {
                Dispatcher.UIThread.Invoke(() =>
                {
        
                    if (EntryType == EntryType.Add)
                    {
                        Model.Date = DateTime.Now;
                        Model.EndDate = DateTime.Now.AddDays(1);
                    }
        
                    Model.Details ??= [];
        
                    OnItemDelete += OnDelete;
                    OnItemEdit += OnEdit;

                    Costs.OnItemEdit += OnCostsEdit;
                    Costs.OnItemDelete += OnCostsDelete; 
        
                    Compositions.OnItemEdit += OnCompositionEdit;
                    Compositions.OnItemDelete += OnCompositionDelete;
        
                    RefreshState(async () => await StartupAsync());

                    AddValidation(() => Model.CollectionRate,
                            [new(d => d is >= 0 and <= 100, "CollectionRateMustBe0To100")],
                            optional: true)
                        .AddValidation(() => Model.ClientId, optional: false);
                });
            });
    }

    private async Task StartupAsync(bool refresh = true)
    {
        await OnAsync(EntryType, EntryType.Add, async () => InvoiceNumber = (await RequestSpecificAsync<long>("Number")).Data);
        On(EntryType, EntryType.Edit, () => InvoiceNumber = Model.Number);
        SetStaticComboBoxes();
        SetDetailsModel(null);
        SetCostsModel(null);
        SetCompositionsModel(null);
        UpdateCompositionsFilter();
        
        DetailsEnforcer.AddValidation(CreateValidation(() => ModelDetails.Model.ItemId, optional: false));
        DetailsEnforcer.AddValidation(CreateValidation(() => ModelDetails.Model.UnitId, optional: false));
        DetailsEnforcer.AddValidation(CreateValidation(() => ModelDetails.Model.Quantity, optional: false));
        DetailsEnforcer.AddValidation(CreateValidation(() => ModelDetails.Model.Price, optional: false));
        
        CompositionsEnforcer.AddValidation(CreateValidation(() => ModelCompositions.Model.ItemCompositionId, optional: false));
        CompositionsEnforcer.AddValidation(CreateValidation(() => ModelCompositions.Model.Quantity, optional: false));
        
        CostsEnforcer.AddValidation(CreateValidation(() => ModelCosts.Model.Price, optional: false));
        CostsEnforcer.AddValidation(CreateValidation(() => ModelCosts.Model.CostName, optional: false));
        
        await LoadDataAsync();
        await OnAsync(() => refresh, async () => await RefreshAsync());
        
        CalculatePrice();
        CheckVisibilities();
    }

    private void SetStaticComboBoxes()
    {
        PaymentTypes.Set(GetPaymentType(Model.PaymentType, EntryType), s =>
        {
            Model.PaymentType = s;
            CheckVisibilities();
        });
        
        FactoringStatus.Set(Model.FactoringStatus, s =>
        {
            Model.FactoringStatus = s;
            CheckVisibilities();
        });
        
        FactoringItemsStatus.Set(Model.FactoringItemsStatus, s =>
        {
            Model.FactoringItemsStatus = s;
            CheckVisibilities();
        });
    }

    private async Task LoadDataAsync()
    {
        Costs.SetData(Model.Costs);
        Compositions.SetData(Model.Compositions);
        SetData(Model.Details);
    }
    
    public void OnInternalFactoringCheckChanged(bool val)
    {
        FactoringSectorVisibility = !val;
    }
    
    protected override async Task RefreshAsync(bool refreshCombos = true)
    {
        SetDataRespectively();
        
        Costs.SetData(Model.Costs);
        Compositions.SetData(Model.Compositions);
        RefreshDataGrid();
        Costs.PublicRefreshDataGrid();
        Compositions.PublicRefreshDataGrid();
        
        if (refreshCombos)
        {
            await Clients.LoadAsync(this, "Clients", Model.ClientId,
                s => Model.ClientId = s.Id,
                adornerRightClick: async () => (await new ClientsPageViewModel().OnAddReturnAsync()).ToString(),
                adornerRightSelectedClick: async a => (await new ClientsPageViewModel().OnEditReturnAsync(a)).ToString(),
                adornerRightClickEndpointJudge: "Client.Add",
                adornerRightSelectedClickEndpointJudge: "Client.Edit",
                notFoundCallback: AutoSuggestNotFoundCallbackModel.CreateClientNotFoundCallback());
            
            var stores = GetVisibleStorages((await RequestComboModelsAsync<ComboModel>("Stores")).Data.ToList());
            var defStore = GetDefaultStorage(stores, Model.StoreId.GetValueOrDefault(), EntryType);
            Stores.Load(stores, defStore, s => Model.StoreId = s.Id);

            var safes = GetVisibleSafes((await RequestComboModelsAsync<ComboModel>("Safes")).Data.ToList());
            var defSafe = GetDefaultSafe(safes, Model.SafeId.GetValueOrDefault(), EntryType);
            Safes.Load(safes, defSafe, s => Model.SafeId = s.Id);
            
            var banks = GetVisibleBanks((await RequestComboModelsAsync<ComboModel>("Banks")).Data.ToList());
            var defBank = GetDefaultBank(banks, Model.BankId.GetValueOrDefault(), EntryType);
            Banks.Load(banks, defBank, s =>
            {
                Model.BankId = s.Id;
                Model.BankName = s.Display;
            });
            
            await FactoringSectors.LoadAsync(this, "ManufacturingFacilities", Model.ManufacturingFacilityId,
                s => Model.ManufacturingFacilityId = s.Id);
        }
        
        CalculatePrice();
    }
    
    private void CalculatePrice()
    {
        TotalPrice = Model.Details
            .Sum(d => (d.Price - d.Discount) * d.Quantity);
        
        TotalExtraCost = Model.Costs.Sum(d => d.Price);
    }
    
    public void CheckVisibilities()
    {
        UpfrontCostVisibility = Model.FactoringStatus is ReceivedFeeInAdvance;
        SafeVisibility = Model.PaymentType is PeymentType.Cash;
        BankVisibility = Model.PaymentType is PeymentType.BankServices;
    }
    
    #region Model
    
    [RelayCommand]
    private async Task OnSavePrintAsync()
    {
        await SaveAsync(() => EventHelper.InvokeFactoringInvoice(new(Model)));
    }

    #endregion
}
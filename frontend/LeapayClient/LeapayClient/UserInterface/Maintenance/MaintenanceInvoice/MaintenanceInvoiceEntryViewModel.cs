using Avalonia.Threading;
using Fly.Shared.Modules.Maintenance.Dtos;
using Fly.Shared.Modules.Maintenance.Enums;
using Fly.Shared.Modules.PurchaseAndSales.Enums;
using Leapay.Client.ClientModels.Dto.DtoModels;
using Leapay.Client.Helpers;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.UserInterface.Sales.Clients;
using SmartErpWin.Core.Enums;
using static Fly.Shared.Modules.Maintenance.Enums.MaintenanceStatus;

namespace Leapay.Client.UserInterface.Maintenance.MaintenanceInvoice;

public partial class MaintenanceInvoiceEntryViewModel : BaseItemPageEntryViewModel<DtoMaintenanceInvoice, DtoMaintenanceInvoiceDetail>
{
    protected override string Endpoint => "MaintenanceInvoice";

    [ObservableProperty]
    public partial bool PaymentMethodVisibility { get; set; }

    [ObservableProperty]
    public partial bool MaintenanceSectorVisibility { get; set; }

    [ObservableProperty]
    public partial bool UpfrontCostVisibility { get; set; }
    
    [ObservableProperty]
    public partial bool BankVisibility { get; set; } = true;
    
    [ObservableProperty]
    public partial bool SafeVisibility { get; set; }
    
    [ObservableProperty]
    public partial long InvoiceNumber { get; set; }

    [ObservableProperty]
    public partial string? SuggestedPrice { get; set; }
    
    protected override double DataGridHeight => 530;

    public StaticComboVm<PeymentType> PaymentTypes { get; } = new();
    
    public StaticComboVm<MaintenanceStatus> MaintenanceStatus { get; } = new();
    
    public StaticComboVm<MaintenanceItemsStatus> MaintenanceItemsStatus { get; } = new();
    
    [ObservableProperty]
    public partial SuperComboVm<DtoMaintenanceInvoiceDetail> Safes { get; set; } = new();

    [ObservableProperty]
    public partial SuperComboVm<DtoMaintenanceInvoiceDetail> Stores { get; set; } = new();
    
    [ObservableProperty]
    public partial SuperComboVm<DtoMaintenanceInvoiceDetail> Banks { get; set; } = new();
    
    [ObservableProperty]
    public partial SearchableComboVm<DtoMaintenanceInvoiceDetail> Clients { get; set; } = new("Client", SplitSearchableComboEntryWidth - 40);

    [ObservableProperty]
    public partial SearchableComboVm<DtoMaintenanceInvoiceDetail> MaintenanceSectors { get; set; } = new("MaintenanceSector", SplitSearchableComboEntryWidth - 40);
    
    [ObservableProperty]
    public partial decimal TotalPrice { get; set; }

    [ObservableProperty]
    public partial decimal TotalExtraCost { get; set; }
    
    [ObservableProperty]
    public partial BasePageEntryViewModel<DtoMaintenanceInvoice, DtoMaintenanceInvoiceCost> Costs { get; set; } = new();
    
    public MaintenanceInvoiceEntryViewModel(
        DtoMaintenanceInvoice? model,
        EntryType entryType,
        Type pageType,
        long? invoiceNumber = null)
    {
        SetDetailsByInvoiceNumberOrModel(model, entryType, pageType, false, invoiceNumber)
            .ContinueWith(_ =>
            {
                Dispatcher.UIThread.Invoke(() =>
                {
                    Model.Details ??= [];
        
                    OnItemDelete += OnDelete;
                    OnItemEdit += OnEdit;

                    Costs.OnItemEdit += OnCostsEdit;
                    Costs.OnItemDelete += OnCostsDelete;
        
                    AddValidation(() => Model.ClientId, optional: false);
        
                    RefreshState(async () => await StartupAsync());
                });
            });
    }

    private async Task StartupAsync(bool refresh = true)
    {
        await OnAsync(EntryType, EntryType.Add,
            async () =>
            {
                Model.Date = DateTime.Now;
                Model.EndDate = DateTime.Now.AddDays(1);

                InvoiceNumber = (await RequestSpecificAsync<long>("Number")).Data;
            });
        
        On(EntryType, EntryType.Edit, () => InvoiceNumber = Model.Number);
        SetStaticComboBoxes();
        SetDetailsModel(null);
        SetCostsModel(null);
        
        DetailsEnforcer.AddValidation(CreateValidation(() => ModelDetails.Model.ItemId, optional: false));
        DetailsEnforcer.AddValidation(CreateValidation(() => ModelDetails.Model.UnitId, optional: false));
        DetailsEnforcer.AddValidation(CreateValidation(() => ModelDetails.Model.Quantity, optional: false));
        DetailsEnforcer.AddValidation(CreateValidation(() => ModelDetails.Model.Price, optional: false));
        
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
        
        MaintenanceStatus.Set(Model.MaintenanceStatus, s =>
        {
            Model.MaintenanceStatus = s;
            CheckVisibilities();
        });
        
        MaintenanceItemsStatus.Set(Model.MaintenanceItemsStatus, s =>
        {
            Model.MaintenanceItemsStatus = s;
        });
    }

    private async Task LoadDataAsync()
    {

    }
    
    public void OnInternalMaintenanceCheckChanged(bool val)
    {
        MaintenanceSectorVisibility = !val;
    }
    
    protected override async Task RefreshAsync(bool refreshCombos = true)
    {
        SetData(Model.Details);
        Costs.SetData(Model.Costs);
        RefreshDataGrid();
        Costs.PublicRefreshDataGrid();
        
        if (refreshCombos)
        {
            await Clients.LoadAsync(this, "Clients", Model.ClientId,
                s => Model.ClientId = s.Id,
                adornerRightClick: async () => (await new ClientsPageViewModel().OnAddReturnAsync()).ToString(),
                adornerRightClickEndpointJudge: "Client.Add",
                adornerRightSelectedClickEndpointJudge: "Client.Edit",
                adornerRightSelectedClick: async a => (await new ClientsPageViewModel().OnEditReturnAsync(a)).ToString(),
                notFoundCallback: AutoSuggestNotFoundCallbackModel.CreateClientNotFoundCallback());
            
            var stores = GetVisibleStorages((await RequestComboModelsAsync<ComboModel>("Stores")).Data.ToList());
            var defStore = GetDefaultStorage(stores, Model.StoreId.GetValueOrDefault(), EntryType);
            Stores.Load(stores, defStore, s => Model.StoreId = s.Id);

            var banks = GetVisibleBanks((await RequestComboModelsAsync<ComboModel>("Banks")).Data.ToList());
            var defBank = GetDefaultBank(banks, Model.BankId.GetValueOrDefault(), EntryType);
            Banks.Load(banks, defBank, s =>
            {
                Model.BankId = s.Id;
                Model.BankName = s.Display;
            });
            
            var safes = GetVisibleSafes((await RequestComboModelsAsync<ComboModel>("Safes")).Data.ToList());
            var defSafe = GetDefaultSafe(safes, Model.SafeId.GetValueOrDefault(), EntryType);
            Safes.Load(safes, defSafe, s => Model.SafeId = s.Id);
            
            await MaintenanceSectors.LoadAsync(this, "MaintenanceSectors", Model.MaintenanceSectorId,
                s => Model.MaintenanceSectorId = s.Id);
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
        UpfrontCostVisibility = Model.MaintenanceStatus is ReceivedFeeInAdvance;
        SafeVisibility = Model.PaymentType is PeymentType.Cash;
        BankVisibility = Model.PaymentType is PeymentType.BankServices;
    }
    
    #region Model
    
    [RelayCommand]
    private async Task OnSavePrintAsync()
    {
        await SaveAsync(() => EventHelper.InvokeMaintenanceInvoice(new(Model)));
    }

    #endregion
}
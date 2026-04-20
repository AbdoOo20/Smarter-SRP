using Avalonia.Threading;
using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Sell;
using Fly.Shared.Modules.PurchaseAndSales.Enums;
using Fly.Shared.Modules.PurchaseAndSales.Requests;
using Leapay.Client.ClientModels.Dto.DtoModels;
using Leapay.Client.Events.Arguments;
using Leapay.Client.Helpers;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.Polymorphism.Interfaces;
using Leapay.Client.UserInterface.Sales.Clients;
using SmartErpWin.Core.Enums;
using DiscountType = Fly.Shared.Modules.PurchaseAndSales.Enums.DiscountType;

namespace Leapay.Client.UserInterface.Sales.SalesInvoice;

public partial class SalesInvoiceEntryViewModel : BaseItemPageEntryViewModel<DtoSalesInvoice, DtoSalesInvoiceDetail>
{
    public StaticComboVm<PeymentMode> PaymentType { get; } = new();

    protected override string Endpoint => "Sales";

    [ObservableProperty]
    public partial long InvoiceNumber { get; set; }
    
    [ObservableProperty]
    public partial bool PaymentMethodVisibility { get; set; }

    [ObservableProperty]
    public partial bool SafeVisibility { get; set; }

    [ObservableProperty]
    public partial bool NormalStoreVisibility { get; set; } = true;

    [ObservableProperty]
    public partial bool BankVisibility { get; set; }
    
    [ObservableProperty]
    public partial bool MarketerStoreVisibility { get; set; }

    [ObservableProperty]
    public partial bool FullVisibility { get; set; } = true;

    private bool _isReturn;
    
    [ObservableProperty]
    public partial decimal TotalPrice { get; set; }

    [ObservableProperty]
    public partial StorageType StorageType { get; set; }
    
    public StaticComboVm<PeymentType> PaymentTypes { get; } = new();
    
    public StaticComboVm<DiscountType> DiscountType { get; } = new();
    
    public StaticComboVm<TaxType> TaxType { get; } = new();
    
    public StaticComboVm<StorageType> StorageTypes { get; } = new();
    
    public StaticComboVm<InvoiceStatus> InvoiceStatus { get; } = new();
    
    [ObservableProperty]
    public partial SuperComboVm<DtoSalesInvoiceDetail> PaymentMethods { get; set; } = new();
    
    [ObservableProperty]
    public partial SuperComboVm<DtoSalesInvoiceDetail> Safes { get; set; } = new();
    
    [ObservableProperty]
    public partial SuperComboVm<DtoSalesInvoiceDetail> Stores { get; set; } = new();
    
    [ObservableProperty]
    public partial SuperComboVm<DtoSalesInvoiceDetail> Banks { get; set; } = new();
    
    [ObservableProperty]
    public partial SuperComboVm<DtoSalesInvoiceDetail> MarketerStores { get; set; } = new();
    
    [ObservableProperty]
    public partial SearchableComboVm<DtoSalesInvoiceDetail> Clients { get; set; }
        = new("Client", SplitSearchableComboEntryWidth);
    
    [ObservableProperty]
    public partial SearchableComboVm<DtoSalesInvoiceDetail> RepresentativeMarketers { get; set; }
        = new("RepresentativeMarketer", SplitSearchableComboEntryWidth);

    public event EventHandler? OnItemSelectionWindowClosed;

    private IValidationEnforcer DetailsEnforcer { get; set; } = new ValidationEnforceImpl();
    
    protected override double DataGridHeight => 560;

    public SalesInvoiceEntryViewModel(
        DtoSalesInvoice? model,
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
                    }
                    
                    Model.Details ??= [];
        
                    OnItemDelete += OnDelete;
                    OnItemEdit += OnEdit;
        
                    RefreshState(async () => await StartupAsync());
                });
            });
    }
    
    private async Task StartupAsync(bool refresh = true)
    {
        SetData(Model.Details);
        
        await OnAsync(EntryType, EntryType.Add, async () => InvoiceNumber = (await RequestSpecificAsync<long>("Number")).Data);
        On(EntryType, EntryType.Edit, () => InvoiceNumber = Model.Number);
        SetDetailsModel(null);

        DetailsEnforcer.AddValidation(CreateValidation(() => ModelDetails.Model.ItemId, optional: false));
        DetailsEnforcer.AddValidation(CreateValidation(() => ModelDetails.Model.UnitId, optional: false));
        DetailsEnforcer.AddValidation(CreateValidation(() => ModelDetails.Model.Quantity, optional: false));
        
        PaymentTypes.Set(GetPaymentType(Model.PeymentType, EntryType), s =>
        {
            Model.PeymentType = s;
            CheckVisibilities();
        });
        
        StorageTypes.Set(GetStorageType(Model.StorageType, EntryType), s =>
        {
            Model.StorageType = s;
            SetStore();
        });
        
        DiscountType.Set(Model.DiscountType, s =>
        {
            Model.DiscountType = s;
            CalculatePrice();
        });
        
        TaxType.Set(Model.TaxType, s =>
        {
            Model.TaxType = s;
            CalculatePrice();
        });
        
        InvoiceStatus.Set(Model.InvoiceStatus, s =>
        {
            Model.InvoiceStatus = s;
        });
        
        SetStore();
        
        CalculatePrice();
        CheckVisibilities();

        if (refresh)
        {
            await RefreshAsync();
        }
    }
    
    private void OnEdit(object? sender, DataGridButtonEventArgs<DtoSalesInvoiceDetail> args)
    {
        SetDetailsModel(args.Model);
        SelectInDataGrid(args.Model);
    }
    
    protected override async Task RefreshAsync(bool refreshCombos = true)
    {
        if (refreshCombos)
        {
            if (Model.RepresentativeId is null)
            {
                await LoadClientsAsync();
            }
            else
            {
                SetClientForRepresentative();
            }
        
            var safes = GetVisibleSafes((await RequestComboModelsAsync<ComboModel>("Safes")).Data.ToList());
            var defSafe = GetDefaultSafe(safes, Model.SafeId.GetValueOrDefault(), EntryType);
            Safes.Load(safes, defSafe, s => Model.SafeId = s.Id);
        
            var stores = GetVisibleStorages((await RequestComboModelsAsync<ComboModel>("Stores")).Data.ToList());
            var defStore = GetDefaultStorage(stores, Model.StoreId.GetValueOrDefault(), EntryType);
            Stores.Load(stores, defStore, s =>
            {
                Model.StoreId = s.Id;
                Model.StoreName = s.Display;
                CheckStorageConditions();
            });
        
            var banks = GetVisibleBanks((await RequestComboModelsAsync<ComboModel>("Banks")).Data.ToList());
            var defBank = GetDefaultBank(banks, Model.BankId.GetValueOrDefault(), EntryType);
            Banks.Load(banks, defBank, s =>
            {
                Model.BankId = s.Id;
                Model.BankName = s.Display;
            });
            
            await MarketerStores.LoadAsync(this, "MarketerStores", Model.MarketerStorageId,
                s =>
                {
                    Model.MarketerStorageId = s.Id;
                    Model.MarketerStorageName = s.Display;
                });
        
            await RepresentativeMarketers.LoadAsync(this, "Representatives",
                Model.RepresentativeId,
                s =>
                {
                    Model.RepresentativeId = s.Id;

                    if (s.Id.IsNullOrEmpty())
                    {
                        _ = LoadClientsAsync();
                    }
                    else
                    {
                        SetClientForRepresentative();
                    }
                });
        }
        
        SetData(Model.Details.ToList());
        RefreshDataGrid();
        CalculatePrice();
    }

    private async Task LoadClientsAsync()
    {
        await Clients.LoadAsync(this, "Clients", Model.ClientId,
            c => Model.ClientId = c.Id,
            adornerRightClick: async () => (await new ClientsPageViewModel().OnAddReturnAsync()).ToString(),
            adornerRightClickEndpointJudge: "Client.Add",
            adornerRightSelectedClickEndpointJudge: "Client.Edit",
            adornerRightSelectedClick: async a => (await new ClientsPageViewModel().OnEditReturnAsync(a)).ToString(),
            notFoundCallback: AutoSuggestNotFoundCallbackModel.CreateClientNotFoundCallback());
    }

    private void CheckVisibilities()
    {
        PaymentMethodVisibility = Model.PeymentType == PeymentType.Cash;
        BankVisibility = Model.PeymentType == PeymentType.BankServices;
        SafeVisibility = PaymentMethodVisibility;
    }

    private async void SetClientForRepresentative()
    {
        var id = RepresentativeMarketers.Combo.SelectedItem?.Id;
        
        if (!RepresentativeMarketers.Combo.IsAnySelected())
            return;
        
        var data = await RequestCombosUsingQueryRequestAsync<ComboModel>("Clients", new ClientQueryRequest
        {
            MarketerId = id
        });

        Clients.Load(this, data.Data.ToList(), Model.ClientId,
            m => Model.ClientId = m.Id);
    }
    
    private ComboModel GetStoreModel()
    {
        var store = GetCurrentStore();
        
        return new ComboModel
        {
            Display = store.name,
            Value = store.storageId.ToString()
        };
    }
    
    private (string? name, Identifier? storageId) GetCurrentStore()
    {
        var (name, storageId) = Model.StorageType == StorageType.Normal
            ? (Model.StoreName, Model.StoreId)
            : (Model.MarketerStorageName, Model.MarketerStorageId);
        return (name, storageId);
    }
    
    public void SetStore()
    {
        var isNormal = StorageTypes.GetSelectedItem() == StorageType.Normal;
        StorageType = isNormal ? StorageType.Normal : StorageType.Marketer;

        NormalStoreVisibility = isNormal.ApplyVisibility();
        MarketerStoreVisibility = (!isNormal).ApplyVisibility();
    }

    public void CalculatePrice(decimal? discountParam = null)
    {
        var (_, discFinal, _) = Model.Count();
        TotalPrice = discFinal;
    }

    [RelayCommand]
    private async Task OnSavePrintAsync()
    {
        await SaveAsync(() => EventHelper.InvokeSales(new(Model)));
    }
}
using SmartErpWin.Core.Extensions;
using Fly.Framework.Shared.Common;
using Avalonia.Threading;
using Fly.Shared.Modules.PurchaseAndSales.Dtos;
using Fly.Shared.Modules.PurchaseAndSales.Enums;
using Leapay.Client.ClientModels.Dto.DtoModels;
using Leapay.Client.Helpers;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.Polymorphism.Interfaces;
using SmartErpWin.Core.Enums;

namespace Leapay.Client.UserInterface.Purchases.PurchasesInvoice;

public partial class PurchaseInvoiceEntryViewModel : BaseItemPageEntryViewModel<DtoPurchaseInvoice, DtoPurchaseInvoiceDetail>
{
    protected override string Endpoint => "Purchase";

    [ObservableProperty]
    public partial bool SafesVisibility { get; set; }
    
    [ObservableProperty]
    public partial bool PaymentMethodVisibility { get; set; }

    [ObservableProperty]
    public partial bool SupplierVisibility { get; set; }

    [ObservableProperty]
    public partial bool BankVisibility { get; set; }
    
    [ObservableProperty]
    public partial long InvoiceNumber { get; set; }

    [ObservableProperty]
    public partial string SuggestedPrice { get; set; }
    
    protected override double DataGridHeight => 550;

    public StaticComboVm<PeymentType> PaymentTypes { get; } = new();
    
    [ObservableProperty]
    public partial SuperComboVm<DtoPurchaseInvoiceDetail> Stores { get; set; } = new();
    
    [ObservableProperty]
    public partial SearchableComboVm<DtoPurchaseInvoiceDetail> Suppliers { get; set; } = new("Supplier");
    
    [ObservableProperty]
    public partial SuperComboVm<DtoPurchaseInvoiceDetail> Banks { get; set; } = new();
    
    [ObservableProperty]
    public partial SuperComboVm<DtoPurchaseInvoiceDetail> Safes { get; set; } = new();

    [ObservableProperty]
    public partial decimal TotalPrice { get; set; }

    [ObservableProperty]
    public partial BasePageEntryViewModel<DtoPurchaseInvoice, DtoPurchaseInvoiceCost> Costs { get; set; } = new();
    
    private IValidationEnforcer DetailsEnforcer { get; set; } = new ValidationEnforceImpl();
    
    private IValidationEnforcer CostsEnforcer { get; set; } = new ValidationEnforceImpl();

    public PurchaseInvoiceEntryViewModel(
        DtoPurchaseInvoice? model,
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

                    Costs.OnItemEdit += OnCostsEdit;
                    Costs.OnItemDelete += OnCostsDelete;
        
                    RefreshState(async () => await StartupAsync());
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
        
        DetailsEnforcer.AddValidation(CreateValidation(() => ModelDetails.Model.ItemId, optional: false));
        DetailsEnforcer.AddValidation(CreateValidation(() => ModelDetails.Model.UnitId, optional: false));
        DetailsEnforcer.AddValidation(CreateValidation(() => ModelDetails.Model.Quantity, optional: false));
        DetailsEnforcer.AddValidation(CreateValidation(() => ModelDetails.Model.Price, optional: false));

        CostsEnforcer.AddValidation(CreateValidation(() => ModelCosts.Model.Name, optional: false));
        CostsEnforcer.AddValidation(CreateValidation(() => ModelCosts.Model.Ammount, optional: false));
        
        await LoadDataAsync();
        await OnAsync(() => refresh, async () => await RefreshAsync());
        
        CalculatePrice();
        CheckVisibilities();
    }

    private void SetStaticComboBoxes()
    {
        PaymentTypes.Set(GetPaymentType(Model.PeymentType, EntryType), s =>
        {
            Model.PeymentType = s;
            CheckVisibilities();
        });
    }

    private async Task LoadDataAsync()
    {
        var data = new List<DtoPurchaseInvoiceDetail>();
        
        foreach (var dto in Model.Details)
        {
            // Lines copied from another invoice have no Id yet; giving them all the same
            // (empty) TemporaryId made editing one line duplicate it over the others and
            // deleting one line delete them all.
            dto.TemporaryId = dto.Id.IsNullOrEmpty() ? Identifier.Create(Guid.NewGuid()) : dto.Id;

            data.Add(dto);
        }
                
        Model?.Details?.Clear();
        Model?.Details?.AddRange(data ?? []);

        var secondaryData = Model.Costs.Select(dto =>
        {
            // Lines copied from another invoice have no Id yet; giving them all the same
            // (empty) TemporaryId made editing one line duplicate it over the others and
            // deleting one line delete them all.
            dto.TemporaryId = dto.Id.IsNullOrEmpty() ? Identifier.Create(Guid.NewGuid()) : dto.Id;
            dto.Ammount = dto.Ammount;

            return dto;
        }).ToList();
        
        Model?.Costs?.Clear();
        Model?.Costs?.AddRange(secondaryData ?? []);
    }

    protected override async Task RefreshAsync(bool refreshCombos = true)
    {
        SetData(Model.Details);
        Costs.SetData(Model.Costs);
        RefreshDataGrid();
        Costs.PublicRefreshDataGrid();
        
        if (refreshCombos)
        {
            await Suppliers.LoadAsync(this, "Supplier", Model.SupplierId,
                s => Model.SupplierId = s.Id);
            
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
            
            var stores = GetVisibleStorages((await RequestComboModelsAsync<ComboModel>("Stores")).Data.ToList());
            var defStore = GetDefaultStorage(stores, Model.StoreId, EntryType);
            Stores.Load(stores, defStore, s => Model.StoreId = s.Id);
        }
        
        CalculatePrice();
    }
    
    private void CalculatePrice()
    {
        TotalPrice = Model.Details
            .Sum(d => (d.Price - d.Discount) * d.Quantity) + Model.Costs
            .Sum(d => d.Ammount);
    }
    
    public void CheckVisibilities()
    {
        PaymentMethodVisibility = Model.PeymentType is PeymentType.Cash;
        SafesVisibility = Model.PeymentType is PeymentType.Cash or PeymentType.Deferred;
        SupplierVisibility = Safes.IsAnySelected();
        BankVisibility = Model.PeymentType is PeymentType.BankServices;
    }
    
    [RelayCommand]
    private async Task OnSavePrintAsync()
    {
        await SaveAsync(() => EventHelper.InvokePurchases(new(Model)));
    }
}
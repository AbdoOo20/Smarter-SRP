using SmartErpWin.Core.Extensions;
using Fly.Framework.Shared.Common;
using AvaloniaEdit.Utils;
using Fly.Shared.Modules.PurchaseAndSales.Dtos;
using Fly.Shared.Modules.PurchaseAndSales.Enums;
using Leapay.Client.ClientModels.Dto.DtoModels;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.Polymorphism.Interfaces;
using SmartErpWin.Core.Enums;

namespace Leapay.Client.UserInterface.Sales.QuantityCrediting;

public partial class QuantityCreditingEntryViewModel : BaseItemPageEntryViewModel<DtoQuantityCreditingInvoice, DtoQuantityCreditingInvoiceDetail>
{
    protected override string Endpoint => "QuantityCrediting";

    [ObservableProperty]
    public partial bool PaymentMethodVisibility { get; set; }

    [ObservableProperty]
    public partial bool SupplierVisibility { get; set; }

    [ObservableProperty]
    public partial long InvoiceNumber { get; set; }
    
    public StaticComboVm<PeymentType> PaymentTypes { get; } = new();
    
    [ObservableProperty]
    public partial SuperComboVm<DtoQuantityCreditingInvoiceDetail> Stores { get; set; } = new();
    
    [ObservableProperty]
    public partial SearchableComboVm<DtoQuantityCreditingInvoiceDetail> Suppliers { get; set; } = new("Supplier");
    
    [ObservableProperty]
    public partial SearchableComboVm<DtoQuantityCreditingInvoiceDetail> Safes { get; set; } = new("Safe");

    [ObservableProperty]
    public partial decimal TotalPrice { get; set; }

    private IValidationEnforcer DetailsEnforcer { get; set; } = new ValidationEnforceImpl();

    public QuantityCreditingEntryViewModel(
        DtoQuantityCreditingInvoice? model,
        EntryType entryType,
        Type pageType,
        bool isSilent = false)
    {
        SetDetails(model, entryType, pageType, false, isSilent);
        
        if (EntryType == EntryType.Add)
        {
            Model.Date = DateTime.Now;
        }
        
        Model.Details ??= [];
        
        OnItemDelete += OnDelete;
        OnItemEdit += OnEdit;
        
        RefreshState(async () => await StartupAsync());
    }

    private async Task StartupAsync(bool refresh = true)
    {
        await OnAsync(EntryType, EntryType.Add, async () => InvoiceNumber = (await RequestSpecificAsync<long>("Number")).Data);
        On(EntryType, EntryType.Edit, () => InvoiceNumber = Model.Number);
        SetStaticComboBoxes();
        SetDetailsModel(null);
        
        DetailsEnforcer.AddValidation(CreateValidation(() => ModelDetails.Model.ItemId, optional: false));
        DetailsEnforcer.AddValidation(CreateValidation(() => ModelDetails.Model.UnitId, optional: false));
        
        await LoadDataAsync();
        await OnAsync(() => refresh, async () => await RefreshAsync());
        
        CalculatePrice();
        CheckVisibilities();
    }

    private void SetStaticComboBoxes()
    {
        PaymentTypes.Set(Model.PeymentType, s =>
        {
            Model.PeymentType = s;
            CheckVisibilities();
        });
    }

    private async Task LoadDataAsync()
    {
        if (IsSilent)
            return;
        
        var categories = (await RequestComboModelsAsync<ComboModel>("Items")).Data;

        var data = new List<DtoQuantityCreditingInvoiceDetail>();
        
        foreach (var dto in Model.Details)
        {
            dto.ItemName = categories.FirstOrDefault(u => u.Value == dto.ItemId.Value.ToString())?
                .Display;
            // Lines copied from another invoice have no Id yet; giving them all the same
            // (empty) TemporaryId made editing one line duplicate it over the others and
            // deleting one line delete them all.
            dto.TemporaryId = dto.Id.IsNullOrEmpty() ? Identifier.Create(Guid.NewGuid()) : dto.Id;

            var units = (await RequestComboModelsAsync<ComboModel>("Units", dto.ItemId.ToString())).Data;

            dto.UnitName = units.FirstOrDefault(u => u.Value == dto.UnitId.Value.ToString())?
                .Display;

            data.Add(dto);
        }
                
        Model?.Details?.Clear();
        Model?.Details?.AddRange(data ?? []);
    }

    protected override async Task RefreshAsync(bool refreshCombos = true)
    {
        SetData(Model.Details);
        RefreshDataGrid();
        
        if (refreshCombos && !IsSilent)
        {
            await Stores.LoadAsync(this, "Stores", Model.StoreId,
                s => Model.StoreId = s.Id);
            
            await Safes.LoadAsync(this, "Safes", Model.SafeId,
                s => Model.SafeId = s.Id);
        }
        
        CalculatePrice();
    }
    
    private void CalculatePrice()
    {
        TotalPrice = Model.Details
            .Sum(d => d.Price * d.Quantity);
    }
    
    public void CheckVisibilities()
    {
        PaymentMethodVisibility = PaymentTypes.Is(PeymentType.Cash);
        SupplierVisibility = Safes.Combo.IsAnySelected();
    }
    
    #region Model
    
    private bool _printReturn;

    [RelayCommand]
    private async Task OnSavePrintAsync()
    {
        _printReturn = true;

        await SaveAsync();

        _printReturn = false;
    }

    #endregion
}
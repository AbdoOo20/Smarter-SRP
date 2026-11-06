using SmartErpWin.Core.Extensions;
using Fly.Framework.Shared.Common;
using Avalonia.Threading;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Sell;
using Fly.Shared.Modules.PurchaseAndSales.Enums;
using Leapay.Client.ClientModels.Dto.DtoModels;
using Leapay.Client.Helpers;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.Polymorphism.Interfaces;
using SmartErpWin.Core.Enums;

namespace Leapay.Client.UserInterface.Sales.SalesReturnInvoice;

public partial class SellReturnInvoiceEntryViewModel : BaseItemPageEntryViewModel<DtoSellReturnInvoice, DtoSellReturnInvoiceDetail>
{
    protected override string Endpoint => "SellReturnInvoice";

    [ObservableProperty]
    public partial bool SafesVisibility { get; set; }

    [ObservableProperty]
    public partial long InvoiceNumber { get; set; }
    
    public StaticComboVm<PeymentType> PaymentTypes { get; } = new();
    
    [ObservableProperty]
    public partial SuperComboVm<DtoSellReturnInvoiceDetail> Safes { get; set; } = new();

    // The store the returned items go back to (sales return) or leave from
    // (purchase return). The backend requires it when the return is not linked
    // to an existing invoice.
    [ObservableProperty]
    public partial SuperComboVm<DtoSellReturnInvoiceDetail> Stores { get; set; } = new();
    
    [ObservableProperty]
    public partial decimal TotalPrice { get; set; }
    
    private IValidationEnforcer DetailsEnforcer { get; set; } = new ValidationEnforceImpl();

    public SellReturnInvoiceEntryViewModel(
        DtoSellReturnInvoice? model,
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
        await OnAsync(EntryType, EntryType.Add, async () => InvoiceNumber = (await RequestSpecificAsync<long>("Number")).Data);
        On(EntryType, EntryType.Edit, () => InvoiceNumber = Model.Number);
        SetStaticComboBoxes();
        SetDetailsModel(null);
        DetailsEnforcer.AddValidation(CreateValidation(() => ModelDetails.Model.ItemId, optional: false));
        DetailsEnforcer.AddValidation(CreateValidation(() => ModelDetails.Model.UnitId, optional: false));
        DetailsEnforcer.AddValidation(CreateValidation(() => ModelDetails.Model.Quantity, optional: false));
        //DetailsEnforcer.AddValidation(CreateValidation(() => ModelDetails.Model.Price, optional: false));

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
        var categories = (await RequestComboModelsAsync<ComboModel>("Items")).Data;

        var data = new List<DtoSellReturnInvoiceDetail>();
        
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
        
        if (refreshCombos)
        {
            await Safes.LoadAsync(this, "Safes", Model.SafeId,
                s => Model.SafeId = s.Id);

            var stores = GetVisibleStorages((await RequestComboModelsAsync<ComboModel>("Stores")).Data.ToList());
            var defStore = GetDefaultStorage(stores, Model.StoreId.GetValueOrDefault(), EntryType);
            Stores.Load(stores, defStore, s =>
            {
                Model.StoreId = s.Id;
                Model.StoreName = s.Display ?? string.Empty;
            });
        }
        
        CalculatePrice();
    }
    
    private void CalculatePrice()
    {
        // Same net total the backend refunds: line discounts, then the sales
        // invoice discount (a fixed one shared in proportion to what is returned).
        TotalPrice = decimal.Round(Model.Count().discountedTotal, 2);
    }
    
    public void CheckVisibilities()
    {
        SafesVisibility = PaymentTypes.Is(PeymentType.Cash);
    }
    
    [RelayCommand]
    private async Task OnSavePrintAsync()
    {
        await SaveAsync(() => EventHelper.InvokeSellReturnInvoice(new(Model)));
    }
}
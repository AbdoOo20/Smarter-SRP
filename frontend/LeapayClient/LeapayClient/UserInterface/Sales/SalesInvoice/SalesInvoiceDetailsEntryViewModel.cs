using Avalonia.Controls.Notifications;
using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Interfaces;
using Fly.Framework.Shared.Extentions;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Sell;
using Fly.Shared.Modules.PurchaseAndSales.Requests;
using Fly.Shared.Modules.Store.Requests;
using Leapay.Client.ClientModels.Dto.DtoModels.SalesModels;
using Leapay.Client.ClientModels.Ui;
using Leapay.Client.Events.Arguments;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.Services;
using Leapay.Client.UserInterface.Common.Items.Models;
using SmartErpWin.Core.Extensions;

namespace Leapay.Client.UserInterface.Sales.SalesInvoice;

public partial class SalesInvoiceEntryViewModel
{
    [ObservableProperty]
    public partial ModelDetailsViewModelBase<DtoSalesInvoiceDetail> ModelDetails { get; set; } = new();
    
    [ObservableProperty]
    public partial SuperComboVm<DtoSalesInvoiceDetail> Units { get; set; } = new();

    [ObservableProperty]
    public partial bool IsExpireDateVisible { get; set; } = false;
    
    [ObservableProperty]
    public partial bool CanAddOrUpdateItems { get; set; } = false;
    
    [ObservableProperty]
    public partial bool ShowItemQuantityInStorage { get; set; } = false;
    
    [ObservableProperty]
    public partial decimal CurrentMinPrice { get; set; }
    
    public TextBlock? ItemQuantityInfoBar
    {
        get;
        set;
    }
    
    [ObservableProperty]
    public partial NotificationType SelectedItemStatusSeverity { get; set; } = NotificationType.Success;

    [ObservableProperty]
    public partial StaticComboVm<ItemPriceFilter> ItemPriceFilterCombo { get; set; } = new();

    private void CheckStorageConditions()
    {
        if (Model.StoreId.IsNullOrEmpty())
        {
            if (ItemInfoBar is not null)
            {
                ItemInfoBar.Severity = NotificationType.Warning;
            }
            SelectedItemStatus = string.Format(this["SelectStorageToAddItem"]);
            CanAddOrUpdateItems = false;
            ShowItemQuantityInStorage = false;
        }
        else
        {
            if (ItemInfoBar is not null)
            {
                ItemInfoBar.Severity = NotificationType.Information;
            }
            
            SetPrice();
            RefreshSelectedItemStatus();
            CanAddOrUpdateItems = true;
            // this function is only called after the one above, so no need to set
            // status again.
        }
        
        SetExpireDate();
    }

    public async void SetUnits()
    {
        if (SelectedItem?.Value is null)
            return;

        await Units.LoadAsync(this, "Units", ModelDetails?.Model.UnitId, s =>
        {
            ModelDetails.Model.UnitId = s.Id;
            ModelDetails.Model.UnitName = s.Display;
            
            SetPrice();
        }, SelectedItem?.Value, false);

        if (ModelDetails.Model?.UnitId == Identifier.Empty)
        {
            Units.SelectAt(0);
        }
    }
    
    private bool _isSettingPrice;
    private bool _hasFilterComboInitialized;

    public async void SetPrice(bool force = false)
    {
        if (_isSettingPrice)
            return;

        try
        {
            _isSettingPrice = true;
            
            var storeId = Model.StoreId;
            var storeIdValue = storeId.GetValueOrDefault();

            if (Units.SelectedItem is null || !Units.SelectedItem.HasValue())
                return;

            var itemInfoQuery = new GetItemInfoQueryRequest
            {
                StoreId = storeIdValue,
                UnitId = Units.SelectedItem?.Id ?? Identifier.Empty,
                ItemId = SelectedItem?.Id ?? Identifier.Empty
            };

            var req = await GetInnerByQueryAsync<ItemInfoQueryResponse>("ItemInfo", itemInfoQuery);

            if (!req.Status || req.Data is null || Model is null)
                return;
            
            if (ItemQuantityInfoBar is not null && !Model.StoreId.GetValueOrDefault().IsNullOrEmpty())
            {
                ShowItemQuantityInStorage = true;

                ItemQuantityInfoBar.Text = string.Format(this["CurrentItemQuantityInStorage"], req.Data.Quntity,
                    ModelDetails.Model.UnitName, Model.StoreName);
            }
                
            if (ModelDetails.Model.Price != req.Data.BulkPrice &&
                ModelDetails.Model.Price != req.Data.RegionalPrice && ModelDetails.Model.Price != 0 && !force && _hasFilterComboInitialized)
            {
                Information("User set a custom price in sales entry, skipping auto suggestion.");
                return;
            }
                
            ModelDetails.Model.Price = ItemPriceFilterCombo.Is(ItemPriceFilter.BulkPrice)
                ? req.Data.BulkPrice
                : req.Data.RegionalPrice;
            IsExpireDateVisible = req.Data.IsExpirable;
            CurrentMinPrice = req.Data.MinPrice;
        }
        catch (Exception ex)
        {
            Error(ex.ToString());
        }
        finally
        {
            _isSettingPrice = false;
        }
    }
    
    public async void SetExpireDate()
    {
        var storeId = Model.StoreId;
        var storeIdValue = storeId.GetValueOrDefault();
        var itemId = ComboModelBase.GetSelectedItemId();

        if (storeId?.IsNullOrEmpty() ?? true || itemId.IsNullOrEmpty())
            return;

        ComboModelBase.Clear(2, true);
        await RequestCombosUsingQueryRequestAsync("ExpireDate", 2, new ExpireDateQueryRequest
        {
            ItemId = itemId,
            StoreId = storeIdValue,
            StorageType = StorageType,
        });

        ComboModelBase.SelectAtOnePoint(2, 1);
    }
    
    private async void SetDetailsModel(DtoSalesInvoiceDetail? model)
    {
        Reset();
        ModelDetails.SetState(model);
        SelectedItem = InitNewComboModel(ModelDetails.Model.ItemName, ModelDetails.Model.ItemId);
        
        ItemPriceFilterCombo = new();
        ItemPriceFilterCombo.Set(ItemPriceFilter.RegionalPrice, _ => SetPrice(true));
        _hasFilterComboInitialized = true;
        
        SetUnits();
        SetPrice();
    }
    
    [RelayCommand]
    private void OnSubmit()
    {
        if(!VerifyValidations(DetailsEnforcer.Enforce(ModelDetails.Model)))
            return;
        
        if (ModelDetails.Model.ItemName.Contains("•"))
        {
            var splitName = ModelDetails.Model.ItemName.Split("•");
            ModelDetails.Model.ItemBarcode = splitName.ElementAtOrDefault(1);
            ModelDetails.Model.ItemName = splitName.ElementAtOrDefault(0);
        }

        var cc = ComboModelBase.GetApiCollectionByIndex(2).Count;
        if (cc > 1 & ModelDetails.Model.ExpireDate is null)
        {
            _ = DialogueServices.ShowMessageAsync(this["ValidationFailed"], 
                "ExpireDateCannotBeEmpty");
            return;
        }
        
        if (ModelDetails.Model.Price <= 0)
        {
            _ = DialogueServices.ShowMessageAsync(this["ValidationFailed"], 
                "PriceIsNotValid");
            return;
        }
        
        UpdateDetailsListing(ModelDetails.Model, true);
        Reset(true);
    }

    public void UpdateDetailsListing(DtoSalesInvoiceDetail model, bool checkForExisting)
    {
        // A line with the same TemporaryId is already in the invoice, so this is
        // an edit of that line: its new quantity replaces the old one.
        var isEditOfExistingLine = Model.Details.Any(m => m.TemporaryId == model.TemporaryId);

        var alreadyExisting = Model.Details.FirstOrDefault(m => m.TemporaryId != model.TemporaryId
                                                       && m.Discount == model.Discount
                                                       && m.UnitId == model.UnitId
                                                       && m.ItemId == model.ItemId
                                                       && m.Price == model.Price);
        if (alreadyExisting is not null && checkForExisting)
        {
            // Another line already has the same item, unit, price and discount,
            // so merge into it instead of adding a duplicate line.
            alreadyExisting.Quantity += model.Quantity;

            if (isEditOfExistingLine)
            {
                // The edited line was merged into the other one, so drop the original.
                var originalLine = Model.Details.FirstOrDefault(m => m.TemporaryId == model.TemporaryId);
                if (originalLine is not null)
                    Model.Details.Remove(originalLine);
            }

            UpdateDetailsListing(alreadyExisting, false);
            return;
        }
        
        Model.Details = UpdateListGlobal(model, Model.Details.CastAs<IDetailsModel>())
            .CastAs<DtoSalesInvoiceDetail>();
        
        SetData(Model.Details.ToList());
        
        RefreshDataGrid();
        CalculatePrice();
    }
    
    private void Reset(bool andSet = false)
    {
        ModelDetails = new();
        Units.Reset();
        IsExpireDateVisible = false;
        ShowItemQuantityInStorage = false;
        
        if (andSet)
        {
            SetDetailsModel(null);
        }
    }

    protected override void OnAfterItemIsSelected()
    {
        OnSelectedItemChanged();
        SetUnits();
        SetExpireDate();
        base.OnAfterItemIsSelected();
    }
    
    protected override void OnAfterItemsAreSelected()
    {
        HandleMultiItems(SelectedItems);
        base.OnAfterItemsAreSelected();
    }
    
    protected override async void OnDelete(object? sender, DataGridButtonEventArgs<DtoSalesInvoiceDetail> args)
    {
        if (!await DialogueServices.ConfirmDeleteAsync())
            return;

        Model.Details.Remove(Model.Details.FirstOrDefault(d => d.TemporaryId == args.Model.TemporaryId));
        _ = RefreshAsync(false);
    }
    
    private void OnSelectedItemChanged()
    {
        if (string.IsNullOrEmpty(SelectedItem?.Value))
        {
            ModelDetails.Model.ItemId = default;
            return;
        }
        
        if (Guid.TryParse(SelectedItem.Value, out var id))
        {
            ModelDetails.Model.ItemId = id;
            ModelDetails.Model.ItemName = SelectedItem.Display;
        }
        
        RefreshSelectedItemStatus();
        //CheckStorageConditions();
    }
    
    private void HandleMultiItems(List<ItemsSelectionModel> models)
    {
        foreach (var model in models)
        {
            var @new = new DtoSalesInvoiceDetailsModel
            {
                UnitId = (model.SelectedUnit ?? new()).UnitId.GetValueOrDefault(),
                UnitName = model.SelectedUnit?.UnitName ?? "",
                ItemId = model.ItemId.GetValueOrDefault(),
                Quantity = model.Quantity,
                Price = model.ItemCustomPrice,
                ItemName = model.ItemName ?? this["ItemWithNoName"],
                ItemBarcode = model.ItemBarcode ?? ""
            };
            
            UpdateDetailsListing(@new, true);
        }
    }
}
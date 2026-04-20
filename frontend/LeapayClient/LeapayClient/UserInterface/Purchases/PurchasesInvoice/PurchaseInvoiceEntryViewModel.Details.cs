using System.Globalization;
using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Interfaces;
using Fly.Shared.Modules.PurchaseAndSales.Dtos;
using Fly.Shared.Modules.PurchaseAndSales.Requests;
using Leapay.Client.Events.Arguments;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.Services;
using Leapay.Client.UserInterface.Common.Items.Models;
using SmartErpWin.Core.Extensions;

namespace Leapay.Client.UserInterface.Purchases.PurchasesInvoice;

public partial class PurchaseInvoiceEntryViewModel
{
    [ObservableProperty]
    public partial SuperComboVm<DtoPurchaseInvoiceDetail> Units { get; set; } = new();
    
    [ObservableProperty]
    public partial ModelDetailsViewModelBase<DtoPurchaseInvoiceDetail> ModelDetails { get; set; } = new();
    
    [ObservableProperty]
    public partial bool IsExpireDateVisible { get; set; } = false;
    
    private void SetDetailsModel(DtoPurchaseInvoiceDetail? model)
    {
        Reset();
        ModelDetails.SetState(model);
        SelectedItem = InitNewComboModel(ModelDetails.Model.ItemName, ModelDetails.Model.ItemId);
        
        SetUnits();
    }
    
    [RelayCommand]
    private void OnSubmit()
    {
        if(!VerifyValidations(DetailsEnforcer.Enforce(ModelDetails.Model)))
            return;
        
        UpdateDetailsListing(ModelDetails.Model);
        Reset(true);
    }

    private void Reset(bool andSet = false)
    {
        ModelDetails = new();
        Units.Reset();
        IsExpireDateVisible = false;
        
        if (andSet)
        {
            SetDetailsModel(null);
        }
    }

    protected override void OnAfterItemIsSelected()
    {
        OnSelectedItemChanged();
        SetUnits();
        base.OnAfterItemIsSelected();
    }

    public async void SetUnits()
    {
        if (SelectedItem?.Value is null)
            return;
        
        await Units.LoadAsync(this, "Units", ModelDetails.Model.UnitId,
            s =>
            {
                ModelDetails.Model.UnitId = s.Id;
                ModelDetails.Model.UnitName = s.Display;
            }, SelectedItem?.Value, false);
        
        if (ModelDetails.Model?.UnitId.IsNullOrEmpty() ?? true)
        {
            Units.SelectAt(0);
        }

        if (!SelectedItem.HasValue())
            return;
        
        var itemInfoQuery = new GetItemInfoQueryRequest
        {
            StoreId = Model.StoreId,
            UnitId = Units.SelectedItem?.Id ?? Identifier.Empty,
            ItemId = SelectedItem?.Id ?? Identifier.Empty
        };

        var req = await GetInnerByQueryAsync<ItemInfoQueryResponse>("ItemInfo", itemInfoQuery);

        if (req.Status)
        {
            SuggestedPrice = req.Data!.Price.ToString(CultureInfo.InvariantCulture);
            IsExpireDateVisible = req.Data.IsExpirable;
        }
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
    }
    
    public void UpdateDetailsListing(DtoPurchaseInvoiceDetail model)
    {
        Model.Details = UpdateListGlobal(model, Model.Details.CastAs<IDetailsModel>())
            .CastAs<DtoPurchaseInvoiceDetail>();
        
        SetData(Model.Details.ToList());
        
        RefreshDataGrid();
        CalculatePrice();
    }
    
    private void OnEdit(object? sender, DataGridButtonEventArgs<DtoPurchaseInvoiceDetail> args)
    {
        SetDetailsModel(args.Model);
        SelectInDataGrid(args.Model);
    }

    protected override void OnAfterItemsAreSelected()
    {
        HandleMultiItems(SelectedItems);
        base.OnAfterItemsAreSelected();
    }

    private void HandleMultiItems(List<ItemsSelectionModel> models)
    {
        foreach (var model in models)
        {
            var @new = new DtoPurchaseInvoiceDetail
            {
                UnitId = (model.SelectedUnit ?? new()).UnitId.GetValueOrDefault(),
                UnitName = model.SelectedUnit?.UnitName ?? "",
                ItemId = model.ItemId.GetValueOrDefault(),
                Quantity = model.Quantity,
                Price = model.ItemCustomPrice,
                ItemName = model.ItemName ?? this["ItemWithNoName"],
                ItemBarcode = model.ItemBarcode ?? ""
            };
            
            UpdateDetailsListing(@new);
        }
    }
    
    protected override async void OnDelete(object? sender, DataGridButtonEventArgs<DtoPurchaseInvoiceDetail> args)
    {
        if (!await DialogueServices.ConfirmDeleteAsync())
            return;

        Model.Details.RemoveAll(m => m.TemporaryId == args.Model.TemporaryId);
        _ = RefreshAsync(false);
    }
}
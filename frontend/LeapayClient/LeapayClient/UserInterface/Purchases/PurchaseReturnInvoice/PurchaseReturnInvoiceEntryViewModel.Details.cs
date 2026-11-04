using Fly.Framework.Shared.Common.Interfaces;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Purchase;
using Leapay.Client.Events.Arguments;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.Services;
using Leapay.Client.UserInterface.Common.Items.Models;
using SmartErpWin.Core.Extensions;

namespace Leapay.Client.UserInterface.Purchases.PurchaseReturnInvoice;

public partial class PurchaseReturnInvoiceEntryViewModel
{
    [ObservableProperty]
    public partial SuperComboVm<DtoPurchaseReturnInvoiceDetail> Units { get; set; } = new();
    
    [ObservableProperty]
    public partial ModelDetailsViewModelBase<DtoPurchaseReturnInvoiceDetail> ModelDetails { get; set; } = new();
    
    private void SetDetailsModel(DtoPurchaseReturnInvoiceDetail? model)
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
                ModelDetails.Model.UnitName = s.Display ?? string.Empty;
            }, SelectedItem?.Value, false);
        
        if (ModelDetails.Model?.UnitId.IsNullOrEmpty() ?? true)
        {
            Units.SelectAt(0);
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
    
    public void UpdateDetailsListing(DtoPurchaseReturnInvoiceDetail model)
    {
        Model.Details = UpdateListGlobal(model, Model.Details.CastAs<IDetailsModel>())
            .CastAs<DtoPurchaseReturnInvoiceDetail>();
        
        SetData(Model.Details.ToList());
        
        RefreshDataGrid();
        CalculatePrice();
    }
    
    private void OnEdit(object? sender, DataGridButtonEventArgs<DtoPurchaseReturnInvoiceDetail> args)
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
            var @new = new DtoPurchaseReturnInvoiceDetail
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
    
    protected override async void OnDelete(object? sender, DataGridButtonEventArgs<DtoPurchaseReturnInvoiceDetail> args)
    {
        if (!await DialogueServices.ConfirmDeleteAsync())
            return;
        
        Model.Details.RemoveAll(m => m.TemporaryId == args.Model.TemporaryId);
        _ = RefreshAsync(false);
    }
}
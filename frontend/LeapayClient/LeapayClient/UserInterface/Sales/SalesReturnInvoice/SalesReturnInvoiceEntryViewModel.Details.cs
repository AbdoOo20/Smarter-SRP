using Fly.Framework.Shared.Common.Interfaces;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Sell;
using Leapay.Client.Events.Arguments;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.Services;
using SmartErpWin.Core.Extensions;

namespace Leapay.Client.UserInterface.Sales.SalesReturnInvoice;

public partial class SellReturnInvoiceEntryViewModel
{
    [ObservableProperty]
    public partial SuperComboVm<DtoSellReturnInvoiceDetail> Units { get; set; } = new();
    
    [ObservableProperty]
    public partial ModelDetailsViewModelBase<DtoSellReturnInvoiceDetail> ModelDetails { get; set; } = new();

    private void SetDetailsModel(DtoSellReturnInvoiceDetail? model)
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
    
    public void UpdateDetailsListing(DtoSellReturnInvoiceDetail model)
    {
        Model.Details = UpdateListGlobal(model, Model.Details.CastAs<IDetailsModel>())
            .CastAs<DtoSellReturnInvoiceDetail>();
        
        SetData(Model.Details.ToList());
        
        RefreshDataGrid();
        CalculatePrice();
    }
    
    private void OnEdit(object? sender, DataGridButtonEventArgs<DtoSellReturnInvoiceDetail> args)
    {
        SetDetailsModel(args.Model);
        SelectInDataGrid(args.Model);
    }
    
    protected override async void OnDelete(object? sender, DataGridButtonEventArgs<DtoSellReturnInvoiceDetail> args)
    {
        if (!await DialogueServices.ConfirmDeleteAsync())
            return;
        
        Model.Details.RemoveAll(m => m.TemporaryId == args.Model.TemporaryId);
        _ = RefreshAsync(false);
    }
}
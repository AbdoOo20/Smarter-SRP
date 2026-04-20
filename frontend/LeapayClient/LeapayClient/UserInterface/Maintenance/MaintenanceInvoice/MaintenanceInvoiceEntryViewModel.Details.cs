using Fly.Framework.Shared.Common.Interfaces;
using Fly.Shared.Modules.Maintenance.Dtos;
using Leapay.Client.Events.Arguments;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.Polymorphism.Interfaces;
using Leapay.Client.Services;
using Leapay.Client.UserInterface.Common.Items.Models;
using SmartErpWin.Core.Extensions;

namespace Leapay.Client.UserInterface.Maintenance.MaintenanceInvoice;

public partial class MaintenanceInvoiceEntryViewModel
{
    [ObservableProperty]
    public partial SuperComboVm<DtoMaintenanceInvoiceDetail> Units { get; set; } = new();
    
    [ObservableProperty]
    public partial ModelDetailsViewModelBase<DtoMaintenanceInvoiceDetail> ModelDetails { get; set; } = new();
    
    [ObservableProperty]
    public partial SuperComboVm<DtoMaintenanceInvoiceDetail> Variances { get; set; } = new();
    
    private IValidationEnforcer DetailsEnforcer { get; set; } = new ValidationEnforceImpl();

    private void SetDetailsModel(DtoMaintenanceInvoiceDetail? model)
    {
        Reset();
        ModelDetails.SetState(model);
        SelectedItem = InitNewComboModel(ModelDetails.Model.ItemName, ModelDetails.Model.ItemId);
        SetUnits();
        RefreshCombos();
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
        
        try
        {
            if(SelectedItem is not null && !string.IsNullOrEmpty(SelectedItem.Display))
                ModelDetails.Model.Price = decimal.Parse(SelectedItem.Display?.Split('•', StringSplitOptions.TrimEntries)[1]);
        }
        catch { }
        
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
    }

    private async void RefreshCombos()
    {
        await Variances.LoadAsync(this, "ItemVariances", ModelDetails.Model.ItemVarianceId,
            s =>
            {
                if (s.Id.IsNullOrEmpty())
                {
                    ModelDetails.Model.ItemVarianceId = null;
                    return;
                }
                
                ModelDetails.Model.ItemVarianceId = s.Id;
                ModelDetails.Model.ItemVarianceName = s.Display;
            });
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
    
    public void UpdateDetailsListing(DtoMaintenanceInvoiceDetail model)
    {
        Model.Details = UpdateListGlobal(model, Model.Details.CastAs<IDetailsModel>())
            .CastAs<DtoMaintenanceInvoiceDetail>();
        
        SetData(Model.Details.ToList());
        
        RefreshDataGrid();
        CalculatePrice();
    }
    
    private void OnEdit(object? sender, DataGridButtonEventArgs<DtoMaintenanceInvoiceDetail> args)
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
            var @new = new DtoMaintenanceInvoiceDetail
            {
                UnitId = (model.SelectedUnit ?? new()).UnitId.GetValueOrDefault(),
                UnitName = model.SelectedUnit?.UnitName ?? "",
                ItemId = model.ItemId.GetValueOrDefault(),
                Quantity = model.Quantity,
                Price = model.ItemCustomPrice,
                ItemName = model.ItemName ?? this["ItemWithNoName"],
            };
            
            UpdateDetailsListing(@new);
        }
    }
    
    protected override async void OnDelete(object? sender, DataGridButtonEventArgs<DtoMaintenanceInvoiceDetail> args)
    {
        if (!await DialogueServices.ConfirmDeleteAsync())
            return;
        
        Model.Details.Remove(Model.Details.FirstOrDefault(s => s.TemporaryId == args.Model.TemporaryId));
        _ = RefreshAsync(false);
    }
}
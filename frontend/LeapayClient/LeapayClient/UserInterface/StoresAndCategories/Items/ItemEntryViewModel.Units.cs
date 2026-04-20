using Fly.Framework.Shared.Common.Interfaces;
using Fly.Shared.Modules.Store.Requests;
using Leapay.Client.Events.Arguments;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.Services;
using SmartErpWin.Core.Extensions;

namespace Leapay.Client.UserInterface.StoresAndCategories.Items;

public partial class ItemEntryViewModel
{
    [ObservableProperty]
    public partial SuperComboVm<CreateUpdateItemUnitRequest> Units { get; set; } = new();
    
    [ObservableProperty]
    public partial ModelDetailsViewModelBase<CreateUpdateItemUnitRequest> ModelDetails { get; set; } = new();
    
    private async void SetDetailsModel(CreateUpdateItemUnitRequest? model)
    {
        if (IsSilent)
            return;
        
        Reset();
        ModelDetails.SetState(model);
        
        await Units.LoadAsync(this, "units", ModelDetails.Model.UnitId,
            s => ModelDetails.Model.UnitId = s.Id);
    }

    private bool PreSaveChecks(CreateUpdateItemUnitRequest model)
    {
        if ((!Units.IsAnySelected()).Stop("ValidationFailed",
            "UnitIdSelectOption")) return false;
        
        ModelDetails.Model.UnitName = Units.SelectedItem.Display;
        
        var units = Model.ItemUnits.Where(u => u.TemporaryId != model?.TemporaryId);
        
        if (units.Any(i => i.UnitId == model?.UnitId)
            .Stop("AnotherItemUnitExistsTitle",
                "AnotherItemUnitExistsMessage"))
            return false;
        
        if ((model.TransferRate <= 0)
            .Stop("ValidationFailed",
                "TransferRateNeedsValue"))
            return false;
        
        return !(units.Any(i => i.IsPrimary) && model.IsPrimary)
            .Stop("AnotherPrimaryItemUnitExistsTitle",
                "AnotherPrimaryItemUnitExistsMessage");
    }
    
    [RelayCommand]
    private void OnSubmit()
    {
        if (!PreSaveChecks(ModelDetails.Model))
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

    public void UpdateDetailsListing(CreateUpdateItemUnitRequest model)
    {
        Model.ItemUnits = UpdateListGlobal(model, Model.ItemUnits.CastAs<IDetailsModel>())
            .CastAs<CreateUpdateItemUnitRequest>();
        
        SetData(Model.ItemUnits.ToList());
        
        RefreshDataGrid();
    }
    
    private void OnEdit(object? sender, DataGridButtonEventArgs<CreateUpdateItemUnitRequest> args)
    {
        SetDetailsModel(args.Model);
        SelectInDataGrid(args.Model);
    }
    
    protected override async void OnDelete(object? sender, DataGridButtonEventArgs<CreateUpdateItemUnitRequest> args)
    {
        if (!await DialogueServices.ConfirmDeleteAsync())
            return;
        
        Model.ItemUnits = Model.ItemUnits.Where(x => x.TemporaryId != args.Model.TemporaryId);
        _ = RefreshAsync(false);
    }
}
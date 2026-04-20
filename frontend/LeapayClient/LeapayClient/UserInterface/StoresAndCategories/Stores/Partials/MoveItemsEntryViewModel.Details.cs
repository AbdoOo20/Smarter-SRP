using Fly.Framework.Shared.Common.Interfaces;
using Fly.Shared.Modules.Store.Requests;
using Leapay.Client.Events.Arguments;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.Services;
using SmartErpWin.Core.Extensions;

namespace Leapay.Client.UserInterface.StoresAndCategories.Stores.Partials;

public partial class MoveItemsEntryViewModel
{
    [ObservableProperty]
    public partial SuperComboVm<StorageItemsRequest> Units { get; set; } = new();
    
    /*
    [ObservableProperty]
    public partial SuperComboVm<StorageItemsRequest> Variances { get; set; } = new();
    */
    
    [ObservableProperty]
    public partial ModelDetailsViewModelBase<StorageItemsRequest> ModelDetails { get; set; } = new();
    
    private async void SetDetailsModel(StorageItemsRequest? model)
    {
        Reset();
        ModelDetails.SetState(model);
        
        SelectedItem = InitNewComboModel(ModelDetails.Model.ItemName, ModelDetails.Model.ItemId);

        await Units.LoadAsync(this, "Units", ModelDetails.Model.UnitId,
            s =>
            {
                ModelDetails.Model.UnitId = s.Id;
                ModelDetails.Model.UnitName = s.Display;
            });
        
        /*await Variances.LoadAsync(this, "ItemVariances", ModelDetails.Model.ItemVarianceId,
            s =>
            {
                ModelDetails.Model.ItemVarianceId = s.Id;
                ModelDetails.Model.ItemVarianceName = s.Display;
            });*/
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

        base.OnAfterItemIsSelected();
    }

    public void UpdateDetailsListing(StorageItemsRequest model)
    {
        Model.Details = UpdateListGlobal(model, Model.Details.CastAs<IDetailsModel>())
            .CastAs<StorageItemsRequest>();
        
        SetData(Model.Details.ToList());
        
        RefreshDataGrid();
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
    
    private void OnEdit(object? sender, DataGridButtonEventArgs<StorageItemsRequest> args)
    {
        SetDetailsModel(args.Model);
        SelectInDataGrid(args.Model);
    }
    
    protected override async void OnDelete(object? sender, DataGridButtonEventArgs<StorageItemsRequest> args)
    {
        if (!await DialogueServices.ConfirmDeleteAsync())
            return;
        
        Model.Details = Model.Details.Where(x => x.TemporaryId != args.Model.TemporaryId);
        _ = RefreshAsync(false);
    }
}
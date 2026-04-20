using Fly.Framework.Shared.Common.Interfaces;
using Fly.Shared.Modules.Store.Dtos;
using Leapay.Client.Events.Arguments;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.Services;
using SmartErpWin.Core.Extensions;

namespace Leapay.Client.UserInterface.StoresAndCategories.SupplyPermits;

public partial class SupplyPermitEntryViewModel
{
    [ObservableProperty]
    public partial SuperComboVm<DtoSupplyPermitDetail> Units { get; set; } = new();
    
    [ObservableProperty]
    public partial ModelDetailsViewModelBase<DtoSupplyPermitDetail> ModelDetails { get; set; } = new();
    
    private async void SetDetailsModel(DtoSupplyPermitDetail? model)
    {
        Reset();
        ModelDetails.SetState(model);
        
        On(() => ModelDetails.Model.IsNew(),
            () => ModelDetails.Model.ExpireDate = DateTime.Now.AddDays(30));
        
        SelectedItem = InitNewComboModel(ModelDetails.Model.ItemName, ModelDetails.Model.ItemId);
        SetUnits();
        
        OnPropertyChanged();
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

    public void UpdateDetailsListing(DtoSupplyPermitDetail model)
    {
        Model.SupplyPermitDetails = UpdateListGlobal(model, Model.SupplyPermitDetails.CastAs<IDetailsModel>())
            .CastAs<DtoSupplyPermitDetail>();
        
        SetData(Model.SupplyPermitDetails.ToList());
        
        RefreshDataGrid();
        ScrollToItem(model);
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
    
    private void OnEdit(object? sender, DataGridButtonEventArgs<DtoSupplyPermitDetail> args)
    {
        SetDetailsModel(args.Model);
        SelectInDataGrid(args.Model);
    }
    
    protected override async void OnDelete(object? sender, DataGridButtonEventArgs<DtoSupplyPermitDetail> args)
    {
        if (!await DialogueServices.ConfirmDeleteAsync())
            return;
        
        Model.SupplyPermitDetails = Model.SupplyPermitDetails.Where(x => x.TemporaryId != args.Model.TemporaryId);
        _ = RefreshAsync(false);
    }
}
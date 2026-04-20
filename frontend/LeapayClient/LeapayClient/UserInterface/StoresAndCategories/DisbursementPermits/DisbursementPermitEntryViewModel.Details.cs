using Fly.Framework.Shared.Common.Interfaces;
using Fly.Shared.Modules.Store.Dtos;
using Leapay.Client.Events.Arguments;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.Services;
using SmartErpWin.Core.Extensions;

namespace Leapay.Client.UserInterface.StoresAndCategories.DisbursementPermits;

public partial class DisbursementPermitEntryViewModel
{
    [ObservableProperty]
    public partial SuperComboVm<DtoDisbursementPermitDetail> Units { get; set; } = new();
    
    [ObservableProperty]
    public partial SuperComboVm<DtoDisbursementPermitDetail> Variances { get; set; } = new();
    
    [ObservableProperty]
    public partial ModelDetailsViewModelBase<DtoDisbursementPermitDetail> ModelDetails { get; set; } = new();
    
    private async void SetDetailsModel(DtoDisbursementPermitDetail? model)
    {
        Reset();
        ModelDetails.SetState(model);
        On(() => ModelDetails.Model.IsNew(),
            () => ModelDetails.Model.ExpireDate = DateTime.Now.AddDays(30));
        
        SelectedItem = InitNewComboModel(ModelDetails.Model.ItemName, ModelDetails.Model.ItemId);
        SetUnits();
        
        /*await Variances.LoadAsync(this, "ItemVariances", ModelDetails.Model.ItemVarianceId,
            s =>
            {
                ModelDetails.Model.ItemVarianceId = s.Id;
                ModelDetails.Model.ItemVarianceName = s.Display;
            });*/
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

    public void UpdateDetailsListing(DtoDisbursementPermitDetail model)
    {
        Model.DisbursementPermitDetails = UpdateListGlobal(model, Model.DisbursementPermitDetails.CastAs<IDetailsModel>())
            .CastAs<DtoDisbursementPermitDetail>();
        
        SetData(Model.DisbursementPermitDetails.ToList());
        
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
    
    private void OnEdit(object? sender, DataGridButtonEventArgs<DtoDisbursementPermitDetail> args)
    {
        SetDetailsModel(args.Model);
        SelectInDataGrid(args.Model);
    }
    
    protected override async void OnDelete(object? sender, DataGridButtonEventArgs<DtoDisbursementPermitDetail> args)
    {
        if (!await DialogueServices.ConfirmDeleteAsync())
            return;
        
        Model.DisbursementPermitDetails = Model.DisbursementPermitDetails.Where(x => x.TemporaryId != args.Model.TemporaryId);
        _ = RefreshAsync(false);
    }
}
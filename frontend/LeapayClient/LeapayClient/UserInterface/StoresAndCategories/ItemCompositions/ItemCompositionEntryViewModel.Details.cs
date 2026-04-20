using Avalonia.Collections;
using Fly.Framework.Shared.Common.Interfaces;
using Fly.Shared.Modules.Store.Dtos;
using Leapay.Client.Events.Arguments;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.Services;
using Leapay.Client.UserInterface.Common.Items.Models;
using SmartErpWin.Core.Extensions;

namespace Leapay.Client.UserInterface.StoresAndCategories.ItemCompositions;

public partial class ItemCompositionEntryViewModel
{
    [ObservableProperty]
    public partial SuperComboVm<DtoItemCompositionDetail> Units { get; set; } = new();
    
    [ObservableProperty]
    public partial AvaloniaList<int> AvailableIndices { get; set; } = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];

    [ObservableProperty]
    public partial int SelectedIndex { get; set; } = 1;
    
    [ObservableProperty]
    public partial SuperComboVm<DtoItemCompositionDetail> Variances { get; set; } = new();
    
    [ObservableProperty]
    public partial ModelDetailsViewModelBase<DtoItemCompositionDetail> ModelDetails { get; set; } = new();
    
    private async void SetDetailsModel(DtoItemCompositionDetail? model)
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
        
        if (ModelDetails.Model?.UnitId.IsNullOrEmpty() ?? true)
        {
            Units.SelectAt(1);
        }

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
        
        if (ModelDetails.Model?.ItemVarianceId?.IsNullOrEmpty() ?? true)
        {
            Variances.SelectAt(0);
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
        try
        {
            if(SelectedItem is not null && !string.IsNullOrEmpty(SelectedItem.Display))
                ModelDetails.Model.Price = decimal.Parse(SelectedItem.Display?.Split('•', StringSplitOptions.TrimEntries)[1]);
        }
        catch { }
        
        base.OnAfterItemIsSelected();
    }

    public void UpdateDetailsListing(DtoItemCompositionDetail model)
    {
        model.CompositionIndex = SelectedIndex;
        Model.Details = UpdateListGlobal(model, Model.Details.CastAs<IDetailsModel>())
            .CastAs<DtoItemCompositionDetail>();
        
        SetDataRespectively();
        CalculatePrice();
    }

    private void SetDataRespectively()
    {
        SetData(Model.Details.Where(i => i.CompositionIndex == SelectedIndex));
        RefreshDataGrid();
    }

    partial void OnSelectedIndexChanged(int value)
    {
        SetDataRespectively();
        SetDetailsModel(null);
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
    
    private void OnEdit(object? sender, DataGridButtonEventArgs<DtoItemCompositionDetail> args)
    {
        SetDetailsModel(args.Model);
        SelectInDataGrid(args.Model);
    }
    
    protected override async void OnDelete(object? sender, DataGridButtonEventArgs<DtoItemCompositionDetail> args)
    {
        if (!await DialogueServices.ConfirmDeleteAsync())
            return;
        
        Model.Details = Model.Details.Where(x => x.TemporaryId != args.Model.TemporaryId);
        _ = RefreshAsync(false);
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
            var @new = new DtoItemCompositionDetail
            {
                UnitId = (model.SelectedUnit ?? new()).UnitId.GetValueOrDefault(),
                UnitName = model.SelectedUnit?.UnitName ?? "",
                ItemId = model.ItemId.GetValueOrDefault(),
                Quantity = model.Quantity,
                Price = model.ItemCustomPrice,
                ItemName = model.ItemName ?? this["ItemWithNoName"]
            };
            
            UpdateDetailsListing(@new);
        }
    }
}
using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Interfaces;
using Fly.Shared.Modules.Factoring.Dtos;
using Fly.Shared.Modules.Store.Dtos;
using Fly.Shared.Modules.Store.Requests;
using Leapay.Client.ClientModels.Dto.DtoModels;
using Leapay.Client.Events.Arguments;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.Polymorphism.Interfaces;
using Leapay.Client.Services;
using Leapay.Client.UserInterface.Common.Items.Models;
using SmartErpWin.Core.Extensions;

namespace Leapay.Client.UserInterface.Factoring.FactoringInvoice;

public partial class FactoringInvoiceEntryViewModel
{
    [ObservableProperty]
    public partial SuperComboVm<DtoFactoringInvoiceDetail> Units { get; set; } = new();
    
    [ObservableProperty]
    public partial ModelDetailsViewModelBase<DtoFactoringInvoiceDetail> ModelDetails { get; set; } = new();
    
    [ObservableProperty]
    public partial SuperComboVm<DtoItemCompositionDetail> CompositionFilter { get; set; } = new();
    
    private IValidationEnforcer DetailsEnforcer { get; set; } = new ValidationEnforceImpl();

    private Identifier _noComposition = Identifier.Create(Guid.Parse("D005B4E4-E865-439B-BA7F-21BEA20E6EC8"));
    private Identifier _all = Identifier.Create(Guid.Parse("8C76F080-184F-4E98-96FD-57B23B15E09F"));
    
    private void SetDetailsModel(DtoFactoringInvoiceDetail? model)
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
                ModelDetails.Model.UnitName = s.Display;
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
    
    public void UpdateDetailsListing(DtoFactoringInvoiceDetail model)
    {
        Model.Details = UpdateListGlobal(model, Model.Details.CastAs<IDetailsModel>())
            .CastAs<DtoFactoringInvoiceDetail>();
        
        SetDataRespectively();

        CalculatePrice();
    }

    private void SetDataRespectively()
    {
        if (CompositionFilter.IsSelected(_all))
        {
            SetData(Model.Details.ToList());
        }
        else if (CompositionFilter.IsSelected(_noComposition))
        {
            SetData(Model.Details.Where(s => s.SourceCompositionId is null).ToList());
        }
        else
        {
            SetData(Model.Details.ToList().Where(d => d.SourceCompositionId.ToString() == CompositionFilter.SelectedItem?.Value));
        }
        
        RefreshDataGrid();
    }

    private void OnEdit(object? sender, DataGridButtonEventArgs<DtoFactoringInvoiceDetail> args)
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
            var @new = new DtoFactoringInvoiceDetail
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
    
    protected override async void OnDelete(object? sender, DataGridButtonEventArgs<DtoFactoringInvoiceDetail> args)
    {
        if (!await DialogueServices.ConfirmDeleteAsync())
            return;
        
        Model.Details.Remove(Model.Details.FirstOrDefault(s => s.TemporaryId == args.Model.TemporaryId));
        _ = RefreshAsync(false);
    }

    private async Task ResyncCompositionAsync(DtoFactoringInvoiceComposition model)
    {
        if (model.CompositionDetailBindingId.IsNullOrEmpty())
        {
            model.CompositionDetailBindingId = Identifier.Create(Guid.NewGuid());
        }
        
        foreach (var raw in Model.Details.ToList().Where(d => d.CompositionDetailBindingId == model.CompositionDetailBindingId))
        {
            Model.Details.Remove(raw);
        }

        var compDetails =
            await GetInnerByQueryAsync<DtoItemComposition>("ItemCompositionDetails",
                new ItemCompositionQueryRequest
                {
                    ItemCompositionId = model.ItemCompositionId
                });

        if (!compDetails.Status)
        {
            Warning("Could not fetch item composition details. {0}", compDetails.ErrorMessage);
            return;
        }

        var byIndex = compDetails.Data?.Details.Where(i => i.CompositionIndex == model.CompositionIndex);

        if (!(byIndex ?? []).Any())
        {
            Model.Compositions.Remove(Model.Compositions.FirstOrDefault(s => s.TemporaryId == model.TemporaryId));
            _ = RefreshAsync(false);
            DialogueServices.ShowMessageAsync("ValidationFailed", "NoItemsInThisCompositionIndex");

            return;
        }
        
        foreach (var detail in byIndex)
        {
            var @new = new DtoFactoringInvoiceDetail
            {
                ItemId = detail.ItemId,
                ItemName = detail.ItemName,
                UnitId = detail.UnitId,
                UnitName = detail.UnitName,
                Price = detail.Price,
                Quantity = detail.Quantity * model.Quantity,
                SourceCompositionId = model.ItemCompositionId,
                SourceCompositionName = model.ItemCompositionName,
                SourceCompositionIndex = model.CompositionIndex,
                CompositionDetailBindingId = model.CompositionDetailBindingId
            };
            
            UpdateDetailsListing(@new);
        }
        
        UpdateCompositionsFilter();
    }

    private void UpdateCompositionsFilter()
    {
        var oldSelected = CompositionFilter.SelectedItem?.Value;
        
        CompositionFilter.Reset();

        var models = Model.Details.DistinctBy(c => c.SourceCompositionId)
            .Select(c => new ComboModel
            {
                Display = c.SourceCompositionName,
                Value = c.SourceCompositionId.ToString()

            }).ToList();
        models.Insert(0, new ComboModel
        {
            Display = this["All"],
            Value = _all.ToString()
        });
        
        models.Insert(1, new ComboModel
        {
            Display = this["NoComposition"],
            Value = _noComposition.ToString()
        });
        
        CompositionFilter.Load(models, Identifier.Parse(oldSelected ?? _all.Value.ToString()), insertDefault: false,
            onSelectionChanged: _ => SetDataRespectively());
    }
}
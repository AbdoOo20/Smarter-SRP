using Avalonia.Collections;
using Fly.Framework.Shared.Common.Interfaces;
using Fly.Shared.Modules.Factoring.Dtos;
using Leapay.Client.Events.Arguments;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.Polymorphism.Interfaces;
using Leapay.Client.Services;
using SmartErpWin.Core.Extensions;

namespace Leapay.Client.UserInterface.Factoring.FactoringInvoice;

public partial class FactoringInvoiceEntryViewModel
{
    [ObservableProperty]
    public partial ModelDetailsViewModelBase<DtoFactoringInvoiceComposition> ModelCompositions { get; set; } = new();
    
    private IValidationEnforcer CompositionsEnforcer { get; set; } = new ValidationEnforceImpl();

    [ObservableProperty]
    public partial AvaloniaList<int> AvailableIndices { get; set; } = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
    
    private void SetCompositionsModel(DtoFactoringInvoiceComposition? model)
    {
        ResetComposition();
        ModelCompositions.SetState(model);
        SelectedItemComposition = InitNewComboModel(ModelCompositions.Model.ItemCompositionName,
            ModelCompositions.Model.ItemCompositionId.GetValueOrDefault());
    }
    
    [RelayCommand]
    private void OnCompositionSubmit()
    {
        if(!VerifyValidations(CompositionsEnforcer.Enforce(ModelCompositions.Model)))
            return;
        
        UpdateCompositionsListing(ModelCompositions.Model);
        ResetComposition(true);
    }

    private void ResetComposition(bool andSet = false)
    {
        ModelCompositions = new();

        if (andSet)
        {
            SetCompositionsModel(null);
        }
    }

    protected override void OnAfterItemCompositionIsSelected()
    {
        OnSelectedCompositionItemChanged();
        try
        {
            if(ModelCompositions.Model.Price == 0 && SelectedItemComposition is not null && !string.IsNullOrEmpty(SelectedItemComposition.Display))
                ModelCompositions.Model.Price = decimal.Parse(SelectedItemComposition.Display?.Split('•', StringSplitOptions.TrimEntries)[1]);
        }
        catch { }
        base.OnAfterItemCompositionIsSelected();
    }
    
    private void OnSelectedCompositionItemChanged()
    {
        if (string.IsNullOrEmpty(SelectedItemComposition?.Value))
        {
            ModelCompositions.Model.ItemCompositionId = default;
            return;
        }
        
        if (Guid.TryParse(SelectedItemComposition.Value, out var id))
        {
            ModelCompositions.Model.ItemCompositionId = id;
            ModelCompositions.Model.ItemCompositionName = SelectedItemComposition.Display;
        }
        
        RefreshSelectedItemCompositionStatus();
    }
    
    public async void UpdateCompositionsListing(DtoFactoringInvoiceComposition model)
    {
        Model.Compositions = UpdateListGlobal(model, Model.Compositions.CastAs<IDetailsModel>())
            .CastAs<DtoFactoringInvoiceComposition>();
        
        Compositions.SetData(Model.Compositions.ToList());
        
        Compositions.PublicRefreshDataGrid();
        await ResyncCompositionAsync(model);
        CalculatePrice();
    }
    
    private void OnCompositionEdit(object? sender, DataGridButtonEventArgs<DtoFactoringInvoiceComposition> args)
    {
        SetCompositionsModel(args.Model);
        Compositions.SelectInDataGrid(args.Model);
    }
    
    protected async void OnCompositionDelete(object? sender, DataGridButtonEventArgs<DtoFactoringInvoiceComposition> args)
    {
        if (!await DialogueServices.ConfirmDeleteAsync())
            return;
        
        Model.Compositions.Remove(Model.Compositions.FirstOrDefault(s => s.TemporaryId == args.Model.TemporaryId));

        foreach (var relatedDetail in Model.Details.ToList().Where(i =>
                     i.CompositionDetailBindingId == args.Model.CompositionDetailBindingId))
        {
            Model.Details.Remove(relatedDetail);
        }
        
        _ = RefreshAsync(false);
    }
}
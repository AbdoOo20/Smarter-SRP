using Fly.Framework.Shared.Common.Interfaces;
using Fly.Shared.Modules.Maintenance.Dtos;
using Leapay.Client.Events.Arguments;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.Polymorphism.Interfaces;
using Leapay.Client.Services;
using SmartErpWin.Core.Extensions;

namespace Leapay.Client.UserInterface.Maintenance.MaintenanceInvoice;

public partial class MaintenanceInvoiceEntryViewModel
{
    [ObservableProperty]
    public partial ModelDetailsViewModelBase<DtoMaintenanceInvoiceCost> ModelCosts { get; set; } = new();
    
    private IValidationEnforcer CostsEnforcer { get; set; } = new ValidationEnforceImpl();

    private void SetCostsModel(DtoMaintenanceInvoiceCost? model)
    {
        ResetCost();
        ModelCosts.SetState(model);
    }

    [RelayCommand]
    private void OnCostSubmit()
    {
        if(!VerifyValidations(CostsEnforcer.Enforce(ModelCosts.Model)))
            return;
        
        UpdateCostsListing(ModelCosts.Model);
        ResetCost(true);
    }

    private void ResetCost(bool andSet = false)
    {
        ModelCosts = new();

        if (andSet)
        {
            SetCostsModel(null);
        }
    }
    
    public void UpdateCostsListing(DtoMaintenanceInvoiceCost model)
    {
        Model.Costs = UpdateListGlobal(model, Model.Costs.CastAs<IDetailsModel>())
            .CastAs<DtoMaintenanceInvoiceCost>();
        Costs.SetData(Model.Costs.ToList());
        
        Costs.PublicRefreshDataGrid();
        CalculatePrice();
    }
    
    private void OnCostsEdit(object? sender, DataGridButtonEventArgs<DtoMaintenanceInvoiceCost> args)
    {
        SetCostsModel(args.Model);
        Costs.SelectInDataGrid(args.Model);
    }
    
    protected async void OnCostsDelete(object? sender, DataGridButtonEventArgs<DtoMaintenanceInvoiceCost> args)
    {
        if (!await DialogueServices.ConfirmDeleteAsync())
            return;

        Model.Costs.Remove(Model.Costs.FirstOrDefault(s => s.TemporaryId == args.Model.TemporaryId));
        _ = RefreshAsync(false);
    }
}
using Fly.Framework.Shared.Common.Interfaces;
using Fly.Shared.Modules.PurchaseAndSales.Dtos;
using Leapay.Client.Events.Arguments;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.Services;
using SmartErpWin.Core.Extensions;

namespace Leapay.Client.UserInterface.Purchases.PurchasesInvoice;

public partial class PurchaseInvoiceEntryViewModel
{
    [ObservableProperty]
    public partial ModelDetailsViewModelBase<DtoPurchaseInvoiceCost> ModelCosts { get; set; } = new();
    
    private void SetCostsModel(DtoPurchaseInvoiceCost? model)
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
    
    public void UpdateCostsListing(DtoPurchaseInvoiceCost model)
    {
        Model.Costs = UpdateListGlobal(model, Model.Costs.CastAs<IDetailsModel>())
            .CastAs<DtoPurchaseInvoiceCost>();
        Costs.SetData(Model.Costs.ToList());
        
        Costs.PublicRefreshDataGrid();
        CalculatePrice();
    }
    
    private void OnCostsEdit(object? sender, DataGridButtonEventArgs<DtoPurchaseInvoiceCost> args)
    {
        SetCostsModel(args.Model);
        Costs.SelectInDataGrid(args.Model);
    }
    
    protected async void OnCostsDelete(object? sender, DataGridButtonEventArgs<DtoPurchaseInvoiceCost> args)
    {
        if (!await DialogueServices.ConfirmDeleteAsync())
            return;

        Model.Costs.RemoveAll(m => m.TemporaryId == args.Model.TemporaryId);
        _ = RefreshAsync(false);
    }
}
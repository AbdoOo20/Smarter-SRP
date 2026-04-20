using Fly.Framework.Shared.Common;
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
    public partial ModelDetailsViewModelBase<CreateUpdateItemBarcodeRequest> ModelBarcodes { get; set; } = new();
    
    private async void SetBarcodesModel(CreateUpdateItemBarcodeRequest? model)
    {
        if (IsSilent)
            return;
        
        ResetBarcodes();
        
        ModelBarcodes.SetState(model);
    }
    
    [RelayCommand]
    private void OnSubmitBarcode()
    {
        if(!VerifyValidations(BarcodesEnforcer.Enforce(ModelBarcodes.Model)))
            return;
        
        UpdateBarcodesListing(ModelBarcodes.Model);
        ResetBarcodes(true);
    }

    private void ResetBarcodes(bool andSet = false)
    {
        ModelBarcodes = new();

        if (andSet)
        {
            SetBarcodesModel(null);
        }
    }

    public void UpdateBarcodesListing(CreateUpdateItemBarcodeRequest model)
    {
        Model.ItemBarcodes = UpdateListGlobal(model, Model.ItemBarcodes.CastAs<IDetailsModel>())
            .CastAs<CreateUpdateItemBarcodeRequest>();
        
        Barcodes.SetData(Model.ItemBarcodes.ToList());
        
        Barcodes.PublicRefreshDataGrid();
    }
    
    private void OnBarcodeEdit(object? sender, DataGridButtonEventArgs<CreateUpdateItemBarcodeRequest> args)
    {
        SetBarcodesModel(args.Model);
        Barcodes.SelectInDataGrid(args.Model);
    }
    
    protected async void OnBarcodeDelete(object? sender, DataGridButtonEventArgs<CreateUpdateItemBarcodeRequest> args)
    {
        if (!await DialogueServices.ConfirmDeleteAsync())
            return;
        
        Model.ItemBarcodes = Model.ItemBarcodes.Where(x => x.TemporaryId != args.Model.TemporaryId);
        _ = RefreshAsync(false);
    }
}
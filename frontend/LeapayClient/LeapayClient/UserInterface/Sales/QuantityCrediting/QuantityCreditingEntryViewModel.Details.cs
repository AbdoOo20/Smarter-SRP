using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Interfaces;
using Fly.Framework.Shared.Extentions;
using Fly.Shared.Modules.PurchaseAndSales.Dtos;
using Fly.Shared.Modules.PurchaseAndSales.Requests;
using Fly.Shared.Modules.Store.Requests;
using Leapay.Client.Events.Arguments;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.Services;
using SmartErpWin.Core.Extensions;

namespace Leapay.Client.UserInterface.Sales.QuantityCrediting;

public partial class QuantityCreditingEntryViewModel
{
    [ObservableProperty]
    public partial SuperComboVm<DtoQuantityCreditingInvoiceDetail> Units { get; set; } = new();
    
    [ObservableProperty]
    public partial ModelDetailsViewModelBase<DtoQuantityCreditingInvoiceDetail> ModelDetails { get; set; } = new();

    [ObservableProperty]
    public partial decimal SuggestedPrice { get; set; }

    private void SetDetailsModel(DtoQuantityCreditingInvoiceDetail? model)
    {
        Reset();
        ModelDetails.SetState(model);
        SelectedItem = InitNewComboModel(ModelDetails.Model.ItemName, ModelDetails.Model.ItemId);
        SetUnits();
        SetPrice();
        SetExpireDate();
    }
    
    [RelayCommand]
    private void OnSubmit()
    {
        ModelDetails.Model.Price = SuggestedPrice;
        
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
    
    public async void SetExpireDate()
    {
        var storeId = Model.StoreId;
        var storeIdValue = storeId.GetValueOrDefault();
        var itemId = SelectedItem.Id;

        if (storeId == Identifier.Empty || itemId == Identifier.Empty)
            return;
        
        await RequestCombosUsingQueryRequestAsync("ExpireDate", 2, new ExpireDateQueryRequest
        {
            ItemId = itemId,
            StoreId = storeIdValue
        });
        
        ComboModelBase.SetComboModelOrDefault(null, 2);
    }
    
    public async void SetPrice()
    {
        SuggestedPrice = ModelDetails.Model.Price;
        
        if (ModelDetails.Model?.Price != 0)
            return;

        var storeId = Model?.StoreId;
        
        if (storeId.IsNullOrEmpty() || SelectedItem is null
                                    || SelectedItem.Id.IsNullOrEmpty()
                                    || ModelDetails.Model.UnitId.IsNullOrEmpty())
            return;
        
        var itemInfoQuery = new GetItemInfoQueryRequest
        {
            StoreId = storeId.GetValueOrDefault(),
            UnitId = ModelDetails.Model.UnitId,
            ItemId = SelectedItem.Id
        };

        var req = await GetInnerByQueryAsync<ItemInfoQueryResponse>("ItemInfo", itemInfoQuery);

        if (req.Status)
        {
            SuggestedPrice = req.Data!.Price;
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
    
    public void UpdateDetailsListing(DtoQuantityCreditingInvoiceDetail model)
    {
        Model.Details = UpdateListGlobal(model, Model.Details.CastAs<IDetailsModel>())
            .CastAs<DtoQuantityCreditingInvoiceDetail>();
        
        SetData(Model.Details.ToList());
        
        RefreshDataGrid();
        CalculatePrice();
    }
    
    private void OnEdit(object? sender, DataGridButtonEventArgs<DtoQuantityCreditingInvoiceDetail> args)
    {
        SetDetailsModel(args.Model);
        SelectInDataGrid(args.Model);
    }
    
    protected override async void OnDelete(object? sender, DataGridButtonEventArgs<DtoQuantityCreditingInvoiceDetail> args)
    {
        if (!await DialogueServices.ConfirmDeleteAsync())
            return;
        
        Model.Details.Remove(Model.Details.FirstOrDefault(s => s.TemporaryId == args.Model.TemporaryId));
        _ = RefreshAsync(false);
    }
}
using Fly.Framework.Shared.Common;
using SmartiErpWin.Client.DtoModels.PurchasesModels;

namespace SmartiErpWin.Client.UserInterface.Purchases.PurchaseInvoices;

public class PurchasesInvoiceDetailsEntryViewModel : BaseDetailsEntryViewModel<DtoPurchasesInvoiceDetailsModel>
{
    protected override string Endpoint => "Purchase";
    
    public PurchasesInvoiceDetailsEntryViewModel(
        LocalisationService locale,
        HttpClient client,
        Type pageType): base(client)
    {
        PageType = pageType;
        
        InitBaseModel(locale, null, null);
        ApplicationTitle = GetPageName(pageType, EntryType);

        SetState(null);
    }

    protected internal sealed override async void Refresh()
    {
        await RequestApiComboItemsFromInnerEndpointAsync("Items");
        ComboModelBase.SetComboModelOrDefault(Model?.ItemId);
    }
    
    public async void SetUnits()
    {
        var selectedItem = ComboModelBase.GetSelectedItemId();

        if (selectedItem == Identifier.Empty)
            return;
        
        await RequestApiComboItemsFromInnerEndpointAsync("Units", 1, selectedItem.ToString());
        ComboModelBase.SetComboModelOrDefault(Model?.UnitId, 1);
    }

    protected override void PreSaveActions()
    {
        var model = GetWorkingModel();
        
        model.UnitId = ComboModelBase.GetSelectedItemId(1);
        model.UnitName = ComboModelBase.SelectedApiComboItems[1]?.Display;
        model.ItemId = ComboModelBase.GetSelectedItemId();
        model.ItemName = ComboModelBase.SelectedApiComboItems[0]?.Display;
        if (model.ItemName.Contains("•"))
        {
            var nnn = model.ItemName.Split("•");
            model.ItemBarcode = nnn[1];
            model.ItemName = nnn[0];
        }
    }

    protected override async Task SaveAsync()
    {
        var model = GetWorkingModel();

        if (model is null)
            return;
        
        PreSaveActions();
        
        SynchroniseTypeBase(PageType, model);

        ComboModelBase.SelectAtOnePoint(0);
        ComboModelBase.SetApiComboItems([], 2);
        _actionClear?.Invoke();

        SetState(null);
    }


    private Action? _actionClear;
    public void OnClear(Action actionClear)
    {
        _actionClear = actionClear;
    }

    public sealed override Task OnSynchronisationRequestedAsync(params object[] args)
    {
        if (args.SingleOrDefault() is DtoPurchasesInvoiceDetailsModel model)
        {
            SetState(model);
        }

        return Task.CompletedTask;
    }
}
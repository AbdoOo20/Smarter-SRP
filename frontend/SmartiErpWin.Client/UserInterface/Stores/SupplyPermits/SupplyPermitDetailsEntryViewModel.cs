using Fly.Framework.Shared.Common;
using SmartiErpWin.Client.DtoModels.StoresCategoriesModels;

namespace SmartiErpWin.Client.UserInterface.Stores.SupplyPermits;

public partial class SupplyPermitDetailsEntryViewModel : BaseDetailsEntryViewModel<DtoSupplyPermitDetailModel>
{
    protected override string Endpoint => "SupplyPermit";
    
    public SupplyPermitDetailsEntryViewModel(
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
        await RequestApiComboItemsFromInnerEndpointAsync("item");
        ComboModelBase.SetComboModelOrDefault(Model?.ItemId);
    }

    public async void SetUnits()
    {
        var selectedItem = ComboModelBase.GetSelectedItemId();

        if (selectedItem == Identifier.Empty)
            return;
        
        await RequestApiComboItemsFromInnerEndpointAsync("units", 1, selectedItem.ToString());
        ComboModelBase.SetComboModelOrDefault(Model?.UnitId, 1);
    }

    protected override void PreSaveActions()
    {
        Model.UnitId = ComboModelBase.GetSelectedItemId(1);
        Model.UnitName = ComboModelBase.SelectedApiComboItems[1]?.Display;
        Model.ItemId = ComboModelBase.GetSelectedItemId();
        Model.ItemName = ComboModelBase.SelectedApiComboItems[0]?.Display;
    }

    protected override async Task SaveAsync()
    {
        var model = GetWorkingModel();
        
        if (Model is null)
            return;
        
        PreSaveActions();

        SynchroniseTypeBase(PageType, model);
        
        SetState(null);
    }

    public override async Task OnSynchronisationRequestedAsync(params object[] args)
    {
        if (args.SingleOrDefault() is DtoSupplyPermitDetailModel model)
        {
            SetState(model);
        }
    }
}
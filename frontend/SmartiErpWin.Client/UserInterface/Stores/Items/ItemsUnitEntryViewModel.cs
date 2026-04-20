using SmartErpWin.Core;
using SmartiErpWin.Client.DtoModels.StoresCategoriesModels;

namespace SmartiErpWin.Client.UserInterface.Stores.Items;

public partial class ItemsUnitEntryViewModel : BaseDetailsEntryViewModel<DtoItemUnitModel>
{
    protected override string Endpoint => "Item";
    
    public ItemsUnitEntryViewModel(
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
        await PrepareComboBoxAsync("units", Model?.UnitId);
    }
    
    protected override void PreSaveActions()
    {
        Model.UnitId = ComboModelBase.GetSelectedItemId();
        Model.UnitName = ComboModelBase.GetSelectedItemDisplay();
    }

    protected override Task SaveAsync()
    {
        var model = GetWorkingModel();
        
        if (model is null)
            return Task.CompletedTask;

        PreSaveActions();

        SynchroniseTypeBase(PageType, model);
        
        return Task.CompletedTask;
    }

    public override Task OnSynchronisationRequestedAsync(params object[] args)
    {
        switch (args.FirstOrDefault())
        {
            case DtoItemUnitModel model:
                SetState(model);
                break;
            case CodeBook.ResState:
                SetState(null);
                break;
        }

        return Task.CompletedTask;
    }
    
    protected override bool PreSaveChecks()
    {
        //exclude the current item from checking
        return true;
    }
}
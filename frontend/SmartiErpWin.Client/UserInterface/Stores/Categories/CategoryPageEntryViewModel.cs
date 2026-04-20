using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.Store.Dtos;

namespace SmartiErpWin.Client.UserInterface.Stores.Categories;

public class CategoryPageEntryViewModel : BaseEntryViewModel<DtoCategory>
{
    protected override string Endpoint => "Category";
    
    public CategoryPageEntryViewModel(Window window,
        LocalisationService locale,
        DtoCategory? model,
        EntryType entryType,
        HttpClient client,
        Type pageType): base(client)
    {
        Model = model ?? new();
        EntryWindow = window;
        EntryType = entryType;
        PageType = pageType;
        InitBaseModel(locale, null, null);
        ApplicationTitle = GetPageName(pageType, EntryType);
        
        Refresh();
    }

    private async void Refresh()
    {
        await RequestApiComboItemsFromInnerEndpointAsync("categuries");

        ComboModelBase.SetComboModelOrDefault(Model.CateguryId);
    }

    protected override void PreSaveActions()
    {
        Identifier? selected = ComboModelBase.GetSelectedItemId();
        selected = selected == Identifier.Empty ? null : selected;
        
        Model.CateguryId = selected;
    }
}
using Fly.Shared.Modules.Store.Dtos;

namespace SmartiErpWin.Client.UserInterface.Stores.Suppliers;

public class SupplierPageEntryViewModel : BaseEntryViewModel<DtoSupplier>
{
    protected override string Endpoint => "Supplier";
    
    public SupplierPageEntryViewModel(Window window,
        LocalisationService locale,
        DtoSupplier? model,
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

    }

    protected override void PreSaveActions()
    {
        
    }
}
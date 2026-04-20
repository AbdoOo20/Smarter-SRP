using Fly.Shared.Modules.PurchaseAndSales.Dtos.Misc;

namespace SmartiErpWin.Client.UserInterface.Sales.Banks;

[Obsolete("Using a generic approach")]
public class BankPageEntryViewModel : BaseEntryViewModel<DtoBank>
{
    public BankPageEntryViewModel(Window window,
        LocalisationService locale,
        DtoBank? model,
        EntryType entryType, HttpClient client): base(client)
    {
        EntryType = entryType;
        EntryWindow = window;
        Model = model ?? new();
        
        InitBaseModel(locale, null, null);
    }
}
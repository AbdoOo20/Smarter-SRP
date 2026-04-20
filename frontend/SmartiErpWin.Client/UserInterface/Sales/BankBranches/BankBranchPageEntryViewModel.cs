using Fly.Shared.Modules.PurchaseAndSales.Dtos.Misc;

namespace SmartiErpWin.Client.UserInterface.Sales.BankBranches;

public class BankBranchPageEntryViewModel : BaseEntryViewModel<DtoBankBranch>
{
    protected override string Endpoint => "BankBranch";

    public BankBranchPageEntryViewModel(Window window,
        LocalisationService locale,
        DtoBankBranch? model,
        EntryType entryType, HttpClient client, Type pageType): base(client)
    {        
        EntryType = entryType;
        EntryWindow = window;
        Model = model ?? new();
        PageType = pageType;
        
        InitBaseModel(locale, null, null);
        ApplicationTitle = GetPageName(pageType, EntryType);

        Refresh();
    }

    private async void Refresh()
    {
        await RequestApiComboItemsFromInnerEndpointAsync("banks");
        
        ComboModelBase.SetComboModelOrDefault(Model.BankId);
    }
    
    protected override void PreSaveActions()
    {
        Model.BankId = ComboModelBase.GetSelectedItemId();
    }
}
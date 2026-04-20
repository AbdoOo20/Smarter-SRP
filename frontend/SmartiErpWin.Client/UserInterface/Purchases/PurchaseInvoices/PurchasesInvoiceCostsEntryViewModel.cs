using SmartiErpWin.Client.DtoModels.PurchasesModels;

namespace SmartiErpWin.Client.UserInterface.Purchases.PurchaseInvoices;

public class PurchasesInvoiceCostsEntryViewModel : BaseDetailsEntryViewModel<DtoPurchasesInvoiceCostsModel>
{
    protected override string Endpoint => "Purchase";
    
    public PurchasesInvoiceCostsEntryViewModel(
        LocalisationService locale,
        HttpClient client,
        Type pageType): base(client)
    {
        PageType = pageType;
        
        InitBaseModel(locale, null, null);
        ApplicationTitle = GetPageName(pageType, EntryType);
        
        SetState(null);
    }
    
    protected override Task SaveAsync()
    {
        var model = GetWorkingModel();

        if (model is null)
            return Task.CompletedTask;
        
        PreSaveActions();
        
        SynchroniseTypeBase(PageType, model);
        
        SetState(null);
        
        return Task.CompletedTask;
    }

    public sealed override async Task OnSynchronisationRequestedAsync(params object[] args)
    {
        if (args.SingleOrDefault() is DtoPurchasesInvoiceCostsModel model)
        {
            SetState(model);
        }
    }
}
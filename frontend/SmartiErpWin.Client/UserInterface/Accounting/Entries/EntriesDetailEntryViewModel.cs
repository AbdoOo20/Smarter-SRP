using SmartiErpWin.Client.DtoModels.AccountingModels;

namespace SmartiErpWin.Client.UserInterface.Accounting.Entries;

public partial class EntriesDetailPageEntryViewModel : BaseDetailsEntryViewModel<DtoEntryDetailModel>
{
    protected override string Endpoint => "Entry";
    
    public EntriesDetailPageEntryViewModel(
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
        await PrepareComboBoxAsync("Accounts", Model?.AccountId);
    }

    protected override void PreSaveActions()
    {
        Model.AccountId = ComboModelBase.GetSelectedItemId();
        Model.AccountName = ComboModelBase.GetSelectedItemDisplay();
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

    public override async Task OnSynchronisationRequestedAsync(params object[] args)
    {
        if (args.SingleOrDefault() is DtoEntryDetailModel model)
        {
            SetState(model);
        }
    }
}
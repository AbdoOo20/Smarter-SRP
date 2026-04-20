using Fly.Shared.Modules.Accounting.Dtos;

namespace SmartiErpWin.Client.UserInterface.Accounting.AccountingTree;

public sealed class AccountingTreePageEntryViewModel : BaseEntryViewModel<DtoAccount>
{
    protected override string Endpoint => "Account";
    
    public AccountingTreePageEntryViewModel(Window window,
        LocalisationService locale,
        DtoAccount? model,
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
        SetStaticCombos(Model.Natural);
        await PrepareComboBoxAsync("accounts", Model.ParentId);
    }

    protected override void PreSaveActions()
    {
        Model.Natural = ComboModelBase.ConvertToEnum<AccountType>(ComboModelBase.SelectedStaticComboItem);
        Model.ParentId = ComboModelBase.GetSelectedItemId();
    }
}
using Fly.Shared.Modules.Financial.Dtos;
using Fly.Shared.Modules.Financial.Enums;
using SmartErpWin.Core.Helpers;

namespace SmartiErpWin.Client.UserInterface.Reports.AccountStatementReport;

public class AccountStatementReportPageEntryViewModel : BaseEntryViewModel<DtoStatment>
{
    protected override string Endpoint => "Statement";
    
    public AccountStatementReportPageEntryViewModel(Window window,
        LocalisationService locale,
        DtoStatment? model,
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

        TimeHelper.SetForStatement(EntryType, out var start, out var end);
        
        Model.StartDate = start;
        Model.EndDate = end;
        
        Refresh();
    }

    private void Refresh()
    {
        ComboModelBase.SetStaticItemsInternal<StatementAccountType>(LocaleService);
        ComboModelBase.SetStaticComboBoxItemIndexInternal(0);
    }

    public async void SetAccounts()
    {
        var pos = ComboModelBase.GetStaticPosition().ToString();
        
        await PrepareComboBoxAsync("Account",
            Model.AccountId, 0,
            pos);
    }
    
    protected override void PreSaveActions()
    {
        Model.AccountId = ComboModelBase.GetSelectedItemId();
    }

    protected override async Task SaveAsync()
    {
        PreSaveActions();

        var data = (await PostSaveReportAsync(Model)).Data;

        SynchroniseTypeBase<AccountStatementReportPageViewModel>(data);
        EntryWindow?.Close();
    }
}
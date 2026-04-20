using Fly.Shared.Modules.Financial.Dtos;
using SmartiErpWin.Client.ViewModels.Windows;
using Wpf.Ui;

namespace SmartiErpWin.Client.UserInterface.Reports.AccountStatementReport;

public partial class AccountStatementReportPageViewModel : BasePageViewModel<DtoStatment>
{
    protected override string Endpoint => "Statement";

    protected override bool ShowError => false;
    
    private readonly MainWindowViewModel _mainViewModel;
    
    public AccountStatementReportPageViewModel(HttpClient client,
        IContentDialogService contentDialogService,
        SessionService sessionService,
        LocalisationService localisationService,
        MainWindowViewModel mainViewModel): base(client)
    {
        _mainViewModel = mainViewModel;
        InitBaseModel(localisationService, sessionService, contentDialogService);
    }

    [RelayCommand]
    private void OnBack()
    {
        _mainViewModel.GoBack();
    }
    
    [RelayCommand]
    private void OnAdd()
    {
        var entry = new AccountStatementReportEntryWindow(LocaleService, null,
            EntryType.Add, HttpClient, GetType());
        
        entry.ShowDialog();
    }

    public sealed override async Task OnSynchronisationRequestedAsync(params object[] args)
    {
        if (args.SingleOrDefault() is List<DtoStatment> data)
        {
            await RefreshAsync(true, data);
        }
    }
}
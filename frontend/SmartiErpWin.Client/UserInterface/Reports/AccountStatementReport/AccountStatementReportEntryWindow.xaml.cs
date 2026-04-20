using System.Windows.Controls;
using Fly.Shared.Modules.Financial.Dtos;

namespace SmartiErpWin.Client.UserInterface.Reports.AccountStatementReport;

public partial class AccountStatementReportEntryWindow
{
    public AccountStatementReportPageEntryViewModel ViewModel { get; }
    
    public AccountStatementReportEntryWindow(LocalisationService localisationService,
        DtoStatment model, EntryType type, HttpClient client, Type pageType)
    {
        ViewModel = new AccountStatementReportPageEntryViewModel(this,
            localisationService, model, type, client, pageType);
        
        InitializeComponent();
        DataContext = this;
    }

    private void SelectorOnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ViewModel.SetAccounts();
    }
}
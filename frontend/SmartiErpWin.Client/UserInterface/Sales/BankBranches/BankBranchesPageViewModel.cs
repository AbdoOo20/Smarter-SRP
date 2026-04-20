using Fly.Shared.Modules.PurchaseAndSales.Dtos.Misc;
using Wpf.Ui;

namespace SmartiErpWin.Client.UserInterface.Sales.BankBranches;

public partial class BankBranchesPageViewModel : BasePageViewModel<DtoBankBranch>
{
    protected override string Endpoint => "BankBranch";

    public BankBranchesPageViewModel(HttpClient client, IContentDialogService contentDialogService,
        SessionService sessionService,
        LocalisationService localisationService): base(client)
    {
        InitBaseModel(localisationService, sessionService, contentDialogService);
        
        OnItemEdit += OnEdit;
        OnItemDelete += OnDelete;
    }

    private void OnEdit(object? sender, EventArgs args)
    {
        var model = DataGridServices.FindModel<DtoBankBranch>(sender);

        if (model is null)
            return;
        
        var entry = new BankBranchEntryWindow(LocaleService, model,
            EntryType.Edit, HttpClient, GetType());
        
        entry.ShowDialog();
    }

    [RelayCommand]
    private void OnAdd()
    {
        var entry = new BankBranchEntryWindow(LocaleService, null,
            EntryType.Add, HttpClient, GetType());
        
        entry.ShowDialog();
    }
}
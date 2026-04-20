using Fly.Shared.Modules.Accounting.Dtos;
using Wpf.Ui;

namespace SmartiErpWin.Client.UserInterface.Accounting.ExpenseTypes;

public partial class ExpenseTypesPageViewModel : BasePageViewModel<DtoExpenseTypes>
{
    protected override string Endpoint => "ExpenseTypes";
    
    public ExpenseTypesPageViewModel(HttpClient client,
        IContentDialogService contentDialogService,
        SessionService sessionService,
        LocalisationService localisationService): base(client)
    {
        InitBaseModel(localisationService, sessionService, contentDialogService);
        
        OnItemEdit += OnEdit;
        OnItemDelete += OnDelete;
    }

    private void OnEdit(object? sender, EventArgs args)
    {
        var model = DataGridServices.FindModel<DtoExpenseTypes>(sender);

        if (model is null)
            return;
        
        var entry = new ExpenseTypesEntryWindow(LocaleService, model,
            EntryType.Edit, HttpClient, GetType());
        
        entry.ShowDialog();
    }
    
    [RelayCommand]
    private void OnAdd()
    {
        var entry = new ExpenseTypesEntryWindow(LocaleService, null,
            EntryType.Add, HttpClient, GetType());
        
        entry.ShowDialog();
    }
}
using Fly.Shared.Modules.Store.Dtos;
using Wpf.Ui;

namespace SmartiErpWin.Client.UserInterface.Stores.Units;

public partial class UnitsPageViewModel : BasePageViewModel<DtoUnit>
{
    protected override string Endpoint => "Unit";
    
    public UnitsPageViewModel(HttpClient client,
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
        var model = DataGridServices.FindModel<DtoUnit>(sender);

        if (model is null)
            return;
        
        var entry = new UnitEntryWindow(LocaleService, model,
            EntryType.Edit, HttpClient, GetType());
        
        entry.ShowDialog();
    }
    
    [RelayCommand]
    private void OnAdd()
    {
        var entry = new UnitEntryWindow(LocaleService, null,
            EntryType.Add, HttpClient, GetType());
        
        entry.ShowDialog();
    }
}
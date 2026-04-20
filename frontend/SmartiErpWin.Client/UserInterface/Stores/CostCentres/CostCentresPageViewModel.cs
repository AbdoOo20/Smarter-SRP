using Fly.Shared.Modules.Store.Dtos;
using Wpf.Ui;

namespace SmartiErpWin.Client.UserInterface.Stores.CostCentres;

public partial class CostCentresPageViewModel : BasePageViewModel<DtoCostCenter>
{
    protected override string Endpoint => "CostCenter";
    
    public CostCentresPageViewModel(HttpClient client,
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
        var model = DataGridServices.FindModel<DtoCostCenter>(sender);

        if (model is null)
            return;
        
        var entry = new CostCentreEntryWindow(LocaleService, model,
            EntryType.Edit, HttpClient, GetType());
        
        entry.ShowDialog();
    }
    
    [RelayCommand]
    private void OnAdd()
    {
        var entry = new CostCentreEntryWindow(LocaleService, null,
            EntryType.Add, HttpClient, GetType());
        
        entry.ShowDialog();
    }
}
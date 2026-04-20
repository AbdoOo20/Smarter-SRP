using Fly.Shared.Modules.Marketers.Dtos;
using Wpf.Ui;

namespace SmartiErpWin.Client.UserInterface.Marketing.MarketerEntry;

public sealed partial class MarketerInfoPageViewModel : BasePageViewModel<DtoMarketer>
{
    protected override string Endpoint => "Marketer";
    
    public MarketerInfoPageViewModel(HttpClient client,
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
        var model = DataGridServices.FindModel<DtoMarketer>(sender);

        if (model is null)
            return;
        
        var entry = new MarketerEntryWindow(LocaleService, model,
            EntryType.Edit, HttpClient, GetType());
        
        entry.ShowDialog();
    }
    
    [RelayCommand]
    private void OnAdd()
    {
        var entry = new MarketerEntryWindow(LocaleService, null,
            EntryType.Add, HttpClient, GetType());
        
        entry.ShowDialog();
    }
}
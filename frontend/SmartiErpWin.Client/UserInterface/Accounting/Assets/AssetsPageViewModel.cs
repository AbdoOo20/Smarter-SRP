using Fly.Shared.Modules.Accounting.Dtos;
using Wpf.Ui;

namespace SmartiErpWin.Client.UserInterface.Accounting.Assets;

public partial class AssetsPageViewModel : BasePageViewModel<DtoAssets>
{
    protected override string Endpoint => "Assets";

    public AssetsPageViewModel (HttpClient client,
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
        var model = DataGridServices.FindModel<DtoAssets>(sender);

        if (model is null)
            return;
        
        var entry = new AssetEntryWindow(LocaleService, model,
            EntryType.Edit, HttpClient, GetType());
        
        entry.ShowDialog();
    }

    [RelayCommand]
    private void OnAdd()
    {
        var entry = new AssetEntryWindow(LocaleService, null,
            EntryType.Add, HttpClient, GetType());
        
        entry.ShowDialog();
    }
}
using Fly.Shared.Modules.Store.Dtos;
using Wpf.Ui;

namespace SmartiErpWin.Client.UserInterface.Stores.Categories;

public partial class CategoriesPageViewModel : BasePageViewModel<DtoCategory>
{
    protected override string Endpoint => "Category";
    
    public CategoriesPageViewModel(HttpClient client,
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
        var model = DataGridServices.FindModel<DtoCategory>(sender);

        if (model is null)
            return;
        
        var entry = new CategoryEntryWindow(LocaleService, model,
            EntryType.Edit, HttpClient, GetType());
        
        entry.ShowDialog();
    }
    
    [RelayCommand]
    private void OnAdd()
    {
        var entry = new CategoryEntryWindow(LocaleService, null,
            EntryType.Add, HttpClient, GetType());
        
        entry.ShowDialog();
    }
}
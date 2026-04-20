using AutoMapper;
using Fly.Shared.Modules.Store.Dtos;
using Wpf.Ui;

namespace SmartiErpWin.Client.UserInterface.Stores.Items;

public partial class ItemsPageViewModel : BasePageViewModel<DtoItem>
{
    protected override string Endpoint => "Item";

    private IMapper _mapper;
    
    public ItemsPageViewModel(HttpClient client,
        IContentDialogService contentDialogService,
        SessionService sessionService,
        IMapper mapper,
        LocalisationService localisationService): base(client)
    {
        InitBaseModel(localisationService, sessionService, contentDialogService);

        _mapper = mapper;
        OnItemEdit += OnEdit;
        OnItemDelete += OnDelete;
    }

    private void OnEdit(object? sender, EventArgs args)
    {
        var model = DataGridServices.FindModel<DtoItem>(sender);

        if (model is null)
            return;
        
        var entry = new ItemsEntryWindow(LocaleService, model,
            EntryType.Edit, HttpClient, GetType(), _mapper);
        
        entry.ShowDialog();
    }
    
    [RelayCommand]
    private void OnAdd()
    {
        var entry = new ItemsEntryWindow(LocaleService, null,
            EntryType.Add, HttpClient, GetType(), _mapper);
        
        entry.ShowDialog();
    }
}
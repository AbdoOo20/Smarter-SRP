using AutoMapper;
using Fly.Shared.Modules.Accounting.Dtos;
using SmartiErpWin.Client.Helpers;
using Wpf.Ui;

namespace SmartiErpWin.Client.UserInterface.Accounting.Entries;

public partial class EntryPageViewModel : BasePageViewModel<DtoEntry>
{
    protected override string Endpoint => "Entry";

    private readonly IMapper _mapper;
    
    protected override List<Tuple<string, EventHandler>> ExtraButtons =>
    [
        new ("Print", OnPrint)
    ];
    
    public EntryPageViewModel(HttpClient client,
        IContentDialogService contentDialogService,
        SessionService sessionService, IMapper mapper,
        LocalisationService localisationService): base(client)
    {
        InitBaseModel(localisationService, sessionService, contentDialogService);
        _mapper = mapper;
        
        OnItemEdit += OnEdit;
        OnItemDelete += OnDelete;
    }

    private static void OnPrint(object? sender, EventArgs args)
    {
        var model = DataGridServices.FindModel<DtoEntry>(sender);

        if (model is null)
            return;
        
        EventHelper.InvokeEntry(new(model));
    }
    
    private void OnEdit(object? sender, EventArgs args)
    {
        var model = DataGridServices.FindModel<DtoEntry>(sender);

        if (model is null)
            return;
        
        var entry = new EntriesEntryWindow(LocaleService, model,
            EntryType.Edit, HttpClient, _mapper, GetType());
        
        entry.ShowDialog();
    }
    
    [RelayCommand]
    private void OnAdd()
    {
        var entry = new EntriesEntryWindow(LocaleService, null,
            EntryType.Add, HttpClient, _mapper, GetType());
        
        entry.ShowDialog();
    }
}
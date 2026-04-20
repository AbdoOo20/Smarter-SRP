using AutoMapper;
using Fly.Shared.Modules.Marketers.Dtos;
using SmartiErpWin.Client.Helpers;
using Wpf.Ui;

namespace SmartiErpWin.Client.UserInterface.Marketing.Marketer;

public sealed partial class MarketerDisbursementPermitPageViewModel : BasePageViewModel<DtoMarketerDisbursementPermit>
{
    private readonly IMapper _mapper;
    
    protected override string Endpoint => "MarketerDisbursementPermit";
    
    protected override List<Tuple<string, EventHandler>> ExtraButtons =>
    [
        new ("Print", OnPrint)
    ];
    
    public MarketerDisbursementPermitPageViewModel(HttpClient client,
        IContentDialogService contentDialogService,
        SessionService sessionService,
        LocalisationService localisationService,
        IMapper mapper): base(client)
    {
        InitBaseModel(localisationService, sessionService, contentDialogService);
        _mapper = mapper;
        
        OnItemEdit += OnEdit;
        OnItemDelete += OnDelete;
    }

    private static void OnPrint(object? sender, EventArgs args)
    {
        var model = DataGridServices.FindModel<DtoMarketerDisbursementPermit>(sender);

        if (model is null)
            return;
        
        EventHelper.InvokeMarketerDisbursement(new(model));
    }
    
    private void OnEdit(object? sender, EventArgs args)
    {
        var model = DataGridServices.FindModel<DtoMarketerDisbursementPermit>(sender);

        if (model is null)
            return;
        
        var entry = new MarketerDisbursementPermitEntryWindow(LocaleService, model,
            EntryType.Edit, HttpClient, GetType(), _mapper);
        
        entry.ShowDialog();
    }
    
    [RelayCommand]
    private void OnAdd()
    {
        var entry = new MarketerDisbursementPermitEntryWindow(LocaleService, null,
            EntryType.Add, HttpClient, GetType(), _mapper);
        
        entry.ShowDialog();
    }
}
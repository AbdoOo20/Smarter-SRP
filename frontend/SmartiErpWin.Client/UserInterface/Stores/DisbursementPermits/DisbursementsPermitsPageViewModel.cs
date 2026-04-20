using AutoMapper;
using Fly.Shared.Modules.Store.Dtos;
using Wpf.Ui;

namespace SmartiErpWin.Client.UserInterface.Stores.DisbursementPermits;

public partial class DisbursementsPermitsPageViewModel : BasePageViewModel<DtoDisbursementPermit>
{
    protected override string Endpoint => "DisbursementPermit";

    private readonly IMapper _mapper;
    
    public DisbursementsPermitsPageViewModel(HttpClient client,
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
        var model = DataGridServices.FindModel<DtoDisbursementPermit>(sender);

        if (model is null)
            return;
        
        var entry = new DisbursementsPermitEntryWindow(LocaleService, model,
            EntryType.Edit, HttpClient, _mapper, GetType());
        
        entry.ShowDialog();
    }
    
    [RelayCommand]
    private void OnAdd()
    {
        var entry = new DisbursementsPermitEntryWindow(LocaleService, null,
            EntryType.Add, HttpClient, _mapper, GetType());
        
        entry.ShowDialog();
    }
}
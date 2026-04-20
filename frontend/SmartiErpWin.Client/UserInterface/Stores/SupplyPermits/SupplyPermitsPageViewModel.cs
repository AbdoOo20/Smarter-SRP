using AutoMapper;
using Fly.Shared.Modules.Store.Dtos;
using Wpf.Ui;

namespace SmartiErpWin.Client.UserInterface.Stores.SupplyPermits;

public partial class SupplyPermitsPageViewModel : BasePageViewModel<DtoSupplyPermit>
{
    protected override string Endpoint => "SupplyPermit";

    private readonly IMapper _mapper;
    
    public SupplyPermitsPageViewModel(HttpClient client,
        IContentDialogService contentDialogService,
        SessionService sessionService, IMapper mapper,
        LocalisationService localisationService): base(client)
    {
        InitBaseModel(localisationService, sessionService, contentDialogService);

        _mapper = mapper;
        
        OnItemEdit += OnEdit;
        OnItemDelete += OnDelete;
    }

    private void OnEdit(object? sender, EventArgs args)
    {
        var model = DataGridServices.FindModel<DtoSupplyPermit>(sender);

        if (model is null)
            return;
        
        var entry = new SupplyPermitEntryWindow(LocaleService, model,
            EntryType.Edit, HttpClient, _mapper, GetType());
        
        entry.ShowDialog();
    }
    
    [RelayCommand]
    private void OnAdd()
    {
        var entry = new SupplyPermitEntryWindow(LocaleService, null,
            EntryType.Add, HttpClient, _mapper, GetType());
        
        entry.ShowDialog();
    }
}
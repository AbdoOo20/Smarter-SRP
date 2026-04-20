using AutoMapper;
using Fly.Shared.Modules.PurchaseAndSales.Dtos;
using SmartiErpWin.Client.Helpers;
using SmartiErpWin.Client.UserInterface.Purchases.PurchaseReturnInvoice.Partials;
using Wpf.Ui;

namespace SmartiErpWin.Client.UserInterface.Purchases.PurchaseInvoices;

public partial class PurchasesInvoicePageViewModel : BasePageViewModel<DtoPurchaseInvoice>
{
    private readonly IMapper _mapper;
    
    protected override string Endpoint => "Purchase";
    
    protected override List<Tuple<string, EventHandler>> ExtraButtons =>
    [
        new ("Print", OnPrint)
    ];
    
    public PurchasesInvoicePageViewModel(HttpClient client,
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

    private void OnEdit(object? sender, EventArgs args)
    {
        var model = DataGridServices.FindModel<DtoPurchaseInvoice>(sender);

        if (model is null)
            return;
        
        var entry = new PurchasesInvoiceEntryWindow(LocaleService, model,
            EntryType.Edit, HttpClient, GetType(), _mapper);
        
        entry.ShowDialog();
    }
    
    private static void OnPrint(object? sender, EventArgs args)
    {
        var model = DataGridServices.FindModel<DtoPurchaseInvoice>(sender);

        if (model is null)
            return;
        
        EventHelper.InvokePurchases(new(model));
    }
    
    [RelayCommand]
    private void OnAdd()
    {
        var entry = new PurchasesInvoiceEntryWindow(LocaleService, null,
            EntryType.Add, HttpClient, GetType(), _mapper);
        
        entry.ShowDialog();
    }

    [RelayCommand]
    private void OnReturn()
    {
        var entry = new PurchaseReturnInvoiceLookupWindow(new PurchaseReturnInvoiceWindowViewModel(Session, LocaleService, HttpClient, _mapper));

        entry.ShowDialog();
    }
}
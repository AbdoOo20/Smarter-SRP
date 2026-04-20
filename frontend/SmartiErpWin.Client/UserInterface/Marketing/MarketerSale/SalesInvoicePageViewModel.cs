using AutoMapper;
using Fly.Shared.Modules.PurchaseAndSales.Dtos;
using Wpf.Ui;

namespace SmartiErpWin.Client.UserInterface.Sales.SaleInvoices;

public partial class SalesInvoicePageViewModel : BasePageViewModel<DtoSalesInvoice>
{
    private readonly IMapper _mapper;
    
    protected override string Endpoint => "Sales";
    
    public SalesInvoicePageViewModel(HttpClient client,
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
        var model = DataGridServices.FindModel<DtoSalesInvoice>(sender);

        if (model is null)
            return;
        
        var entry = new SalesInvoiceEntryWindow(LocaleService, model,
            EntryType.Edit, HttpClient, GetType(), _mapper);
        
        entry.ShowDialog();
    }
    
    [RelayCommand]
    private void OnAdd()
    {
        var entry = new SalesInvoiceEntryWindow(LocaleService, null,
            EntryType.Add, HttpClient, GetType(), _mapper);
        
        entry.ShowDialog();
    }
}
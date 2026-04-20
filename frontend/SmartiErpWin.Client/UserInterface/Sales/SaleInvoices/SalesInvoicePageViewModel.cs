using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Sell;
using SmartErpWin.Core;
using SmartiErpWin.Client.Helpers;
using SmartiErpWin.Client.UserInterface.Sales.Lookup;
using Wpf.Ui;
using IMapper = AutoMapper.IMapper;

namespace SmartiErpWin.Client.UserInterface.Sales.SaleInvoices;

public partial class SalesInvoicePageViewModel : BasePageViewModel<DtoSalesInvoice>
{
    private readonly IMapper _mapper;

    protected override string Endpoint => "Sales";

    protected override List<Tuple<string, EventHandler>> ExtraButtons =>
        [
            new ("Copy", OnCopy),
            new ("Print", OnPrint)
        ];

    public SalesInvoicePageViewModel(HttpClient client,
        IContentDialogService contentDialogService,
        SessionService sessionService,
        LocalisationService localisationService,
        IMapper mapper) : base(client)
    {
        InitBaseModel(localisationService, sessionService, contentDialogService);
        _mapper = mapper;

        OnItemEdit += OnEdit;
        OnItemDelete += OnDelete;
    }

    private void OnPrint(object? sender, EventArgs args)
    {
        var model = DataGridServices.FindModel<DtoSalesInvoice>(sender);
        
        if (model is null)
            return;

        EventHelper.InvokeSales(new(model));
        //var response = await this.GetInnerByQueryAsync<FileResponse>("Report/" + model.Id.ToString(), new());
        //ReportRunner.PrintApiReport(response.Data);
    }

    private async void OnCopy(object? sender, EventArgs args)
    {
        var model = DataGridServices.FindModel<DtoSalesInvoice>(sender);

        var deepCopy = await CommonHelpers.DeepCopyValueOfReferenceTypeAsync(model);
        deepCopy.Id = Identifier.Empty;

        if (model is null)
            return;

        var entry = new SalesInvoiceEntryWindow(LocaleService, deepCopy,
            EntryType.Add, HttpClient, GetType(), _mapper);

        entry.ShowDialog();
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

    [RelayCommand]
    private void OnReturnSale()
    {
        var entry = new LookupWindow(new LookupWindowViewModel(Session, LocaleService, HttpClient, _mapper));

        entry.ShowDialog();
    }
}
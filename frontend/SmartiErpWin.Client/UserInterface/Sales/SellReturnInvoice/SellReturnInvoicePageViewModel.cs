using AutoMapper;
using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Sell;
using SmartErpWin.Core;
using SmartiErpWin.Client.Helpers;
using SmartiErpWin.Client.UserInterface.Sales.Lookup;
using Wpf.Ui;

namespace SmartiErpWin.Client.UserInterface.Sales.SellReturnInvoice;

public partial class SellReturnInvoicePageViewModel : BasePageViewModel<DtoSellReturnInvoice>
{
    private readonly IMapper _mapper;

    protected override string Endpoint => "SellReturnInvoice";

    protected override List<Tuple<string, EventHandler>> ExtraButtons =>
        [
            //new ("Copy", OnCopy),
            new ("Print", OnPrint)
        ];

    public SellReturnInvoicePageViewModel(HttpClient client,
        IContentDialogService contentDialogService,
        SessionService sessionService,
        LocalisationService localisationService,
        IMapper mapper) : base(client)
    {
        InitBaseModel(localisationService, sessionService, contentDialogService);
        _mapper = mapper;

        PrintVisibility = Visibility.Collapsed;
        OnItemEdit += OnEdit;
        OnItemDelete += OnDelete;
    }

    private static void OnPrint(object? sender, EventArgs args)
    {
        var model = DataGridServices.FindModel<DtoSellReturnInvoice>(sender);

        if (model is null)
            return;

        EventHelper.InvokeSellReturnInvoice(new(model));
    }

    private async void OnCopy(object? sender, EventArgs args)
    {
        var model = DataGridServices.FindModel<DtoSellReturnInvoice>(sender);

        var deepCopy = await CommonHelpers.DeepCopyValueOfReferenceTypeAsync(model);
        deepCopy.Id = Identifier.Empty;

        if (model is null)
            return;

        var entry = new SellReturnInvoiceEntryWindow(LocaleService, deepCopy,
            EntryType.Add, HttpClient, GetType(), _mapper);

        entry.ShowDialog();
    }

    private void OnEdit(object? sender, EventArgs args)
    {
        var model = DataGridServices.FindModel<DtoSellReturnInvoice>(sender);

        if (model is null)
            return;

        var entry = new SellReturnInvoiceEntryWindow(LocaleService, model,
            EntryType.Edit, HttpClient, GetType(), _mapper);

        entry.ShowDialog();
    }

    [RelayCommand]
    private void OnAdd()
    {
        var entry = new SellReturnInvoiceEntryWindow(LocaleService, null,
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
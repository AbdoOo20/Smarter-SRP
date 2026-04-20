using AutoMapper;
using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Purchase;
using SmartErpWin.Core;
using SmartiErpWin.Client.Helpers;
using Wpf.Ui;

namespace SmartiErpWin.Client.UserInterface.Purchases.PurchaseReturnInvoice;

public partial class PurchaseReturnInvoicePageViewModel : BasePageViewModel<DtoPurchaseReturnInvoice>
{
    private readonly IMapper _mapper;

    protected override string Endpoint => "PurchaseReturnInvoice";

    protected override List<Tuple<string, EventHandler>> ExtraButtons =>
        [
            //new ("Copy", OnCopy),
            new ("Print", OnPrint)
        ];

    public PurchaseReturnInvoicePageViewModel(HttpClient client,
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

    private static void OnPrint(object? sender, EventArgs args)
    {
        var model = DataGridServices.FindModel<DtoPurchaseReturnInvoice>(sender);

        if (model is null)
            return;

        EventHelper.InvokePurchaseReturnInvoice(new(model));
    }

    private async void OnCopy(object? sender, EventArgs args)
    {
        var model = DataGridServices.FindModel<DtoPurchaseReturnInvoice>(sender);

        var deepCopy = await CommonHelpers.DeepCopyValueOfReferenceTypeAsync(model);
        deepCopy.Id = Identifier.Empty;

        if (model is null)
            return;

        var entry = new PurchaseReturnInvoiceEntryWindow(LocaleService, deepCopy,
            EntryType.Add, HttpClient, GetType(), _mapper);

        entry.ShowDialog();
    }

    private void OnEdit(object? sender, EventArgs args)
    {
        var model = DataGridServices.FindModel<DtoPurchaseReturnInvoice>(sender);

        if (model is null)
            return;

        var entry = new PurchaseReturnInvoiceEntryWindow(LocaleService, model,
            EntryType.Edit, HttpClient, GetType(), _mapper);

        entry.ShowDialog();
    }

    [RelayCommand]
    private void OnAdd()
    {
        var entry = new PurchaseReturnInvoiceEntryWindow(LocaleService, null,
            EntryType.Add, HttpClient, GetType(), _mapper);

        entry.ShowDialog();
    }

    
}
using Fly.Shared.Modules.Factoring.Dtos;
using Leapay.Client.Events.Arguments;
using Leapay.Client.Helpers;
using Leapay.Client.Polymorphism.Bases;
using SmartErpWin.Core.Enums;

namespace Leapay.Client.UserInterface.Factoring.FactoringInvoice;

public partial class FactoringInvoicesPageViewModel : BasePageViewModel<DtoFactoringInvoice>
{
    protected override string Endpoint => "FactoringInvoice";
    
    protected override List<Tuple<object, EventHandler<DataGridButtonEventArgs<DtoFactoringInvoice>>>> ExtraButtons =>
    [
        new (MaterialIconKind.Printer, OnPrint)
    ];
    
    public FactoringInvoicesPageViewModel()
    {
        OnItemEdit += OnEdit;
        OnItemDelete += OnDelete;
    }

    private static void OnPrint(object? sender, DataGridButtonEventArgs<DtoFactoringInvoice> args)
    {
        EventHelper.InvokeFactoringInvoice(new(args.Model));
    }

    protected override async void OnEdit(object? sender, DataGridButtonEventArgs<DtoFactoringInvoice> args)
    {
        var entry = new FactoringInvoiceEntryWindow(new WindowEntryModel<DtoFactoringInvoice>(args.Model,
            EntryType.Edit, GetType()));

        await ShowContentAsync(entry, options: new(GetPageName(GetType(), EntryType.Edit), true, true, true, true));
    }
    
    protected override async Task OnAddAsync()    {
        var entry = new FactoringInvoiceEntryWindow(new WindowEntryModel<DtoFactoringInvoice>(new(),
            EntryType.Add, GetType()));

        await ShowContentAsync(entry, options: new(GetPageName(GetType(), EntryType.Add), true, true, true, true));
    }
}
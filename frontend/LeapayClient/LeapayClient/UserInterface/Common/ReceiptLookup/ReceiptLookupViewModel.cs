using Fly.Framework.Shared.Common.Dtos;
using Fly.Framework.Shared.Common.ValueTypes;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Sell;
using Leapay.Client.Helpers;
using Leapay.Client.Options;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.Services;
using Leapay.Client.UserInterface.Sales.SalesReturnInvoice;
using SmartErpWin.Core.Enums;

namespace Leapay.Client.UserInterface.Common.ReceiptLookup;

public sealed partial class ReceiptLookupViewModel : BaseRefreshableEntryViewModel<DtoBase>
{
    public ReceiptLookupWindow? Window { get; set; }

    [ObservableProperty]
    public partial string? ActionText { get; set; }
    
    [ObservableProperty]
    public partial string? SearchText { get; set; }
    
    [ObservableProperty]
    public partial string? InfoText { get; set; }
    
    [ObservableProperty]
    public partial long LookupNumber { get; set; }
    
    public ReceiptLookupViewModel(
        DtoBase? model,
        EntryType entryType)
    {
        SetDetails(model, entryType, GetType());

        InfoText = LocaleService["EnterNumberToSearch"];
        ApplicationTitle = LocaleService["ReturnSales"];
        ActionText = LocaleService?["Search"];
        
        SetRefreshState(false);
    }
    
    [RelayCommand]
    private async Task OnActionAsync(string parameter)
    {
        SetRefreshState(true);

        if (LookupNumber < 1)
        {
            InfoText = LocaleService["InvoiceNumberEmpty"];
            SetRefreshState(false);
            return;
        }

        try
        {
            SetInfo(LocaleService["SearchingForInvoice"]);
            var request = await RequestBySingleQueryAsync<DtoSellReturnInvoice>("SellReturnInvoice/VerifyInvoice",
                "Number",
                LookupNumber.ToString());

            await Task.Delay(1000);

            if (request is { Data: not null, Status: true })
            {
                Success(request.Data);
            }
            else
            {
                Unsuccessful(request?.ErrorMessage ?? "NO ERROR MESSAGE");
            }
        }
        catch (Exception x)
        {
            Unsuccessful(x.Message);
        }

        return;

        void Unsuccessful(string message)
        {
            SetRefreshState(false);
            InfoText = $"{LocaleService["UnsuccessfulLookup"]}\n{message}";
        }

        async void Success(DtoSellReturnInvoice sale)
        {
            if (parameter is "F")
            {
                try
                {
                    SetInfo(LocaleService["ReturningInvoice"]);
                    var req = await PostSaveSpecificAndReturnAsync<TypeOf<long>, DtoSellReturnInvoice>(
                        new TypeOf<long>(LookupNumber),
                        "SellReturnInvoice/ReturnByNumber",
                        appendOriginal: false);

                    if (req is { Status: false, Data: null })
                    {
                        throw new(req.ErrorMessage);
                    }
                    
                    WindowClosure();
                    
                    if (await DialogueServices.ConfirmAsync("InvoiceReturned",
                            "InvoiceReturnedPrintPrompt"))
                    {
                        EventHelper.InvokeSellReturnInvoice(new(req.Data));
                    }
                    
                    SynchroniseTypeBase<SellReturnInvoicesPageViewModel>();
                }
                catch (Exception x)
                {
                    Unsuccessful(x.Message);
                }
                finally
                {
                    SetRefreshState(false);
                }

                return;
            }

            SetInfo(this["LoadingInvoice"]);
            sale.InvoiceNumber = LookupNumber;
            WindowClosure();
            
            await ContentHelper.ShowCrossPlatformContentAsync(
                new SellReturnInvoiceEntryWindow(
                    new SellReturnInvoiceEntryViewModel(sale, EntryType.Add, typeof(SellReturnInvoicesPage))),
                new WindowOptions(this["SellReturnInvoicesPageViewModelAdd"], IsMaximized: true, CanMaximize: true,
                    CanMinimize: true, CanResize: true));
            
            SetRefreshState(false);
        }
    }
}
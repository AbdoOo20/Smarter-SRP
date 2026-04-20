using Fly.Framework.Shared.Common.Dtos;
using Fly.Framework.Shared.Common.ValueTypes;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Purchase;
using Leapay.Client.Helpers;
using Leapay.Client.Options;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.Services;
using Leapay.Client.UserInterface.Purchases.PurchaseReturnInvoice;
using SmartErpWin.Core.Enums;

namespace Leapay.Client.UserInterface.Common.ReceiptLookupPurchase;

public sealed partial class PurchaseReceiptLookupViewModel : BaseRefreshableEntryViewModel<DtoBase>
{
    [ObservableProperty]
    public partial string? ActionText { get; set; }
    
    [ObservableProperty]
    public partial string? SearchText { get; set; }
    
    [ObservableProperty]
    public partial string? InfoText { get; set; }
    
    [ObservableProperty]
    public partial long LookupNumber { get; set; }
    
    public PurchaseReceiptLookupViewModel(
        DtoBase? model,
        EntryType entryType)
    {
        SetDetails(model, entryType, GetType());

        InfoText = LocaleService["EnterNumberToSearch"];
        ApplicationTitle = LocaleService["ReturnPurchase"];
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
            var request = await RequestBySingleQueryAsync<DtoPurchaseReturnInvoice>("PurchaseReturnInvoice/VerifyInvoice",
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

        async void Success(DtoPurchaseReturnInvoice purchase)
        {
            if (parameter is "F")
            {
                try
                {
                    SetInfo(LocaleService["ReturningInvoice"]);
                    var req = await PostSaveSpecificAndReturnAsync<TypeOf<long>, DtoPurchaseReturnInvoice>(
                        new TypeOf<long>(LookupNumber),
                        "PurchaseReturnInvoice/ReturnByNumber",
                        appendOriginal: false);

                    if (req is { Status: false, Data: null })
                    {
                        throw new(req.ErrorMessage);
                    }
                    
                    WindowClosure();
                    
                    if (await DialogueServices.ConfirmAsync("InvoiceReturned",
                            "InvoiceReturnedPrintPrompt"))
                    {
                        EventHelper.InvokePurchaseReturnInvoice(new(req.Data));
                    }
                    
                    SynchroniseTypeBase<PurchaseReturnInvoicesPageViewModel>();
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
            purchase.InvoiceNumber = LookupNumber;
            WindowClosure();
            
            await ContentHelper.ShowCrossPlatformContentAsync(
                new PurchaseReturnInvoiceEntryWindow(
                    new PurchaseReturnInvoiceEntryViewModel(purchase, EntryType.Add, typeof(PurchaseReturnInvoicesPage))),
                new WindowOptions(this["PurchaseReturnInvoicesPageViewModelAdd"], IsMaximized: true, CanMaximize: true,
                    CanMinimize: true, CanResize: true));
            
            SetRefreshState(false);
        }
    }
}
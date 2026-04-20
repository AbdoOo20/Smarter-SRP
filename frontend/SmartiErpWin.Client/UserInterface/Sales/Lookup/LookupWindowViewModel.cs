using AutoMapper;
using Fly.Framework.Shared.Common.Dtos;
using Fly.Framework.Shared.Common.ValueTypes;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Sell;
using SmartErpWin.Core;
using SmartiErpWin.Client.Helpers;
using SmartiErpWin.Client.UserInterface.Sales.SellReturnInvoice;

namespace SmartiErpWin.Client.UserInterface.Sales.Lookup;

public partial class LookupWindowViewModel : BaseRefreshableViewModel<DtoBase>
{
    public LookupWindow? LoginWindow { get; set; }

    [ObservableProperty]
    private string _actionText;

    [ObservableProperty]
    private long _lookupNumber;

    public LookupWindowViewModel(
        SessionService session,
        LocalisationService localeService,
        HttpClient client,
        IMapper mapper) : base(client)
    {
        InitBaseModel(localeService, session, null);
        InfoText = LocaleService["EnterNumberToSearch"];
        ApplicationTitle = LocaleService["ReturnSales"];
        ActionText = LocaleService?["Search"];
        _mapper = mapper;
    }

    [ObservableProperty]
    private string? _infoText;

    private readonly IMapper _mapper;

    [RelayCommand]
    private async Task OnActionAsync(string parameter)
    {
        SetRefreshState(true);

        if (LookupNumber < 1)
        {
            InfoText = LocaleService["InvoiceNumberEmpty"];
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

                    LoginWindow?.Close();
                    if (await DialogueServices.ConfirmAsync(LocaleService, "SaleInvoiceReturned",
                            "InvoiceReturnedPrintPrompt"))
                    {
                        EventHelper.InvokeCompleteSalesReturn(new(req.Data));
                    }
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
            var sales = new SellReturnInvoiceEntryWindow(LocaleService,
                sale, EntryType.Add, HttpClient,
                typeof(SellReturnInvoicePageViewModel),
                App.GetService<IMapper>());

            SynchroniseTypeBase<SellReturnInvoicePageViewModel>(CodeBook.LimVis);
            LoginWindow.DialogResult = true;
            LoginWindow?.Close();

            SetRefreshState(false);
            sales.ShowDialog();
        }
    }
}
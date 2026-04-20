using System.ComponentModel;
using System.Globalization;
using Fly.Framework.Shared.Common.ValueTypes;
using Fly.Shared.Modules.Financial.Requests;
using SmartiErpWin.Client.Reports;
using SmartiErpWin.Client.UserInterface.Financial.FinancingOperations.ModelBase;

namespace SmartiErpWin.Client.UserInterface.Financial.FinancingOperations.OperationViewModels;

public sealed partial class MarketerFinanceEntryViewModel(
    Window window,
    LocalisationService locale,
    MarketerPercentRequest? model,
    EntryType entryType,
    HttpClient client,
    Type pageType)
    : FinanceOperationViewModelBase<MarketerPercentRequest>(
        window,
        locale,
        model,
        entryType,
        client,
        pageType,
        "Marketers",
        "MarketerPaymentReceipt")
{
    protected override OperationType? FinanceType => OperationType.Deposit;

    [ObservableProperty]
    private decimal _clientPercentage;

    [ObservableProperty]
    private decimal _clientValue;

    [ObservableProperty]
    private decimal _marketerDebt;

    [ObservableProperty]
    private decimal _clientPrice;

    [ObservableProperty]
    private string? _visibleInfo;

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        if (Model is null)
            return;

        ClientValue = ClientPrice * (ClientPercentage / 100);

        VisibleInfo = (ClientValue - ClientPrice).ToString(CultureInfo.InvariantCulture);
    }

    public async void SetMarketerInfo()
    {
        var id = ComboModelBase.GetSelectedItemId();

        if (id.IsNullOrEmpty())
            return;

        MarketerDebt = (await RequestOneUsingQueryRequestAsync<TypeOf<decimal>>("AccountDeption", new StatementQueryRequest
        {
            AccountId = id
        })).Data!.Value;

        ClientPercentage = (await RequestOneUsingQueryRequestAsync<TypeOf<decimal>>("MarketerPercentage",
            new StatementQueryRequest
            {
                AccountId = id
            })).Data!.Value;
    }

    protected override void LoadReport(bool isDeposit, string? others, string? data = null)
    {
        ReportRunner.PrintMarketer(isDeposit, this);
        base.LoadReport(isDeposit, others, data);
    }

    protected override void PreSaveActions()
    {
        base.PreSaveActions();

        Model.Amount = ClientPrice;
    }
}
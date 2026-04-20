using System.ComponentModel;
using System.Globalization;
using Fly.Framework.Shared.Common.ValueTypes;
using Fly.Shared.Modules.Financial.Requests;
using Leapay.Client.Reports;
using SmartErpWin.Core.Enums;

namespace Leapay.Client.UserInterface.FinancingOperations.OperationViewModels;

public sealed partial class MarketerFinanceEntryViewModel(
    MarketerPercentRequest? model,
    EntryType entryType,
    Type pageType)
    : ModelBase.FinanceOperationViewModelBase<MarketerPercentRequest>(
        model,
        entryType,
        pageType,
        "Marketers",
        "MarketerPaymentReceipt")
{
    protected override OperationType? FinanceType => OperationType.Deposit;

    [ObservableProperty]
    public partial decimal ClientPercentage { get; set; }
    
    [ObservableProperty]
    public partial decimal ClientValue { get; set; }
    
    [ObservableProperty]
    public partial decimal MarketerDebt { get; set; }
    
    [ObservableProperty]
    public partial decimal ClientPrice { get; set; }
    
    [ObservableProperty]
    public partial string? VisibleInfo { get; set; }

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
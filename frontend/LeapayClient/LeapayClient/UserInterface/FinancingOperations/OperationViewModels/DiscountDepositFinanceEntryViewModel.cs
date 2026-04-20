using Fly.Shared.Modules.Accounting.Requests;
using Leapay.Client.Reports;
using SmartErpWin.Core.Enums;

namespace Leapay.Client.UserInterface.FinancingOperations.OperationViewModels;

public sealed class DiscountDepositFinanceEntryViewModel(
    RSimpleRequest? model,
    EntryType entryType,
    Type pageType)
    : ModelBase.FinanceOperationViewModelBase<RSimpleRequest>(
        model,
        entryType,
        pageType,
        "Clients",
        "DiscountPaymentReceipt",
        "ClientName")
{
    protected override OperationType? FinanceType => OperationType.Deposit;

    protected override void LoadReport(bool isDeposit, string? others, string? data = null)
    {
        ReportRunner.PrintDiscountDeposit(isDeposit, this);
        base.LoadReport(isDeposit, others, data);
    }
}
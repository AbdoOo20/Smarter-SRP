using Fly.Shared.Modules.Accounting.Requests;
using SmartiErpWin.Client.Reports;
using SmartiErpWin.Client.UserInterface.Financial.FinancingOperations.ModelBase;

namespace SmartiErpWin.Client.UserInterface.Financial.FinancingOperations.OperationViewModels;

public sealed class DiscountDepositFinanceEntryViewModel(
    Window window,
    LocalisationService locale,
    RSimpleRequest? model,
    EntryType entryType,
    HttpClient client,
    Type pageType)
    : FinanceOperationViewModelBase<RSimpleRequest>(
        window,
        locale,
        model,
        entryType,
        client,
        pageType,
        "Clients",
        "DiscountPaymentReceipt")
{
    protected override OperationType? FinanceType => OperationType.Deposit;

    protected override void LoadReport(bool isDeposit, string? others, string? data = null)
    {
        ReportRunner.PrintDiscountDeposit(isDeposit, this);
        base.LoadReport(isDeposit, others, data);
    }
}
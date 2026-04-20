using Fly.Shared.Modules.Accounting.Requests;
using Leapay.Client.Reports;
using SmartErpWin.Core.Enums;

namespace Leapay.Client.UserInterface.FinancingOperations.OperationViewModels;

public sealed class ClientBillEntryViewModel(
    RSimpleRequest? model,
    EntryType entryType,
    Type pageType)
    : ModelBase.FinanceOperationViewModelBase<RSimpleRequest>(
        model,
        entryType,
        pageType,
        "Clients",
        "ClientPaymentReceipt",
        "ClientName")
{
    protected override void LoadReport(bool isDeposit, string? others, string? data = null)
    {
        ReportRunner.PrintClientBill(isDeposit, this, data);
        base.LoadReport(isDeposit, others, data);
    }
}
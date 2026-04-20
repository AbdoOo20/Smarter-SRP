using Fly.Shared.Modules.Accounting.Requests;
using Leapay.Client.Reports;
using SmartErpWin.Core.Enums;

namespace Leapay.Client.UserInterface.FinancingOperations.OperationViewModels;

public sealed class EmployeeFinanceEntryViewModel(
    RSimpleRequest? model,
    EntryType entryType,
    Type pageType)
    : ModelBase.FinanceOperationViewModelBase<RSimpleRequest>(
        model,
        entryType,
        pageType,
        "Employees",
        "EmployeePaymentReceipt",
        "EmployeeName")
{
    protected override void LoadReport(bool isDeposit, string? others, string? data = null)
    {
        ReportRunner.PrintEmployee(isDeposit, this);
        base.LoadReport(isDeposit, others, data);
    }
}
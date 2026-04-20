using Fly.Shared.Modules.Accounting.Requests;
using SmartiErpWin.Client.Reports;
using SmartiErpWin.Client.UserInterface.Financial.FinancingOperations.ModelBase;

namespace SmartiErpWin.Client.UserInterface.Financial.FinancingOperations.OperationViewModels;

public sealed class EmployeeFinanceEntryViewModel(
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
        "Employees",
        "EmployeePaymentReceipt")
{
    protected override void LoadReport(bool isDeposit, string? others, string? data = null)
    {
        ReportRunner.PrintEmployee(isDeposit, this);
        base.LoadReport(isDeposit, others, data);
    }
}
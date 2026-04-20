using Fly.Shared.Modules.Accounting.Requests;
using SmartErpWin.Core.Enums;

namespace Leapay.Client.UserInterface.FinancingOperations.OperationViewModels;

public sealed class GeneralExpenseEntryViewModel(
    RSimpleRequest? model,
    EntryType entryType,
    Type pageType)
    : ModelBase.FinanceOperationViewModelBase<RSimpleRequest>(
        model,
        entryType,
        pageType,
        "Expenses",
        "ExpensePaymentReceipt",
        "ExpenseName")
{
    protected override void LoadReport(bool isDeposit, string? others, string? data = null)
    {
        //ReportRunner.PrintGeneralExpense(this);
        base.LoadReport(isDeposit, others, data);
    }
}
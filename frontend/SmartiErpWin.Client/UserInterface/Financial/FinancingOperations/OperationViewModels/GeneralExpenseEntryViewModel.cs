using Fly.Shared.Modules.Accounting.Requests;
using SmartiErpWin.Client.UserInterface.Financial.FinancingOperations.ModelBase;

namespace SmartiErpWin.Client.UserInterface.Financial.FinancingOperations.OperationViewModels;

public sealed class GeneralExpenseEntryViewModel(
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
        "Expenses",
        "ExpensePaymentReceipt")
{
    protected override void LoadReport(bool isDeposit, string? others, string? data = null)
    {
        //ReportRunner.PrintGeneralExpense(this);
        base.LoadReport(isDeposit, others, data);
    }
}
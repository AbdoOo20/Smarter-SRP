using Fly.Shared.Modules.Accounting.Requests;
using SmartiErpWin.Client.Reports;
using SmartiErpWin.Client.UserInterface.Financial.FinancingOperations.ModelBase;

namespace SmartiErpWin.Client.UserInterface.Financial.FinancingOperations.OperationViewModels;

public sealed class SupplierBillEntryViewModel(
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
        "Suppliers",
        "SupplierPaymentReceipt")
{
    protected override void LoadReport(bool isDeposit, string? others, string? data = null)
    {
        //  await SendFinancialRequestAsync("SupplierPaymentReceipt", Model);
        ReportRunner.PrintupplierBill(isDeposit, this, data);
        base.LoadReport(isDeposit, others, data);
    }
}
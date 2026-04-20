using Fly.Shared.Modules.Accounting.Requests;
using Leapay.Client.Reports;
using SmartErpWin.Core.Enums;

namespace Leapay.Client.UserInterface.FinancingOperations.OperationViewModels;

public sealed class SupplierBillEntryViewModel(
    RSimpleRequest? model,
    EntryType entryType,
    Type pageType)
    : ModelBase.FinanceOperationViewModelBase<RSimpleRequest>(
        model,
        entryType,
        pageType,
        "Suppliers",
        "SupplierPaymentReceipt",
        "SupplierName")
{
    protected override void LoadReport(bool isDeposit, string? others, string? data = null)
    {
        //  await SendFinancialRequestAsync("SupplierPaymentReceipt", Model);
        ReportRunner.PrintupplierBill(isDeposit, this, data);
        base.LoadReport(isDeposit, others, data);
    }
}
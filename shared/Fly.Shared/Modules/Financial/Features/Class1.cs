using Fly.Framework.Shared.Common.Features;
using Fly.Framework.Shared.Common.Http;
using Fly.Framework.Shared.Common.ValueTypes;
using Fly.Shared.Modules.Financial.Dtos;
using Fly.Shared.Modules.Financial.Requests;
using Fly.Shared.Modules.PurchaseAndSales.Requests;
using Fly.Shared.Modules.Store.Requests;

namespace Fly.Shared.Modules.Financial.Features;

public interface IStetementFeatures
{
    ValueTask<HttpResponseWrapperList<DefaultLookup>> GetAccounts(int itemId);
    ValueTask<HttpResponseWrapperList<DtoStatment>> GetStetements(StatementQueryRequest request);
}
public interface IFinancialFeatures
//  : IFeatureBase<DtoAccount>
: IFeatureBase
{
    //  POST
    /// <summary>
    /// دفع لموظف
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    ValueTask<HttpResponseWrapper<DtoStatment>> PostEmployeePaymentReceipt(EmployeeFinancialRequest request);

    /// <summary>
    /// دفع لعميل
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    ValueTask<HttpResponseWrapper<DtoStatment>> PostClientPaymentReceipt(ClientFinancialRequest request);

    /// <summary>
    /// دفع لمورد
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    ValueTask<HttpResponseWrapper<DtoStatment>> PostSupplierPaymentReceipt(SupplierFinancialRequest request);

    /// <summary>
    /// دفع لحساب
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    ValueTask<HttpResponseWrapper<DtoStatment>> PostAccountPaymentReceipt(AccountFinancialRequest request);

    /// <summary>
    /// مصروفات عمومية
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    ValueTask<HttpResponseWrapper<DtoStatment>> PostExpensePayment(ExpenseFinancialRequest request);
    //  GET
    ValueTask<HttpResponseWrapperList<DefaultLookup>> GetExpenses(GetExpensesRequest request);
    ValueTask<HttpResponseWrapperList<DefaultLookup>> GetEmployees(GetEmployeesRequest request);
    //ValueTask<HttpResponseWrapperList<DefaultLookup>> GetClients(GetClientsRequest request);
    ValueTask<HttpResponseWrapperList<DefaultLookup>> GetClients(ClientQueryRequest request);
    ValueTask<HttpResponseWrapperList<DefaultLookup>> GetSuppliers(GetSuppliersRequest request);
    ValueTask<HttpResponseWrapperList<DefaultLookup>> GetAccounts(GetAccountsRequest request);
    ValueTask<HttpResponseWrapperList<DefaultLookup>> GetSafes(GetSafesRequest request);
    ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupItems(ItemQueryRequest request);
    ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupUnits(UnitQueryRequest request);
}

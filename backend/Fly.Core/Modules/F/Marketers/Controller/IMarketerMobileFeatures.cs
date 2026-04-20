using Fly.Framework.Shared.Common.Features;
using Fly.Framework.Shared.Common.Http;
using Fly.Framework.Shared.Common.ValueTypes;
using Fly.Shared.Modules.Financial.Dtos;
using Fly.Shared.Modules.Financial.Requests;
using Fly.Shared.Modules.Marketers.Dtos;
using Fly.Shared.Modules.Marketers.Requests;
using Fly.Shared.Modules.Store.Requests;

namespace Fly.Core.Modules.F.Marketers.Controller;

public interface IMarketerMobileFeatures : IFeatureBase
{

    ValueTask<HttpResponseWrapperList<DtoMarketerSalesInvoice>> GetTodyInvoices(MarketerSalesInvoiceQueryRequest queryRequest);
    ValueTask<HttpResponseWrapper<DtoStatment>> PostReciept(ClientFinancialRequest request);

    //ValueTask<HttpResponseWrapperList<DefaultLookup>> GetPrice();
    ValueTask<HttpResponseWrapperList<DefaultLookup>> GetUnits(UnitQueryRequest request);
    ValueTask<HttpResponseWrapperList<DefaultLookup>> GetItems(ItemQueryRequest request);
    ValueTask<HttpResponseWrapperList<DefaultLookup>> GetAccounts(GetAccountsRequest request);

    ValueTask<HttpResponseWrapper<DtoStatment>> PostPayment(ClientFinancialRequest request);
    ValueTask<HttpResponseWrapper<MarketerFailingPermitCreateUpdateRequest>> PostSampleFailing(MarketerFailingPermitDetail request);
    ValueTask<HttpResponseWrapper<MarketerFailingPermitCreateUpdateRequest>> PostHardFailing(MarketerFailingPermitCreateUpdateRequest request);
    ValueTask<HttpResponseWrapper<DtoStatment>> PostLower(DiscountItemBasedRequest request);
    ValueTask<HttpResponseWrapper<DtoStatment>> PostExcnce(ExpenseFinancialRequest request);

    ValueTask<HttpResponseWrapperList<DtoMarketerInventory>> GetStorageReport();
    ValueTask<HttpResponseWrapperList<DtoStatmentInfo>> GetDeptionReport();
    ValueTask<HttpResponseWrapperList<DtoStatmentInfo>> GetExcnceReport();

    // SellInvoice
    // Reciept
    // payment
    //  SampleFailing
    //  HardFailing
    // Lower
    // Excnces

    // Today sales
    // Deptions
    // Excnces

}
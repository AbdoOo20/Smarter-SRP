using Fly.Framework.Shared.Common.Features;
using Fly.Shared.Modules.Marketers.Dtos;
using Fly.Shared.Modules.Marketers.Requests;

namespace Fly.Shared.Modules.Marketers.Features;

public interface IMarketersFeatures
    : IFeatureBase<DtoMarketer, MarketerQueryRequest, MarketerDeleteRequest, MarketerCreateUpdateRequest>
{
        
}

public interface IMarketerDisbursementPermitFeatures
    : IFeatureBase<DtoMarketerDisbursementPermit, MarketerDisbursementPermitQueryRequest, MarketerDisbursementPermitDeleteRequestBase, MarketerDisbursementPermitCreateUpdateRequest>
{
        
}

public interface IMarketerSalesInvoiceFeatures
    : IFeatureBase<DtoMarketerSalesInvoice, MarketerSalesInvoiceQueryRequest, MarketerDeleteSalesInvoiceRequest, MarketerSalesInvoiceCreateRequest>
{
        
}
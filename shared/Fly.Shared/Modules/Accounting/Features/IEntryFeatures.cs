using Fly.Framework.Shared.Common.Features;
using Fly.Framework.Shared.Common.Http;
using Fly.Framework.Shared.Common.ValueTypes;
using Fly.Shared.Modules.Accounting.Dtos;
using Fly.Shared.Modules.Accounting.Requests;
using Fly.Shared.Modules.Accounting.Response;

namespace Fly.Shared.Modules.Accounting.Features;

public interface IEntryFeatures : IFeatureBase<DtoEntry, EntryQueryRequest, EntryDeleteRequest, CreateEntryRequest>
{
    ValueTask<HttpResponseWrapper<long>> GetNextNumber();
    ValueTask<HttpResponseWrapper<DtoEntry>> PostCreateSimpleEntry(RSimpleRequest request);
    ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupAccounts(AccountQueryRequest request);
    ValueTask<HttpResponseWrapperList<EntryReportResponse>> GetAccountMovmentReport(EntryReportRequest request);
}
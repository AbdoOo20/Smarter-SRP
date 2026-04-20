using Fly.Framework.Shared.Common.Http;
using Fly.Framework.Shared.Common.Requests;
using Fly.Framework.Shared.Common.ValueTypes;
using Fly.Shared.Modules.Financial.Requests;
using Fly.Shared.Modules.Reports.Requests;

namespace Fly.Shared.Modules.Reports.Features;

public interface IReportsFeatures
{
    ValueTask<HttpResponseWrapperList<DefaultLookup>> GetAccounts(QueryRequestBase query);
    ValueTask<HttpResponseWrapper<DynamicApiReports>> PostAccountMovementsStetements(StatementQueryRequest request);
}

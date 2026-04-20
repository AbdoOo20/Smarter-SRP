using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Collections;
using Fly.Shared.Modules.Accounting.Response;
using MediatR;

namespace Fly.Shared.Modules.Accounting.Requests;

public class EntryReportRequest : IRequest<PageList<EntryReportResponse>>
{
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public Identifier? AccountId { get; set; }
}
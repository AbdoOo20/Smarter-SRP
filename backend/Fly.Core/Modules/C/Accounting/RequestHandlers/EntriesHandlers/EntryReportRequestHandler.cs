using Fly.Core.Persistence.Ef.Models;
using Fly.Framework.Server.Persistence;
using Fly.Framework.Shared.Common.Collections;
using Fly.Shared.Modules.Accounting.Dtos;
using Fly.Shared.Modules.Accounting.Requests;
using Fly.Shared.Modules.Accounting.Response;
using LinqKit;
using MediatR;

namespace Fly.Core.Modules.C.Accounting.RequestHandlers.EntriesHandlers;

internal class EntryReportRequestHandler : IRequestHandler<EntryReportRequest, PageList<EntryReportResponse>>
{

    IServiceBase<DtoEntry, TEntry> _entryService;
    IServiceBase<DtoEntryDetail, TEntryDetail> _entryDetailService;

    public EntryReportRequestHandler(IServiceBase<DtoEntry, TEntry> entryService, IServiceBase<DtoEntryDetail, TEntryDetail> entryDetailService)
    {
        _entryService = entryService;
        _entryDetailService = entryDetailService;
    }

    public async Task<PageList<EntryReportResponse>> Handle(EntryReportRequest request, CancellationToken cancellationToken)
    {
        var w = _entryService.Build();

        if (request.FromDate is not null & request.ToDate is not null)
        {
            w = w.And(w => w.Date >= request.FromDate.Value && w.Date <= request.ToDate.Value);
        }
        if (request.AccountId is not null)
        {
            w = w.And(w => w.Details.Any(ww => ww.AccountId == request.AccountId));
        }
        var ids = await _entryService.GetAllAsAsync(s => s.Id, w);
        var all = await _entryDetailService.GetAllAsync(w => ids.Items.Contains(w.EntryId));
        var result = all.Items.Select(s => new EntryReportResponse()
        {
            Credit = s.Credit,
            Debit = s.Debit,
            Date = s.EntryDate,
            Balance = 0,
            AccountName = s.AccountName,
            //AccountName = s.Account?.Name,
            //EntryNumber = s.Entry?.Number,
        });
        return PageList<EntryReportResponse>.Create(result, 1);
    }
}

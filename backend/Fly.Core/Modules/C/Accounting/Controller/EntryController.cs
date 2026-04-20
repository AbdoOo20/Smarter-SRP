using Fly.Core.Modules.C.Accounting.Services;
using Fly.Core.Modules.C.Accounting.Validators;
using Fly.Core.Persistence.Ef.Models;
using Fly.Framework.Server.Common;
using Fly.Framework.Server.Common.Controllers.CommandsBased;
using Fly.Framework.Server.Common.Localization;
using Fly.Framework.Shared.Common.Http;
using Fly.Framework.Shared.Common.ValueTypes;
using Fly.Shared.Modules.Accounting.Dtos;
using Fly.Shared.Modules.Accounting.Features;
using Fly.Shared.Modules.Accounting.Requests;
using Fly.Shared.Modules.Accounting.Response;
using LinqKit;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Fly.Core.Modules.C.Accounting.Controller;

public class EntryController
    : CustomControllerFeatrueBased<DtoEntry, TEntry, EntryQueryRequest, EntryDeleteRequest, CreateEntryRequest>
        // : CustomControllerFeatrueBased<DtoAccount>
        , IEntryFeatures
//DeleteRequestBase, RCreateAccountRequest>
{
    private readonly EntryService _service;
    private readonly AccountService accountservice;

    //private readonly IServiceBase<DtoEntryDetail, TEntryDetail> _detailsservice;

    public EntryController(ILocalizationService localizationService, ILogger<EntryController> logger, IMediator mediator, EntryService service, AccountService accountservice,
        AutoMapper.IMapper mapper,
        MapsterMapper.IMapper mapster)
        : base(localizationService, logger, mediator, service, mapper, mapster)
    {
        _service = service;
        this.accountservice = accountservice;
        //_detailsservice = detailsservice;

        OnCreateUpdate += EntryController_OnCreateUpdate;
    }

    public override ValueTask<HttpResponseWrapper<DtoEntry?>?> ValidateDeleteAction(EntryDeleteRequest command, ResultOrFailer<DtoEntry?> opt)
    {
        return DeleteValidate(new EntryDeleteRequestValidation(_service), command, opt);
    }

    private async ValueTask<DtoEntry> EntryController_OnCreateUpdate(CreateEntryRequest q)
    {

        var tr = _service.BeginTransaction();

        try
        {
            var isNew = q.Id.IsNullOrEmpty();
            if (isNew)
            {
                var dto = q.Adapt<DtoEntry>();
                dto.OprationType = Shared.Modules.Accounting.Enums.OprationTypeEnum.Normal;
                await _service.Add(dto);
                tr.Commit();
                return dto;
            }
            else
            {
                var detailSerice = _service.MakeChildService<DtoEntryDetail, TEntryDetail>();
                detailSerice.BeginTransaction(tr);

                await detailSerice.Remove(w => w.EntryId == q.Id);
                var det = q.Details.Adapt<List<DtoEntryDetail>>();
                det.ForEach(f => f.EntryId = q.Id);
                await detailSerice.AddRange(det);

                var db = await _service.GetById(q.Id);
                if (db == null) return null;  // TODO: change it
                q.Adapt(db);
                db.Details = null;
                var status = await Service.Update(db);
                tr.Commit();
                db.Details = det;
                return db;
            }
        }
        catch
        {
            tr.Rollback();
            return null;
        }
    }

    [HttpGet("next")]
    public ValueTask<HttpResponseWrapper<long>> GetNextNumber()
    =>
        TryOne<long>(async (opt) =>
        {
            var next = await _service.NextNumber(x => x.Number);
            return opt.Success(next);
        });


    [HttpPost("simple")]
    public ValueTask<HttpResponseWrapper<DtoEntry>> PostCreateSimpleEntry(RSimpleRequest request)
    =>
        TryOne<DtoEntry>(async (opt) =>
        {
            var newDto = await _service.AddSimpleEntry(request);
            if (newDto is null) return PrintError(opt, request.Adapt<DtoEntry>());
            return opt.Success(newDto);
        });


    [HttpGet("accounts")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupAccounts(
        [FromQuery] AccountQueryRequest request)
    =>
        TryMeny<DefaultLookup>(async (opt) =>
        {
            var all = await accountservice.AccountsLookup(request);
            return opt.Success(all);
        });


    [HttpGet("report/movment")]
    public ValueTask<HttpResponseWrapperList<EntryReportResponse>> GetAccountMovmentReport([FromQuery] EntryReportRequest request)

     => TryMeny<EntryReportResponse>(async (opt) =>
        {
            var result = await Mediator.Send(request);
            return opt.Success(result);
        });


    [NonAction]
    public override Expression<Func<DtoEntry, bool>> GetWhere(Expression<Func<DtoEntry, bool>> expression, EntryQueryRequest query)
    {
        if (!string.IsNullOrEmpty(query.SearchText))
        {
            query.SearchText = query.SearchText.Trim();

            expression = expression.And((x) =>

                x.Number.ToString() == (query.SearchText) |
                //x.Note?.Equals(query.SearchText)  |
                x.Details.Any(w=> w.AccountName.Contains(query.SearchText)) | 
                x.Details.Sum(s => s.Debit + s.Credit).ToString().Equals(query.SearchText)
            );

            //expression = expression.Or(x => x.Details.Any(w => w.AccountName == query.SearchText));
            //expression = expression.Or(x => x.Note.Contains(query.SearchText));
            //expression = expression.Or(x => x.Details.Sum(s => s.Debit + s.Credit).ToString().Equals(query.SearchText));
        }

        return base.GetWhere(expression, query);
    }

    protected override async ValueTask AfterDelete(DtoEntry dto, EntryDeleteRequest command, ResultOrFailer<DtoEntry?> opt)
    {
        var detailService = _service.MakeChildService<DtoEntryDetail, TEntryDetail>();
        await detailService.Remove(w=> w.EntryId == dto.Id);
        await base.AfterDelete(dto, command, opt);
    }
}
using AutoMapper;
using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.Accounting.Dtos;
using Fly.Shared.Modules.Accounting.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Fly.Core.Modules.C.Accounting.Services;

public class AccountingFinacial
{
    private readonly IMapper _mapper;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<AccountingFinacial> _logger;
    private readonly MapsterMapper.IMapper _mapsterMapper;

    public AccountingFinacial(ILogger<AccountingFinacial> logger,
        AutoMapper.IMapper mapper,
        MapsterMapper.IMapper mapsterMapper,
        IServiceProvider serviceProvider)
    {

        _logger = logger;
        _mapper = mapper;
        _mapsterMapper = mapsterMapper;
        _serviceProvider = serviceProvider;
        //_serviceProvider = serviceProvider.CreateScope().ServiceProvider;
    }

    public FinacialTransaction BeginFinacialTransaction(DateTime date, OprationTypeEnum? oprationType, int? relatedIndex, Identifier? relatedId, long displayNumber, string? note = "", TagTypeEnum? tagType = null)
    {
        var tr = new FinacialTransaction(this, date, oprationType, relatedIndex, relatedId, displayNumber, note, tagType);
        return tr;
    }

    public async Task<DtoEntry> RegesterTransaction(FinacialTransaction tr)
    {
        //var entryService = _serviceProvider.GetRequiredService<Controller.EntryController>();
        /*
        var entryController = _serviceProvider.GetRequiredService<Controller.EntryController>();
         var res = await entryController.PostSaveAsync(tr.CreateEntryRequest());
        */
        var entryService = _serviceProvider.GetRequiredService<EntryService>();
        var entryRequest = tr.CreateEntryRequest();

        /*
        var dto = entryRequest.Adapt<DtoEntry>();
        dto.OprationType = tr.OprationType;
        dto.RelatedId = tr.RelatedId;
        dto.RelatedIndex = tr.RelatedIndex;
        //var dtoDetails = entryRequest.Details.Adapt<List<DtoEntryDetail>>();
        //dto.Details = null;
        dto.Number = await entryService.NextNumber(c => c.Number);
        var newDto = await entryService.Add(dto);
        */
        var newDto = await entryService
            .AddFullEntry(entryRequest, tr.OprationType ?? OprationTypeEnum.None, tr.RelatedId, tr.RelatedIndex, tagType: tr.TagType);
        //dtoDetails.ForEach(f => f.EntryId = newDto.Id);
        //await _detailsService.AddRange(dtoDetails);
        //dto.Details = dtoDetails;
        return newDto;

    }

    public async Task<DtoEntry?> ReverseTransaction(FinacialTransaction tr, string note, bool reverseEntry = false)
    {

        var entryService = _serviceProvider.GetRequiredService<EntryService>();
        if (reverseEntry)
        {
            var entryRequest = tr.CreateEntryRequest();
            //
            entryRequest.Note = note;
            entryRequest.Details.ForEach(f =>
            {
                (f.Credit, f.Debit) = (f.Debit, f.Credit);
            });
            //
            var newDto = await entryService
                .AddFullEntry(entryRequest, tr.OprationType ?? OprationTypeEnum.None, tr.RelatedId, tr.RelatedIndex, tagType: tr.TagType);
            return newDto;
        }
        else
        {
            var oldEntry = await entryService.GetAsync(w => w.RelatedId == tr.RelatedId && w.RelatedIndex == tr.RelatedIndex);
            await entryService.RemoveDetails(oldEntry!.Id, true);
            await entryService.Remove(oldEntry.Id, true);
            return oldEntry;
        }
    }


}
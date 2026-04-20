using System.Data;
using Fly.Core.Persistence.Ef.Models;
using Fly.Framework.Server.Persistence;
using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Collections;
using Fly.Shared.Modules.Accounting.Dtos;
using Fly.Shared.Modules.Accounting.Enums;
using Fly.Shared.Modules.Accounting.Requests;
using Fly.Shared.Modules.Financial.Dtos;
using Fly.Shared.Modules.Financial.Enums;

namespace Fly.Core.Modules.C.Accounting.Services;

public interface IEntryService : IServiceBase<DtoEntry, TEntry>
{
    Task<DtoEntry?> AddFullEntry(CreateEntryRequest entryRequest, OprationTypeEnum oprationTypeEnum, Identifier? relaredId = null, int? relatedIndex = 0, IDbTransaction? transaction = null, Func<IDbTransaction, DtoEntry, Task>? action = null, TagTypeEnum? tagType = null, bool resetDate = true);
    Task<DtoEntry?> AddSimpleEntry(RSimpleRequest entryRequest, Func<IDbTransaction, DtoEntry, Task>? action = null, Identifier? relaredId = null, int? relatedIndex = 0, IDbTransaction? transaction = null, bool autoCommit = true, TagTypeEnum? tagType = null);

    Task<decimal> GetBalance(Identifier accountId, DateTime? from = null, DateTime? to = null, TagTypeEnum? tagType = null);
    Task<MultiAccountQuery[]> GetBalance(Identifier[] accountsId, DateTime? from = null, DateTime? to = null, TagTypeEnum? tagType = null);

    Task<EntryService.AccountInfo> GetLastCreditInfo(Identifier accountId, DateTime? from = null, DateTime? to = null, TagTypeEnum? tagType = null);
    Task<EntryService.AccountInfo> GetLastDebitInfo(Identifier accountId, DateTime? from = null, DateTime? to = null, TagTypeEnum? tagType = null);
    Task<PageList<DtoEntryDetail>> GetDetails(DateTime? from = null, DateTime? to = null, Identifier? accountId = null, int? pageIndex = null, int? pageSize = null, TagTypeEnum? tagType = null);
    Task<PageList<DtoStatment>> GetDetailsStatements(DateTime? from = null, DateTime? to = null, Identifier? accountId = null, bool collecttTheQuntitiesBeforTime = false, int? pageIndex = null, int? pageSize = null, Identifier? parentId = null, TagTypeEnum? tagType = null, StatementAccountType? statementAccountType = null);
    Task<decimal> GetTotalDebitInfo(Identifier accountId, DateTime? from = null, DateTime? to = null, TagTypeEnum? tagType = null);
    Task<decimal> GetTotalCreditInfo(Identifier accountId, DateTime? from = null, DateTime? to = null, TagTypeEnum? tagType = null);
    Task<List<DtoStatment>> GetNonPayedDebtiobsAccounts(DateTime? from = null, DateTime? to = null, Identifier? accountId = null, TagTypeEnum? tagType = null);
    Task RemoveDetails(Identifier entryId, bool sofrDelete = false);

    Task<DtoStatmentInfo> GetFastStatementInfo(Identifier accountId, DateTime? from = null, DateTime? to = null, TagTypeEnum? tagType = null);
    Task RemoveAllRelated(Identifier relatedId, OprationTypeEnum oprationTypeEnum, int relatedIndex, bool? sofrDelete = null);
    Task<IEnumerable<DtoStatmentInfo>> GetFastStatementListInfo(Identifier? accountId = null, DateTime? from = null, DateTime? to = null, TagTypeEnum? tagType = null);
}
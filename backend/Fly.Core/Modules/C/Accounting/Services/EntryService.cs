// 

using MySqlConnector;
using Fly.Core.Persistence.Ef.Models;
using Fly.Framework.Server.Persistence;
using Fly.Framework.Server.Persistence.Implementations;
using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Collections;
using Fly.Shared.Modules.Accounting.Dtos;
using Fly.Shared.Modules.Accounting.Enums;
using Fly.Shared.Modules.Accounting.Requests;
using Fly.Shared.Modules.Financial.Dtos;
using LinqKit;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Text;
using Fly.Shared.Modules.Financial.Enums;

namespace Fly.Core.Modules.C.Accounting.Services;

public class EntryService : ServiceBase<DtoEntry, TEntry>, IEntryService
{

    public class AccountInfo
    {
        public DateTime? Date { get; set; }
        public decimal Ammount { get; set; }
        public string? Name { get; set; }
        public OprationTypeEnum? EntryOprationType { get; }
        public long DisplayNumber { get; }

        public string OprationTypeString => EntryOprationType switch
        {
            OprationTypeEnum.None => "بدون",
            OprationTypeEnum.Purchases => "فاتورة مشتريات",
            OprationTypeEnum.Sales => "فاتورة مبيعات",
            OprationTypeEnum.Marketer => "مسوق",
            OprationTypeEnum.AccountFinancial => "معاملة مع حساب",
            OprationTypeEnum.ClientFinancial => "معاملة مع حساب زبون",
            OprationTypeEnum.EmployeeFinancial => "معاملة مع حساب موظف",
            OprationTypeEnum.ExpenseFinancial => "معاملة مع حساب مصروف",
            OprationTypeEnum.SupplierFinancial => "معاملة مع حساب مورد",
            OprationTypeEnum.PurchasesCost => "تكاليف مشتريات",
            OprationTypeEnum.DiscountingFinancial => "تخفيض",
            OprationTypeEnum.DiscountingItemFinancial => "تخفيض بسبب صنف",
            OprationTypeEnum.MarketerFinancial => "قبض مسوق",
            OprationTypeEnum.DiscountFinancial => "سند تخفيض",
            OprationTypeEnum.DiscountByItemFinancial => "سند تخفيض على صنف",
            OprationTypeEnum.Normal => "قيد عادي",

            OprationTypeEnum.Sales_Return => "ارجاع مبيعات",
            OprationTypeEnum.CountingTotal => "",
            OprationTypeEnum.OpeningBalance => "رصيد افتتاحي",
            OprationTypeEnum.PurchasesCost_Return => "ارجاع مشتريات",
            OprationTypeEnum.RepresentativePaymentReceipt => "معاملة مندوب عن عميل",
            OprationTypeEnum.Purchases_Return => "ارجاع مشتريات",
            OprationTypeEnum.SalaryConting => "احتساب رواتب",
            OprationTypeEnum.Factoring => "فاتورة تصنيع",
            OprationTypeEnum.Maintenance => "فاتورة صيانة",
            _ => "غير معروف",
        };

        public AccountInfo(DateTime? dateTime, decimal ammount, string? name, OprationTypeEnum? entryOprationType, long displayNumber)
        {
            Date = dateTime;
            Ammount = ammount;
            Name = name;
            EntryOprationType = entryOprationType ?? OprationTypeEnum.None;
            DisplayNumber = displayNumber;
        }
    }

    public EntryService(IRepositryBase<TEntry> repositry,
                          ILogger<EntryService> logger,
                          AutoMapper.IMapper mapper,
                          MapsterMapper.IMapper mapsterMapper,
                          IServiceProvider serviceProvider) : base(logger, mapper, mapsterMapper, serviceProvider)
    {
    }

    private async Task<DtoEntry?> CoreAdd(DtoEntry entry, Identifier? relaredId = null, int? relatedIndex = 0, bool resetDate = true)
    {
        if (resetDate)
        {
            entry.Date = DateTime.Now;
        }
        
        //entry.Number = await NextNumber(c => c.Number);
        entry.RelatedId = relaredId;
        entry.RelatedIndex = relatedIndex;
        var newDto = await Add(entry);
        return newDto;
    }

    public async Task<DtoEntry?> AddFullEntry(CreateEntryRequest entryRequest,
        OprationTypeEnum oprationTypeEnum = OprationTypeEnum.None, Identifier? relaredId = null,
        int? relatedIndex = 0,
        IDbTransaction? transaction = null,
        Func<IDbTransaction, DtoEntry, Task>? action = null,
        TagTypeEnum? tagType = null,
        bool resetDate = true)
    {
        var entityDetailservice = _serviceProvider.GetRequiredService<IServiceBase<DtoEntryDetail, TEntryDetail>>();
        if (transaction is null)
        {
            try
            {
                // todo: solve it
                transaction ??= entityDetailservice.BeginTransaction(isolationLevel: IsolationLevel.ReadCommitted);
            }
            catch { }
        }
        var dto = entryRequest.Adapt<DtoEntry>();
        dto.Tag = tagType;
        dto.OprationType = oprationTypeEnum;
        var entry = await CoreAdd(dto, relaredId, relatedIndex, resetDate);

        if (action is not null)
        {
            await action.Invoke(transaction, entry);
        }

        return entry;
    }

    public async Task<DtoEntry?> AddSimpleEntry(RSimpleRequest entryRequest, Func<IDbTransaction, DtoEntry, Task>? action = null, Identifier? relaredId = null, int? relatedIndex = 0, IDbTransaction? transaction = null, bool autoCommit = true, TagTypeEnum? tagType = null)
    {
        var entityDetailservice = _serviceProvider.GetRequiredService<IServiceBase<DtoEntryDetail, TEntryDetail>>();

        var a = Repositry.DatabaseAccesorId;
        var b = entityDetailservice.Repositry.DatabaseAccesorId;

        // TODO: Check accounts
        if (transaction is null)
        {
            transaction = entityDetailservice.BeginTransaction(isolationLevel: IsolationLevel.ReadCommitted);
        }
        try
        {
            var ent = entryRequest.CreateEntry();
            ent.Tag = tagType;
            //ent.OprationType = oprationTypeEnum;
            //var entDet = ent.Details.Adapt<List<DtoEntryDetail>>();
            //ent.Details = null;
            await CoreAdd(ent, relaredId, relatedIndex);
            //entDet.ForEach(f =>
            //{
            //    f.EntryId = ent.Id;
            //});
            //await entityDetailservice.AddRange(entDet);

            if (action is not null)
            {
                await action.Invoke(transaction, ent);
            }

            if (autoCommit)
                transaction.Commit();
            //ent.Details = entDet;
            return ent.Adapt<DtoEntry>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "BeginTransaction");
            transaction.Rollback();
            return null;
        }


    }

    public async Task<PageList<DtoEntryDetail>> GetDetails(DateTime? from = null, DateTime? to = null, Identifier? accountId = null, int? pageIndex = null, int? pageSize = null, TagTypeEnum? tagType = null)
    {
        var entityDetailservice = _serviceProvider.GetRequiredService<IServiceBase<DtoEntryDetail, TEntryDetail>>();

        var w = entityDetailservice.Build();
        if (accountId is not null)
        {
            w = w.And(w => w.AccountId == accountId);
        }
        if (from.HasValue && to.HasValue)
        {
            w = w.And(w => w.EntryDate >= from.Value && w.EntryDate <= to.Value);
        }
        w = w.And(w => w.EntryTag == tagType);
        var all = await entityDetailservice.GetAllAsync(w, pageSize, pageIndex);
        return all;

    }

    public async Task<AccountInfo> GetLastDebitInfo(Identifier accountId, DateTime? from = null, DateTime? to = null, TagTypeEnum? tagType = null)
    {
        var entityDetailservice = _serviceProvider.GetRequiredService<IServiceBase<DtoEntryDetail, TEntryDetail>>();

        var where = entityDetailservice.Build();
        where = where.And(w => w.AccountId == accountId && w.Debit != 0);
        where = where.And(w => w.EntryTag == tagType);
        var info = await entityDetailservice
                .GetTopOneAsAsync(
                det => new AccountInfo(det.EntryDate, det.Debit, det.AccountName, det.EntryOprationType, det.EntryNumber), where);

        if (info is null)
        {
            return new AccountInfo(null, 0, "", OprationTypeEnum.None, 0);
        }

        return info;
    }

    public async Task<decimal> GetTotalDebitInfo(Identifier accountId, DateTime? from = null, DateTime? to = null, TagTypeEnum? tagType = null)
    {
        var entityDetailservice = _serviceProvider.GetRequiredService<IServiceBase<DtoEntryDetail, TEntryDetail>>();
        var where = entityDetailservice.Build();
        where = where.And(w => w.AccountId == accountId && w.Debit != 0);
        where = where.And(w => w.EntryTag == tagType);
        var info = await entityDetailservice
                .GetSumAsync(
                det => det.Debit + det.Account.OpeningDebtor, where);
        return info < 0 ? info * -1 : info;
    }

    public async Task<decimal> GetTotalCreditInfo(Identifier accountId, DateTime? from = null, DateTime? to = null, TagTypeEnum? tagType = null)
    {
        var entityDetailservice = _serviceProvider.GetRequiredService<IServiceBase<DtoEntryDetail, TEntryDetail>>();
        var where = entityDetailservice.Build();
        where = where.And(w => w.AccountId == accountId && w.Credit != 0);
        where = where.And(w => w.EntryTag == tagType);
        var info = await entityDetailservice
                .GetSumAsync(
                det => det.Credit + det.Account.OpeningCreditor, where);
        return info < 0 ? info * -1 : info;
    }

    public async Task<AccountInfo> GetLastCreditInfo(Identifier accountId, DateTime? from = null, DateTime? to = null, TagTypeEnum? tagType = null)
    {
        var entityDetailservice = _serviceProvider.GetRequiredService<IServiceBase<DtoEntryDetail, TEntryDetail>>();
        var where = entityDetailservice.Build();

        where = where.And(w => w.AccountId == accountId && w.Credit != 0);
        where = where.And(w => w.EntryTag == tagType);

        var info = await entityDetailservice
                .GetTopOneAsAsync(
                det => new AccountInfo(det.EntryDate, det.Credit, det.AccountName, det.EntryOprationType, det.EntryNumber), where);

        if (info is null)
        {
            return new AccountInfo(null, 0, "", OprationTypeEnum.None, 0);
        }
        return info;
    }

    public async Task<decimal> GetBalance(Identifier accountId, DateTime? from = null, DateTime? to = null, TagTypeEnum? tagType = null)
    {

        var accountService = _serviceProvider.GetRequiredService<IServiceBase<DtoAccount, TAccount>>();
        var entityDetailservice = _serviceProvider.GetRequiredService<IServiceBase<DtoEntryDetail, TEntryDetail>>();
        var where = entityDetailservice.Build();
        where = where.And(w => w.AccountId == accountId);
        if (from is not null && to is not null)
        {
            where = where.And(w => w.EntryDate >= from && w.EntryDate <= to);
        }
        where = where.And(w => w.EntryTag == tagType);
        var accountInfo = await accountService.GetOneAsAsync(s => new { Balance = s.OpeningDebtor - s.OpeningCreditor }, w => w.Id == accountId);
        var total = await entityDetailservice.GetSumAsync(s => s.Debit - s.Credit, where);
        return accountInfo.Balance + total;
    }

    public async Task<MultiAccountQuery[]> GetBalance(Identifier[] accountsId, DateTime? from = null, DateTime? to = null, TagTypeEnum? tagType = null)
    {

        var accountService = _serviceProvider.GetRequiredService<IServiceBase<DtoAccount, TAccount>>();
        var entityDetailservice = _serviceProvider.GetRequiredService<IServiceBase<DtoEntryDetail, TEntryDetail>>();
        var where = entityDetailservice.Build();
        var accountWhere = accountService.Build();
        where = where.And(w => ((IEnumerable<Identifier>)accountsId).Contains(w.AccountId));
        accountWhere = accountWhere.And(w => ((IEnumerable<Identifier>)accountsId).Contains(w.Id));
        if (from is not null && to is not null)
        {
            where = where.And(w => w.EntryDate >= from && w.EntryDate <= to);
        }
        where = where.And(w => w.EntryTag == tagType);
        var accountInfo = await accountService.GetAllAsAsync(s => new { Balance = s.OpeningDebtor - s.OpeningCreditor, s.Id }, accountWhere);
        var total = await entityDetailservice.GetAllAsAsync(
            s =>
            new { AccountId = s.Account.Id, s.Debit, s.Credit }
        , where);
        return total
            .Items
            .GroupBy(g => g.AccountId)
            .Select(s => new MultiAccountQuery()
            {
                AccountId = s.Key,
                Balance = s.Sum(s => (s.Debit - s.Credit))
            })
            .Join(accountInfo.Items
                , _ => _.AccountId
                , _ => _.Id
                , (x, y) =>
                {
                    x.Balance += y.Balance;
                    return x;
                }
            )
            .ToArray();
    }

    public async Task<List<DtoStatment>> GetNonPayedDebtiobsAccounts2(DateTime? from = null, DateTime? to = null, Identifier? accountId = null, TagTypeEnum? tagType = null)
    {
        var accountService = _serviceProvider.GetRequiredService<IServiceBase<DtoAccount, TAccount>>();
        var entityDetailService = _serviceProvider.GetRequiredService<IServiceBase<DtoEntryDetail, TEntryDetail>>();

        //  TODO: FIX the quey size limit
        var limitAccounts = (await accountService.GetAllAsAsync(s => s.Id, w => w.ParentId == accountId)).Items;
        var allDebtions = (await entityDetailService
                .GetAllAsAsync(w => new { w.AccountId, w.Debit }, x => x.Debit > 0 && limitAccounts.Contains(x.AccountId))).Items;
        var allDebtionsTotal = allDebtions.GroupBy(w => w.AccountId).Select(w => new { AccountId = w.Key, TotalDebit = w.Sum(ss => ss.Debit) });
        var debitAccounts = allDebtionsTotal.Select(ww => ww.AccountId);

        var where = entityDetailService.Build();
        where = where.And(w => w.EntryDate >= from && to >= w.EntryDate && (!debitAccounts.Contains(w.AccountId) && w.Credit == 0));
        where = where.And(w => w.EntryTag == tagType);
        var movments = await entityDetailService.GetAllAsAsync(
                s => new { s.AccountId, s.AccountName }, where);
        return movments.Items.Select(s => new DtoStatment()
        {
            AccountId = s.AccountId,
            AccountName = s.AccountName,
        }).ToList();
    }

    public async Task<List<DtoStatment>> GetNonPayedDebtiobsAccounts(DateTime? from = null, DateTime? to = null, Identifier? accountId = null, TagTypeEnum? tagType = null)
    {
        var accountService = _serviceProvider.GetRequiredService<IServiceBase<DtoAccount, TAccount>>();
        var entityDetailService = _serviceProvider.GetRequiredService<IServiceBase<DtoEntryDetail, TEntryDetail>>();

        //  TODO: FIX the quey size limit
        var limitAccounts = (await accountService.GetAllAsAsync(s => s.Id, w => w.ParentId == accountId)).Items;
        var allDebtions = (await entityDetailService
                .GetAllAsAsync(w => new { w.AccountId, w.Debit }, x => x.Debit > 0 && limitAccounts.Contains(x.AccountId))).Items;
        var allDebtionsTotal = allDebtions.GroupBy(w => w.AccountId).Select(w => new { AccountId = w.Key, TotalDebit = w.Sum(ss => ss.Debit) });
        var debitAccounts = allDebtionsTotal.Select(ww => ww.AccountId);

        var where = entityDetailService.Build();
        where = where.And(w => w.EntryDate >= from && w.EntryDate <= to && !debitAccounts.Contains(w.AccountId) && w.Credit > 0);
        where = where.And(w => w.EntryTag == tagType);

        var movments = await entityDetailService.GetAllAsAsync(
                s => new { s.AccountId, s.AccountName }, where);
        return movments.Items.Select(s => new DtoStatment()
        {
            AccountId = s.AccountId,
            AccountName = s.AccountName,
        }).ToList();
    }

    public async Task<PageList<DtoStatment>> GetDetailsStatements(DateTime? from = null, DateTime? to = null, Identifier? accountId = null, bool collecttTheQuntitiesBeforTime = false, int? pageIndex = null, int? pageSize = null, Identifier? parentId = null, TagTypeEnum? tagType = null, StatementAccountType? statementAccountType = null)
    {
        var accountService = _serviceProvider.GetRequiredService<IAccountModuleService>();
        var entityDetailservice = _serviceProvider.GetRequiredService<IServiceBase<DtoEntryDetail, TEntryDetail>>();

        var totalBefore = 0m;
        var addBalance = false;
        var allExpression = entityDetailservice.Build2();
        var beforExpression = entityDetailservice.Build();
        if (accountId is not null)
        {
            addBalance = true;
            allExpression = allExpression.And(w => w.AccountId == accountId);
            beforExpression = beforExpression.And(w => w.AccountId == accountId);
        }
        //if (parentId is not null)
        //{
        //    allExpression = allExpression.And(w => w.AccountIdAccountId == parentId);
        //    //beforExpression = beforExpression.And(w => w.AccountId == accountId);
        //}
        if (from is not null && to is not null)
        {
            if (collecttTheQuntitiesBeforTime)
            {
                beforExpression = beforExpression.And(det => det.EntryDate < from);
                totalBefore = await entityDetailservice.GetSumAsync(s => s.Credit - s.Debit, beforExpression);
            }
            allExpression = allExpression.And(w => w.Entry.Date >= from && w.Entry.Date <= to);
        }

        if (statementAccountType is not null && statementAccountType is not StatementAccountType.All)
        {
            allExpression = allExpression.And(w => w.Account.StatementAccountType == statementAccountType);
        }
        allExpression = allExpression.And(w => w.Entry.Tag == tagType);

        var all = await entityDetailservice.GetAllAsEntityAsync(s => new DtoStatment
        {
            AccountId = s.AccountId,
            AccountName = s.Account.Name,
            Credit = s.Credit,
            Debit = s.Debit,
            Notes = s.Note,
            Balance = 0,
            Number = s.Entry.Number,
            OprationType = s.Entry.OprationType,
            EntryDateString = s.Entry.Date.ToString("yyyy-MM-dd HH:mm"),
            DisplayNumber = s.Entry.DisplayNumber,


        }, allExpression, pageSize, pageIndex);
        var allItems = all.Items.ToList();//.Adapt<List< DtoStatment>>();

        if (false) // addBalance
        {
            //allItems.All(w=> w.AccountId == accountId)
            var balance = allItems.Sum(s => s.Credit - s.Debit);
            allItems.Insert(0, new DtoStatment()
            {
                AccountName = "رصيد نهائي",
                Debit = balance < 0 ? balance * -1 : 0,
                Credit = balance > 0 ? balance : 0,
            });
        }
        // collecttTheQuntitiesBeforTime
        if (false)
        {
            allItems.Insert(0, new DtoStatment()
            {
                CollecttTheQuntitiesBeforTime = true,
                AccountName = "اجمالي ماقبل الفترة",
                Debit = totalBefore < 0 ? totalBefore * -1 : 0,
                Credit = totalBefore > 0 ? totalBefore : 0,
                Balance = totalBefore,
            });
        }

        if (accountId is not null)
        {
            var selectedAccountInfo = await accountService.GetOneAsAsync(s => new { s.OpeningDebtor, s.OpeningCreditor }, w => w.Id == accountId.Value);
            allItems.Add(new DtoStatment()
            {
                AccountId = accountId.Value,
                CollecttTheQuntitiesBeforTime = true,
                AccountName = "رصيد افتتاحي",
                Debit = selectedAccountInfo.OpeningDebtor,
                Credit = selectedAccountInfo.OpeningCreditor,
                Balance = selectedAccountInfo.OpeningDebtor - selectedAccountInfo.OpeningCreditor,
                OprationType = OprationTypeEnum.OpeningBalance,
            });
        }

        all.Items = allItems;
        return all;
        //return new PageList<DtoStatment>()
        //{
        //    Items = allItems
        //};

    }

    public async Task RemoveDetails(Identifier entryId, bool sofrDelete = false)
    {
        var entityDetailservice = _serviceProvider.GetRequiredService<IServiceBase<DtoEntryDetail, TEntryDetail>>();
        await entityDetailservice.Remove(w => w.EntryId == entryId, sofrDelete);
    }

    public async Task<DtoStatmentInfo> GetFastStatementInfo(Identifier accountId, DateTime? from = null, DateTime? to = null, TagTypeEnum? tagType = null)
    {
        var entityDetailservice = _serviceProvider.GetRequiredService<IServiceBase<DtoEntryDetail, TEntryDetail>>();

        // Start the query
        var q = new StringBuilder("SELECT SUM(Credit) AS Credit, SUM(Debit) AS Debit FROM `tentrydetail` AS t  ");

        q.Append(" INNER JOIN `tentry` AS e ON t.EntryId = e.ID ");

        q.Append(" WHERE (NOT t.ISDELETED) ");
        q.Append(" AND e.Tag = @tagType ");

        q.Append(" AND t.accountId = @accountId");

        // Parameters
        var parameters = new List<object> { new MySqlParameter("@accountId", accountId.Value) };

        // Add date filtering if provided
        if (from.HasValue)
        {
            q.Append(" AND t.Date >= @fromDate");
            parameters.Add(new MySqlParameter("@fromDate", from.Value));
        }

        if (to.HasValue)
        {
            q.Append(" AND t.Date <= @toDate");
            parameters.Add(new MySqlParameter("@toDate", to.Value));
        }

        // Execute the query using the parameterized query and parameters
        var result = await entityDetailservice.ExcuteQuery<DtoStatmentInfo>(q.ToString(), parameters.ToArray());

        return result;
    }

    public async Task<IEnumerable<DtoStatmentInfo>> GetFastStatementListInfo(Identifier? accountId = null, DateTime? from = null, DateTime? to = null, TagTypeEnum? tagType = null)
    {
        var entityDetailservice = _serviceProvider.GetRequiredService<IServiceBase<DtoEntryDetail, TEntryDetail>>();

        // Start the query
        var q = new StringBuilder("SELECT  " +
            "SUM(Credit) AS Credit, " +
            "SUM(Debit) AS Debit," +
            "a.Id as AccountId," +
            "a.Name" +
            " FROM `tentrydetail` AS t  ");

        q.Append(" INNER JOIN `tentry` AS e ON t.EntryId = e.ID ");
        q.Append(" INNER JOIN `accounts` AS a ON t.AccountId = a.ID ");

        q.Append(" WHERE (NOT t.ISDELETED) ");
        q.Append(" AND e.Tag = @tagType ");


        // Parameters
        var parameters = new List<object> { new MySqlParameter("@tagType", tagType) };


        // Add date filtering if provided
        if (accountId.HasValue)
        {
            q.Append(" AND t.accountId = @accountId");
            new MySqlParameter("@accountId", accountId.Value);
        }

        // Add date filtering if provided
        if (from.HasValue)
        {
            q.Append(" AND t.Date >= @fromDate");
            parameters.Add(new MySqlParameter("@fromDate", from.Value));
        }

        if (to.HasValue)
        {
            q.Append(" AND t.Date <= @toDate");
            parameters.Add(new MySqlParameter("@toDate", to.Value));
        }
        q.AppendLine(" GROUP BY a.Id ,a.Name ");

        // Execute the query using the parameterized query and parameters
#if DEBUG
        _logger.LogTrace(q.ToString());
#endif
        var result = await entityDetailservice.ExcuteListQuery<DtoStatmentInfo>(q.ToString(), parameters.ToArray());

        return result;
    }

    public async Task<IEnumerable<DtoStatmentInfo>> GetFastStatementDeptionListInfo(Identifier? accountId = null, DateTime? from = null, DateTime? to = null, TagTypeEnum? tagType = null)
    {
        var entityDetailservice = _serviceProvider.GetRequiredService<IServiceBase<DtoEntryDetail, TEntryDetail>>();

        // Start the query
        var q = new StringBuilder("SELECT " +
            "SUM(t.Credit) AS Credit, " +
            "SUM(t.Debit) AS Debit, " +
            "a.Id AS AccountId, " +
            "a.Name " +
            "FROM `tentrydetail` AS t ");

        // Joins
        q.Append("INNER JOIN `tentry` AS e ON t.EntryId = e.ID ");
        q.Append("INNER JOIN `accounts` AS a ON t.AccountId = a.ID ");

        // Conditions
        q.Append("WHERE t.ISDELETED = 0 ");  // Assuming 0 means not deleted
        q.Append("AND e.Tag = @tagType ");   // Filtering by entry type (expense, etc.)
        var parameters = new List<object> { new MySqlParameter("@tagType", tagType) };

        //if (accountId.HasValue)
        //{
        //    q.Append("AND t.AccountId = @accountId "); // Filtering by Employee Account
        //    q.Append("AND t.Debit > 0 "); // Employee account should be debited (receiving expense)
        //    parameters.Add(new MySqlParameter("@accountId", accountId.Value.Value));
        //}

        //Ensure there is a corresponding credited expense account in the same entry
        q.Append("AND EXISTS ( ");
        q.Append("    SELECT 1 FROM `tentrydetail` AS te ");
        q.Append("    WHERE te.EntryId = t.EntryId ");
        q.Append("    AND te.Credit > 0 "); // Expense account should be credited Debit    Credit
        q.Append("    AND te.AccountId = @accountId "); // Expense account should be credited Debit    Credit
        q.Append(") ");
        parameters.Add(new MySqlParameter("@accountId", accountId.Value.Value));

        // Filter by date range if provided
        if (from.HasValue)
        {
            q.Append("AND e.Date >= @fromDate "); // Assuming e.Date is the correct column
            parameters.Add(new MySqlParameter("@fromDate", from.Value));
        }

        if (to.HasValue)
        {
            q.Append("AND e.Date <= @toDate ");
            parameters.Add(new MySqlParameter("@toDate", to.Value));
        }

        // Group by AccountId and Name
        q.AppendLine("GROUP BY a.Id, a.Name");

        // Execute the query using the parameterized query and parameters
#if DEBUG
        _logger.LogTrace(q.ToString());
#endif
        var result = await entityDetailservice.ExcuteListQuery<DtoStatmentInfo>(q.ToString(), parameters.ToArray());

        return result;
    }

    public async Task RemoveAllRelated(Identifier relatedId, OprationTypeEnum oprationTypeEnum, int relatedIndex, bool? sofrDelete = null)
    {
        var entityDetailservice = _serviceProvider.GetRequiredService<IServiceBase<DtoEntryDetail, TEntryDetail>>();
        //
        var toBeDeleted = await GetAllAsync(w => w.OprationType == oprationTypeEnum && w.RelatedId == relatedId && w.RelatedIndex == relatedIndex);
        var toBeDeletedEntriesIds = toBeDeleted.Items.Select(x => x.Id).ToList();
        await entityDetailservice.Remove(w => toBeDeletedEntriesIds.Contains(w.EntryId), sofrDelete);
        await Remove(w => toBeDeletedEntriesIds.Contains(w.Id), sofrDelete);
    }

}
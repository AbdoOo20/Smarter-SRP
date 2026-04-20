using System.Linq.Expressions;
using Fly.Core.Modules.C.Accounting.Services;
using Fly.Core.Modules.F.PurchaseAndSales.Models.DbModels;
using Fly.Framework.Server.Common.Controllers.CommandsBased;
using Fly.Framework.Server.Common.Localization;
using Fly.Framework.Server.Persistence;
using Fly.Framework.Shared.Common.Http;
using Fly.Framework.Shared.Common.ValueTypes;
using Fly.Shared.Modules.Accounting.Dtos;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Misc;
using Fly.Shared.Modules.PurchaseAndSales.Features;
using Fly.Shared.Modules.PurchaseAndSales.Requests;
using LinqKit;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Fly.Core.Modules.F.PurchaseAndSales.Controller;

public class BankBranchController
    // : CustomControllerBase
    : CustomControllerFeatrueBased<DtoBankBranch, TBankBranch, BankBranchQueryRequest, BankBranchDeleteRequest, BankBranchCreateUpdateRequest>
        // : CustomControllerFeatrueBased<DtoAccount>
        , IBankBranchFeatures
{
    private readonly IServiceBase<DtoBankBranch, TBankBranch> _service;

    public BankBranchController(ILocalizationService localizationService, ILogger<BankBranchController> logger, IMediator mediator, IServiceBase<DtoBankBranch, TBankBranch> service,
        AutoMapper.IMapper mapper,
        MapsterMapper.IMapper mapster)
        : base(localizationService, logger, mediator, service, mapper, mapster)
    {
        _service = service;

        OnCreateUpdate += BackBranchOnCreateUpdate;
    }

    [NonAction]
    public override Expression<Func<DtoBankBranch, bool>> GetWhere(Expression<Func<DtoBankBranch, bool>> expression, BankBranchQueryRequest query)
    {
        if (!string.IsNullOrEmpty(query.SearchText))
            expression = expression.And(x => x.Name.Contains(query.SearchText));
        return base.GetWhere(expression, query);
    }

    [HttpGet("banks")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupBanks([FromQuery] BankQueryRequest request)
        =>
            GetSelectableList<DtoBank, TBank>(x => x.Name);

    [NonAction]
    private async ValueTask<DtoBankBranch> BackBranchOnCreateUpdate(BankBranchCreateUpdateRequest q)
    {
        var bankService = _service.MakeChildService<DtoBank, TBank>();
        var accountService = _service.MakeChildService<IAccountModuleService>();

        var dto = q.Adapt<DtoBankBranch>();

        _service.BeginTransaction(isolationLevel: System.Data.IsolationLevel.ReadCommitted);

        if (q.IsNew())
        {
            var bankAccountId = await bankService.GetById(q.BankId);
            if (bankAccountId is null)
                throw new Exception("bank account not ready - 1"); // TODO: Localization

            var baseAccount = await accountService.GetAsync(w => w.Id == bankAccountId.AccountId);
            if (baseAccount is null)
                throw new Exception("bank account not ready - 2"); // TODO: Localization

            var nextNumber = 
                await accountService.GetMaxAsync(c => c.Number, w => w.Number.ToString()
                    .StartsWith(baseAccount.Number.ToString()));
            var bankNumber = baseAccount.Number;
            
            var rawStr = nextNumber.ToString();
            if (rawStr.Length == 16)
            {
                var basePart = rawStr.Substring(0, 4);     // "1232"
                var counter = rawStr.Substring(12, 4);     // "0007"

                // You can now construct the new number as needed
                nextNumber = long.Parse($"{basePart}{counter}"); // "12320007"
                Logger.LogInformation("Cleaned number: {0}", nextNumber);
            }
            else
            {
                Logger.LogWarning("Consistent: {0}", rawStr);
            }
            
            var rawStrB = bankNumber.ToString();
            if (rawStrB.Length == 16)
            {
                var basePart = rawStrB.Substring(0, 4);     // "1232"
                var counter = rawStrB.Substring(12, 4);     // "0007"

                // You can now construct the new number as needed
                bankNumber = long.Parse($"{basePart}{counter}"); // "12320007"
                Logger.LogInformation("Cleaned bank number: {0}", bankNumber);
            }
            else
            {
                Logger.LogWarning("Consistent bank: {0}", rawStr);
            }
            
            Logger.LogInformation("Base number: {0}", baseAccount.Number);
            
            if (nextNumber == bankNumber)
            {
                var n = 1.ToString("D4");
                nextNumber = long.Parse($"{nextNumber}{n}");
            }
            else
            {
                var las = nextNumber.ToString().Substring(bankNumber.ToString().Length);
                var a = (long.Parse(las) + 1).ToString("D4");
                nextNumber = long.Parse($"{bankNumber}{a}");
            }

            var account = await accountService.Add(new DtoAccount()
            {
                Name = dto.Name,
                Natural = baseAccount.Natural,
                Number = nextNumber,
                AccountCat = baseAccount.AccountCat,
                CurrencyId = baseAccount.CurrencyId,
                ParentId = baseAccount.Id,
                OpeningCreditor = q.AccountOpeningCreditor,
                OpeningDebtor = q.AccountOpeningDebtor,
            });
            dto.AccountId = account.Id;
            await _service.Add(dto);
        }
        else
        {
            var db = await _service.GetById(dto.Id);
            q.Adapt(db);
            dto = await _service.Update(db);
            await accountService.UpdateOpeningBalance(db.AccountId, q.AccountOpeningDebtor, q.AccountOpeningCreditor);
        }

        _service.CommitTransaction();
        return dto;
    }
}
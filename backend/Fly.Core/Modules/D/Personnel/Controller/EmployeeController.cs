using Fly.Core.Modules.C.Accounting.Services;
using Fly.Core.Modules.D.Personnel.DbModel;
using Fly.Core.Modules.F.PurchaseAndSales;
using Fly.Core.Persistence.Ef.Models;
using Fly.Core.Persistence.Ef.Models.Global;
using Fly.Core.Persistence.Ef.Models.Personnel;
using Fly.Framework.Server.Common.Controllers.CommandsBased;
using Fly.Framework.Server.Common.Localization;
using Fly.Framework.Server.Persistence;
using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Http;
using Fly.Framework.Shared.Common.Requests;
using Fly.Framework.Shared.Common.ValueTypes;
using Fly.Shared.Modules.Accounting.Dtos;
using Fly.Shared.Modules.Accounting.Enums;
using Fly.Shared.Modules.Accounting.Requests;
using Fly.Shared.Modules.Global.Dtos;
using Fly.Shared.Modules.Personnel.Dtos;
using Fly.Shared.Modules.Personnel.Features;
using Fly.Shared.Modules.Personnel.Requests;
using LinqKit;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using Fly.Shared.Modules.Financial.Enums;

namespace Fly.Core.Modules.D.Personnel.Controller;

public class EmployeeController
        :
        //ApiSimpleController<DtoAccount, TAccount>,
        CustomControllerFeatrueBased<DtoEmployee, TEmployee, EmployeeQueryRequest, DeleteRequestBase, CreateUpdateEmployeeRequest>,
        IEmployeeFeatures
{
    public EmployeeController(ILocalizationService localizationService, ILogger<CustomControllerFeatrueBased<DtoEmployee, TEmployee, EmployeeQueryRequest, DeleteRequestBase, CreateUpdateEmployeeRequest>> logger, IMediator mediator, IServiceBase<DtoEmployee, TEmployee> service,
        AutoMapper.IMapper mapper,
        MapsterMapper.IMapper mapster) : base(localizationService, logger, mediator, service, mapper, mapster)
    {
        OnCreateUpdate += EmployeeController_OnCreateUpdate;
    }

    private async ValueTask<DtoEmployee> EmployeeController_OnCreateUpdate(CreateUpdateEmployeeRequest q)
    {
        var configService = _service.MakeChildService<DtoConfig, TConfig>();
        var accountService = _service.MakeChildService<IAccountModuleService>();

        var supplyer = q.Adapt<DtoEmployee>();

        _service.BeginTransaction(isolationLevel: System.Data.IsolationLevel.ReadCommitted);

        if (q.IsNew())
        {
            var suplAccountId = await configService.GetOneAsAsync(x => x.EmployeesAccount);
            if (suplAccountId is null)
                throw new Exception("Employeer account not ready"); // TODO: Localization

            var supplyerBaseAccount = await accountService.GetAsync(w => w.Id == Identifier.Parse(suplAccountId));
            if (supplyerBaseAccount is null)
                throw new Exception("Employeer account not ready"); // TODO: Localization

            var nextNumber = await accountService.GetMaxAsync(c => c.Number, w => w.Number.ToString().StartsWith(supplyerBaseAccount.Number.ToString()));

            if (nextNumber == supplyerBaseAccount.Number)
            {
                var n = 1.ToString("D4");
                nextNumber = long.Parse($"{nextNumber}{n}");
            }
            else
            {
                var las = nextNumber.ToString().Substring(supplyerBaseAccount.Number.ToString().Length);
                var a = (long.Parse(las) + 1).ToString("D4");
                nextNumber = long.Parse($"{supplyerBaseAccount.Number}{a}");
            }

            var account = await accountService.Add(new DtoAccount()
            {
                Name = supplyer.Name,
                Natural = supplyerBaseAccount.Natural,
                Number = nextNumber,
                AccountCat = supplyerBaseAccount.AccountCat,
                CurrencyId = supplyerBaseAccount.CurrencyId,
                ParentId = supplyerBaseAccount.Id,
                StatementAccountType = StatementAccountType.Employees
            });
            supplyer.AccountId = account.Id;
            await _service.Add(supplyer);
        }
        else
        {
            var db = await _service.GetById(supplyer.Id);
            q.Adapt(db);
            supplyer = await _service.Update(db);
            await accountService.UpdateOpeningBalance(db.AccountId, q.AccountOpeningDebtor, q.AccountOpeningCreditor);
        }

        _service.CommitTransaction();
        return supplyer;
    }

    [HttpGet("accounts")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupAccounts([FromQuery] AccountQueryRequest request)
        =>
        GetSelectableList<DtoAccount, TAccount>(x => x.Name);

    [HttpGet("departments")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupDepartments([FromQuery] DepartmentQueryRequest request)
        =>
        GetSelectableList<DtoDepartment, TDepartment>(x => x.Name);

    public override Expression<Func<DtoEmployee, bool>> GetWhere(Expression<Func<DtoEmployee, bool>> expression, EmployeeQueryRequest query)
    {
        if (!string.IsNullOrEmpty(query.SearchText))
        {
            query.SearchText = query.SearchText.Trim();

            expression = expression.And((x) =>

                x.Name.Contains(query.SearchText) |
                x.Email.Contains(query.SearchText) |
                x.Phone.Contains(query.SearchText) |
                x.Identifier.Contains(query.SearchText) |
                x.DepartmentName.Contains(query.SearchText)
            );
        }

        return base.GetWhere(expression, query);
    }

    [HttpPost("CountingSalary/{mounthNumber}")]
    public ValueTask<HttpResponseWrapper<TypeOf<bool>>> PostCountingSalary(int mounthNumber)
    => TryOne<TypeOf<bool>>(async _ =>
    {
        var entryService = _service.MakeChildService<IEntryService>();
        var configService = _service.MakeChildService<DtoConfig, TConfig>();
        var accountService = _service.MakeChildService<DtoAccount, TAccount>();
        var accountingFinacialService = _service.MakeChildService<AccountingFinacial>();
        var empAssigments = _service.MakeChildService<DtoEmployeeAssignment, TEmployeeAssignment>();
        var emplSlatyCounting = _service.MakeChildService<DtoEmployeeSalaryCounting, TEmployeeSalaryCounting>();
        var emplSlatyCountingPermit = _service.MakeChildService<DtoEmployeeSalaryCountingPermit, TEmployeeSalaryCountingPermit>();

        List<DtoEmployeeSalaryCounting> salaryCountings = [];


        var expenseAccountInfo = await configService.GetOneAsAsync(w => w.ExpenseAccount);
        if (expenseAccountInfo is null) return _.Problem("ExpenseAccount not ready");
        Identifier expenseAccountId = Identifier.Parse(expenseAccountInfo);
        var expencesAccount = await accountService.GetAsync(W => W.Id == expenseAccountId);
        if (expencesAccount is null) return _.Problem("ExpenseAccount not exists");

        _service.BeginTransaction();
        long nextNumber = 0;
        var old = await emplSlatyCountingPermit.GetAsync(w => w.WorkMounth == mounthNumber);

        if (old is null)
        {
            nextNumber = await emplSlatyCountingPermit.NextNumber(w => w.Number);
            old = new()
            {
                Number = (int)nextNumber,
                WorkMounth = mounthNumber,
                WorkTime = DateTime.Now,
            };
            await emplSlatyCountingPermit.Add(old);

            await Counting();
        }
        else
        {
            await entryService.RemoveAllRelated(old.Id, OprationTypeEnum.SalaryConting, PurchaseAndSalesConstants.TransactionOprationType__SalariesRelated);
            nextNumber = old.Number;
            await emplSlatyCounting.Remove(w => w.EmployeeSalaryCountingPermitId == old.Id);

            await Counting();
        }

        _service.CommitTransaction();
        return _.Success(new TypeOf<bool>(true));

        async Task Counting()
        {

            var assigmentsInfo = await empAssigments.GetAllAsync();
            //await empAssigments.GetAllAsAsync(s => new
            //{
            //    s.EmployeeId,
            //    s.EmployeeName,
            //    s.Amount,
            //    s.StartDate,
            //    s.EndDate,
            //    s.Note,
            //});

            var finacialTransaction = accountingFinacialService
                .BeginFinacialTransaction(DateTime.Now, // TODO: check date, invoice or now
                    OprationTypeEnum.SalaryConting,
                    PurchaseAndSalesConstants.TransactionOprationType__SalariesRelated,
                    old.Id,
                    nextNumber,
                    l.GetFormatedText(l["salaries-for"], mounthNumber.ToString()));


            decimal totalPayments = 0;
            List<Identifier> emplAccounts = [];
            List<decimal> emplAccountsAmount = [];
            foreach (var emplAssignment in assigmentsInfo.Items)
            {
                // get all deption
                decimal totalDebtion = 0, totalCredit = emplAssignment.Amount, total = 0;
                total = totalDebtion - totalCredit;

                if (total < 0) total *= -1;

                totalPayments += total;
                salaryCountings.Add(new DtoEmployeeSalaryCounting()
                {
                    TotalCredit = totalCredit,
                    TotalDebit = totalDebtion,
                    Salaty = emplAssignment.Amount,
                    Total = total,
                    EmployeeSalaryCountingPermitId = old.Id,
                });
                emplAccountsAmount.Add(total);
                emplAccounts.Add(emplAssignment.EmployeeAccountId);
                finacialTransaction.AppendCreditEntry(emplAssignment.EmployeeAccountId, total);
            }

            finacialTransaction.AppendDebitEntry(expencesAccount.Id, totalPayments);
            
            await finacialTransaction.ComplateTransaction();
        }

    });
}

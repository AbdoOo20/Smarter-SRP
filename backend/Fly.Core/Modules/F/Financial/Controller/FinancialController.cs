using MediatR;
using Fly.Core.Modules.F.PurchaseAndSales.Models.DbModels;
using Fly.Core.Persistence.Ef.Models;
using Fly.Framework.Shared.Common.Http;
using Fly.Framework.Shared.Common.ValueTypes;
using Fly.Shared.Modules.Accounting.Dtos;
using Fly.Shared.Modules.Accounting.Requests;
using Fly.Shared.Modules.PurchaseAndSales.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Fly.Shared.Modules.Financial.Features;
using Fly.Shared.Modules.Financial.Requests;
using Fly.Shared.Modules.Financial.Dtos;
using Fly.Shared.Modules.Personnel.Dtos;
using Fly.Core.Persistence.Ef.Models.Personnel;
using Fly.Core.Persistence.Ef.Models.Store;
using Fly.Shared.Modules.Store.Dtos;
using Fly.Framework.Server.Common.Controllers.Base;
using Fly.Framework.Server.Persistence.Common;
using Fly.Framework.Shared.Common.Dtos;
using System.Linq.Expressions;
using Fly.Core.Modules.C.Accounting.Services;
using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.Accounting.Enums;
using Fly.Shared.Modules.Store.Requests;
using Fly.Shared.Modules.Marketers.Dtos;
using Fly.Core.Modules.F.Marketers.Models.DbModels;
using Fly.Framework.Server.Common.Localization;
using Fly.Shared.Modules.Global.Dtos;
using Fly.Core.Persistence.Ef.Models.Global;
using FluentValidation;
using Ardalis.Result.FluentValidation;
using Fly.Framework.Shared.Common.Collections;
using LinqKit;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Misc;

namespace Fly.Core.Modules.F.Financial.Controller;
public partial class FinancialController
        :
        //ApiSimpleController<DtoAccount, TAccount>,
        CustomControllerBase,
        IFinancialFeatures
{

    public class RSimpleRequestValidator : AbstractValidator<RSimpleRequest>
    {
        public RSimpleRequestValidator()
        {
            RuleFor(prp => prp.Amount)
                .GreaterThan(0).WithMessage("")
                .NotEmpty().WithMessage("")
                .NotNull().WithMessage("")
            ;
            RuleFor(prp => prp.FromAccount)
                .NotEmpty().WithMessage("")
                .NotNull().WithMessage("")
            ;
            RuleFor(prp => prp.ToAccount)
                .NotEmpty().WithMessage("")
                .NotNull().WithMessage("")
            ;
        }
    }

    public class ClientFinancialRequestValidator : AbstractValidator<ClientFinancialRequest>
    {
        public ClientFinancialRequestValidator(ILocalizationService l) : base()
        {
            RuleFor(prp => prp.Amount)
                .GreaterThan(0).WithMessage(l.GetFormatedText("v-grator-than", nameof(ClientFinancialRequest.Amount), "0"))
                .NotEmpty().WithMessage(l.GetFormatedText("validation-empty", nameof(ClientFinancialRequest.Amount)))
                .NotNull().WithMessage(l.GetFormatedText("validation-empty", nameof(ClientFinancialRequest.Amount)))
            ;
            RuleFor(prp => prp.FromAccount)
                .NotEmpty().WithMessage("x")
                .NotNull().WithMessage("x")
            ;
            RuleFor(prp => prp.ToAccount)
                .NotEmpty().WithMessage("y")
                .NotNull().WithMessage("y")
            ;
        }
    }
    public class EmployeeFinancialRequestValidator : AbstractValidator<EmployeeFinancialRequest>
    {
        public EmployeeFinancialRequestValidator(ILocalizationService l) : base()
        {
            RuleFor(prp => prp.Amount)
                .GreaterThan(0).WithMessage(l.GetFormatedText("v-grator-than", nameof(EmployeeFinancialRequest.Amount), "0"))
                .NotEmpty().WithMessage(l.GetFormatedText("validation-empty", nameof(EmployeeFinancialRequest.Amount)))
                .NotNull().WithMessage(l.GetFormatedText("validation-empty", nameof(EmployeeFinancialRequest.Amount)))
            ;
            RuleFor(prp => prp.FromAccount)
                .NotEmpty().WithMessage("x")
                .NotNull().WithMessage("x")
            ;
            RuleFor(prp => prp.ToAccount)
                .NotEmpty().WithMessage("y")
                .NotNull().WithMessage("y")
            ;
        }
    }

    public class AccountFinancialRequestValidator : AbstractValidator<AccountFinancialRequest>
    {
        public AccountFinancialRequestValidator(ILocalizationService l) : base()
        {
            RuleFor(prp => prp.Amount)
                .GreaterThan(0).WithMessage(l.GetFormatedText("v-grator-than", nameof(ClientFinancialRequest.Amount), "0"))
                .NotEmpty().WithMessage(l.GetFormatedText("validation-empty", nameof(ClientFinancialRequest.Amount)))
                .NotNull().WithMessage(l.GetFormatedText("validation-empty", nameof(ClientFinancialRequest.Amount)))
            ;
            RuleFor(prp => prp.FromAccount)
                .NotEmpty().WithMessage("x")
                .NotNull().WithMessage("x")
            ;
            RuleFor(prp => prp.ToAccount)
                .NotEmpty().WithMessage("y")
                .NotNull().WithMessage("y")
            ;
        }
    }

    public class ExpenseFinancialRequestValidator : AbstractValidator<ExpenseFinancialRequest>
    {
        public ExpenseFinancialRequestValidator(ILocalizationService l) : base()
        {
            RuleFor(prp => prp.Amount)
                .GreaterThan(0).WithMessage(l.GetFormatedText("v-grator-than", nameof(ClientFinancialRequest.Amount), "0"))
                .NotEmpty().WithMessage(l.GetFormatedText("validation-empty", nameof(ClientFinancialRequest.Amount)))
                .NotNull().WithMessage(l.GetFormatedText("validation-empty", nameof(ClientFinancialRequest.Amount)))
            ;
            RuleFor(prp => prp.FromAccount)
                .NotEmpty().WithMessage("x")
                .NotNull().WithMessage("x")
            ;
            RuleFor(prp => prp.ToAccount)
                .NotEmpty().WithMessage("y")
                .NotNull().WithMessage("y")
            ;
        }
    }

    public class RepresentativeFinancialRequestValidator : AbstractValidator<RepresentativeFinancialRequest>
    {
        public RepresentativeFinancialRequestValidator(ILocalizationService l) : base()
        {
            RuleFor(prp => prp.Amount)
                .GreaterThan(0).WithMessage(l.GetFormatedText("v-grator-than", nameof(ClientFinancialRequest.Amount), "0"))
                .NotEmpty().WithMessage(l.GetFormatedText("validation-empty", nameof(ClientFinancialRequest.Amount)))
                .NotNull().WithMessage(l.GetFormatedText("validation-empty", nameof(ClientFinancialRequest.Amount)))
            ;
            RuleFor(prp => prp.FromAccount)
                .NotEmpty().WithMessage("FromAccount")
                .NotNull().WithMessage("FromAccount")
            ;
            RuleFor(prp => prp.ToAccount)
                .NotEmpty().WithMessage("ToAccount")
                .NotNull().WithMessage("ToAccount")
            ;
            RuleFor(prp => prp.ClientId)
                .NotEmpty().WithMessage("ClientId")
                .NotNull().WithMessage("ClientId")
            ;
            RuleFor(prp => prp.MarketerId)
                .NotEmpty().WithMessage("MarketerId")
                .NotNull().WithMessage("MarketerId")
            ;
        }
    }

    public class SupplierFinancialRequestValidator : AbstractValidator<SupplierFinancialRequest>
    {
        public SupplierFinancialRequestValidator(ILocalizationService l) : base()
        {
            RuleFor(prp => prp.Amount)
                .GreaterThan(0).WithMessage(l.GetFormatedText("v-grator-than", nameof(ClientFinancialRequest.Amount), "0"))
                .NotEmpty().WithMessage(l.GetFormatedText("validation-empty", nameof(ClientFinancialRequest.Amount)))
                .NotNull().WithMessage(l.GetFormatedText("validation-empty", nameof(ClientFinancialRequest.Amount)))
            ;
            RuleFor(prp => prp.FromAccount)
                .NotEmpty().WithMessage("FromAccount")
                .NotNull().WithMessage("FromAccount")
            ;
            RuleFor(prp => prp.ToAccount)
                .NotEmpty().WithMessage("ToAccount")
                .NotNull().WithMessage("ToAccount")
            ;
        }
    }

    public class MarketerPercentRequestValidator : AbstractValidator<MarketerPercentRequest>
    {
        public MarketerPercentRequestValidator(ILocalizationService l) : base()
        {
            RuleFor(prp => prp.Amount)
                .GreaterThan(0).WithMessage(l.GetFormatedText("v-grator-than", nameof(ClientFinancialRequest.Amount), "0"))
                .NotEmpty().WithMessage(l.GetFormatedText("validation-empty", nameof(ClientFinancialRequest.Amount)))
                .NotNull().WithMessage(l.GetFormatedText("validation-empty", nameof(ClientFinancialRequest.Amount)))
            ;
            RuleFor(prp => prp.FromAccount)
                .NotEmpty().WithMessage("FromAccount")
                .NotNull().WithMessage("FromAccount")
            ;
            RuleFor(prp => prp.ToAccount)
                .NotEmpty().WithMessage("ToAccount")
                .NotNull().WithMessage("ToAccount")
            ;
        }
    }

    public class DiscountItemBasedRequestValidator : AbstractValidator<DiscountItemBasedRequest>
    {
        public DiscountItemBasedRequestValidator(ILocalizationService l) : base()
        {
            RuleFor(prp => prp.Amount)
                .GreaterThan(0).WithMessage(l.GetFormatedText("v-grator-than", nameof(ClientFinancialRequest.Amount), "0"))
                .NotEmpty().WithMessage(l.GetFormatedText("validation-empty", nameof(ClientFinancialRequest.Amount)))
                .NotNull().WithMessage(l.GetFormatedText("validation-empty", nameof(ClientFinancialRequest.Amount)))
            ;
            //RuleFor(prp => prp.FromAccount)
            //    .NotEmpty().WithMessage("FromAccount")
            //    .NotNull().WithMessage("FromAccount")
            ;
            RuleFor(prp => prp.ToAccount)
                .NotEmpty().WithMessage("ToAccount")
                .NotNull().WithMessage("ToAccount")
            ;
        }
    }


    private readonly IEntryService _service;

    public FinancialController(
        ILocalizationService localizationService, ILogger<FinancialController> logger
        , IMediator mediator
        , AutoMapper.IMapper mapper
        , MapsterMapper.IMapper mapster
        , IEntryService service) : base(localizationService, logger, mediator, mapper, mapster)
    {
        _service = service;
    }

    [NonAction]
    protected ValueTask<HttpResponseWrapperList<DefaultLookup>> GetSelectableList<TDtoo, TEnto>(Func<TDtoo, string> textSelector, Expression<Func<TDtoo, bool>>? where = null, Expression<Func<TDtoo, object>>? order = null)
        where TDtoo : DtoBase
        where TEnto : TEntity, new()
    =>
        TryMeny<DefaultLookup>(async (opt) => opt.Success(await _service.MakeChildService<TDtoo, TEnto>().GetSelecListAsync(textSelector, where, order)));

    [NonAction]
    protected ValueTask<HttpResponseWrapperList<DefaultLookup>> GetSelectableList<TDtoo, TEnto>(Func<TDtoo, Identifier> idSelector, Func<TDtoo, string> textSelector, Expression<Func<TDtoo, bool>>? where = null, Expression<Func<TDtoo, object>>? order = null)
        where TDtoo : DtoBase
        where TEnto : TEntity, new()
    =>
        TryMeny<DefaultLookup>(async (opt) => opt.Success(await _service.MakeChildService<TDtoo, TEnto>().GetDefaultLookupAsync(idSelector, textSelector, where, order)));


    [NonAction]
    public ValueTask<HttpResponseWrapper<DtoStatment?>> PostSaveAsync(DtoStatment dto)
    => throw new NotImplementedException();


    #region GET


    [HttpGet("Accounts")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetAccounts(
        [FromQuery] GetAccountsRequest request)
        =>
        GetSelectableList<DtoAccount, TAccount>(x => x.Id, x => x.Name, order: o => o.Name);


    [HttpGet("Marketers")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetMarketers([FromQuery] GetClientsRequest request)
        =>
        GetSelectableList<DtoMarketer, TMarketer>(x => x.AccountId, x => x.Name, order: o => o.Name);


    [HttpGet("Clients")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetClients([FromQuery] ClientQueryRequest request)
        //=> GetSelectableList<DtoClient, TClient>(x => x.AccountId, x => x.Name);
        => TryMeny<DefaultLookup>(async _ =>
        {
            var clientService = _.GetService<DtoClient, TClient>();
            var where = clientService.Build();

            if (request.MarketerId is not null)
            {
                var markeServic = _.GetService<DtoMarketer, TMarketer>();
                var marketerInfo = await markeServic.GetOneAsAsync(s => new { s.Id }, w => w.AccountId == request.MarketerId);
                var marketerServic = _.GetService<DtoMarketerClient, TMarketerClient>();
                var seletedMarketerUsers = await marketerServic.GetAllAsAsync(s => new { s.ClientId }, w => w.MarketerId == marketerInfo.Id);
                var ids = seletedMarketerUsers.Items.Select(s => s.ClientId);
                where = where.And(w => ids.Contains(w.Id));
            }

            var all = await clientService.GetSelecListAsync(x => x.AccountId, s => s.Name, where, ord => ord.Name);
            var aaa = all.Items.Select(s => new DefaultLookup() { Display = s.Text, Value = s.Id });
            return _.Success(aaa);
        });


    [HttpGet("Unitsx")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetUnits([FromQuery] UnitQueryRequest request)
            =>
            GetSelectableList<DtoItemUnit, TItemUnit>(x => x.UnitId, x => x.UnitName, w => w.ItemId == Identifier.Parse(request.ItemId.ToString()), order: o => o.UnitName);


    [HttpGet("Employees")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetEmployees([FromQuery] GetEmployeesRequest request)
        =>
        GetSelectableList<DtoEmployee, TEmployee>(x => x.AccountId, x => x.Name, order: o => o.Name);


    [HttpGet("Expenses")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetExpenses([FromQuery] GetExpensesRequest request)
        =>
        GetSelectableList<DtoExpenseTypes, TExpenseTypes>(x => x.AccountId.Value, x => x.Name, w => w.AccountId != null, order: o => o.Name);


    [HttpGet("Suppliers")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetSuppliers([FromQuery] GetSuppliersRequest request)
        =>
        GetSelectableList<DtoSupplier, TSupplier>(x => x.AccountId, x => x.Name, order: o => o.Name);


    [HttpGet("Safes")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetSafes([FromQuery] GetSafesRequest request)
        => TryMeny<DefaultLookup>(async _ =>
        {
            var srorageServic = _.GetService<DtoSafe, TSafe>();
            var MarketerServic = _.GetService<DtoBankBranch, TBankBranch>();

            var allStorages = await srorageServic.GetAllAsAsync(s => new DefaultLookup { Display = s.Name, Value = s.AccountId });
            var allMarketers = await MarketerServic.GetAllAsAsync(s => new DefaultLookup { Display = s.Name, Value = s.AccountId });

            var all = allMarketers.Items
                .Concat(allStorages.Items)
                .OrderByDescending(w=> w.Display);

            return _.Success(PageList<DefaultLookup>.Create(all, 0, 0));
        });
    //=>
    //GetSelectableList<DtoSafe, TSafe>(x => x.AccountId, x => x.Name);


    [HttpGet("Items")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupItems([FromQuery] ItemQueryRequest request)
      =>
      GetSelectableList<DtoItem, TItem>(x => x.Name, order: x=> x.Name);


    [HttpGet("Units")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupUnits([FromQuery] UnitQueryRequest request)
        =>
            GetSelectableList<DtoItemUnit, TItemUnit>(
                x => x.UnitId,
                x => x.Unit!.Name,
                w => w.ItemId == Identifier.Parse(request.ItemId ?? Guid.Empty), order: x => x.UnitName);


    [HttpGet("AccountDeption")]
    public ValueTask<HttpResponseWrapper<TypeOf<decimal>>> GetAccountDeption(Identifier? AccountId)
           =>
           TryOne<TypeOf<decimal>>(async opt =>
           {
               if (AccountId is null) return opt.Problem("الطلب لا يحتوي على رقم الحساب");  // localize
               var entryService = opt.GetService<IEntryService>();
               var total = await entryService.GetTotalDebitInfo(accountId: AccountId.Value);
               return opt.Success(new TypeOf<decimal>(total));

           });


    [HttpGet("MarketerPercentage")]
    public ValueTask<HttpResponseWrapper<TypeOf<decimal>>> GetMarketerPercentage(Identifier? AccountId)
    => TryOne<TypeOf<decimal>>(async opt =>
    {
        if (AccountId is null) return opt.Problem("الطلب لا يحتوي على رقم الحساب");  // localize
        var marketerService = _service.MakeChildService<DtoMarketer, TMarketer>();
        var marketerInfo = await marketerService.GetOneAsAsync(s => new { s.Percentage }, w => w.AccountId == AccountId);
        if (marketerInfo is null) return opt.Problem("لم يتم العثور على الحساب"); // localize
        return opt.Success(new TypeOf<decimal>(marketerInfo.Percentage));
    });


    #endregion


    [HttpPost("AccountPaymentReceipt")]
    public ValueTask<HttpResponseWrapper<DtoStatment>> PostAccountPaymentReceipt(AccountFinancialRequest request)
    =>
    TryOne<DtoStatment>(async opt =>
    {

        var validationResult = await opt.Validate(request);
        if (!validationResult.IsValid) return opt.Problem(validationResult.AsErrors());

        DtoStatmentInfo? info = null;
        var service = (EntryService)_service;
        var they = request.Type == 1 ? request.FromAccount : request.ToAccount;
        //var entryRequest = request.CreateEntry();
        var entry = await service.AddSimpleEntry(request, async (tr, entr) =>
        {
            info = await service.GetFastStatementInfo(they);
        });
        if (entry is null) return opt.Problem("Cannot process payment");

        return opt.Success(new DtoStatment()
        {
            Debit = info.Debit,
            Credit = info.Credit,
            Balance = info.Balance,
            Notes = entry.Note,
            AccountName = entry.Details[0].AccountName,
            Number = entry.Number,
            DisplayNumber = entry.DisplayNumber,

        });
    });

    [HttpPost("ClientPaymentReceipt0")]
    public ValueTask<HttpResponseWrapper<DtoStatment>> PostClientPaymentReceipt(ClientFinancialRequest request)
    =>
        TryOne<DtoStatment>(async opt =>
        {
            var validationResult = await opt.Validate(request);
            if (!validationResult.IsValid) return opt.Problem(validationResult.AsErrors());

            using var tr = _service.BeginTransaction();

            DtoEntry? entry = null;
            DtoStatmentInfo? info = null;
            var makeDebit = request.TotalAmount >= 0;
            var they = request.Type == 1 ? request.FromAccount : request.ToAccount;
            var service = (EntryService)_service;

            if (request.TotalAmount > 0)
            {
                var configService = opt.GetService<DtoConfig, TConfig>();

                var generalGlobalShareAccountConfig = await configService.GetOneAsAsync(s => s.DiscountAccount);
                if (generalGlobalShareAccountConfig is null)
                    throw new Exception("generalGlobalShareAccount not found");

                var debtionAccount = Identifier.Parse(generalGlobalShareAccountConfig);
                var entr = request.CreateEntr2y(debtionAccount);
                entry = await service.AddFullEntry(entr, OprationTypeEnum.ClientFinancial, action: async (tr, entr) =>
                {
                    info = await service.GetFastStatementInfo(they);
                }, transaction: tr);
            }
            else
            {
                entry = await service.AddSimpleEntry(request, async (tr, entr) =>
                {
                    info = await service.GetFastStatementInfo(they);
                }, transaction: tr, autoCommit: false);
            }

            if (entry is null) return opt.Problem("Cannot process payment");

            //var entryService = opt.ServiceProvider.GetRequiredService<IEntryService>();
            //entryService.GetFastStatementInfo(req)

            _service.CommitTransaction();

            return opt.Success(new DtoStatment()
            {
                Debit = info.Debit,
                Credit = info.Credit,
                Balance = info.Balance,
                Notes = entry.Note,
                AccountName = entry.Details[0].AccountName,
                Number = entry.Number,
                DisplayNumber = entry.DisplayNumber,
            });
        });

    [HttpPost("EmployeePaymentReceipt0")]
    public ValueTask<HttpResponseWrapper<DtoStatment>> PostEmployeePaymentReceipt(EmployeeFinancialRequest request)
    =>
        TryOne<DtoStatment>(async opt =>
        {
            var validationResult = await opt.Validate(request);
            if (!validationResult.IsValid) return opt.Problem(validationResult.AsErrors());

            DtoStatmentInfo? info = null;
            var they = request.Type == 1 ? request.FromAccount : request.ToAccount;
            var service = (EntryService)_service;
            var entryRequest = new RSimpleRequest()
            {
                Amount = request.Amount,
                FromAccount = request.FromAccount,
                Date = request.Date,
                Note = request.Note,
                SerialNumber = request.SerialNumber,
                ToAccount = request.ToAccount,
                OprationTypeEnum = OprationTypeEnum.EmployeeFinancial,
            };

            var entry = await service.AddSimpleEntry(request, async (tr, entr) =>
            {
                info = await service.GetFastStatementInfo(they);
            });
            if (entry is null) return opt.Problem("Cannot process payment");
            return opt.Success(new DtoStatment()
            {
                Debit = info.Debit,
                Credit = info.Credit,
                Balance = info.Balance,
                Notes = entry.Note,
                AccountName = entry.Details[0].AccountName,
                Number = entry.Number,
                DisplayNumber = entry.DisplayNumber,
            });
        });

    [HttpPost("ExpensePaymentReceipt")]
    public ValueTask<HttpResponseWrapper<DtoStatment>> PostExpensePayment(ExpenseFinancialRequest request)
    =>
        TryOne<DtoStatment>(async opt =>
        {
            var validationResult = await opt.Validate(request);
            if (!validationResult.IsValid) return opt.Problem(validationResult.AsErrors());

            DtoStatmentInfo? info = null;
            var they = request.Type == 1 ? request.FromAccount : request.ToAccount;
            var service = (EntryService)_service;

            var entryRequest = new RSimpleRequest()
            {
                Amount = request.Amount,
                FromAccount = request.FromAccount,
                Date = request.Date,
                Note = request.Note,
                SerialNumber = request.SerialNumber,
                ToAccount = request.ToAccount,
                OprationTypeEnum = OprationTypeEnum.ExpenseFinancial,
            };
            var entry = await service.AddSimpleEntry(request, async (tr, entr) =>
            {
                info = await service.GetFastStatementInfo(they);
            });
            if (entry is null) return opt.Problem("Cannot process payment");
            return opt.Success(new DtoStatment()
            {
                Debit = info.Debit,
                Credit = info.Credit,
                Balance = info.Balance,
                Notes = entry.Note,
                AccountName = entry.Details[0].AccountName,
                Number = entry.Number,
                DisplayNumber = entry.DisplayNumber,
            });
        });

    [HttpPost("SupplierPaymentReceipt0")]
    public ValueTask<HttpResponseWrapper<DtoStatment>> PostSupplierPaymentReceipt(SupplierFinancialRequest request)
    =>
        TryOne<DtoStatment>(async opt =>
        {
            var validationResult = await opt.Validate(request);
            if (!validationResult.IsValid) return opt.Problem(validationResult.AsErrors());

            DtoStatmentInfo? info = null;
            var they = request.Type == 1 ? request.FromAccount : request.ToAccount;
            var service = (EntryService)_service;
            //var supplierService = opt.GetService<IServiceBase<DtoSupplier, TSupplier>>();
            //var supplier = await supplierService.GetOneAsAsync(s => new { s.Name }, w => w.Id == request.);

            var entryRequest = RSimpleRequest.Create(request, OprationTypeEnum.SupplierFinancial);
            var entry = await service.AddSimpleEntry(request, async (tr, entr) =>
            {
                info = await service.GetFastStatementInfo(they);
            });
            if (entry is null) return opt.Problem("Cannot process payment");

            return opt.Success(new DtoStatment()
            {
                Debit = info.Debit,
                Credit = info.Credit,
                Balance = info.Balance,
                Notes = entry.Note,
                AccountName = entry.Details[0].AccountName,
                Number = entry.Number,
                DisplayNumber = entry.DisplayNumber,
            });

            //var financialReportService = opt.ServiceProvider.GetRequiredService<ISaveReport>();
            //var financialReport = new FinancialReports("Representative001");
            //var fileName = "";
            //var result = await financialReport.Generate(fillDatasource: async () =>
            //{
            //    return new List<RTRepresentative>()
            //    {
            //        new RTRepresentative()
            //        {
            //            SafeName = safe.Name,
            //            Amount = request.Amount,
            //            ClientName = client.Name,
            //            Date = entryRequest.Date,
            //            MarketerName = marketer.Name,
            //            Notes = request.Note,
            //            ReceiptNumber = entryRequest.Number.ToString(),
            //        }
            //    };
            //},
            //fillParameters: async () =>
            //{
            //    //return new List<Microsoft.Reporting.NETCore.ReportParameter> { 
            //    //    //new Microsoft.Reporting.NETCore.ReportParameter("Date", request.Date),
            //    //};
            //    return null;
            //},
            //action: async (by) =>
            //{
            //    fileName = await financialReportService.SaveBytes(by);

            //});
            //result.FileName = fileName;
            //return opt.Success(result);
        });

    [HttpPost("DiscountPaymentReceipt")]
    public ValueTask<HttpResponseWrapper<DtoStatment>> PostMarketerPercent(MarketerPercentRequest request)
    =>
        TryOne<DtoStatment>(async opt =>
        {
            var validationResult = await opt.Validate(request);
            if (!validationResult.IsValid) return opt.Problem(validationResult.AsErrors());

            DtoStatmentInfo? info = null;
            var they = request.Type == 1 ? request.FromAccount : request.ToAccount;

            var service = (EntryService)_service;
            var entryRequest = new RSimpleRequest()
            {
                Amount = request.Amount,
                FromAccount = request.FromAccount,
                Date = request.Date,
                Note = request.Note,
                SerialNumber = request.SerialNumber,
                ToAccount = request.ToAccount,
                OprationTypeEnum = OprationTypeEnum.DiscountFinancial,
            };

            var entry = await service.AddSimpleEntry(request, async (tr, entr) =>
            {
                info = await service.GetFastStatementInfo(they);
            });
            if (entry is null) return opt.Problem("Cannot process payment");
            return opt.Success(new DtoStatment()
            {
                Debit = info.Debit,
                Credit = info.Credit,
                Balance = info.Balance,
                Notes = entry.Note,
                AccountName = entry.Details[0].AccountName,
                Number = entry.Number,
                DisplayNumber = entry.DisplayNumber,
            });
        });

    [HttpPost("MarketerPaymentReceipt")]
    public ValueTask<HttpResponseWrapper<DtoStatment>> PostMarketerPaymentReceipt(MarketerPercentRequest request)
    =>
        TryOne<DtoStatment>(async opt =>
        {
            var validationResult = await opt.Validate(request);
            if (!validationResult.IsValid) return opt.Problem(validationResult.AsErrors());

            DtoStatmentInfo? info = null;
            var they = request.Type == 1 ? request.FromAccount : request.ToAccount;
            var service = (EntryService)_service;
            var entryRequest = new RSimpleRequest()
            {
                Amount = request.Amount,
                FromAccount = request.FromAccount,
                Date = request.Date,
                Note = request.Note,
                SerialNumber = request.SerialNumber,
                ToAccount = request.ToAccount,
                OprationTypeEnum = OprationTypeEnum.MarketerFinancial,
            };

            var entry = await service.AddSimpleEntry(request, async (tr, entr) =>
            {
                info = await service.GetFastStatementInfo(they);
            });
            if (entry is null) return opt.Problem("Cannot process payment");
            return opt.Success(new DtoStatment()
            {
                Debit = info.Debit,
                Credit = info.Credit,
                Balance = info.Balance,
                Notes = entry.Note,
                AccountName = entry.Details[0].AccountName,
                Number = entry.Number,
                DisplayNumber = entry.DisplayNumber,
            });
        });

}
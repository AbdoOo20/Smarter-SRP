using MediatR;
using Fly.Core.Persistence.Ef.Models;
using Fly.Framework.Server.Persistence;
using Fly.Framework.Shared.Common.Http;
using Fly.Framework.Shared.Common.ValueTypes;
using Fly.Shared.Modules.Accounting.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Fly.Framework.Server.Common.Controllers.Base;
using Fly.Core.Modules.F.PurchaseAndSales.Models.DbModels;
using Fly.Core.Persistence.Ef.Models.Personnel;
using LinqKit;
using Fly.Core.Persistence.Ef.Models.Store;
using Fly.Shared.Modules.Personnel.Dtos;
using Fly.Shared.Modules.PurchaseAndSales.Dtos;
using Fly.Shared.Modules.Store.Dtos;
using Fly.Shared.Modules.Financial.Dtos;
using Fly.Shared.Modules.Financial.Requests;
using Microsoft.Extensions.DependencyInjection;
using Fly.Core.Modules.C.Accounting.Services;
using Fly.Shared.Modules.Reports.Features;
using Fly.Shared.Modules.Reports.Requests;
using Fly.Framework.Shared.Extentions;
using Fly.Framework.Shared.Common;
using Fly.Core.Modules.E.Store.Services.ModuleService;
using Fly.Core.Modules.F.PurchaseAndSales.Services.GlobalServices;
//using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using Fly.Shared.Modules.Store.Enums;
using Fly.Framework.Server.Common.Localization;
using Fly.Shared.Modules.Marketers.Dtos;
using Fly.Core.Modules.F.Marketers.Models.DbModels;
using Fly.Framework.Server.Persistence.Common;
using Fly.Framework.Shared.Common.Dtos;
using System.Linq.Expressions;
using Humanizer;
using Fly.Core.Modules.F.Marketers.Services.InternalServices;
using Fly.Framework.Shared.Common.Collections;
using Fly.Shared.Modules.Accounting.Enums;
using Fly.Core.Modules.E.Store.Models.DbModels;
using Fly.Core.Modules.F.Maintenance.MaintenanceInvoices.DbModels;
using Fly.Core.Modules.Factoring.FactoringInvoices.Models.DbModels;
using Microsoft.IdentityModel.Tokens;
using Fly.Shared.Modules.Global.Dtos;
using Fly.Core.Persistence.Ef.Models.Global;
using Fly.Framework.Shared.Common.Requests;
using Fly.Shared.Common;
using Fly.Shared.Modules.Factoring.Dtos;
using Fly.Shared.Modules.Factoring.Enums;
using Fly.Shared.Modules.Maintenance.Dtos;
using Fly.Shared.Modules.Maintenance.Enums;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Misc;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Sell;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Purchase;

namespace Fly.Core.Modules.H.Controller;

public partial class ReportsController
        :
        CustomControllerBase,
        IReportsFeatures
{
    public ReportsController(
        ILocalizationService localizationService, ILogger<ReportsController> logger,
        IMediator mediator,
        AutoMapper.IMapper mapper,
        MapsterMapper.IMapper mapster) : base(localizationService, logger, mediator, mapper, mapster)
    {
    }

    [NonAction]
    protected ValueTask<HttpResponseWrapperList<DefaultLookup>> GetSelectableList<TDtoo, TEnto>(Func<TDtoo, string> textSelector, Expression<Func<TDtoo, bool>>? where = null)
        where TDtoo : DtoBase
        where TEnto : TEntity, new()
    =>
        TryMeny<DefaultLookup>(async (opt) => opt.Success(await opt.MakeChildService<TDtoo, TEnto>().GetSelecListAsync(textSelector, where)));

    [NonAction]
    protected ValueTask<HttpResponseWrapperList<DefaultLookup>> GetSelectableList<TDtoo, TEnto>(Func<TDtoo, Identifier> idSelector, Func<TDtoo, string> textSelector, Expression<Func<TDtoo, bool>>? where = null)
        where TDtoo : DtoBase
        where TEnto : TEntity, new()
    =>
        TryMeny<DefaultLookup>(async (opt) => opt.Success(await opt.MakeChildService<TDtoo, TEnto>().GetDefaultLookupAsync(idSelector, textSelector, where)));

    private const string InvoiceMode = "9F22288E-DAE6-4337-BDB7-71E2DF0018C6";
    private const string FinacialMode = "9F22288E-DAE6-4337-BDB7-71E2DF0018C5";


    [HttpGet("LostTypes")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetSerialsMode()
    =>
        TryMeny<DefaultLookup>(async _ =>
        {
            await Task.CompletedTask;
            return _.Success(PageList<DefaultLookup>.Create(new[]
            {
                new DefaultLookup()
                {
                    Display = "فواتير",
                    Value = Guid.Parse(InvoiceMode),
                },
                new DefaultLookup()
                {
                    Display = "ايصالات",
                    Value = Guid.Parse(FinacialMode),
                },
            }, 0, 0));
        });


    [HttpGet("Representatives")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetRepresentative()
    =>
        GetSelectableList<DtoMarketer, TMarketer>(x => x.Id, x => x.Name, w => w.IsRepresentative);
    
    [HttpGet("accounts")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetAccounts([FromQuery] QueryRequestBase query)
    =>
        TryMeny<DefaultLookup>(async (opt) =>
        {
            var @try = int.TryParse(query.SearchText, out var itemIdPot);
            var itemId = !@try ? 0 : itemIdPot;
            
            switch (itemId)
            {
                case 0:
                    return opt.Success(await opt.MakeChildService<DtoAccount, TAccount>().GetDefaultLookupAsync(x => x.Id, x => x.Name));
                case 1:
                    return opt.Success(await opt.MakeChildService<DtoSupplier, TSupplier>().GetDefaultLookupAsync(x => x.AccountId, x => x.Name));
                case 2:
                    return opt.Success(await opt.MakeChildService<DtoEmployee, TEmployee>().GetDefaultLookupAsync(x => x.AccountId, x => x.Name));
                case 3:
                    return opt.Success(await opt.MakeChildService<DtoClient, TClient>().GetDefaultLookupAsync(x => x.AccountId, x => x.Name));
                case 4:
                    return opt.Success(await opt.MakeChildService<DtoBankBranch, TBankBranch>().GetDefaultLookupAsync(x => x.AccountId, x => x.Name));
                case 5:
                    return opt.Success(await opt.MakeChildService<DtoSafe, TSafe>().GetDefaultLookupAsync(x => x.AccountId, x => x.Name));
                case 6:
                    return opt.Success(await opt.MakeChildService<DtoMarketer, TMarketer>().GetDefaultLookupAsync(x => x.AccountId, x => x.Name));
                default:
                    return opt.Problem("Unknown select"); // TODO: localization
            }
        });

    [HttpGet("items")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetItems(int itemId)
    =>
        TryMeny<DefaultLookup>(async (opt) =>
        {
            //itemId = (itemId == -1) ? new Random().Next(0, 4) : itemId;
            return opt.Success(await opt.MakeChildService<DtoItem, TItem>().GetDefaultLookupAsync(x => x.Id, x => x.Name + " - " + x.Barcode));
        });

    private string VIEW_MODE__SECURE = ("7B72B11D-AC1A-4C1F-85AF-05DA703FC70D");
    private string VIEW_MODE__INSECURE = ("7B72B11D-AC1A-4C1F-85AF-05DA703FC70D");

    [HttpGet("Views")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetViews()
    =>
        TryMeny<DefaultLookup>(async (opt) =>
        {
            return opt.Success([
                new DefaultLookup(){
                    Display = "إظهار الكل",
                    Value = Identifier.Parse(VIEW_MODE__SECURE),
                },
                new DefaultLookup(){
                    Display = "إظهار جزئي",
                    Value = Identifier.Parse(VIEW_MODE__INSECURE),
                },
            ]);
        });

    [HttpGet("MaintenanceStatuses")]
    public ValueTask<HttpResponseWrapperList<StaticLookup>> GetMaintenanceStatuses()
        =>
            TryMeny<StaticLookup>(async (opt) =>
            {
                var ret = new List<StaticLookup>();
                
                foreach (MaintenanceStatus val in Enum.GetValues(typeof(MaintenanceStatus)))
                {
                    var display = MaintenanceStatusToString(val);

                    ret.Add(new StaticLookup
                    {
                        Display = display,
                        Value = ((int)val).ToString()    
                    });
                }

                return opt.Success(ret);
            });

    [HttpGet("MaintenanceItemsStatuses")]
    public ValueTask<HttpResponseWrapperList<StaticLookup>> GetMaintenanceItemsStatuses()
        =>
            TryMeny<StaticLookup>(async (opt) =>
            {
                var ret = new List<StaticLookup>();
                
                foreach (MaintenanceItemsStatus val in Enum.GetValues(typeof(MaintenanceItemsStatus)))
                {
                    var display = MaintenanceItemsStatusToString(val);

                    ret.Add(new StaticLookup
                    {
                        Display = display,
                        Value = ((int)val).ToString()    
                    });
                }

                return opt.Success(ret);
            });
    
    [HttpGet("FactoringItemsStatuses")]
    public ValueTask<HttpResponseWrapperList<StaticLookup>> GetFactoringItemsStatuses()
        =>
            TryMeny<StaticLookup>(async (opt) =>
            {
                var ret = new List<StaticLookup>();
                
                foreach (FactoringItemsStatus val in Enum.GetValues(typeof(FactoringItemsStatus)))
                {
                    var display = FactoringItemsStatusToString(val);

                    ret.Add(new StaticLookup
                    {
                        Display = display,
                        Value = ((int)val).ToString()    
                    });
                }

                return opt.Success(ret);
            });
    
    [HttpGet("FactoringStatuses")]
    public ValueTask<HttpResponseWrapperList<StaticLookup>> GetFactoringStatuses()
        =>
            TryMeny<StaticLookup>(async (opt) =>
            {
                var ret = new List<StaticLookup>();
                
                foreach (FactoringStatus val in Enum.GetValues(typeof(FactoringStatus)))
                {
                    var display = FactoringStatusToString(val);

                    ret.Add(new StaticLookup
                    {
                        Display = display,
                        Value = ((int)val).ToString()    
                    });
                }

                return opt.Success(ret);
            });
    
    private string FactoringStatusToString(FactoringStatus status)
    {
        var display = status switch
        {
            FactoringStatus.ReceivedUnpaid => "غير مدفعوع",
            FactoringStatus.ReceivedFeeInAdvance => "تم استلام عربون",
            FactoringStatus.FullyPaid => "مدفوع بالكامل",
            FactoringStatus.Cancelled => "ملغي",
            FactoringStatus.OnHold => "معلق",
            FactoringStatus.Rejected => "مرفوض",
            _ => status.ToString()
        };

        return display;
    }
    
    private string FactoringItemsStatusToString(FactoringItemsStatus status)
    {
        var display = status switch
        {
            FactoringItemsStatus.AwaitingParts => "بأنتظار الأصناف",
            FactoringItemsStatus.ReceivedInvoice => "تم أستلام الفاتورة",
            FactoringItemsStatus.Factoring => "تحت التصنيع",
            FactoringItemsStatus.OnHold => "تم الأيقاف مؤفتاً",
            FactoringItemsStatus.PartsOnTheWay => "الأصناف في الطريق",
            FactoringItemsStatus.PartsReceived => "تم أستلام الأصناف",
            FactoringItemsStatus.PartsUnavailable => "الأصناف غير متوفرة حالياً",
            FactoringItemsStatus.Finished => "مكتملة",
            _ => "MISSING"
        };

        return display;
    }
    
    private string MaintenanceItemsStatusToString(MaintenanceItemsStatus status)
    {
        var display = status switch
        {
            MaintenanceItemsStatus.AwaitingParts => "بأنتظار الأصناف",
            MaintenanceItemsStatus.ReceivedInvoice => "تم أستلام الفاتورة",
            MaintenanceItemsStatus.Maintaining => "تحت الصيانة",
            MaintenanceItemsStatus.OnHold => "تم الأيقاف مؤفتاً",
            MaintenanceItemsStatus.PartsOnTheWay => "الأصناف في الطريق",
            MaintenanceItemsStatus.PartsReceived => "تم أستلام الأصناف",
            MaintenanceItemsStatus.PartsUnavailable => "الأصناف غير متوفرة حالياً",
            MaintenanceItemsStatus.Finished => "مكتملة",
            _ => "MISSING"
        };

        return display;
    }
    
    private string MaintenanceStatusToString(MaintenanceStatus status)
    {
        var display = status switch
        {
            MaintenanceStatus.ReceivedUnpaid => "غير مدفوع",
            MaintenanceStatus.ReceivedFeeInAdvance => "تم استلام عربون",
            MaintenanceStatus.FullyPaid => "مدفوع بالكامل",
            MaintenanceStatus.Cancelled => "ملغي",
            MaintenanceStatus.OnHold => "معلق",
            MaintenanceStatus.Rejected => "مرفوض",
            _ => status.ToString()
        };

        return display;
    }
    
    [HttpGet("Units")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetUnits()
    =>
        TryMeny<DefaultLookup>(async (opt) =>
        {
            return opt.Success(await opt.MakeChildService<DtoUnit, TUnit>().GetDefaultLookupAsync(x => x.Id, x => x.Name));
        });

    [HttpGet("Categories")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetCategories()
    =>
        TryMeny<DefaultLookup>(async (opt) =>
        {
            return opt.Success(await opt.MakeChildService<DtoCategory, TCategory>().GetDefaultLookupAsync(x => x.Id, x => x.Name));
        });

    [HttpGet("Clients")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetClients()
        =>
            TryMeny<DefaultLookup>(async (opt) =>
            {
                return opt.Success(await opt.MakeChildService<DtoClient, TClient>().GetDefaultLookupAsync(x => x.Id, x => x.Name));
            });
    
    [HttpGet("StoragesAndMarketers")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetStoragesAndMarketers()
    =>
        TryMeny<DefaultLookup>(async (opt) =>
        {
            var storaages = await opt.MakeChildService<DtoStore, TStore>().GetDefaultLookupAsync(x => x.Id, x => x.Name);
            var marketers = await opt.MakeChildService<DtoMarketer, TMarketer>().GetDefaultLookupAsync(x => x.Id, x => x.Name);

            return opt.Success(new PageList<DefaultLookup>()
            {
                Items = storaages.Items.Concat(marketers.Items),
            });
        });

    //
    //

    //  AccountMovements
    [HttpPost("AccountMovements")]
    public ValueTask<HttpResponseWrapper<DynamicApiReports>> PostAccountMovementsStetements(StatementQueryRequest request)
    =>
        TryOne<DynamicApiReports>(async opt =>
        {
            var dynamicApiRow = new DynamicApiReports();
            var entryService = opt.ServiceProvider.GetRequiredService<IEntryService>();
            var accountService = opt.ServiceProvider.GetRequiredService<IServiceBase<DtoAccount, TAccount>>();

            decimal balaneTotal = 0, balane = 0;
            if (request.AccountId is null)
            {
                dynamicApiRow.AppendRow([
                    $"{SharedConstants.InternalTableColumn}_InvoiceInfo",
                    "رقم القيد",
                    "رقم العملية",
                    "الحساب",
                    "التاريخ",
                    "الحركة",
                    "مدين",
                    "دائن",
                    "الرصيد",
                    "ملاحظات"
                    ]);
            }
            else
            {
                balane = await entryService.GetBalance(request.AccountId.Value, new DateTime(1994, 07, 28), request.StartDate.AddDays(-1).AtMidnight());
                balaneTotal = await entryService.GetBalance(request.AccountId.Value);
                var accointInfo = await accountService.GetOneAsAsync(s => new { s.Name }, w => w.Id == request.AccountId);
                dynamicApiRow.AddUpper("الحساب", accointInfo!.Name);
                dynamicApiRow.AddUpper("قيمة قبل التاريخ", (balane).ToDottedCommaedNumber());
                dynamicApiRow.AddUpper("الرصيد", (balaneTotal).ToDottedCommaedNumber());
                dynamicApiRow.AppendRow([
                    $"{SharedConstants.InternalTableColumn}_InvoiceInfo",
                    "رقم القيد",
                    "رقم العملية",
                    "التاريخ",
                    "الحركة",
                    "مدين",
                    "دائن",
                    "الرصيد",
                    "ملاحظات"
                    ]);
            }
            //
            //
            //  3) Fetch the report raw data
            //
            request.CollectTheQuantitiesBeforeTime = true;
            var all = (await entryService.GetDetailsStatements(request.StartDate,
                request.EndDate,
                request.AccountId,
                request.CollectTheQuantitiesBeforeTime,
                request.Page,
                request.Liimit,
                statementAccountType: request.AccountType))
                .Items.ToList();
            //
            //
            //  4) Prepare rows data
            //
            //all.RemoveAt(0);
            if (all.Count == 0)
            {

            }
            else
            {
                //var mayBeOld = all.ElementAt(0);
                //if (mayBeOld is not null && mayBeOld.CollecttTheQuntitiesBeforTime == true)
                //{
                //    var am = "";
                //    if (mayBeOld.Credit == 0 & mayBeOld.Debit == 0)
                //    {
                //        am = "صفر";
                //        DynamicApiRow.AddUpper(mayBeOld.AccountName, am);
                //    }
                //    else if (mayBeOld.Credit == 0)
                //    {
                //        am = " مدين بـ " + " " + mayBeOld.Debit.ToString();
                //        DynamicApiRow.AddUpper(mayBeOld.AccountName, am);
                //    }
                //    else if (mayBeOld.Debit == 0)
                //    {
                //        am = " دائن بـ " + " " + mayBeOld.Credit.ToString();
                //        DynamicApiRow.AddUpper(mayBeOld.AccountName, am);
                //    }
                //    all.RemoveAt(0);
                //}
                
                var datav = CalculateBalance(all);
                
                var d = datav.Select(s =>
                {
                    ICollection<string> row =
                        (request.AccountId is null)
                        ? [
                            $"{s.OprationType.ToString()}",
                            s.Number.ToString(),
                            s.DisplayNumber.ToString(),
                            s.AccountName.ToString(),
                            s.EntryDateString,
                            s.OprationTypeString,
                            s.Debit.ToDottedCommaComputedNumber(),
                            (s.Credit * -1).ToDottedCommaComputedNumber(),
                            s.Balance.ToDottedCommaComputedNumber(),
                            s.Notes,
                            ]
                        : [
                            $"{s.OprationType.ToString()}",
                            s.Number.ToString(),
                            s.DisplayNumber.ToString(),
                            s.EntryDateString,
                            s.OprationTypeString,
                            s.Debit.ToDottedCommaComputedNumber(),
                            (s.Credit * -1).ToDottedCommaComputedNumber(),
                            s.Balance.ToDottedCommaComputedNumber(),
                            s.Notes,
                        ];
                    return new DynamicApiRow()
                    {
                        Row = row
                    };
                });

                dynamicApiRow.AppendRows(d);
                if (request.AccountId is not null)
                {
                    //DynamicApiRow.AddUpper("قيمة قبل التاريخ", (balane).ToDottedCommaedNumber());
                    //DynamicApiRow.AddUpper("الرصيد", (balaneTotal).ToDottedCommaedNumber());
                    dynamicApiRow.AppendRow([
                            "",
                            "",
                            "",
                            "",
                            "",
                            datav.Sum(w=> w.Debit).ToDottedCommaComputedNumber(),
                            datav.Sum(w=> w.Credit).ToDottedCommaComputedNumber(),
                            datav.Select(s => s.Balance).Last().ToDottedCommaComputedNumber(),
                    ]);
                }
            }
            //
            //
            //  5) return response
            //
            return opt.Success(dynamicApiRow);
        });


    //  TheClientDebtion, TheMarketerDebtion
    [HttpPost("TheClientDebtion")]
    public ValueTask<HttpResponseWrapper<DynamicApiReports>> PostTheDebtionReport(StatementQueryRequest request)
    =>
        TryOne<DynamicApiReports>(async opt =>
        {
            //
            //
            //  1) Prepare needed servcies
            //
            var DynamicApiRow = new DynamicApiReports();
            var entryService = opt.ServiceProvider.GetRequiredService<IEntryService>();
            var marketerService = opt.ServiceProvider.GetRequiredService<IServiceBase<DtoMarketer, TMarketer>>();
            var marketerClientsService = opt.ServiceProvider.GetRequiredService<IServiceBase<DtoMarketerClient, TMarketerClient>>();
            var clientService = opt.ServiceProvider.GetRequiredService<IServiceBase<DtoClient, TClient>>();
            var accountService = opt.ServiceProvider.GetRequiredService<IServiceBase<DtoAccount, TAccount>>();
            var salesService = opt.ServiceProvider.GetRequiredService<IServiceBase<DtoSalesInvoice, TSalesInvoice>>();
            var entryDetailsService = opt.ServiceProvider.GetRequiredService<IServiceBase<DtoEntryDetail, TEntryDetail>>();
            ICollection<DynamicApiRow> rows = [];
            //
            //
            //  2) Prepare columns headers
            //
            List<Identifier> clientsAccointsIDs = [];

            var selectedAccountName = "";
            if (request.AccountId is null)
            {
                var clientsAccoints = await clientService.GetAllAsync();

                clientsAccointsIDs = clientsAccoints.Items.Select(c => c.AccountId).ToList();

                DynamicApiRow.AppendRow(["العميل",
                    "المديونية", "آخر دفعة", "تاريخ آخر دفعة", "حركة الدفعة", "رقم حركة الدفع",
                    "أخر فاتورة", "تاريخ أخر فاتورة", "حركة الفاتورة", "رقم حركة الفاتورة"]);
            }
            else
            {
                clientsAccointsIDs = [request.AccountId.Value];
                var accointInfo = await accountService.GetOneAsAsync(s => new { s.Name }, w => w.Id == request.AccountId);
                selectedAccountName = accointInfo!.Name;
                DynamicApiRow.AddUpper("العميل", accointInfo!.Name);
                DynamicApiRow.AppendRow([
                    "المديونية", "آخر دفعة", "تاريخ آخر دفعة", "حركة الدفعة", "رقم حركة الدفع",
                    "أخر فاتورة", "تاريخ أخر فاتورة", "حركة الفاتورة", "رقم حركة الفاتورة"]);
            }
            var selectedMarketer = request.DynamicCombos.FirstOrDefault(w => w.QueryName == "RepresentativeId");
            var showDetail = selectedMarketer is not null && selectedMarketer.SelectedCombo is not null
                && selectedMarketer.SelectedCombo?.Value is not null && !selectedMarketer.SelectedCombo.Value.IsNullOrEmpty();
            if (showDetail)
            {
                var marketerId = Identifier.Parse(selectedMarketer.SelectedCombo.Value);
                var marketerInfo = await marketerService.GetOneAsAsync(s => new { s.Name }, w => w.Id == marketerId);
                var clients = await marketerClientsService.GetAllAsAsync(s => s.ClientAccountId, w => w.MarketerId == marketerId);
                
                clientsAccointsIDs = clients.Items.ToList();
                DynamicApiRow.AddUpper("المندوب", marketerInfo.Name);
            }
            //
            //
            //  3) Fetch the report raw data
            //
            var totalDebit = 0m;
            var totalCredit = 0m;
            var totalBalance = 0m;

            var list = new List<TheClientDebtionList>();
            foreach (var item in clientsAccointsIDs)
            {
                var totlBalance = await entryService.GetBalance(item, request.StartDate, request.EndDate);
                var clientDeptionData = await entryService.GetLastDebitInfo(item, request.StartDate, request.EndDate);
                // Client wants an unedited date of sale invoices:
                var clientByAccountId = await clientService.GetAllAsync(c => c.AccountId == item);
                var lastSales = await salesService.GetAllAsync(s => s.ClientId == (clientByAccountId.Items.FirstOrDefault() ?? new()).Id && s.Date >= request.StartDate && s.Date <= request.EndDate);
                var lastDate = lastSales.Items.OrderByDescending(d => d.Date).FirstOrDefault()?.Date;
                
                var clientCreditionData = await entryService.GetLastCreditInfo(item);
                var accointInfo = await accountService.GetOneAsAsync(s => new { s.Name }, w => w.Id == item);

                totalDebit += clientDeptionData.Ammount;
                totalCredit += clientCreditionData.Ammount;
                totalBalance += totlBalance;

                TheClientDebtionList theClientDebtionList = new()
                {
                    AccountName = accointInfo.Name ?? "",
                    totlBalance = totlBalance,
                    clientCreditionDataAmmount = clientCreditionData.Ammount,
                    clientCreditionDataDate = clientCreditionData.Date,
                    clientCreditionDataOprationTypeString = clientCreditionData.OprationTypeString,
                    clientCreditionDataDisplayNumber = clientCreditionData.DisplayNumber,
                    clienttDeptionDataAmmount = clientDeptionData.Ammount,
                    clienttDeptionDataDate = lastDate,
                    clienttDeptionDataOprationTypeString = clientDeptionData.OprationTypeString,
                    clienttDeptionDataDisplayNumber = clientDeptionData.DisplayNumber,
                };
                list.Add(theClientDebtionList);
            }
            var order = list.OrderByDescending(W => W.totlBalance);
            foreach (var item in order)
            {
                if (request.AccountId.HasValue)
                {
                    rows.Add(new DynamicApiRow(
                        [
                            item.totlBalance.ToDottedCommaedNumber(),
                            item.clientCreditionDataAmmount.ToDottedCommaedNumber(),
                            item.clientCreditionDataDate.ToDottedDate(),
                            item.clientCreditionDataOprationTypeString,
                            item.clientCreditionDataDisplayNumber.ToString(),

                            item.clienttDeptionDataAmmount.ToDottedCommaedNumber(),
                            item.clienttDeptionDataDate.ToDottedDate(),
                            item.clienttDeptionDataOprationTypeString,
                            item.clienttDeptionDataDisplayNumber.ToString(),
                        ]));
                }
                else
                {
                    rows.Add(new DynamicApiRow(
                        [
                            item.AccountName ?? "",
                            item.totlBalance.ToDottedCommaedNumber(),
                            item.clientCreditionDataAmmount.ToDottedCommaedNumber(),
                            item.clientCreditionDataDate.ToDottedDate(),
                            item.clientCreditionDataOprationTypeString,
                            item.clientCreditionDataDisplayNumber.ToString(),

                            item.clienttDeptionDataAmmount.ToDottedCommaedNumber(),
                            item.clienttDeptionDataDate.ToDottedDate(),
                            item.clienttDeptionDataOprationTypeString,
                            item.clienttDeptionDataDisplayNumber.ToString(),
                        ]));
                }
            }
            if (showDetail)
                DynamicApiRow.AddUpper("الرصيد", (totalDebit - totalCredit).ToDottedCommaedNumber());
            DynamicApiRow.AddUpper("اجمالي المديونية ضمن التاريخ", totalBalance.ToDottedCommaedNumber());
            //
            //
            //  4) Prepare rows data
            //
            //
            DynamicApiRow.AppendRows(rows);
            //
            //
            //  5) return response
            //
            return opt.Success(DynamicApiRow);
        });


    //  TheClientDebtion, TheMarketerDebtion
    [HttpPost("TheCSupplierDebtion")]
    public ValueTask<HttpResponseWrapper<DynamicApiReports>> PostTheCSupplierDebtion(StatementQueryRequest request)
    =>
        TryOne<DynamicApiReports>(async opt =>
        {
            //
            //
            //  1) Prepare needed servcies
            //
            var DynamicApiRow = new DynamicApiReports();
            var entryService = opt.ServiceProvider.GetRequiredService<IEntryService>();
            var clientService = opt.ServiceProvider.GetRequiredService<IServiceBase<DtoSupplier, TSupplier>>();
            var accountService = opt.ServiceProvider.GetRequiredService<IServiceBase<DtoAccount, TAccount>>();
            var entryDetailsService = opt.ServiceProvider.GetRequiredService<IServiceBase<DtoEntryDetail, TEntryDetail>>();
            ICollection<DynamicApiRow> rows = [];
            //
            //
            //  2) Prepare columns headers
            //
            List<Identifier> clientsAccointsIDs = [];
            var selectedAccountName = "";
            if (request.AccountId is null)
            {
                var clientsAccoints = await clientService.GetAllAsAsync(s => s.AccountId);
                clientsAccointsIDs = clientsAccoints.Items.ToList();
                DynamicApiRow.AppendRow(["المورد",
                    "المديونية", "آخر دفعة", "تاريخ آخر دفعة", "حركة الدفعة", "رقم حركة الدفع",
                    "أخر دين", "تاريخ أخر دين", "حركة الدين", "رقم حركة الدين"]);
            }
            else
            {
                clientsAccointsIDs = [request.AccountId.Value];
                var accointInfo = await accountService.GetOneAsAsync(s => new { s.Name }, w => w.Id == request.AccountId);
                selectedAccountName = accointInfo!.Name;
                DynamicApiRow.AddUpper("المورد", accointInfo!.Name);
                DynamicApiRow.AppendRow(["المديونية",
                     "آخر دفعة", "تاريخ آخر دفعة", "حركة الدفعة", "رقم حركة الدفع",
                    "أخر دين", "تاريخ أخر دين", "حركة الدين", "رقم حركة الدين"]);
            }
            //
            //
            //  3) Fetch the report raw data
            //
            foreach (var item in clientsAccointsIDs)
            {
                var totlBalance = await entryService.GetBalance(item);
                var clientDeptionData = await entryService.GetLastDebitInfo(item);
                var clientCreditionData = await entryService.GetLastCreditInfo(item);
                var accointInfo = await accountService.GetOneAsAsync(s => new { s.Name }, w => w.Id == item);


                if (request.AccountId.HasValue)
                {
                    rows.Add(new DynamicApiRow(
                        [
                            totlBalance.ToDottedCommaedNumber(),
                            clientDeptionData.Ammount.ToDottedCommaedNumber(),
                            clientDeptionData.Date.ToDottedDate(),
                            clientDeptionData.OprationTypeString,
                            clientDeptionData.DisplayNumber.ToString(),


                            clientCreditionData.Ammount.ToDottedCommaedNumber(),
                            clientCreditionData.Date.ToDottedDate(),
                            clientCreditionData.OprationTypeString,
                            clientCreditionData.DisplayNumber.ToString(),

                        ]));
                }
                else
                {
                    rows.Add(new DynamicApiRow(
                        [
                            accointInfo.Name ?? "",
                            totlBalance.ToDottedCommaedNumber(),
                            clientDeptionData.Ammount.ToDottedCommaedNumber(),
                            clientDeptionData.Date.ToDottedDate(),
                            clientDeptionData.OprationTypeString,
                            clientDeptionData.DisplayNumber.ToString(),


                            clientCreditionData.Ammount.ToDottedCommaedNumber(),
                            clientCreditionData.Date.ToDottedDate(),
                            clientCreditionData.OprationTypeString,
                            clientCreditionData.DisplayNumber.ToString(),

                        ]));
                }
            }
            //
            //
            //  4) Prepare rows data
            //
            //
            DynamicApiRow.AppendRows(rows);
            //
            //
            //  5) return response
            //
            return opt.Success(DynamicApiRow);
        });


    [HttpPost("MissedSerial")]  //  ============================== MissedSerial
    public ValueTask<HttpResponseWrapper<DynamicApiReports>> PostMissedSerial(StatementQueryRequest request)
     => TryOne<DynamicApiReports>(async opt =>
        {
            //
            //
            //  1) Prepare needed servcies
            //
            long? startpoint = null;
            long endPoint = 50;
            var startPointField = request.DynamicFields.FirstOrDefault(w => w.QueryName == "StartPoint");
            var endPointField = request.DynamicFields.FirstOrDefault(w => w.QueryName == "EndPoint");
            var typeField = request.DynamicCombos.FirstOrDefault(w => w.QueryName == "LostTypeId");

            if (startPointField?.Value is not null)
            {
                startpoint = long.Parse(startPointField.Value.ToString());
            }
            if (endPointField?.Value is not null)
            {
                endPoint = long.Parse(endPointField.Value.ToString());
            }
            if (endPoint >= 100) endPoint = 100;
            if (startpoint is null) endPoint = 0;
            var DynamicApiRow = new DynamicApiReports();
            var entryService = opt.ServiceProvider.GetRequiredService<IEntryService>();
            var saleService = opt.GetService<DtoSalesInvoice, TSalesInvoice>();
            ICollection<DynamicApiRow> rows = [];
            //
            //
            //  2) Prepare columns headers
            //
            List<Identifier> clientsAccointsIDs = [];
            DynamicApiRow.AppendRow([
                "الرقم الضائع",
            ]);
            //
            //
            //  3) Fetch the report raw data
            //
            PageList<long>? all = null;
            var selectedMode = typeField?.SelectedCombo?.Value ?? InvoiceMode;
            switch (selectedMode.ToUpper())
            {
                case InvoiceMode:
                    all = await saleService.GetAllAsAsync(w => w.SerialNumber);
                    break;
                case FinacialMode:
                    all = await entryService.GetAllAsAsync(w => w.SerialNumber);
                    break;
            }

            if (all is not null)
            {
                IEnumerable<long> FindLostNumbers(IEnumerable<long> numbers)
                {
                    long minNumber = startPointField?.Value is not null ? int.Parse(startPointField.Value!.ToString()!) : 0;
                    long maxNumber = minNumber + endPoint;
                    var realNumbers = Enumerable.Range(Convert.ToInt32(minNumber), Convert.ToInt32((maxNumber - minNumber)))
                        .Select(s => Convert.ToInt64(s));

                    var lostNumber = realNumbers.Except(numbers);
                    return lostNumber;
                }

                var lost = FindLostNumbers(all.Items.ToArray());
                foreach (var item in lost)
                {
                    rows.Add(new DynamicApiRow(
                        [
                            item.ToString(),
                        ]
                    ));
                }
            }

            //
            //
            //  4) Prepare rows data
            //
            //
            DynamicApiRow.AppendRows(rows);
            //
            //
            //  5) return response
            //
            return opt.Success(DynamicApiRow);
        });



    [HttpPost("SalesProfit")]
    public ValueTask<HttpResponseWrapper<DynamicApiReports>> PostSaleProfits(StatementQueryRequest request)
     => TryOne<DynamicApiReports>(async opt =>
        {
            //
            //
            //  1) Prepare needed servcies
            //
            var DynamicApiRow = new DynamicApiReports();
            var entryService = opt.ServiceProvider.GetRequiredService<IEntryService>();
            var clientService = opt.ServiceProvider.GetRequiredService<IServiceBase<DtoClient, TClient>>();
            var accountService = opt.ServiceProvider.GetRequiredService<IServiceBase<DtoAccount, TAccount>>();

            var salesService = opt.ServiceProvider.GetRequiredService<IServiceBase<DtoSalesInvoice, TSalesInvoice>>();
            var allSales = salesService.GetAllAsync(w => w.Date >= request.StartDate && w.Date <= request.EndDate);
            ICollection<DynamicApiRow> rows = [];
            //
            //
            //  2) Prepare columns headers
            //
            List<Identifier> clientsAccointsIDs = [];
            var selectedAccountName = "";
            if (request.AccountId is null)
            {
                var clientsAccoints = await clientService.GetAllAsAsync(s => s.AccountId);
                clientsAccointsIDs = clientsAccoints.Items.ToList();
                DynamicApiRow.AppendRow(["العميل", "اجمالي الدين", "اجمالي المقبوض", "نسبة التحصيل", "مدة الخلاص"]);
            }
            else
            {
                clientsAccointsIDs = [request.AccountId.Value];
                var accointInfo = await accountService.GetOneAsAsync(s => new { s.Name }, w => w.Id == request.AccountId);
                selectedAccountName = accointInfo!.Name;
                DynamicApiRow.AddUpper("العميل", accointInfo!.Name);
                DynamicApiRow.AppendRow(["اجمالي الدين", "اجمالي المقبوض", "نسبة التحصيل", "مدة الخلاص"]);
            }
            //
            //
            //  3) Fetch the report raw data
            //
            foreach (var item in clientsAccointsIDs)
            {
                var totlBalance = await entryService.GetBalance(item, request.StartDate, request.EndDate);
                var clientDeptionData = await entryService.GetTotalDebitInfo(item, request.StartDate, request.EndDate);
                if (clientDeptionData == 0) continue;
                var clientCreditionData = await entryService.GetTotalCreditInfo(item, request.StartDate, request.EndDate);
                var accointInfo = await accountService.GetOneAsAsync(s => new { s.Name }, w => w.Id == item);

                var percent = (clientCreditionData * 100) / clientDeptionData;

                if (request.AccountId.HasValue)
                {
                    rows.Add(new DynamicApiRow(
                        [
                            clientDeptionData.ToDottedCommaedNumber(),
                            clientCreditionData.ToDottedCommaedNumber(),
                            percent.ToDottedCommaedNumber()+"%",
                            "",
                        ]));
                }
                else
                {
                    rows.Add(new DynamicApiRow(
                        [
                            accointInfo.Name ?? "",
                            clientDeptionData.ToDottedCommaedNumber(),
                            clientCreditionData.ToDottedCommaedNumber(),
                            percent.ToDottedCommaedNumber()+"%",
            "",
                        ]));
                }
            }
            //
            //
            //  4) Prepare rows data
            //
            //
            DynamicApiRow.AppendRows(rows);
            //
            //
            //  5) return response
            //
            return opt.Success(DynamicApiRow);
        });


    [HttpPost("DiscountItemsBased")] // ================ DiscountItemsBased
    public ValueTask<HttpResponseWrapper<DynamicApiReports>> PostDiscountItemsBased(StatementQueryRequest request)
    =>
        TryOne<DynamicApiReports>(async opt =>
        {
            //
            //
            //  1) Prepare needed servcies
            //
            var DynamicApiRow = new DynamicApiReports();
            var discountitedItemServiceService = opt.ServiceProvider.GetRequiredService<IDiscountitedItemService>();
            ICollection<DynamicApiRow> rows = [];
            //
            //
            //  2) Prepare columns headers
            //
            DynamicApiRow.AppendRow([
                "التاريخ",
                "العميل",
                "الصنف",
                "الكمية",
                "الوحدة",
                "القيمة",
                //"الربح",
                ]);
            var w = discountitedItemServiceService.Build();
            w = w.And(w => w.Date >= request.StartDate && request.EndDate >= w.Date);
            if (request.AccountId is not null)
            {
                w = w.And(w => w.ItemId == request.AccountId.Value);
            }
            var all = await discountitedItemServiceService.GetAllAsync(w);
            if (all.Items.Any())
            {
                foreach (var item in all.Items)
                {
                    rows.Add(new DynamicApiRow(
                    [
                        item.Date.ToDottedDate(),
                        item.ClientName,
                        item.ItemName,
                        item.Quantity.ToDottedCommaedNumber(),
                        item.UnitName,
                        item.Ammount.ToDottedCommaedNumber(),
                        //""
                    ]));
                }
            }
            else
            {

            }
            //
            //
            //  4) Prepare rows data
            //
            //
            DynamicApiRow.AppendRows(rows);
            //
            //
            //  5) return response
            //
            return opt.Success(DynamicApiRow);
        });


    [HttpPost("MaxDiscountItemsBased")] // ================ DiscountItemsBased
    public ValueTask<HttpResponseWrapper<DynamicApiReports>> PostMaxDiscountItemsBased(StatementQueryRequest request)
    =>
        TryOne<DynamicApiReports>(async opt =>
        {
            //
            //
            //  1) Prepare needed servcies
            //
            var DynamicApiRow = new DynamicApiReports();
            var discountitedItemServiceService = opt.ServiceProvider.GetRequiredService<IDiscountitedItemService>();
            ICollection<DynamicApiRow> rows = [];
            //
            //
            //  2) Prepare columns headers
            //
            DynamicApiRow.AppendRow([
                "الصنف",
                "الكمية",
                //"0الكمية",
                //"القيمة",
                //"الربح",
                ]);
            var all = await discountitedItemServiceService.GetAllAsync(w => w.Date >= request.StartDate && request.EndDate >= w.Date);
            if (all.Items.Any())
            {
                var byitem = all.Items.GroupBy(w => w.ItemId).ToList();
                var detailByItem = byitem.Select(w =>
                new
                {
                    ItemId = w.Key,
                    w.First().ItemName,
                    Quntity = w.Sum(w => w.TotalQuantity),
                    w.LastOrDefault()?.Date
                })
                .OrderBy(w => w.Date)
                .ToArray();
                foreach (var item in detailByItem)
                {
                    rows.Add(new DynamicApiRow(
                    [
                        item.ItemName,
                        item.Quntity.ToDottedCommaedNumber(),
                        //item.Date.ToDottedDate(),
                        //item.Ammount.ToDottedCommaedNumber(),
                        //""
                    ]));
                }
            }
            else
            {

            }
            //
            //
            //  4) Prepare rows data
            //
            //
            DynamicApiRow.AppendRows(rows);
            //
            //
            //  5) return response
            //
            return opt.Success(DynamicApiRow);
        });


    //  اعمار مديونية
    [HttpPost("TheClientDebtionDetails")]
    public ValueTask<HttpResponseWrapper<DynamicApiReports>> PostTheDebtionDetailsReport(StatementQueryRequest request)
        =>
            TryOne<DynamicApiReports>(async opt =>
            {
                //
                //
                //  1) Prepare needed servcies
                //
                var DynamicApiRow = new DynamicApiReports();
                var entryService = opt.ServiceProvider.GetRequiredService<IEntryService>();
                var clientService = opt.ServiceProvider.GetRequiredService<IServiceBase<DtoClient, TClient>>();
                var accountService = opt.ServiceProvider.GetRequiredService<IServiceBase<DtoAccount, TAccount>>();
                var marketerService = opt.ServiceProvider.GetRequiredService<IServiceBase<DtoMarketer, TMarketer>>();
                var entryDetailsService = opt.ServiceProvider.GetRequiredService<IServiceBase<DtoEntryDetail, TEntryDetail>>();
                var marketerClientService = opt.ServiceProvider.GetRequiredService<IServiceBase<DtoMarketerClient, TMarketerClient>>();
                ICollection<DynamicApiRow> rows = [];
                //
                //
                //  2) Prepare columns headers
                //
                var selectedAccountName = "";
                var useRepresentativeId = false;
                List<Identifier> clientsAccointsIDs = [];
                if (request.AccountId is null)
                {
                    var clientsAccoints = await clientService.GetAllAsAsync(s => s.AccountId);
                    clientsAccointsIDs = clientsAccoints.Items.ToList();
                    DynamicApiRow.AppendRow(["العميل", "اجمالي الدين", "اجمالي المقبوض", "نسبة التحصيل"/*, "مدة الخلاص"*/]);

                    var representativeId = request.DynamicCombos.Where(W => W.QueryName == "RepresentativeId").FirstOrDefault();
                    if (representativeId is not null && representativeId.SelectedCombo is not null && !Identifier.Parse(representativeId.SelectedCombo.Value).IsNullOrEmpty())
                    {
                        useRepresentativeId = true;
                        var representativeIdValue = Identifier.Parse(representativeId.SelectedCombo.Value);
                        var clients = await marketerClientService.GetAllAsAsync(w => w.ClientAccountId, c => c.MarketerId == representativeIdValue);
                        clientsAccointsIDs = clients.Items.ToList();
                    }
                }
                else
                {
                    clientsAccointsIDs = [request.AccountId.Value];
                    var accointInfo = await accountService.GetOneAsAsync(s => new { s.Name }, w => w.Id == request.AccountId);
                    selectedAccountName = accointInfo!.Name;
                    DynamicApiRow.AddUpper("العميل", accointInfo!.Name);
                    DynamicApiRow.AppendRow(["اجمالي الدين", "اجمالي المقبوض", "نسبة التحصيل"/*, "مدة الخلاص"*/]);
                }
                //
                //
                //  3) Fetch the report raw data
                //
                var __clientDeptionData = 1m;
                var __clientCreditionData = 0m;
                foreach (var item in clientsAccointsIDs)
                {
                    var totlBalance = await entryService.GetBalance(item, request.StartDate, request.EndDate);
                    var clientDeptionData = await entryService.GetTotalDebitInfo(item, request.StartDate, request.EndDate);
                    if (clientDeptionData == 0) continue;
                    var clientCreditionData = await entryService.GetTotalCreditInfo(item, request.StartDate, request.EndDate);
                    var accointInfo = await accountService.GetOneAsAsync(s => new { s.Name }, w => w.Id == item);

                    __clientDeptionData += clientDeptionData;
                    __clientCreditionData += clientCreditionData;

                    var percent = (clientCreditionData * 100) / clientDeptionData;

                    if (request.AccountId.HasValue)
                    {
                        rows.Add(new DynamicApiRow(
                            [
                                clientDeptionData.ToDottedCommaedNumber(),
                            clientCreditionData.ToDottedCommaedNumber(),
                            percent.ToDottedCommaedNumber()+"%",
                            //"",
                            ]));
                    }
                    else
                    {
                        rows.Add(new DynamicApiRow(
                            [
                                accointInfo.Name ?? "",
                            clientDeptionData.ToDottedCommaedNumber(),
                            clientCreditionData.ToDottedCommaedNumber(),
                            percent.ToDottedCommaedNumber()+"%",
                            //"",
                            ]));
                    }
                }
                //
                //
                //  4) Prepare rows data
                //
                //
                DynamicApiRow.AppendRows(rows);
                if (useRepresentativeId)
                {
                    var __percent = (__clientCreditionData * 100) / __clientDeptionData;
                    DynamicApiRow.AddUpper("النسبة الاجمالية", __percent.ToDottedCommaedNumber() + "%");
                }
                //
                //
                //  5) return response
                //
                return opt.Success(DynamicApiRow);
            });


    //  اعمار مديونية  - مندوب
    [HttpPost("TheRepresentativeDebtionDetails")]
    public ValueTask<HttpResponseWrapper<DynamicApiReports>> PostTheDebtionRepresentativeDetailsReport(StatementQueryRequest request)
        =>
            TryOne<DynamicApiReports>(async opt =>
            {
                //
                //
                //  1) Prepare needed servcies
                //
                var DynamicApiRow = new DynamicApiReports();
                var entryService = opt.ServiceProvider.GetRequiredService<IEntryService>();
                var clientService = opt.ServiceProvider.GetRequiredService<IServiceBase<DtoMarketer, TMarketer>>();
                var accountService = opt.ServiceProvider.GetRequiredService<IServiceBase<DtoAccount, TAccount>>();
                var entryDetailsService = opt.ServiceProvider.GetRequiredService<IServiceBase<DtoEntryDetail, TEntryDetail>>();
                var marketerService = opt.ServiceProvider.GetRequiredService<IServiceBase<DtoMarketer, TMarketer>>();
                ICollection<DynamicApiRow> rows = [];
                //
                //
                //  2) Prepare columns headers
                //
                List<Identifier> clientsAccointsIDs = [];
                var selectedAccountName = "";
                if (request.AccountId is null)
                {
                    var clientsAccoints = await clientService.GetAllAsAsync(s => s.AccountId);
                    clientsAccointsIDs = clientsAccoints.Items.ToList();
                    DynamicApiRow.AppendRow(["المندوب",
                    "الرصيد",
                     "أخر استحقاق", "تاريخ أخر استحقاق", "حركة استحقاق", "رقم حركة استحقاق",
                        "آخر دفعة", "تاريخ آخر دفعة", "حركة الدفعة", "رقم حركة الدفع"
                     ]);
                }
                else
                {
                    var mark = await marketerService.GetOneAsAsync(s => new { s.Name, s.AccountId }, w => w.Id == request.AccountId);
                    clientsAccointsIDs = [mark.AccountId];

                    selectedAccountName = mark!.Name;
                    DynamicApiRow.AddUpper("المندوب", mark!.Name);
                    DynamicApiRow.AppendRow(["الرصيد",
                    "أخر استحقاق", "تاريخ أخر استحقاق", "حركة استحقاق", "رقم حركة استحقاق",
                    "آخر دفعة", "تاريخ آخر دفعة", "حركة الدفعة", "رقم حركة الدفع"
                    ]);
                }
                //
                //
                //  3) Fetch the report raw data
                //
                foreach (var item in clientsAccointsIDs)
                {
                    var totlBalance = await entryService.GetBalance(item);
                    var clientDeptionData = await entryService.GetLastDebitInfo(item);
                    var clientCreditionData = await entryService.GetLastCreditInfo(item);
                    var accointInfo = await accountService.GetOneAsAsync(s => new { s.Name }, w => w.Id == item);


                    if (request.AccountId.HasValue)
                    {
                        rows.Add(new DynamicApiRow(
                            [
                                totlBalance.ToDottedCommaedNumber(),

                            clientDeptionData.Ammount.ToDottedCommaedNumber(),
                            clientDeptionData.Date.ToDottedDate(),
                            clientDeptionData.OprationTypeString,
                            clientDeptionData.DisplayNumber.ToString(),

                            clientCreditionData.Ammount.ToDottedCommaedNumber(),
                            clientCreditionData.Date.ToDottedDate(),
                            clientCreditionData.OprationTypeString,
                            clientCreditionData.DisplayNumber.ToString(),


                        ]));
                    }
                    else
                    {
                        rows.Add(new DynamicApiRow(
                            [
                                accointInfo.Name ?? "",
                            totlBalance.ToDottedCommaedNumber(),


                            clientCreditionData.Ammount.ToDottedCommaedNumber(),
                            clientCreditionData.Date.ToDottedDate(),
                            clientCreditionData.OprationTypeString,
                            clientCreditionData.DisplayNumber.ToString(),


                            clientDeptionData.Ammount.ToDottedCommaedNumber(),
                            clientDeptionData.Date.ToDottedDate(),
                            clientDeptionData.OprationTypeString,
                            clientDeptionData.DisplayNumber.ToString(),


                        ]));
                    }
                }
                //
                //
                //  4) Prepare rows data
                //
                //
                DynamicApiRow.AppendRows(rows);
                //
                //
                //  5) return response
                //
                return opt.Success(DynamicApiRow);
            });


    private Identifier? GetDynamicProperty(StatementQueryRequest request, string name)
    {
        Identifier? representativeId = null;
        var representativeIdPrp = request.DynamicCombos.FirstOrDefault(x => x.QueryName == name);
        if (representativeIdPrp is not null
            && representativeIdPrp.SelectedCombo is not null
            && !representativeIdPrp.SelectedCombo.Value.IsNullOrEmpty())
        {
            representativeId = Identifier.Parse(representativeIdPrp.SelectedCombo.Value);
            representativeId = representativeId.Value;
        }
        return representativeId;
    }


    //  ارصدة العهد
    [HttpPost("ItemsMarkersQuantities")]
    public ValueTask<HttpResponseWrapper<DynamicApiReports>> PostItemsMarkersQuantities([FromBody] ItemInfoQueryRequest request)
    =>
        TryOne<DynamicApiReports>(async opt =>
        {
            //
            //
            //  1) Prepare needed servcies
            //
            ICollection<DynamicApiRow> rows = [];
            var DynamicApiRow = new DynamicApiReports();
            var storageModuleService = opt.GetService<IMarketerInventoryService>();
            //
            var representativeId = GetDynamicProperty(request, "RepresentativeId");
            if (representativeId is not null)
                request.StoreId = representativeId.Value;
            //
            //
            //  2) Prepare columns headers
            //
            DynamicApiRow.AppendRow([
                    "الصنف",
                    "المسوق",
                    "الكمية",
                    "الصلاحيات",
                ]);

            //
            //
            //  3) Fetch the report raw data
            //

            var all = await storageModuleService.GetDetails(request.StoreId, request.ItemIdd, request.UnitId, request.StartDate, request.EndDate);

            var grpByItem = all.GroupBy(w => new { w.ItemId, w.ItemName });

            foreach (var item in grpByItem)
            {
                var name = item.Key.ItemName;
                var st = item.GroupBy(g => new { g.MarketerId, g.MarketerName });

                foreach (var byStore in st)
                {
                    var storeName = byStore.Key.MarketerName;

                    foreach (var byDate in byStore.GroupBy(g => new { g.ExpireDate }))
                    {

                        var expireDate = (byDate.Key.ExpireDate.HasValue) ? byDate.Key.ExpireDate.Value.ToDottedDate() : "بدون";
                        //var cost = byDate.Key.AvrageCost;


                        var currentInqomeQauntity = byStore.Where(W => W.Ingress).Sum(W => W.TotalQuantity);
                        var currentOutcomeQauntity = byStore.Where(W => !W.Ingress).Sum(W => W.TotalQuantity);

                        var currentQauntity = currentInqomeQauntity - currentOutcomeQauntity;


                        DynamicApiRow.AppendRow([
                            name,
                            storeName,
                            currentQauntity.ToDottedCommaedNumber(),
                            expireDate,
                        ]);
                    }
                }



            }
            //
            //
            //  4) Prepare rows data
            //
            //
            //DynamicApiRow.AppendRows(rows );
            //
            //
            //  5) return response
            //
            return opt.Success(DynamicApiRow);
        });


    //  ارصدة مخازن
    [HttpPost("ItemsStoragesQuantities")]
    public ValueTask<HttpResponseWrapper<DynamicApiReports>> PostItemsStoragesQuantities([FromBody] ItemInfoQueryRequest request)
    =>
        TryOne<DynamicApiReports>(async opt =>
        {
            //
            //
            //  1) Prepare needed servcies
            //
            var DynamicApiRow = new DynamicApiReports();

            var catService = opt.GetService<DtoCategory, TCategory>();
            var itemService = opt.GetService<DtoItem, TItem>();
            var itemUnitService = opt.GetService<DtoItemUnit, TItemUnit>();
            var inventoryService = opt.GetService<DtoInventory, TInventory>();
            var inventoryCostService = opt.GetService<DtoInventoryCost, TInventoryCost>();
            var storageModuleService = opt.GetService<IPurchaseAndSalesModuleService>();

            var all = await itemUnitService.GetAllAsAsync(s => new { s.UnitId, s.ItemId, s.TransferRate, s.UnitName });
            var storageId = GetDynamicProperty(request, "storageId");

            var selectedView = request.DynamicCombos.FirstOrDefault(w => w.QueryName == "ViewId");
            var selectedUnitedProperty = request.DynamicCombos.FirstOrDefault(w => w.QueryName == "UnitId");
            var selectedCategoryId = GetDynamicProperty(request, "CategoryId");

            var selectedUnited = selectedUnitedProperty?.SelectedCombo?.Value ?? string.Empty;
            Identifier? selectedUnitedId = null;
            Identifier? selectedItemId = request.AccountId;
            ICollection<DynamicApiRow> rows = [];
            //
            //
            //  2) Prepare columns headers
            //
            var useSecure = false;// selectedView?.SelectedCombo?.Value == VIEW_MODE__SECURE;
            string[] arr = [
                    "الصنف",
                    "المخزن",
                    "الكمية",
                    "الفرطة",
                    "الصلاحيات",
                    "التكلفة",
                    "التكلفة الحالية",
                    "التكلفة الكلية",
                    "سعر البيع",
                    "سعر بيع الكمية الحالية",
                ];
            if (useSecure)
            {
                arr = [
                    "الصنف",
                    "المخزن",
                    "الكمية",
                    "الفرطة",
                    "الصلاحيات"
                ];
            }
            DynamicApiRow.AppendRow(arr);

            //
            //
            //  3) Fetch the report raw data
            //
            var inventoryMovements = (await inventoryService.GetAllAsync()).Items.ToArray();
            var inventoryCostMovements = (await inventoryCostService.GetAllAsync()).Items.ToArray();

            request.CategoryId = GetDynamicProperty(request, "CategoryId");
            if (!selectedItemId.IsNullOrEmpty())
            {
                inventoryMovements = inventoryMovements.Where(w => w.ItemId == selectedItemId).ToArray();
                inventoryCostMovements = inventoryCostMovements.Where(w => w.ItemId == selectedItemId).ToArray();
            }
            if (!selectedCategoryId.IsNullOrEmpty())
            {
                var selectedItemsIds =
                    await itemService.GetAllAsAsync(s => s.Id, w => w.CategoryId == selectedCategoryId.Value);
                inventoryMovements = inventoryMovements
                    .Where(w => selectedItemsIds.Items.Contains(w.ItemId))
                    .ToArray();
                inventoryCostMovements = inventoryCostMovements
                    .Where(w => selectedItemsIds.Items.Contains(w.Inventory.ItemId))
                    .ToArray();
            }

            if (!storageId.IsNullOrEmpty())
            {
                inventoryMovements = inventoryMovements.Where(w => w.StoreId == storageId).ToArray();
                inventoryCostMovements = inventoryCostMovements.Where(w => w.StoreId == storageId).ToArray();
            }
            //var all = await storageModuleService.GetDetails(request.StoreId, request.ItemIdd, request.UnitId, request.StartDate, request.EndDate);

            var grpByItem = inventoryMovements
                //.Where(w=> w.RelatedIndex != RelatedInventory.MarketerSalePreparation)
                .GroupBy(w => new { w.ItemId, w.ItemName, w.ItemPrice });
            decimal _realCost = 0, _totalCost = 0, _totalBeforeCost = 0, _totlPrice = 0, _totlPriceBefore = 0, _currentQauntity = 0;
            foreach (var item in grpByItem)
            {
                var name = item.Key.ItemName;
                var st = item.GroupBy(g => new { g.StoreId, g.StoreName });

                foreach (var byStore in st)
                {
                    var storeName = byStore.Key.StoreName;

                    foreach (var byDate in byStore.GroupBy(g => new { g.ExpireDate }))
                    {

                        var expireDate = (byDate.Key.ExpireDate.HasValue) ? byDate.Key.ExpireDate.Value.ToDottedDate() : "بدون";
                        //var cost = byDate.Key.AvrageCost;

                        var currentInqomeQauntity = byDate
                            .Where(W => W.Ingress && W.ItemId == item.Key.ItemId)
                            .Sum(W => W.TotalQuantity);

                        var currentOutcomeQauntity = byDate
                            .Where(W => !W.Ingress)
                            .Sum(W => W.TotalQuantity);
                        //  todo: what if item disburment without cost  ?
                        var realCost = inventoryCostMovements
                            .Where(W => W.Inventory.Ingress && W.Inventory.ItemId == item.Key.ItemId && W.ExpireDate == byDate.Key.ExpireDate)
                            .Select(W => W.AvragePiceCost)
                            .LastOrDefault() ?? 0m;
                        //
                        var currentQauntity = currentInqomeQauntity - currentOutcomeQauntity;
                        if (currentQauntity is 0) continue;

                        var biggestCurrentQauntity = 0m;
                        var extraCurrentQauntity = currentQauntity;
                        var extraCurrentQauntityName = "";
                        var biggestCurrentQauntityName = "";
                        //if (selectedUnitedId is not null)
                        {
                            var uniQ = all.Items.Where(W => W.ItemId == item.Key.ItemId);
                            if (uniQ.Any())
                            {
                                var mini = uniQ.MinBy(w => w.TransferRate);
                                var maxi = uniQ.MaxBy(w => w.TransferRate);

                                extraCurrentQauntityName = mini?.UnitName ?? "";
                                biggestCurrentQauntityName = maxi?.UnitName ?? "";

                                biggestCurrentQauntity = Math.Floor(currentQauntity / maxi.TransferRate);
                                extraCurrentQauntity = currentQauntity % maxi.TransferRate;
                            }
                        }
                        //
                        var maxCost = inventoryCostMovements.Average(s => s.AvrageCost);
                        var totalBeforeCost = realCost * currentInqomeQauntity;
                        var totalCost = realCost * currentQauntity;
                        var price = item.Key.ItemPrice;
                        var priceTotal = item.Key.ItemPrice * currentQauntity;

                        _currentQauntity += currentQauntity;
                        _realCost += realCost;
                        _totalCost += totalCost;
                        _totalBeforeCost += totalBeforeCost;

                        _totlPrice += price;
                        _totlPriceBefore += priceTotal;

                        string[] vvv = [
                                name,
                                storeName,
                                biggestCurrentQauntity.ToDottedCommaedNumber() + " " + biggestCurrentQauntityName,
                                extraCurrentQauntity.ToDottedCommaedNumber() + " " + extraCurrentQauntityName,
                                expireDate,
                                realCost.ToDottedCommaedNumber(),
                                totalCost.ToDottedCommaedNumber(),
                                totalBeforeCost.ToDottedCommaedNumber(),
                                item.Key.ItemPrice.ToDottedCommaedNumber(),
                                (item.Key.ItemPrice * currentQauntity).ToDottedCommaedNumber(),
                            ];
                        if (useSecure)
                        {
                            vvv = [
                                name,
                                    storeName,
                                    biggestCurrentQauntity.ToDottedCommaedNumber() + " " + biggestCurrentQauntityName,
                                    extraCurrentQauntity.ToDottedCommaedNumber() + " " + extraCurrentQauntityName,
                                    expireDate,
                                ];
                        }

                        DynamicApiRow.AppendRow(vvv);
                    }
                }
            }
            if (!useSecure)
            {
                DynamicApiRow.AppendRow([
                                "-------",
                                "-------",
                                "-------",
                                _currentQauntity.ToDottedCommaedNumber(),
                                "-------",
                                _realCost.ToDottedCommaedNumber(),
                                _totalCost.ToDottedCommaedNumber(),
                                _totalBeforeCost.ToDottedCommaedNumber(),
                                _totlPrice.ToDottedCommaedNumber(),
                                _totlPriceBefore.ToDottedCommaedNumber(),
                            ]);
            }
            //DynamicApiRow.AddLower("اجمالي التكلفة", _realCost.ToDottedCommaedNumber());
            //DynamicApiRow.AddLower("اجمالي التكلفة الحالية", _totalCost.ToDottedCommaedNumber());
            //DynamicApiRow.AddLower("اجمالي التكلفة الكلية", _totalBeforeCost.ToDottedCommaedNumber());
            //DynamicApiRow.AddLower("اجمالي البيع", _totlPrice.ToDottedCommaedNumber());
            //DynamicApiRow.AddLower("اجمالي بيع الكملية الحالية", _totlPriceBefore.ToDottedCommaedNumber());
            //
            //
            //  4) Prepare rows data
            //
            //
            //DynamicApiRow.AppendRows(rows );
            //
            //
            //  5) return response
            //
            return opt.Success(DynamicApiRow);
        });

    [HttpPost("DamageItems")]
    public ValueTask<HttpResponseWrapper<DynamicApiReports>> PostItemsDamagedQuantities([FromBody] ItemInfoQueryRequest request)
    =>
        TryOne<DynamicApiReports>(async opt =>
        {
            var dynamicApiRow = new DynamicApiReports();
            var itemService = opt.GetService<DtoItem, TItem>();
            var inventoryService = opt.GetService<DtoInventory, TInventory>();
            var selectedCategoryId = GetDynamicProperty(request, "CategoryId");
            Identifier? selectedUnitedId = null;
            Identifier? selectedItemId = request.AccountId;
            ICollection<DynamicApiRow> rows = [];

            var storageId = GetDynamicProperty(request, "storageId");

            string[] arr = [
                "التاريخ",
                    "المخزن",
                    "الصنف",
                    "الكمية",
                    "الكمية الكلية",
                ];

            dynamicApiRow.AppendRow(arr);
            
            var inventoryMovements = (await inventoryService.GetAllAsync()).Items.ToArray();

            request.CategoryId = GetDynamicProperty(request, "CategoryId");
            if (!selectedItemId.IsNullOrEmpty())
            {
                inventoryMovements = inventoryMovements.Where(w => w.ItemId == selectedItemId).ToArray();
            }
            if (!selectedCategoryId.IsNullOrEmpty())
            {
                var selectedItemsIds =
                    await itemService.GetAllAsAsync(s => s.Id, w => w.CategoryId == selectedCategoryId.Value);
                inventoryMovements = inventoryMovements
                    .Where(w => selectedItemsIds.Items.Contains(w.ItemId))
                    .ToArray();
            }

            if (!storageId.IsNullOrEmpty())
            {
                inventoryMovements = inventoryMovements.Where(w => w.StoreId == storageId).ToArray();
            }
            
            inventoryMovements = inventoryMovements.Where(x => x.RelatedIndex == RelatedInventory.Damaged).ToArray();
            var grpByItem = inventoryMovements
                .GroupBy(w => new { w.ItemId, w.ItemName, w.UnitName, w.ItemPrice, w.StoreName });
            foreach (var item in inventoryMovements)
            {
                dynamicApiRow.AppendRow([item.CreateDate.ToDottedDate(), item.StoreName, item.ItemName, $"{item.UnitName} {item.Quantity}", item.TotalQuantity.ToString()]);
            }

            return opt.Success(dynamicApiRow);
        });
    
    string GetTypeString(RelatedInventory related) => related switch
    {
        RelatedInventory.None => "غير محدد",
        RelatedInventory.Purchase => "فاتورة مشتريات",
        RelatedInventory.Sale => "فاتورة مبيعات",
        RelatedInventory.Transfer => "نقل مواد",
        RelatedInventory.Marketer => "",
        RelatedInventory.Disbursement => "صرف",
        RelatedInventory.Disbursement_Return => "إرجاع صرف",
        RelatedInventory.Supply => "توريد",
        RelatedInventory.Supply_Return => "إرجاع توريد",
        RelatedInventory.MarketerDisbursement => "صرف لمندوب",
        RelatedInventory.ReturnSale => "إرجاع مبيعات",
        RelatedInventory.QuantityCrediting => "ترصيد",
        RelatedInventory.QuantityCrediting_Zero => "تصفير",
        RelatedInventory.MarketerSalePreparationUndo => "تراجع عن الصرف",
        RelatedInventory.MarketerSalePreparation_Marketer => "بيع من عهدة المسوق",
        RelatedInventory.ReturnPurchase => "ارجاع مشتريات",
        RelatedInventory.Maintenance => "صيانة",
        RelatedInventory.Factoring => "تصنيع",
        RelatedInventory.Damaged => "أتلاف",
        _ => "غير مدرجة"
    };

    //  حركة مخازن
    //  item movment
    [HttpPost("ItemsMovements")]
    public ValueTask<HttpResponseWrapper<DynamicApiReports>> PostItemsMovements([FromBody] ItemInfoQueryRequest request)
    =>
        TryOne<DynamicApiReports>(async opt =>
        {

            //
            //
            //  1) Prepare needed servcies
            //
            var DynamicApiRow = new DynamicApiReports();

            var saleService = opt.GetService<DtoSalesInvoice, TSalesInvoice>();
            var purchesService = opt.GetService<DtoPurchaseInvoice, TPurchaseInvoice>();
            var sellInvouceReturnService = opt.GetService<DtoSellReturnInvoice, TSellReturnInvoice>();
            var purchaseReturnInvoice = opt.GetService<DtoPurchaseReturnInvoice, TPurchaseReturnInvoice>();
            var quantityCreditingInvoiceService = opt.GetService<DtoQuantityCreditingInvoice, TQuantityCreditingInvoice>();
            var disbursementPermitInvoiceService = opt.GetService<DtoMarketerDisbursementPermit, TMarketerDisbursementPermit>();

            //

            var storageModuleService = opt.GetService<IPurchaseAndSalesModuleService>();
            var storageModuleServic3e = opt.GetService<IMarketerInventoryService>();
            var storageModuleServic2e = opt.GetService<IStorageModuleService>();

            var storageId = GetDynamicProperty(request, "storageId");

            ICollection<DynamicApiRow> rows = [];
            //
            //
            //  2) Prepare columns headers
            //
            DynamicApiRow.AppendRow([
                    $"{SharedConstants.InternalTableColumn}_ItemId",
                    "رقم الفاتورة",
                    "تاريخ",
                    "الصنف",
                    "الوحدة",
                    "المخزن",
                    "نوع الحركة",
                    "الكمية",
                    "الصلاحيات",
                    //"الكميـة",
                    //"الرصيد",
                ]);

            //
            //
            //  3) Fetch the report raw data
            //
            async Task<List<DtoInventoryCost>> GetAllMovements()
            {
                var inventoriesCosts = await storageModuleService.GetMovements(request.AccountId, storageId);
                var preInventories = (await storageModuleServic2e.GetMovements(request.AccountId, storageId)).ToList();
                var marketerInventories = (await storageModuleServic3e.GetMovements(request.AccountId, storageId)).ToList();

                var __x = inventoriesCosts.Select(ss => ss.TInventoryId);
                preInventories.RemoveAll(w => __x.Contains(w.Id));

                var inventories = preInventories.Select(s => new DtoInventoryCost()
                {
                    Inventory = s,
                });
                var all = inventoriesCosts
                    .Concat(inventories)
                    .Concat(
                        from a in marketerInventories
                        select new DtoInventoryCost()
                        {
                            Inventory = new DtoInventory()
                            {
                                Id = a.Id,
                                ItemId = a.ItemId,
                                UnitId = a.UnitId,
                                Ingress = a.Ingress,
                                ItemName = a.ItemName,
                                UnitName = a.UnitName,
                                Quantity = a.Quantity,
                                RelatedId = a.RelatedId,
                                ExpireDate = a.ExpireDate,
                                CreateDate = a.CreateDate,
                                RelatedIndex = a.RelatedIndex,
                                OprationDate = a.OprationDate,
                                CreatedByName = a.CreatedByName,
                                TotalQuantity = a.TotalQuantity,
                                AdditionalQuantity = a.AdditionalQuantity,

                                StoreId = a.MarketerId,
                                StoreName = a.MarketerName,
                            }
                        }
                    ).ToList();

                return all;
            }

            var all = await GetAllMovements();

            var targetSalesinvoices = all
                .Where(w =>
                    (
                        
                        w.Inventory.RelatedIndex == RelatedInventory.Sale
                        || w.Inventory.RelatedIndex == RelatedInventory.MarketerSalePreparation_Marketer
                    )
                )
                .Select(s => s.Inventory.RelatedId).ToArray();

            var targetPurchesInvoices = all
                .Where(w => (w.Inventory.RelatedIndex == RelatedInventory.Purchase))
                .Select(s => s.Inventory.RelatedId).ToArray();

            var targetReturnInvoices = all
                .Where(w => (w.Inventory.RelatedIndex == RelatedInventory.ReturnSale))
                .Select(s => s.Inventory.RelatedId).ToArray();

            var quantityCreditingInvoices = all
                .Where(w => (
                    w.Inventory.RelatedIndex == RelatedInventory.QuantityCrediting
                    || w.Inventory.RelatedIndex == RelatedInventory.QuantityCrediting_Zero
                 ))
                .Select(s => s.Inventory.RelatedId).ToArray();

            var disbursementPermitInvoice = all
                .Where(w =>
                    w.Inventory.RelatedIndex == RelatedInventory.MarketerDisbursement ||
                    w.Inventory.RelatedIndex == RelatedInventory.MarketerSalePreparation_Marketer
                )
                .Select(s => s.Inventory.RelatedId).ToArray();

            var returnPurchaseInvoice = all
                .Where(w =>
                    w.Inventory.RelatedIndex == RelatedInventory.ReturnPurchase
                )
                .Select(s => s.Inventory.RelatedId).ToArray();


            var salesInvoices = (await saleService.GetAllAsAsync(s => new
            {
                s.Id,
                s.Number,
                s.Date
            }, w => targetSalesinvoices.Contains(w.Id))).Items;

            var purcesInvoices = (await purchesService.GetAllAsAsync(s => new
            {
                s.Id,
                s.Number,
                s.Date
            }, w => targetPurchesInvoices.Contains(w.Id))).Items;

            var returnInvoices = (await sellInvouceReturnService.GetAllAsAsync(s => new
            {
                s.Id,
                s.Number,
                s.Date
            }, w => targetReturnInvoices.Contains(w.Id))).Items;

            var creditingInvoicess = (await quantityCreditingInvoiceService.GetAllAsAsync(s => new
            {
                s.Id,
                s.Number,
                s.Date
            }, w => quantityCreditingInvoices.Contains(w.Id))).Items;

            var disbursementPermit = (await disbursementPermitInvoiceService.GetAllAsAsync(s => new
            {
                s.Id,
                s.Number,
                s.Date
            }, w => disbursementPermitInvoice.Contains(w.Id))).Items;

            var returnPurchase = (await purchaseReturnInvoice.GetAllAsAsync(s => new
            {
                s.Id,
                s.Number,
                s.Date
            }, w => returnPurchaseInvoice.Contains(w.Id))).Items;
            //
            //
            //
            var grpByItem1 = all.Join(salesInvoices, x => x.Inventory.RelatedId, y => y.Id, (inv, sls) => new { sls, inv })
            .Where(w =>
                    w.inv.Inventory.RelatedIndex != RelatedInventory.MarketerSalePreparation_Sale
                    && w.inv.Inventory.RelatedIndex != RelatedInventory.MarketerSaleSell
                 //|| w.inv.Inventory.RelatedIndex == RelatedInventory.QuantityCrediting_Zero
                 )
            .ToList();

            var grpByItem2 = all.Join(purcesInvoices, x => x.Inventory.RelatedId, y => y.Id, (inv, sls) => new { sls, inv }).ToList();
            var grpByItem3 = all.Join(returnInvoices, x => x.Inventory.RelatedId, y => y.Id, (inv, sls) => new { sls, inv }).ToList();

            var grpByItem4 = all
            .Join(creditingInvoicess, x => x.Inventory.RelatedId, y => y.Id, (inv, sls) => new { sls, inv })
            .Where(w =>
                    w.inv.Inventory.RelatedIndex == RelatedInventory.QuantityCrediting
                 //|| w.inv.Inventory.RelatedIndex == RelatedInventory.QuantityCrediting_Zero
                 )
            .ToList();

            var grpByItem5 = all.Join(disbursementPermit, x => x.Inventory.RelatedId, y => y.Id, (inv, sls) => new { sls, inv }).ToList();
            var grpByItem6 = all.Join(returnPurchase, x => x.Inventory.RelatedId, y => y.Id, (inv, sls) => new { sls, inv }).ToList();
            //
            //
            var grpByItem0 = grpByItem1
                .Concat(grpByItem2)
                .Concat(grpByItem3)
                .Concat(grpByItem4)
                .Concat(grpByItem5)
                .Concat(grpByItem6)
                .ToList();
            //
            var sort = grpByItem0.OrderBy(s => s.sls.Date).ToList();
            var chach = new Dictionary<string, decimal>();
            decimal GetBalance(DtoInventoryCost dtoInventory)
            {
                var key = $"{dtoInventory.ItemId}-{dtoInventory.StoreId}-{dtoInventory.ExpireDate.ToDottedDate()}";
                var balance = 0m;
                if (chach.ContainsKey(key))
                {
                    if (dtoInventory.Inventory.RelatedIndex == RelatedInventory.QuantityCrediting)
                    {
                        chach[key] = dtoInventory.Inventory.TotalQuantity;
                    }
                    else if (dtoInventory.Inventory.RelatedIndex == RelatedInventory.QuantityCrediting_Zero)
                    {

                    }
                    else
                    {
                        if (dtoInventory.Inventory.Ingress)
                            chach[key] += dtoInventory.Inventory.TotalQuantity;
                        else
                            chach[key] -= dtoInventory.Inventory.TotalQuantity;
                    }
                }
                else
                {
                    chach.Add(key, dtoInventory.Inventory.TotalQuantity);
                }
                balance = chach[key];
                return balance;
            };
            //

            var singleTotalQuantity = 0m;
            
            foreach (var item in sort.Where(i => i.sls.Date < request.EndDate && i.sls.Date > request.StartDate))
            {
                if (item.inv.ItemId == request.AccountId)
                {
                    if (item.inv.Inventory.Ingress)
                    {
                        singleTotalQuantity += item.inv.Inventory.Quantity;
                    }
                    else
                    {
                        singleTotalQuantity -= item.inv.Inventory.Quantity;
                    }
                }
                
                DynamicApiRow.AppendRow([
                    $"{item.inv.ItemId.ToString() ?? ""}",
                    item.sls.Number.ToString(),
                    item.sls.Date.ToDashedDate(),
                    item.inv.ItemName,
                    item.inv.UnitName,
                    item.inv.StoreName,
                    GetTypeString(item.inv.Inventory.RelatedIndex),
                    item.inv.Inventory.Quantity.ToDottedCommaNumberWithSign(item.inv.Inventory.Ingress),
                    item.inv.ExpireDate.ToDashedDate(),
                    //item.inv.Inventory.TotalQuantity.ToDottedCommaedNumber(),
                    //GetBalance(item.inv).ToDottedCommaComputedNumber(),
                ]);
            }

            if (!request.AccountId.IsNullOrEmpty())
            {
                DynamicApiRow.AppendRow([
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    singleTotalQuantity.ToDottedCommaComputedNumber(),
                ]);
                
                DynamicApiRow.AddLower("الأجمالي:", singleTotalQuantity.ToDottedCommaComputedNumber());
            }
            //
            //  4) Prepare rows data
            //
            //  5) return response
            //
            return opt.Success(DynamicApiRow);
        });
    //
    //  item movment
    [HttpPost("RepresentativeItemsMovements")]
    public ValueTask<HttpResponseWrapper<DynamicApiReports>> PostRepresentativeItemsMovements([FromBody] ItemInfoQueryRequest request)
    =>
        TryOne<DynamicApiReports>(async opt =>
        {
            //
            //
            //
            var saleInvoiceServcie = opt.GetService<DtoSalesInvoice, TSalesInvoice>();
            //
            //
            //
            //  1) Prepare needed servcies
            //
            var DynamicApiRow = new DynamicApiReports();

            var RepresentativeIdCombo = request.DynamicCombos.FirstOrDefault(w => w.QueryName == "RepresentativeId");
            Identifier? RepresentativeId = null;
            if (RepresentativeIdCombo is not null && RepresentativeIdCombo.SelectedCombo is not null)
            {
                RepresentativeId = Identifier.Parse(RepresentativeIdCombo.SelectedCombo.Value);
            }


            var storageModuleService = opt.GetService<IPurchaseAndSalesModuleService>();

            ICollection<DynamicApiRow> rows = [];
            //
            //
            //  2) Prepare columns headers
            //
            DynamicApiRow.AppendRow([
                    "رقم الفاتورة",
                    "تاريخ",
                    "المخزن",
                    "العميل",
                    "المندوب",
                    "نسبة المندوب",
                    "حصة المندوب",
                    "اجمالي الفاتورة",
                    //"رقم الفاتورة",
                ]);

            //
            //
            //  3) Fetch the report raw data
            //

            // todo: inhencment



            var getSalesWhere = saleInvoiceServcie.Build();
            getSalesWhere = getSalesWhere.And(w => w.RepresentativeId != null);
            if (RepresentativeId is not null)
            {
                getSalesWhere = getSalesWhere.And(w => w.RepresentativeId == RepresentativeId);
            }
            var sales = await saleInvoiceServcie.GetAllAsAsync(s => new
            {
                s.Id,
                s.RepresentativeId,
                s.RepresentativeName,
                s.RepresentativePercentage,
                Apunt = ((s.RepresentativePercentage / 100) * s.TotalAmount),
                s.TotalAmount,
                s.Date,
                s.ClientName,
                s.Number,
                s.StoreName,
                s.MarketerStorageName
            }, getSalesWhere);

            foreach (var item in sales.Items)
            {
                var ds = item.Date.ToDottedDate() ?? "";
                DynamicApiRow.AppendRow([
                            item.Number.ToString(),
                            ds,
                            item.StoreName ?? item.MarketerStorageName ?? "",
                            item.ClientName ?? "",
                            item.RepresentativeName ?? "",
                            (item.RepresentativePercentage).ToDottedCommaedNumber(),
                            (item.Apunt).ToDottedCommaedNumber(),
                            item.TotalAmount.ToDottedCommaedNumber(),
                        ]);

            }
            //
            //
            //  4) Prepare rows data
            //
            //
            //DynamicApiRow.AppendRows(rows );
            //
            //
            //  5) return response
            //
            return opt.Success(DynamicApiRow);
        });
    //
    
    
    [HttpPost("MaintenanceInvoicesByStatus")]
    public ValueTask<HttpResponseWrapper<DynamicApiReports>> PostMaintenanceInvoicesByStatus([FromBody] StatementQueryRequest request)
        => TryOne<DynamicApiReports>(async opt =>
        {
            // 1) Prepare needed services
            var maintenanceInvoiceService = opt.ServiceProvider.GetRequiredService<IServiceBase<DtoMaintenanceInvoice, TMaintenanceInvoice>>();
            var DynamicApiRow = new DynamicApiReports();
            ICollection<DynamicApiRow> rows = [];

            // 2) Extract Status from DynamicCombos
            var statusCombo = request.DynamicCombos.FirstOrDefault(x => x.QueryName == "StatusId");
            var statusItemsCombo = request.DynamicCombos.FirstOrDefault(x => x.QueryName == "ItemsStatusId");
            var clientCombo = request.DynamicCombos.FirstOrDefault(x => x.QueryName == "ClientId");
            int? selectedStatus = null;
            int? selectedItemsStatus = null;
            if (statusCombo?.SelectedCombo?.Display is not null && int.TryParse(statusCombo.SelectedCombo.Value, out var statusVal))
                selectedStatus = statusVal;
            if (statusItemsCombo?.SelectedCombo?.Display is not null && int.TryParse(statusItemsCombo.SelectedCombo.Value, out var statusItemsVal))
                selectedItemsStatus = statusItemsVal;
            // 3) Prepare columns headers
            DynamicApiRow.AppendRow([
                "رقم الفاتورة", // Invoice Number
                "تاريخ الفاتورة", // Invoice Date
                "تاريخ الانتهاء", // End Date
                "اسم العميل",    // Client Name
                "الحالة"         // Status
            ]);
            var where = maintenanceInvoiceService.Build();

            if (clientCombo?.SelectedCombo != null)
            {
                var clientService = opt.ServiceProvider.GetRequiredService<IServiceBase<DtoClient, TClient>>();
                var id = Identifier.Parse(clientCombo.SelectedCombo.Value);
                var selectedClient =
                    await clientService.GetOneAsAsync(x => x, y => y.Id == id);
            
                if (selectedClient != null)
                {
                    where = where.And(w => w.ClientId.Value == selectedClient.Id);
                    DynamicApiRow.Uppers.Add(new DynamicText { Value = selectedClient.Name, Title = "العميل"});
                }
            }
            
            // 4) Build filter: by Status and Date range
            if (selectedStatus.HasValue)
            {
                where = where.And(w => (int)w.MaintenanceStatus == selectedStatus.Value);
                DynamicApiRow.Uppers.Add(new DynamicText { Value = statusCombo?.SelectedCombo?.Display, Title = "حالة الصيانة"});
            }
           
            if (selectedItemsStatus.HasValue)
            {
                where = where.And(w => (int)w.MaintenanceItemsStatus == selectedItemsStatus.Value);
                DynamicApiRow.Uppers.Add(new DynamicText { Value = statusItemsCombo?.SelectedCombo?.Display, Title = "حالة الأصناف"});
            }
            
            if (request.StartDate != default && request.EndDate != default)
                where = where.And(w => w.Date >= request.StartDate && w.Date <= request.EndDate);

            // 5) Fetch MaintenanceInvoices filtered by Status and Date
            var invoices = await maintenanceInvoiceService.GetAllAsync(where);

            // 6) Prepare rows data
            foreach (var invoice in invoices.Items)
            {
                rows.Add(new DynamicApiRow([
                    invoice.Number.ToString() ?? "",
                    invoice.Date.ToDottedDate() ?? "",
                    invoice.EndDate.ToDottedDate() ?? "",
                    invoice.ClientName ?? "",
                    MaintenanceStatusToString(invoice.MaintenanceStatus)
                ]));
            }

            DynamicApiRow.AppendRows(rows);

            // 7) Return response
            return opt.Success(DynamicApiRow);
        });
    
    [HttpPost("FactoringInvoicesByStatus")]
    public ValueTask<HttpResponseWrapper<DynamicApiReports>> PostFactoringInvoicesByStatus([FromBody] StatementQueryRequest request)
        => TryOne<DynamicApiReports>(async opt =>
        {
            // 1) Prepare needed services
            var factoringInvoiceService = opt.ServiceProvider.GetRequiredService<IServiceBase<DtoFactoringInvoice, TFactoringInvoice>>();
            var DynamicApiRow = new DynamicApiReports();
            ICollection<DynamicApiRow> rows = [];

            // 2) Extract Status from DynamicCombos
            var statusCombo = request.DynamicCombos.FirstOrDefault(x => x.QueryName == "StatusId");
            var statusItemsCombo = request.DynamicCombos.FirstOrDefault(x => x.QueryName == "ItemsStatusId");
            var clientCombo = request.DynamicCombos.FirstOrDefault(x => x.QueryName == "ClientId");
            int? selectedStatus = null;
            if (statusCombo?.SelectedCombo?.Display is not null && int.TryParse(statusCombo.SelectedCombo.Value, out var statusVal))
                selectedStatus = statusVal;

            int? selectedItemsStatus = null;
            if (statusItemsCombo?.SelectedCombo?.Display is not null && int.TryParse(statusItemsCombo.SelectedCombo.Value, out var statusItemsVal))
                selectedItemsStatus = statusItemsVal;

            // 3) Prepare columns headers
            DynamicApiRow.AppendRow([
                "رقم الفاتورة", // Invoice Number
                "تاريخ الفاتورة", // Invoice Date
                "تاريخ الانتهاء", // End Date
                "اسم العميل",    // Client Name
                "الحالة"         // Status
            ]);
            
            // 4) Build filter: by Status and Date range
            var where = factoringInvoiceService.Build();
            
            if (clientCombo?.SelectedCombo != null)
            {
                var clientService = opt.ServiceProvider.GetRequiredService<IServiceBase<DtoClient, TClient>>();
                var id = Identifier.Parse(clientCombo.SelectedCombo.Value);
                var selectedClient =
                    await clientService.GetOneAsAsync(x => x, y => y.Id == id);
                
                if (selectedClient != null)
                {
                    where = where.And(w => w.ClientId.Value == selectedClient.Id);
                    DynamicApiRow.Uppers.Add(new DynamicText { Value = selectedClient.Name, Title = "العميل"});
                }
            }
            
            if (selectedStatus.HasValue)
            {
                where = where.And(w => (int)w.FactoringStatus == selectedStatus.Value);
                DynamicApiRow.Uppers.Add(new DynamicText { Value = statusCombo?.SelectedCombo?.Display, Title = "حالة التصنيع"});
            }
            
            if (selectedItemsStatus.HasValue)
            {
                where = where.And(w => (int)w.FactoringItemsStatus == selectedItemsStatus.Value);
                DynamicApiRow.Uppers.Add(new DynamicText { Value = statusCombo?.SelectedCombo?.Display, Title = "حالة الأصناف"});
            }
            
            if (request.StartDate != default && request.EndDate != default)
                where = where.And(w => w.Date >= request.StartDate && w.Date <= request.EndDate);

            // 5) Fetch MaintenanceInvoices filtered by Status and Date
            var invoices = await factoringInvoiceService.GetAllAsync(where);

            // 6) Prepare rows data
            foreach (var invoice in invoices.Items)
            {
                rows.Add(new DynamicApiRow([
                    invoice.Number.ToString() ?? "",
                    invoice.Date.ToDottedDate() ?? "",
                    invoice.EndDate.ToDottedDate() ?? "",
                    invoice.ClientName ?? "",
                    FactoringStatusToString(invoice.FactoringStatus)
                ]));
            }

            DynamicApiRow.AppendRows(rows);

            // 7) Return response
            return opt.Success(DynamicApiRow);
        });
    
    //  item movment
    [HttpPost("RepresentativeItemsMovementDetails")]
    public ValueTask<HttpResponseWrapper<DynamicApiReports>> PostRepresentativeItemsMovementDetails([FromBody] ItemInfoQueryRequest request)
    =>
        TryOne<DynamicApiReports>(async opt =>
        {
            //
            //
            //
            var saleInvoiceServcie = opt.GetService<DtoSalesInvoice, TSalesInvoice>();
            //
            //
            //
            //  1) Prepare needed servcies
            //
            var DynamicApiRow = new DynamicApiReports();

            var RepresentativeIdCombo = request.DynamicCombos.FirstOrDefault(w => w.QueryName == "RepresentativeId");
            Identifier? RepresentativeId = null;
            if (RepresentativeIdCombo is not null && RepresentativeIdCombo.SelectedCombo is not null)
            {
                RepresentativeId = Identifier.Parse(RepresentativeIdCombo.SelectedCombo.Value);
            }


            var storageModuleService = opt.GetService<IPurchaseAndSalesModuleService>();

            ICollection<DynamicApiRow> rows = [];
            //
            //
            //  2) Prepare columns headers
            //
            DynamicApiRow.AppendRow([
                    "رقم الفاتورة",
                    "تاريخ",
                    "الصنف",
                    "المخزن",
                    "العميل",
                    "المندوب",
                    "نسبة المندوب",
                    "حصة المندوب",
                    "اجمالي الفاتورة",
                    "الكمية",
                    "الصلاحية",
                    "نوع الحركة",
                    //"رقم الفاتورة",
                ]);

            //
            //
            //  3) Fetch the report raw data
            //

            // todo: inhencment

            var salesIds = (await saleInvoiceServcie.GetAllAsAsync(
                s => s.Id
                , w => w.RepresentativeId != null && request.StartDate <= w.Date && request.EndDate >= w.Date
            )).Items.ToList();


            var all = await storageModuleService.GetMovements(request.ItemIdd, request.StoreId, salesIds.ToArray(), RelatedInventory.Sale);

            all = all.Where(w => w.Inventory.RelatedIndex == RelatedInventory.Sale && salesIds.Contains(w.Inventory.RelatedId.Value));
            var targetSalesinvoices = all.Select(s => s.Inventory.RelatedId).ToArray();

            var getSalesWhere = saleInvoiceServcie.Build();
            getSalesWhere.And(w => targetSalesinvoices.Contains(w.Id) & w.RepresentativeId != null);
            if (RepresentativeId is not null)
            {
                getSalesWhere = getSalesWhere.And(w => w.RepresentativeId == RepresentativeId);
            }

            var sales = await saleInvoiceServcie.GetAllAsAsync(s => new
            {
                s.Id,
                s.RepresentativeId,
                s.RepresentativeName,
                s.RepresentativePercentage,
                Apunt = ((s.RepresentativePercentage / 100) * s.TotalAmount),
                s.TotalAmount,
                s.Date,
                s.ClientName,
                s.Number
            }, getSalesWhere);

            var joined = all
                .Join(sales.Items,
                    inv => inv.Inventory.RelatedId,
                    sls => sls.Id,
                    (inv, sls) =>
                        new
                        {
                            inv,
                            sls

                        });
            var grpByItem = joined
                .GroupBy(w => new { w.inv.ItemId, w.inv.ItemName });

            foreach (var item in joined)
            {
                var ds = item.sls.Date.ToDottedDate() ?? "";
                DynamicApiRow.AppendRow([
                            item.sls.Number.ToString(),
                            ds,
                            item.inv.ItemName,
                            item.inv.StoreName,
                            item.sls?.ClientName ?? "",
                            item.sls?.RepresentativeName ?? "",
                            (item.sls?.RepresentativePercentage ?? 0).ToDottedCommaedNumber(),
                            item.sls.Apunt.ToDottedCommaedNumber(),
                            item.sls.TotalAmount.ToDottedCommaedNumber(),
                            item.inv.Inventory.Quantity.ToDottedCommaedNumber(),
                            item.inv.ExpireDate.ToDottedDate() ?? "بدون",
                            GetTypeString(item.inv.Inventory.RelatedIndex)
                        ]);

            }
            //
            //
            //  4) Prepare rows data
            //
            //
            //DynamicApiRow.AppendRows(rows );
            //
            //
            //  5) return response
            //
            return opt.Success(DynamicApiRow);
        });
    //
    // 9- sale item profit
    [HttpPost("SalesItemsProfit")]
    public ValueTask<HttpResponseWrapper<DynamicApiReports>> PostSalesItemsProfit([FromBody] ItemInfoQueryRequest request)
        =>
        TryOne<DynamicApiReports>(async opt =>
        {
            //
            //
            //  1) Prepare needed servcies
            //
            var DynamicApiRow = new DynamicApiReports();

            var itemService = opt.GetService<DtoItem, TItem>();
            var storageModuleService = opt.GetService<IPurchaseAndSalesModuleService>();

            ICollection<DynamicApiRow> rows = [];
            //
            //
            //  2) Prepare columns headers
            //
            DynamicApiRow.AppendRow([
                    "الصنف",
                    "المخزن",
                    "الكمية",
                    "الصلاحيات",
                    "التكلفة",
                ]);

            //
            //
            //  3) Fetch the report raw data
            //

            var all = await storageModuleService.GetDetails(request.StoreId, request.ItemIdd, request.UnitId, request.StartDate, request.EndDate);

            var grpByItem = all.GroupBy(w => new { w.ItemId, w.ItemName });

            foreach (var item in grpByItem)
            {
                var name = item.Key.ItemName;
                var st = item.GroupBy(g => new { g.StoreId, g.StoreName });

                foreach (var byStore in st)
                {
                    var storeName = byStore.Key.StoreName;

                    foreach (var byDate in byStore.GroupBy(g => g.ExpireDate))
                    {

                        var expireDate = (byDate.Key.HasValue) ? byDate.Key.Value.ToDottedDate() : "بدون";

                        var currentInqomeQauntity = byStore.Where(W => W.Inventory.Ingress).Sum(W => W.Inventory.TotalQuantity);
                        var currentOutcomeQauntity = byStore.Where(W => !W.Inventory.Ingress).Sum(W => W.Inventory.TotalQuantity);
                        var currentQauntity = currentInqomeQauntity - currentOutcomeQauntity;

                        //var maxCost = byDate.Max(s => s.AvrageCost);
                        var itemInfo = await itemService.GetOneAsAsync(s => new { s.Cost }, w => w.Id == item.Key.ItemId);
                        var maxCost = itemInfo.Cost;

                        DynamicApiRow.AppendRow([
                            name,
                            storeName,
                            currentQauntity.ToDottedCommaedNumber(),
                            expireDate,
                            maxCost.ToDottedCommaedNumber(),
                        ]);
                    }
                }



            }
            //
            //
            //  4) Prepare rows data
            //
            //
            //DynamicApiRow.AppendRows(rows );
            //
            //
            //  5) return response
            //
            return opt.Success(DynamicApiRow);
        });

    [HttpPost("SalesItemsProfit3")]
    public ValueTask<HttpResponseWrapper<DynamicApiReports>> PostSalesItemsProfit3([FromBody] ItemInfoQueryRequest request)
        =>
        TryOne<DynamicApiReports>(async opt =>
        {
            //
            //
            //  1) Prepare needed servcies
            //
            var DynamicApiRow = new DynamicApiReports();

            var itemService = opt.GetService<DtoItem, TItem>();
            var storageModuleService = opt.GetService<IPurchaseAndSalesModuleService>();

            ICollection<DynamicApiRow> rows = [];
            //
            //
            //  2) Prepare columns headers
            //
            var totalQ = 0m;
            var totalCost = 0m;
            var totalPrice = 0m;

            DynamicApiRow.AppendRow([
                    "الصنف",
                    "الكمية",
                    "التكلفة",
                    "البيع",
                ]);

            //
            //
            //  3) Fetch the report raw data
            //
            var selectedItemID = request.AccountId;
            var all = await storageModuleService.GetDetails(itemId: selectedItemID, from: request.StartDate, to: request.EndDate);

            all = all.Where(w => w.Inventory.RelatedIndex == RelatedInventory.Sale);

            var allItemsIds = all.Select(s => s.ItemId).Distinct().ToArray();
            var itemsInfo = (await itemService.GetAllAsAsync(itm => new { itm.Price, itm.Cost, itm.Id }, w => allItemsIds.Contains(w.Id))).Items;

            var grpByItem = all.GroupBy(w => new { w.ItemId, w.ItemName });

            foreach (var item in grpByItem)
            {
                var itmDetail = itemsInfo.First(w => w.Id == item.Key.ItemId);
                var name = item.Key.ItemName;
                var moveQauntity = item.Sum(w => w.Inventory.TotalQuantity);
                var cost = itmDetail.Cost; //item.Select(s => s.AvragePiceCost).LastOrDefault();
                var price = itmDetail.Price;//item.Select(w => w.Price).LastOrDefault();

                totalQ += moveQauntity;
                totalCost += cost;
                totalPrice += price;

                DynamicApiRow.AppendRow([
                    name,
                            moveQauntity.ToDottedCommaedNumber(),
                            cost.ToDottedCommaedNumber(),
                            price.ToDottedCommaedNumber(),
                        ]);
            }
            DynamicApiRow.AppendRow([
                    "-----------",
                      totalQ.ToDottedCommaedNumber(),
                        totalCost.ToDottedCommaedNumber(),
                        totalPrice.ToDottedCommaedNumber(),
                        ]);
            return opt.Success(DynamicApiRow);
        });
    //
    // 9- sale item profit2
    [HttpPost("SalesItemsProfit2")]
    public ValueTask<HttpResponseWrapper<DynamicApiReports>> PostSalesItemsProfit2(
        [FromBody] ItemInfoQueryRequest request)
    {
        return TryOne<DynamicApiReports>(async opt =>
        {
            var report = new DynamicApiReports();

            var storageService = opt.GetService<IPurchaseAndSalesModuleService>();
            var salesService = opt.GetService<IServiceBase<DtoSalesInvoice, TSalesInvoice>>();

            report.AppendRow([
                "ر.فاتورة",
                "الصنف",
                "المخزن",
                "الوحدة",
                "الكمية",
                "اجمالي الكمية",
                "الصلاحيات",
                "التكلفة",
                "البيع",
                "الربحية",
                "اجمالي الربحية"
            ]);

            var results = await SalesProfitCalculator.CalculateAllItemsAsync(
                request.StoreId,
                request.AccountId.GetValueOrDefault(),
                request.UnitId,
                request.StartDate,
                request.EndDate,
                storageService,
                salesService);

            foreach (var row in results.FullPeriod.Rows)
                report.AppendRow(row);

            report.AppendRow([
                "-----------",
                "-----------",
                "-----------",
                "-----------",
                results.FullPeriod.Quantity.ToDottedCommaedNumber(),
                results.FullPeriod.OverallQuantity.ToDottedCommaedNumber(),
                "-----------",
                results.FullPeriod.Cost.ToDottedCommaedNumber(),
                results.FullPeriod.Sales.ToDottedCommaedNumber(),
                results.FullPeriod.Profit.ToDottedCommaedNumber(),
                results.FullPeriod.TotalProfit.ToDottedCommaedNumber()
            ]);

            report.CustomData.Add("TotalProfit", results.FullPeriod.TotalProfit.ToString());
            report.CustomData.Add("TotalSales", results.FullPeriod.SalesTimesQuantity.ToString());
            report.CustomData.Add("TotalPurchase", results.FullPeriod.CostTimesQuantity.ToString());

            for (var index = 0; index < results.Periods.Count; index++)
            {
                var period = results.Periods[index];
                report.CustomData.Add($"Q{index}", period.GetRepresentation());
            }

            return opt.Success(report);
        });
    }

//
    // مقبوضات مندوب
    [HttpPost("MarketerCredtions")]
    public ValueTask<HttpResponseWrapper<DynamicApiReports>> PostSalesItemsProfit2([FromBody] StatementQueryRequest request)
     =>
        TryOne<DynamicApiReports>(async opt =>
        {
            //
            //
            //  1) Prepare needed servcies
            //
            var DynamicApiRow = new DynamicApiReports();
            var entryService = opt.ServiceProvider.GetRequiredService<IEntryService>();
            var marketerService = opt.ServiceProvider.GetRequiredService<IServiceBase<DtoMarketer, TMarketer>>();
            var marketerClientsService = opt.ServiceProvider.GetRequiredService<IServiceBase<DtoMarketerClient, TMarketerClient>>();
            var clientService = opt.ServiceProvider.GetRequiredService<IServiceBase<DtoClient, TClient>>();
            var accountService = opt.ServiceProvider.GetRequiredService<IServiceBase<DtoAccount, TAccount>>();
            var entryDetailsService = opt.ServiceProvider.GetRequiredService<IServiceBase<DtoEntryDetail, TEntryDetail>>();
            ICollection<DynamicApiRow> rows = [];
            //
            //
            //  2) Prepare columns headers
            //
            List<Identifier> clientsAccointsIDs = [];
            if (request.AccountId is null)
            {
                return opt.Problem("");
            }
            var marketerId = request.AccountId.Value;
            var marketerInfo = await marketerService.GetOneAsAsync(s => new { s.Name, s.AccountId }, w => w.Id == marketerId);
            var clients = await marketerClientsService.GetAllAsAsync(s => s.ClientAccountId, w => w.MarketerId == marketerId);
            clientsAccointsIDs = clients.Items.ToList();
            DynamicApiRow.AddUpper("المندوب", marketerInfo.Name);


            DynamicApiRow.AppendRow([
                "اسم المندوب",
                "اسم الزبون",
                "الاجمالي"
                ]);


            //
            //
            //  3) Fetch the report raw data
            //
            var totalDebit = 0m;
            var totalCredit = 0m;

            var where = entryDetailsService.Build();
            where = where.And(w =>
                w.EntryOprationType == OprationTypeEnum.RepresentativePaymentReceipt &&
                clientsAccointsIDs.Contains(w.AccountId) &&
                //w.AccountId == marketerInfo.AccountId && 
                w.Credit > 0);

            if (request.StartDate != null && request.EndDate != null)
            {
                where = where.And(w => w.EntryDate >= request.StartDate && w.EntryDate <= request.EndDate);
            }
            var marketerCredids = await entryDetailsService.GetAllAsAsync(s => new { s.AccountName, s.Credit }, where);
            foreach (var item in marketerCredids.Items)
            {
                totalCredit += item.Credit;
                rows.Add(new DynamicApiRow(
                    [
                    marketerInfo.Name,
                    item.AccountName ?? "",
                        item.Credit.ToDottedCommaedNumber(),
                    ]
                ));
            }
            DynamicApiRow.AddUpper("الرصيد", (totalCredit).ToDottedCommaedNumber());
            //
            //
            //  4) Prepare rows data
            //
            //
            rows.Add(new DynamicApiRow(
                    [
                        "--------",
                        "--------",
                        (marketerCredids.Items.Sum(s=> s.Credit)).ToDottedCommaedNumber(),
                    ]
                ));
            DynamicApiRow.AppendRows(rows);
            //
            //
            //  5) return response
            //
            return opt.Success(DynamicApiRow);
        });
    //
    // حركة النقدية
    [HttpPost("FinancialHestory")]
    public ValueTask<HttpResponseWrapper<DynamicApiReports>> PostFinancialHestory([FromBody] StatementQueryRequest request)
     =>
        TryOne<DynamicApiReports>(async opt =>
        {
            var safeService = opt.GetService<DtoSafe, TSafe>();
            var entryDetailsService = opt.GetService<DtoEntryDetail, TEntryDetail>();

            var allowedTypes = new int[] {
                (int)OprationTypeEnum.ClientFinancial,
                (int)OprationTypeEnum.SupplierFinancial,
                (int)OprationTypeEnum.EmployeeFinancial,
                (int)OprationTypeEnum.ExpenseFinancial,
                (int)OprationTypeEnum.RepresentativePaymentReceipt,
                (int)OprationTypeEnum.MarketerFinancial,
            };

            var bannedAccountid = (await safeService.GetAllAsAsync(s => s.AccountId)).Items;

            var where = entryDetailsService.Build();
            where = where.And(w =>
                    allowedTypes.Contains((int)w.EntryOprationType)
                    && !bannedAccountid.Contains(w.AccountId));

            if (request?.StartDate != null && request?.EndDate != null)
            {
                where = where.And(w => w.EntryDate >= request.StartDate && w.EntryDate <= request.EndDate);
            }

            var movements = await entryDetailsService.GetAllAsAsync(s => new { s.EntryNumber, s.EntryDate, s.AccountName, s.Account.Number, s.Credit, s.Debit }, where);

            var DynamicApiRow = new DynamicApiReports();
            ICollection<DynamicApiRow> rows = [];
            DynamicApiRow.AppendRow([
               "رقم تسلسل",
                "التاريخ",
                "رقم الحساب",
                "اسم الحساب",
                "دائن",
                "مدين",
            ]);

            foreach (var item in movements.Items)
            {
                rows.Add(new DynamicApiRow(
                    [
                        item.EntryNumber.ToDottedCommaedNumber(),
                        item.EntryDate.ToDottedDate(),
                        item.AccountName,
                        item.Number.ToDottedCommaedNumber(),
                        item.Credit.ToDottedCommaedNumber(),
                        item.Debit.ToDottedCommaedNumber(),
                    ]
                ));
            }
            DynamicApiRow.AppendRows(rows);

            return opt.Success(DynamicApiRow);

        });
    //
    // حركة المصروفات
    [HttpPost("ExpencesHestory")]
    public ValueTask<HttpResponseWrapper<DynamicApiReports>> PostExpencesHestory([FromBody] StatementQueryRequest request)
     =>
        TryOne<DynamicApiReports>(async opt =>
        {
            var configService = opt.GetService<DtoConfig, TConfig>();
            var accountService = opt.GetService<DtoAccount, TAccount>();
            var entryDetailsService = opt.GetService<DtoEntryDetail, TEntryDetail>();

            var expencesAccountInfo = await configService.GetOneAsAsync(w => w.ExpenseAccount);
            if (expencesAccountInfo is null)
                opt.Problem("expencesAccount not ready"); // todo: localize
            var expencesAccount = Identifier.Parse(expencesAccountInfo);
            var underExpences = await accountService.GetAllAsAsync(s => s.Id, w => w.ParentId == expencesAccount);

            var where = entryDetailsService.Build();
            where = where.And(w => underExpences.Items.Contains(w.AccountId) && w.Credit == 0);

            if (request?.StartDate != null && request?.EndDate != null)
            {
                where = where.And(w => w.EntryDate >= request.StartDate && w.EntryDate <= request.EndDate);
            }

            var movements = await entryDetailsService.GetAllAsAsync(s => new { s.EntryNumber, s.EntryDate, s.AccountName, s.Account.Number, s.Credit, s.Debit, s.EntryNote }, where);

            var DynamicApiRow = new DynamicApiReports();
            ICollection<DynamicApiRow> rows = [];
            DynamicApiRow.AppendRow([
               "رقم تسلسل",
                "التاريخ",
                "اسم الحساب",
                "القيمة",
                "ملاحظات",
            ]);

            foreach (var item in movements.Items)
            {
                rows.Add(new DynamicApiRow(
                    [
                        item.EntryNumber.ToDottedCommaedNumber(),
                        item.EntryDate.ToDottedDate(),
                        item.AccountName,
                        item.Debit.ToDottedCommaedNumber(),
                        item.EntryNote,
                    ]
                ));
            }
            rows.Add(new DynamicApiRow(
                    [
                        "-----",
                        "-----",
                        "-----",
                        movements.Items.Sum(s=> s.Debit).ToDottedCommaedNumber(),
                        "-----",
                    ]
                ));
            DynamicApiRow.AppendRows(rows);

            return opt.Success(DynamicApiRow);

        });


    [NonAction]
    private static List<DtoStatment> CalculateBalance(IEnumerable<DtoStatment> transactions)
    {
        var transactionsWithBalance = new List<DtoStatment>();

        transactions = transactions.Reverse();

        // Will calculate according to date
        var groupedTransactions = transactions.GroupBy(t => t.AccountId);

        foreach (var group in groupedTransactions)
        {
            var balance = 0m;

            foreach (var transaction in group.OrderBy(b =>
                     {
                         if (DateTime.TryParse(b.EntryDateString, out var date))
                             return date;

                         return default;
                     }))
            {
                balance += transaction.Debit - transaction.Credit;
                transactionsWithBalance.Add(new DtoStatment
                {
                    Balance = balance,
                    Debit = transaction.Debit,
                    Credit = transaction.Credit,
                    AccountId = transaction.AccountId,
                    EntryDateString = transaction.EntryDateString,
                    OprationType = transaction.OprationType,
                    AccountName = transaction.AccountName,
                    DisplayNumber = transaction.DisplayNumber,
                    Number = transaction.Number,
                    Notes = transaction.Notes,
                    //Notes = "اجمالي الرصيد",
                });
            }
        }

        return transactionsWithBalance;
    }

    //  MOBILE NEEDS
}
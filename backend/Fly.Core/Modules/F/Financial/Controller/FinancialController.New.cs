using Fly.Core.Common.Reporting;
using Fly.Core.Modules.C.Accounting.Services;
using Fly.Core.Modules.F.Financial.Reports;
using Fly.Core.Modules.F.Marketers.Models.DbModels;
using Fly.Core.Modules.F.PurchaseAndSales.Models.DbModels;
using Fly.Core.Modules.F.PurchaseAndSales.Services.GlobalServices;
using Fly.Core.Persistence.Ef.Models.Global;
using Fly.Core.Persistence.Ef.Models.Store;
using Fly.Core.Persistence.Ef.Models;
using Fly.Framework.Server.Persistence;
using Fly.Framework.Shared.Common.Http;
using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.Accounting.Dtos;
using Fly.Shared.Modules.Accounting.Enums;
using Fly.Shared.Modules.Accounting.Requests;
using Fly.Shared.Modules.Financial.Dtos;
using Fly.Shared.Modules.Financial.Requests;
using Fly.Shared.Modules.Global.Dtos;
using Fly.Shared.Modules.Marketers.Dtos;
using Fly.Shared.Modules.Store.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Ardalis.Result.FluentValidation;
using Fly.Framework.Server.Common;
using Fly.Framework.Shared.Helpers.NumberToString;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Misc;
using Fly.Core.Modules.Global.Models.DbModels;
using Microsoft.Extensions.Logging;
using Fly.Shared.Modules.Personnel.Dtos;
using Fly.Core.Persistence.Ef.Models.Personnel;

namespace Fly.Core.Modules.F.Financial.Controller;

public partial class FinancialController
{
    private async Task<FileResponse> BuildReport<TReportModel>(ResultOrFailer<FileResponse> opt, DtoEntry entry, TReportModel request, string reportFileName, string dataSourceName)
           where TReportModel : RTModel
    {

        var companyInfo = _service.MakeChildService<DtoCompanyInfo, TCompanyInfo>();
        var company = await companyInfo.GetAllAsync();

        var financialReportService = opt.ServiceProvider.GetRequiredService<ISaveReport>();
        var financialReport = new FinancialReports(reportFileName);
        var fileName = "";
        var result = await financialReport.Generate(fillDatasource: async () =>
        {
            return new List<TReportModel>()
            {
                request
            };
        },
        fillParameters: async () =>
        {
            //return new List<Microsoft.Reporting.NETCore.ReportParameter> { 
            //    //new Microsoft.Reporting.NETCore.ReportParameter("Date", request.Date),
            //};
            return null;
        },
        action: async (by) =>
        {
            var __fileName = entry.Id.ToString() + ".pdf";
            fileName = await financialReportService.SaveBytes(by, __fileName);

        }, datasourceName: dataSourceName,
        dataSources: [company.Items],
        dataSourcesNames: ["LuncherInfo"]);
        result.FileName = fileName;
        return result;
    }



    [HttpPost("ClientPaymentReceipt")]
    public ValueTask<HttpResponseWrapper<FileResponse>> PostNewClientPaymentReceipt(ClientFinancialRequest request)
    =>
      TryOne<FileResponse>(async opt =>
      {
          var validationResult = await opt.Validate(request);
          if (!validationResult.IsValid) return opt.Problem(validationResult.AsErrors());

          var configService = _service.MakeChildService<DtoConfig, TConfig>();

          //configService.GetOneAsAsync(s => s.d)

          //    1 = receipt, 2 = pay

          DtoStatmentInfo? info = null;
          var they = request.Type == 1 ? request.FromAccount : request.ToAccount;
          var me = request.Type == 1 ? request.ToAccount : request.FromAccount;
          //
          var nextBookNumber = 0l;
          Dictionary<string, string> l = new Dictionary<string, string>()
          {
                {"payment-processing-failed", "payment-processing-failed" },
                {"client-not-found", "client-not-found" },
                {"safe-not-found", "safe-not-found" },
          };
          //
          var tr = _service.BeginTransaction();
          try
          {
              var clientServcie = _service.MakeChildService<DtoClient, TClient>();
              var clientInfo = await clientServcie.GetOneAsAsync(s => new { s.Name }, w => w.AccountId == they);
              if (clientInfo is null) return opt.Problem(l["client-not-found"]);
              var clientName = clientInfo.Name;
              //
              var safeServcie = _service.MakeChildService<DtoSafe, TSafe>();
              var safeInfo = await safeServcie.GetOneAsAsync(s => new { s.Name }, w => w.AccountId == me);
              if (safeInfo is null) return opt.Problem(l["safe-not-found"]);
              var safeName = safeInfo.Name;
              //
              var pp = Prepare();
              var userId = GetUserId();
              var userName = "";
              if (userId is not null)
              {
                  var userServcie = _service.MakeChildService<DtoUser, TUser>();
                  var userInfo = await userServcie.GetOneAsAsync(s => new { s.Name }, w => w.Id == userId);
                  if (safeInfo is null) return opt.Problem(l["user-not-found"]);
                  userName = safeInfo.Name ?? "";
              }
              //
              var service = (EntryService)_service;
              var entry = await service.AddSimpleEntry(request, transaction: tr, autoCommit: false);
              if (entry is null) return opt.Problem(l["payment-processing-failed"]);
              var balance = await service.GetBalance(they);


              var result = await BuildReport(opt, entry, new RTClientReceipt()
              {
                  Type = request.Type,
                  Amount = request.Amount,
                  ClientName = clientName,
                  SafeName = safeName,
                  Date = request.Date,
                  Notes = request.Note,
                  Remaining = balance,
                  Currency = "",
                  UserName = userName,
                  TextAmount = NumberToTextV6.GetText(request.Amount),
                  Number = entry.Number
              }, "ClientReceipt", "DataSet1");
              _service.CommitTransaction();
              return opt.Success(result);
          }
          catch (Exception ex)
          {
              Logger.LogError(ex, "Error-ClientPaymentReceipt");
              _service.RolebackTransaction();
              return opt.Problem("");
          }

      });

    [HttpPost("SupplierPaymentReceipt")]
    public ValueTask<HttpResponseWrapper<FileResponse>> PostNewSupplierPaymentReceipt(ClientFinancialRequest request)
    =>
      TryOne<FileResponse>(async opt =>
      {
          var validationResult = await opt.Validate(request);
          if (!validationResult.IsValid) return opt.Problem(validationResult.AsErrors());

          var configService = _service.MakeChildService<DtoConfig, TConfig>();

          //configService.GetOneAsAsync(s => s.d)

          //    1 = receipt, 2 = pay

          DtoStatmentInfo? info = null;
          var they = request.Type == 1 ? request.FromAccount : request.ToAccount;
          var me = request.Type == 1 ? request.ToAccount : request.FromAccount;
          //
          var nextBookNumber = 0l;
          Dictionary<string, string> l = new Dictionary<string, string>()
          {
                {"payment-processing-failed", "payment-processing-failed" },
                {"client-not-found", "client-not-found" },
                {"safe-not-found", "safe-not-found" },
          };
          //
          var tr = _service.BeginTransaction();
          try
          {
              var supplierServcie = _service.MakeChildService<DtoSupplier, TSupplier>();
              var supplierInfo = await supplierServcie.GetOneAsAsync(s => new { s.Name }, w => w.AccountId == they);
              if (supplierInfo is null) return opt.Problem(l["supplier-not-found"]);
              var supplierName = supplierInfo.Name;
              //
              var safeServcie = _service.MakeChildService<DtoSafe, TSafe>();
              var safeInfo = await safeServcie.GetOneAsAsync(s => new { s.Name }, w => w.AccountId == me);
              if (safeInfo is null) return opt.Problem(l["safe-not-found"]);
              var safeName = safeInfo.Name;
              //
              var pp = Prepare();
              var userId = GetUserId();
              var userName = "";
              if (userId is not null)
              {
                  var userServcie = _service.MakeChildService<DtoUser, TUser>();
                  var userInfo = await userServcie.GetOneAsAsync(s => new { s.Name }, w => w.Id == userId);
                  if (safeInfo is null) return opt.Problem(l["user-not-found"]);
                  userName = safeInfo.Name ?? "";
              }
              //
              var service = (EntryService)_service;
              var entry = await service.AddSimpleEntry(request, transaction: tr, autoCommit: false);
              if (entry is null) return opt.Problem(l["payment-processing-failed"]);
              var balance = await service.GetBalance(they);


              var result = await BuildReport(opt, entry, new RTSupplierReceipt()
              {
                  Type = request.Type,
                  Amount = request.Amount,
                  SupplierName = supplierName,
                  SafeName = safeName,
                  Date = request.Date,
                  Notes = request.Note,
                  Remaining = balance,
                  Currency = "",
                  UserName = userName,
                  TextAmount = NumberToTextV6.GetText(request.Amount),
                  Number = entry.Number
              }, "supplierReceipt", "DataSet1");
              _service.CommitTransaction();
              return opt.Success(result);
          }
          catch (Exception ex)
          {
              Logger.LogError(ex, "Error-supplierPaymentReceipt");
              _service.RolebackTransaction();
              return opt.Problem("");
          }

      });


    [HttpPost("EmployeePaymentReceipt")]
    public ValueTask<HttpResponseWrapper<FileResponse>> PostNewEmployeePaymentReceipt(EmployeeFinancialRequest request)
    =>
        TryOne<FileResponse>(async opt =>
        {
            var validationResult = await opt.Validate(request);
            if (!validationResult.IsValid) return opt.Problem(validationResult.AsErrors());

            var configService = _service.MakeChildService<DtoConfig, TConfig>();

            //configService.GetOneAsAsync(s => s.d)

            //    1 = receipt, 2 = pay

            DtoStatmentInfo? info = null;
            var they = request.Type == 1 ? request.FromAccount : request.ToAccount;
            var me = request.Type == 1 ? request.ToAccount : request.FromAccount;
            //
            var nextBookNumber = 0l;
            Dictionary<string, string> l = new Dictionary<string, string>()
          {
                {"payment-processing-failed", "payment-processing-failed" },
                {"employee-not-found", "employee-not-found" },
                {"safe-not-found", "safe-not-found" },
          };
            //
            var tr = _service.BeginTransaction();
            try
            {
                var employeeServcie = _service.MakeChildService<DtoEmployee, TEmployee>();
                var employeeInfo = await employeeServcie.GetOneAsAsync(s => new { s.Name }, w => w.AccountId == they);
                if (employeeInfo is null) return opt.Problem(l["employee-not-found"]);
                var employeeName = employeeInfo.Name;
                //
                var safeServcie = _service.MakeChildService<DtoSafe, TSafe>();
                var safeInfo = await safeServcie.GetOneAsAsync(s => new { s.Name }, w => w.AccountId == me);
                if (safeInfo is null) return opt.Problem(l["safe-not-found"]);
                var safeName = safeInfo.Name;
                //
                var pp = Prepare();
                var userId = GetUserId();
                var userName = "";
                if (userId is not null)
                {
                    var userServcie = _service.MakeChildService<DtoUser, TUser>();
                    var userInfo = await userServcie.GetOneAsAsync(s => new { s.Name }, w => w.Id == userId);
                    if (safeInfo is null) return opt.Problem(l["user-not-found"]);
                    userName = safeInfo.Name ?? "";
                }
                //
                var service = (EntryService)_service;
                var entry = await service.AddSimpleEntry(request, transaction: tr, autoCommit: false);
                if (entry is null) return opt.Problem(l["payment-processing-failed"]);
                var balance = await service.GetBalance(they);


                var result = await BuildReport(opt, entry, new RTEmployeeReceipt()
                {
                    Type = request.Type,
                    Amount = request.Amount,
                    EmployeeName = employeeName,
                    SafeName = safeName,
                    Date = request.Date,
                    Notes = request.Note,
                    Remaining = balance,
                    Currency = "",
                    UserName = userName,
                    TextAmount = NumberToTextV6.GetText(request.Amount),
                    Number = entry.Number
                }, "employeeReceipt", "DataSet1");
                _service.CommitTransaction();
                return opt.Success(result);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error-employeePaymentReceipt");
                _service.RolebackTransaction();
                return opt.Problem("");
            }

        });
    
    [HttpPost("DiscountItemPaymentReceipt")]
    public ValueTask<HttpResponseWrapper<FileResponse>> PostDiscountItemPaymentReceipt(DiscountItemBasedRequest request)
  =>
      TryOne<FileResponse>(async opt =>
      {
          var validationResult = await opt.Validate(request);
          if (!validationResult.IsValid) return opt.Problem(validationResult.AsErrors());

          DtoStatmentInfo? info = null;
          var they = request.Type == 1 ? request.FromAccount : request.ToAccount;
          //
          var nextBookNumber = 0l;
          Dictionary<string, string> l = new Dictionary<string, string>()
          {
                {"payment-processing-failed", "payment-processing-failed" },
                {"item-not-found", "item-not-found" },
                {"employee-not-found", "client-not-found" },
                {"config-discount-account-not-found", "config-discount-account-not-found" },
                {"discount-account-not-found", "discount-account-not-found" },
                {"unit-item-not-found", "unit-item-not-found" },
          };
          //
          var itemServcie = _service.MakeChildService<DtoItem, TItem>();
          var itemInfo = await itemServcie.GetOneAsAsync(s => new { s.Name }, w => w.Id == request.ItemId);
          if (itemInfo is null) return opt.Problem(l["item-not-found"]);
          var itemName = itemInfo.Name;
          //
          var clientServcie = _service.MakeChildService<DtoClient, TClient>();
          var clientInfo = await clientServcie.GetOneAsAsync(s => new { s.Name, s.AccountId, s.Id }, w => w.AccountId == request.ClientId);
          if (clientInfo is null) return opt.Problem(l["client-not-found"]);
          var clientName = clientInfo.Name;
          //
          var unitServcie = _service.MakeChildService<DtoUnit, TUnit>();
          var unitItemServcie = _service.MakeChildService<DtoItemUnit, TItemUnit>();
          var unitInfo = await unitServcie.GetOneAsAsync(s => new { s.Name, s.Id }, w => w.Id == request.UnitId);
          if (unitInfo is null) return opt.Problem(l["unit-not-found"]);
          var unitItemInfo = await unitItemServcie.GetOneAsAsync(s => new { s.UnitId, s.ItemId, s.IsPrimary, s.TransferRate }, w => w.ItemId == request.ItemId && w.UnitId == request.UnitId);
          if (unitItemInfo is null) return opt.Problem(l["unit-item-not-found"]);
          //
          var accountService = _service.MakeChildService<DtoAccount, TAccount>();
          var configService = _service.MakeChildService<DtoConfig, TConfig>();
          var discountAccountInfo = await configService.GetOneAsAsync(s => new { s.DiscountAccount });
          if (discountAccountInfo is null || discountAccountInfo.DiscountAccount is null) return opt.Problem(l["config-discount-account-not-found"]);
          var discountAccount = await accountService.GetOneAsAsync(s => new { s.Id }, w => w.Id == Identifier.Parse(discountAccountInfo.DiscountAccount));
          if (discountAccount is null) return opt.Problem(l["discount-account-not-found"]);
          //
          var discountitedItemService = _service.MakeChildService<IDiscountitedItemService>();
          var service = (EntryService)_service;
          //
          var entryRequest = RSimpleRequest
              .Create(request.SerialNumber, request.Amount, discountAccount.Id, clientInfo.AccountId, request.Date, request.Note, OprationTypeEnum.DiscountByItemFinancial);
          var entry = await service.AddSimpleEntry(entryRequest, action: async (tr, dtoentry) =>
          {
              nextBookNumber = await discountitedItemService.NextNumber(x => x.Number);
              var newBook = await discountitedItemService.Add(new DtoDiscountitedItem()
              {
                  ExpireDate = request.ExpireDate,
                  ClientId = clientInfo.Id,
                  Date = request.Date,
                  ItemId = request.ItemId,
                  Quantity = request.Quantity,
                  ReleatedId = dtoentry.Id,
                  Ammount = request.Amount,
                  Number = nextBookNumber,
                  UnitId = request.UnitId,
                  TotalQuantity = request.Quantity * unitItemInfo.TransferRate,
              });
          });
          if (entry is null) return opt.Problem(l["payment-processing-failed"]);
          //
          var financialReportService = opt.ServiceProvider.GetRequiredService<ISaveReport>();
          var financialReport = new FinancialReports("ReceiptItemDisount");
          var fileName = "";
          var result = await financialReport.Generate(fillDatasource: async () =>
          {
              return new List<RTDiscountItemReceipt>()
              {
                    new (entry.Date, request.Amount, request.Note, unitInfo.Name, entry.Number.ToString("D10"), request.Quantity, itemName, clientName, entry.Number)
              };
          },
          fillParameters: async () =>
          {
              //return new List<Microsoft.Reporting.NETCore.ReportParameter> { 
              //    //new Microsoft.Reporting.NETCore.ReportParameter("Date", request.Date),
              //};
              return null;
          },
          action: async (by) =>
          {
              var __fileName = entry.Id.ToString() + ".pdf";
              fileName = await financialReportService.SaveBytes(by, __fileName);

          });
          result.FileName = fileName;
          return opt.Success(result);
      });


    [HttpPost("RepresentativePaymentReceipt")]
    public ValueTask<HttpResponseWrapper<FileResponse>> RepresentativePaymentReceipt(RepresentativeFinancialRequest request)
    =>
        TryOne<FileResponse>(async opt =>
        {
            var validationResult = await opt.Validate(request);
            if (!validationResult.IsValid) return opt.Problem(validationResult.AsErrors());


            var entryService = _service.MakeChildService<IEntryService>();
            var safeService = _service.MakeChildService<IServiceBase<DtoSafe, TSafe>>();
            var accountingFinacialService = _service.MakeChildService<AccountingFinacial>();
            var configService = _service.MakeChildService<IServiceBase<DtoConfig, TConfig>>();
            var clientService = _service.MakeChildService<IServiceBase<DtoClient, TClient>>();
            var accountervice = _service.MakeChildService<IServiceBase<DtoAccount, TAccount>>();
            var discountitedItemService = _service.MakeChildService<IDiscountitedItemService>();
            var marketerService = _service.MakeChildService<IServiceBase<DtoMarketer, TMarketer>>();
            var marketerSharedEntryService = _service.MakeChildService<IServiceBase<DtoMarketerSharedEntry, TMarketerSharedEntry>>();

            var client = await clientService
                .GetOneAsAsync(x => new { x.Name, x.AccountId }, x => x.AccountId == request.ClientId);
            if (client is null) return opt.Problem("Client not found");
            var marketer = await marketerService
                .GetOneAsAsync(x => new { x.Name, x.AccountId, x.Id }, x => x.AccountId == request.MarketerId);
            if (marketer is null) return opt.Problem("Marketer not found");
            var safe = await safeService
                .GetOneAsAsync(x => new { x.Name, x.AccountId }, x => x.AccountId == request.SafeId);
            if (safe is null) return opt.Problem("safe not found");
            var generalGlobalShareAccountConfig = await configService.GetOneAsAsync(s => s.GeneralShareAccount);
            if (generalGlobalShareAccountConfig is null) return opt.Problem("generalGlobalShareAccount not found");
            var generalGlobalShareAccount = await accountervice
                .GetOneAsAsync(s => new { s.Id }, w => w.Id == Identifier.Parse(generalGlobalShareAccountConfig));
            if (generalGlobalShareAccount is null) return opt.Problem("generalGlobalShareAccount not found.");

            var sharedAmount = request.Amount * (request.MarketerShare / 100);

            _service.BeginTransaction();

            var entryRequest = await _service.AddFullEntry(new CreateEntryRequest
            {
                Details = new List<RcEntryDetail>
                {
                    new RcEntryDetail
                    {
                        Credit = request.Amount,
                        AccountId = client.AccountId,
                    },
                    new RcEntryDetail
                    {
                        Debit = request.Amount,
                        AccountId = safe.AccountId,

                    },

                    new RcEntryDetail
                    {
                        Credit = sharedAmount,
                        AccountId = marketer.AccountId,
                    },
                    new RcEntryDetail
                    {
                        Debit = sharedAmount,
                        AccountId = generalGlobalShareAccount.Id,
                    }
                },
                Note = request.Note,
                Date = request.Date,
                // todo: add relatedIndex
            }, OprationTypeEnum.RepresentativePaymentReceipt, relaredId: marketer.Id, relatedIndex: 15, action: async (tr, entry) =>
            {
                await marketerSharedEntryService.Add(new DtoMarketerSharedEntry
                {
                    EntryId = entry.Id,
                    MarketerId = marketer.Id,
                    SharedAmount = request.MarketerShare,
                });
            }, resetDate: false);

            var status = _service.CommitTransaction();
            if (!status) return opt.Problem("Cannot process payment");

            var clientBalance = await entryService.GetBalance(client.AccountId);
            var financialReportService = opt.ServiceProvider.GetRequiredService<ISaveReport>();
            var financialReport = new FinancialReports("Representative001");
            var fileName = "";
            var result = await financialReport.Generate(fillDatasource: async () =>
            {
                return new List<RTRepresentative>()
                {
                    new()
                    {
                        SafeName = safe.Name,
                        Amount = request.Amount,
                        ClientName = client.Name,
                        Date = entryRequest.Date,
                        MarketerName = marketer.Name,
                        Notes = request.Note,
                        ReceiptNumber = entryRequest.Number.ToString(),
                        ClientBalance = clientBalance,
                    }
                };
            },
            fillParameters: async () =>
            {

                return null;
            },
            action: async (by) =>
            {
                fileName = await financialReportService.SaveBytes(by);
            });
            
            result.FileName = fileName;
            return opt.Success(result);
        });
}

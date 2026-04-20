using Fly.Core.Modules.F.PurchaseAndSales.Models.DbModels;
using Fly.Core.Persistence.Ef.Models;
using Fly.Core.Persistence.Ef.Models.Global;
using Fly.Framework.Server.Common.Controllers.Base;
using Fly.Framework.Server.Common.Localization;
using Fly.Framework.Server.Persistence;
using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Http;
using Fly.Shared.Modules.Accounting.Dtos;
using Fly.Shared.Modules.Global.Dtos;
using Fly.Shared.Modules.PurchaseAndSales.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Text;
using Fly.Shared.Modules.Store.Dtos;
using Fly.Core.Persistence.Ef.Models.Store;
using Fly.Shared.Modules.Store.Enums;
using Fly.Core.Modules.F.PurchaseAndSales.Services.GlobalServices;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Misc;

namespace Fly.Core.Modules.Global.Controllers;

public class SeedingController(ILocalizationService localizationService, ILogger<SeedingController> logger, IMediator mediator, AutoMapper.IMapper mapper, MapsterMapper.IMapper mapster)
    : CustomControllerBase(localizationService, logger, mediator, mapper, mapster)
{

    public class FileSeedUploadRequest
    {
        public IFormFile File { get; set; }
        public bool SaveDublicatedNames { get; set; }
    }

    public class FileSeedUploadResponse
    {
        public int SuccessSaved { get; set; }
        public int DublicatedIgnored { get; set; }
    }

    private async Task<long> GetNumber(IServiceBase<DtoAccount, TAccount> accountService, DtoAccount baseAccount)
    {
        var nextNumber = await accountService.GetMaxAsync(c => c.Number, w => w.Number.ToString().StartsWith(baseAccount.Number.ToString()));

        if (nextNumber == baseAccount.Number)
        {
            var n = 1.ToString("D4");
            nextNumber = long.Parse($"{nextNumber}{n}");
        }
        else
        {
            var las = nextNumber.ToString().Substring(baseAccount.Number.ToString().Length);
            var a = (long.Parse(las) + 1).ToString("D4");
            nextNumber = long.Parse($"{baseAccount.Number}{a}");
        }
        return nextNumber;
    }


    [HttpPost("Clients")]
    public ValueTask<HttpResponseWrapper<FileSeedUploadResponse>> SeedClients(FileSeedUploadRequest request)
    =>
        TryOne<FileSeedUploadResponse>(async opt =>
        {

            if (request.File.FileName != "clients.csv")
                return opt.Problem("is not `clients.csv` file");

            var clientService = opt.GetService<DtoClient, TClient>();
            var accountService = opt.GetService<DtoAccount, TAccount>();
            var configervice = opt.GetService<DtoConfig, TConfig>();

            var tr = clientService.BeginTransaction(isolationLevel: IsolationLevel.RepeatableRead);
            var clientAccount = await configervice.GetOneAsAsync(w => w.ClientsAccount);
            var clientAccountId = Identifier.Parse(clientAccount);
            var clientAccountInfo = await accountService.GetById(clientAccountId);

            var str = new StreamReader(request.File.OpenReadStream(), Encoding.UTF8);
            int c = 0, d = 0;
            var clientsData = new List<DtoClient>();
            var accountsData = new List<DtoAccount>();
            str.ReadLine();
            while (!str.EndOfStream)
            {
                var line = str.ReadLine();
                var parts = line.Split(';');

                var phoene = parts[0];
                decimal.TryParse(parts[1], out var creditor);
                decimal.TryParse(parts[2], out var deposit);
                var name = parts[3];
                var number = parts[4];
                var accNumber = await GetNumber(accountService, clientAccountInfo);

                var exists = await accountService.AnyAsync(w => w.Name == name);
                if (!request.SaveDublicatedNames & exists)
                {
                    d++;
                    continue;
                }

                DtoAccount dtoAccount = new()
                {
                    Name = name,
                    Natural = clientAccountInfo.Natural,
                    Number = accNumber,
                    AccountCat = clientAccountInfo.AccountCat,
                    CurrencyId = clientAccountInfo.CurrencyId,
                    ParentId = clientAccountId,
                    OpeningCreditor = creditor,
                    OpeningDebtor = deposit,
                };
                await accountService.Add(dtoAccount);
                accountsData.Add(dtoAccount);

                DtoClient client = new()
                {
                    AccountId = dtoAccount.Id,
                    Name = name,
                    Phone = phoene,
                    Email = "",
                    MaxDebt = 0,
                };
                await clientService.Add(client);
                clientsData.Add(client);
                c++;
            }

            tr.Commit();

            return opt.Success(new FileSeedUploadResponse()
            {
                DublicatedIgnored = d,
                SuccessSaved = c,
            });
        });


    [HttpPost("Suppliers")]
    public ValueTask<HttpResponseWrapper<FileSeedUploadResponse>> SeedSuppliers(FileSeedUploadRequest request)
    =>
        TryOne<FileSeedUploadResponse>(async opt =>
        {

            if (request.File.FileName != "suppliers.csv")
                return opt.Problem("is not `suppliers.csv` file");

            var supplierService = opt.GetService<DtoSupplier, TSupplier>();
            var accountService = opt.GetService<DtoAccount, TAccount>();
            var configervice = opt.GetService<DtoConfig, TConfig>();

            var tr = supplierService.BeginTransaction(isolationLevel: IsolationLevel.RepeatableRead);
            var supplierAccount = await configervice.GetOneAsAsync(w => w.SupplyersAccount);
            var supplierAccountId = Identifier.Parse(supplierAccount);
            var supplierAccountInfo = await accountService.GetById(supplierAccountId);

            var str = new StreamReader(request.File.OpenReadStream(), Encoding.UTF8);
            int c = 0, d = 0;
            var suppliersData = new List<DtoSupplier>();
            var accountsData = new List<DtoAccount>();
            str.ReadLine();
            while (!str.EndOfStream)
            {
                var line = str.ReadLine();
                var parts = line.Split(';');

                var phoene = parts[0];
                decimal.TryParse(parts[1], out var creditor);
                decimal.TryParse(parts[2], out var deposit);
                var name = parts[3];
                var number = parts[4];
                var accNumber = await GetNumber(accountService, supplierAccountInfo);

                var exists = await accountService.AnyAsync(w => w.Name == name);
                if (!request.SaveDublicatedNames & exists)
                {
                    d++;
                    continue;
                }

                DtoAccount dtoAccount = new()
                {
                    Name = name,
                    Natural = supplierAccountInfo.Natural,
                    Number = accNumber,
                    AccountCat = supplierAccountInfo.AccountCat,
                    CurrencyId = supplierAccountInfo.CurrencyId,
                    ParentId = supplierAccountInfo.Id,
                    OpeningCreditor = creditor,
                    OpeningDebtor = deposit,
                };
                await accountService.Add(dtoAccount);
                accountsData.Add(dtoAccount);

                DtoSupplier supplier = new()
                {
                    AccountId = dtoAccount.Id,
                    Name = name,
                    Phone = phoene,
                    Email = "",
                };
                await supplierService.Add(supplier);
                suppliersData.Add(supplier);
                c++;
            }

            tr.Commit();

            return opt.Success(new FileSeedUploadResponse()
            {
                DublicatedIgnored = d,
                SuccessSaved = c,
            });
        });



    [HttpPost("items")]
    public ValueTask<HttpResponseWrapper<FileSeedUploadResponse>> SeedItems(FileSeedUploadRequest request)
    =>
        TryOne<FileSeedUploadResponse>(async opt =>
        {

            if (request.File.FileName != "items.csv")
                return opt.Problem("is not `items.csv` file");

            var itemUnitService = opt.GetService<DtoItemUnit, TItemUnit>();
            var unitService = opt.GetService<DtoUnit, TUnit>();
            var itemService = opt.GetService<DtoItem, TItem>();
            var configervice = opt.GetService<DtoConfig, TConfig>();
            var categoryService = opt.GetService<DtoCategory, TCategory>();

            var tr = itemService.BeginTransaction();

            var str = new StreamReader(request.File.OpenReadStream(), Encoding.UTF8);
            int c = 0, d = 0;

            var unitsData = new List<DtoUnit>();
            var itemsData = new List<DtoItem>();
            var catData = new List<DtoCategory>();

            str.ReadLine();
            while (!str.EndOfStream)
            {
                var line = str.ReadLine();
                var parts = line.Split(';');

                //  التصنيف;التصنيف الأساسي;الشركة;التكلفة;الجملة;القطاعي;الوحدة;الكمية;العبوة;رقم القطعة;الاسم;الرقم

                var cat = parts[1];
                var company = parts[2];
                decimal.TryParse(parts[3], out var cost);
                decimal.TryParse(parts[4], out var sellMeny);
                decimal.TryParse(parts[5], out var sellLess);
                var unit = parts[6];
                decimal.TryParse(parts[7], out var qauntity);
                var globalSerial = parts[9];
                var name = parts[10];
                var number = parts[11];

                var dbCat = await categoryService.GetAsync(w => w.Name == cat);
                if (dbCat is null)
                {
                    dbCat = new DtoCategory()
                    {
                        Name = cat,
                        Description = cat,
                        CateguryId = null,
                    };
                    await categoryService.Add(dbCat);
                }
                var dbUnit = await unitService.GetAsync(w => w.Name == unit);
                if (dbUnit is null)
                {
                    dbUnit = new DtoUnit()
                    {
                        Name = unit,
                        Description = unit,
                    };
                    await unitService.Add(dbUnit);
                }


                var dbItem = await itemService.GetAsync(w => w.Name == name);
                if (dbItem is null || request.SaveDublicatedNames)
                {
                    DtoItem item = new()
                    {
                        Name = name,
                        Barcode = "",
                        CategoryId = dbCat.Id,
                        Price = sellLess,
                        Cost = cost,
                        ExpireDate = null,
                    };
                    await itemService.Add(item);

                    await itemUnitService.Add(new DtoItemUnit()
                    {
                        TransferRate = 1,
                        ItemId = item.Id,
                        IsPrimary = true,
                        UnitId = dbUnit.Id,
                    });
                    item.Price = qauntity;
                    item.CategoryId = dbUnit.Id;
                    itemsData.Add(item);
                    c++;
                }
            }

            {

                var storageServcie = opt.GetService<DtoStore, TStore>();
                var storageModuleService = opt.GetService<IPurchaseAndSalesModuleService>();
                var quantityCreditingInvoiceService = opt.GetService<DtoQuantityCreditingInvoice, TQuantityCreditingInvoice>();

                async Task AddDetails(DtoQuantityCreditingInvoice invoice)
                {
                    var itemsUnitsO = invoice.Details.Select(w => w.ItemId).ToArray();
                    var unitInfo = await itemUnitService
                        .GetAllAsAsync(
                            s => new { s.ItemId, s.UnitId, s.IsPrimary, s.TransferRate },
                            s => itemsUnitsO.Contains(s.ItemId)
                        );

                    foreach (var item in invoice.Details)
                    {

                        var currentunit = unitInfo.Items.FirstOrDefault(w => w.UnitId == item.UnitId);
                        var myPrimary = currentunit; // unitInfo.Items.FirstOrDefault(w => w.IsPrimary);
                        // if (myPrimary is null) throw new Exception("PrimaryUnitNotFound"); // Primary unit not found


                        var info = await storageModuleService.GetItemQuntity(item.ItemId, myPrimary.UnitId, invoice.StoreId.Value);
                        var all = info.Inventory.TotalQuantity;

                        if (all > 0)
                            all *= -1;

                        if (all != 0)
                        {
                            await storageModuleService
                            .LoseItem(item.ItemId, item.UnitId, invoice.StoreId.Value
                            , all
                            , all
                            , 0
                            , relatedId: invoice.Id, relatedInventory: RelatedInventory.QuantityCrediting_Zero);
                        }
                        //  
                        //  
                        await storageModuleService
                            .RegainItem(item.ItemId, item.UnitId, invoice.StoreId.Value
                            , item.Quantity
                            , currentunit.TransferRate * item.Quantity
                            , cost: 0, price: item.Price, avrageCost: item.Price + 0, avragePiceCost: item.Price
                            , relatedId: invoice.Id, relatedInventory: RelatedInventory.QuantityCrediting);
                    }
                }


                var allStores = await storageServcie.GetAllAsync();
                DtoStore? dtoStore = null;
                if (!allStores.Items.Any())
                {
                    dtoStore = new DtoStore()
                    {
                        Name = "الرئيسي",
                    };
                    await storageServcie.Add(dtoStore);
                }
                else
                {
                    dtoStore = allStores.Items.Last();
                }

                DtoQuantityCreditingInvoice invoice = new()
                {
                    Number = 1,
                    Date = DateTime.Now,
                    Details = itemsData.Select(s => new DtoQuantityCreditingInvoiceDetail()
                    {
                        ItemId = s.Id,
                        Quantity = s.Price, // it's a quantity
                        UnitId = s.CategoryId, // it's an unitId
                        Price = s.Cost,
                    }).ToList(),
                    StorageType = Shared.Modules.PurchaseAndSales.Enums.StorageType.Normal,
                    StoreId = dtoStore.Id,
                };
                await quantityCreditingInvoiceService.Add(invoice);
                await AddDetails(invoice);
            }

            tr.Commit();

            return opt.Success(new FileSeedUploadResponse()
            {
                DublicatedIgnored = d,
                SuccessSaved = c,
            });
        });

}

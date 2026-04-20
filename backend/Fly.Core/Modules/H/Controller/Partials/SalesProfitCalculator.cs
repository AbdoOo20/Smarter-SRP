using Fly.Core.Modules.F.PurchaseAndSales.Models.DbModels;
using Fly.Core.Modules.F.PurchaseAndSales.Services.GlobalServices;
using Fly.Framework.Server.Persistence;
using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Extentions;
using Fly.Shared.Modules.PurchaseAndSales.Dtos;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Sell;

namespace Fly.Core.Modules.H.Controller;

public static class SalesProfitCalculator
{
    public static async Task<ItemCalculationResult?> CalculateSingleItemAsync(
        DtoInventoryCost movement,
        IServiceBase<DtoSalesInvoice, TSalesInvoice> salesService)
    {
        var invoice = await salesService.GetOneAsAsync(s => new { s.Number }, w => w.Id == movement.Inventory.RelatedId);
        if (invoice == null) return null;

        var cost = movement.Cost;
        var sales = movement.Price ?? 0;
        var profit = sales - cost;
        var totalProfit = profit * movement.Inventory.Quantity;
        var salesTimesQuantity = sales * movement.Inventory.Quantity;
        var costTimesQuantity = cost * movement.Inventory.Quantity;

        return new ItemCalculationResult
        {
            Row =
            [
                invoice.Number.ToString(),
                movement.ItemName,
                movement.StoreName,
                movement.UnitName,
                movement.Inventory.Quantity.ToDottedCommaedNumber(),
                movement.Inventory.TotalQuantity.ToDottedCommaedNumber(),
                movement.ExpireDate?.ToDottedDate() ?? "بدون",
                cost.ToDottedCommaedNumber(),
                sales.ToDottedCommaedNumber(),
                profit.ToDottedCommaedNumber(),
                totalProfit.ToDottedCommaedNumber()
            ],
            Quantity = movement.Inventory.Quantity,
            OverallQuantity = movement.Inventory.TotalQuantity,
            Cost = cost,
            Sales = sales,
            Profit = profit,
            TotalProfit = totalProfit,
            SalesTimesQuantity = salesTimesQuantity,
            CostTimesQuantity = costTimesQuantity
        };
    }

    public static async Task<PeriodSplitResult> CalculateAllItemsAsync(
        Identifier storeId,
        Identifier accountId,
        Identifier unitId,
        DateTime startDate,
        DateTime endDate,
        IPurchaseAndSalesModuleService storageService,
        IServiceBase<DtoSalesInvoice, TSalesInvoice> salesService)
    {
        var splitResult = new PeriodSplitResult();

        var movements = await storageService.GetDetails(storeId, accountId, unitId, startDate, endDate);
        var salesMovements = movements.Where(m => !m.Inventory.Ingress).ToArray();

        //splitResult.AllTimeBeforePeriod.EndDate = startDate;

        foreach (var movement in salesMovements.OrderBy(s => s.CreateDate))
        {
            var itemResult = await CalculateSingleItemAsync(movement, salesService);
            if (itemResult == null) continue;
            var date = movement.Inventory.CreateDate;
            
            AddToResult(splitResult.FullPeriod, itemResult);

            var period = new PeriodCalculationResult
            {
                EndDate = date
            };
            AddToResult(period, itemResult);
            splitResult.Periods.Add(period);
        }

        return splitResult;
    }

    private static void AddToResult(PeriodCalculationResult result, ItemCalculationResult item)
    {
        result.Rows.Add(item.Row);
        result.Quantity += item.Quantity;
        result.OverallQuantity += item.OverallQuantity;
        result.Cost += item.Cost;
        result.Sales += item.Sales;
        result.Profit += item.Profit;
        result.TotalProfit += item.TotalProfit;
        result.SalesTimesQuantity += item.SalesTimesQuantity;
        result.CostTimesQuantity += item.CostTimesQuantity;
    }
}

// older:

    /*[HttpPost("SalesItemsProfit2")]
    public ValueTask<HttpResponseWrapper<DynamicApiReports>> PostSalesItemsProfit2([FromBody] ItemInfoQueryRequest request)
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
            var saleInvoiceService = opt.GetService<DtoSalesInvoice, TSalesInvoice>();

            ICollection<DynamicApiRow> rows = [];
            //
            //
            //  2) Prepare columns headers
            //
            DynamicApiRow.AppendRow([
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
                    "اجمالي الربحية",
                ]);

            //
            //
            //  3) Fetch the report raw data
            //

            var allMovemnt = await storageModuleService.GetDetails(request.StoreId, request.AccountId, request.UnitId, request.StartDate, request.EndDate);
            var all = allMovemnt.Where(w => !w.Inventory.Ingress).ToArray();

            decimal _x___q =0,_x___tq =0, _x____cost = 0, _x____sell = 0, _x____profit = 0, x_____totalProfit = 0, costTimesQuantityTotal = 0, saleTimesQuantityTotal = 0;

            foreach (var item in all)
            {
                var itemPrice = item.Price ?? 0m;
                var itemCost = item.Cost;
                var expireDate = (item.ExpireDate?.ToDottedDate()) ?? "بدون";
                var invoiceInfo = await saleInvoiceService.GetOneAsAsync(s => new { s.Number }, w => w.Id == item.Inventory.RelatedId);
                if (invoiceInfo is null) continue;

                var ____cost = item.Cost;
                var ____sell = item.Price;
                var salesTimeQ = item.Price * item.Inventory.Quantity;
                var purchaseTimesQuantity = item.Cost * item.Inventory.Quantity;
                
                var ____profit = ____sell - ____cost;
                var ____totalProfit = ____profit * item.Inventory.Quantity;

                _x___q += item.Inventory.Quantity;
                _x___tq += item.Inventory.TotalQuantity;
                _x____cost += ____cost;
                _x____sell += ____sell ?? 0;
                _x____profit += ____profit ?? 0;
                x_____totalProfit += ____totalProfit ?? 0;
                saleTimesQuantityTotal += salesTimeQ ?? 0;
                costTimesQuantityTotal += purchaseTimesQuantity;
                
                //var itemInfo = await itemService.GetOneAsAsync(s => new { s.Cost }, w => w.Id == item.ItemId);
                var maxCost = item.Cost * item.Inventory.TotalQuantity;

                DynamicApiRow.AppendRow([
                        invoiceInfo.Number.ToString(),
                        item.ItemName,
                        item.StoreName,
                        item.UnitName,
                        item.Inventory.Quantity.ToDottedCommaedNumber(),
                        item.Inventory.TotalQuantity.ToDottedCommaedNumber(),
                        expireDate,
                        ____cost.ToDottedCommaedNumber(),
                        ____sell.ToDottedCommaedNumber(),
                        (____profit).ToDottedCommaedNumber(),
                        (____totalProfit).ToDottedCommaedNumber(),
                    ]);

            }
            DynamicApiRow.AppendRow([
                        "-----------",
                        "-----------",
                        "-----------",
                        "-----------",
                        _x___q.ToDottedCommaedNumber(),
                        _x___tq.ToDottedCommaedNumber(),
                        "-----------",
                        _x____cost.ToDottedCommaedNumber(),
                        _x____sell.ToDottedCommaedNumber(),
                        (_x____profit).ToDottedCommaedNumber(),
                        (x_____totalProfit).ToDottedCommaedNumber(),
                    ]);
            
            DynamicApiRow.CustomData.Add("TotalProfit", x_____totalProfit.ToString());
            DynamicApiRow.CustomData.Add("TotalSales", saleTimesQuantityTotal.ToString());
            DynamicApiRow.CustomData.Add("TotalPurchase", costTimesQuantityTotal.ToString());
            
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
        });*/
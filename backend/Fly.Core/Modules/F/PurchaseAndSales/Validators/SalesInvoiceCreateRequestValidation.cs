using FluentValidation;
using Fly.Shared.Modules.PurchaseAndSales.Enums;
using Fly.Core.Modules.C.Accounting.Services;
using Fly.Core.Modules.F.PurchaseAndSales.Models.DbModels;
using Fly.Framework.Server.Persistence;
using Fly.Shared.Modules.PurchaseAndSales.Requests;
using Fly.Shared.Modules.Store.Dtos;
using Fly.Core.Persistence.Ef.Models.Store;
using Fly.Framework.Server.Exceptions.Controllers;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Misc;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Sell;

namespace Fly.Core.Modules.F.PurchaseAndSales.Validators;

public class SalesInvoiceCreateRequestValidation : AbstractValidator<SalesInvoiceCreateRequest>
{
    public SalesInvoiceCreateRequestValidation(
        IEntryService entryService
        , IServiceBase<DtoItem, TItem> itemService
        , IServiceBase<DtoClient, TClient> clientService
        , IServiceBase<DtoSalesInvoice, TSalesInvoice> service
        , IServiceBase<DtoSellReturnInvoice, TSellReturnInvoice> sellSReturnInvoiceeervice
    )
    {

        RuleFor(prp => prp.Id)
            //.NotEmpty()
            //.NotNull()
            .CustomAsync(async (id, cntxt, cans) =>
            {
                var isNew = id.IsNullOrEmpty();
                var xx = cntxt.InstanceToValidate;
                //if (xx.ClientId.IsNullOrEmpty())
                //{
                //    cntxt.AddFailure("رجاءً قم بتحديد المورد"); // Localized
                //}
                if (!xx.Details.Any())
                {
                    cntxt.AddFailure("يرجى أضافة الأصناف المباعة."); // Localized
                }
                
                if (xx.Discount < 0 || xx.Details.Any(w => w.Discount < 0))
                {
                    cntxt.AddFailure("قيمة التخفيض غير صحيحة"); // Localized
                }

                if (xx.Details.Any(w => w.UnitId.IsNullOrEmpty()))
                {
                    cntxt.AddFailure("يبدو أن أحد الأصناف التي تم اختيارها لم يتم تحديد الوحدة الخاصة به."); // Localized
                }

                if (xx.Details.Sum(s => s.Quantity * s.Price) < 0)
                {
                    cntxt.AddFailure("لا يمكن اتمام الشراء لإجمالي الفاتورة الخاصة بك اقل من 0"); // Localized
                }

                if ((!xx.StoreId.HasValue || xx.StoreId.Value.IsNullOrEmpty()) && xx.StorageType == StorageType.Normal)
                {
                    cntxt.AddFailure("رجاءً قم بتحديد المخزن"); // Localized
                }

                if ((!xx.MarketerStorageId.HasValue || xx.MarketerStorageId.Value.IsNullOrEmpty()) && xx.StorageType == StorageType.Marketer)
                {
                    cntxt.AddFailure("رجاءً قم بتحديد المخزن"); // Localized
                }

                if (xx.PeymentType == PeymentType.Cash && (!xx.SafeId.HasValue || xx.SafeId.Value.IsNullOrEmpty()))
                {
                    cntxt.AddFailure("يجب تحديد الخزينة عند اختيار نوع الدفع - نقدي"); // Localized
                }

                if (xx.PeymentType == PeymentType.Deferred && (!xx.ClientId.HasValue || xx.ClientId.Value.IsNullOrEmpty()))
                {
                    cntxt.AddFailure("يجب تحديد الزبون عند اختيار نوع الدفع - آجل"); // Localized
                }

                var totalNonDiscounted = xx.CountingPrince(-100);
                var totalDetails = xx.CountingPrince((int)xx.DiscountType);
                var totalxx = xx.DiscountType == (int)Shared.Modules.PurchaseAndSales.Enums.DiscountType.Percent ? totalDetails - (totalDetails * xx.Discount / 100) : totalDetails - xx.Discount;
                if (totalxx < 0)
                {
                    throw new InvalidInputException("الاجمالي يجب أن يكون أكبر من صفر");
                }


                var itemsIds = xx.Details.Select(W => W.ItemId).ToList();
                var itemInfo = await itemService.GetAllAsAsync(s => new { s.Id, s.Name, s.Cost, s.Price }, w => itemsIds.Contains(w.Id));


                //xx.Details.ForEach(f =>
                //{
                //    var selectedItem = itemInfo.Items.First(w => w.Id == f.ItemId);
                //    if (selectedItem.Cost > f.Price)
                //    {
                //        cntxt.AddFailure(string.Format("لا يمكن بيع الصنف [{0}] بأقل من سعر التكلفة", selectedItem.Name)); // Localized
                //    }
                //});

                if (!xx.Id.IsNullOrEmpty())
                {
                    var isAnyInvoice = await sellSReturnInvoiceeervice.AnyAsync(x => x.InvoiceId == xx.Id);
                    if (isAnyInvoice)
                    {
                        cntxt.AddFailure("لا يمكن تعديل فاتورة لها مراجعات");
                    }
                }

                if (xx.PeymentType == PeymentType.Deferred && xx.ClientId.HasValue && !xx.ClientId.Value.IsNullOrEmpty())
                {
                    var s = await clientService.GetOneAsAsync(s => new { s.AccountId, s.MaxDebt }, w => w.Id == xx.ClientId);
                    if (s.MaxDebt > 0)
                    {
                        var clientBalance = await entryService.GetBalance(s.AccountId);
                        if (totalxx + clientBalance > s.MaxDebt)
                        {
                            cntxt.AddFailure("لقد وصل العميل إلى سقف الدين");
                        }
                    }
                }

            });
    }
}
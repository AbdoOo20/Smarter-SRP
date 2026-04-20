using FluentValidation;
using Fly.Core.Modules.C.Accounting.Services;
using Fly.Core.Modules.F.Maintenance.MaintenanceInvoices.DbModels;
using Fly.Core.Modules.F.PurchaseAndSales.Models.DbModels;
using Fly.Core.Persistence.Ef.Models.Store;
using Fly.Framework.Server.Persistence;
using Fly.Shared.Modules.Maintenance.Dtos;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Misc;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Sell;
using Fly.Shared.Modules.Store.Dtos;
using Fly.Shared.Modules.Store.Requests;

namespace Fly.Core.Modules.F.ItemsTransferInvoice.Validations;

public class ItemsTransferInvoiceCreateRequestValidation : AbstractValidator<ItemsTransferInvoiceCreateUpdateRequest>
{
    public ItemsTransferInvoiceCreateRequestValidation(
        IEntryService entryService,
        IServiceBase<DtoItem, TItem> itemService,
        IServiceBase<DtoClient, TClient> clientService,
        IServiceBase<DtoMaintenanceInvoice, TMaintenanceInvoice> service,
        IServiceBase<DtoSellReturnInvoice, TSellReturnInvoice> sellSReturnInvoiceeervice
    )
    {
        RuleFor(prp => prp)
            .CustomAsync(async (id, cntxt, cans) =>
            {
                var xx = cntxt.InstanceToValidate;
                //if (xx.ClientId.IsNullOrEmpty())
                //{
                //    cntxt.AddFailure("رجاءً قم بتحديد المورد"); // Localized
                //}
                if (!xx.Details.Any())
                {
                    cntxt.AddFailure("يرجى أختيار ألاصناف التي تريد نقلها."); // Localized
                }

                if (xx.FromStorageId.IsNullOrEmpty())
                {
                    cntxt.AddFailure("يرجى أختيار المخزن الذي تريد النقل منه."); // Localized
                }
                
                if (xx.ToStorageId.IsNullOrEmpty())
                {
                    cntxt.AddFailure("يرجى أختيار المخزن الذي تريد النقل أليه."); // Localized
                }
                
                if (xx.FromStorageId.Equals(xx.ToStorageId))
                {
                    cntxt.AddFailure("لا يمكن للمخزن الأول أن يماثل المخزن الثاني."); // Localized
                }
            });
    }
}
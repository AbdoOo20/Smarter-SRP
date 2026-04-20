using FluentValidation;
using Fly.Shared.Modules.PurchaseAndQuantityCrediting.Requests;
using Fly.Framework.Server.Common.Localization;
using Fly.Framework.Server.Persistence;
using Fly.Shared.Modules.Store.Dtos;
using Fly.Core.Persistence.Ef.Models.Store;
using LinqKit;

namespace Fly.Core.Modules.F.PurchaseAndSales.Validators;

public class QuantityCreditingInvoiceCreateRequestValidation : AbstractValidator<QuantityCreditingInvoiceCreateRequest>
{
    public QuantityCreditingInvoiceCreateRequestValidation(
        ILocalizationService l
        , IServiceBase<DtoUnit, TUnit> unitReposotpry
        , IServiceBase<DtoItem, TItem> itemReposotpry)
    {

        RuleFor(prp => prp.StoreId)
            .NotNull().WithMessage("StoreId")
            .NotEmpty().WithMessage("StoreId");

        RuleFor(prp => prp.Date)
            .NotNull().WithMessage("Date")
            .NotEmpty().WithMessage("Date");

        RuleFor(prp => prp.Id)
            .CustomAsync(async (id, cntxm, cancellationToken) =>
            {
                var x = cntxm.InstanceToValidate;
                var ids = x.Details.Select(w => w.UnitId).ToArray();
                //var q = unitReposotpry.GetAll(w => ids.Contains(w.Id));
                //var a = await unitReposotpry.QueryList(q);
                x.Details.ForEach(async f =>
                {
                    var exists = await unitReposotpry.AnyAsync(w => w.Id == f.UnitId);
                    if (!exists)
                    {
                        var itemName = await itemReposotpry.GetOneAsAsync(s => s.Name, w => w.Id == f.ItemId);
                        cntxm.AddFailure($"لم يتم ايجاد الوحدة المخصصة للصنف {itemName}");
                    }
                });
            });
    }
}
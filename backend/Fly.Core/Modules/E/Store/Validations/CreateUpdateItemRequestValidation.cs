using FluentValidation;
using Fly.Core.Persistence.Ef.Models.Store;
using Fly.Framework.Server.Persistence;
using Fly.Shared.Modules.Store.Dtos;
using Fly.Shared.Modules.Store.Requests;
using LinqKit;

namespace Fly.Core.Modules.E.Store.Validations;

public class CreateUpdateItemRequestValidation : AbstractValidator<CreateUpdateItemRequest>
{
    public CreateUpdateItemRequestValidation(IServiceBase<DtoItem, TItem> serviceBase)
    {

        RuleFor(r => r.CategoryId)
            .NotEmpty().WithMessage("لا يمكن ترك الفئة فارغة") // localize
            ;
        RuleFor(r => r.Name)
            .NotEmpty().WithMessage("لا يمكن ترك اسم الصنف فارغا") // localize
            ;

        RuleFor(w => w.Id)
            .Custom((id, xntx) =>
            {
                var itms = xntx.InstanceToValidate.ItemUnits;
                if (!itms.Any())
                {
                    xntx.AddFailure("رجاءً قم بتحديد وحدات الصنف");
                }
            });

        RuleFor(r => r.Barcode)
            .CustomAsync(async (barcode, cnxt, cancellationToken) =>
            {
                var isNew = cnxt.InstanceToValidate.Id.IsNullOrEmpty();
                if (string.IsNullOrEmpty(barcode) & isNew)
                {
                    return;
                }

                foreach (var itemUnit in cnxt.InstanceToValidate.ItemUnits)
                {
                    CheckInternalBarcode(itemUnit.Barcode, false);
                    await CheckBarcode(itemUnit.Barcode);
                }
                foreach (var itemBarcode in cnxt.InstanceToValidate.ItemBarcodes)
                {
                    CheckInternalBarcode(itemBarcode.Barcode, false);
                    await CheckBarcode(itemBarcode.Barcode);
                }
                
                CheckInternalBarcode(barcode, true);
                await CheckBarcode(barcode);

                async Task<bool> CheckForConflicts(string barcode)
                {
                    var buldr = serviceBase.Build();
                    buldr = buldr.And(w => w.Barcode == barcode);
                    var isNewLoc = cnxt.InstanceToValidate.Id.IsNullOrEmpty();

                    buldr = buldr.Or(w => w.ItemUnits.Any(c => c.Barcode == barcode));
                    buldr = buldr.Or(w => w.ItemBarcodes.Any(c => c.Barcode == barcode));
                    
                    if (!isNewLoc)
                    {
                        buldr = buldr.And(w => w.Id != cnxt.InstanceToValidate.Id);
                    }

                    return await serviceBase.AnyAsync(buldr);
                }
                
                async Task CheckBarcode(string barcode)
                {
                    if (!string.IsNullOrEmpty(barcode))
                    {
                        var exists = await CheckForConflicts(barcode);
                        if (exists)
                        {
                            cnxt.AddFailure($"الباركود {barcode} مستخدم من قبل صنف آخر"); // Localization
                        }
                    }
                }

                void CheckInternalBarcode(string barcode, bool skipMainBarcode)
                {
                    var countBarcodes = cnxt.InstanceToValidate.ItemBarcodes.Count(c => c.Barcode == barcode);
                    var countUnits = cnxt.InstanceToValidate.ItemUnits.Count(c => c.Barcode == barcode);

                    if (countUnits > 1)
                    {
                        cnxt.AddFailure($"الباركود {barcode} متكرر مع الباركود في احدى باركودات الوحدات.");
                    }

                    if (countBarcodes > 1)
                    {
                        cnxt.AddFailure($"الباركود {barcode} متكرر مع الباركود في احدى الباركودات الأضافية.");
                    }
                    
                    if (!skipMainBarcode)
                    {
                        if (cnxt.InstanceToValidate.Barcode == barcode)
                        {
                            cnxt.AddFailure($"الباركود {barcode} متكرر مع الباركود الأساسي.");
                        }
                    }
                } 
            });
    }
}
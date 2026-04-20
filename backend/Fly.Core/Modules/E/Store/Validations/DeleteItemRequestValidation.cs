using FluentValidation;
using Fly.Core.Modules.E.Store.Models.DbModels;
using Fly.Core.Modules.F.Marketers.Models.DbModels;
using Fly.Core.Persistence.Ef.Models.Store;
using Fly.Framework.Server.Persistence;
using Fly.Shared.Modules.Marketers.Dtos;
using Fly.Shared.Modules.Store.Dtos;
using Fly.Shared.Modules.Store.Requests;

namespace Fly.Core.Modules.E.Store.Validations;

public class DeleteItemRequestValidation : AbstractValidator<DeleteItemRequest>
{
    public DeleteItemRequestValidation(
        IServiceBase<DtoItem, TItem> serviceBase
        , IServiceBase<DtoInventory, TInventory> inventoryService
        , IServiceBase<DtoMarketerInventory, TMarketerInventory> inventoryMarketerService
    )
    {
        RuleFor(x => x.Id)
            .CustomAsync(async (id, cntx, cancellationToken) =>
            {
                // var id = cntx.InstanceToValidate.Id;
                var any = await inventoryService.AnyAsync(w => w.ItemId == id);
                var anyMarketer = await inventoryMarketerService.AnyAsync(w => w.ItemId == id);
                if (any || anyMarketer)
                {
                    cntx.AddFailure("هذا الصنف جرت عليه عمليات، لا يمكن حذفه.");
                }
            });
    }
}
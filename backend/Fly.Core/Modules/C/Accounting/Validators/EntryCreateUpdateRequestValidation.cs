using FluentValidation;
using Fly.Core.Persistence.Ef.Models;
using Fly.Framework.Server.Persistence;
using Fly.Shared.Modules.Accounting.Dtos;
using Fly.Shared.Modules.Accounting.Requests;

namespace Fly.Core.Modules.C.Accounting.Validators;

public class EntryCreateUpdateRequestValidation : AbstractValidator<CreateEntryRequest>
{
    public EntryCreateUpdateRequestValidation(IServiceBase<DtoEntry, TEntry> service)
    {
        //RuleFor(prp => prp.Name)
        //    .NotEmpty()
        //    .NotNull()
        //    .CustomAsync(async (name, cntxt, cans) =>
        //    {
        //        var id = cntxt.InstanceToValidate.Id;
        //        var isNEw = cntxt.InstanceToValidate.IsNew();
        //        var w = service.Build();
        //        if (!isNEw) w = w.And(w => w.Id != id);
        //        w = w.And(w => w.Name.Contains(name));
        //        var isAny = await service.AnyAsync(w);
        //        if (isAny) cntxt.AddFailure("please chose other name");
        //    })
        //;

        RuleFor(prp => prp.Id)
            .CustomAsync(async (id, cntxt, cans) =>
            {
                var isNEw = cntxt.InstanceToValidate.IsNew();
                if (!isNEw)
                {
                    var ent = await service.GetAsync(w => w.Id == id);
                    if (ent.OprationType != Shared.Modules.Accounting.Enums.OprationTypeEnum.Normal & !cntxt.InstanceToValidate.IgnoreType)
                    {
                        cntxt.AddFailure("تم إنشاء هذا القيد بطريقة تلقائية، في حالة كنت تتحمل مسؤولية تعديل هذا القيد اضغط على خانة تجاهل منع التعديل على القيود التلقائية");
                    }
                }
            })
        ;


        RuleFor(prp => prp.Details)
            .NotEmpty()
            .NotNull()
            .CustomAsync(async (details, cntxt, cans) =>
            {
                var depit = details.Sum(x => x.Debit);
                var credir = details.Sum(x => x.Credit);
                if (depit != credir)
                {
                    cntxt.AddFailure("القيد غير متزن"); // TODO: Localization
                }


                details.ForEach(async f =>
                {
                    var accountService = service.MakeChildService<DtoAccount, TAccount>();

                    var isThisAccountParent = await accountService.AnyAsync(w => w.ParentId == f.AccountId);
                    var accountIfno = await accountService.GetOneAsAsync(s => new { s.Name }, w => w.Id == f.AccountId);

                    if (isThisAccountParent)
                    {
                        cntxt.AddFailure($"الحساب {accountIfno!.Name} له فروع ولا يمكن استخدامه في القيد"); // TODO: Localization
                    }
                });
            })
        ;

    }
}
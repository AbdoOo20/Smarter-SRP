using FluentValidation;
using Fly.Core.Modules.F.Marketers.Models.DbModels;
using Fly.Framework.Server.Common.Localization;
using Fly.Framework.Server.Persistence;
using Fly.Shared.Modules.Marketers.Dtos;
using Fly.Shared.Modules.Marketers.Requests;

namespace Fly.Core.Modules.F.Marketers.Validations;


public class MarketerDisbursementPermitCreateUpdateRequestValidator : AbstractValidator<MarketerDisbursementPermitCreateUpdateRequest>
{
    public MarketerDisbursementPermitCreateUpdateRequestValidator(
        ILocalizationService l
        , IServiceBase<DtoMarketerDisbursementPermit, TMarketerDisbursementPermit> repositry)
    {
        //RuleFor(prp => prp.Name)
        //    .NotEmpty().WithMessage(/*l.GetFormatedText("v-NotEmpty")*/"لا يمكن ترك الاسم فارغ")
        //    .NotNull().WithMessage(/*l.GetFormatedText("v-NotNull")*/"لا يمكن ترك الاسم فارغ")
        //    .CustomAsync(async (name, cntxt, cancellationToken) =>
        //    {
        //        var isEdit = !cntxt.InstanceToValidate.Id.IsNullOrEmpty();
        //        var where = repositry.Build();
        //        where = where.And(w => w.Name == name);
        //        if (isEdit)
        //        {
        //            where = where.And(w => w.Id != cntxt.InstanceToValidate.Id);
        //        }
        //        var isExists = await repositry.AnyAsync(where);
        //        if (isExists)
        //        {
        //            cntxt.AddFailure(/*l.GetFormatedText("v-exists", name)*/"الاسم موجود مسبقا، الرجاء اختيار اسم جديد");
        //        }
        //    })
        //;

        //this.RuleFor(prp => prp.Id)
        //    .CustomAsync(async (id, cntxt, cancellationToken) =>
        //    {
        //        var inctance = cntxt.InstanceToValidate;
        //        var anyDtails = inctance.Roles.Any();
        //        if (!anyDtails)
        //        {
        //            cntxt.AddFailure(/*l.GetFormatedText("v-RoleGroupCreateUpdate-details")*/"لم يتم تحديد اي صلاحية");
        //        }
        //    });
    }
}
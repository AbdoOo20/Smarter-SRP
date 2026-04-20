using Ardalis.Result.FluentValidation;
using FluentValidation;
using Fly.Core.Modules.Global.Models.DbModels;
using Fly.Core.Modules.Global.Service;
using Fly.Framework.Server.Authentications;
using Fly.Framework.Server.Common;
using Fly.Framework.Server.Common.Controllers.CommandsBased;
using Fly.Framework.Server.Common.Localization;
using Fly.Framework.Server.Persistence;
using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Http;
using Fly.Shared.Modules.Global.Dtos;
using Fly.Shared.Modules.Global.Features;
using Fly.Shared.Modules.Global.Requests;
using LinqKit;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Fly.Core.Modules.Global.Controllers;

public class GroupController
    : CustomControllerFeatrueBased<DtoRoleGroup, TRoleGroup, RoleGroupQuery, RoleGroupDelete, RoleGroupCreateUpdate>, IRoleGroupFeatures
{

    class CreateUpdateValidator : AbstractValidator<RoleGroupCreateUpdate>
    {
        public CreateUpdateValidator(ILocalizationService l, IServiceBase<DtoRoleGroup, TRoleGroup> repositry)
        {
            RuleFor(prp => prp.Name)
                .NotEmpty().WithMessage(/*l.GetFormatedText("v-NotEmpty")*/"لا يمكن ترك الاسم فارغ")
                .NotNull().WithMessage(/*l.GetFormatedText("v-NotNull")*/"لا يمكن ترك الاسم فارغ")
                .CustomAsync(async (name, cntxt, cancellationToken) =>
                {
                    var isEdit = !cntxt.InstanceToValidate.Id.IsNullOrEmpty();
                    var where = repositry.Build();
                    where = where.And(w => w.Name == name);
                    if (isEdit)
                    {
                        where = where.And(w => w.Id != cntxt.InstanceToValidate.Id);
                    }
                    var isExists = await repositry.AnyAsync(where);
                    if (isExists)
                    {
                        cntxt.AddFailure(/*l.GetFormatedText("v-exists", name)*/"الاسم موجود مسبقا، الرجاء اختيار اسم جديد");
                    }
                })
            ;

            this.RuleFor(prp => prp.Id)
                .CustomAsync(async (id, cntxt, cancellationToken) =>
                {
                    var inctance = cntxt.InstanceToValidate;
                    var anyDtails = inctance.Roles.Any();
                    if (!anyDtails)
                    {
                        cntxt.AddFailure(/*l.GetFormatedText("v-RoleGroupCreateUpdate-details")*/"لم يتم تحديد اي صلاحية");
                    }
                });
        }
    }

    class DeleteValidator : AbstractValidator<RoleGroupDelete>
    {
        public DeleteValidator()
        {
            this.RuleFor(prp => prp.Id)
                .CustomAsync(async (id, cntxt, cancellationToken) =>
                {
                    var inctance = cntxt.InstanceToValidate;

                });
        }
    }

    class UserRoleCreateUpdateValidator : AbstractValidator<UserRoleCreateUpdate>
    {
        public UserRoleCreateUpdateValidator()
        {
            this.RuleFor(prp => prp.Id)
                .CustomAsync(async (id, cntxt, cancellationToken) =>
                {
                    var inctance = cntxt.InstanceToValidate;

                });
        }
    }


    private readonly JwtTokenHelper _tokenHelper;
    private readonly IServiceBase<DtoRoleGroupDetail, TRoleGroupDetail> _groupDetailservice;
    private readonly ITokenService _tokenService;

    public GroupController(ILocalizationService localizationService, ILogger<GroupController> logger, JwtTokenHelper tokenHelper, IMediator mediator,
        AutoMapper.IMapper mapper, IServiceBase<DtoRoleGroup, TRoleGroup> service, IServiceBase<DtoRoleGroupDetail, TRoleGroupDetail> groupDetailservice,
        MapsterMapper.IMapper mapster, ITokenService tokenService)
        : base(localizationService, logger, mediator, service, mapper, mapster)
    {
        _tokenHelper = tokenHelper;
        _groupDetailservice = groupDetailservice;
        _tokenService = tokenService;

        OnCreateUpdate += GroupControllerOnCreateUpdate;
    }

    private async ValueTask<DtoRoleGroup> GroupControllerOnCreateUpdate(RoleGroupCreateUpdate q)
    {

        using var trn = _service.BeginTransaction();
        _groupDetailservice.BeginTransaction(trn);

        try
        {
            DtoRoleGroup? dto = null;
            //  todo: stupid code
            if (q.Id.IsNullOrEmpty())
            {
                dto = q.Adapt<DtoRoleGroup>();
                await _service.Add(dto);
                var groupDetials = q.Roles.Select(s => new DtoRoleGroupDetail()
                {
                    RoleGroupId = dto.Id,
                    RoleId = s
                }).ToList();
                await _groupDetailservice.AddRange(groupDetials);
            }
            else
            {
                await _groupDetailservice.Remove(w => w.RoleGroupId == q.Id);
                dto = q.Adapt<DtoRoleGroup>();
                await _service.Update(dto);
                var groupDetials = q.Roles.Select(s => new DtoRoleGroupDetail()
                {
                    RoleGroupId = dto.Id,
                    RoleId = s
                }).ToList();
                await _groupDetailservice.AddRange(groupDetials);
            }
            trn.Commit();
            return dto;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "GroupControllerOnCreateUpdate");
            //trn.Rollback();
            throw;
        }
    }

    public override async ValueTask<HttpResponseWrapper<DtoRoleGroup?>?> ValidateCreateUpdateAction(RoleGroupCreateUpdate command, ResultOrFailer<DtoRoleGroup?> opt)
    {
        CreateUpdateValidator validationRules = new CreateUpdateValidator(l, Service);
        var validationStatusinfo = await validationRules.ValidateAsync(command);
        if (!validationStatusinfo.IsValid) return opt.Problem(validationStatusinfo.AsErrors());
        return null;
    }

    public override async ValueTask<HttpResponseWrapper<DtoRoleGroup?>> ValidateDeleteAction(RoleGroupDelete command, ResultOrFailer<DtoRoleGroup?> opt)
    {
        DeleteValidator validationRules = new DeleteValidator();
        var validationStatusinfo = await validationRules.ValidateAsync(command);
        if (!validationStatusinfo.IsValid) return opt.Problem(validationStatusinfo.AsErrors());
        return null;
    }

    

    [HttpGet("Roles/{id}")]
    public ValueTask<HttpResponseWrapperList<dynamic>> PostUserRoles(string id)
    =>
        TryMeny<dynamic>(async opt =>
        {
            var dbId = Identifier.Parse(id);
            var roleService = opt.GetService<DtoRole, TRole>();
            var gtoupDetailService = opt.GetService<DtoRoleGroupDetail, TRoleGroupDetail>();

            var all = await roleService.GetAllAsync(w=> !w.IsSecret);
            var selected = await gtoupDetailService.GetAllAsAsync(s => s.RoleId, w => w.RoleGroupId == dbId);
            var uiGrouped = all.Items.Where(w => w.Key.Contains('.')).GroupBy(xx => xx.Key.Split(".")[0]);

            var uiNeeded = uiGrouped.Select(s => new
            {
                Id = Identifier.New(),
                WindowName = s.Key,
                Keys = s.Select(cc => new RoleKey
                {
                    Id = cc.Id,
                    KeyName = cc.Key,
                    //RoleName = "",
                    IsChecked = selected.Items.Contains(cc.Id),
                }).ToList()
            });

            return opt.Success(uiNeeded);
        });



}


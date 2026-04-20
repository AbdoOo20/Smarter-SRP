using Ardalis.Result.FluentValidation;
using FluentValidation;
using Fly.Core.Modules.F.PurchaseAndSales.Models.DbModels;
using Fly.Core.Modules.Global.Models.DbModels;
using Fly.Core.Modules.Global.Service;
using Fly.Core.Persistence.Ef.Models.Store;
using Fly.Framework.Server.Authentications;
using Fly.Framework.Server.Common;
using Fly.Framework.Server.Common.Controllers.CommandsBased;
using Fly.Framework.Server.Common.Localization;
using Fly.Framework.Server.Persistence;
using Fly.Framework.Shared.Common.Collections;
using Fly.Framework.Shared.Common.Http;
using Fly.Framework.Shared.Common.Requests;
using Fly.Framework.Shared.Common.ValueTypes;
using Fly.Shared.Modules.Global.Dtos;
using Fly.Shared.Modules.Global.Features;
using Fly.Shared.Modules.Global.Requests;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Misc;
using Fly.Shared.Modules.Store.Dtos;
using LinqKit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Fly.Core.Modules.Global.Controllers;

public class UserController
    : CustomControllerFeatrueBased<DtoUser, TUser, UserQuery, UserDelete, UserCreateUpdate>, IUserFeatures
{
    class CreateUpdateValidator : AbstractValidator<UserCreateUpdate>
    {
        public CreateUpdateValidator(IServiceBase<DtoUser, TUser> service, ILocalizationService l)
        {
            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("v-NotEmpty")
                .NotNull().WithMessage("v-NotNull")

                .CustomAsync(async (phone, cntxt, cancellationToken) =>
                {
                    var isEdit = !cntxt.InstanceToValidate.Id.IsNullOrEmpty();
                    var where = service.Build();
                    where = where.And(w => w.Phone == phone);
                    if (isEdit)
                    {
                        where = where.And(w => w.Id != cntxt.InstanceToValidate.Id);
                    }
                    var isExists = await service.AnyAsync(where);
                    if (isExists)
                    {
                        cntxt.AddFailure("الرقم موجود مسبقا، الرجاء اختيار رقم هاتف آخر"); // localization
                    }
                });
        }
    }


    private readonly JwtTokenHelper _tokenHelper;
    private readonly ITokenService _tokenService;

    public UserController(ILocalizationService localizationService, ILogger<UserController> logger, JwtTokenHelper tokenHelper, IMediator mediator,
        AutoMapper.IMapper mapper, IServiceBase<DtoUser, TUser> service,
        MapsterMapper.IMapper mapster, ITokenService tokenService)
        : base(localizationService, logger, mediator, service, mapper, mapster)
    {
        _tokenHelper = tokenHelper;
        _tokenService = tokenService;
    }

    public override async ValueTask<HttpResponseWrapper<DtoUser?>> ValidateCreateUpdateAction(UserCreateUpdate command, ResultOrFailer<DtoUser?> opt)
    {
        var l = opt.GetService<ILocalizationService>();
        var validator = new CreateUpdateValidator(_service, l);
        var validationStatusinfo = await validator.ValidateAsync(command);
        if (!validationStatusinfo.IsValid) return opt.Problem(validationStatusinfo.AsErrors());
        return null;
    }

    protected override ValueTask BeforAdd(DtoUser dto, UserCreateUpdate command, TUser? entity)
    {
        if (dto.Id.IsNullOrEmpty() || (!dto.Id.IsNullOrEmpty() & dto.Password.Length > 0))
        {
            _tokenService.CreatePasswordHash(dto.Password, out var passwordHash, out var salt);
            dto.Sult = salt;
            dto.Password = passwordHash;
        }
        else if(dto.Password.Length is 0 && entity is not null) 
        {
            // Preserve the credentials when the user provides an empty list
            dto.Sult = entity.Sult;
            dto.Password = entity.Password;
        }
        if (dto.RoleGroupId.GetValueOrDefault().IsNullOrEmpty()) dto.RoleGroupId = null;
        return base.BeforAdd(dto, command, entity);
    }
    
    public override Task BeforReturn(PageList<DtoUser> list)
    {
        list.Items.ForEach(f =>
        {
            f.Sult = null;
            f.Password = null;
        });
        return base.BeforReturn(list);
    }

    [HttpGet("Groups")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetGroups(
        [FromQuery] RoleGroupQuery request)
        => GetSelectableList<DtoRoleGroup, TRoleGroup>(x => x.Id, x => x.Name);

    [HttpGet("Safes")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetSafes(
        [FromQuery] QueryRequestBase request)
        => GetSelectableList<DtoSafe, TSafe>(x => x.Id, x => x.Name);

    [HttpGet("Banks")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetBanks(
        [FromQuery] QueryRequestBase request)
        => GetSelectableList<DtoBank, TBank>(x => x.Id, x => x.Name);
    
    [HttpGet("Storages")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetStorages(
        [FromQuery] QueryRequestBase request) 
        => GetSelectableList<DtoStore, TStore>(x => x.Id, x => x.Name);
}


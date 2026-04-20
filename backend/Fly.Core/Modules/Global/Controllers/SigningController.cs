//

using Fly.Core.Modules.F.Marketers.Models.DbModels;
using Fly.Core.Modules.Global.Models.DbModels;
using Fly.Core.Modules.Global.Service;
using Fly.Framework.Server.Authentications;
using Fly.Framework.Server.Common.Controllers.Base;
using Fly.Framework.Server.Common.Controllers.Helpers;
using Fly.Framework.Server.Common.Localization;
using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Http;
using Fly.Framework.Shared.Modules.Identity.Dtos;
using Fly.Shared.Modules.Global.Dtos;
using Fly.Shared.Modules.Marketers.Dtos;
using Fly.Shared.Modules.Marketers.Response;
using Fly.Shared.Modules.Subscription.Features;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Fly.Core.Modules.Global.Controllers;

public class SigningController
    : CustomControllerBase, ISigningFeature
//DeleteRequestBase, RCreateAccountRequest>
{
    private readonly JwtTokenHelper _tokenHelper;
    private readonly ITokenService _tokenService;

    public SigningController(ILocalizationService localizationService, ILogger<SigningController> logger, JwtTokenHelper tokenHelper, IMediator mediator,
        AutoMapper.IMapper mapper,
        MapsterMapper.IMapper mapster, ITokenService tokenService) : base(localizationService, logger, mediator, mapper, mapster)
    {
        _tokenHelper = tokenHelper;
        _tokenService = tokenService;
    }

    [Authorize]
    [HttpGet("company-settings")]
    public ValueTask<HttpResponseWrapper<DtoCompanyInfo>> GetCompany()
    => TryOne<DtoCompanyInfo>(async opt =>
    {
        var service = opt.GetService<DtoCompanyInfo, TCompanyInfo>();
        var options = await service.GetAllAsync();
        if (options == null || options.Items.Count() == 0)
            return opt.Problem("Options not set yet."); // TODO: Localization
        var appOption = options.Items.First();
        return opt.Success(appOption);
    });
    
    [Authorize]
    [HttpPost]
    public ValueTask<HttpResponseWrapper<DtoUserInfo>> GetUserInfo()
    =>
        TryOne<DtoUserInfo>(async opt =>
        {
            var userService = opt.GetService<IUserService>();
            var encryptionAlgorithm = Prepare();
            if (encryptionAlgorithm is null)
                return opt.Problem("401");
            var hlper = new ClaimsHelper(encryptionAlgorithm, User);

            var userId = hlper.GetUserId();
            var user = await userService.GetById(userId);
            if (user is null)
                return opt.Problem("User not found");

            if (!user.IsGhost)
            {
                if (!user.RoleGroupId.HasValue || user.RoleGroupId.Value.IsNullOrEmpty())
                {
                    return opt.Success(new DtoUserInfo()
                    {
                        Name = user.Name,
                        Id = user.Id,
                        Phone = user.Phone,
                        VisibleStorages = user.VisibleStorages,
                        VisibleSafes = user.VisibleSafes,
                        VisibleBanks = user.VisibleBanks,
                        DefaultPaymentType = user.DefaultPaymentType,
                        DefaultStorageType = user.DefaultStorageType,
                        DefaultStorageId = user.DefaultStorageId,
                        DefaultSafeId = user.DefaultSafeId,
                        DefaultBankId = user.DefaultBankId,
                    });
                }
            }

            var roleService = opt.GetService<DtoRole, TRole>();
            var groupService = opt.GetService<DtoRoleGroupDetail, TRoleGroupDetail>();

            var rolesInfo = await groupService.GetAllAsAsync(s => new { s.RoleId }, w => w.RoleGroupId == user.RoleGroupId);
            var rolesIds = rolesInfo.Items.Select(w => w.RoleId).ToList();
            var roles = (await roleService.GetAllAsync(w => rolesIds.Contains(w.Id))).Items.ToList();
            if (user.IsGhost)
            {
                roles.Add(
                    new DtoRole()
                    {
                        Name = "h",
                        Key = "GHOST",
                        IsSecret = true,
                        Id = Identifier.New(),
                    }
                );
            }
            return opt.Success(new DtoUserInfo()
            {
                Id = user.Id,
                Name = user.Name,
                Phone = user.Phone,
                Roles = roles,
                VisibleStorages = user.VisibleStorages,
                VisibleSafes = user.VisibleSafes,
                VisibleBanks = user.VisibleBanks,
                DefaultPaymentType = user.DefaultPaymentType,
                DefaultStorageType = user.DefaultStorageType,
                DefaultStorageId = user.DefaultStorageId,
                DefaultSafeId = user.DefaultSafeId,
                DefaultBankId = user.DefaultBankId
            });
        });

    public async Task AddRootUserAsync()
    {
        await TryOne<DtoUser>(async opt =>
        {
            var userService = opt.GetService<IUserService>();
            
            var pass = await _tokenService.GeneratePassword("test");
                
            var newUser = new DtoUser
            {
                Name = "Root",
                Id = Guid.NewGuid(),
                Phone = "0110",
                Password = pass.Password,
                Sult = pass.Salt,
                IsGhost = true,
                IsAdmin = true,
                RoleGroupId = null,
            };
            await userService.Add(newUser);

            return opt.Success(newUser);
        });
    }
    
    [AllowAnonymous]
    [HttpPost("signin")]
    public ValueTask<HttpResponseWrapper<SubscriperInfoResponse>> Signing([FromBody] SigningRequest request)
    =>
        TryOne<SubscriperInfoResponse>(async opt =>
        {
            //await AddRootUserAsync();
            //await Task.CompletedTask;

            var userService = opt.GetService<IUserService>();
            var marketerService = opt.GetService<DtoMarketer, TMarketer>();

            var userInfo = await userService
                .GetOneAsAsync(w =>
                    new
                    {
                        w.Accesser,
                        w.Password,
                        w.Sult,
                        w.Id,
                        w.Name,
                        w.Phone,
                        w.RoleGroupId,
                    }, w => w.Phone == request.Phone);

            if (userInfo is null)
            {
                return opt.Problem("البيانات غير صحيحة"); // localization
                //return opt.Problem(statusCodes: System.Net.HttpStatusCode.NotFound, "");
            }

            var isValidPassword = await _tokenService.VerifyPassword(userInfo.Sult, request.Password, userInfo.Password);
            if (!isValidPassword)
            {
                return opt.Problem("البيانات غير صحيحة"); // localization
            }

            var user = await userService.GetById(userInfo.Id);
            var marketerInfo = await marketerService.GetOneAsAsync(w => new { w.Id }, w => w.Phone == request.Phone);

            if (marketerInfo is null && request.IsMarketer)
            {
                return opt.Problem("يرجى أدخال معلومات دخول تابعة لمسوق"); // localization
            }
            
            var userToken = _tokenHelper.CreateToken(user!, (claims, algorithm) =>
            {
                if (marketerInfo is not null)
                {
                    var encryptedMaeketer = algorithm.Encrypt(marketerInfo.Id.ToString());
                    claims.Add(new System.Security.Claims.Claim(CustomClaims.MarketerIdentifier, encryptedMaeketer));
                }

            });
            return opt.Success(new SubscriperInfoResponse(userToken, userInfo.Name, userInfo.Phone, "", $"/images/website/user-avatar.jpg"));
        });
}


//public class UserController
//    : CustomControllerFeatrueBased<DtoUser, TUser, UserQuery, UserDelete, UserCreateUpdate>, IUserFeatures
//{

//    class UserCreateUpdateValidator : AbstractValidator<UserCreateUpdate>
//    {
//        public UserCreateUpdateValidator()
//        {

//        }
//    }
//    class UserRoleCreateUpdateValidator : AbstractValidator<UserRoleCreateUpdate>
//    {
//        public UserRoleCreateUpdateValidator()
//        {
//            this.RuleFor(prp => prp.Id)
//                .CustomAsync( async (id, cntxt, cancellationToken) =>
//                {
//                    var inctance = cntxt.InstanceToValidate;

//                });
//        }
//    }

//    private readonly JwtTokenHelper _tokenHelper;
//    private readonly ITokenService _tokenService;

//    public UserController(ILocalizationService localizationService, ILogger<UserController> logger, JwtTokenHelper tokenHelper, IMediator mediator,
//        AutoMapper.IMapper mapper, IServiceBase<DtoUser, TUser> service,
//        MapsterMapper.IMapper mapster, ITokenService tokenService)
//        : base(localizationService, logger, mediator, service, mapper, mapster)
//    {
//        _tokenHelper = tokenHelper;
//        _tokenService = tokenService;
//    }

//    public ValueTask<HttpResponseWrapper<DtoUser>> PostUserRoles(UserRoleCreateUpdate userRoleCreateUpdate)
//    =>
//        TryOne<DtoUser>(async opt =>
//        {
//            UserRoleCreateUpdateValidator validationRules = new();
//            var validationStatusInfo = await validationRules.ValidateAsync(userRoleCreateUpdate);
//            if (!validationStatusInfo.IsValid)
//            {
//                return opt.Problem(validationStatusInfo.AsErrors());
//            }

//            return opt.Success(new());
//        });

//}
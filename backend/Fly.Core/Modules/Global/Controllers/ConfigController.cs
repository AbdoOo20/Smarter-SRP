using Fly.Core.Modules.Global.Models.DbModels;
using Fly.Core.Persistence.Ef.Models;
using Fly.Core.Persistence.Ef.Models.Global;
using Fly.Framework.Server.Common.Controllers.Base;
using Fly.Framework.Server.Common.Localization;
using Fly.Framework.Server.Persistence;
using Fly.Framework.Shared.Common.Http;
using Fly.Framework.Shared.Common.ValueTypes;
using Fly.Shared.Modules.Accounting.Dtos;
using Fly.Shared.Modules.Accounting.Requests;
using Fly.Shared.Modules.Global.Dtos;
using Fly.Shared.Modules.Global.Features;
using Fly.Shared.Modules.Global.Requests;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Fly.Core.Modules.Global.Controllers;

public class ConfigController(ILocalizationService localizationService, ILogger<CustomControllerBase> logger, IMediator mediator, AutoMapper.IMapper mapper, MapsterMapper.IMapper mapster, IServiceBase<DtoConfig, TConfig> _service)
                : CustomControllerBase(localizationService, logger, mediator, mapper, mapster)
        , IConfigFeatures

{
    [HttpPost]
    public ValueTask<HttpResponseWrapper<DtoConfig>> PostSaveAsync(SettingsRequest request)
     => TryOne<DtoConfig>(async opt =>
     {
         var service = opt.GetService<DtoConfig, TConfig>();
         var companyInfoService = opt.GetService<DtoCompanyInfo, TCompanyInfo>();

         // 001
         DtoConfig? conf = null;
         var options = await service.GetAllAsync();
         if (options == null || options.Items.Count() == 0)
         {
             conf = request.Config.Adapt<DtoConfig>();
             await service.Add(conf);
         }
         else
         {
             conf = options.Items.First();
             request.Config.Adapt(conf);
             await service.Update(conf);
         }

         // 002
         DtoCompanyInfo? companyInfo = null;
         var infoList = await companyInfoService.GetAllAsync();
         if (infoList is null || infoList.Items.Count() == 0)
         {
             companyInfo = request.CompanyInfo.Adapt<DtoCompanyInfo>();
             await companyInfoService.Add(companyInfo);
         }
         else
         {
             companyInfo = infoList.Items.First();
             request.CompanyInfo.Adapt(companyInfo);
             await companyInfoService.Update(companyInfo);
         }

         return opt.Success(conf);
     });

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

    [HttpGet("account-settings")]
    public ValueTask<HttpResponseWrapper<DtoConfig>> GetAllAsync()
     => TryOne<DtoConfig>(async opt =>
     {
         var service = opt.GetService<IServiceBase<DtoConfig, TConfig>>();

         var options = await service.GetAllAsync();
         if (options == null || options.Items.Count() == 0)
             return opt.Problem("Options not set yet."); // TODO: Localization
         var appOption = options.Items.First();
         return opt.Success(appOption);
     });

    [HttpGet("accounts")]
    public ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupAccounts([FromQuery] AccountQueryRequest request)
        =>
        TryMeny<DefaultLookup>(async (opt) => opt.Success(await _service.MakeChildService<DtoAccount, TAccount>().GetSelecListAsync(w => w.Name)));

}
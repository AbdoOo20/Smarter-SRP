using AutoMapper;
using Fly.Framework.Server.Common.Controllers.Helpers;
using Fly.Framework.Server.Common.Localization;
using Fly.Framework.Server.Constsnts;
using Fly.Framework.Server.Exceptions.Controllers;
using Fly.Framework.Server.Security.Algorithms;
using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Http;
using Fly.Framework.Shared.Modules.Identity.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Fly.Framework.Server.Common.Controllers.Base;

[ApiController]
[Route("[controller]")]
public class CustomControllerBase : ControllerBase
{
    protected readonly ILocalizationService _localizationService;
    public ILocalizationService l => _localizationService;

    public CustomControllerBase(
        ILocalizationService localizationService,
        ILogger<CustomControllerBase> logger, IMediator mediator,
        IMapper mapper,
        MapsterMapper.IMapper mapster)
    {
        _localizationService = localizationService;
        Logger = logger;
        Mediator = mediator;
        Mapper = mapper;
        Mapster = mapster;
    }

    protected ILogger Logger { get; }
    protected IMediator Mediator { get; }
    public IMapper Mapper { get; }
    public MapsterMapper.IMapper Mapster { get; }

    bool _prepared = false;
    private ClaimsHelper hlper;
    ICustomEncryptionAlgorithm? _customEncryptionAlgorithm;

    public ClaimsHelper Helper => hlper;


    protected ICustomEncryptionAlgorithm? Prepare()
    {
        if (User?.Identity?.IsAuthenticated == true)
        {

            var algorithm = this.Response.HttpContext.RequestServices.GetRequiredService<ICustomEncryptionAlgorithm>();

            var keySaltClaim = User.FindFirst(CustomClaims.ReqestTime) ?? throw new Exception("key not found");
            
            var getKey = long.Parse(keySaltClaim.Value);
            var reqestTime = DateTime.FromBinary(getKey);
            var key = long.Parse(reqestTime.ToString(DatetimeConstants.DataFingerprint)).ToString("D16") ?? throw new Exception("key not found!");

            algorithm.SetKey(key);
            _prepared = true;
            _customEncryptionAlgorithm = algorithm;
            return _customEncryptionAlgorithm;
        }
        return null;
    }

    [NonAction]
    protected ClaimsHelper? PrepareNow()
    {
        if (_prepared) return hlper;

        _customEncryptionAlgorithm = Prepare();
        if (_customEncryptionAlgorithm is not null)
        {
            hlper = new ClaimsHelper(_customEncryptionAlgorithm, User);
            _prepared = true;
            return hlper;
        }
        return null;
    }
    
    protected Identifier? GetUserId()
    {
        if (!_prepared)
        {
            Prepare();
        }
        if (_customEncryptionAlgorithm is not null)
        {
            var userIdClaim = User.FindFirst(w => w.Value == CustomClaims.Identifier);

            if (userIdClaim is null) return null;
            else
            {
                return Identifier.Parse(userIdClaim.Value);
            }
        }
        return null;
    }
    
    protected async ValueTask<HttpResponseWrapper<T>> TryOne<T>(Func<ResultOrFailer<T>, ValueTask<HttpResponseWrapper<T>>> func)
    {
        var guid = Guid.NewGuid().ToString();
        try
        {
            var serviceProvider = Response.HttpContext.RequestServices.GetRequiredService<IServiceProvider>().CreateScope().ServiceProvider;
            var r = await func(new ResultOrFailer<T>(serviceProvider));
            if (r.Status)
            {
                return r;
            }
            else
            {
                Logger.LogWarning("returning non 200 ok response with {type} id {id}\n{error}", r.StatusCode, guid, r.ErrorMessage);
                return r;
            }
        }
        catch (InvalidInputException ex)
        {
            Logger.LogWarning(ex, "Error when processing {type} controller with {id}, {input}", typeof(T).Name, guid, ex.Message);
            var err = HttpResponseWrapper<T>.Problem(400, ex.Message);
            return err;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error when processing {type} controller with {id}", typeof(T).Name, guid);
            var err = HttpResponseWrapper<T>.Problem(500, ex.Message);
            return err;
        }
    }
    
    protected async ValueTask<HttpResponseWrapperList<T>> TryMeny<T>(Func<ResultListOrFailer<T>, ValueTask<HttpResponseWrapperList<T>>> func)
    {
        try
        {
            var serviceProvider = Response.HttpContext.RequestServices.GetRequiredService<IServiceProvider>().CreateScope().ServiceProvider;
            var r = await func(new ResultListOrFailer<T>(serviceProvider));
            return r;
        }
        catch (Exception ex)
        {
            var errorGuid = Guid.NewGuid().ToString();
            Logger.LogError(ex, errorGuid);
            return HttpResponseWrapperList<T>.Problem($"error id: [{errorGuid}]");
        }
    }


    //public void Prepare(bool doit)
    //{
    //    if (doit)
    //    {
    //        var type = typeof(DtoAssets).Name;
    //        var id = Request.HttpContext.TraceIdentifier;
    //        Logger.LogError("error with {id} | Error in Add new entity for type {type}\n\robject : [{dto}]", id, type, dto.ToString());
    //        opt.Problem("error");
    //    }
    //}

}
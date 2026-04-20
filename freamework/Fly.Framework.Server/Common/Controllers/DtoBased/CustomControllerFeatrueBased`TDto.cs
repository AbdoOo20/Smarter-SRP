namespace Fly.Framework.Server.Common.Controllers.DtoBased;


//[Obsolete]
//[Authorize]
//public class CustomControllerFeatrueBased<TDto, TEn> : CustomControllerBase, IFeatureBase<TDto>
//    where TEn : TEntity
//    where TDto : DtoBase
//{
//    private readonly IServiceBase<TDto, TEn> _service;

//    protected IServiceBase<TDto, TEn> Sservice => _service;

//    public CustomControllerFeatrueBased(ILogger<CustomControllerFeatrueBased<TDto, TEn>> logger, IMediator mediator, IServiceBase<TDto, TEn> service,
//        AutoMapper.IMapper mapper,
//        MapsterMapper.IMapper mapster) : base(logger, mediator, mapper, mapster)
//    {
//        _service = service;
//    }

//    [HttpGet("{Id}")]
//    public ValueTask<HttpResponseWrapper<TDto>> GetOneAsync(Guid Id)
//    {
//        throw new NotImplementedException();
//    }

//    [HttpPost]
//    public ValueTask<HttpResponseWrapper<TDto?>> PostSaveAsync(TDto dto)
//    {
//        throw new NotImplementedException();
//    }

//    [HttpPost("{guid}")]
//    public ValueTask<HttpResponseWrapper<TDto>> DeleteRemoveAsync(Guid guid)
//    {
//        throw new NotImplementedException();
//    }

//    [HttpGet]
//    public ValueTask<HttpResponseWrapperList<TDto>> GetAllAsync(int? page = null, int? limit = null, string? text = null, Dictionary<string, object[]>? filter = null)
//    {
//        throw new NotImplementedException();
//    }

//    #region Helpers

//    //protected ValueTask<HttpResponseWrapperList<DefaultLookup>> GetArrayOf<TDtoo, TEno>(IServiceBase<TDtoo, TEno> service,)
//    //{
//    //    return service.GetAllAsAsync(x => new DefaultLookup() { Display})
//    //}

//    #endregion

//}


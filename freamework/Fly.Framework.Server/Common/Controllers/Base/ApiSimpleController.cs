namespace Fly.Framework.Server.Common.Controllers.Base;



//public class PageListOrException<T>
//{
//    public PageList<T>? Data { get; set; }
//    public Exception? Exception { get; set; }
//    public string? ErrorMessage { get; set; } = string.Empty;

//    public PageListOrException<T> Create(PageList<T>? data = null, Exception? exception = null)
//    {
//        if (data is null & exception is null) throw new Exception();
//        else if (data is null & exception is not null)
//        {
//            Exception = exception;
//        }
//        else if (data is not null & exception is null)
//        {
//            Data = data;
//        }
//        return this;
//    }

//    public PageListOrException<T> CreateOk(IEnumerable<T> data, int currentPage = 0, int? totalCount = null)
//    {
//        Data = new PageList<T>()
//        {
//            Items = data,
//            CurrentPage = currentPage,
//            TotalCount = totalCount ?? 0,
//        };
//        return this;
//    }
//    public PageListOrException<T> CreateOk(PageList<T> data)
//    {
//        Data = new PageList<T>()
//        {
//            Items = data.Items,
//            CurrentPage = data.CurrentPage,
//            TotalCount = data.TotalCount,
//        };
//        return this;
//    }
//    public PageListOrException<T> CreateProblem(Exception exception)
//    {
//        Exception = exception;
//        return this;
//    }
//}

//public class ResultOrException<T>
//{
//    public T? Data { get; set; }
//    public Exception? Exception { get; set; }

//    public ResultOrException<T> Create(T? data = default, Exception? exception = null)
//    {
//        if (data is null & exception is null) throw new Exception();
//        else if (data is null & exception is not null)
//        {
//            Exception = exception;
//        }
//        else if (data is not null & exception is null)
//        {
//            Data = data;
//        }
//        return this;
//    }

//    public ResultOrException<T> CreateOk(T data, int currentPage = 0, int? totalCount = null)
//    {
//        Data = data;
//        return this;
//    }
//    public ResultOrException<T> CreateProblem(Exception exception)
//    {
//        Exception = exception;
//        return this;
//    }
//}

//[ApiController]
//[Route("[controller]")]
//public class ApiSimpleController : ControllerBase
//{

//    protected readonly ILogger _logger;

//    public delegate ValueTask<T?> GetOne<T>();
//    public delegate ValueTask<PageList<T>> GetList<T>();

//    public delegate ValueTask<ResultOrException<T>> GetOneOrExc<T>(ResultOrException<T> opt);
//    public delegate ValueTask<PageListOrException<T>> GetListOrExc<T>(PageListOrException<T> opt);

//    protected virtual string FINGERPRINT => "";

//    public ApiSimpleController(ILogger<ApiSimpleController> logger)
//    {
//        _logger = logger;
//    }

//    [NonAction]
//    public async ValueTask<HttpResponseWrapper<T>> TryCatch<T>(GetOne<T> func)
//    {
//        try
//        {
//            var res = await func();
//            return HttpResponseWrapper<T>.Success(res!);
//        }
//        catch (Exception ex)
//        {
//            var random = "x01:" + Guid.NewGuid().ToString();
//            _logger.LogError(ex, random);
//            return HttpResponseWrapper<T>.Problem(new CallAdminException(random));
//        }
//    }

//    [NonAction]
//    public async ValueTask<HttpResponseWrapper<T>> TryCatch0<T>(Func<ValueTask<T>> func)
//    {
//        try
//        {
//            var res = await func();
//            var s = HttpResponseWrapper<T>.Success(res!);
//            return s;
//        }
//        catch (Exception ex)
//        {
//            var random = "x01:" + Guid.NewGuid().ToString();
//            _logger.LogError(ex, random);
//            var s = HttpResponseWrapper<T>.Problem(new CallAdminException(random));
//            return s;
//        }
//    }

//    [NonAction]
//    public async ValueTask<HttpResponseWrapperList<T>> TryCatchLit<T>(GetList<T> func)
//    {
//        try
//        {
//            //  ValueTask<HttpResponseWrapperList<T>>
//            var funReturn = await func();
//            var result = HttpResponseWrapperList<T>.Success(funReturn);
//            var response = new ObjectResult(result)
//            {
//                StatusCode = 200,
//            };
//            return result;
//        }
//        catch (Exception ex)
//        {
//            var random = FINGERPRINT + ":" + Guid.NewGuid().ToString();
//            _logger.LogError(ex, random);
//            return HttpResponseWrapperList<T>.Problem(new CallAdminException(random));
//        }
//    }

//    [NonAction] // taken
//    public async ValueTask<HttpResponseWrapperList<T>> TryCatchList<T>(GetListOrExc<T> func)
//    {
//        try
//        {
//            //  ValueTask<HttpResponseWrapperList<T>>
//            PageListOrException<T> opt = new PageListOrException<T>();
//            var funReturn = await func(opt);
//            if (funReturn.Exception != null)
//            {
//                var result = HttpResponseWrapperList<T>.Problem(funReturn.Exception);
//                return result;
//            }
//            else
//            {
//                var result = HttpResponseWrapperList<T>.Success(funReturn.Data!);
//                return result;
//            }
//        }
//        catch (Exception ex)
//        {
//            var random = FINGERPRINT + ":" + Guid.NewGuid().ToString();
//            _logger.LogError(ex, random);
//            return HttpResponseWrapperList<T>.Problem(new CallAdminException(random));
//        }
//    }

//    [NonAction]
//    public async ValueTask<HttpResponseWrapper<T>> TryCatchOne<T>(GetOneOrExc<T> func)
//    {
//        try
//        {
//            var opt = new ResultOrException<T>();
//            var funReturn = await func(opt);
//            if (funReturn.Exception != null)
//            {
//                var result = HttpResponseWrapper<T>.Problem(funReturn.Exception);
//                return result;
//            }
//            else
//            {
//                var result = HttpResponseWrapper<T>.Success(funReturn.Data!);
//                return result;
//            }
//        }
//        catch (Exception ex)
//        {
//            var random = FINGERPRINT + ":" + Guid.NewGuid().ToString();
//            _logger.LogError(ex, random);
//            return HttpResponseWrapper<T>.Problem(new CallAdminException(random));
//        }
//    }

//    [NonAction]
//    public HttpResponseWrapperList<T> GetListException<T>(Exception ex, string fingerPrint)
//    {
//        var random = fingerPrint + " " + Guid.NewGuid().ToString();
//        _logger.LogError(ex, random);
//        return HttpResponseWrapperList<T>.Problem(new CallAdminException(random));
//    }

//    [NonAction]
//    public HttpResponseWrapper<T> GetException<T>(Exception ex)
//    {
//        var random = Guid.NewGuid().ToString();
//        _logger.LogError(ex, random);
//        return HttpResponseWrapper<T>.Problem(new CallAdminException(random));
//    }
//}

//[Authorize]
//public class ApiSimpleController<TDto, YEntity> : ApiSimpleController, IFeatureBase<TDto>
//    where TDto : DtoBase
//    where YEntity : TEntity
//{

//    private readonly IServiceBase<TDto, YEntity> _service;

//    public ApiSimpleController(ILogger<ApiSimpleController> logger, IServiceBase<TDto, YEntity> service) : base(logger)
//    {
//        _service = service;
//    }

//    [NonAction]
//    public virtual Expression<Func<TDto, bool>> CreatePredecateBuilder(Expression<Func<TDto, bool>>? where = null)
//    {
//        where ??= _service.Build();
//        //if (Request.QueryString.Value != null)
//        //{
//        //    var searcing = QueryStringHelper.Deserialize<DtoUnit>(Request.QueryString.Value);
//        //    if (!string.IsNullOrEmpty(searcing.Name))
//        //    {
//        //        where = where.And(x => x.Name.Contains(searcing.Name));
//        //    }
//        //}
//        return where;
//    }

//    [HttpGet]
//    public virtual ValueTask<HttpResponseWrapperList<TDto>> GetAllAsync(int? page = null, int? limit = null, string? text = null, Dictionary<string, object[]>? filter = null)
//    {
//        page ??= 1;
//        limit ??= 50;
//        return TryCatchLit(async () =>
//        {
//            var where = CreatePredecateBuilder();
//            var allData = await _service.GetAllAsync(where, page, limit);
//            return allData;
//        });
//    }

//    [HttpGet("{id}")]
//    public ValueTask<HttpResponseWrapper<TDto>> GetOneAsync(Guid id)
//    {
//        return TryCatch(() => _service.GetByGuId(id));
//    }

//    [HttpPost]
//    public virtual ValueTask<HttpResponseWrapper<TDto?>> PostSaveAsync(TDto dto)
//    {
//        return TryCatch0(async () =>
//        {
//            var obj = JsonConvert.SerializeObject(dto, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
//            _logger.LogDebug("POST: [{0}]", obj);
//            if (dto.IsNew())
//                return await _service.Add(dto);
//            else
//                return await _service.Update(dto);
//        });
//    }

//    [HttpDelete("{guid}")]
//    public ValueTask<HttpResponseWrapper<TDto>> DeleteRemoveAsync(Guid guid)
//    {
//        return TryCatch0(async () =>
//        {
//            var entityId = await _service.GetId(guid) ?? throw new Exception();
//            return await _service.Remove(entityId.Value);
//        });
//    }

//}


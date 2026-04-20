using Fly.Framework.Shared.Common.Collections;
using Fly.Framework.Shared.Common.Http.Abstractions;

namespace Fly.Framework.Shared.Common.Http;

public class HttpResponseWrapperList<T> : IHttpResponseWrapperList<T>
{
    public bool Status { get; set; }
    public IEnumerable<T> Data { get; set; } = Enumerable.Empty<T>();

    //[JsonIgnore] public Exception? Exception { get; set; }


    public string? ErrorMessage { get; set; }
    
    public int StatusCode { get; set; }
    
    public int Page { get; set; }
    public int Limit { get; set; }
    public int Total { get; set; }

    //public void SuccessOrException()
    //{
    //    if (Exception is not null)
    //        if (Exception is RequestingApiException eeex)
    //            throw eeex!;
    //    if (!Status || Data is null) throw new CallAdminException(ErrorMessage ?? "Responce not success");
    //}

    //public IEnumerable<T> GetDataOrException()
    //{
    //    SuccessOrException();
    //    return Data!;
    //}

    //public List<T> GetDataListOrException()
    //{
    //    SuccessOrException();
    //    return Data!.ToList();
    //}

    public static HttpResponseWrapperList<T> Success(PageList<T> data)
    {
        return new HttpResponseWrapperList<T>()
        {
            Data = data.Items,
            Status = true,
            Page = data.CurrentPage,
            Total = data.TotalCount,
            StatusCode = 200,
        };
    }
    public static HttpResponseWrapperList<T> Success(IEnumerable<T> data)
    {
        return new HttpResponseWrapperList<T>
        {
            Data = data, 
            Status = true,
            StatusCode = 200,
        };
    }


    public static HttpResponseWrapperList<T> Moc(IEnumerable<T>? data = default)
    {
        return new HttpResponseWrapperList<T>
        {
            Data = Activator.CreateInstance<List<T>>(), Status = true
        };
    }


    public static HttpResponseWrapperList<T> Problem(Exception exception)
    {
        return new HttpResponseWrapperList<T>
        {
            Status = false, 
            ErrorMessage = exception.Message, 
            StatusCode = 500,
            Data = Enumerable.Empty<T>()
        };
    }
    public static HttpResponseWrapperList<T> Problem(string exceptionMessage)
    {
        return new HttpResponseWrapperList<T>
        {
            Status = false, 
            Data = Enumerable.Empty<T>(),
            StatusCode = 500,
            ErrorMessage = exceptionMessage,
        };
    }
    public static HttpResponseWrapperList<T> Problem(int statusCodes, string message)
    {
        return new HttpResponseWrapperList<T>
        {
            Data = default,
            Status = false,
            ErrorMessage = message,
            StatusCode = statusCodes,
        };
    }


    public static HttpResponseWrapperList<T> ShowMessage(string exceptionMessage)
    {
        return new HttpResponseWrapperList<T>
        {
            Status = false,
            Data = Enumerable.Empty<T>(),
            StatusCode = 400,
            ErrorMessage = exceptionMessage,
        };
    }
    
    /*
    public static HttpResponseWrapperList<T> Success<T>(ListDtoResults<T> data)
        where T : DtoEntity
    {
        return new HttpResponseWrapperList<T>()
        {
            Data = data.Items,
            Status = true,
            Page = data.CurrentPage,
            Total = data.TotalCount,
        };
    }
    */
}
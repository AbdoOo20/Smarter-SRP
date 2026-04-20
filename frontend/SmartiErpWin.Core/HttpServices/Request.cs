using System.Net;

namespace SmartErpWin.Core.HttpServices;

public class Request<T>
{
    public T? Entity { get; }
    
    public HttpStatusCode? Code { get; }

    public bool IsSuccess { get;  }

    public string? ResponseText { get; }
        
    public Request(T? entity, HttpStatusCode code, bool isSuccess)
    {
        Entity = entity;
        Code = code;
        IsSuccess = isSuccess;
    }

    public Request(string? responseText, HttpStatusCode code, bool isSuccess)
    {
        ResponseText = responseText;
        Code = code;
        IsSuccess = isSuccess;
    }
}
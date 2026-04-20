namespace Fly.Framework.Shared.Common.Http.Abstractions;

public interface IHttpResponseWrapperBase
{
    //string? Error { get; }
    bool Status { get; set; }
    string? ErrorMessage { get; set; }
    
    int StatusCode { get; set; }
    //Exception? Exception { get; set; }
}
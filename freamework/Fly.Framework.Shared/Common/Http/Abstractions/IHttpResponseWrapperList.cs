namespace Fly.Framework.Shared.Common.Http.Abstractions;

public interface IHttpResponseWrapperList<T> : IHttpResponseWrapperBase
{
    //IEnumerable<T> Data { get; set; }
    int Limit { get; set; }
    int Page { get; set; }
    int Total { get; set; }
}
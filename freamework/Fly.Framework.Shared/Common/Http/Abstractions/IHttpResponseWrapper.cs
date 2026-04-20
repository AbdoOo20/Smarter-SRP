namespace Fly.Framework.Shared.Common.Http.Abstractions;

public interface IHttpResponseWrapper<T> : IHttpResponseWrapperBase
{
    T? Data { get; set; }
}
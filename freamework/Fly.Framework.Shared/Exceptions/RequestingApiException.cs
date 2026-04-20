namespace Fly.Framework.Shared.Exceptions;

public class RequestingApiException : Exception
{
    public RequestingApiException(string endpoint)
    {
        Endpoint = endpoint;
    }

    public string Endpoint { get; set; }
}
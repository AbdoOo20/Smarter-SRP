using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using Fly.Framework.Shared.Common.Features;
using Fly.Framework.Shared.Common.Http;
using Fly.Framework.Shared.Common.Http.Abstractions;
using Fly.Framework.Shared.Common.Requests;
using Fly.Shared.Modules.Accounting.Requests;
using Fly.Shared.Modules.Global.Requests;
using Serilog;
using SmartErpWin.Core.Exceptions;
using SmartErpWin.Core.Extensions;
using SmartErpWin.Core.Globals;
using SmartErpWin.Core.Models;
using SmartErpWin.Core.RequestModels;
using SmartErpWin.Core.ResponseModels;
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident
// ReSharper disable UnassignedGetOnlyAutoProperty
#pragma warning disable CS8613 // Nullability of reference types in return type doesn't match implicitly implemented member.

namespace SmartErpWin.Core.HttpServices;

public class FeatureBase<T> : BaseModel, IFeatureBase<T>
    where T : class, new()
{
    protected virtual string? Endpoint { get; set; }

    protected HttpClient? HttpClient { get; private set; } = new();
    
    protected HttpClientHandler? HttpClientHandler { get; private set; } = new();
    
    private string? BackendUrl => StringGlobals.BackendUrl;
    
    public FeatureBase(HttpClient? client = null, HttpClientHandler handler = null)
    {
        if (HttpClient is not null)
        {
            SetClient(client, handler);
        }
    }
    
    protected void SetClient(HttpClient? client, HttpClientHandler handler)
    {
        HttpClientHandler = handler;
        HttpClient = client;
        HttpClient?.DefaultRequestHeaders.Add("Accept", "text/plain");
    }
    
    public void AuthenticateRequests(string token)
    {
        var authenticationHeaderValue = new AuthenticationHeaderValue("Bearer", token);
        HttpClient.DefaultRequestHeaders.Authorization = authenticationHeaderValue;
    }

    public async ValueTask<HttpResponseWrapper<T>> GetOneAsync(Guid objId)
        => await GlobalSendRequestAsync<HttpResponseWrapper<T>, T>(
            Method.Get,
            paths: [objId.ToString()]);

    public async ValueTask<HttpResponseWrapper<TS>> GetSpecificOneAsync<TS>(Guid objId)
        => await GlobalSendRequestAsync<HttpResponseWrapper<TS>, TS>(
            Method.Get,
            paths: [objId.ToString()]);
    
    protected async ValueTask<HttpResponseWrapper<T>> GetInnerOneAsync(string inner)
        => await GetSpecificInnerOneAsync<T>(inner);

    protected async ValueTask<HttpResponseWrapper<TS>> GetSpecificInnerOneAsync<TS>(string inner)
        => await GlobalSendRequestAsync<HttpResponseWrapper<TS>, TS>(
            Method.Get,
            paths: [inner]);

    protected async ValueTask<HttpResponseWrapper<TS>> PostSpecificOneAsync<TS>(string inner)
        => await GlobalSendRequestAsync<HttpResponseWrapper<TS>, TS>(
            Method.Post,
            paths: [inner]);

    public async ValueTask<HttpResponseWrapper<T>> PostSaveAsync(T dto)
        => await GlobalSendRequestAsync<HttpResponseWrapper<T>, T>(
            Method.Post,
            postObject: dto);

    public async ValueTask<HttpResponseWrapper<string>> PostSaveBatchAsync(List<T> dto)
        => await GlobalSendRequestAsync<HttpResponseWrapper<string>, List<T>>(
            Method.Post,
            postObject: dto,
            paths: ["batch"]);
    
    public async ValueTask<HttpResponseWrapper<T>> PostSaveInnerAsync(T dto, string inner)
        => await GlobalSendRequestAsync<HttpResponseWrapper<T>, T>(
            Method.Post,
            postObject: dto,
            paths: [inner]);

    protected async ValueTask<HttpResponseWrapperList<T>> PostSaveReportAsync(T dto)
        => await GlobalSendRequestAsync<HttpResponseWrapperList<T>, T>(
            Method.Post,
            postObject: dto);

    protected async ValueTask<HttpResponseWrapper<string>> PostSaveStatementLookupAsync(T dto)
        => await GlobalSendRequestAsync<HttpResponseWrapper<string>, T>(
            Method.Post,
            postObject: dto);

    protected async ValueTask<HttpResponseWrapper<TS>> PostSaveSpecificAsync<TS>(TS? dto,
        string innerEndpoint = "",
        bool appendOriginal = true)
        where TS : class
        => await GlobalSendRequestAsync<HttpResponseWrapper<TS>, TS>(
            Method.Post,
            postObject: dto,
            paths: [innerEndpoint],
            appendOriginal: appendOriginal);

    protected async ValueTask<HttpResponseWrapper<TReturn>> PostSaveSpecificAndReturnAsync<TS, TReturn>(
        TS? dto,
        string innerEndpoint = "",
        bool appendOriginal = true)
        where TS : class, new()
        => await GlobalSendRequestAsync<HttpResponseWrapper<TReturn>, TS>(
            Method.Post,
            postObject: dto,
            paths: [innerEndpoint],
            appendOriginal: appendOriginal);

    public async ValueTask<HttpResponseWrapper<T>> DeleteRemoveAsync(Guid guid, Func<Task>? onExceptionAction = null)
        => await GlobalSendRequestAsync<HttpResponseWrapper<T>, T>(
            Method.Delete,
            paths: [guid.ToString()],
            onExceptionAction: onExceptionAction);

    protected async ValueTask<HttpResponseWrapper<T>> GetOneByStringQueryAsync(Tuple<string, string?>[] queries)
        => await GlobalSendRequestAsync<HttpResponseWrapper<T>, T>(
            Method.Get,
            queries: queries.ToList());
    
    protected async ValueTask<HttpResponseWrapper<TSpecific>> GetSpecificOneByStringQueryAsync<TSpecific>(string path = "", Tuple<string, string?>[]? queries = null)
        => await GlobalSendRequestAsync<HttpResponseWrapper<TSpecific>, TSpecific>(
            Method.Get,
            paths: [path],
            queries: (queries ?? []).ToList());
    
    protected async ValueTask<HttpResponseWrapperList<T>> GetAllByQueryAsync(QueryRequestBase query)
        => await GlobalSendRequestAsync<HttpResponseWrapperList<T>, T>(
            Method.Get,
            queries: [new(string.Empty, query.ToQueryString())]);

    protected async ValueTask<HttpResponseWrapper<DynamicReports>> SendByQueryAsync<TQuery>(
        TQuery query,
        string? innerEndpoint)
    where TQuery : new()
        => await GlobalSendRequestAsync<HttpResponseWrapper<DynamicReports>, TQuery>(
            Method.Post,
            paths: [innerEndpoint],
            postObject: query);

    protected async ValueTask<HttpResponseWrapper<TS>> GetInnerByQueryAsync<TS>(string innerEndpoint,
        QueryRequestBase query) where TS : new()
    {
        var uri = BuildPath([innerEndpoint]);
        var queryStr = query.ToQueryString();
        var final = new Uri(uri + "?" + queryStr);

        var req = await GlobalSendRequestAsync<HttpResponseWrapper<TS>, TS>(
            Method.Get, uri: final);

        return req;
    }

    protected async ValueTask<HttpResponseWrapperList<TS>> GetListInnerByQueryAsync<TS>(string innerEndpoint,
        QueryRequestBase query) where TS : new()
    {
        var uri = BuildPath([innerEndpoint]);
        var queryStr = query.ToQueryString();
        var final = new Uri(uri + "?" + queryStr);

        var req = await GlobalSendRequestAsync<HttpResponseWrapperList<TS>, TS>(
            Method.Get, uri: final);

        return req;
    }

    public async ValueTask<HttpResponseWrapperList<T>> GetAllAsync(
        int? page = null,
        int? limit = null,
        string? text = "",
        Dictionary<string, object[]>? search = null)
        => await GlobalSendRequestAsync<HttpResponseWrapperList<T>, T>(
            Method.Get,
            queries:
            [
                new("Page", page.ToString()),
                new("Liimit", limit.ToString()),
                new("SearchText", text)
            ]
        );

    protected async ValueTask<HttpResponseWrapper<TC>> RequestBySingleQueryAsync<TC>(string innerEndpoint,
        string queryTitle,
        string? opHeader = "")
        where TC : class, new()
        => await GlobalSendRequestAsync<HttpResponseWrapper<TC>, TC>(
            Method.Get,
            queries: [new(queryTitle, opHeader)],
            paths: [innerEndpoint]
        );

    public async ValueTask<HttpResponseWrapperList<T>> GetAllInnerAsync(
        string innerEndpoint,
        Guid? objId)
        => await GetInnerAllSpecificAsync<T>(innerEndpoint, objId);

    public async ValueTask<HttpResponseWrapperList<TSpecific>> GetInnerAllSpecificAsync<TSpecific>(
        string innerEndpoint,
        Guid? objId)
        => await GlobalSendRequestAsync<HttpResponseWrapperList<TSpecific>, TSpecific>(
            Method.Get,
            paths: [innerEndpoint, objId?.ToString()]);

    public async ValueTask<HttpResponseWrapperList<TC>> RequestComboModelsAsync<TC>(string innerEndpoint, string opHeader = "")
        where TC : class, new()
        => await GlobalSendRequestAsync<HttpResponseWrapperList<TC>, TC>(
            Method.Get,
            queries: [new("ItemId", opHeader)],
            paths: [innerEndpoint]
        );

    protected async ValueTask<HttpResponseWrapper<TC>> RequestSpecificAsync<TC>(string innerEndpoint)
        where TC : new()
        => await GlobalSendRequestAsync<HttpResponseWrapper<TC>, TC>(
            Method.Get,
            paths: [innerEndpoint]
        );

    protected async ValueTask<HttpResponseWrapper<TC>> RequestSpecificByQueryAsync<TC>(QueryRequestBase request, string innerEndpoint)
        where TC : new()
        => await GlobalSendRequestAsync<HttpResponseWrapper<TC>, TC>(
            Method.Get,
            getRequestBody: request,
            paths: [innerEndpoint]
        );
    
    protected async ValueTask<HttpResponseWrapperList<TC>> RequestCombosUsingQueryRequestAsync<TC>(
        string innerEndpoint,
        QueryRequestBase queryRequestBase)
        where TC : class, new()
    {
        var uri = BuildPath([innerEndpoint]);
        var queryStr = queryRequestBase.ToQueryString();
        var final = new Uri(uri + "?" + queryStr);

        var req = await GlobalSendRequestAsync<HttpResponseWrapperList<TC>, TC>(
            Method.Get, uri: final);

        return req;
    }

    protected async ValueTask<HttpResponseWrapper<TC>> RequestOneUsingQueryRequestAsync<TC>(
        string innerEndpoint,
        QueryRequestBase queryRequestBase)
        where TC : new()
        => await GlobalSendRequestAsync<HttpResponseWrapper<TC>, TC>(
            Method.Get,
            queries: [new("", queryRequestBase.ToQueryString())],
            paths: [innerEndpoint]
        );

    protected async Task<HttpResponseWrapper<object>> SendFinancialRequestAsync(
        string innerEndpoint, RSimpleRequest request)
        => await GlobalSendRequestAsync<HttpResponseWrapper<object>, object>(
            Method.Post,
            postObject: request,
            paths: [innerEndpoint]
        );

    private Uri BuildPath(string?[] paths, bool appendOriginal = true)
    {
        var uri = new Uri(BackendUrl ?? Constants.BackendUrlFallback)
            .AppendPath(appendOriginal ? Endpoint : string.Empty);

        return paths.Aggregate(uri, (current, path) => current.AppendPath(path ?? ""));
    }

    protected async Task<HttpResponseWrapper<SignInResponseModel>?> SignInAsync(SignInRequestModel signInRequest)
    {
        var requestUri = BuildPath(["Signing/signin"]);

        var response = await HttpClient.PostAsJsonAsync(requestUri, signInRequest);
        response.EnsureSuccessStatusCode();

        var responseObject = await response.Content.ReadFromJsonAsync<HttpResponseWrapper<SignInResponseModel>>();

        return responseObject;
    }

    private async ValueTask<TWrapper> GlobalSendRequestAsync<TWrapper, TObject>(
        Method method,
        TObject? postObject = default,
        Uri? uri = null,
        string?[]? paths = null,
        List<Tuple<string, string?>>? queries = null,
        object? getRequestBody = null,
        [CallerMemberName] string caller = "",
        bool appendOriginal = true,
        Func<Task>? onExceptionAction = null)
        where TWrapper : class, IHttpResponseWrapperBase, new()
    {
        ArgumentNullException.ThrowIfNull(HttpClient, "HttpClient configuration required.");
        
        uri ??= (queries ?? []).Aggregate(BuildPath(paths ?? [], appendOriginal),
            (currentUri, query) => !string.IsNullOrEmpty(query.Item2)
                ? currentUri.AppendQueryString(query.Item1, query.Item2)
                : currentUri);

        var request = $"({method.ToString().ToUpper()}: {uri.ToString()}) by {caller}";

        ArgumentNullException.ThrowIfNull(HttpClient, "HttpClient != null");
        Log.Information("Sending request {0}...", request);
        HttpResponseMessage responseMessage;

        Log.Verbose("Executed final {0}, ensuring success response...", request);

        TWrapper? responseObject;

        try
        {
            switch (method)
            {
                case Method.Get:
                {
                    if (getRequestBody is not null)
                    {
                        Log.Verbose("Received GET body for {0} of type: {PD}", request, getRequestBody.GetType());
                        var jsonContent = JsonSerializer.Serialize(getRequestBody);

                        var requestBody = new HttpRequestMessage
                        {
                            RequestUri = uri,
                            Method = HttpMethod.Get,
                            Content = new StringContent(jsonContent, Encoding.UTF8, "application/json")
                        };

                        responseMessage = await HttpClient.SendAsync(requestBody);
                    }
                    else
                    {
                        responseMessage = await HttpClient.GetAsync(uri);
                    }

                    break;
                }
                case Method.Post:
                    responseMessage = await HttpClient.PostAsJsonAsync(uri, postObject);
                    break;
                case Method.Delete:
                    responseMessage = await HttpClient.DeleteAsync(uri);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(method), method, null);
            }
            
            var str = await responseMessage.Content.ReadAsStringAsync();
            
            if (responseMessage.StatusCode == HttpStatusCode.RequestHeaderFieldsTooLarge)
            {
                // Request header fields too large. Restart HttpClient.
                var currToken = HttpClient.DefaultRequestHeaders.Authorization?.Parameter;
                SetClient(new HttpClient(HttpClientHandler), HttpClientHandler);
                AuthenticateRequests(currToken);
                
                Log.Verbose("Restarting HttpClient. Reason: header fields became too large.");
                
                return await GlobalSendRequestAsync<TWrapper, TObject>(method, postObject, uri, paths, queries, getRequestBody,
                    caller, appendOriginal, onExceptionAction);
            }
            
            responseMessage.EnsureSuccessStatusCode();

            responseObject = await responseMessage.Content.ReadFromJsonAsync<TWrapper>();
            Log.Debug("Receiving response object from {0} with deserialized " +
                      "data type: {1}", request, responseObject?.GetType().Name);
        }
        catch (Exception exception)
        {
            throw exception switch
            {
                HttpRequestException req => new LoggedException(req.Message, (int)req.StatusCode, req),
                JsonException json => new LoggedException($"JSON object parsing error: {json.Message}", 400),
                _ => new LoggedException($"Unidentified error: {exception.Message}", 400)
            };
        }

        LoggedException.ThrowIfNull(responseObject, "NoResponseFromServer", 500);

        if (responseObject.StatusCode != 200 && onExceptionAction is not null)
            await onExceptionAction.Invoke();

        switch (responseObject)
        {
            case HttpResponseWrapperList<TObject> list:
            {
                ArgumentNullError(list.ErrorMessage, list.StatusCode);
                break;
            }
            case HttpResponseWrapper<TObject> item:
            {
                ArgumentNullError(item.ErrorMessage, item.StatusCode);
                LoggedException.ThrowIfNull(item.Data, item.ErrorMessage, item.StatusCode);
                break;
            }
        }

        Log.Debug("Request for {0} ({1}) successfully finished with result code: {FD}",
            typeof(TObject).Name, request, responseObject.StatusCode);

        return responseObject!;

        void ArgumentNullError(string? message, int status)
        {
            LoggedException.ThrowIfNotNull(message, $"{message}", status);
        }
    }
}
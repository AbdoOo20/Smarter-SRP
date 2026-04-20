using System.Net;
using System.Text;
using System.Text.Json;
using System.Web;
using static SmartErpWin.Core.Constants;

namespace SmartErpWin.Core.HttpServices;

public static class Requester
{
    private static readonly HttpClient Client;
    
    static Requester()
    {
        Client = new HttpClient
        {
            BaseAddress = new Uri(Config.Get.BackendUrl)
        };
    }

    public static async Task<Request<T>> InternalRequestAsync<T>(string endpoint,
        HttpMethod method,
        params (string, object?)[] content)
    {
        var request = new HttpRequestMessage
        {
            Method = method,
            RequestUri = new Uri(Config.Get.BackendUrl + endpoint)
        };

        foreach (var (key, value) in content)
        {
            var shouldBeEncoded = value.ToString().Contains(Encoded);

            if (key == BodyParam)
            {
                var con = JsonSerializer.Serialize(value, options: new JsonSerializerOptions
                {
                    WriteIndented = true
                });
                var jsonContent = new StringContent(con,
                    Encoding.UTF8, "application/json");
                request.Content = jsonContent;
            }
            else if (!string.IsNullOrEmpty(value?.ToString()))
            {
                request.Headers.Add(key,
                    shouldBeEncoded
                        ? HttpUtility.UrlEncode(value.ToString()
                            .Replace(Encoded, string.Empty))
                        : value.ToString());
            }
        }

        try
        {
            var response = await Client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var responseText = await response.Content.ReadAsStringAsync();

                return new Request<T>(responseText, response.StatusCode, false);
            }

            if (typeof(T) == typeof(string))
            {
                var contentText = await response.Content.ReadAsStringAsync();
                return new Request<T>(contentText, response.StatusCode, true);
            }

            var contentStream = await response.Content.ReadAsStreamAsync();
            var contentTextT = await response.Content.ReadAsStringAsync();
                
            var responseObject = await JsonSerializer.DeserializeAsync<T>(contentStream,
            new JsonSerializerOptions(JsonSerializerDefaults.Web));

            return new Request<T>(responseObject, response.StatusCode, true);
        }
        catch (HttpRequestException x)
        {
            return new Request<T>(x.Message, x.StatusCode ?? HttpStatusCode.InternalServerError, false);
        }
    }
}

public enum Method
{
    Get,
    Post,
    Delete
}
using System.Text.Json.Serialization;

namespace SmartErpWin.Core.ResponseModels;

public class SignInResponseModel
{
    [JsonPropertyName("token")]
    public string? Token { get; set; }
}
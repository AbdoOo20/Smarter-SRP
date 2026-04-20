using System.Text.Json.Serialization;

namespace SmartErpWin.Core.RequestModels;

public class SignInRequestModel
{
    [JsonPropertyName("phone")]
    public required string Phone { get; set; }

    [JsonPropertyName("password")]
    public required string Password { get; set; }
}
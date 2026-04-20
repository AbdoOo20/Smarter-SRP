using System.Text.Json.Serialization;

namespace SmartErpWin.Core.RequestModels;

public class SignUpRequestModel
{
    [JsonPropertyName("username")]
    public required string Username { get; set; }
    
    [JsonPropertyName("password")]
    public required string Password { get; set; }
}
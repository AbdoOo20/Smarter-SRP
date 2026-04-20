using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace SmartErpWin.Core.NativeAoT;

public class NativeAotOptions
{
    public static JsonSerializerOptions JsonOptions = new()
    {
        TypeInfoResolver = new DefaultJsonTypeInfoResolver()
    };
}
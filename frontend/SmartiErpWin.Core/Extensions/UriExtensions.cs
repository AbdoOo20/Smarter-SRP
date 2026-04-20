using System.Reflection;
using System.Web;

namespace SmartErpWin.Core.Extensions;

public static class UriExtensions
{
    public static Uri AppendPath(this Uri uri, string? path)
    {
        ArgumentNullException.ThrowIfNull(uri);

        if (path == null)
        {
            //throw new ArgumentNullException(nameof(path));
            return uri;
        }

        var newPath = uri.AbsolutePath.TrimEnd('/') + "/" + path.TrimStart('/');
        var uriBuilder = new UriBuilder(uri)
        {
            Path = newPath
        };

        return uriBuilder.Uri;
    }

    private static Uri Create(this Uri uri, string queryString)
    {
        var currentQueryString = uri.Query.TrimStart('?');
        var newQueryString = string.IsNullOrEmpty(currentQueryString)
            ? queryString
            : currentQueryString + "&" + queryString + "";
        
        var uriBuilder = new UriBuilder(uri)
        {
            Query = newQueryString
        };

        return uriBuilder.Uri;
    }

    public static Uri AppendQueryString(this Uri uri, string? queryString)
    {
        ArgumentNullException.ThrowIfNull(uri);

        return queryString == null ? uri : Create(uri, queryString);
    }
    
    public static Uri AppendQueryString(this Uri uri, string? queryName, object? value)
    {
        ArgumentNullException.ThrowIfNull(uri);

        if (queryName == null || value == null)
        {
            return uri;
        }

        var queryString = $"&{queryName}={value}";

        return Create(uri, queryString);
    }
    
    public static Uri AppendQueryString(this Uri uri, string? queryName, object[]? value)
    {
        ArgumentNullException.ThrowIfNull(uri);

        if (queryName == null || value == null)
        {
            return uri;
        }

        var queryString = "";
        for (var i = 1; i < value.Length; i++)
            queryString += $"&{queryName}={value[i]}";
        queryString += $"&{queryName}={value[0]}";

        return Create(uri, queryString);
    }

    public static Uri AppendQueryStringArray(this Uri uri, IDictionary<string, object[]>? queryParameters)
    {
        ArgumentNullException.ThrowIfNull(uri);

        if (queryParameters == null || !queryParameters.Any())
        {
            //throw new ArgumentNullException(nameof(queryParameters));
            return uri;
        }

        var currentQueryParameters = HttpUtility.ParseQueryString(uri.Query);

        foreach (var (key, values) in queryParameters)
        {
            foreach (string value in values)
            {
                currentQueryParameters.Add(key, value);
            }
        }

        var uriBuilder = new UriBuilder(uri)
        {
            Query = currentQueryParameters.ToString()
        };

        return uriBuilder.Uri;
    }
}

public static class QueryStringHelper
{
    public static T Deserialize<T>(string queryString) where T : new()
    {
        var queryParameters = HttpUtility.ParseQueryString(queryString);
        var obj = new T();

        foreach (var property in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            var propertyName = property.Name;
            var propertyValue = queryParameters[propertyName];

            if (string.IsNullOrEmpty(propertyValue))
                continue;
            
            var propertyType = property.PropertyType;

            if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                propertyType = Nullable.GetUnderlyingType(propertyType);
            }

            var convertedValue = Convert.ChangeType(propertyValue, propertyType);
            property.SetValue(obj, convertedValue);
        }

        return obj;
    }
    
    public static string ToQueryString(this object obj)
    {
        // Get all properties of the object
        var properties = obj.GetType().GetProperties();

        // Create a list to store the query string parameters
        var parameters = new List<string>();

        // Iterate through each property
        foreach (var property in properties)
        {
            // Get the property value
            var value = property.GetValue(obj);

            // If the value is not null, add it to the query string parameters
            if (value != null)
            {
                // Encode the property name and value
                var encodedName = Uri.EscapeDataString(property.Name);
                var encodedValue = Uri.EscapeDataString(value.ToString());

                // Add the parameter to the list
                parameters.Add($"{encodedName}={encodedValue}");
            }
        }

        // Join the parameters with '&' and return the query string
        return string.Join("&", parameters);
    }
}
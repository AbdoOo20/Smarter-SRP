using CommunityToolkit.Mvvm.ComponentModel;
using Fly.Framework.Shared.Common.Attributes;
using Newtonsoft.Json;


namespace Fly.Framework.Shared.Common.Dtos;

public class DtoBase: ObservableObject, IDto
{
    [ClientInvisible]
    public Identifier Id { get; set; }
    
    [ClientInvisible]
    public DateTime CreateDate { get; set; }
    [ClientInvisible]
    public string? CreatedByName { get; set; }
    //public Guid Guid { get; set; }

    [ClientInvisible]
    public string? RequestLocale { get; set; }
    
    public bool IsNew() => Id == Identifier.Empty;

    public override string ToString()
    {

        #region old
        /*
        StringBuilder stringBuilder = new StringBuilder();
        var prps = GetType().GetProperties();
        foreach (var p in prps)
        {
            stringBuilder.AppendLine();
            string vl = "UNKown";
            try
            {
                vl = p.GetValue(this)?.ToString() ?? "NULL";
            }
            catch { }
            stringBuilder.AppendFormat("{0}={1}", p.Name, vl);
        }
        return stringBuilder.ToString();
        */
        #endregion
        var settings = new JsonSerializerSettings {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
        return JsonConvert.SerializeObject(this, Formatting.Indented, settings);

    }

    // After 70 hours of debugging, this is the function that is breaking
    // the DataGrid selection logic because it overrides temporary ID matching.
    
    public override bool Equals(object? obj)
    {
        if (obj is DtoBase dto && !dto.Id.IsNullOrEmpty()) // to fix, add a condition that the Id must not be
                                                           // null to make a custom Equals match
        {
            return this.Id == dto.Id;
        }
        
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
    
    [ClientInvisible]
    public Identifier TenantId { get; set; }
}

public interface IDto
{
    public Identifier Id { get; set; }
}
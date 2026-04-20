using Fly.Framework.Shared.Common.Requests;

namespace Fly.Shared.Modules.Personnel.Requests;

public class DepartmentQueryRequest : QueryRequestBase
{
    public string Name { get; set; } = string.Empty;
}
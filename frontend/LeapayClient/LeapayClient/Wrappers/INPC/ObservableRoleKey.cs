using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.Global.Dtos;

namespace Leapay.Client.Wrappers.INPC;

public partial class ObservableRoleKey: ObservableObject, IRoleKey
{
    public Identifier Id { get; set; }

    [ObservableProperty]
    public partial string? KeyName { get; set; }

    [ObservableProperty]
    public partial bool IsChecked { get; set; }
}
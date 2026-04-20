using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.Global.Dtos;

namespace SmartiErpWin.Client.Wrappers.INPC;

public partial class ObservableRoleKey: ObservableObject, IRoleKey
{
    public Identifier Id { get; set; }
    
    [ObservableProperty]
    private string? _keyName;

    [ObservableProperty]
    private bool _isChecked;
}
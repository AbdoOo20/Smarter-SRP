using Fly.Shared.Modules.Global.Dtos;

namespace SmartiErpWin.Client.Helpers;

public static class IdentityHelper
{
    public static Visibility DetermineVisibility(DtoUser user, string name)
    {
        return Visibility.Visible;
    }
}
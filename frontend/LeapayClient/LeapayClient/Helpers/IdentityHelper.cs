using Fly.Shared.Modules.Global.Dtos;

namespace Leapay.Client.Helpers;

public static class IdentityHelper
{
    public static bool DetermineVisibility(DtoUser user, string name)
    {
        return true;
    }
}
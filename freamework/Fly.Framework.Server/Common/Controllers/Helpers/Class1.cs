using Fly.Framework.Server.Security.Algorithms;
using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Modules.Identity.Dtos;
using System.Security.Claims;

namespace Fly.Framework.Server.Common.Controllers.Helpers;

public class ClaimsHelper
{
    private readonly ClaimsPrincipal _user;
    private readonly ICustomEncryptionAlgorithm _algorithm;

    public ClaimsHelper(ICustomEncryptionAlgorithm algorithm, ClaimsPrincipal user)
    {
        _user = user;
        _algorithm = algorithm;
    }

        
    public Identifier GetUserId()
    {
        var encryptedUserId = _user.FindFirst(CustomClaims.Identifier)!.Value;
        var userId = _algorithm.Decrypt(encryptedUserId);
        return Identifier.Parse(userId);
    }

    public Identifier GetId(string claimType)
    {
        var encryptedUserId = _user.FindFirst(claimType)!.Value;
        var userId = _algorithm.Decrypt(encryptedUserId);
        return Identifier.Parse(userId);
    }
        
    public string GetPhone()
    {
        var encryptedUserId = _user.FindFirst(CustomClaims.Phone)!.Value;
        var phone = _algorithm.Decrypt(encryptedUserId);
        return phone;
    }
        
    public string GetAccesser()
    {
        var encryptedUserId = _user.FindFirst(CustomClaims.Accesser)!.Value;
        var accesser = _algorithm.Decrypt(encryptedUserId);
        return accesser;
    }
}
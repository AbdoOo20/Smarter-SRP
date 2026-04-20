using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Fly.Framework.Server.Common.Helpers;
using Fly.Framework.Server.Constsnts;
using Fly.Framework.Server.Security.Algorithms;
using Fly.Framework.Shared.Helpers;
using Fly.Framework.Shared.Modules.Identity.Dtos;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Fly.Framework.Server.Authentications;

public class JwtTokenHelper(IConfiguration configuration, ICustomEncryptionAlgorithm customEncryptionAlgorithm)
{
    
    private readonly IConfiguration _configuration = configuration;
    private readonly ICustomEncryptionAlgorithm _customEncryptionAlgorithm = customEncryptionAlgorithm;
    
    public string CreateToken0<TSubs>(TSubs user)
        where TSubs : DtoUserBase
    {

        var tokenKey = _configuration.GetValue<string>("Token:Key");
        var tokenIssuer = _configuration.GetValue<string>("Token:Issuer");
        var tokenExpire = _configuration.GetValue<int>("Token:Expire");
        var tokenAudience = _configuration.GetValue<string>("Token:Audience");
        var dataEncyptionKey = DateTime.UtcNow.ToString("fffffffHHssddMMyy");
        if (string.IsNullOrEmpty(tokenKey) || string.IsNullOrEmpty(tokenIssuer) || tokenExpire == 0)
            throw new ArgumentNullException("Token Credentials Most Be Provided!");

        var id = _customEncryptionAlgorithm.Encrypt(user.Id.ToString());
        var isAdmin = _customEncryptionAlgorithm.Encrypt(user.IsAdmin.ToString());
        var isGhost = _customEncryptionAlgorithm.Encrypt(user.IsGhost.ToString());

        var claims = new List<Claim>
        {
              new(CustomClaims.Identifier, id)
            , new(CustomClaims.IsAdmin, isAdmin)
            , new(CustomClaims.IsGhost, isGhost)
            , new(CustomClaims.EncryptKey, dataEncyptionKey)
        };

        if (user.Roles.Any()) claims.AddRange(user.Roles.Select(s => new Claim(ClaimTypes.Role, s.Key.ToString())));
        //else claims.Add(new Claim(ClaimTypes.Role, RoleEnum.None.ToString()));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        //  TODO: Check UTC
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DatetimeHelpers.GetUTC().AddHours(tokenExpire),
            signingCredentials: creds,
            issuer: tokenIssuer,
            audience: tokenAudience
        );
        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return jwt;
    }
    
    public string? CreateToken<TSubs>(TSubs user, Action<List<Claim>, ICustomEncryptionAlgorithm>? action = null)
        where TSubs : DtoUserBase
    {
        var reqestTime = DateTime.UtcNow;
        var tokenKey = DataHelper.Shutdown(configuration.GetValue<string>("Token:Key"));
        var tokenIssuer = _configuration.GetValue<string>("Token:Issuer");
        var tokenExpire = _configuration.GetValue<int>("Token:Expire");
        var tokenAudience = _configuration.GetValue<string>("Token:Audience");
        var dataEncyptionKey = long.Parse(reqestTime.ToString(DatetimeConstants.DataFingerprint)).ToString("D16");

        if (string.IsNullOrEmpty(tokenKey) || string.IsNullOrEmpty(tokenIssuer) || tokenExpire == 0)
            throw new ArgumentNullException("Token Credentials Most Be Provided!");

        _customEncryptionAlgorithm.SetKey(dataEncyptionKey);

        var id = _customEncryptionAlgorithm.Encrypt(user.Id.ToString());
        var isAdmin = _customEncryptionAlgorithm.Encrypt(user.IsAdmin.ToString());
        var isGhost = _customEncryptionAlgorithm.Encrypt(user.IsGhost.ToString());
        var phone = _customEncryptionAlgorithm.Encrypt(user.Phone.ToString());
        var accesser = _customEncryptionAlgorithm.Encrypt(user.Accesser.ToString());

        var claims = new List<Claim>
        {
                new(CustomClaims.Identifier, id)
            ,   new(CustomClaims.Phone, phone)
            ,   new(CustomClaims.IsAdmin, isAdmin)
            ,   new(CustomClaims.IsGhost, isGhost)
            ,   new(CustomClaims.Accesser, accesser)
            ,   new(CustomClaims.ReqestTime, reqestTime.ToBinary().ToString())
        };

        if(action is not null) action(claims, _customEncryptionAlgorithm);

        if (user.Roles.Any()) claims.AddRange(user.Roles.Select(s => new Claim(ClaimTypes.Role, s.Key.ToString())));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        //  TODO: Check UTC
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DatetimeHelpers.GetUTC().AddHours(tokenExpire),
            signingCredentials: creds,
            issuer: tokenIssuer,
            audience: tokenAudience
        );
        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return jwt;
    }

    public string? CreateToken<TSubs>(TSubs user, Func<TSubs, DateTime, ICustomEncryptionAlgorithm, List<Claim>> action)
        where TSubs : class
    {
        var reqestTime = DateTime.UtcNow;
        var tokenKey = _configuration.GetValue<string>("Token:Key");
        var tokenIssuer = _configuration.GetValue<string>("Token:Issuer");
        var tokenExpire = _configuration.GetValue<int>("Token:Expire");
        var tokenAudience = _configuration.GetValue<string>("Token:Audience");

        if (string.IsNullOrEmpty(tokenKey) || string.IsNullOrEmpty(tokenIssuer) || tokenExpire == 0)
            throw new ArgumentNullException("Token Credentials Most Be Provided!");

        var dataEncyptionKey = long.Parse(reqestTime.ToString(DatetimeConstants.DataFingerprint)).ToString("D16");
        _customEncryptionAlgorithm.SetKey(dataEncyptionKey);

        var claims = action(user, reqestTime, _customEncryptionAlgorithm);

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        //  TODO: Check UTC
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DatetimeHelpers.GetUTC().AddHours(tokenExpire),
            signingCredentials: creds,
            issuer: tokenIssuer,
            audience: tokenAudience
        );
        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return jwt;
    }
}
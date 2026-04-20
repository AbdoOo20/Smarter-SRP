using System.Text;
using Fly.Framework.Server.Common.Helpers;
using Fly.Shared.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Fly.Framework.Server.Authentications;

public static class AuthenticationStartup
{
    public static IServiceCollection InjectAuthentication
        (this IServiceCollection services, IConfiguration configuration)
    {
        services.InjectJWT(configuration);
        services.AddScoped<JwtTokenHelper>();
        return services;
    }

    public static WebApplication UseAppAuthentication(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
        return app;
    }

    private static void InjectJWT(this IServiceCollection services, IConfiguration configuration)
    {
        var tokenKey = DataHelper.Shutdown(configuration.GetValue<string>("Token:Key"));
        var tokenIssuer = configuration.GetValue<string>("Token:Issuer");
        var tokenExpire = configuration.GetValue<int>("Token:Expire");
        var tokenAudience = configuration.GetValue<string>("Token:Audience");
        if (string.IsNullOrEmpty(tokenKey)) throw new ArgumentNullException(nameof(tokenKey) + " token key must be provided!");
        //---------------------------------------------------------------
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = tokenIssuer,
                    ValidAudience = tokenAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
                };
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            context.Response.ContentType = "application/json";
                            context.Response.WriteAsync("{\"error\":\"Token expired\"}");
                        }
                        return Task.CompletedTask;
                    }
                };
            })
            /*.AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = tokenIssuer,
                    ValidAudience = tokenAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(SharedConstants.DiscoveryConfig.SharedDiscoveryConfig),
                };
            })*/;
    }
}
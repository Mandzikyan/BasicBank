using BL.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebAPI.Domain;
using System.IdentityModel.Tokens.Jwt;
using WebAPI.Token;
using System.Security.Claims;
namespace WebAPI
{
    public static class TokenExtension
    {
        public static IServiceCollection AddTokenDocumentation(this IServiceCollection services, WebApplicationBuilder builder)

        {
            services.AddAuthentication(options =>
             {
                 options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                 options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                 options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
             })
        .AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = builder.Configuration["JWT:Issuer"],
                ValidAudience = builder.Configuration["JWT:Audience"],
                RequireExpirationTime = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
            };
            options.Events = new JwtBearerEvents
            {
                OnTokenValidated = context =>
                {
                    // Retrieve the validated token from the context
                    var accessToken = context.SecurityToken as JwtSecurityToken;
                    if (accessToken == null)
                    {
                        context.Fail("Unauthorized");
                    }
                    else
                    {
                        var tokenValidationParameters = options.TokenValidationParameters;
                        try
                        {
                            var principal = new JwtSecurityTokenHandler().ValidateToken(
                                accessToken.RawData, tokenValidationParameters, out var securityToken);

                            context.Principal = principal;
                            context.Success();
                        }
                        catch (Exception ex)
                        {
                            if (ex is not SecurityTokenExpiredException)
                            {
                                var tokenBL = context.HttpContext.RequestServices.GetRequiredService<TokenBL>();
                                try
                                {
                                    var user = tokenBL.GetTokens(context.Principal.FindFirst(ClaimTypes.Name).Value);
                                    if (user is null)
                                        throw new Exception("Revoke failed!"); ;
                                    user.RefreshToken = null;
                                    tokenBL.Save();
                                }
                                catch (Exception)
                                {
                                    throw new Exception("Revoke failed!");
                                }
                            }
                        }
                    }
                    return Task.CompletedTask;
                }
            };
        });
            return services;
        }
        public static IApplicationBuilder UseTokenDocumentation(IApplicationBuilder app, TokenService tokenService)
        {
            app.Use(async (context, next) =>
            {
                try
                {
                    await next();
                }
                catch (Exception ex)
                {
                    if (ex is SecurityTokenExpiredException)
                    {
                        RefreshTokenRequest tokenApiModel = new RefreshTokenRequest
                        {
                            AccessToken = context.Request.Headers["AccessToken"].FirstOrDefault(),
                            RefreshToken = context.Request.Headers["RefreshToken"].FirstOrDefault()
                        };
                        if (!string.IsNullOrEmpty(tokenApiModel.AccessToken) && !string.IsNullOrEmpty(tokenApiModel.RefreshToken))
                        {
                            var newtokenPair = await tokenService.Refresh(tokenApiModel);
                            if (!string.IsNullOrEmpty(newtokenPair.ToString()))
                            {
                                var httpResponse = context.Response;
                                httpResponse.Headers["AccessToken"] = newtokenPair.AccessToken;
                                httpResponse.Headers["RefreshToken"] = newtokenPair.RefreshToken;
                                httpResponse.Headers["Access-Control-Expose-Headers"] = "AccessToken, RefreshToken";
                            }
                        }
                    }
                }
            });
            return app;
        }
    } 
}
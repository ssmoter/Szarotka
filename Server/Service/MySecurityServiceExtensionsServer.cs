using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

using Server.Model;

using System.Text;

namespace Server.Service
{
    public static class MySecurityServiceExtensionsServer
    {
        public static void AddSecurityServicesServer(this IServiceCollection services,
                                            IConfiguration configuration)
        {
            services.Configure<JSONWebTokensSettings>
                (configuration.GetSection("JSONWebTokensSettings"));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                var config = new JSONWebTokensSettings(
                    configuration["JSONWebTokensSettings:Key"],
                    configuration["JSONWebTokensSettings:Issuer"],
                    configuration["JSONWebTokensSettings:Audience"],
                    configuration["JSONWebTokensSettings:DurationInMinutes"]
                    );

                var bytes = Encoding.UTF8.GetBytes(config.Key);
                o.RequireHttpsMetadata = false;
                o.SaveToken = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = config.Issuer,
                    ValidAudience = config.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(bytes)
                };

                o.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = c =>
                    {
                        c.NoResult();
                        c.Response.StatusCode = 500;
                        c.Response.ContentType = "text/plain";
                        return c.Response.WriteAsync(c.Exception.ToString());
                    },
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        var result = "401 Not authorized";
                        return context.Response.WriteAsync(result);
                    },
                    OnForbidden = context =>
                    {
                        context.Response.StatusCode = 403;
                        context.Response.ContentType = "application/json";
                        var result = "403 Not authorized";
                        return context.Response.WriteAsync(result);
                    },
                };
            });
        }


    }
}

using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using WebApiIdentityServer.BusinessLayer.Settings;

namespace WebApiIdentityServer.BusinessLayer.Services
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAuthenticationOptions(this IServiceCollection services, IConfiguration configuration)
        {
            
            T Configure<T>(string sectionName) where T : class
            {
                var section = configuration.GetSection(sectionName);
                var settings = section.Get<T>();
                services.Configure<T>(section);
                return settings;
            }

            var jwtSettings = Configure<JwtSettings>(nameof(JwtSettings));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtSettings.Audience,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecurityKey)),
                    RequireExpirationTime = true,
                    ClockSkew = TimeSpan.Zero
                };

            });
            return services;
        }
    }

}

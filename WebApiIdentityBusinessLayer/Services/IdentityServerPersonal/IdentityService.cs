using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApiIdentityServer.BusinessLayer.Settings;
using WebApiIdentityServer.Shared.Models;

namespace WebApiIdentityServer.BusinessLayer.Services.IdentityServerPersonal
{
    public class IdentityService : IIdentityService
    {
        private readonly JwtSettings jwtSettings;

        public IdentityService(IOptions<JwtSettings> jwtSettingsOptions)
        {
            this.jwtSettings = jwtSettingsOptions.Value;
        }
        public Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            if (request.UserName == request.Password)
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Name, request.UserName),
                    new Claim(ClaimTypes.Role, "Administrator"),
                    new Claim(ClaimTypes.Role, "PowerUser")
                };

                var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecurityKey));

                var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

                var jwtSecurityToken = new JwtSecurityToken(jwtSettings.Issuer, jwtSettings.Audience, claims,
                        DateTime.UtcNow, DateTime.UtcNow.AddDays(10), signingCredentials);

                var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

                var response = new AuthResponse { AccessToken = accessToken };

                return Task.FromResult(response);
            }
            return Task.FromResult<AuthResponse>(null);
        }
    }
}

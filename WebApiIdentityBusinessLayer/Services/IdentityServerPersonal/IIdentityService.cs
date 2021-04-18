using System.Threading.Tasks;
using WebApiIdentityServer.Shared.Models;

namespace WebApiIdentityServer.BusinessLayer.Services.IdentityServerPersonal
{
    public interface IIdentityService
    {
        Task<AuthResponse> LoginAsync(LoginRequest request);
    }
}
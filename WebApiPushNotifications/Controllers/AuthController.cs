using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiIdentityServer.BusinessLayer.Services.IdentityServerPersonal;
using WebApiIdentityServer.Shared.Models;

namespace WebApiPushNotifications.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly ILogger<AuthController> _logger;
        private readonly IIdentityService _identityservice;

        public AuthController(IIdentityService identityservice, ILogger<AuthController> logger)
        {
            this._logger = logger;
            this._identityservice = identityservice;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginRequest request)
        {
            var response = await this._identityservice.LoginAsync(request);

            if(response != null)
            {
                return Ok(response);
            }
            return BadRequest();
        }

    }
}

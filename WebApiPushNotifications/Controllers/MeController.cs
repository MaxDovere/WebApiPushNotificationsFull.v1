using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApiPushNotifications.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeController : ControllerBase
    {
        public MeController()
        {         
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public IActionResult GetMe()
        {
            return Ok(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }
    }
}

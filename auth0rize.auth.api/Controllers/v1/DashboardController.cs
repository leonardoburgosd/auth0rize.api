using auth0rize.auth.application.Features.User.Queries.UserGetCounter;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace auth0rize.auth.api.Controllers.v1
{
    [Asp.Versioning.ApiVersion("1.0")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DashboardController : BaseApiController
    {
        [HttpGet("counter")]
        public async Task<IActionResult> counter()
        {
            return Ok(await Mediator.Send(new UserGetCounter()));
        }
    }
}

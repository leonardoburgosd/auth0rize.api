using auth0rize.auth.application.Features.Role.Queries.RoleGet;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace auth0rize.auth.api.Controllers.v1
{
    [ApiVersion("1.0")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TypeController : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> get()
        {
            return Ok(await Mediator.Send(new TypeGet()));
        }
    }
}

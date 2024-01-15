using Asp.Versioning;
using auth0rize.auth.application.Features.Autentication.Queries.Login;
using Microsoft.AspNetCore.Mvc;

namespace auth0rize.auth.api.Controllers.v1
{
    [ApiVersion("1.0")]
    [ApiController]
    public class AuthController : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> login([FromBody] LoginRequest login)
        {
            return Ok(await Mediator.Send(new LoginQuery(login.UserName, login.Password, login.Application)));
        }
    }
}

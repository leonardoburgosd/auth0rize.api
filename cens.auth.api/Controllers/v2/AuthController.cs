using cens.auth.application.Features.Authentication.Queries.Login;
using Microsoft.AspNetCore.Mvc;

namespace cens.auth.api.Controllers.v2
{
    [ApiVersion("2.0")]
    public class AuthController : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> login([FromBody] LoginRequest login)
        {
            return Ok(await Mediator.Send(new LoginQuery(login.UserName, login.Password, login.Key)));
        }
    }
}
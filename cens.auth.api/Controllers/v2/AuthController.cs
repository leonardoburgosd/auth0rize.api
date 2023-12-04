using cens.auth.application.Features.Authentication.Queries.BasicUser;
using cens.auth.application.Features.Authentication.Queries.Login;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace cens.auth.api.Controllers.v2
{
    [ApiVersion("2.0")]
    [EnableCors("authenticationPolicy")]
    public class AuthController : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> login([FromBody] LoginRequest login)
        {
            return Ok(await Mediator.Send(new LoginQuery(login.UserName, login.Password, login.Key)));
        }

        [HttpPost("user")]
        public async Task<IActionResult> basicUser([FromBody] BasicUserRequest basicUser)
        {
            return Ok(await Mediator.Send(new BasicUserQuery(basicUser.UserName, basicUser.Key)));
        }

        [HttpPost("renewpassword")]
        public async Task<IActionResult> renewPassword()
        {
            return Ok("renovado");
        }
    }
}
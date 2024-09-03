using Asp.Versioning;
using auth0rize.auth.application.Features.Autentication.Queries.BasicUser;
using auth0rize.auth.application.Features.Autentication.Queries.Login;
using auth0rize.auth.application.Features.Autentication.Queries.RecoveryByEmail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace auth0rize.auth.api.Controllers.v1
{
    [ApiVersion("1.0")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> login([FromBody] LoginRequest login)
        {
            return Ok(await Mediator.Send(new LoginQuery(login.UserName, login.Password, login.Application)));
        }

        [HttpPost("user")]
        public async Task<IActionResult> validationUser([FromBody] BasicUserRequest user)
        {
            return Ok(await Mediator.Send(new BasicUserQuery(user.UserName, user.Application)));
        }

        [HttpPost("recovery-by-email")]
        public async Task<IActionResult> recoveryByEmail([FromBody] RecoveryByEmailRequest recoveryByEmail)
        {
            return Ok(await Mediator.Send(new RecoveryByEmailQuery(recoveryByEmail.Email)));
        }

        [HttpPost("recovery-by-phonenumber")]
        public async Task<IActionResult> recoveryByPhoneNumber([FromBody] RecoveryByEmailRequest recoveryByEmail)
        {
            return Ok(await Mediator.Send(new RecoveryByEmailQuery(recoveryByEmail.Email)));
        }
    }
}

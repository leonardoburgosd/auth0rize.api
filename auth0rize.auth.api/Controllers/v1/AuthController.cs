using Asp.Versioning;
using auth0rize.auth.application.Features.Autentication.Command.ConfirmAccount;
using auth0rize.auth.application.Features.Autentication.Command.Register2fa;
using auth0rize.auth.application.Features.Autentication.Command.Verification2fa;
using auth0rize.auth.application.Features.Autentication.Queries.Login;
<<<<<<< Updated upstream
<<<<<<< Updated upstream
=======
=======
>>>>>>> Stashed changes
using Microsoft.AspNetCore.Authorization;
>>>>>>> Stashed changes
using Microsoft.AspNetCore.Mvc;

namespace auth0rize.auth.api.Controllers.v1
{
    [ApiVersion("1.0")]
    [ApiController]
    public class AuthController : BaseApiController
    {
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> login([FromBody] LoginRequest login)
        {
            return Ok(await Mediator.Send(new LoginQuery(login.UserName, login.Password)));
        }

        [HttpPost("confirm-account")]
        public async Task<IActionResult> confirmAccount()
        {
            return Ok(await Mediator.Send(new ConfirmAccount()));
<<<<<<< Updated upstream
        }
<<<<<<< Updated upstream
=======

        [HttpPost("register-2fa")]
        public async Task<IActionResult> register2FA()
        {
            return Ok(await Mediator.Send(new Register2fa()));
        }

        [HttpPost("verification-2fa")]
        public async Task<IActionResult> Verification2FA()
        {
=======
        }

        [HttpPost("register-2fa")]
        public async Task<IActionResult> register2FA()
        {
            return Ok(await Mediator.Send(new Register2fa()));
        }

        [HttpPost("verification-2fa")]
        public async Task<IActionResult> Verification2FA()
        {
>>>>>>> Stashed changes
            return Ok(await Mediator.Send(new Verification2fa()));
        }
>>>>>>> Stashed changes
    }
}

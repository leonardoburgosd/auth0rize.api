using auth0rize.auth.application.Features.Autentication.Command.ConfirmAccount;
using auth0rize.auth.application.Features.Autentication.Command.Register2fa;
using auth0rize.auth.application.Features.Autentication.Command.Verification2fa;
using auth0rize.auth.application.Features.Autentication.Queries.Login;
using auth0rize.auth.application.Features.User.Queries.UserVerification;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace auth0rize.auth.api.Controllers.v1
{
    [Asp.Versioning.ApiVersion("1.0")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AuthController : BaseApiController
    {
        [HttpPost("user")]
        [AllowAnonymous]
        public async Task<IActionResult> userVerification([FromBody] UserVerificationRequest user)
        {
            return Ok(await Mediator.Send(new UserVerification(user.UserName)));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> login([FromBody] LoginRequest login)
        {
            return Ok(await Mediator.Send(new LoginQuery(login.UserName, login.Password)));
        }

        [HttpPost("confirm-account")]
        public async Task<IActionResult> confirmAccount([FromBody] ConfirmAccountRequest confirmAccount)
        {
            return Ok(await Mediator.Send(new ConfirmAccount(confirmAccount.Code)));
        }

        [HttpPost("register-2fa")]
        public async Task<IActionResult> register2FA()
        {
            return Ok(await Mediator.Send(new Register2fa()));
        }

        [HttpPost("verification-2fa")]
        public async Task<IActionResult> Verification2FA()
        {
            return Ok(await Mediator.Send(new Verification2fa()));
        }

        
    }
}

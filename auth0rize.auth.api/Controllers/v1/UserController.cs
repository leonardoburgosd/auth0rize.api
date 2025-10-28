using auth0rize.auth.application.Features.User.Command.FirstAdminCreate;
using auth0rize.auth.application.Features.User.Command.UserCreate;
using auth0rize.auth.application.Features.User.Command.VerificationAccount;
using auth0rize.auth.application.Features.User.Command.VerificationUser;
using auth0rize.auth.application.Features.User.Queries.UserGet;
using auth0rize.auth.application.Features.User.Queries.UserNameVerification;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace auth0rize.auth.api.Controllers.v1
{
    [ApiVersion("1.0")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : BaseApiController
    {

        [HttpPost("first-register")]
        [AllowAnonymous]
        public async Task<IActionResult> firstRegister([FromBody] FirstAdminCreateRequest user)
        {
            return Ok(await Mediator.Send(new FirstAdminCreate(user.Name, user.MotherLastName, user.LastName, user.UserName, user.Email, user.Password)));
        }

        [HttpPost]
        public async Task<IActionResult> userRegister([FromBody] UserCreateRequest user)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "user_id");
            int userId = 0;
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int parsedId))
            {
                userId = parsedId;
            }
            return Ok(await Mediator.Send(new UserCreate(user.Name, user.MotherLastName, user.LastName, user.UserName, user.Email, user.Password, user.TypeUserId, user.DomainId, userId)));
        }

        [HttpGet("verification")]
        [AllowAnonymous]
        public async Task<IActionResult> verification()
        {
            return Ok(await Mediator.Send(new VerificationUser()));
        }

        [HttpGet("verification/{userName}")]
        public async Task<IActionResult> verificationUserName(string userName)
        {
            return Ok(await Mediator.Send(new UserNameVerification(userName)));
        }

        //Me sirve para poder activar la cuenta del usuario
        [HttpPost("verification-account")]
        [AllowAnonymous]
        public async Task<IActionResult> verificationAccount([FromBody] VerificationAccountRequest account, [FromHeader(Name = "complement")] string complementToken)
        {
            return Ok(await Mediator.Send(new VerificationAccount(account.UserName, complementToken)));
        }

        [HttpGet]
        public async Task<IActionResult> get(string? search, string? type, bool? deleted, int page = 1, int size = 10)
        {
            return Ok(await Mediator.Send(new UserGet(search, type, deleted, page, size)));
        }

    }
}

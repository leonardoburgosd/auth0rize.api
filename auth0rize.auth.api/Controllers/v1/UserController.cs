using auth0rize.auth.application.Features.User.Command.FirstAdminCreate;
using auth0rize.auth.application.Features.User.Command.UserCreate;
using auth0rize.auth.application.Features.User.Command.VerificationUser;
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
            int userId = 0;
            return Ok(await Mediator.Send(new UserCreate(user.Name, user.MotherLastName, user.LastName, user.UserName, user.Email, user.Password, user.TypeUserId, user.DomainId, userId)));
        }

        [HttpGet("verification")]
        [AllowAnonymous]
        public async Task<IActionResult> verification()
        {
            return Ok(await Mediator.Send(new VerificationUser()));
        }
    }
}

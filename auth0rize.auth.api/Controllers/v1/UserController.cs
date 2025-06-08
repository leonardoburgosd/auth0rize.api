using Microsoft.AspNetCore.Authentication.JwtBearer;
using auth0rize.auth.application.Features.User.Command.FirstAdminCreate;
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
        public async Task<IActionResult> login([FromBody] FirstAdminCreateRequest user)
        {
            return Ok(await Mediator.Send(new FirstAdminCreate(user.Name, user.MotherLastName, user.LastName, user.UserName, user.Email, user.Password)));
        }
    }
}

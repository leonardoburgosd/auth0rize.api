using cens.auth.application.Features.Authentication.Queries.Login;
using Microsoft.AspNetCore.Mvc;

namespace cens.auth.api.Controllers.v1
{
    [ApiVersion("1.0")]
    public class AuthController : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> login([FromBody] LoginRequest login)
        {
            LoginQuery loginQuery = new LoginQuery() { Key = login.Key, Password = login.Password, UserName = login.UserName };

            return Ok(await Mediator.Send(loginQuery));
        }
    }
}

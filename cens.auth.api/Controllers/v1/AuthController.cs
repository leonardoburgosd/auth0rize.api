using cens.auth.application.Features.Authentication.Queries.Login;
using cens.auth.application.Features.Authentication.Queries.SessionExist;
using Microsoft.AspNetCore.Mvc;

namespace cens.auth.api.Controllers.v1
{
    [ApiVersion("1.0")]
    public class AuthController : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> login(string userName, string password, string key/*[FromBody] LoginRequest login*/)
        {
            LoginQuery loginQuery = new LoginQuery() { Key = key, Password = password, UserName = userName, HttpContext = HttpContext };
            //LoginQuery loginQuery = new LoginQuery() { Key = login.Key, Password = login.Password, UserName = login.UserName, HttpContext = HttpContext };

            return Ok(await Mediator.Send(loginQuery));
        }

        [HttpGet]
        [Route("sessionExist")]
        public async Task<IActionResult> sessionExist()
        {
            return Ok(await Mediator.Send(new SessionExistQuery() { HttpContext = HttpContext }));
        }

        [HttpGet]
        [Route("sessionClose")]
        public async Task<IActionResult> sessionClose(string key, string redirecturl)
        {
            HttpContext.Session.Clear();
            return Redirect($"https://auth.censperu.com/?secretkey={key}&redirecturl={redirecturl}");
        }
    }
}

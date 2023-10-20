using cens.auth.application.Common.Entities;
using cens.auth.application.Features.User.Command.CreateUser;
using cens.auth.application.Features.User.Queries.GetUser;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cens.auth.api.Controllers.v2
{
    [ApiVersion("2.0")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> create([FromBody] CreateUserRequest user)
        {
            return Ok(await Mediator.Send(new CreateUserCommand(user.Name, user.LastName, user.MotherLastName,
                                                                user.Birthday, user.UserName, user.Password, user.Email,
                                                                new SecurityTokenData()
                                                                {
                                                                    UserId = Convert.ToInt32(HttpContext.User.Claims.First(c => c.Type == "user_id").Value),
                                                                    UserName = HttpContext.User.Claims.First(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value,
                                                                    TypeUser = HttpContext.User.Claims.First(c => c.Type == "user_rol").Value
                                                                })
                                                                ));
        }

        [HttpGet]
        public async Task<IActionResult> get()
        {
            return Ok(await Mediator.Send(new GetUserQuery(new SecurityTokenData()
            {
                UserId = Convert.ToInt32(HttpContext.User.Claims.First(c => c.Type == "user_id").Value),
                UserName = HttpContext.User.Claims.First(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value,
                TypeUser = HttpContext.User.Claims.First(c => c.Type == "user_rol").Value
            }
            )));
        }

        [HttpPut]
        [Route("{userId}")]
        public async Task<IActionResult> update()
        {
            return Ok();
        }

        [HttpPost]
        [Route("{userId}")]
        public async Task<IActionResult> delete(int userId)
        {
            return Ok();
        }
    }
}
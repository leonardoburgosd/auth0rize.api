using cens.auth.application.Common.Entities;
using cens.auth.application.Features.User.Command.CreateUser;
using cens.auth.application.Features.User.Command.GeneratePassword;
using cens.auth.application.Features.User.Delete.DeleteUser;
using cens.auth.application.Features.User.Queries.GetUser;
using cens.auth.application.Features.User.Update.UpdateUser;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace cens.auth.api.Controllers.v2
{
    [ApiVersion("2.0")]
    [EnableCors("consultingPolicy")]
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
        public async Task<IActionResult> update([FromBody] UpdateUserRequest user, int userId)
        {
            return Ok(await Mediator.Send(new UpdateUser(userId, user.Name, user.LastName, user.MotherLastName, user.Birthday, user.UserName, user.Password, user.Email,
                                                                new SecurityTokenData()
                                                                {
                                                                    UserId = Convert.ToInt32(HttpContext.User.Claims.First(c => c.Type == "user_id").Value),
                                                                    UserName = HttpContext.User.Claims.First(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value,
                                                                    TypeUser = HttpContext.User.Claims.First(c => c.Type == "user_rol").Value
                                                                })));
        }

        [HttpDelete]
        [Route("{userId}")]
        public async Task<IActionResult> delete(int userId)
        {
            return Ok(await Mediator.Send(new DeleteUser(userId,
                                                                new SecurityTokenData()
                                                                {
                                                                    UserId = Convert.ToInt32(HttpContext.User.Claims.First(c => c.Type == "user_id").Value),
                                                                    UserName = HttpContext.User.Claims.First(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value,
                                                                    TypeUser = HttpContext.User.Claims.First(c => c.Type == "user_rol").Value
                                                                })));
        }

        [HttpGet]
        [Route("newPassword")]
        public async Task<IActionResult> getRandomPassword()
        {
            return Ok(await Mediator.Send(new GeneratePasswordCommand()));
        }
    }
}
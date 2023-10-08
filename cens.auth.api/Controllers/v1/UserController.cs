using cens.auth.application.Common.Entities;
using cens.auth.application.Features.User.Command.CreateUser;
using cens.auth.application.Features.User.Queries.GetUser;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cens.auth.api.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> create([FromBody] CreateUserRequest create)
        {
            return Ok(await Mediator.Send(new CreateUserCommand()
            {
                Birthday = create.Birthday,
                Email = create.Email,
                MotherLastName = create.MotherLastName,
                LastName = create.LastName,
                Name = create.Name,
                Password = create.Password,
                UserName = create.UserName,
                SecurityTokenData = new SecurityTokenData()
                {
                    UserId = Convert.ToInt32(HttpContext.User.Claims.First(c => c.Type == "UserId").Value),
                    UserName = HttpContext.User.Claims.First(c => c.Type == "unique_name").Value,
                    TypeUser = HttpContext.User.Claims.First(c => c.Type == "UserRol").Value
                }
            }));
        }

        [HttpGet]
        public async Task<IActionResult> get()
        {
            return Ok(await Mediator.Send(new GetUserQuery()));
        }
    }
}

using Asp.Versioning;
using auth0rize.auth.application.Common.Entities;
using auth0rize.auth.application.Features.User.Command.UserRegister;
using auth0rize.auth.application.Features.User.Queries.AllUsersQuery;
using auth0rize.auth.application.Features.User.Update.UserUpdate;
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
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> createSuperadmin([FromBody] SuperUserRegisterRequest user)
        {
            return Ok(await Mediator.Send(new UserRegister(user.Name, user.LastName, user.MotherLastName, user.UserName, user.Email, user.Password, null)));
        }

        [HttpPost("new")]
        public async Task<IActionResult> createChildren([FromBody] UserRegisterRequest user)
        {
            DataSession session = new DataSession()
            {
                Id = Convert.ToInt32(HttpContext.User.Claims.First(c => c.Type == "user_id").Value),
                MotherLastName = HttpContext.User.Claims.First(c => c.Type == "motherlastname").Value,
                DomainName = HttpContext.User.Claims.First(c => c.Type == "domain").Value,
                LastName = HttpContext.User.Claims.First(c => c.Type == "lastname").Value,
                Name = HttpContext.User.Claims.First(c => c.Type == "name").Value,
                UserName = HttpContext.User.Claims.First(c => c.Type == "username").Value
            };
            return Ok(await Mediator.Send(new UserRegister(user.Name, user.LastName, user.MotherLastName, user.UserName, user.Email, user.Password, session, user.Type)));
        }

        [HttpGet]
        public async Task<IActionResult> get()
        {
            DataSession session = new DataSession()
            {
                Id = Convert.ToInt32(HttpContext.User.Claims.First(c => c.Type == "user_id").Value)
            };

            return Ok(await Mediator.Send(new AllUsersQuery(session)));

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> updateChildren([FromBody] UserUpdateRequest user, long id)
        {
            DataSession session = new DataSession()
            {
                Id = Convert.ToInt32(HttpContext.User.Claims.First(c => c.Type == "user_id").Value),
                MotherLastName = HttpContext.User.Claims.First(c => c.Type == "motherlastname").Value,
                DomainName = HttpContext.User.Claims.First(c => c.Type == "domain").Value,
                LastName = HttpContext.User.Claims.First(c => c.Type == "lastname").Value,
                Name = HttpContext.User.Claims.First(c => c.Type == "name").Value,
                UserName = HttpContext.User.Claims.First(c => c.Type == "username").Value
            };
            return Ok(await Mediator.Send(new UserUpdate(id, user.Name, user.LastName, user.MotherLastName, user.Email, session, user.Type)));
        }
    }
}

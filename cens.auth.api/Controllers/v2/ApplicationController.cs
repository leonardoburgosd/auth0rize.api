using cens.auth.application.Common.Entities;
using cens.auth.application.Features.Application.Command.CreateApplication;
using cens.auth.application.Features.Application.Queries.GetApplication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace cens.auth.api.Controllers.v2
{
    [ApiVersion("2.0")]
    [EnableCors("consultingPolicy")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ApplicationController : BaseApiController
    {

        [HttpPost]
        public async Task<IActionResult> create([FromBody] CreateApplicationCommandRequest application)
        {
            return Ok(await Mediator.Send(new CreateApplicationCommand(application.Name, application.Icon, application.Description, new SecurityTokenData()
            {
                UserId = Convert.ToInt32(HttpContext.User.Claims.First(c => c.Type == "user_id").Value),
                UserName = HttpContext.User.Claims.First(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value,
                TypeUser = HttpContext.User.Claims.First(c => c.Type == "user_rol").Value
            })));
        }

        [HttpGet]
        public async Task<IActionResult> get()
        {
            return Ok(await Mediator.Send(new GetApplicationQuery()));
        }
    }
}
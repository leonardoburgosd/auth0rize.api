using Asp.Versioning;
using auth0rize.auth.application.Features.Application.Queries.ApplicationAllQuery;
using Microsoft.AspNetCore.Mvc;

namespace auth0rize.auth.api.Controllers.v1
{
    [ApiVersion("1.0")]
    [ApiController]
    public class ApplicationController : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            int userId = Convert.ToInt32(HttpContext.User.Claims.First(c => c.Type == "user_id").Value);
            return Ok(await Mediator.Send(new ApplicationAllQuery(userId)));
        }
    }
}

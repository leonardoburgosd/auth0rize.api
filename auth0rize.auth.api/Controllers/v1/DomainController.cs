using auth0rize.auth.application.Features.Domain.Command.DomainCreate;
using auth0rize.auth.application.Features.Domain.Queries.DomainGet;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace auth0rize.auth.api.Controllers.v1
{
    [ApiVersion("1.0")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DomainController : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> Get(string? search, string? state, int page = 1, int size = 10)
        {
            return Ok(await Mediator.Send(new DomainGet(search, state, page, size)));
        }

        [HttpPost]
        public async Task<IActionResult> Create()
        {
            return Ok(await Mediator.Send(new DomainCreate(25)));
        }
    }
}

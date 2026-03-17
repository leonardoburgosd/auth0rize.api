using auth0rize.auth.application.Features.Company.Command.CompanyCreate;
using auth0rize.auth.application.Features.Company.Command.CompanyUpdate;
using auth0rize.auth.application.Features.Company.Command.CompanyDelete;
using auth0rize.auth.application.Features.Company.Queries.CompanyGet;
using auth0rize.auth.application.Features.Company.Queries.CompanyGetById;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace auth0rize.auth.api.Controllers.v1
{
    [ApiVersion("1.0")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CompanyController : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CompanyCreateRequest request)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "user_id");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized();
            }

            return Ok(await Mediator.Send(new CompanyCreate(
                request.Name,
                request.DomainId,
                request.Avatar,
                userId
            )));
        }

        [HttpGet]
        public async Task<IActionResult> Get(
            string? search,
            int? domainId,
            int page = 1,
            int size = 10)
        {
            return Ok(await Mediator.Send(new CompanyGet(search, domainId, page, size)));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await Mediator.Send(new CompanyGetById(id)));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CompanyUpdateRequest request)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "user_id");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized();
            }

            return Ok(await Mediator.Send(new CompanyUpdate(
                id,
                request.Name,
                request.DomainId,
                request.Avatar,
                userId
            )));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "user_id");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized();
            }

            return Ok(await Mediator.Send(new CompanyDelete(id, userId)));
        }
    }
}

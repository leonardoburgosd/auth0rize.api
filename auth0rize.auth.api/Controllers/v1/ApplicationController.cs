using auth0rize.auth.application.Features.Application.Command.ApplicationCreate;
using auth0rize.auth.application.Features.Application.Command.ApplicationDelete;
using auth0rize.auth.application.Features.Application.Command.ApplicationUpdate;
using auth0rize.auth.application.Features.Application.Command.ApplicationAddCompany;
using auth0rize.auth.application.Features.Application.Command.ApplicationRemoveCompany;
using auth0rize.auth.application.Features.Application.Queries.ApplicationGet;
using auth0rize.auth.application.Features.Application.Queries.ApplicationGetCompanies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace auth0rize.auth.api.Controllers.v1
{
    [ApiVersion("1.0")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ApplicationController : BaseApiController
    {
        private int GetUserIdFromToken()
        {
            var claim = User.Claims.FirstOrDefault(c => c.Type == "user_id");
            if (claim == null || !int.TryParse(claim.Value, out int userId))
                throw new UnauthorizedAccessException();
            return userId;
        }

        /// <summary>Listar aplicaciones con paginación y filtros opcionales.</summary>
        [HttpGet]
        public async Task<IActionResult> get(string? search, int page = 1, int size = 10)
        {
            return Ok(await Mediator.Send(new ApplicationGet(search, page, size)));
        }

        /// <summary>Crear una nueva aplicación.</summary>
        [HttpPost]
        public async Task<IActionResult> create([FromBody] ApplicationCreateRequest request)
        {
            int userId = GetUserIdFromToken();
            return Ok(await Mediator.Send(new ApplicationCreate(
                request.Name,
                request.Description,
                request.Avatar,
                userId
            )));
        }

        /// <summary>Actualizar una aplicación existente.</summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> update(int id, [FromBody] ApplicationUpdateRequest request)
        {
            int userId = GetUserIdFromToken();
            return Ok(await Mediator.Send(new ApplicationUpdate(
                id,
                request.Name,
                request.Description,
                request.Avatar,
                userId
            )));
        }

        /// <summary>Eliminar (soft delete) una aplicación.</summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> delete(int id)
        {
            int userId = GetUserIdFromToken();
            return Ok(await Mediator.Send(new ApplicationDelete(id, userId)));
        }

        /// <summary>Listar compañías asociadas a una aplicación.</summary>
        [HttpGet("{id:int}/companies")]
        public async Task<IActionResult> getCompanies(int id, int page = 1, int size = 10)
        {
            return Ok(await Mediator.Send(new ApplicationGetCompanies(id, page, size)));
        }

        /// <summary>Asociar una compañía a una aplicación.</summary>
        [HttpPost("{id:int}/companies")]
        public async Task<IActionResult> addCompany(int id, [FromBody] ApplicationAddCompanyRequest request)
        {
            int userId = GetUserIdFromToken();
            return Ok(await Mediator.Send(new ApplicationAddCompany(id, request.CompanyId, userId)));
        }

        /// <summary>Desasociar una compañía de una aplicación.</summary>
        [HttpDelete("{id:int}/companies/{companyId:int}")]
        public async Task<IActionResult> removeCompany(int id, int companyId)
        {
            int userId = GetUserIdFromToken();
            return Ok(await Mediator.Send(new ApplicationRemoveCompany(id, companyId, userId)));
        }
    }
}

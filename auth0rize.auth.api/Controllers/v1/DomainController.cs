using Asp.Versioning;
using auth0rize.auth.application.Common.Entities;
using auth0rize.auth.application.Features.Domain.Command.CreateDomain;
using auth0rize.auth.application.Features.Domain.Command.DeleteDomain;
using auth0rize.auth.application.Features.Domain.Queries.GetAllByIdQuery;
using auth0rize.auth.application.Features.Domain.Queries.GetAllQuery;
using Microsoft.AspNetCore.Mvc;

namespace auth0rize.auth.api.Controllers.v1
{
    [ApiVersion("1.0")]
    [ApiController]
    public class DomainController : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> get()
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
            return Ok(await Mediator.Send(new GetAllQuery(session)));
        }

        [HttpPost]
        public async Task<IActionResult> create([FromBody] CreateDomainRequest domain)
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
            return Ok(await Mediator.Send(new CreateDomain(domain.Name, session)));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> delete(int id)
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

            return Ok(await Mediator.Send(new DeleteDomain(id, session)));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> get(int id)
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
            return Ok(await Mediator.Send(new GetAllByIdQuery(id, session)));
        }
    }
}

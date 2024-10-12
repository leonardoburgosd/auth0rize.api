using Asp.Versioning;
using auth0rize.auth.application.Common.Entities;
using auth0rize.auth.application.Features.Application.Common.CreateApplication;
using auth0rize.auth.application.Features.Application.Common.DeletedApplication;
using auth0rize.auth.application.Features.Application.Queries.ApplicationAllQuery;
using Microsoft.AspNetCore.Mvc;

namespace auth0rize.auth.api.Controllers.v1
{
    [ApiVersion("1.0")]
    [ApiController]
    public class ApplicationController : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> get()
        {
            int userId = Convert.ToInt32(HttpContext.User.Claims.First(c => c.Type == "user_id").Value);
            return Ok(await Mediator.Send(new ApplicationAllQuery(userId)));
        }

        [HttpPost]
        public async Task<IActionResult> create([FromBody] CreateApplicationRequest application)
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
            return Ok(await Mediator.Send(new CreateApplication(application.Name, application.Description, session)));
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
            return Ok(await Mediator.Send(new DeletedApplication(id, session)));
        }
    }
}

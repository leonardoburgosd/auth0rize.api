﻿using Asp.Versioning;
using auth0rize.auth.application.Features.Autentication.Queries.BasicUser;
using auth0rize.auth.application.Features.Autentication.Queries.Login;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace auth0rize.auth.api.Controllers.v1
{
    [ApiVersion("1.0")]
    [ApiController]
    public class AuthController : BaseApiController
    {
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> login([FromBody] LoginRequest login)
        {
            return Ok(await Mediator.Send(new LoginQuery(login.UserName, login.Password, login.Application)));
        }

        [HttpPost("user")]
        [AllowAnonymous]
        public async Task<IActionResult> validationUser([FromBody] BasicUserRequest user)
        {
            return Ok(await Mediator.Send(new BasicUserQuery(user.UserName, user.Application)));
        }
    }
}

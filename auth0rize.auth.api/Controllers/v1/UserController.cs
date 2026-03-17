using auth0rize.auth.application.Features.User.Command.FirstAdminCreate;
using auth0rize.auth.application.Features.User.Command.UserCreate;
using auth0rize.auth.application.Features.User.Command.UserDomainDelete;
using auth0rize.auth.application.Features.User.Command.VerificationUser;
using auth0rize.auth.application.Features.User.Queries.UserGet;
using auth0rize.auth.application.Features.User.Queries.UserGetByDomain;
using auth0rize.auth.application.Features.User.Queries.UserInfoGet;
using auth0rize.auth.application.Features.User.Command.UserUpdateDoubleFactor;
using auth0rize.auth.application.Features.User.Command.UserUpdate;
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
        private int GetUserIdFromToken()
        {
            var claim = User.Claims.FirstOrDefault(c => c.Type == "user_id");
            if (claim == null || !int.TryParse(claim.Value, out int userId))
                throw new UnauthorizedAccessException();
            return userId;
        }

        [HttpPost("first-register")]
        [AllowAnonymous]
        public async Task<IActionResult> firstRegister([FromBody] FirstAdminCreateRequest user)
        {
            return Ok(await Mediator.Send(new FirstAdminCreate(user.Name, user.MotherLastName, user.LastName, user.UserName, user.Email, user.Password)));
        }

        [HttpPost]
        public async Task<IActionResult> userRegister([FromBody] UserCreateRequest user)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "user_id");
            int userId = 0;
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int parsedId))
            {
                userId = parsedId;
            }
            return Ok(await Mediator.Send(new UserCreate(user.Name, user.MotherLastName, user.LastName, user.UserName, user.Email, user.Password, user.TypeUserId, user.DomainId, userId)));
        }

        [HttpGet("verification")]
        [AllowAnonymous]
        public async Task<IActionResult> verification() 
        {
            return Ok(await Mediator.Send(new VerificationUser()));
        }

        [HttpGet]
        public async Task<IActionResult> get(string? search, string? type, bool? deleted, int page = 1, int size = 10)
        {
            return Ok(await Mediator.Send(new UserGet(search, type, deleted, page, size)));
        }

        [HttpGet("info")]
        public async Task<IActionResult> getUserInfo()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "user_id");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized();
            }
            return Ok(await Mediator.Send(new UserInfoGet(userId)));
        }

        [HttpGet("domain/{code}")]
        public async Task<IActionResult> getByDomain(string code, string? search, int page = 1, int size = 10)
        {
            return Ok(await Mediator.Send(new UserGetByDomain(code, search, page, size)));
        }

        /// <summary>Desasociar un usuario de un dominio (soft delete en UserDomain).</summary>
        [HttpDelete("domain/{code}/{userId:int}")]
        public async Task<IActionResult> removeFromDomain(string code, int userId)
        {
            int currentUserId = GetUserIdFromToken();
            return Ok(await Mediator.Send(new UserDomainDelete(userId, code, currentUserId)));
        }

        [HttpPut("double-factor")]
        public async Task<IActionResult> updateDoubleFactor([FromBody] bool isActive)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "user_id");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized();
            }
            return Ok(await Mediator.Send(new UserUpdateDoubleFactor(userId, isActive)));
        }

        [HttpPut]
        public async Task<IActionResult> update([FromBody] UserUpdateRequest request)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "user_id");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized();
            }
            return Ok(await Mediator.Send(new UserUpdate(
                userId, 
                request.FirstName, 
                request.LastName, 
                request.MotherLastName, 
                request.UserName, 
                request.Email, 
                request.TypeId
            )));
        }
    }
}

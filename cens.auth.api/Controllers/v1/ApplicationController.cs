using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cens.auth.api.Controllers.v1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiVersion("1.0")]
    public class ApplicationController : BaseApiController
    {

        //[HttpPost]
        //public async Task<IActionResult> create()
        //{

        //}
    }
}

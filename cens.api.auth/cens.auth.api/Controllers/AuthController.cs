using Microsoft.AspNetCore.Mvc;

namespace cens.auth.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost]
        public async Task<BaseResponse<responseAuthDto>> auth(authDto auth) => new BaseResponse<responseAuthDto>()
        {
            success = true,
            data = new responseAuthDto()
            {
                token = "akjshdkajsdkjasdjkasd",
                redirect = "http://localhost:59050/"
            },
            message = "Usuario ingreso correctamente."
        };

        [HttpGet]
        public async Task<bool> status()
        {
            return true;
        }
    }

    public class authDto
    {
        public string identificator { get; set; }
        public string password { get; set; }
        public string urlRedirect { get; set; }
        public string secretKey { get; set; }
        public string applicationId { get; set; }
    }

    public class BaseResponse<T>
    {
        public bool success { get; set; }
        public T data { get; set; }
        public string message { get; set; }
        //public IEnumerable<ValidationFailure>? Errors { get; set; }
    }

    public class responseAuthDto
    {
        public string token { get; set; }
        public string redirect { get; set; }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Oversite.Core.BLL;
using Oversite.Core.BLL.Login;
using Oversite.DTO.Login.Request;
using Oversite.DTO.Login.Response;
using Oversite.DTO.Response;

namespace Oversite.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IDistributedCache _distributedCache;

        public LoginController(IDistributedCache distributedCache)
        {

            _distributedCache = distributedCache;

        }
        [HttpPost("UserLogin")]
        public ActionResult<Response<LoginResponse>> Login([FromBody] LoginRequest request)
        {

            if (ModelState.IsValid)
            {
                return LoginBLL.Instance.userLogin(request, _distributedCache);
            }
            else
            {
                return BadRequest(ModelState);
            }
                }
    }
}

using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Oversite.PublicApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CsrfController : ControllerBase
    {
        private readonly IAntiforgery _antiforgery;

        public CsrfController(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
        }

        [HttpGet]
        [Route("token")]
        public IActionResult GetCsrfToken()
        {
            var tokens = _antiforgery.GetAndStoreTokens(HttpContext);
            return Ok(new { csrfToken = tokens.RequestToken });
        }
    }
}

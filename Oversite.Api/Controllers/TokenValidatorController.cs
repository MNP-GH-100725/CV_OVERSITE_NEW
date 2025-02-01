using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Oversite.Core.BLL.TokenValidator;
using Oversite.DTO.Response;
using Oversite.DTO.TokenValidator.Request;
using Oversite.DTO.TokenValidator.Response;

namespace Oversite.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenValidatorController : ControllerBase
    {
        private readonly IDistributedCache _distributedCache;

        public TokenValidatorController(IDistributedCache distributedCache)
        {

            _distributedCache = distributedCache;

        }
        [HttpPost("TokenValidator")]
        public ActionResult<Response<ValidTokenResponse>> Token([FromBody] ValidTokenRequest request)
        {
            if (ModelState.IsValid)
            {
                return TokenValidatorBLL.instance.ValidToken(request);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        [HttpPost("ValidateTokenWithEmpCode")]
        public ActionResult<Response<ValidTokenResponse>> ValidateTokenWithEmpCode([FromBody] ValidateTokenWithEmpcodeRequest request)
        {
            if (ModelState.IsValid)
            {
                return TokenValidatorBLL.instance.ValidateTokenWithEmpCode(request);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        [HttpPost("updateTokenWhenLogout")]
        public ActionResult<Response<updateTokenWhenLogoutResponse>> updateTokenWhenLogout([FromBody] ValidateTokenWithEmpcodeRequest request)
        {
            if (ModelState.IsValid)
            {
                return TokenValidatorBLL.instance.updateTokenWhenLogout(request, _distributedCache);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost("GetotpemployeeData")]
        public ActionResult<Response<EmployeResponse>> GetotpemployeeData([FromBody] EmployeRequest request)
        {
            if (ModelState.IsValid)
            {
                return TokenValidatorBLL.instance.GetotpemployeeData(request, _distributedCache);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        [HttpPost("Preauth")]
        public ActionResult<Response<PreauthResponse>> Preauth([FromBody] PreauthRequest request)
        {
            if (ModelState.IsValid)
            {
                return TokenValidatorBLL.instance.Preauth(request, _distributedCache);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        [HttpPost("AddPreauth")]
        public ActionResult<AddpreauthResponse> Add([FromBody] AddpreauthRequest request)
        {
            if (ModelState.IsValid)
            {
                return TokenValidatorBLL.instance.Add(request);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        
    }
}

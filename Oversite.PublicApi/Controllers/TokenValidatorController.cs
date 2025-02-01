using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RSA_Angular_.NET_CORE.RSA;
using Oversite.DTO.TokenValidator.Response;
using Oversite.DTO.TokenValidator.Request;
using Oversite.DTO.Response;
using Oversite.PublicApi.Utilities;
using Microsoft.AspNetCore.RateLimiting;

namespace Oversite.PublicApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    //[TypeFilter(typeof(TokenValidator))]
    [ResultDetailFilter]
    [EnableRateLimiting("fixed")]


    public class TokenValidatorController : ControllerBase
    {
        IConfiguration configuration;
        public TokenValidatorController(IConfiguration iConfig)
        {
            configuration = iConfig;

        }

        [HttpPost("TokenValidator")]
        public ActionResult<Response<ValidTokenResponse>> Token([FromBody] ValidTokenRequest request)
        {
            Oversite.DTO.Response.Response<ValidTokenResponse> response = new Oversite.DTO.Response.Response<ValidTokenResponse>();
            string baseUrl = configuration.GetSection("baseUrl").Value;
            response = new Oversite.Helpers.ApiManager().InvokePostHttpClient<Oversite.DTO.Response.Response<ValidTokenResponse>, ValidTokenRequest>(request, baseUrl + "/api/TokenValidator/TokenValidator").Item1;
            return response;
        }

        //[TypeFilter(typeof(PreTokenValidator))]
        [HttpPost("ValidateTokenWithEmpCode")]
        public ActionResult<Response<ValidTokenResponse>> ValidateTokenWithEmpCode([FromBody] ValidateTokenWithEmpcodeRequest request)
        {
            request.empCode = new RsaEncHelper().Decrypt(request.empCode);
            Oversite.DTO.Response.Response<ValidTokenResponse> response = new Oversite.DTO.Response.Response<ValidTokenResponse>();
            string baseUrl = configuration.GetSection("baseUrl").Value;
            response = new Oversite.Helpers.ApiManager().InvokePostHttpClient<Oversite.DTO.Response.Response<ValidTokenResponse>, ValidateTokenWithEmpcodeRequest>(request, baseUrl + "/api/TokenValidator/ValidateTokenWithEmpCode").Item1;
            return response;
        }
        //[TypeFilter(typeof(TokenValidator))]
        [HttpPost("updateTokenWhenLogout")]
        public ActionResult<Response<updateTokenWhenLogoutResponse>> updateTokenWhenLogout([FromBody] ValidateTokenWithEmpcodeRequest request)
        {
            request.empCode = new RsaEncHelper().Decrypt(request.empCode);
            Oversite.DTO.Response.Response<updateTokenWhenLogoutResponse> response = new Oversite.DTO.Response.Response<updateTokenWhenLogoutResponse>();
            string baseUrl = configuration.GetSection("baseUrl").Value;
            response = new Oversite.Helpers.ApiManager().InvokePostHttpClient<Oversite.DTO.Response.Response<updateTokenWhenLogoutResponse>, ValidateTokenWithEmpcodeRequest>(request, baseUrl + "/api/TokenValidator/updateTokenWhenLogout").Item1;
            return response;
        }
        //[TypeFilter(typeof(PreTokenValidator))]
        [HttpPost("GetotpemployeeData")]
        public ActionResult<Response<EmployeResponse>> GetotpemployeeData([FromBody] EmployeRequest request)
        {
            Oversite.DTO.Response.Response<EmployeResponse> response = new Oversite.DTO.Response.Response<EmployeResponse>();
            string baseUrl = configuration.GetSection("baseUrl").Value;
            response = new Oversite.Helpers.ApiManager().InvokePostHttpClient<Oversite.DTO.Response.Response<EmployeResponse>, EmployeRequest>(request, baseUrl + "/api/TokenValidator/GetotpemployeeData").Item1;
            return response;
        }
        [HttpPost("Preauth")]
        public ActionResult<Response<PreauthResponse>> Preauth([FromBody] PreauthRequest request)
        {
            Oversite.DTO.Response.Response<PreauthResponse> response = new Oversite.DTO.Response.Response<PreauthResponse>();
            string baseUrl = configuration.GetSection("baseUrl").Value;
            response = new Oversite.Helpers.ApiManager().InvokePostHttpClient<Oversite.DTO.Response.Response<PreauthResponse>, PreauthRequest>(request, baseUrl + "/api/TokenValidator/Preauth").Item1;
            return response;
        }

    }
}

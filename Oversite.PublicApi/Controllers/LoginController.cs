using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Configuration;
using Oversite.DTO.Login.Request;
using Oversite.DTO.Login.Response;
using Oversite.DTO.Response;
using Oversite.PublicApi.Utilities;
using RSA_Angular_.NET_CORE.RSA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oversite.PublicApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ResultDetailFilter]
    [EnableRateLimiting("fixed")]

    public class LoginController : ControllerBase
    {
        IConfiguration configuration;
        public LoginController(IConfiguration iConfig)
        {
            configuration = iConfig;

        }
        [HttpPost("UserLogin")]
        public ActionResult<Response<LoginResponse>> UserLogin([FromBody] LoginRequest request)
        {
            request.employeeId = new RsaEncHelper().Decrypt(request.employeeId);
            request.password = new RsaEncHelper().Decrypt(request.password);
            Oversite.DTO.Response.Response<LoginResponse> response = new Oversite.DTO.Response.Response<LoginResponse>();
            string baseUrl = configuration.GetSection("baseUrl").Value;
            response = new Oversite.Helpers.ApiManager().InvokePostHttpClient<Oversite.DTO.Response.Response<LoginResponse>, LoginRequest>(request, baseUrl + "/api/Login/UserLogin").Item1;
            return response;
        }
    }
}

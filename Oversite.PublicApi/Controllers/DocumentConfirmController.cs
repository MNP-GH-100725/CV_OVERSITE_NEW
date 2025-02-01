using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Configuration;
using Oversite.DTO.DocumentConfirm.Request;
using Oversite.DTO.DocumentConfirm.Response;
using Oversite.DTO.Response;
using Oversite.PublicApi.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oversite.PublicApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [TypeFilter(typeof(TokenValidator))]
    [ResultDetailFilter]
    [EnableRateLimiting("fixed")]

    public class DocumentConfirmController : ControllerBase
    {
        IConfiguration configuration;
        public DocumentConfirmController(IConfiguration iConfig)
        {
            configuration = iConfig;

        }
        [HttpPost("ConfirmData")]
        public ActionResult<Response<DocumentConfirmResponse>> Get([FromBody] DocumentConfirmRequest request)
        {
            Response<DocumentConfirmResponse> response = new Response<DocumentConfirmResponse>();
            string baseUrl = configuration.GetSection("baseUrl").Value;
            response = new Oversite.Helpers.ApiManager().InvokePostHttpClient<Oversite.DTO.Response.Response<DocumentConfirmResponse>, DocumentConfirmRequest>(request, baseUrl + "/api/DocumentConfirm/ConfirmData").Item1;
            return response;
        }
    }
}

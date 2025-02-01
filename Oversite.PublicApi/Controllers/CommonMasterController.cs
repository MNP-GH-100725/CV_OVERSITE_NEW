using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Configuration;
using Oversite.DTO.CommonMaster.Request;
using Oversite.DTO.CommonMaster.Response;
using Oversite.DTO.Documents.Request;
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
    public class CommonMasterController : ControllerBase
    {
        IConfiguration configuration;
        public CommonMasterController(IConfiguration iConfig)
        {
            configuration = iConfig;

        }
        [HttpPost("GetCommonMaster")]
        public ActionResult<Response<CommonMasterReponse>> Get([FromBody] CommonMasterRequest request)
        {
           Response<CommonMasterReponse> response = new Response<CommonMasterReponse>();
            string baseUrl = configuration.GetSection("baseUrl").Value;
            response = new Oversite.Helpers.ApiManager().InvokePostHttpClient<Oversite.DTO.Response.Response<CommonMasterReponse>, CommonMasterRequest>(request, baseUrl + "/api/CommonMaster/GetCommonMaster").Item1;
            return response;
        }
        [HttpPost("GetDocumentmaster")]
        public ActionResult<Response<GetdocMasterReponse>> Getdoc([FromBody] GetdocMasterRequest request)
        {
            Response<GetdocMasterReponse> response = new Response<GetdocMasterReponse>();
            string baseUrl = configuration.GetSection("baseUrl").Value;
            response = new Oversite.Helpers.ApiManager().InvokePostHttpClient<Oversite.DTO.Response.Response<GetdocMasterReponse>, GetdocMasterRequest>(request, baseUrl + "/api/CommonMaster/GetDocumentmaster").Item1;
            return response;
        }

        [HttpPost("SendBackTo")]
        public ActionResult<Response<CommonMasterReponse>> SendBackTo([FromBody] CommonMasterRequest request)
        {
            Response<CommonMasterReponse> response = new Response<CommonMasterReponse>();
            string baseUrl = configuration.GetSection("baseUrl").Value;
            response = new Oversite.Helpers.ApiManager().InvokePostHttpClient<Oversite.DTO.Response.Response<CommonMasterReponse>, CommonMasterRequest>(request, baseUrl + "/api/CommonMaster/SendBackTo").Item1;
            return response;
        }
    }
}

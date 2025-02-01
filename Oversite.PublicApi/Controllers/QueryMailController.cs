using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Configuration;
using Oversite.DTO.Documents.Response;
using Oversite.DTO.Email.Request;
using Oversite.DTO.Email.Response;
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

    public class QueryMailController : ControllerBase
    {
        IConfiguration configuration;
        public QueryMailController(IConfiguration iConfig)
        {
            configuration = iConfig;

        }
        [HttpPost("QueryEmail")]
        public ActionResult<Response<QueryEmailResponse>> QueryMail()
        {
            Response<QueryEmailResponse> response = new Response<QueryEmailResponse>();
            string baseUrl = configuration.GetSection("baseUrl").Value;
            response = new Helpers.ApiManager().InvokePostHttpClientWithoutRequest<Response<QueryEmailResponse>>(baseUrl + "/api/QueryMail/QueryEmail");
            return response;
        }

        [HttpPost("TotalDisb")]
        public ActionResult<Response<TotalDisResponse>> TotalDisb()
        {
            Response<TotalDisResponse> response = new Response<TotalDisResponse>();
            string baseUrl = configuration.GetSection("baseUrl").Value;
            response = new Helpers.ApiManager().InvokePostHttpClientWithoutRequest<Response<TotalDisResponse>>(baseUrl + "/api/QueryMail/TotalDisb");
            return response;
        }


        [HttpPost("NchMail")]
        public ActionResult<Response<NCHResponse>> NchMail()
        {
            Response<NCHResponse> response = new Response<NCHResponse>();
            string baseUrl = configuration.GetSection("baseUrl").Value;
            response = new Helpers.ApiManager().InvokePostHttpClientWithoutRequest<Response<NCHResponse>>(baseUrl + "/api/QueryMail/NchMail");
            return response;
        }


    }
}

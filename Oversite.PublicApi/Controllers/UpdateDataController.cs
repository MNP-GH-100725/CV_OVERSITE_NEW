using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Configuration;
using Oversite.DTO.Response;
using Oversite.DTO.UpdateData;
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

    public class UpdateDataController : ControllerBase
    {
   
            IConfiguration configuration;
            public UpdateDataController(IConfiguration iConfig)
            {
                configuration = iConfig;

            }
            [HttpPost("UpdateData")]
            public ActionResult<Response<UpdateDataResponse>> Get([FromBody] UpdateDataRequest request)
            {
                Response<UpdateDataResponse> response = new Response<UpdateDataResponse>();
                string baseUrl = configuration.GetSection("baseUrl").Value;
                response = new Oversite.Helpers.ApiManager().InvokePostHttpClient<Oversite.DTO.Response.Response<UpdateDataResponse>, UpdateDataRequest>(request, baseUrl + "/api/UpdateData/UpdateData").Item1;
                return response;
            }
        
    }
}

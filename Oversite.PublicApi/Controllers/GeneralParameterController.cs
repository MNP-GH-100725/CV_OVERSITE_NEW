using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Configuration;
using Oversite.DTO.Reports.Request;
using Oversite.DTO.Reports.Response;
using Oversite.PublicApi.Utilities;
using OversitePublic.Utilities;
using System.Configuration;

namespace Oversite.PublicApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [ResultDetailFilter]
    [EnableRateLimiting("fixed")]
    public class GeneralParameterController : ControllerBase
    {
        IConfiguration configuration;
        public GeneralParameterController(IConfiguration iConfig)
        {
            configuration = iConfig;

        }
       [TypeFilter(typeof(TokenValidator))]
        [HttpGet]
        public ActionResult<GeneralParameterResponse> Get([FromQuery] GeneralParameterRequest request)
        {
            if (ModelState.IsValid)
            {
                string baseUrl = configuration.GetSection("report").Value;
                string Uri = baseUrl + "api/v1/GeneralParameter?ParameterID=" + request.ParameterID + "&ModuleID=" + request.ModuleID + "&productId=" + request.productId + "";
                GeneralParameterResponse loanApplicantResponse = new Oversite.Helpers.ApiManager().InvokeGetHttpClientWithoutRequest<GeneralParameterResponse>(Uri, null);
                return Ok(loanApplicantResponse);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}

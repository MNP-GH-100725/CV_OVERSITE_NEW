using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Configuration;
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

    public class Repaymentcontroller : ControllerBase
    {
        IConfiguration configuration;
        public Repaymentcontroller(IConfiguration iConfig)
        {
            configuration = iConfig;

        }
        [HttpPost("RepaymentSchedule")]
        public ActionResult<Response<RepaymentScheduleResponse>> repay([FromBody] AccRepaymentScheduleRequest request)
        {
            Oversite.DTO.Response.Response<RepaymentScheduleResponse> response = new Oversite.DTO.Response.Response<RepaymentScheduleResponse>();
            string baseUrl = configuration.GetSection("baseUrl").Value;
            response = new Oversite.Helpers.ApiManager().InvokePostHttpClient<Oversite.DTO.Response.Response<RepaymentScheduleResponse>, AccRepaymentScheduleRequest>(request, baseUrl + "/api/Repayment/RepaymentSchedule").Item1;
            return response;
        }

        [HttpPost("LoanDetails")]
        public ActionResult<Response<LoanDetailsResponse>> fetch([FromBody] LoanDetailsRequest request)
        {
            Oversite.DTO.Response.Response<LoanDetailsResponse> response = new Oversite.DTO.Response.Response<LoanDetailsResponse>();
            string baseUrl = configuration.GetSection("baseUrl").Value;
            response = new Oversite.Helpers.ApiManager().InvokePostHttpClient<Oversite.DTO.Response.Response<LoanDetailsResponse>, LoanDetailsRequest>(request, baseUrl + "/api/Repayment/LoanDetails").Item1;
            return response;
        }
        [HttpPost("SaveRepayDetails")]
        public ActionResult<Response<SaveRepayDetailsResponse>> save([FromBody] SaveRepayDetailsRequest request)
        {
            Oversite.DTO.Response.Response<SaveRepayDetailsResponse> response = new Oversite.DTO.Response.Response<SaveRepayDetailsResponse>();
            string baseUrl = configuration.GetSection("baseUrl").Value;
            response = new Oversite.Helpers.ApiManager().InvokePostHttpClient<Oversite.DTO.Response.Response<SaveRepayDetailsResponse>, SaveRepayDetailsRequest>(request, baseUrl + "/api/Repayment/SaveRepayDetails").Item1;
            return response;
        }
    }
}

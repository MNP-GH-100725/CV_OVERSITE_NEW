using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Configuration;
using Oversite.DTO.Reports.Request;
using Oversite.DTO.Reports.Response;
using Oversite.DTO.Response;
using Oversite.PublicApi.Service;
using Oversite.PublicApi.Utilities;
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

    public class ReportsController : ControllerBase
    {
        IConfiguration configuration;
        public ReportsController(IConfiguration iConfig)
        {
            configuration = iConfig;

        }


        //public ActionResult<Response<ProductDataResponse>> userLogin([FromBody] ProductDataRequest request)
        //{
        //    Oversite.DTO.Response.Response<ProductDataResponse> response = new Oversite.DTO.Response.Response<ProductDataResponse>();
        //    string baseUrl = configuration.GetSection("baseUrl").Value;
        //    response = new Oversite.Helpers.ApiManager().InvokePostHttpClient<Oversite.DTO.Response.Response<ProductDataResponse>, ProductDataRequest>(request, baseUrl + "/api/Reports/ReportProductData").Item1;
        //    return response;
        //}
        [HttpPost("PreReportProductData")]

        public ActionResult<Response<ProductDataResponse>> PreReportProductData([FromBody] ProductDataRequest request)
        {
            Oversite.DTO.Response.Response<ProductDataResponse> response = new Oversite.DTO.Response.Response<ProductDataResponse>();
            string baseUrl = configuration.GetSection("baseUrl").Value;
            response = new Oversite.Helpers.ApiManager().InvokePostHttpClient<Oversite.DTO.Response.Response<ProductDataResponse>, ProductDataRequest>(request, baseUrl + "/api/Reports/ReportProductData").Item1;
            return response;
        }

        [TypeFilter(typeof(TokenValidator))]
        [HttpPost("ReportProductData")]

        public ActionResult<Response<ProductDataResponse>> ReportProductData([FromBody] ProductDataRequest request)
        {
            Oversite.DTO.Response.Response<ProductDataResponse> response = new Oversite.DTO.Response.Response<ProductDataResponse>();
            string baseUrl = configuration.GetSection("baseUrl").Value;
            response = new Oversite.Helpers.ApiManager().InvokePostHttpClient<Oversite.DTO.Response.Response<ProductDataResponse>, ProductDataRequest>(request, baseUrl + "/api/Reports/ReportProductData").Item1;
            return response;
        }
        [HttpPost("VerificationStatusReports")]
        public ActionResult<Response<VerificationStatusReportResponse>> VerificationStatusReport([FromBody] VerificationStatusReportRequest request)
        {
            Oversite.DTO.Response.Response<VerificationStatusReportResponse> response = new Oversite.DTO.Response.Response<VerificationStatusReportResponse>();
            string baseUrl = configuration.GetSection("baseUrl").Value;
            response = new Oversite.Helpers.ApiManager().InvokePostHttpClient<Oversite.DTO.Response.Response<VerificationStatusReportResponse>, VerificationStatusReportRequest>(request, baseUrl + "/api/Reports/VerificationStatusReports").Item1;
            return response;
        }


        [TypeFilter(typeof(TokenValidator))]
        [HttpGet("GenerateReportTW")]
        public ActionResult<GenerateReportResponse> GenerateReportTW([FromQuery] GenerateReportRequests request)
        {
            if (ModelState.IsValid)
            {
                string baseUrl = configuration.GetSection("report").Value;
                string Uri = baseUrl + "api/v1/ReportDetails/GenerateReport?reportId=" + request.reportId + "&type=" + request.type + "&param1=" + request.param1 + "&param2=" + request.param2 + "&param3=" + request.param3 + "&param4=" + request.param4 + "&param5=" + request.param5 + "&param6=" + request.param6 + "&param7=" + request.param7 + "&productId=" + request.productId + "&firmId=" + request.firmId + "&userId=" + request.userId + "";
                GenerateReportResponse response = new Oversite.Helpers.ApiManager().InvokeGetHttpClientWithoutRequest<GenerateReportResponse>(Uri, null);
                return Ok(response);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        [TypeFilter(typeof(TokenValidator))]
        [HttpGet("GetReportsUserTW")]
        public ActionResult<GetReportResponse> GetReportsUserTW([FromQuery] GetReportRequest Request)
        {
            if (ModelState.IsValid)
            {
                string baseUrl = configuration.GetSection("report").Value;
                string Uri = baseUrl + "api/v1/Reports/GetReportsUser?FirmID=" + Request.FirmID + "&UserID=" + Request.UserID + "&ProductID=" + Request.ProductID + "";
                GetReportResponse response = new Oversite.Helpers.ApiManager().InvokeGetHttpClientWithoutRequest<GetReportResponse>(Uri, null);
                return Ok(response);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


        [TypeFilter(typeof(TokenValidator))]
        [HttpGet("DrillDownReports")]
        public ActionResult<GenerateReportResponse1> GenerateDrilldown([FromQuery] DrilldownReportsRequest request)
        {
            if (ModelState.IsValid)
            {
                string baseUrl = configuration.GetSection("report").Value;
                string Uri = baseUrl + "api/v1/ReportDetails/DrillDownReports?type=" + request.type + "&report_id=" + request.report_id + "&level_no=" + request.level_no + "&parms=" + request.parms + "&link_field=" + request.link_field + "&link_value=" + request.link_value + "&productId=" + request.productId + "&firmId=" + request.firmId + "";
                GenerateReportResponse1 response = new Oversite.Helpers.ApiManager().InvokeGetHttpClientWithoutRequest<GenerateReportResponse1>(Uri, null);
                return Ok(response);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


        //[TypeFilter(typeof(TokenValidator))]
        [HttpGet("GenerateReportTWOne")]
        public ActionResult<GenerateReportResponse> GenerateReportTWOne([FromQuery] GenerateReportRequests request)
        {
            var aesService = new AesEncryptionService();

            if (ModelState.IsValid)
            {
                //try
                //{

                //    int decodedReportId, decodedProductId, decodedFirmId;

                //    if (int.TryParse(aesService.Decode(request.reportId.ToString()), out decodedReportId) &&
                //        int.TryParse(aesService.Decode(request.productId.ToString()), out decodedProductId) &&
                //        int.TryParse(aesService.Decode(request.firmId.ToString()), out decodedFirmId))
                //    {
                //        request.reportId = decodedReportId;
                //        request.productId = decodedProductId;
                //        request.firmId = decodedFirmId;
                //    }
                //    else
                //    {
                //        // Handle the error, e.g., return a BadRequest response
                //        Console.WriteLine("Invalid input data for reportId, productId, or firmId");
                //        return BadRequest("Invalid input data.");
                //    }

                //string reportId = aesService.Decode(request.reportId.ToString());
                //int.TryParse(reportId, out decodedReportId);


                //string productId = aesService.Decode(request.productId.ToString());
                //int.TryParse(reportId, out decodedProductId);


                //string firmId = aesService.Decode(request.firmId.ToString());
                //int.TryParse(reportId, out decodedFirmId);

                // Decrypt each incoming parameter

                ////request.reportId = aesService.Decode(request.reportId.ToString());
                ////request.reportId = int.Parse(aesService.Decode(request.reportId.ToString()));
                //request.reportId = decodedReportId;
                //request.type = aesService.Decode(request.type);
                //request.param1 = aesService.Decode(request.param1);
                //request.param2 = aesService.Decode(request.param2);
                //request.param3 = aesService.Decode(request.param3);
                //request.param4 = aesService.Decode(request.param4);
                //request.param5 = aesService.Decode(request.param5);
                //request.param6 = aesService.Decode(request.param6);
                //request.param7 = aesService.Decode(request.param7);
                ////request.productId = aesService.Decode(request.productId.ToString());
                ////request.productId = int.Parse(aesService.Decode(request.productId.ToString()));
                //request.productId = decodedProductId;
                ////request.firmId = aesService.Decode(request.firmId.ToString());
                ////request.firmId = int.Parse(aesService.Decode(request.firmId.ToString()));
                //request.firmId = decodedFirmId;
                //request.userId = aesService.Decode(request.userId);

                string baseUrl = configuration.GetSection("report").Value;
                string Uri = baseUrl + "api/v1/ReportDetails/GenerateReport?reportId=" + request.reportId + "&type=" + request.type + "&param1=" + request.param1 + "&param2=" + request.param2 + "&param3=" + request.param3 + "&param4=" + request.param4 + "&param5=" + request.param5 + "&param6=" + request.param6 + "&param7=" + request.param7 + "&productId=" + request.productId + "&firmId=" + request.firmId + "&userId=" + request.userId + "";
                GenerateReportResponse response = new Oversite.Helpers.ApiManager().InvokeGetHttpClientWithoutRequest<GenerateReportResponse>(Uri, null);
                return Ok(response);

                //}
                //catch (Exception ex)
                //{
                //    return BadRequest(new { message = "Decoding failed", error = ex.Message });
                //}

            }
            else
            {
                return BadRequest(ModelState);
            }
        }

    }
}

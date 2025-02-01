using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Configuration;
using Oversite.DTO.DashBoard.Request;
using Oversite.DTO.DashBoard.Response;
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
    public class DashBoardController : ControllerBase
    {
        IConfiguration configuration;
        public DashBoardController(IConfiguration iConfig)
        {
            configuration = iConfig;

        }
        [HttpPost("DashBoardData")]
        public ActionResult<Response<DashBoardResponse>> Get([FromBody] DashBoardRequest request)
        {
            Response<DashBoardResponse> response = new Response<DashBoardResponse>();
            string baseUrl = configuration.GetSection("baseUrl").Value;
            response = new Oversite.Helpers.ApiManager().InvokePostHttpClient<Oversite.DTO.Response.Response<DashBoardResponse>, DashBoardRequest>(request, baseUrl + "/api/DashBoard/DashBoardData").Item1;
            return response;
        }

        [HttpPost("FillMonth")]
        public ActionResult<Response<MonthListResponse>> fill([FromBody] MonthListRequest request)
        {
            Response<MonthListResponse> response = new Response<MonthListResponse>();
            string baseUrl = configuration.GetSection("baseUrl").Value;
            response = new Oversite.Helpers.ApiManager().InvokePostHttpClient<Oversite.DTO.Response.Response<MonthListResponse>, MonthListRequest>(request, baseUrl + "/api/DashBoard/FillMonth").Item1;
            return response;
        }

        [HttpPost("FillRegion")]
        public ActionResult<Response<RegionListResponse>> fill1([FromBody] RegionListRequest request)
        {
            Response<RegionListResponse> response = new Response<RegionListResponse>();
            string baseUrl = configuration.GetSection("baseUrl").Value;
            response = new Oversite.Helpers.ApiManager().InvokePostHttpClient<Oversite.DTO.Response.Response<RegionListResponse>, RegionListRequest>(request, baseUrl + "/api/DashBoard/FillRegion").Item1;
            return response;
        }
        [HttpPost("FillProduct")]

        public ActionResult<Response<ProductListResponse>> fill2([FromBody] ProductListRequest request)
        {
            Response<ProductListResponse> response = new Response<ProductListResponse>();
            string baseUrl = configuration.GetSection("baseUrl").Value;
            response = new Oversite.Helpers.ApiManager().InvokePostHttpClient<Oversite.DTO.Response.Response<ProductListResponse>, ProductListRequest>(request, baseUrl + "/api/DashBoard/FillProduct").Item1;
            return response;
        }

        [HttpPost("FillStatus")]

        public ActionResult<Response<StatusListResponse>> fill3([FromBody] StatusListRequest request)
        {
            Response<StatusListResponse> response = new Response<StatusListResponse>();
            string baseUrl = configuration.GetSection("baseUrl").Value;
            response = new Oversite.Helpers.ApiManager().InvokePostHttpClient<Oversite.DTO.Response.Response<StatusListResponse>, StatusListRequest>(request, baseUrl + "/api/DashBoard/FillStatus").Item1;
            return response;
        }

        [HttpPost("FillBranch")]
        public ActionResult<Response<BranchListResponse>> fill4([FromBody] BranchListRequest request)
        {
            Response<BranchListResponse> response = new Response<BranchListResponse>();
            string baseUrl = configuration.GetSection("baseUrl").Value;
            response = new Oversite.Helpers.ApiManager().InvokePostHttpClient<Oversite.DTO.Response.Response<BranchListResponse>, BranchListRequest>(request, baseUrl + "/api/DashBoard/FillBranch").Item1;
            return response;
        }
    }
}


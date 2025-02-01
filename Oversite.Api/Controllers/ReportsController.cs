

using Microsoft.AspNetCore.Mvc;
using Oversite.Core.BLL.Reports;
using Oversite.DTO.Reports.Request;
using Oversite.DTO.Reports.Response;
using Oversite.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oversite.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
       
        [HttpPost("ReportProductData")]
        public ActionResult<Response<ProductDataResponse>> ProductData([FromBody] ProductDataRequest request)
        {
            if (ModelState.IsValid)
            {
                return ReportBLL.instance.ProductDataReport(request);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


        [HttpPost("VerificationStatusReports")]
        public ActionResult<Response<VerificationStatusReportResponse>> VerificationStatusReport([FromBody] VerificationStatusReportRequest request)
        {
            if (ModelState.IsValid)
            {
                return ReportBLL.instance.VerificationStatusReportBLL(request);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }



    }
}

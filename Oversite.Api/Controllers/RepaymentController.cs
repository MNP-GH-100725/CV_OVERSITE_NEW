using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Oversite.Core.BLL.DocumentConfirm;
using Oversite.DTO.CommonMaster.Response;
using Oversite.DTO.Documents.Request;
using Oversite.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oversite.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RepaymentController : ControllerBase
    {
        [HttpPost("RepaymentSchedule")]

        public ActionResult<Response<RepaymentScheduleResponse>> Repay([FromBody] AccRepaymentScheduleRequest request)
        {

            if (ModelState.IsValid)
            {
                return ClonfirmBLL.instance.Repay(request);
            }
            else
            {
                return BadRequest(ModelState);
            }

        }

        [HttpPost("LoanDetails")]

        public ActionResult<Response<LoanDetailsResponse>> Fetch([FromBody] LoanDetailsRequest request)
        {

            if (ModelState.IsValid)
            {
                return ClonfirmBLL.instance.FetchData(request);
            }
            else
            {
                return BadRequest(ModelState);
            }

        }
        [HttpPost("SaveRepayDetails")]

        public ActionResult<Response<SaveRepayDetailsResponse>> save([FromBody] SaveRepayDetailsRequest request)
        {

            if (ModelState.IsValid)
            {
                return ClonfirmBLL.instance.SaveData(request);
            }
            else
            {
                return BadRequest(ModelState);
            }

        }
    }
}

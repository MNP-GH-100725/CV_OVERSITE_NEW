using Microsoft.AspNetCore.Mvc;
using Oversite.Core.BLL.Email;
using Oversite.DTO.Email.Request;
using Oversite.DTO.Email.Response;
using Oversite.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oversite.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QueryMailController : ControllerBase
    {
        [HttpPost("QueryEmail")]
        public ActionResult<Response<QueryEmailResponse>> QueryMail()
        {
            if (ModelState.IsValid)
            {
                return QueryEmailBLL.instance.QueryMail();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost("TotalDisb")]
        public ActionResult<Response<TotalDisResponse>> TotalDisb()
        {
            if (ModelState.IsValid)
            {
                return QueryEmailBLL.instance.TotalDisb();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost("NchMail")]
        public ActionResult<Response<NCHResponse>> NchMail()
        {
            if (ModelState.IsValid)
            {
                return QueryEmailBLL.instance.NCHMail();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}

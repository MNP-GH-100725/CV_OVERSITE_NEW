using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Oversite.Core.BLL.DocumentConfirm;
using Oversite.DTO.DocumentConfirm.Request;
using Oversite.DTO.DocumentConfirm.Response;
using Oversite.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oversite.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentConfirmController : ControllerBase
    {
        [HttpPost("ConfirmData")]
        public ActionResult<Response<DocumentConfirmResponse>> Get([FromBody] DocumentConfirmRequest request)
        {
            if (ModelState.IsValid)
            {
                return ClonfirmBLL.instance.Confirm(request);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Oversite.Core.BLL.UpdateData;
using Oversite.DTO.Response;
using Oversite.DTO.UpdateData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oversite.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateDataController : ControllerBase
    {
        [HttpPost("UpdateData")]
        public ActionResult<Response<UpdateDataResponse>> Get([FromBody] UpdateDataRequest request)
        {
            if (ModelState.IsValid)
            {
                return UpdateDataBLL.Instance.updateData(request);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}

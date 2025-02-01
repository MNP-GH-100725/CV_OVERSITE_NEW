using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Oversite.Core.BLL.GeneralData;
using Oversite.DTO.CommonMaster.Request;
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
    public class CommonMasterController : ControllerBase
    {
        [HttpPost("GetCommonMaster")]
        public ActionResult<Response<CommonMasterReponse>> Get([FromBody] CommonMasterRequest request)
        {
            if (ModelState.IsValid)
            {
                return CommonMasterBLL.Instance.CommonData(request);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost("GetDocumentmaster")]
        public ActionResult<Response<GetdocMasterReponse>> Getdoc([FromBody] GetdocMasterRequest request)
        {
            if (ModelState.IsValid)
            {
                return CommonMasterBLL.Instance.Getdoc(request);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost("SendBackTo")]
        public ActionResult<Response<CommonMasterReponse>> SendBackTo([FromBody] CommonMasterRequest request)
        {
            if (ModelState.IsValid)
            {
                return CommonMasterBLL.Instance.SendBackTo(request);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}

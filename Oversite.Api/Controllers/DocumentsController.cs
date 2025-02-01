using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Oversite.Core.BLL.Documents;
using Oversite.DTO.CommonMaster.Request;
using Oversite.DTO.Documents.Response;
using Oversite.DTO.Request;
using Oversite.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oversite.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        [HttpPost("DocumentDetails")]
        public ActionResult<Response<GetDocumentResponse>> Get([FromBody] GetDocumentRequest request)
        {
            if (ModelState.IsValid)
            {
                return GetDocumentsBLL.instance.GetDocument(request);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        [HttpPost("Document")]
        public ActionResult<Response<DocumentResponse>> GetDocument([FromBody] DocumentRequest request)
        {
            if (ModelState.IsValid)
            {
                return GetDocumentsBLL.instance.ViewDocument(request);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        [HttpPost("UpdateDocumentSearch")]
        public ActionResult<Response<UpdateDocumentResponse>>Fetch([FromBody] UpdateDocumentRequest request)
        {
            
            
            if (ModelState.IsValid)
            {
                return GetDocumentsBLL.instance.GetDocDetails(request);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        [HttpPost("AddDocument")]
        public ActionResult<Response<AddDocumentResponse>> add([FromBody] AddDocumentRequest request)
        {
            if (ModelState.IsValid)
            {
                return GetDocumentsBLL.instance.AddDoc(request);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        [HttpPost("VerifiedDocumentDetails")]
        public ActionResult<Response<GetVerifiedDocumentResponse>> VerifiedDocumentDetails([FromBody] GetDocumentRequest request)
        {
            if (ModelState.IsValid)
            {
                return GetDocumentsBLL.instance.VerifiedDocumentDetails(request);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}

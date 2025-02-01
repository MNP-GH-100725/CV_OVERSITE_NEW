using Maibro.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Oversite.DTO.CommonMaster.Request;
using Oversite.DTO.Documents.Response;
using Oversite.DTO.Login.Request;
using Oversite.DTO.Reports.Request;
using Oversite.DTO.Reports.Response;
using Oversite.DTO.Request;
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

    public class DocumentsController : ControllerBase
    {
        IConfiguration configuration;
        public DocumentsController(IConfiguration iConfig)
        {
            configuration = iConfig;

        }
        [HttpPost("DocumentDetails")]
        public ActionResult<Response<GetDocumentResponse>> Get([FromBody] GetDocumentRequest request)
        {
            Response<GetDocumentResponse> response = new Response<GetDocumentResponse>();
            string baseUrl = configuration.GetSection("baseUrl").Value;
            response = new Helpers.ApiManager().InvokePostHttpClient<Response<GetDocumentResponse>, GetDocumentRequest>(request, baseUrl + "/api/Documents/DocumentDetails").Item1;
            return response;
        }
        [HttpPost("Document")]
        public ActionResult<Response<DocumentResponsenew>> GetDocument([FromBody] DocumentRequest request)
        {
            Oversite.DTO.Response.Response<DocumentResponsenew> response = new Oversite.DTO.Response.Response<DocumentResponsenew>();
            string baseUrl = configuration.GetSection("baseUrl").Value;
            response = new Oversite.Helpers.ApiManager().InvokePostHttpClient<Oversite.DTO.Response.Response<DocumentResponsenew>, DocumentRequest>(request, baseUrl + "/api/Documents/Document").Item1;
            return response;
        }
        [HttpPost("UpdateDocumentSearch")]
        public ActionResult<Response<UpdateDocumentResponse>> Fetch([FromBody] UpdateDocumentRequest request) 
        {
            Response<UpdateDocumentResponse> response = new Response<UpdateDocumentResponse>();
            string baseUrl = configuration.GetSection("baseUrl").Value;
            response = new Helpers.ApiManager().InvokePostHttpClient<Response<UpdateDocumentResponse>, UpdateDocumentRequest>(request, baseUrl + "/api/Documents/UpdateDocumentSearch").Item1;
            return response;

        }
        [HttpPost("AddDocument")]
        public ActionResult<Response<AddDocumentResponse>> AddDocument([FromBody] WebApiRequest webApi)
        {
            var requestContent1 = (new EncryptDecryptUtil().DecodeFrom64(webApi.apiRequest.ToString()));
            AddDocumentRequest request = new AddDocumentRequest();
            request =JsonConvert.DeserializeObject<AddDocumentRequest>(new EncryptDecryptUtil().DecryptProperty(requestContent1));
            Response<AddDocumentResponse> response = new Response<AddDocumentResponse>();
            string baseUrl = configuration.GetSection("baseUrl").Value;
            response = new Helpers.ApiManager().InvokePostHttpClient<Response<AddDocumentResponse>, AddDocumentRequest>(request, baseUrl + "/api/Documents/AddDocument").Item1;
            return response;

        }

        [HttpPost("VerifiedDocumentDetails")]
        public ActionResult<Response<GetVerifiedDocumentResponse>> VerifiedDocumentDetails([FromBody] GetDocumentRequest request)
        {
            Response<GetVerifiedDocumentResponse> response = new Response<GetVerifiedDocumentResponse>();
            string baseUrl = configuration.GetSection("baseUrl").Value;
            response = new Helpers.ApiManager().InvokePostHttpClient<Response<GetVerifiedDocumentResponse>, GetDocumentRequest>(request, baseUrl + "/api/Documents/VerifiedDocumentDetails").Item1;
            return response;
        }

        

    }
}

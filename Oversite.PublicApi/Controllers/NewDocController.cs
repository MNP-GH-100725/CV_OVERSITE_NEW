using Maibro.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Oversite.DTO.CommonMaster.Request;
using Oversite.DTO.Documents.Response;
using Oversite.DTO.Request;
using Oversite.DTO.Response;
using System.Configuration;

namespace Oversite.PublicApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewDocController : ControllerBase
    {
        IConfiguration configuration;
        public NewDocController(IConfiguration iConfig)
        {
            configuration = iConfig;

        }
        [HttpPost("AddDocument")]
        public ActionResult<Response<AddDocumentResponse>> AddDocument([FromBody] WebApiRequest webApi)
        {
            var requestContent1 = (new EncryptDecryptUtil().DecodeFrom64(webApi.apiRequest.ToString()));
            AddDocumentRequest request = new AddDocumentRequest();
            request = JsonConvert.DeserializeObject<AddDocumentRequest>(new EncryptDecryptUtil().DecryptProperty(requestContent1));
            Response<AddDocumentResponse> response = new Response<AddDocumentResponse>();
            string baseUrl = configuration.GetSection("baseUrl").Value;
            response = new Helpers.ApiManager().InvokePostHttpClient<Response<AddDocumentResponse>, AddDocumentRequest>(request, baseUrl + "/api/Documents/AddDocument").Item1;
            return response;

        }

    }
}

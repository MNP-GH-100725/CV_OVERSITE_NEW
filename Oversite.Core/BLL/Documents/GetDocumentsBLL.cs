using Newtonsoft.Json;
using Oversite.Core.DataSource.Documents;
using Oversite.DTO.CommonMaster.Request;
using Oversite.DTO.Documents.Response;
using Oversite.DTO.Request;
using Oversite.DTO.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Oversite.Core.BLL.Documents
{
    public class GetDocumentsBLL
    {
		private readonly static Lazy<GetDocumentsBLL> m_instance;
		public static GetDocumentsBLL instance
		{
			get
			{
				return GetDocumentsBLL.m_instance.Value;
			}
		}

		static GetDocumentsBLL()
		{
			GetDocumentsBLL.m_instance = new Lazy<GetDocumentsBLL>(() => new GetDocumentsBLL());

		}
		public Response<GetDocumentResponse> GetDocument(GetDocumentRequest request)
		{
			var log1 = new GetDocumentsDataSource().WriteLog("View document Data");
			log1 = new GetDocumentsDataSource().WriteLog(JsonConvert.SerializeObject(request));
			Response<GetDocumentResponse> response = new Response<GetDocumentResponse>();
			GetDocumentResponse documents = new GetDocumentResponse();
			try
			{
				documents = new GetDocumentsDataSource().GetDocument(request);
				log1 = new GetDocumentsDataSource().WriteLog(JsonConvert.SerializeObject(documents));
				if (documents.isDataAvailable)
				{
					response.Data = documents;
					response.status = ResponseTypeContants.SUCCESS;
					response.apiStatus = ApiStatusConstants.COMPLETED;
					response.responseMsg = documents.message;
				}
				else
				{
					response.status = ResponseTypeContants.FAIL;
					response.apiStatus = ApiStatusConstants.COMPLETED;
					response.responseMsg = documents.message;
				}
			}
			catch (Exception ex)
			{
				Exception exception = ex;
				response.status = "Exception";
				response.responseMsg = "Internal Server Error";
				response.SetExceptionError(ex.Message);
				log1 = new GetDocumentsDataSource().WriteLog(JsonConvert.SerializeObject(response));
			}

			return response;
		}
		public Response<DocumentResponse> ViewDocument(DocumentRequest request)
		{

			Response<DocumentResponse> response = new Response<DocumentResponse>();
			DocumentResponse documents = new DocumentResponse();
			try
			{
				documents = new GetDocumentsDataSource().ViewDocument(request);

				if (documents.isDataAvailable)
				{
					response.Data = documents;
					response.status = ResponseTypeContants.SUCCESS;
					response.apiStatus = ApiStatusConstants.COMPLETED;
					response.responseMsg = documents.message;
				}
				else
				{
					response.apiStatus = ApiStatusConstants.COMPLETED;
					response.responseMsg = documents.message;
				}
			}
			catch (Exception ex)
			{
				Exception exception = ex;
				response.status = "Exception";
				response.responseMsg = "Internal Server Error";
				response.SetExceptionError(ex.Message);
			}

			return response;
		}
		public Response<UpdateDocumentResponse> GetDocDetails(UpdateDocumentRequest request)
		{
			Response<UpdateDocumentResponse> updateDocumentResponse = new Response<UpdateDocumentResponse>();
			updateDocumentResponse = new GetDocumentsDataSource().GetDocDetails(request);
			return updateDocumentResponse;
		}
		public Response<AddDocumentResponse> AddDoc(AddDocumentRequest request)
		{
			Response<AddDocumentResponse> addDocumentResponse = new Response<AddDocumentResponse>();
			addDocumentResponse = new GetDocumentsDataSource().AddDoc(request);
			return addDocumentResponse;
		}
        public Response<GetVerifiedDocumentResponse> VerifiedDocumentDetails(GetDocumentRequest request)
        {

            Response<GetVerifiedDocumentResponse> response = new Response<GetVerifiedDocumentResponse>();
            GetVerifiedDocumentResponse documents = new GetVerifiedDocumentResponse();
            try
            {
                documents = new GetDocumentsDataSource().VerifiedDocumentDetails(request);

                if (documents.isDataAvailable)
                {
                    response.Data = documents;
                    response.status = ResponseTypeContants.SUCCESS;
                    response.apiStatus = ApiStatusConstants.COMPLETED;
                    response.responseMsg = documents.message;
                }
                else
                {
                    response.apiStatus = ApiStatusConstants.COMPLETED;
                    response.responseMsg = documents.message;
                }
            }
            catch (Exception ex)
            {
                Exception exception = ex;
                response.status = "Exception";
                response.responseMsg = "Internal Server Error";
                response.SetExceptionError(ex.Message);
            }

            return response;
        }

    }
}

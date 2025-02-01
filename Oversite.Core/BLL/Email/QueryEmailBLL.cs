using Oversite.Core.DataSource.Email;
using Oversite.DTO.Email.Request;
using Oversite.DTO.Email.Response;
using Oversite.DTO.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Oversite.Core.BLL.Email
{
   public class QueryEmailBLL
    {
		private readonly static Lazy<QueryEmailBLL> m_instance;
		public static QueryEmailBLL instance
		{
			get
			{
				return QueryEmailBLL.m_instance.Value;
			}
		}

		static QueryEmailBLL()
		{
			QueryEmailBLL.m_instance = new Lazy<QueryEmailBLL>(() => new QueryEmailBLL());

		}
		public Response<QueryEmailResponse> QueryMail()
		{

			Response<QueryEmailResponse> response = new Response<QueryEmailResponse>();
			QueryEmailResponse queryEmailResponse = new QueryEmailResponse();
			try
			{
				queryEmailResponse = new QueryEmailDataSource().QueryMail();
				response.Data = queryEmailResponse;

				//if (queryEmailResponse.isDataAvailable)
				//{
				//	response.Data = queryEmailResponse;
				//	response.status = ResponseTypeContants.SUCCESS;
				//	response.apiStatus = ApiStatusConstants.COMPLETED;
				//	response.responseMsg = queryEmailResponse.message;
				//}
				//else
				//{
				//	response.apiStatus = ApiStatusConstants.COMPLETED;
				//	response.responseMsg = queryEmailResponse.message;
				//}
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
		public Response<TotalDisResponse> TotalDisb()
		{

			Response<TotalDisResponse> response = new Response<TotalDisResponse>();
			TotalDisResponse totalDis  = new TotalDisResponse();
			try
			{
				totalDis = new QueryEmailDataSource().TotalDisb();

				if (totalDis.isDataAvailable)
				{
					response.Data = totalDis;
					response.status = ResponseTypeContants.SUCCESS;
					response.apiStatus = ApiStatusConstants.COMPLETED;
					response.responseMsg = totalDis.message;
				}
				else
				{
					response.apiStatus = ApiStatusConstants.COMPLETED;
					response.responseMsg = totalDis.message;
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


		public Response<NCHResponse> NCHMail()
		{

			Response<NCHResponse> response = new Response<NCHResponse>();
			NCHResponse nCHResponse = new NCHResponse();
			try
			{
				nCHResponse = new QueryEmailDataSource().NCHMail();

				if (nCHResponse.isDataAvailable)
				{
					response.Data = nCHResponse;
					response.status = ResponseTypeContants.SUCCESS;
					response.apiStatus = ApiStatusConstants.COMPLETED;
					response.responseMsg = nCHResponse.message;
				}
				else
				{
					response.apiStatus = ApiStatusConstants.COMPLETED;
					response.responseMsg = nCHResponse.message;
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

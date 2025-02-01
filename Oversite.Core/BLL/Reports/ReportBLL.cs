
using CVHelpers;
using Newtonsoft.Json;
using Oversite.Core.DataSource.Reports;
using Oversite.DTO.Reports.Request;
using Oversite.DTO.Reports.Response;
using Oversite.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oversite.Core.BLL.Reports
{
    public class ReportBLL
    {
        private readonly static Lazy<ReportBLL> m_instance;
        public static ReportBLL instance
        {
            get
            {
                return ReportBLL.m_instance.Value;
            }
        }
        static ReportBLL()
        {
            ReportBLL.m_instance = new Lazy<ReportBLL>(() => new ReportBLL());
        }
        

        public Response<ProductDataResponse> ProductDataReport(ProductDataRequest request)
        {
            ProductDataResponse p_response = new ProductDataResponse();
            Response<ProductDataResponse> response = new Response<ProductDataResponse>();
            try
            {

                p_response = new ProductReportDataSource().product(request);
                if (p_response.IsDataAvailable == true)
                {
                    response.Data = p_response;
                    response.status = ResponseTypeContants.SUCCESS;
                    response.apiStatus = ApiStatusConstants.COMPLETED;
                    response.responseMsg = p_response.message;
                }
                else
                {
                    response.status = ResponseTypeContants.FAIL;
                    response.apiStatus = ApiStatusConstants.COMPLETED;
                    response.responseMsg = p_response.message;
                }

            }
            catch (Exception ex)
            {
                Exception exception = ex;
                response.status = "exception";
                response.responseMsg = "Internal Server Error";
                response.SetExceptionError(ex.Message);

            }

            return response;
        }

        public Response<VerificationStatusReportResponse> VerificationStatusReportBLL(VerificationStatusReportRequest request)
        {
            VerificationStatusReportResponse verificationStatusReportResponse = new VerificationStatusReportResponse();
            Response<VerificationStatusReportResponse> response = new Response<VerificationStatusReportResponse>();
            try
            {

                verificationStatusReportResponse = new ProductReportDataSource().VerificationStatusReport(request);
                if (verificationStatusReportResponse.IsDataAvailable == true)
                {
                    response.Data = verificationStatusReportResponse;
                    response.status = ResponseTypeContants.SUCCESS;
                    response.apiStatus = ApiStatusConstants.COMPLETED;
                    response.responseMsg = verificationStatusReportResponse.message;
                }
                else
                {
                    response.status = ResponseTypeContants.FAIL;
                    response.apiStatus = ApiStatusConstants.COMPLETED;
                    response.responseMsg = verificationStatusReportResponse.message;
                }

            }
            catch (Exception ex)
            {
                Exception exception = ex;
                response.status = "exception";
                response.responseMsg = "Internal Server Error";
                response.SetExceptionError(ex.Message);

            }

            return response;
        }
    }
}

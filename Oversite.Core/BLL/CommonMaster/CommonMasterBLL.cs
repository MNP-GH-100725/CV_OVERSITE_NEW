using Oversite.Core.DataSource.CommonMaster;
using Oversite.DTO.CommonMaster.Request;
using Oversite.DTO.CommonMaster.Response;
using Oversite.DTO.Documents.Request;
using Oversite.DTO.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Oversite.Core.BLL.GeneralData
{
    public class CommonMasterBLL
    {
        public readonly static Lazy<CommonMasterBLL> m_instance;
        public static CommonMasterBLL Instance
        {
            get
            {
                return CommonMasterBLL.m_instance.Value;
            }
        }
        static CommonMasterBLL()
        {
            CommonMasterBLL.m_instance = new Lazy<CommonMasterBLL>(() => new CommonMasterBLL());
        }
        public Response<CommonMasterReponse> CommonData(CommonMasterRequest request) 
        {
            CommonMasterReponse customerMasterResponse = new CommonMasterReponse();
            Response<CommonMasterReponse> response = new Response<CommonMasterReponse>();
            try
            {
                customerMasterResponse = new CommonDataSource().GetCommonMasterData(request);

                if (customerMasterResponse.commonDataList != null)
                {
                    response.Data = customerMasterResponse;
                    response.status = ResponseTypeContants.SUCCESS;
                    response.apiStatus = ApiStatusConstants.COMPLETED;
                    response.responseMsg = ResponseTypeContants.SUCCESS;
                }
                else
                {
                    response.apiStatus = ApiStatusConstants.COMPLETED;
                    response.responseMsg = "No Data Found";
                }
            }
            catch (Exception ex)
            {
                Exception exception = ex;
                response.status = "exception";
                response.responseMsg = "internal server error";
                response.SetExceptionError(ex.Message);

            }
            return response;

        }
        public Response<GetdocMasterReponse> Getdoc(GetdocMasterRequest request)
        {
            Response<GetdocMasterReponse> dashBoardResponse = new Response<GetdocMasterReponse>();
            dashBoardResponse = new CommonDataSource().Getdoc(request);
            return dashBoardResponse;


        }


        public Response<CommonMasterReponse> SendBackTo(CommonMasterRequest request)
        {
            CommonMasterReponse customerMasterResponse = new CommonMasterReponse();
            Response<CommonMasterReponse> response = new Response<CommonMasterReponse>();
            try
            {
                customerMasterResponse = new CommonDataSource().SendBackTo(request);

                if (customerMasterResponse.commonDataList != null)
                {
                    response.Data = customerMasterResponse;
                    response.status = ResponseTypeContants.SUCCESS;
                    response.apiStatus = ApiStatusConstants.COMPLETED;
                    response.responseMsg = ResponseTypeContants.SUCCESS;
                }
                else
                {
                    response.apiStatus = ApiStatusConstants.COMPLETED;
                    response.responseMsg = "No Data Found";
                }
            }
            catch (Exception ex)
            {
                Exception exception = ex;
                response.status = "exception";
                response.responseMsg = "internal server error";
                response.SetExceptionError(ex.Message);

            }
            return response;

        }
    }
}

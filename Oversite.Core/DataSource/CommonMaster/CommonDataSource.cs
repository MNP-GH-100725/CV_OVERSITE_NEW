
using Newtonsoft.Json;
using Oversite.DTO.CommonMaster.Properties;
using Oversite.DTO.CommonMaster.Request;
using Oversite.DTO.CommonMaster.Response;
using Oversite.DTO.Documents.Request;
using Oversite.DTO.ErrorLog.Request;
using Oversite.DTO.Response;
using Oversite.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Oversite.Core.DataSource.CommonMaster
{
    public class CommonDataSource
    {
        public CommonMasterReponse GetCommonMasterData(CommonMasterRequest request)
        {
            CommonMasterReponse commonMasterReponse = new CommonMasterReponse();
            string output = JsonConvert.SerializeObject(request);
            try
            {
                string query = string.Empty;
                if (request.commonDataTypeID == 6)
                    query = "select to_char(COMMON_DATA_ID) commonDataId,COMMON_DATA_NAME commonDataName,DESCRIPTION desription from common_master where status_id = 1  and common_dataType_id = " + request.commonDataTypeID + "  order by DESCRIPTION";
                else
                    query = "select to_char(COMMON_DATA_ID) commonDataId,COMMON_DATA_NAME commonDataName,DESCRIPTION desription from common_master where (status_id = 1 or status_id = 3) and common_dataType_id = " + request.commonDataTypeID + "  order by DESCRIPTION";
                commonMasterReponse.commonDataList = new OracleHelper().GetRecords<CommonMasterProperties>(query);
                if (commonMasterReponse.commonDataList.Count > 0)
                {
                    commonMasterReponse.isDataAvailable = true;
                    commonMasterReponse.message = ResponseTypeContants.SUCCESS;
                }
                else
                {
                    commonMasterReponse.isDataAvailable = false;
                    commonMasterReponse.message = "No Data Found";
                }
            }
            catch (Exception ex)
            {
                ErrorLogRequest errorLogRequest = new Oversite.DTO.ErrorLog.Request.ErrorLogRequest();
                errorLogRequest.Function_Name = "Documents Get - GetDocument";
                errorLogRequest.V_Data = output;
                errorLogRequest.V_Exception = ex.Message;
                new ErrorLog.ErrorLogDataSource().ErrorLog(errorLogRequest);
                commonMasterReponse.message = ex.Message;
            }

            return commonMasterReponse;
        }
        public Response<GetdocMasterReponse> Getdoc(GetdocMasterRequest request)
        {
            GetdocMasterReponse getdocMasterReponse = new GetdocMasterReponse();
            Response<GetdocMasterReponse> response = new Response<GetdocMasterReponse>();
            List<GetdocProperties> doclist = new List<GetdocProperties>();
            string output = JsonConvert.SerializeObject(request);
            string query = string.Empty;
            try
            {
                //string query11 = "select c.checklist_id from  TBL_CHECKLIST_MASTER c where c.scheme_id=" + request.enterBy + "";
                //object scheme = new OracleHelper().ExecuteScalar<object>(query11);

                string query1 = "select c.checklist_id from  TBL_CHECKLIST_MASTER c where c.scheme_id="+request.schemeId+"";
                object checklist = new OracleHelper().ExecuteScalar<object>(query1);

                //query = "select to_char(t.document_id) as docType,t.document_name as docName from vw_document_master t where to_char(t.product_id)='" + request.productId + "' and t.status_id=1 /*and t.AUDIT_FLAG=1*/";
                query = "select to_char(t.document_id) as docType,t.document_name as docName from TBL_CHECKLIST_MST t where t.checklist_id=" + checklist+" and t.mandate_flag=1";

                doclist = new OracleHelper().GetRecords<GetdocProperties>(query);
                getdocMasterReponse.GetDocList = doclist;
                response.Data = getdocMasterReponse;
                response.responseMsg = "Success";
                response.apiStatus = ResponseTypeContants.SUCCESS;
                response.status = ApiStatusConstants.COMPLETED;
            }
            catch (Exception ex)
            {
                ErrorLogRequest errorLogRequest = new Oversite.DTO.ErrorLog.Request.ErrorLogRequest();
                errorLogRequest.Function_Name = "Documents Get - GetDocument";
                errorLogRequest.V_Data = output;
                errorLogRequest.V_Exception = ex.Message;
                new ErrorLog.ErrorLogDataSource().ErrorLog(errorLogRequest);
                response.responseMsg = "Failed!Please Contact The Support Team.";


            }
            return response;


        }


        public CommonMasterReponse SendBackTo(CommonMasterRequest request)
        {
            CommonMasterReponse commonMasterReponse = new CommonMasterReponse();
            string output = JsonConvert.SerializeObject(request);
            try
            {
                string query = string.Empty;
                    query = "select to_char(COMMON_DATA_ID) commonDataId,COMMON_DATA_NAME commonDataName,DESCRIPTION desription from common_master where (status_id = 1 or status_id = 3) and common_dataType_id = " + request.commonDataTypeID + "  order by DESCRIPTION";
                commonMasterReponse.commonDataList = new OracleHelper().GetRecords<CommonMasterProperties>(query);
                if (commonMasterReponse.commonDataList.Count > 0)
                {
                    commonMasterReponse.isDataAvailable = true;
                    commonMasterReponse.message = ResponseTypeContants.SUCCESS;
                }
                else
                {
                    commonMasterReponse.isDataAvailable = false;
                    commonMasterReponse.message = "No Data Found";
                }
            }
            catch (Exception ex)
            {
                ErrorLogRequest errorLogRequest = new Oversite.DTO.ErrorLog.Request.ErrorLogRequest();
                errorLogRequest.Function_Name = "Documents Get - GetDocument";
                errorLogRequest.V_Data = output;
                errorLogRequest.V_Exception = ex.Message;
                new ErrorLog.ErrorLogDataSource().ErrorLog(errorLogRequest);
                commonMasterReponse.message = ex.Message;
            }

            return commonMasterReponse;
        }

    }
}

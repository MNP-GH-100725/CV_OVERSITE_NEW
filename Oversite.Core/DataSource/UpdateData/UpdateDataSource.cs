using Dapper;
using Newtonsoft.Json;
using Oversite.Core.DataSource.General;
using Oversite.DTO.DocumentConfirm.Properties;
using Oversite.DTO.ErrorLog.Request;
using Oversite.DTO.Response;
using Oversite.DTO.UpdateData;
using Oversite.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Oversite.Core.DataSource.UpdateData
{
    public class UpdateDataSource
    {
        public Response<UpdateDataResponse> update(UpdateDataRequest request)
        {
            Response<UpdateDataResponse>response = new Response<UpdateDataResponse>();
            string query = string.Empty;
            string query1 = string.Empty;
            List<string> cmd = new List<string>();
                cmd.Clear();
            List<UpdateDataProperties> DataProperties = new List<UpdateDataProperties>();
            string output = JsonConvert.SerializeObject(request);
            try
            {
                //List<decimal> role_id = new List<decimal>();

                //role_id = new OracleHelper().GetRecords<decimal>("select distinct t1.Role_Id from USER_ROLE_DETAILS t1 left outer join user_master t2 ON  t1.user_id = t2.user_id and t2.product_id = " + request.productId + "  WHERE t2.emp_code = '" + request.enterBy + "' order by t1.role_id asc");
                //decimal rolecnt = new OracleHelper().ExecuteScalar<decimal>("select count(*) from USER_ROLE_DETAILS  t1 left outer join user_master t2 ON  t1.user_id = t2.user_id and t2.product_id = " + request.productId + "  WHERE t2.emp_code = '" + request.enterBy + "' and t1.role_id=49");
                //decimal rolecnt1 = new OracleHelper().ExecuteScalar<decimal>("select count(*) from USER_ROLE_DETAILS  t1 left outer join user_master t2 ON  t1.user_id = t2.user_id and t2.product_id = " + request.productId + "  WHERE t2.emp_code = '" + request.enterBy + "' and t1.role_id=50");
                decimal cvrolenho = new OracleHelper().ExecuteScalar<decimal>("select count(*) from vw_cv_user_role_details  t1 left outer join vw_cv_user_master t2 ON  t1.user_id = t2.user_id  WHERE t2.emp_code = '" + request.enterBy + "' and t1.role_id=81");//oversite nho
                decimal cvrolench = new OracleHelper().ExecuteScalar<decimal>("select count(*) from vw_cv_user_role_details  t1 left outer join vw_cv_user_master t2 ON  t1.user_id = t2.user_id WHERE t2.emp_code = '" + request.enterBy + "' and t1.role_id=77"); //oversite nch
                decimal cvrolebcm = new OracleHelper().ExecuteScalar<decimal>("select count(*) from vw_cv_user_role_details  t1 left outer join vw_cv_user_master t2 ON  t1.user_id = t2.user_id WHERE t2.emp_code = '" + request.enterBy + "' and t1.role_id=79");//BCM//oversite bch
                decimal cvrolerch = new OracleHelper().ExecuteScalar<decimal>("select count(*) from vw_cv_user_role_details  t1 left outer join vw_cv_user_master t2 ON  t1.user_id = t2.user_id WHERE t2.emp_code = '" + request.enterBy + "' and t1.role_id=78");//RCH //oversite rch
                decimal rho = new OracleHelper().ExecuteScalar<decimal>("select count(*) from vw_cv_user_role_details  t1 left outer join vw_cv_user_master t2 ON  t1.user_id = t2.user_id WHERE t2.emp_code = '" + request.enterBy + "' and t1.role_id=80");//ROH//oversite rho
                decimal BCO = new OracleHelper().ExecuteScalar<decimal>("select count(*) from vw_cv_user_role_details  t1 left outer join vw_cv_user_master t2 ON  t1.user_id = t2.user_id WHERE t2.emp_code = '" + request.enterBy + "' and t1.role_id=82");//bco//oversite bho
                decimal role_cvnho_ass_cnt = new OracleHelper().ExecuteScalar<decimal>("select count(*) from lms_tw.vw_cv_user_role_details  t1 left outer join lms_tw.vw_cv_user_master t2 ON  t1.user_id = t2.user_id  WHERE t2.emp_code = '" + request.enterBy + "' and t1.role_id=84");


                //docSeq = docSeq + 1;
                for (int i = 0; i < request.docupdatelist.Count; i++)
                {
                    

                    if (cvrolenho > 0 || role_cvnho_ass_cnt > 0)  //NHO
                    {
                        decimal sendback = new OracleHelper().ExecuteScalar<decimal>("select count(*) from  TBL_OVERSITE_DOCUMENTS_mst t  where t.application_id=" + request.docupdatelist[i].applicationId + " and t.document_type=" + request.docupdatelist[i].documentType + "and t.customerid=" + request.docupdatelist[i].customerId + " and t.nch_sendback_flag =1");
                        if (sendback > 0)
                        {

                            response.responseMsg = "Already SendBack By NCH";
                            response.apiStatus = ApiStatusConstants.NOT_COMPLETED;
                            response.status = ResponseTypeContants.FAIL;
                            return response;

                        }
                        decimal sendback1 = new OracleHelper().ExecuteScalar<decimal>("select count(*) from  TBL_OVERSITE_DOCUMENTS_mst t  where t.application_id=" + request.docupdatelist[i].applicationId + " and t.document_type=" + request.docupdatelist[i].documentType + "and t.customerid=" + request.docupdatelist[i].customerId + " and t.nch_sendback_flag =2 and t.nch_verify_by is null");
                        if (sendback1 > 0)
                        {

                            response.responseMsg = "Already SendBack By NCH";
                            response.apiStatus = ApiStatusConstants.NOT_COMPLETED;
                            response.status = ResponseTypeContants.FAIL;
                            return response;

                        }
                        //sendback = new OracleHelper().ExecuteScalar<decimal>("select count(*) from  TBL_OVERSITE_DOCUMENTS_mst t  where t.application_id=" + request.docupdatelist[i].applicationId + " and t.document_type=" + request.docupdatelist[i].documentType + "and t.customerid=" + request.docupdatelist[i].customerId + " and t.nho_sendback_flag =1");

                        //if (sendback > 0)
                        //{

                        //    response.responseMsg = "Already SendBack";
                        //    response.apiStatus = ApiStatusConstants.NOT_COMPLETED;
                        //    response.status = ResponseTypeContants.FAIL;
                        //    return response;

                        //}
                        //sendback = new OracleHelper().ExecuteScalar<decimal>("select count(*) from  TBL_OVERSITE_DOCUMENTS_mst t  where t.application_id=" + request.docupdatelist[i].applicationId + " and t.document_type=" + request.docupdatelist[i].documentType + "and t.customerid=" + request.docupdatelist[i].customerId + " and t.nho_verify_by  is not null");
                        //if (sendback > 0)
                        //{

                        //    response.responseMsg = "Already Verified";
                        //    response.apiStatus = ApiStatusConstants.NOT_COMPLETED;
                        //    response.status = ResponseTypeContants.FAIL;
                        //    return response;

                        //}

                        if (request.docStatus == "V")
                        {
                            int newstatus = 4;
                            decimal nchcheck = new OracleHelper().ExecuteScalar<decimal>("select count(*) from  TBL_OVERSITE_DOCUMENTS_mst t  where t.application_id=" + request.docupdatelist[i].applicationId + " and t.document_type=" + request.docupdatelist[i].documentType + "and t.customerid=" + request.docupdatelist[i].customerId + " and t.nch_verify_by is not null");
                            if (nchcheck == 1)
                            {
                                cmd.Add("update TBL_OVERSITE_DOCUMENTS_mst t set  t.VERIFY_FLAG = " + newstatus + ",t.doc_remarks='" + request.docRemarks + "',t.nho_verified_date=sysdate,t.nho_verify_by=" + request.enterBy + ",t.nho_verified_remarks='" + request.docRemarks + "',DOC_STATUS=1 where  t.APPLICATION_ID=" + request.docupdatelist[i].applicationId + " and t.DOCUMENT_TYPE=" + request.docupdatelist[i].documentType + " and customerid='" + request.docupdatelist[i].customerId + "'");

                            }
                            else
                            {
                                cmd.Add("update TBL_OVERSITE_DOCUMENTS_mst t set  t.VERIFY_FLAG = " + newstatus + ",t.doc_remarks='" + request.docRemarks + "',t.nho_verified_date=sysdate,t.nho_verify_by=" + request.enterBy + ",t.nho_verified_remarks='" + request.docRemarks + "' where  t.APPLICATION_ID=" + request.docupdatelist[i].applicationId + " and t.DOCUMENT_TYPE=" + request.docupdatelist[i].documentType + "and customerid='" + request.docupdatelist[i].customerId + "'");

                            }
                        }
                        if (request.docStatus == "S")
                        {
                            int newstatus = 5;
                            List<DocReConfirmProperties> DataPropertiesnew = new List<DocReConfirmProperties>();
                            cmd.Add("update TBL_OVERSITE_DOCUMENTS_mst t set t.VERIFY_FLAG = " + newstatus + ",t.nho_sendback_flag =1,t.sendback_remarks='" + request.docRemarks + "',t.sendback_by=" + request.enterBy + ",t.sendback_dt=sysdate,t.rho_verify_by=null,t.rho_verify_flag =null,t.rho_verified_date=null where  t.APPLICATION_ID=" + request.docupdatelist[i].applicationId + " and t.DOCUMENT_TYPE=" + request.docupdatelist[i].documentType + " and customerid='" + request.docupdatelist[i].customerId + "'");
                            decimal seq_ID = new OracleHelper().ExecuteScalar<decimal>("select nvl(max(sequence_id ),0)+1  from TBL_OVERSITE_DOCUMENTS_DTL where  application_id = '" + request.docupdatelist[i].applicationId + "' ");
                            cmd.Add("insert into TBL_OVERSITE_DOCUMENTS_dtl (APPLICATION_ID,LOAN_ID,customerid,DOCUMENT_NO,DOCUMENT_TYPE,VERIFICATION_STATUS,VERIFY_DATE,VERIFY_BY,VERIFY_REMARKS,SENDBACK_FLAG,PRODUCT_ID,SEQUENCE_ID,role_id)" +
                                "values (" + request.docupdatelist[i].applicationId + "," + request.docupdatelist[i].loanId + ",'" + request.docupdatelist[i].customerId + "','" + request.docupdatelist[i].documentNo + "'," + request.docupdatelist[i].documentType + "," + newstatus + ",sysdate," + request.enterBy + ",'" + request.docRemarks + "',1," + request.productId + "," + seq_ID + ",49)");
                        }

                        new OracleHelper().ExecuteNonQuerynew(cmd.ToArray(), OracleHelper.SQLMode.Query, null);
                        response.responseMsg = "Success";
                        response.apiStatus = ApiStatusConstants.COMPLETED;
                        response.status = ResponseTypeContants.SUCCESS;


                    }
                    else if (/*rolecnt1 > 0 || */cvrolench > 0)//NCH
                    {
                        decimal sendback = new OracleHelper().ExecuteScalar<decimal>("select count(*) from  TBL_OVERSITE_DOCUMENTS_mst t  where t.application_id=" + request.docupdatelist[i].applicationId + " and t.document_type=" + request.docupdatelist[i].documentType + "and t.customerid=" + request.docupdatelist[i].customerId + " and t.nho_sendback_flag =1");
                        //decimal sendback1 = new OracleHelper().ExecuteScalar<decimal>("select count(*) from  TBL_OVERSITE_DOCUMENTS_mst t  where t.application_id=" + request.docupdatelist[i].applicationId + " and t.document_type=" + request.docupdatelist[i].documentType + "and t.customerid=" + request.docupdatelist[i].customerId + " and t.nho_sendback_flag =1");
                        if (sendback > 0)
                        {

                            response.responseMsg = "Already SendBack By NHO";
                            response.apiStatus = ApiStatusConstants.NOT_COMPLETED;
                            response.status = ResponseTypeContants.FAIL;
                            return response;


                        }

                        decimal sendback1 = new OracleHelper().ExecuteScalar<decimal>("select count(*) from  TBL_OVERSITE_DOCUMENTS_mst t  where t.application_id=" + request.docupdatelist[i].applicationId + " and t.document_type=" + request.docupdatelist[i].documentType + "and t.customerid=" + request.docupdatelist[i].customerId + " and t.nho_sendback_flag =2 and t.nho_verify_by is null");
                        if (sendback1 > 0)
                        {
                            response.responseMsg = "Already SendBack By NHO";
                            response.apiStatus = ApiStatusConstants.NOT_COMPLETED;
                            response.status = ResponseTypeContants.FAIL;
                            return response;

                        }
                        sendback = new OracleHelper().ExecuteScalar<decimal>("select count(*) from  TBL_OVERSITE_DOCUMENTS_mst t  where t.application_id=" + request.docupdatelist[i].applicationId + " and t.document_type=" + request.docupdatelist[i].documentType + "and t.customerid=" + request.docupdatelist[i].customerId + " and t.nch_sendback_flag =1");

                        if (sendback > 0)
                        {

                            response.responseMsg = "Already SendBack";
                            response.apiStatus = ApiStatusConstants.NOT_COMPLETED;
                            response.status = ResponseTypeContants.FAIL;
                            return response;

                        }
                        sendback = new OracleHelper().ExecuteScalar<decimal>("select count(*) from  TBL_OVERSITE_DOCUMENTS_mst t  where t.application_id=" + request.docupdatelist[i].applicationId + " and t.document_type=" + request.docupdatelist[i].documentType + "and t.customerid=" + request.docupdatelist[i].customerId + " and t.nch_verify_by  is not null");
                        if (sendback > 0)
                        {

                            response.responseMsg = "Already Verified";
                            response.apiStatus = ApiStatusConstants.NOT_COMPLETED;
                            response.status = ResponseTypeContants.FAIL;
                            return response;

                        }
                        //decimal sendback1 = new OracleHelper().ExecuteScalar<decimal>("select count(*) from  TBL_OVERSITE_DOCUMENTS_mst t  where t.application_id=" + request.docupdatelist[i].applicationId + " and t.document_type=" + request.docupdatelist[i].documentType + "and t.customerid=" + request.docupdatelist[i].customerId + " and t.nch_sendback_flag =1");
                        //if (sendback1 == 0)
                        //{
                        if (request.docStatus == "V")
                        {
                            int newstatus = 2;
                            decimal nhocheck = new OracleHelper().ExecuteScalar<decimal>("select count(*) from  TBL_OVERSITE_DOCUMENTS_mst t  where t.application_id=" + request.docupdatelist[i].applicationId + " and t.document_type=" + request.docupdatelist[i].documentType + "and t.customerid=" + request.docupdatelist[i].customerId + " and t.NHO_VERIFY_BY is not null");
                            if (nhocheck == 1)
                            {
                                cmd.Add("update TBL_OVERSITE_DOCUMENTS_mst t set  t.VERIFY_FLAG = " + newstatus + ",t.doc_remarks='" + request.docRemarks + "',t.NCH_VERIFY_DATE=sysdate,t.NCH_VERIFY_BY=" + request.enterBy + ",t.NCH_VERIFY_REMARKS='" + request.docRemarks + "',DOC_STATUS=1  where  t.APPLICATION_ID=" + request.docupdatelist[i].applicationId + " and t.DOCUMENT_TYPE=" + request.docupdatelist[i].documentType + " and customerid='" + request.docupdatelist[i].customerId + "'");

                            }
                            else
                            {
                                cmd.Add("update TBL_OVERSITE_DOCUMENTS_mst t set  t.VERIFY_FLAG = " + newstatus + ",t.doc_remarks='" + request.docRemarks + "',t.NCH_VERIFY_DATE=sysdate,t.NCH_VERIFY_BY=" + request.enterBy + ",t.NCH_VERIFY_REMARKS='" + request.docRemarks + "' where  t.APPLICATION_ID=" + request.docupdatelist[i].applicationId + " and t.DOCUMENT_TYPE=" + request.docupdatelist[i].documentType + " and customerid='" + request.docupdatelist[i].customerId + "'");

                            }


                        }
                        if (request.docStatus == "S")
                        {
                            decimal docSeq = new OracleHelper().ExecuteScalar<decimal>("select nvl(max(t.sequence_id),0)+1  from TBL_OVERSITE_DOCUMENTS_DTL t where t.application_id='" + request.docupdatelist[i].applicationId + "'");

                            int newstatus = 3;
                            List<DocReConfirmProperties> DataPropertiesnew = new List<DocReConfirmProperties>();
                            cmd.Add("update TBL_OVERSITE_DOCUMENTS_mst t set t.VERIFY_FLAG = " + newstatus + ",t.nch_sendback_flag =1,t.sendback_remarks='" + request.docRemarks + "',t.sendback_by=" + request.enterBy + ",t.sendback_dt=sysdate,t.NCH_SENDBACK_TO='"+request.docupdatelist[i].sendbackto+ "',NCH_SENDBACK_REMARKS='" + request.docRemarks + "' where  t.APPLICATION_ID=" + request.docupdatelist[i].applicationId + "  and t.DOCUMENT_TYPE=" + request.docupdatelist[i].documentType + " and customerid='" + request.docupdatelist[i].customerId + "'");
                            decimal seq_ID = new OracleHelper().ExecuteScalar<decimal>("select nvl(max(sequence_id ),0)+1  from TBL_OVERSITE_DOCUMENTS_DTL where  application_id = '" + request.docupdatelist[i].applicationId + "' ");
                            cmd.Add("insert into TBL_OVERSITE_DOCUMENTS_dtl (APPLICATION_ID,LOAN_ID,CUSTOMERID,DOCUMENT_NO,DOCUMENT_TYPE,VERIFICATION_STATUS,VERIFY_DATE,VERIFY_BY,VERIFY_REMARKS,SENDBACK_FLAG,PRODUCT_ID,SEQUENCE_ID,role_id)" +
                                "values (" + request.docupdatelist[i].applicationId + "," + request.docupdatelist[i].loanId + ",'" + request.docupdatelist[i].customerId + "','" + request.docupdatelist[i].documentNo + "'," + request.docupdatelist[i].documentType + "," + newstatus + ",sysdate," + request.enterBy + ",'" + request.docRemarks + "',1," + request.productId + "," + seq_ID + ",50)");
                        }
                        new OracleHelper().ExecuteNonQuerynew(cmd.ToArray(), OracleHelper.SQLMode.Query, null);
                        //}
                        //else
                        //{
                        response.responseMsg = "Success";
                        response.apiStatus = ApiStatusConstants.COMPLETED;
                        response.status = ResponseTypeContants.SUCCESS;
                          
                            //}
                     
                    }





                    //BCM
                    else if (/*rolecnt1 > 0 || */cvrolebcm > 0)//BCM
                    {

                        if (request.docStatus == "V")
                        {
                            int newstatus = 9;

                            cmd.Add("update TBL_OVERSITE_DOCUMENTS_mst t set  t.VERIFY_FLAG = " + newstatus + ",t.Bcm_Verify_Date=sysdate,t.Bcm_Verify_By=" + request.enterBy + ",t.Bcm_Verify_Remarks='" + request.docRemarks + "' where  t.APPLICATION_ID=" + request.docupdatelist[i].applicationId + " and t.DOCUMENT_TYPE=" + request.docupdatelist[i].documentType + " and customerid='" + request.docupdatelist[i].customerId + "'");

                        }
                        new OracleHelper().ExecuteNonQuerynew(cmd.ToArray(), OracleHelper.SQLMode.Query, null);
                        //}
                        //else
                        //{
                        response.responseMsg = "Success";
                        response.apiStatus = ApiStatusConstants.COMPLETED;
                        response.status = ResponseTypeContants.SUCCESS;

                        //}

                    }
                    else if (BCO > 0)//BCO
                    {

                        if (request.docStatus == "V")
                        {
                            int newstatus = 13;

                            cmd.Add("update TBL_OVERSITE_DOCUMENTS_mst t set  t.VERIFY_FLAG = " + newstatus + ",t.bco_verify_date=sysdate,t.bco_verify_by=" + request.enterBy + ",t.bco_verify_remarks='" + request.docRemarks + "' where  t.APPLICATION_ID=" + request.docupdatelist[i].applicationId + " and t.DOCUMENT_TYPE=" + request.docupdatelist[i].documentType + " and customerid='" + request.docupdatelist[i].customerId + "'");

                        }
                        new OracleHelper().ExecuteNonQuerynew(cmd.ToArray(), OracleHelper.SQLMode.Query, null);
                        //}
                        //else
                        //{
                        response.responseMsg = "Success";
                        response.apiStatus = ApiStatusConstants.COMPLETED;
                        response.status = ResponseTypeContants.SUCCESS;

                        //}

                    }
                    //100890 s
                    else if(cvrolerch >0 && rho >0)
                    {
                        decimal sendback = new OracleHelper().ExecuteScalar<decimal>("select count(*) from  TBL_OVERSITE_DOCUMENTS_mst t  where t.application_id=" + request.docupdatelist[i].applicationId + " and t.document_type=" + request.docupdatelist[i].documentType + "and t.customerid=" + request.docupdatelist[i].customerId + " and t.RCH_sendback_flag =1");

                        if (sendback > 0)
                        {

                            response.responseMsg = "Already SendBack";
                            response.apiStatus = ApiStatusConstants.NOT_COMPLETED;
                            response.status = ResponseTypeContants.FAIL;
                            return response;

                        }
                        sendback = new OracleHelper().ExecuteScalar<decimal>("select count(*) from  TBL_OVERSITE_DOCUMENTS_mst t  where t.application_id=" + request.docupdatelist[i].applicationId + " and t.document_type=" + request.docupdatelist[i].documentType + "and t.customerid=" + request.docupdatelist[i].customerId + " and t.RCH_verify_by  is not null");
                        if (sendback > 0)
                        {

                            response.responseMsg = "Already Verified";
                            response.apiStatus = ApiStatusConstants.NOT_COMPLETED;
                            response.status = ResponseTypeContants.FAIL;
                            return response;

                        }

                        if (request.docStatus == "V")
                        {
                            int newstatus = 17;

                            cmd.Add("update TBL_OVERSITE_DOCUMENTS_mst t set  t.VERIFY_FLAG = " + newstatus + ",t.RCH_Verify_Date=sysdate,t.RCH_Verify_By=" + request.enterBy + ",t.RCH_Verify_Remarks='" + request.docRemarks + "'" +
                                ",t.RHO_VERIFIED_DATE=sysdate,RHO_VERIFY_FLAG=1,t.rho_verify_by=" + request.enterBy + ",t.rho_remarks='" + request.docRemarks + "' where  t.APPLICATION_ID=" + request.docupdatelist[i].applicationId + " and t.DOCUMENT_TYPE=" + request.docupdatelist[i].documentType + " and customerid='" + request.docupdatelist[i].customerId + "'");

                        }
                        if (request.docStatus == "S")
                        {
                            //decimal docSeq = new OracleHelper().ExecuteScalar<decimal>("select nvl(max(t.sequence_id),0)+1  from TBL_OVERSITE_DOCUMENTS_DTL t where t.application_id='" + request.docupdatelist[i].applicationId + "'");

                            int newstatus = 16;
                            List<DocReConfirmProperties> DataPropertiesnew = new List<DocReConfirmProperties>();
                            cmd.Add("update TBL_OVERSITE_DOCUMENTS_mst t set t.VERIFY_FLAG = " + newstatus + ",t.RCH_sendback_flag =1,t.RCH_SENDBACK_REMARKS='" + request.docRemarks + "',t.rho_sendback_flag =1,t.rho_sendback_remarks='" + request.docRemarks + "',t.RHO_SENDBACK_DATE=sysdate,t.RHO_SENDBACK_BY=" + request.enterBy + "  where  t.APPLICATION_ID=" + request.docupdatelist[i].applicationId + "  and t.DOCUMENT_TYPE=" + request.docupdatelist[i].documentType + " and customerid='" + request.docupdatelist[i].customerId + "'");
                            decimal seq_ID = new OracleHelper().ExecuteScalar<decimal>("select nvl(max(sequence_id ),0)+1  from TBL_OVERSITE_DOCUMENTS_DTL where  application_id = '" + request.docupdatelist[i].applicationId + "' ");
                            cmd.Add("insert into TBL_OVERSITE_DOCUMENTS_dtl (APPLICATION_ID,LOAN_ID,CUSTOMERID,DOCUMENT_NO,DOCUMENT_TYPE,VERIFICATION_STATUS,VERIFY_DATE,VERIFY_BY,VERIFY_REMARKS,SENDBACK_FLAG,PRODUCT_ID,SEQUENCE_ID,role_id)" +
                                "values (" + request.docupdatelist[i].applicationId + "," + request.docupdatelist[i].loanId + ",'" + request.docupdatelist[i].customerId + "','" + request.docupdatelist[i].documentNo + "'," + request.docupdatelist[i].documentType + "," + newstatus + ",sysdate," + request.enterBy + ",'" + request.docRemarks + "',1," + request.productId + "," + seq_ID + ",7880)");
                        }
                         new OracleHelper().ExecuteNonQuerynew(cmd.ToArray(), OracleHelper.SQLMode.Query, null);
                        //}
                        //else
                        //{
                        response.responseMsg = "Success";
                        response.apiStatus = ApiStatusConstants.COMPLETED;
                        response.status = ResponseTypeContants.SUCCESS;

                        //}
                    }
                    //100890 e
                    else if (/*rolecnt1 > 0 || */cvrolerch > 0)//RCH
                    {

                        decimal sendback = new OracleHelper().ExecuteScalar<decimal>("select count(*) from  TBL_OVERSITE_DOCUMENTS_mst t  where t.application_id=" + request.docupdatelist[i].applicationId + " and t.document_type=" + request.docupdatelist[i].documentType + "and t.customerid=" + request.docupdatelist[i].customerId + " and t.RCH_sendback_flag =1");

                        if (sendback > 0)
                        {

                            response.responseMsg = "Already SendBack";
                            response.apiStatus = ApiStatusConstants.NOT_COMPLETED;
                            response.status = ResponseTypeContants.FAIL;
                            return response;

                        }
                        sendback = new OracleHelper().ExecuteScalar<decimal>("select count(*) from  TBL_OVERSITE_DOCUMENTS_mst t  where t.application_id=" + request.docupdatelist[i].applicationId + " and t.document_type=" + request.docupdatelist[i].documentType + "and t.customerid=" + request.docupdatelist[i].customerId + " and t.RCH_verify_by  is not null");
                        if (sendback > 0)
                        {

                            response.responseMsg = "Already Verified";
                            response.apiStatus = ApiStatusConstants.NOT_COMPLETED;
                            response.status = ResponseTypeContants.FAIL;
                            return response;

                        }

                        if (request.docStatus == "V")
                        {
                            int newstatus = 8;

                            cmd.Add("update TBL_OVERSITE_DOCUMENTS_mst t set  t.VERIFY_FLAG = " + newstatus + ",t.RCH_Verify_Date=sysdate,t.RCH_Verify_By=" + request.enterBy + ",t.RCH_Verify_Remarks='" + request.docRemarks + "' where  t.APPLICATION_ID=" + request.docupdatelist[i].applicationId + " and t.DOCUMENT_TYPE=" + request.docupdatelist[i].documentType + " and customerid='" + request.docupdatelist[i].customerId + "'");

                        }
                        if (request.docStatus == "S")
                        {
                            decimal docSeq = new OracleHelper().ExecuteScalar<decimal>("select nvl(max(t.sequence_id),0)+1  from TBL_OVERSITE_DOCUMENTS_DTL t where t.application_id='" + request.docupdatelist[i].applicationId + "'");

                            int newstatus = 10;
                            List<DocReConfirmProperties> DataPropertiesnew = new List<DocReConfirmProperties>();
                            cmd.Add("update TBL_OVERSITE_DOCUMENTS_mst t set t.VERIFY_FLAG = " + newstatus + ",t.RCH_sendback_flag =1,t.RCH_SENDBACK_REMARKS='" + request.docRemarks + "' where  t.APPLICATION_ID=" + request.docupdatelist[i].applicationId + "  and t.DOCUMENT_TYPE=" + request.docupdatelist[i].documentType + " and customerid='" + request.docupdatelist[i].customerId + "'");
                            decimal seq_ID = new OracleHelper().ExecuteScalar<decimal>("select nvl(max(sequence_id ),0)+1  from TBL_OVERSITE_DOCUMENTS_DTL where  application_id = '" + request.docupdatelist[i].applicationId + "' ");
                            cmd.Add("insert into TBL_OVERSITE_DOCUMENTS_dtl (APPLICATION_ID,LOAN_ID,CUSTOMERID,DOCUMENT_NO,DOCUMENT_TYPE,VERIFICATION_STATUS,VERIFY_DATE,VERIFY_BY,VERIFY_REMARKS,SENDBACK_FLAG,PRODUCT_ID,SEQUENCE_ID,role_id)" +
                                "values (" + request.docupdatelist[i].applicationId + "," + request.docupdatelist[i].loanId + ",'" + request.docupdatelist[i].customerId + "','" + request.docupdatelist[i].documentNo + "'," + request.docupdatelist[i].documentType + "," + newstatus + ",sysdate," + request.enterBy + ",'" + request.docRemarks + "',1," + request.productId + "," + seq_ID + ",50)");
                        }
                        new OracleHelper().ExecuteNonQuerynew(cmd.ToArray(), OracleHelper.SQLMode.Query, null);
                        //}
                        //else
                        //{
                        response.responseMsg = "Success";
                        response.apiStatus = ApiStatusConstants.COMPLETED;
                        response.status = ResponseTypeContants.SUCCESS;

                        //}

                    }
                    else if (rho > 0)//ROH
                    {

                        decimal sendback = new OracleHelper().ExecuteScalar<decimal>("select count(*) from  TBL_OVERSITE_DOCUMENTS_mst t  where t.application_id=" + request.docupdatelist[i].applicationId + " and t.document_type=" + request.docupdatelist[i].documentType + "and t.customerid=" + request.docupdatelist[i].customerId + " and t.rho_sendback_flag =1");

                        if (sendback > 0)
                        {

                            response.responseMsg = "Already SendBack";
                            response.apiStatus = ApiStatusConstants.NOT_COMPLETED;
                            response.status = ResponseTypeContants.FAIL;
                            return response;

                        }
                        sendback = new OracleHelper().ExecuteScalar<decimal>("select count(*) from  TBL_OVERSITE_DOCUMENTS_mst t  where t.application_id=" + request.docupdatelist[i].applicationId + " and t.document_type=" + request.docupdatelist[i].documentType + "and t.customerid=" + request.docupdatelist[i].customerId + " and t.rho_verify_by  is not null");
                        if (sendback > 0)
                        {

                            response.responseMsg = "Already Verified";
                            response.apiStatus = ApiStatusConstants.NOT_COMPLETED;
                            response.status = ResponseTypeContants.FAIL;
                            return response;

                        }

                        if (request.docStatus == "V")
                        {
                            int newstatus = 11;

                            cmd.Add("update TBL_OVERSITE_DOCUMENTS_mst t set  t.VERIFY_FLAG = " + newstatus + ",t.RHO_VERIFIED_DATE=sysdate,RHO_VERIFY_FLAG=1,t.rho_verify_by=" + request.enterBy + ",t.rho_remarks='" + request.docRemarks + "' where  t.APPLICATION_ID=" + request.docupdatelist[i].applicationId + " and t.DOCUMENT_TYPE=" + request.docupdatelist[i].documentType + " and customerid='" + request.docupdatelist[i].customerId + "'");

                        }
                        if (request.docStatus == "S")
                        {
                            decimal docSeq = new OracleHelper().ExecuteScalar<decimal>("select nvl(max(t.sequence_id),0)+1  from TBL_OVERSITE_DOCUMENTS_DTL t where t.application_id='" + request.docupdatelist[i].applicationId + "'");

                            int newstatus = 12;
                            List<DocReConfirmProperties> DataPropertiesnew = new List<DocReConfirmProperties>();
                            cmd.Add("update TBL_OVERSITE_DOCUMENTS_mst t set t.VERIFY_FLAG = " + newstatus + ",t.rho_sendback_flag =1,t.rho_sendback_remarks='" + request.docRemarks + "',t.RHO_SENDBACK_DATE=sysdate,t.RHO_SENDBACK_BY=" + request.enterBy + " where  t.APPLICATION_ID=" + request.docupdatelist[i].applicationId + "  and t.DOCUMENT_TYPE=" + request.docupdatelist[i].documentType + " and customerid='" + request.docupdatelist[i].customerId + "'");
                            decimal seq_ID = new OracleHelper().ExecuteScalar<decimal>("select nvl(max(sequence_id ),0)+1  from TBL_OVERSITE_DOCUMENTS_DTL where  application_id = '" + request.docupdatelist[i].applicationId + "' ");
                            cmd.Add("insert into TBL_OVERSITE_DOCUMENTS_dtl (APPLICATION_ID,LOAN_ID,CUSTOMERID,DOCUMENT_NO,DOCUMENT_TYPE,VERIFICATION_STATUS,VERIFY_DATE,VERIFY_BY,VERIFY_REMARKS,SENDBACK_FLAG,PRODUCT_ID,SEQUENCE_ID,role_id)" +
                                "values (" + request.docupdatelist[i].applicationId + "," + request.docupdatelist[i].loanId + ",'" + request.docupdatelist[i].customerId + "','" + request.docupdatelist[i].documentNo + "'," + request.docupdatelist[i].documentType + "," + newstatus + ",sysdate," + request.enterBy + ",'" + request.docRemarks + "',1," + request.productId + "," + seq_ID + ",52)");
                        }
                        new OracleHelper().ExecuteNonQuerynew(cmd.ToArray(), OracleHelper.SQLMode.Query, null);
                        response.responseMsg = "Success";
                        response.apiStatus = ApiStatusConstants.COMPLETED;
                        response.status = ResponseTypeContants.SUCCESS;
                    }
                    else
                    {
                        response.responseMsg = "You Have No Permission";
                        response.apiStatus = ApiStatusConstants.NOT_COMPLETED;
                        response.status = ResponseTypeContants.SUCCESS;
                    }


                }
                //query = "select m.application_id applicationId, m.loan_id loanId, m.document_no documentNo, m.document_type documentType, m.old_document_no oldDocumentNo, m.nch_verify_date nchVerifyDate, m.nch_verify_by nchVerifyBy, m.nch_verify_remarks nchVerifyRemarks, m.nho_verified_date nhoVerifiedDate, " +
                //    "m.nho_verify_by nhoVerifyBy, m.nho_verified_remarks nhoVerifiedRemarks, m.verify_flag verifyFlag, m.doc_status docStatus, m.doc_remarks docRemarks, m.sendback_remarks sendbackRemarks, m.sendback_by sendbackBy, m.sendback_dt sendbackDt, m.recaptured_remarks recapturedRemarks, m.recaptured_by recapturedBy," +
                //    " m.recaptured_dt recapturedDt, m.sendback_flag sendbackFlag, m.product_id productId from TBL_OVERSITE_DOCUMENTS_mst m where t.old_document_no = " + request.documentId + "";
                //response.UpdateData = new OracleHelper().GetRecords<UpdateDataProperties>(query);





                //new OracleHelper().ExecuteNonQuerynew(cmd.ToArray(), OracleHelper.SQLMode.Query, null);
             

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
    }
}


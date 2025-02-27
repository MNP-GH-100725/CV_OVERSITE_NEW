using Newtonsoft.Json;
using Oversite.DTO.CommonMaster.Properties;
using Oversite.DTO.CommonMaster.Response;
using Oversite.DTO.DocumentConfirm.Properties;
using Oversite.DTO.DocumentConfirm.Request;
using Oversite.DTO.DocumentConfirm.Response;
using Oversite.DTO.Documents.Request;
using Oversite.DTO.ErrorLog.Request;
using Oversite.DTO.Response;
using Oversite.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Oversite.Core.DataSource.DocumentConfirm
{
    public  class ConfirmDataSource
    {
        public Response<DocumentConfirmResponse> confirm(DocumentConfirmRequest request)
        {
            DocumentConfirmResponse documentConfirmResponse = new DocumentConfirmResponse();
            Response<DocumentConfirmResponse> response = new Response<DocumentConfirmResponse>();
            TATDetailsProperties tATDetails = new TATDetailsProperties();

            string query = string.Empty;
            string query1 = string.Empty;
            List<string> cmd = new List<string>();
            cmd.Clear();
            string output = JsonConvert.SerializeObject(request);
            try
            {
                string tatquery = "";
                tatquery = "select ph.application_id,ph.loan_id,to_char(ph.disbursed_date) disbursed_date,ph.TAT,ph.product_id,ph.firm_id,ph.region_id from lms_tw.vw_public_holiday ph where (ph.application_id='" + request.searchType+"' or ph.loan_id='"+request.searchType+"')";
                tATDetails= new OracleHelper().GetRecord<TATDetailsProperties>(tatquery);


                //decimal rolenho = new OracleHelper().ExecuteScalar<decimal>("select count(*) from USER_ROLE_DETAILS  t1 left outer join user_master t2 ON  t1.user_id = t2.user_id  WHERE t2.emp_code = '" + request.enterBy + "' and t1.role_id=49");
                //decimal rolecnch = new OracleHelper().ExecuteScalar<decimal>("select count(*) from USER_ROLE_DETAILS  t1 left outer join user_master t2 ON  t1.user_id = t2.user_id WHERE t2.emp_code = '" + request.enterBy + "' and t1.role_id=50");
                decimal cvrolenho = new OracleHelper().ExecuteScalar<decimal>("select count(*) from lms_tw.vw_cv_user_role_details  t1 left outer join lms_tw.vw_cv_user_master t2 ON  t1.user_id = t2.user_id  WHERE t2.emp_code = '" + request.enterBy + "' and t1.role_id=49");
                decimal cvrolench = new OracleHelper().ExecuteScalar<decimal>("select count(*) from lms_tw.vw_cv_user_role_details  t1 left outer join lms_tw.vw_cv_user_master t2 ON  t1.user_id = t2.user_id WHERE t2.emp_code = '" + request.enterBy + "' and t1.role_id=50");

                decimal cvrolebcm = new OracleHelper().ExecuteScalar<decimal>("select count(*) from lms_tw.vw_cv_user_role_details  t1 left outer join lms_tw.vw_cv_user_master t2 ON  t1.user_id = t2.user_id WHERE t2.emp_code = '" + request.enterBy + "' and t1.role_id=19");
                decimal cvrolerch = new OracleHelper().ExecuteScalar<decimal>("select count(*) from lms_tw.vw_cv_user_role_details  t1 left outer join lms_tw.vw_cv_user_master t2 ON  t1.user_id = t2.user_id WHERE t2.emp_code = '" + request.enterBy + "' and t1.role_id=55");
                decimal rho = new OracleHelper().ExecuteScalar<decimal>("select count(*) from lms_tw.vw_cv_user_role_details  t1 left outer join lms_tw.vw_cv_user_master t2 ON  t1.user_id = t2.user_id WHERE t2.emp_code = '" + request.enterBy + "' and t1.role_id=52");
                decimal bco = new OracleHelper().ExecuteScalar<decimal>("select count(*) from lms_tw.vw_cv_user_role_details  t1 left outer join lms_tw.vw_cv_user_master t2 ON  t1.user_id = t2.user_id WHERE t2.emp_code = '" + request.enterBy + "' and t1.role_id=74");

                decimal role_cvnho_ass_cnt = new OracleHelper().ExecuteScalar<decimal>("select count(*) from lms_tw.vw_cv_user_role_details  t1 left outer join lms_tw.vw_cv_user_master t2 ON  t1.user_id = t2.user_id  WHERE t2.emp_code = '" + request.enterBy + "' and t1.role_id=84");

                if (request.flag == 1)
                {
                
                    if ((cvrolenho > 0 || role_cvnho_ass_cnt >0) && cvrolench ==0)//NHO
                    {
                        query = "select count(*) from TBL_OVERSITE_DOCUMENTS_mst t where (t.NHO_SENDBACK_FLAG=2 and  t.nho_verify_by is null) and (t.application_id ='" + request.searchType + "' or t.loan_id='" + request.searchType + "')";
                        decimal cnt = new OracleHelper().ExecuteScalar<decimal>(query);

                            query = "select count(*) from TBL_OVERSITE_DOCUMENTS_mst t where t.NHO_SENDBACK_FLAG= 1 and t.verify_flag =5 and (t.application_id ='" + request.searchType + "' or t.loan_id='" + request.searchType + "')";
                            decimal count1 = new OracleHelper().ExecuteScalar<decimal>(query);
                            query = "select count(*) from TBL_OVERSite_master t where t.nch_verify = 1 and t.rch_verify = 1 and (t.application_id ='" + request.searchType + "' or t.loan_id='" + request.searchType + "')";
                            decimal count21 = new OracleHelper().ExecuteScalar<decimal>(query);

                            if (count1 > 0)
                            {
                                cmd.Add("Update tbl_oversite_master set nho_decision_flag=" + request.decisionFlag + ",nho_risk_category =" + request.categoryId + ", NHO_SENDBACK_DATE=sysdate,verification_status=5,NHO_sendback=1, tradate = sysdate  , verify_doc_count=" + count21 + ",nho_overall_remark='" + request.remarks + "' where  (application_id ='" + request.searchType + "' or loan_id='" + request.searchType + "')");
                                cmd.Add("insert into tbl_oversite_TATDetails (loanid,applicationid,STATUSID,ROLEID,TAT,DISBURSEDDATE,enteredby,entereddate,FIRMID,PRODUCTID,REGIONID,SENBACKFLAG,decision_flag,risk_category) " +
                                      "values('" + tATDetails.loan_id + "','" + tATDetails.application_id + "','5','49','" + tATDetails.TAT + "',to_date('" + tATDetails.disbursed_date + "'),'" + request.enterBy + "',sysdate,'" + tATDetails.firm_id + "','" + tATDetails.product_id + "','" + tATDetails.region_id + "',1," + request.decisionFlag + "," + request.categoryId + ")");
                            cmd.Add("Update tbl_ops_oversite_master t set t.roh_verify =null, t.roh_verify_date = null, t.roh_verify_by = null where (t.application_id = '" + request.searchType + "' or t.loan_id = '" + request.searchType + "')");
                            }
                            else
                            {
                                if (count21 > 0)
                                {
                                    cmd.Add("Update tbl_oversite_master set nho_decision_flag=" + request.decisionFlag + ",verification_status=6,NHO_verify=1, tradate = sysdate , verify_doc_count=" + count21 + ",nho_overall_remark='" + request.remarks + "', nho_verify_date = sysdate  ,nho_verify_by='" + request.enterBy + "',cr_verification_status=6 where  (application_id ='" + request.searchType + "' or loan_id='" + request.searchType + "')");
                                    cmd.Add("insert into tbl_oversite_TATDetails (loanid,applicationid,STATUSID,ROLEID,TAT,DISBURSEDDATE,enteredby,entereddate,FIRMID,PRODUCTID,REGIONID,SENBACKFLAG,decision_flag,risk_category) " +
                                             "values('" + tATDetails.loan_id + "','" + tATDetails.application_id + "','6','49','" + tATDetails.TAT + "',to_date('" + tATDetails.disbursed_date + "'),'" + request.enterBy + "',sysdate,'" + tATDetails.firm_id + "','" + tATDetails.product_id + "','" + tATDetails.region_id + "',1," + request.decisionFlag + "," + request.categoryId + ")");

                            }
                            else
                                {
                                    cmd.Add("Update tbl_oversite_master set nho_decision_flag=" + request.decisionFlag + ",verification_status=4,NHO_verify=1, tradate = sysdate , verify_doc_count=" + count21 + ",nho_overall_remark='" + request.remarks + "', nho_verify_date = sysdate  ,nho_verify_by='" + request.enterBy + "' where  (application_id ='" + request.searchType + "' or loan_id='" + request.searchType + "')");
                                    cmd.Add("insert into tbl_oversite_TATDetails (loanid,applicationid,STATUSID,ROLEID,TAT,DISBURSEDDATE,enteredby,entereddate,FIRMID,PRODUCTID,REGIONID,SENBACKFLAG,decision_flag,risk_category) " +
                                             "values('" + tATDetails.loan_id + "','" + tATDetails.application_id + "','4','49','" + tATDetails.TAT + "',to_date('" + tATDetails.disbursed_date + "'),'" + request.enterBy + "',sysdate,'" + tATDetails.firm_id + "','" + tATDetails.product_id + "','" + tATDetails.region_id + "',1," + request.decisionFlag + "," + request.categoryId + ")");

                                }

                            }
                    }
                    if (/*rolecnch > 0||*/ cvrolench > 0 && cvrolenho==0)//NCH
                    {
                        query = "select count(*) from TBL_OVERSITE_DOCUMENTS_mst t where (t.NCH_SENDBACK_FLAG=2 and  t.nch_verify_by is null) and (t.application_id ='" + request.searchType + "' or t.loan_id='" + request.searchType + "')";
                        decimal cnt = new OracleHelper().ExecuteScalar<decimal>(query);
                        //query = "select to_char(d.document_name) from TBL_OVERSITE_DOCUMENTS_mst t left outer join document_master d on t.document_type=d.document_id  and t.product_id=d.product_id where t.NCH_SENDBACK_FLAG = 1 and (t.application_id ='" + request.searchType + "' or t.loan_id='" + request.searchType + "')";
                        //object name = new OracleHelper().ExecuteScalar<object>(query);
                        if (cnt == 0)
                        {
                            query = "select count(*) from TBL_OVERSITE_DOCUMENTS_mst t where t.NCH_SENDBACK_FLAG= 1 and t.verify_flag =3 and (t.application_id ='" + request.searchType + "' or t.loan_id='" + request.searchType + "')";
                            decimal count1 = new OracleHelper().ExecuteScalar<decimal>(query);
                            query = "select count(*) from TBL_OVERSite_master  t where t.nho_verify = 1 and t.rch_verify=1 and (t.application_id ='" + request.searchType + "' or t.loan_id='" + request.searchType + "')";
                            decimal count21 = new OracleHelper().ExecuteScalar<decimal>(query);

                            if (count1 > 0)
                            {
                                //sendback
                                cmd.Add("Update tbl_oversite_master set decision_flag=" + request.decisionFlag + ",risk_category =" + request.categoryId + ",NCH_SENDBACK_DATE=sysdate,verification_status=3, NCH_sendback=1,tradate = sysdate  , verify_doc_count=" + count21 + ",nch_overall_remark='" + request.remarks + "',SENDBACK=1,CR_VERIFICATION_STATUS=3 where  (application_id ='" + request.searchType + "' or loan_id='" + request.searchType + "')");
                                cmd.Add("insert into tbl_oversite_TATDetails (loanid,applicationid,STATUSID,ROLEID,TAT,DISBURSEDDATE,enteredby,entereddate,FIRMID,PRODUCTID,REGIONID,SENBACKFLAG,decision_flag,risk_category) " +
                                        "values('" + tATDetails.loan_id + "','" + tATDetails.application_id + "','3','50','" + tATDetails.TAT + "',to_date('" + tATDetails.disbursed_date + "'),'" + request.enterBy + "',sysdate,'" + tATDetails.firm_id + "','" + tATDetails.product_id + "','" + tATDetails.region_id + "',1," + request.decisionFlag + "," + request.categoryId + ")");
                                cmd.Add("update TBL_OVERSITE_DOCUMENTS_mst t set t.NCH_SENDBACK_TO= '" + request.sendbackto + "' where (t.application_id ='" + request.searchType + "' or t.loan_id='" + request.searchType + "') and t.nch_sendback_flag=1 and t.nch_verify_by is null");

                            }
                            else
                            {
                                if (count21 > 0)
                                {
                                    //nho verfied
                                    cmd.Add("Update tbl_oversite_master set decision_flag=" + request.decisionFlag + ",risk_category=" + request.categoryId + ",verification_status=6, Nch_verify=1,tradate = sysdate , verify_doc_count=" + count21 + ",nch_overall_remark='" + request.remarks + "', nch_verify_date = sysdate  ,nch_verify_by='" + request.enterBy + "',CR_VERIFICATION_STATUS=6 where  (application_id ='" + request.searchType + "' or loan_id='" + request.searchType + "')");
                                    cmd.Add("insert into tbl_oversite_TATDetails (loanid,applicationid,STATUSID,ROLEID,TAT,DISBURSEDDATE,enteredby,entereddate,FIRMID,PRODUCTID,REGIONID,SENBACKFLAG,decision_flag,risk_category) " +
                                        "values('" + tATDetails.loan_id + "','" + tATDetails.application_id + "','6','50','" + tATDetails.TAT + "',to_date('" + tATDetails.disbursed_date + "'),'" + request.enterBy + "',sysdate,'" + tATDetails.firm_id + "','" + tATDetails.product_id + "','" + tATDetails.region_id + "',0," + request.decisionFlag + "," + request.categoryId + ")");
                                }
                                else
                                {
                                    cmd.Add("Update tbl_oversite_master set decision_flag=" + request.decisionFlag + ",risk_category=" + request.categoryId + ",verification_status=2, Nch_verify=1,tradate = sysdate , verify_doc_count=" + count21 + ",nch_overall_remark='" + request.remarks + "', nch_verify_date = sysdate  ,nch_verify_by='" + request.enterBy + "',CR_VERIFICATION_STATUS=2 where  (application_id ='" + request.searchType + "' or loan_id='" + request.searchType + "')");
                                    cmd.Add("insert into tbl_oversite_TATDetails (loanid,applicationid,STATUSID,ROLEID,TAT,DISBURSEDDATE,enteredby,entereddate,FIRMID,PRODUCTID,REGIONID,SENBACKFLAG,decision_flag,risk_category) " +
                                            "values('" + tATDetails.loan_id + "','" + tATDetails.application_id + "','2','50','" + tATDetails.TAT + "',to_date('" + tATDetails.disbursed_date + "'),'" + request.enterBy + "',sysdate,'" + tATDetails.firm_id + "','" + tATDetails.product_id + "','" + tATDetails.region_id + "',0," + request.decisionFlag + "," + request.categoryId + ")");
                                }
                                //query = "select count(*) from TBL_OVERSITE_DOCUMENTS_mst t where  (t.application_id ='" + request.searchType + "' or t.loan_id='" + request.searchType + "') and t.verify_status != 2 ";
                                //decimal count3 = new OracleHelper().ExecuteScalar<decimal>(query);
                                //if (count3 == 0)
                                //{

                                //}
                            }
                        }
                        else
                        {
                            //query = "select count(*) from TBL_OVERSITE_DOCUMENTS_mst t where t.NHO_SENDBACK_FLAG=1 and  t.nch_verify_by is null and (t.application_id ='" + request.searchType + "' or t.loan_id='" + request.searchType + "')";
                            //decimal cnt = new OracleHelper().ExecuteScalar<decimal>(query);

                            //response.responseMsg = "The Document " + $"{name}" + " is Not Recaptured Yet !";
                            response.responseMsg = "Please Verify The Recaptured Document!";
                            response.apiStatus = ApiStatusConstants.NOT_COMPLETED;
                            response.status = ResponseTypeContants.FAIL;
                            return response;
                        }
                    }
                    
                    if (bco > 0)//BCO
                    {

                        query = "select count(*) from TBL_OVERSite_master  t where t.nho_verify = 1 and (t.application_id ='" + request.searchType + "' or t.loan_id='" + request.searchType + "')";
                        decimal count21 = new OracleHelper().ExecuteScalar<decimal>(query);
                        cmd.Add("Update tbl_oversite_master set tradate = sysdate, VERIFY_DOC_COUNT = " + count21 + ", VERIFICATION_STATUS = 13 where (application_id = '" + request.searchType + "' or loan_id = '" + request.searchType + "')");
                        cmd.Add("insert into tbl_oversite_TATDetails (loanid,applicationid,STATUSID,ROLEID,TAT,DISBURSEDDATE,enteredby,entereddate,FIRMID,PRODUCTID,REGIONID,SENBACKFLAG,decision_flag,risk_category) " +
                                "values('" + tATDetails.loan_id + "','" + tATDetails.application_id + "','13','74','" + tATDetails.TAT + "',to_date('" + tATDetails.disbursed_date + "'),'" + request.enterBy + "',sysdate,'" + tATDetails.firm_id + "','" + tATDetails.product_id + "','" + tATDetails.region_id + "',0," + request.decisionFlag + "," + request.categoryId + ")");
                        cmd.Add("update tbl_ops_oversite_master t set t.ROH_SENDBACK=2,t.BCO_DECISION_FLAG =" + request.decisionFlag + ", t.BCO_RISK_CATEGORY = " + request.categoryId + ", t.bco_verify=1, t.tradate=sysdate, t.bco_overall_remark='" + request.remarks + "', t.bco_verify_date = sysdate, t.bco_verify_by = '" + request.enterBy + "', t.status_id=13 where (t.application_id = '" + request.searchType + "' or t.loan_id = '" + request.searchType + "')");

                    }
                    if (cvrolebcm > 0)//BCM
                    {

                        query = "select count(*) from TBL_OVERSite_master  t where t.nho_verify = 1 and (t.application_id ='" + request.searchType + "' or t.loan_id='" + request.searchType + "')";
                        decimal count21 = new OracleHelper().ExecuteScalar<decimal>(query);
                        cmd.Add("Update tbl_oversite_master set bcm_decision_flag=" + request.decisionFlag + ",bcm_risk_category=" + request.categoryId + ", BCM_VERIFY=1,tradate = sysdate ,VERIFY_DOC_COUNT=" + count21 + ",bcm_overall_remark='" + request.remarks + "', BCM_VERIFY_DATE = sysdate  ,BCM_VERIFY_BY='" + request.enterBy + "',CR_VERIFICATION_STATUS=9 where  (application_id ='" + request.searchType + "' or loan_id='" + request.searchType + "')");
                        cmd.Add("insert into tbl_oversite_TATDetails (loanid,applicationid,STATUSID,ROLEID,TAT,DISBURSEDDATE,enteredby,entereddate,FIRMID,PRODUCTID,REGIONID,SENBACKFLAG,decision_flag,risk_category) " +
                                "values('" + tATDetails.loan_id + "','" + tATDetails.application_id + "','9','19','" + tATDetails.TAT + "',to_date('" + tATDetails.disbursed_date + "'),'" + request.enterBy + "',sysdate,'" + tATDetails.firm_id + "','" + tATDetails.product_id + "','" + tATDetails.region_id + "',0," + request.decisionFlag + "," + request.categoryId + ")");
                    }

                    if (/*rolecnch > 0||*/ cvrolerch > 0 && rho ==0)//RCH
                    {
                        query = "select count(*) from TBL_OVERSITE_DOCUMENTS_mst t where (t.RCH_SENDBACK_FLAG=2 and  t.RCH_VERIFY_BY is null) and (t.application_id ='" + request.searchType + "' or t.loan_id='" + request.searchType + "')";
                        decimal cnt = new OracleHelper().ExecuteScalar<decimal>(query);
                        if (cnt == 0)
                        {
                            query = "select count(*) from TBL_OVERSITE_DOCUMENTS_mst t where t.RCH_SENDBACK_FLAG= 1 and t.verify_flag =10 and (t.application_id ='" + request.searchType + "' or t.loan_id='" + request.searchType + "')";
                            decimal count1 = new OracleHelper().ExecuteScalar<decimal>(query);
                            query = "select count(*) from TBL_OVERSite_master  t where t.nho_verify = 1 and t.nch_verify=1 and (t.application_id ='" + request.searchType + "' or t.loan_id='" + request.searchType + "')";
                            decimal count21 = new OracleHelper().ExecuteScalar<decimal>(query);

                            if (count1 > 0)
                            {
                                cmd.Add("Update tbl_oversite_master set rch_decision_flag=" + request.decisionFlag + ",rch_risk_category =" + request.categoryId + ",RCH_SENDBACK_DATE=sysdate, RCH_sendback=1,tradate = sysdate  , verify_doc_count=" + count21 + ",Rch_overall_remark='" + request.remarks + "',CR_VERIFICATION_STATUS=10 where  (application_id ='" + request.searchType + "' or loan_id='" + request.searchType + "')");
                                cmd.Add("insert into tbl_oversite_TATDetails (loanid,applicationid,STATUSID,ROLEID,TAT,DISBURSEDDATE,enteredby,entereddate,FIRMID,PRODUCTID,REGIONID,SENBACKFLAG,decision_flag,risk_category) " +
                                        "values('" + tATDetails.loan_id + "','" + tATDetails.application_id + "','10','55','" + tATDetails.TAT + "',to_date('" + tATDetails.disbursed_date + "'),'" + request.enterBy + "',sysdate,'" + tATDetails.firm_id + "','" + tATDetails.product_id + "','" + tATDetails.region_id + "',1," + request.decisionFlag + "," + request.categoryId + ")");
                            }
                            else
                            {
                                cmd.Add("Update tbl_oversite_master set rch_decision_flag=" + request.decisionFlag + ",rch_risk_category=" + request.categoryId + ",Rch_verify=1,tradate = sysdate , verify_doc_count=" + count21 + ",Rch_overall_remark='" + request.remarks + "', Rch_verify_date = sysdate  ,Rch_verify_by='" + request.enterBy + "',CR_VERIFICATION_STATUS=8 where  (application_id ='" + request.searchType + "' or loan_id='" + request.searchType + "')");
                                cmd.Add("insert into tbl_oversite_TATDetails (loanid,applicationid,STATUSID,ROLEID,TAT,DISBURSEDDATE,enteredby,entereddate,FIRMID,PRODUCTID,REGIONID,SENBACKFLAG,decision_flag,risk_category) " +
                                        "values('" + tATDetails.loan_id + "','" + tATDetails.application_id + "','8','55','" + tATDetails.TAT + "',to_date('" + tATDetails.disbursed_date + "'),'" + request.enterBy + "',sysdate,'" + tATDetails.firm_id + "','" + tATDetails.product_id + "','" + tATDetails.region_id + "',0," + request.decisionFlag + "," + request.categoryId + ")");
                            }
                        }
                        else
                        {
                            response.responseMsg = "Please Verify The Recaptured Document!";
                            response.apiStatus = ApiStatusConstants.NOT_COMPLETED;
                            response.status = ResponseTypeContants.FAIL;
                            return response;
                        }
                    }
                    if (rho > 0 && cvrolerch ==0)//ROH
                    {
                        query = "select count(*) from lms_tw.TBL_OVERSITE_DOCUMENTS_mst t where (t.rho_sendback_flag = 2 and t.rho_verify_by is null) and (t.application_id = '" + request.searchType + "' or t.loan_id = '" + request.searchType + "')";
                        decimal cnt = new OracleHelper().ExecuteScalar<decimal>(query);
                        if (cnt == 0)
                        {
                            query = "select count(*) from lms_tw.TBL_OVERSITE_DOCUMENTS_mst t where t.rho_sendback_flag= 1 and t.verify_flag =12 and (t.application_id ='" + request.searchType + "' or t.loan_id='" + request.searchType + "')";
                            decimal count1 = new OracleHelper().ExecuteScalar<decimal>(query);
                            query = "select count(*) from lms_tw.TBL_OVERSite_master  t where t.nho_verify = 1 and t.nch_verify=1 and (t.application_id ='" + request.searchType + "' or t.loan_id='" + request.searchType + "')";
                            decimal count21 = new OracleHelper().ExecuteScalar<decimal>(query);
                            if (count1 > 0)
                            {
                                query = "select count(*) from lms_tw.tbl_ops_oversite_master t where (t.application_id = '" + request.searchType + "' or t.loan_id = '" + request.searchType + "')";
                                decimal cntt = new OracleHelper().ExecuteScalar<decimal>(query);
                                cmd.Add("Update lms_tw.tbl_oversite_master t set tradate = sysdate, verify_doc_count = " + count21 + ", VERIFICATION_STATUS = 12 where (application_id = '" + request.searchType + "' or loan_id = '" + request.searchType + "')");
                                cmd.Add("insert into lms_tw.tbl_oversite_TATDetails (loanid,applicationid,STATUSID,ROLEID,TAT,DISBURSEDDATE,enteredby,entereddate,FIRMID,PRODUCTID,REGIONID,SENBACKFLAG,decision_flag,risk_category) " +
                                        "values('" + tATDetails.loan_id + "','" + tATDetails.application_id + "','12','52','" + tATDetails.TAT + "',to_date('" + tATDetails.disbursed_date + "'),'" + request.enterBy + "',sysdate,'" + tATDetails.firm_id + "','" + tATDetails.product_id + "','" + tATDetails.region_id + "',1," + request.decisionFlag + "," + request.categoryId + ")");
                                if(cntt>0)
                                {
                                    //query = "select count(*) from tbl_ops_oversite_master t where t.ROH_SENDBACK is not null and  (t.application_id = '" + request.searchType + "' or t.loan_id = '" + request.searchType + "')";
                                    //decimal cn = new OracleHelper().ExecuteScalar<decimal>(query);
                                    cmd.Add("Update lms_tw.tbl_ops_oversite_master t set t.roh_verify = 1, t.roh_decision_flag = " + request.decisionFlag + ", t.roh_risk_category = " + request.categoryId + ", t.roh_sendback_date = sysdate,t.roh_sendback = 1, t.tradate = sysdate, t.status_id = 12 where(application_id = '" + request.searchType + "' or loan_id = '" + request.searchType + "')");
                                }
                                else
                                {
                                    cmd.Add("insert into lms_tw.tbl_ops_oversite_master t (application_id, roh_verify ,loan_id, status_id, roh_decision_flag, roh_risk_category, ROH_SENDBACK, roh_overall_remark, ROH_SENDBACK_DATE, ROH_SENDBACK_BY, tradate) values ( '" + tATDetails.application_id + "', 1,'" + tATDetails.loan_id + "', 12, " + request.decisionFlag + ", " + request.categoryId + ", 1, '" + request.remarks + "', sysdate, '" + request.enterBy + "', sysdate )");

                                    //cmd.Add("insert into tbl_ops_oversite_master (application_id, loan_id, status_id, roh_decision_flag, roh_risk_category, roh_verify, roh_overall_remark, roh_verify_date, roh_verify_by, tradate) values ( '" + tATDetails.application_id + "', '" + tATDetails.loan_id + "', 12, " + request.decisionFlag + ", " + request.categoryId + ") 1, '" + request.remarks + "', sysdate, '" + request.enterBy + "', sysdate )");
                                }
                            }
                            else 
                            {
                                cmd.Add("Update lms_tw.tbl_oversite_master t set tradate = sysdate, verify_doc_count = " + count21 + ", VERIFICATION_STATUS = 11 where (application_id = '" + request.searchType + "' or loan_id = '" + request.searchType + "')");
                                cmd.Add("insert into lms_tw.tbl_oversite_TATDetails (loanid,applicationid,STATUSID,ROLEID,TAT,DISBURSEDDATE,enteredby,entereddate,FIRMID,PRODUCTID,REGIONID,SENBACKFLAG,decision_flag,risk_category) " +
                                       "values('" + tATDetails.loan_id + "','" + tATDetails.application_id + "','11','52','" + tATDetails.TAT + "',to_date('" + tATDetails.disbursed_date + "'),'" + request.enterBy + "',sysdate,'" + tATDetails.firm_id + "','" + tATDetails.product_id + "','" + tATDetails.region_id + "',1," + request.decisionFlag + "," + request.categoryId + ")");
                                query = "select count(*) from lms_tw.tbl_ops_oversite_master t where (t.application_id = '" + request.searchType + "' or t.loan_id = '" + request.searchType + "')";
                                decimal cntt = new OracleHelper().ExecuteScalar<decimal>(query);
                                if (cntt > 0)
                                {
                                    cmd.Add("Update lms_tw.tbl_ops_oversite_master t set t.roh_verify = 1,t.roh_decision_flag = " + request.decisionFlag + ", t.roh_risk_category = " + request.categoryId + ", t.roh_sendback_date = sysdate, t.tradate = sysdate, t.status_id = 11 where(application_id = '" + request.searchType + "' or loan_id = '" + request.searchType + "')");
                                }
                                else
                                {

                                    cmd.Add("insert into lms_tw.tbl_ops_oversite_master (application_id,roh_verify, loan_id, status_id, roh_decision_flag, roh_risk_category,  roh_overall_remark, roh_verify_date, roh_verify_by, tradate) values ( '" + tATDetails.application_id + "',1, '" + tATDetails.loan_id + "', 11, " + request.decisionFlag + ", " + request.categoryId + ",  '" + request.remarks + "', sysdate, '" + request.enterBy + "', sysdate )");
                                }
                            }
                        }
                        else
                        {
                            response.responseMsg = "Please Verify The Recaptured Document!";
                            response.apiStatus = ApiStatusConstants.NOT_COMPLETED;
                            response.status = ResponseTypeContants.FAIL;
                            return response;
                        }
                    }
                    new OracleHelper().ExecuteNonQuerynew(cmd.ToArray(), OracleHelper.SQLMode.Query, null);
                    response.Data = null;
                    response.responseMsg = "Verified Successfully";
                    response.apiStatus = ResponseTypeContants.SUCCESS;
                    response.status = ApiStatusConstants.COMPLETED;

                }

                //recapture 
                if (request.flag == 2)
                {
                    
                        string query3 = "select count(*) from lms_tw.TBL_OVERSITE_MASTER t where (t.application_id = '" + request.searchType + "' or t.loan_id = '" + request.searchType + "') and nvl(t.lms,0)=0 and nvl(t.verification_status,0)=7";
                        decimal count212 = new OracleHelper().ExecuteScalar<decimal>(query3);
                        if (count212 > 0)
                        {


                            string query11 = "select c.checklist_id from  lms_tw.TBL_CHECKLIST_MASTER c where c.scheme_id=" + request.schemeId + "";
                            object checklist = new OracleHelper().ExecuteScalar<object>(query11);
                            decimal app = new OracleHelper().ExecuteScalar<decimal>("select count(*) from lms_tw.TBL_CHECKLIST_MST t where t.checklist_id=" + checklist + " and t.mandate_flag=1");
                            decimal cnt = new OracleHelper().ExecuteScalar<decimal>("select count(*) from lms_tw.TBL_OVERSITE_DOCUMENTS_MST t where (t.application_id = '" + request.searchType + "' or t.loan_id = '" + request.searchType + "')");
                            if (app != cnt)
                            {
                                response.apiStatus = ResponseTypeContants.FAIL;
                                response.status = ApiStatusConstants.COMPLETED;
                                response.responseMsg = "Please Upload All Scheme Based Documents!";
                                return response;
                            }
                            else
                            {
                                cmd.Add("Update lms_tw.tbl_oversite_master set verification_status=1,tradate=sysdate where (application_id = '" + request.searchType + "' or loan_id = '" + request.searchType + "')");

                            }
                        }
                    
                    query = "select count(*) from lms_tw.TBL_OVERSITE_MASTER t where (t.application_id = '" + request.searchType + "' or t.loan_id = '" + request.searchType + "') and (t.NHO_SENDBACK=2 and t.NCH_SENDBACK=2 and t.RCH_SENDBACK=2)";
                    decimal cnt1 = new OracleHelper().ExecuteScalar<decimal>(query);
                    query = "select count(*) from lms_tw.tbl_ops_oversite_master t where (t.application_id = '" + request.searchType + "' or t.loan_id = '" + request.searchType + "') and t.ROH_SENDBACK=2";
                    decimal opscnt1 = new OracleHelper().ExecuteScalar<decimal>(query);
                    if (cnt1 > 0 || opscnt1 >0)
                    {
                        response.apiStatus = ResponseTypeContants.FAIL;
                        response.status = ApiStatusConstants.COMPLETED;
                        response.responseMsg = "Already Updated All Documents";
                        return response;
                    }
                    else
                    {
                        if (rho > 0)//ROH
                        {
                            query = "select count(*) from lms_tw.TBL_OVERSITE_MASTER t where (t.application_id = '" + request.searchType + "' or t.loan_id = '" + request.searchType + "') and t.NHO_SENDBACK=1";
                            //query1 = "select count(*) from TBL_OVERSITE_MASTER t where (t.application_id = '" + request.searchType + "' or t.loan_id = '" + request.searchType + "') and nvl(t.lms,0)=0 and t.verification_status=6";
                            decimal count111 = new OracleHelper().ExecuteScalar<decimal>(query);
                            if (count111 > 0)
                            {
                                decimal count214 = new OracleHelper().ExecuteScalar<decimal>("select count(*) from lms_tw.TBL_OVERSITE_DOCUMENTS_MST t where (t.application_id = '" + request.searchType + "' or t.loan_id = '" + request.searchType + "') and t.nho_sendback_flag=1 and t.recaptured_by is null");
                                if (count214 > 0)
                                {
                                    response.apiStatus = ResponseTypeContants.FAIL;
                                    response.status = ApiStatusConstants.COMPLETED;
                                    response.responseMsg = "Please Upload All Documents!";
                                    return response;
                                }
                                cmd.Add("Update lms_tw.tbl_oversite_master set tradate=sysdate,nho_sendback=2 where (application_id = '" + request.searchType + "' or loan_id = '" + request.searchType + "') and NHO_SENDBACK=1");


                                for (int i = 0; i < request.confirmlist.Count; i++)
                                {
                                    decimal status = new OracleHelper().ExecuteScalar<decimal>("select t.verify_flag from lms_tw.TBL_OVERSITE_DOCUMENTS_MST t where t.application_id = '" + request.confirmlist[i].applicationId + "' and document_type=" + request.confirmlist[i].docType + "");

                                    cmd.Add("Update lms_tw.TBL_OVERSITE_DOCUMENTS_MST set NHO_SENDBACK_FLAG=2,recaptured_dt = sysdate,recaptured_by='" + request.enterBy + "',recaptured_remarks='" + request.rmRemarks + "' where application_id = '" + request.confirmlist[i].applicationId + "' and document_type=" + request.confirmlist[i].docType + " and customerid='" + request.confirmlist[i].customerid + "' and NHO_SENDBACK_FLAG=1");
                                    decimal seq_ID = new OracleHelper().ExecuteScalar<decimal>("select nvl(max(sequence_id ),0)+1  from lms_tw.TBL_OVERSITE_DOCUMENTS_DTL where  application_id = '" + request.confirmlist[i].applicationId + "' ");
                                    cmd.Add("insert into lms_tw.TBL_OVERSITE_DOCUMENTS_DTL(application_id,loan_id,document_no,document_type,verification_status,verify_by,verify_date,verify_remarks,sendback_flag,sequence_id,customerid) values('" + request.confirmlist[i].applicationId + "','" + request.confirmlist[i].loanId + "','" + request.confirmlist[i].docNo + "','" + request.confirmlist[i].docType + "'," + status + ",'" + request.enterBy + "',sysdate,'" + request.rmRemarks + "',2," + seq_ID + ",'" + request.confirmlist[i].customerid + "')");

                                }
                            }
                        }
                        if (cvrolerch > 0)//RCH
                        {
                            query = "select count(*) from lms_tw.TBL_OVERSITE_MASTER t where (t.application_id = '" + request.searchType + "' or t.loan_id = '" + request.searchType + "') and t.NCH_SENDBACK=1";
                            decimal count2 = new OracleHelper().ExecuteScalar<decimal>(query);
                            if (count2 > 0)
                            {
                                decimal count213 = new OracleHelper().ExecuteScalar<decimal>("select count(*) from lms_tw.TBL_OVERSITE_DOCUMENTS_MST t where (t.application_id = '" + request.searchType + "' or t.loan_id = '" + request.searchType + "') and t.nch_sendback_flag=1 and t.recaptured_by is null");
                                if (count213 > 0)
                                {
                                    response.apiStatus = ResponseTypeContants.FAIL;
                                    response.status = ApiStatusConstants.COMPLETED;
                                    response.responseMsg = "Please Upload All Documents!";
                                    return response;
                                }
                                cmd.Add("Update lms_tw.tbl_oversite_master set tradate=sysdate,nch_sendback=2 where (application_id = '" + request.searchType + "' or loan_id = '" + request.searchType + "') and NCH_SENDBACK=1");

                                for (int i = 0; i < request.confirmlist.Count; i++)
                                {
                                    decimal status = new OracleHelper().ExecuteScalar<decimal>("select t.verify_flag from lms_tw.TBL_OVERSITE_DOCUMENTS_MST t where t.application_id = '" + request.confirmlist[i].applicationId + "' and document_type=" + request.confirmlist[i].docType + "");

                                    cmd.Add("Update lms_tw.TBL_OVERSITE_DOCUMENTS_MST set NCH_SENDBACK_FLAG=2,recaptured_dt = sysdate,recaptured_by='" + request.enterBy + "',recaptured_remarks='" + request.rmRemarks + "' where application_id = '" + request.confirmlist[i].applicationId + "' and document_type=" + request.confirmlist[i].docType + " and customerid='" + request.confirmlist[i].customerid + "' and NCH_SENDBACK_FLAG=1");
                                    decimal seq_ID = new OracleHelper().ExecuteScalar<decimal>("select nvl(max(sequence_id ),0)+1  from lms_tw.TBL_OVERSITE_DOCUMENTS_DTL where  application_id = '" + request.confirmlist[i].applicationId + "' ");
                                    cmd.Add("insert into lms_tw.TBL_OVERSITE_DOCUMENTS_DTL(application_id,loan_id,document_no,document_type,verification_status,verify_by,verify_date,verify_remarks,sendback_flag,sequence_id,customerid) values('" + request.confirmlist[i].applicationId + "','" + request.confirmlist[i].loanId + "','" + request.confirmlist[i].docNo + "','" + request.confirmlist[i].docType + "'," + status + ",'" + request.enterBy + "',sysdate,'" + request.rmRemarks + "',2," + seq_ID + ",'" + request.confirmlist[i].customerid + "')");

                                }
                            }
                        }


                        if (cvrolebcm > 0 && bco==0)//BCH
                        {
                            query = "select count(*) from lms_tw.TBL_OVERSITE_MASTER t where (t.application_id = '" + request.searchType + "' or t.loan_id = '" + request.searchType + "') and t.RCH_SENDBACK=1";
                            decimal count3 = new OracleHelper().ExecuteScalar<decimal>(query);
                            if (count3 > 0)
                            {
                                decimal count215 = new OracleHelper().ExecuteScalar<decimal>("select count(*) from lms_tw.TBL_OVERSITE_DOCUMENTS_MST t where (t.application_id = '" + request.searchType + "' or t.loan_id = '" + request.searchType + "') and t.RCH_sendback_flag=1 and t.recaptured_by is null");
                                if (count215 > 0)
                                {
                                    response.apiStatus = ResponseTypeContants.FAIL;
                                    response.status = ApiStatusConstants.COMPLETED;
                                    response.responseMsg = "Please Upload All Documents!";
                                    return response;
                                }
                                cmd.Add("Update lms_tw.tbl_oversite_master set tradate=sysdate,Rch_sendback=2 where (application_id = '" + request.searchType + "' or loan_id = '" + request.searchType + "') and RCH_SENDBACK=1");

                                for (int i = 0; i < request.confirmlist.Count; i++)
                                {
                                    decimal status = new OracleHelper().ExecuteScalar<decimal>("select t.verify_flag from lms_tw.TBL_OVERSITE_DOCUMENTS_MST t where t.application_id = '" + request.confirmlist[i].applicationId + "' and document_type=" + request.confirmlist[i].docType + "");

                                    cmd.Add("Update lms_tw.TBL_OVERSITE_DOCUMENTS_MST set RCH_SENDBACK_FLAG=2,recaptured_dt = sysdate,recaptured_by='" + request.enterBy + "',recaptured_remarks='" + request.rmRemarks + "' where application_id = '" + request.confirmlist[i].applicationId + "' and document_type=" + request.confirmlist[i].docType + " and customerid='" + request.confirmlist[i].customerid + "' and RCH_SENDBACK_FLAG=1");
                                    decimal seq_ID = new OracleHelper().ExecuteScalar<decimal>("select nvl(max(sequence_id ),0)+1  from lms_tw.TBL_OVERSITE_DOCUMENTS_DTL where  application_id = '" + request.confirmlist[i].applicationId + "' ");
                                    cmd.Add("insert into lms_tw.TBL_OVERSITE_DOCUMENTS_DTL(application_id,loan_id,document_no,document_type,verification_status,verify_by,verify_date,verify_remarks,sendback_flag,sequence_id,customerid) values('" + request.confirmlist[i].applicationId + "','" + request.confirmlist[i].loanId + "','" + request.confirmlist[i].docNo + "','" + request.confirmlist[i].docType + "'," + status + ",'" + request.enterBy + "',sysdate,'" + request.rmRemarks + "',2," + seq_ID + ",'" + request.confirmlist[i].customerid + "')");

                                }
                            }
                        }
                        if (bco > 0 && cvrolebcm==0)//BOH
                        {
                            query = "select count(*) from lms_tw.tbl_ops_oversite_master t where (t.application_id = '" + request.searchType + "' or t.loan_id = '" + request.searchType + "') and t.ROH_SENDBACK=1";
                            decimal count4 = new OracleHelper().ExecuteScalar<decimal>(query);
                            query1 = "select count(*) from lms_tw.tbl_oversite_master t where (t.application_id = '" + request.searchType + "' or t.loan_id = '" + request.searchType + "') and t.nho_sendback = 1";
                            decimal count44 = new OracleHelper().ExecuteScalar<decimal>(query1);

                            if (count4 > 0 || count44 > 0)
                            {
                                //100743
                                decimal count216 = new OracleHelper().ExecuteScalar<decimal>("select count(*) from lms_tw.TBL_OVERSITE_DOCUMENTS_MST t where (t.application_id = '" + request.searchType + "' or t.loan_id = '" + request.searchType + "') and t.RHO_sendback_flag=1 and t.recaptured_by is null");
                                decimal count217= new OracleHelper().ExecuteScalar<decimal>("select count(*) from lms_tw.TBL_OVERSITE_DOCUMENTS_MST t where (t.application_id = '" + request.searchType + "' or t.loan_id = '" + request.searchType + "') and t.nho_sendback_flag = 1 and t.recaptured_by is null");

                                if (count216 > 0 || count217 >0)
                                {
                                    response.apiStatus = ResponseTypeContants.FAIL;
                                    response.status = ApiStatusConstants.COMPLETED;
                                    response.responseMsg = "Please Upload All Documents!";
                                    return response;
                                }
                                cmd.Add("Update lms_tw.tbl_oversite_master set tradate=sysdate,VERIFICATION_STATUS=13,nho_sendback=2 where (application_id = '" + request.searchType + "' or loan_id = '" + request.searchType + "')and nho_sendback=1");
                                cmd.Add("update tbl_ops_oversite_master t set t.ROH_SENDBACK=2, t.BCO_DECISION_FLAG =" + request.decisionFlag + ", t.BCO_RISK_CATEGORY = " + request.categoryId + ", t.bco_verify=1, t.tradate=sysdate ,t.bco_overall_remark='" + request.remarks + "', t.bco_verify_date = sysdate, t.bco_verify_by = '" + request.enterBy + "', t.status_id=13 where (t.application_id = '" + request.searchType + "' or t.loan_id = '" + request.searchType + "')and t.roh_sendback=1");
                                for (int i = 0; i < request.confirmlist.Count; i++)
                                {
                                    decimal status = new OracleHelper().ExecuteScalar<decimal>("select t.verify_flag from lms_tw.TBL_OVERSITE_DOCUMENTS_MST t where t.application_id = '" + request.confirmlist[i].applicationId + "' and document_type=" + request.confirmlist[i].docType + "");

                                    cmd.Add("Update lms_tw.TBL_OVERSITE_DOCUMENTS_MST set RHO_SENDBACK_FLAG=2,recaptured_dt = sysdate,recaptured_by='" + request.enterBy + "',recaptured_remarks='" + request.rmRemarks + "' where application_id = '" + request.confirmlist[i].applicationId + "' and document_type=" + request.confirmlist[i].docType + " and customerid='" + request.confirmlist[i].customerid + "' and RHO_SENDBACK_FLAG=1");
                                    cmd.Add("Update lms_tw.TBL_OVERSITE_DOCUMENTS_MST set nho_sendback_flag=2,recaptured_dt = sysdate,recaptured_by='" + request.enterBy + "',recaptured_remarks='" + request.rmRemarks + "' where application_id = '" + request.confirmlist[i].applicationId + "' and document_type=" + request.confirmlist[i].docType + " and customerid='" + request.confirmlist[i].customerid + "' and nho_sendback_flag=1");

                                    decimal seq_ID = new OracleHelper().ExecuteScalar<decimal>("select nvl(max(sequence_id ),0)+1  from lms_tw.TBL_OVERSITE_DOCUMENTS_DTL where  application_id = '" + request.confirmlist[i].applicationId + "' ");
                                    cmd.Add("insert into lms_tw.TBL_OVERSITE_DOCUMENTS_DTL(application_id,loan_id,document_no,document_type,verification_status,verify_by,verify_date,verify_remarks,sendback_flag,sequence_id,customerid) values('" + request.confirmlist[i].applicationId + "','" + request.confirmlist[i].loanId + "','" + request.confirmlist[i].docNo + "','" + request.confirmlist[i].docType + "'," + status + ",'" + request.enterBy + "',sysdate,'" + request.rmRemarks + "',2," + seq_ID + ",'" + request.confirmlist[i].customerid + "')");

                                }
                            }

                        }
                        cmd.Add("insert into tbl_oversite_TATDetails (loanid,applicationid,ROLEID,TAT,DISBURSEDDATE,enteredby,entereddate,FIRMID,PRODUCTID,REGIONID,SENBACKFLAG,decision_flag,risk_category) " +
                                "values('" + tATDetails.loan_id + "','" + tATDetails.application_id + "','"+request.enterBy+"','" + tATDetails.TAT + "',to_date('" + tATDetails.disbursed_date + "'),'" + request.enterBy + "',sysdate,'" + tATDetails.firm_id + "','" + tATDetails.product_id + "','" + tATDetails.region_id + "',2," + request.decisionFlag + "," + request.categoryId + ")");

                        int result= new OracleHelper().ExecuteNonQuerynew(cmd.ToArray(), OracleHelper.SQLMode.Query, null);

                        if(result <0)
                        {
                            response.apiStatus = ResponseTypeContants.FAIL;
                            response.status = ApiStatusConstants.COMPLETED;
                            response.responseMsg = "Something went wrong !!!";
                            return response;
                        }


                        response.Data = null;
                        response.apiStatus = ResponseTypeContants.SUCCESS;
                        response.status = ApiStatusConstants.COMPLETED;
                        response.responseMsg = "Success";
                        return response;

                    }

                }
            }

            catch (Exception ex)
            {
                ErrorLogRequest errorLogRequest = new Oversite.DTO.ErrorLog.Request.ErrorLogRequest();
                errorLogRequest.Function_Name = "Documents Get - GetDocument";
                errorLogRequest.V_Data = output;
                errorLogRequest.V_Exception = ex.Message;
                new ErrorLog.ErrorLogDataSource().ErrorLog(errorLogRequest);
                response.responseMsg = "Failed!Please Contact The Support Team.";
                response.Data = null;
                response.apiStatus = ResponseTypeContants.FAIL;
                response.status = ApiStatusConstants.NOT_COMPLETED;


            }
            return response;
        }

        public Response<RepaymentScheduleResponse> Repay(AccRepaymentScheduleRequest request)
        {


            RepaymentScheduleResponse repaymentScheduleResponse = new RepaymentScheduleResponse();
            Response<RepaymentScheduleResponse> response = new Response<RepaymentScheduleResponse>();
            string query = "";
            //LoansManager loansManager = new LoansManager();
            try
            {
                // string intStartDate = objDbConnectionHandler.GetRecord<string>("select INT_START_DATE intStartDate from loan_parameters where application_id=" + repaymentScheduleRequest.applicationId + "", DbConnectionHandler.SQLMode.Query, null);
                query = "select t.installment_no,t.due_date,t.installment_amount,t.principal_amount,t.interest_amount,t.closing_balance,t.opening_balance,to_char(t.effective_interest_rate) as EFFECTIVE_INTEREST_RATE ,to_char(t.effective_rate_days) as effectivedays from LOAN_INSTALLMENT_DTL t  where  t.LOAN_ID ='" + request.Loan_Id + "' order by t.installment_no asc";
                //commonMasterReponse.commonDataList = new OracleHelper().GetRecords<CommonMasterProperties>(query);
                //query = "select t.installment_no,t.due_date,t.installment_amount,t.principal_amount,t.interest_amount,t.closing_balance,t.opening_balance,t.effective_interest_rate /*,to_char(t.effective_rate_days) as effectivedays*/  from LOAN_INSTALLMENT_DTL t  where  t.LOAN_ID ='" + request.Loan_Id + "'";

                List<RepaymentScheduleProperties> repaymentschList = new OracleHelper().GetRecords<RepaymentScheduleProperties>(query);
                query = "select sum(t.installment_amount) as total_inst_amt,sum(t.principal_amount) as total_pricipal_amt,sum(t.interest_amount) total_intr_amt from LOAN_INSTALLMENT_DTL t where  t.LOAN_ID ='" + request.Loan_Id + "'";
                List<RepaymentSumProperties> repaymentschTotalList = new OracleHelper().GetRecords<RepaymentSumProperties>(query);


                if (repaymentschList != null)
                {
                    repaymentScheduleResponse.repaymentschList = repaymentschList;
                    repaymentScheduleResponse.repaymentTotalList = repaymentschTotalList;
                    response.Data = repaymentScheduleResponse;
                    response.status = ResponseTypeContants.SUCCESS;
                    response.apiStatus = ApiStatusConstants.COMPLETED;
                    response.responseMsg = ResponseTypeContants.SUCCESS;
                }
                else
                {
                    response.responseMsg = "Please check the loan id";
                    response.apiStatus = ApiStatusConstants.NOT_COMPLETED;
                    response.status = ResponseTypeContants.FAIL;

                }
            }
            catch (Exception ex)
            {
                //response.Status.flag = ProcessStatus.failed;
                //response.Status.code = APIStatus.exception;
                response.responseMsg = ex.Message;
            }
            return response;

        }
        public Response<LoanDetailsResponse> FetchData(LoanDetailsRequest request)
        {
            LoanDetailsResponse loanDetailsResponse = new LoanDetailsResponse();
            Response<LoanDetailsResponse> response = new Response<LoanDetailsResponse>();
            //LoansManager loansManager = new LoansManager();
            try
            {
                string query = "";
                query = "select count(*) from vw_cv_loan_master t where t.loan_id = '" + request.Loan_Id + "'";
                decimal count2 = new OracleHelper().ExecuteScalar<decimal>(query);
                query = "select count(*) from vw_cv_loan_master t where t.application_id = '" + request.Loan_Id + "'";
                decimal count21 = new OracleHelper().ExecuteScalar<decimal>(query);

                if (count2 > 0)
                {
                    query = "select a.cust_id,a.first_name ||' '||a.middle_name||' '||a.last_name name,b.house_name,b.address_line_2,b.address_line_3,to_char(b.pincode) as pincode ,t.loan_date,to_char(t.loan_amount) as loan_amount,to_char(t.interest_rate) as interest_rate,to_char(t.tenure) as tenure,t.application_id as applicationId,t.loan_id as loanId,s.state_name, d.district_name,c.country_name,nvl(a.mobile_phone,a.primary_phone)mobile_phone,a.primary_phone,a.primary_email_id,to_char(t.accounting_irr) as accIrr,to_char(t.customer_irr) as custIrr,br.branch_name as branch,sm.scheme_name as scheme from vw_cv_loan_master t,vw_cv_customer a,vw_cv_customer_address b,vw_cv_state_master s,vw_cv_district_master  d,vw_cv_country_master   c,vw_cv_branch_master br,vw_cv_scheme_master sm where b.cust_id=a.cust_id and  a.cust_id=t.customer_id and b.is_primary_address = 'Y' and b.state_id=s.state_id  and b.district_id=d.district_id   and b.country=c.country_id and t.branch_id=br.branch_id and sm.scheme_id=t.scheme_id and t.loan_id='" + request.Loan_Id + "'";
                    List<CustomerDetailsProperties> customerlist = new OracleHelper().GetRecords<CustomerDetailsProperties>(query);
                    query = "select t.APPLICATION_ID, (select c.description from common_master c where t.int_rate_type = c.common_data_name and c.common_datatype_id=18) as INT_RATE_TYPE, (select c.description from common_master c where t.repayment_frequency = c.common_data_name and c.common_datatype_id=19)as repayment_frequency, (select c.description from common_master c where t.installment_type = c.common_data_name and c.common_datatype_id = 74) as installment_type, to_char(t.adv_inst), p.product_name, case l.loan_status when 1 then 'Active' when 0 then 'Inactive' end as loan_status, (select to_char(count(RECEIPT_DATE)) from LOAN_COLLECTION_MASTER where loan_id='" + request.Loan_Id + "' and RECEIPT_DATE<(select due_date from LOAN_INSTALLMENT_DTL where loan_id='" + request.Loan_Id + "' and installment_no=1)) as no_of_adv_installments, (select to_char(sum(COLLECTION_AMOUNT)) from LOAN_COLLECTION_MASTER where loan_id='" + request.Loan_Id + "' and RECEIPT_DATE<(select due_date from LOAN_INSTALLMENT_DTL where loan_id='" + request.Loan_Id + "' and installment_no=1)) as adv_amount from vw_cv_loan_parameters t, vw_cv_loan_master l, product_master p where t.application_id = l.application_id and l.product_id=p.product_id and l.loan_id = '" + request.Loan_Id + "'";
                    List<LoanotherDataPropeties> loandatalist = new OracleHelper().GetRecords<LoanotherDataPropeties>(query);
                    query = "select count(*) from TBL_OVERSITE_MASTER t where nvl(t.repay_verify_status,0)=0 and t.loan_id='" + request.Loan_Id + "'";
                    decimal cnt2 = new OracleHelper().ExecuteScalar<decimal>(query);
                    if (cnt2 ==0)
                    {
                        query = "select to_char(t.repay_verify_status) as decisionflag ,t.repay_remarks as remarks from TBL_OVERSITE_MASTER t where t.loan_id='" + request.Loan_Id + "'";
                        List<Repaydecisionproperties> Repaydatalist = new OracleHelper().GetRecords<Repaydecisionproperties>(query);
                        loanDetailsResponse.Repaylist = Repaydatalist;

                    }
                    query = "select count(*) from loan_installment_dtl a where a.loan_id = '" + request.Loan_Id + "' and a.installment_no = 1";
                    decimal cnt3 = new OracleHelper().ExecuteScalar<decimal>(query);
                    if (cnt3 > 0)
                    {
                        query = "select to_char(a.installment_amount) as emi, to_date(a.due_date) as instStartDate,( select max(to_date(a.due_date))  from loan_installment_dtl a where a.loan_id = '" + request.Loan_Id + "') as instEndDate from loan_installment_dtl a where a.loan_id='" + request.Loan_Id + "' and a.installment_no=1";
                        List<repayemiproperties> Repaydatalist1 = new OracleHelper().GetRecords<repayemiproperties>(query);
                        loanDetailsResponse.instDetailsList = Repaydatalist1;
                    }
                    loanDetailsResponse.customerDtlsList = customerlist;
                    loanDetailsResponse.Customerotherdatalist = loandatalist;
                    response.Data = loanDetailsResponse;
                    response.status = ResponseTypeContants.SUCCESS;
                    response.apiStatus = ApiStatusConstants.COMPLETED;
                    response.responseMsg = ResponseTypeContants.SUCCESS;

                }
                else if (count21 > 0)
                {    
                    query = "select a.cust_id,a.first_name ||' '||a.middle_name||' '||a.last_name name,b.house_name,b.address_line_2,b.address_line_3,b.pincode,t.loan_date,t.loan_amount,t.interest_rate,t.tenure,t.application_id as applicationId,t.loan_id as loanId, s.state_name, d.district_name,c.country_name,nvl(a.mobile_phone,a.primary_phone)mobile_phone,a.primary_phone,a.primary_email_id from vw_cv_loan_master t,vw_cv_customer a,vw_cv_customer_address b,vw_cv_state_master s,vw_cv_district_master  d,vw_cv_country_master   c where b.cust_id=a.cust_id and  a.cust_id=t.customer_id and b.is_primary_address = 'Y' and b.state_id=s.state_id  and b.district_id=d.district_id   and b.country=c.country_id and t.application_id='" + request.Loan_Id + "'";
                    List<CustomerDetailsProperties> customerlist = new OracleHelper().GetRecords<CustomerDetailsProperties>(query);
                    query = "select t.APPLICATION_ID, (select c.description from common_master c where t.int_rate_type = c.common_data_name and c.common_datatype_id=18) as INT_RATE_TYPE, (select c.description from common_master c where t.repayment_frequency = c.common_data_name and c.common_datatype_id=19)as repayment_frequency, (select c.description from common_master c where t.installment_type = c.common_data_name and c.common_datatype_id = 74) as installment_type, to_char(t.adv_inst), p.product_name, case l.loan_status when 1 then 'Active' when 0 then 'Inactive' end as loan_status, (select to_char(count(RECEIPT_DATE)) from LOAN_COLLECTION_MASTER where loan_id='" + request.Loan_Id + "' and RECEIPT_DATE<(select due_date from LOAN_INSTALLMENT_DTL where loan_id='" + request.Loan_Id + "' and installment_no=1)) as no_of_adv_installments, (select to_char(sum(COLLECTION_AMOUNT)) from LOAN_COLLECTION_MASTER where loan_id='" + request.Loan_Id + "' and RECEIPT_DATE<(select due_date from LOAN_INSTALLMENT_DTL where loan_id='" + request.Loan_Id + "' and installment_no=1)) as adv_amount from vw_cv_loan_parameters t, vw_cv_loan_master l, product_master p where t.application_id = l.application_id and l.product_id=p.product_id and l.loan_id = '" + request.Loan_Id + "'";
                    List<LoanotherDataPropeties> loandatalist = new OracleHelper().GetRecords<LoanotherDataPropeties>(query);
                    loanDetailsResponse.customerDtlsList = customerlist;
                    loanDetailsResponse.Customerotherdatalist = loandatalist;
                    response.Data = loanDetailsResponse;
                    response.status = ResponseTypeContants.SUCCESS;
                    response.apiStatus = ApiStatusConstants.COMPLETED;
                    response.responseMsg = ResponseTypeContants.SUCCESS;

                }
                else
                {
                    response.responseMsg = "Please check the loan id";
                    response.apiStatus = ApiStatusConstants.NOT_COMPLETED;
                    response.status = ResponseTypeContants.FAIL;
                }


                // string intStartDate = objDbConnectionHandler.GetRecord<string>("select INT_START_DATE intStartDate from loan_parameters where application_id=" + repaymentScheduleRequest.applicationId + "", DbConnectionHandler.SQLMode.Query, null);

            }
            catch (Exception ex)
            {
                //response.Status.flag = ProcessStatus.failed;
                //response.Status.code = APIStatus.exception;
                response.responseMsg = ex.Message;
            }
            return response;
        }
        public Response<SaveRepayDetailsResponse> SaveData(SaveRepayDetailsRequest request)
        {
            SaveRepayDetailsResponse saveRepayDetailsResponse = new SaveRepayDetailsResponse();
            Response<SaveRepayDetailsResponse> response = new Response<SaveRepayDetailsResponse>();
            try
            {
                string query = "";
                List<string> cmd = new List<string>();
                cmd.Clear();
                query = "select count(*) from tbl_oversite_master t where t.LOAN_ID='" + request.loanId + "'";
                decimal count2 = new OracleHelper().ExecuteScalar<decimal>(query);
                if (count2 > 0)
                {

                    cmd.Add("update tbl_oversite_master t set t.repay_verify_status='" + request.repaydecisionflag + "',t.repay_remarks='" + request.repayremarks + "' where t.LOAN_ID='" + request.loanId + "'");
                    new OracleHelper().ExecuteNonQuerynew(cmd.ToArray(), OracleHelper.SQLMode.Query, null);
                    response.status = ResponseTypeContants.SUCCESS;
                    response.apiStatus = ApiStatusConstants.COMPLETED;
                    response.responseMsg = ResponseTypeContants.SUCCESS;
                }
                else
                {
                    response.responseMsg = "Please check the loan id";
                    response.apiStatus = ApiStatusConstants.NOT_COMPLETED;
                    response.status = ResponseTypeContants.FAIL;
                }

            }
            catch (Exception ex)
            {
                //response.Status.flag = ProcessStatus.failed;
                //response.Status.code = APIStatus.exception;
                response.responseMsg = ex.Message;
            }
            return response;

        }
    }
}

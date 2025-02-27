using Newtonsoft.Json;
using Oversite.DTO.DashBoard.Properties;
using Oversite.DTO.DashBoard.Request;
using Oversite.DTO.DashBoard.Response;
using Oversite.DTO.ErrorLog.Request;
using Oversite.DTO.Response;
using Oversite.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Oversite.Core.DataSource.Dashboard
{
    public class DashBoardDataSource
    {
        public Response<DashBoardResponse> Data(DashBoardRequest request)
        {

            DashBoardResponse dashBoardResponse = new DashBoardResponse();
            Response<DashBoardResponse> response = new Response<DashBoardResponse>();
            
            string query = string.Empty;
            string query1 = string.Empty;
            string output = JsonConvert.SerializeObject(request);

            try
            {
                string Qblock1 = "";
                string Qblock2 = "";
                string Qblock3 = "";
                string Qblock4 = "";
                string Qblock5 = "";
                string Qblock = "";
                string Qblock6 = "";
                //string Qblocks = "";
                if (!string.IsNullOrEmpty(request.regionId))
                {
                    Qblock1 = "and t1.region_id=" + request.regionId + "";

                }
                if (!string.IsNullOrEmpty(request.branchId))
                {
                    Qblock6 = "and t1.branch_id=" + request.branchId + "";

                }
                if (!string.IsNullOrEmpty(request.productId))
                {
                    Qblock2 = "and  t1.product_id=" + request.productId + "";

                }
                if (!string.IsNullOrEmpty(request.MonthId))
                {
                    Qblock3 = "and to_date(t1.disbursed_date) between to_date('" + request.MonthId+ "','dd-MM-yyyy') and to_date('"+request.ToDate+"','dd-MM-yyyy')";

                }
                if (!string.IsNullOrEmpty(request.status))
                {
                    if(request.status=="2")
                    {
                        Qblock4 = " and t1.nch_verify is not null";
                    }
                    else if (request.status == "3")
                    {
                        Qblock4 = " and t1.nch_verify is null and t1.nch_sendback in(1,2)";
                    }
                    else if (request.status == "8")
                    {
                        Qblock4 = " and t1.rch_verify is not null ";
                    }
                    else if (request.status == "10")
                    {
                        Qblock4 = " and t1.rch_verify is null and t1.rch_sendback in(1,2)";
                    }
                    //else if (request.status == "17")
                    //{
                    //    Qblock4 = " and t1.rch_verify is not null and  ops.roh_verify is not null ";
                    //}
                    else if(request.status=="4")
                    {
                        Qblock4 = " and t1.nho_verify is not null";
                    }
                    //else if(request.status =="15")
                    //{
                    //    Qblock4 = " and t1.nho_verify is not null and t1.nch_verify is not null ";

                    //}
                    //else if (request.status == "16")
                    //{
                    //    Qblock4 = " and t1.rch_verify is null and t1.rch_sendback in(1,2) and ops.roh_verify is null and ops.roh_sendback in(1,2)";
                    //}
                    else if(request.status=="12")
                    {
                        Qblock4 = " and ops.roh_verify is null and ops.roh_sendback in(1,2)";

                    }
                    else if (request.status == "11")
                    {
                        Qblock4 = " and ops.roh_verify is not null";

                    }
                    //else if (request.status == "14")
                    //{
                    //    Qblock4 = " and t1.nch_verify is null and t1.nch_sendback in(1,2)  and t1.nho_verify is null and t1.nho_sendback in(1,2) ";

                    //}
                    else if (request.status == "5")
                    {
                        Qblock4 = " and t1.nho_verify is null and t1.nho_sendback in(1,2) ";

                    }
                    else if(request.status== "13")
                    {
                        Qblock4 = " and ( t1.nho_sendback  =2 or ops.roh_sendback =2)";
                    }
                    else
                    {
                        if (request.roleId == "55" || request.roleId == "50" || request.roleId == "19"  )
                        {
                            Qblock4 = "and t1.cr_verification_status = " + request.status + "";

                        }
                        else
                        {
                            Qblock4 = "and t1.verification_status = " + request.status + "";
                        }
                    }
                }

                if (!string.IsNullOrEmpty(request.searchValue))
                {
                    Qblock1 = ""; Qblock2 = ""; Qblock3 = ""; Qblock4 = ""; Qblock6 = "";
                    Qblock5 = "and (t1.application_id='" + request.searchValue + "' or t1.Loan_Id='" + request.searchValue + "')";

                }
                //Qblocks = Qblock1 + Qblock2 + Qblock3 + Qblock5;
                Qblock = Qblock1 + Qblock2 + Qblock3 + Qblock4 + Qblock5+ Qblock6;
                decimal role_Id = new OracleHelper().ExecuteScalar<decimal>("select count(*) from lms_tw.USER_ROLE_DETAILS t1 left outer join lms_tw.user_master t2 ON  t1.user_id = t2.user_id  WHERE t2.emp_code = '" + request.enterBy + "' and t1.Role_id=1");
                if (role_Id > 0)
                {
                    query = "select to_char(t1.Loan_Id) LoanId,to_char(t1.application_id) ApplicationNo,t1.customer_name cusName,to_char(t1.disbursed_date,'dd-mm-yyyy HH24:mi:ss') disDate,t2.branch_name branch,to_char(t1.loan_amount) as loanAmount,s.ovrdescription as status,to_char(s.ovrstatusid) as statusId,'1' as redirectFlag,ph.tat from lms_tw.TBL_OVERSITE_MASTER t1 left outer join lms_tw.branch_master t2 on t1.branch_id=t2.branch_id " +
                    " left outer join lms_tw.oversite_status_master s on s.ovrstatusid = t1.verification_status left outer join vw_public_holiday ph on t1.application_id=ph.application_id where (nvl(t1.verification_status,0) not in (6)) " + Qblock + "";
                    dashBoardResponse.DataList = new OracleHelper().GetRecords<DashBoardProperties>(query);

                }
                else
                {
                    decimal role_ro_cnt = new OracleHelper().ExecuteScalar<decimal>("select count(*) from lms_tw.vw_cv_User_Role_Details t1 left outer join lms_tw.vw_cv_user_master t2 ON  t1.user_id = t2.user_id  WHERE t2.emp_code = '" + request.enterBy + "' and t1.Role_id=7");//CVro
                    decimal role_cvnho_cnt = new OracleHelper().ExecuteScalar<decimal>("select count(*) from lms_tw.vw_cv_User_Role_Details t1 left outer join lms_tw.vw_cv_user_master t2 ON  t1.user_id = t2.user_id  WHERE t2.emp_code = '" + request.enterBy + "' and t1.Role_id=49");//cv nho 
                    decimal role_cvnch_cnt = new OracleHelper().ExecuteScalar<decimal>("select count(*) from lms_tw.vw_cv_User_Role_Details t1 left outer join lms_tw.vw_cv_user_master t2 ON  t1.user_id = t2.user_id  WHERE t2.emp_code = '" + request.enterBy + "' and t1.Role_id=50");//cv nch 
                    decimal role_cvrm_cnt = new OracleHelper().ExecuteScalar<decimal>("select count(*) from lms_tw.vw_cv_User_Role_Details t1 left outer join lms_tw.vw_cv_user_master t2 ON  t1.user_id = t2.user_id  WHERE t2.emp_code = '" + request.enterBy + "' and t1.Role_id=48");//cv rm
                    decimal role_cvrch_cnt = new OracleHelper().ExecuteScalar<decimal>("select count(*) from lms_tw.vw_cv_User_Role_Details t1 left outer join lms_tw.vw_cv_user_master t2 ON  t1.user_id = t2.user_id  WHERE t2.emp_code = '" + request.enterBy + "' and t1.Role_id=55");//cv rch 
                    decimal role_cvroh_cnt = new OracleHelper().ExecuteScalar<decimal>("select count(*) from lms_tw.vw_cv_User_Role_Details t1 left outer join lms_tw.vw_cv_user_master t2 ON  t1.user_id = t2.user_id  WHERE t2.emp_code = '" + request.enterBy + "' and t1.Role_id=52");//cv rho 

                    decimal role_cvbcm_cnt = new OracleHelper().ExecuteScalar<decimal>("select count(*) from lms_tw.vw_cv_User_Role_Details t1 left outer join lms_tw.vw_cv_user_master t2 ON  t1.user_id = t2.user_id  WHERE t2.emp_code = '" + request.enterBy + "' and t1.Role_id=19"); //cv bcm
                                                                                                                                                                                                                                                                                          
                    decimal role_bco = new OracleHelper().ExecuteScalar<decimal>("select count(*) from lms_tw.vw_cv_User_Role_Details t1 left outer join lms_tw.vw_cv_user_master t2 ON  t1.user_id = t2.user_id  WHERE t2.emp_code = '" + request.enterBy + "' and t1.Role_id=74");//cv bco

                    decimal role_report = new OracleHelper().ExecuteScalar<decimal>("select count(*) from lms_tw.vw_cv_User_Role_Details t1 left outer join lms_tw.vw_cv_user_master t2 ON  t1.user_id = t2.user_id  WHERE t2.emp_code = '" + request.enterBy + "' and t1.Role_id=83");//cv oversite_complete
                    decimal role_cvnho_ass_cnt = new OracleHelper().ExecuteScalar<decimal>("select count(*) from lms_tw.vw_cv_User_Role_Details t1 left outer join lms_tw.vw_cv_user_master t2 ON  t1.user_id = t2.user_id  WHERE t2.emp_code = '" + request.enterBy + "' and t1.Role_id=84");// cv nho_assistant


                    List<DashBoardProperties> Dashboardlist = new List<DashBoardProperties>();



                    if (role_bco > 0 && role_cvbcm_cnt == 0)//BCO
                    {
                        DashBoardProperties Dashboardpro = new DashBoardProperties();

                        if (!string.IsNullOrEmpty(request.status.ToString()))
                        {
                            if (request.status == "12" || request.status == "5" /*|| request.status == "14"*/)
                            {
                                Dashboardlist = new OracleHelper().GetRecords<DashBoardProperties>("select to_char(t1.Loan_Id) LoanId, to_char(t1.application_id) ApplicationNo, t1.customer_name cusName, to_char(t1.disbursed_date, 'dd-mm-yyyy') disDate, t2.branch_name branch, to_char(t1.loan_amount) as loanAmount, case nvl(t1.verification_status, 0) when 4 then 'Verify By NOH' when 5 then case nvl(t1.nho_sendback, 0) when 2 then 'Recaptured' when 1 then 'Sendback by NOH' end when 11 then 'Verify By ROH' when 13 then 'Verify By BCO' when 12 then case nvl(OPs.ROH_SENDBACK, 0) when 2 then 'Recaptured' when 1 then 'Sendback by ROH' end else 'Not Verified' end status, to_char(s.ovrstatusid) as statusId, '1' as redirectFlag, '2' roleid, c.num_weekdays TAT from lms_tw.TBL_OVERSITE_MASTER t1 CROSS APPLY (SELECT COUNT(*) AS num_weekdays FROM dual WHERE TO_CHAR(to_date(t1.disbursed_date, 'dd-mm-yyyy') + LEVEL, 'DY', 'NLS_DATE_LANGUAGE=ENGLISH') NOT IN ('SUN') AND TO_CHAR(to_date(t1.disbursed_date, 'dd-mm-yyyy') + LEVEL, 'DD-MM', 'NLS_DATE_LANGUAGE=ENGLISH') NOT IN ('26-01', '02-10', '01-05', '15-08') CONNECT BY LEVEL < sysdate - to_date(t1.disbursed_date, 'dd-mm-yyyy')) c left outer join lms_tw.TBL_OPS_OVERSITE_MASTER ops on t1.application_id=ops.application_id left outer join lms_tw.branch_master t2 on t1.branch_id = t2.branch_id left outer join lms_tw.oversite_status_master s on s.ovrstatusid = t1.cr_verification_status where ((t1.nho_sendback = 1 or OPs.ROH_SENDBACK in (1))) " + Qblock + " order by c.num_weekdays desc");
                            }
                            else
                            {
                                Dashboardlist = new OracleHelper().GetRecords<DashBoardProperties>("select to_char(t1.Loan_Id) LoanId, to_char(t1.application_id) ApplicationNo, t1.customer_name cusName, to_char(t1.disbursed_date, 'dd-mm-yyyy') disDate, t2.branch_name branch, to_char(t1.loan_amount) as loanAmount, case nvl(t1.verification_status, 0) when 4 then 'Verified By NOH' when 5 then case nvl(t1.nho_Sendback, 0) when 2 then 'Recaptured' when 1 then 'Sendback by NOH' end when 11 then 'Verified By ROH' when 13 then 'Verified By BCO' when 12 then case nvl(ops.roh_sendback, 0) when 2 then 'Recaptured' when 1 then 'Sendback by ROH' end else 'Not Verified' end status, to_char(s.ovrstatusid) as statusId, '0' as redirectFlag, '2' roleid, c.num_weekdays TAT from lms_tw.TBL_OVERSITE_MASTER t1 CROSS APPLY (SELECT COUNT(*) AS num_weekdays FROM dual WHERE TO_CHAR(to_date(t1.disbursed_date, 'dd-mm-yyyy') + LEVEL, 'DY', 'NLS_DATE_LANGUAGE=ENGLISH') NOT IN ('SUN') AND TO_CHAR(to_date(t1.disbursed_date, 'dd-mm-yyyy') + LEVEL, 'DD-MM', 'NLS_DATE_LANGUAGE=ENGLISH') NOT IN ('26-01', '02-10', '01-05', '15-08') CONNECT BY LEVEL < sysdate - to_date(t1.disbursed_date, 'dd-mm-yyyy')) c left outer join lms_tw.TBL_OPS_OVERSITE_MASTER ops on t1.application_id=ops.application_id left outer join lms_tw.branch_master t2 on t1.branch_id = t2.branch_id left outer join lms_tw.oversite_status_master s on s.ovrstatusid = t1.cr_verification_status where (t1.cr_verification_status not in (6)) " + Qblock + " order by c.num_weekdays desc");

                            }
                        }
                        else
                        {
                            Dashboardlist = new OracleHelper().GetRecords<DashBoardProperties>("select to_char(t1.Loan_Id) LoanId, to_char(t1.application_id) ApplicationNo, t1.customer_name cusName, to_char(t1.disbursed_date, 'dd-mm-yyyy') disDate, t2.branch_name branch, to_char(t1.loan_amount) as loanAmount, case nvl(t1.verification_status, 0) when 4 then 'Verified By NOH' when 5 then case nvl(t1.nho_Sendback, 0) when 2 then 'Recaptured' when 1 then 'Sendback by NOH' end when 11 then 'Verified By ROH' when 13 then 'Verified By BCO' when 12 then case nvl(ops.roh_sendback, 0) when 2 then 'Recaptured' when 1 then 'Sendback by ROH' end else 'Not Verified' end status, to_char(s.ovrstatusid) as statusId, '1' as redirectFlag, '2' roleid, c.num_weekdays TAT from lms_tw.TBL_OVERSITE_MASTER t1 CROSS APPLY (SELECT COUNT(*) AS num_weekdays FROM dual WHERE TO_CHAR(to_date(t1.disbursed_date, 'dd-mm-yyyy') + LEVEL, 'DY', 'NLS_DATE_LANGUAGE=ENGLISH') NOT IN ('SUN') AND TO_CHAR(to_date(t1.disbursed_date, 'dd-mm-yyyy') + LEVEL, 'DD-MM', 'NLS_DATE_LANGUAGE=ENGLISH') NOT IN ('26-01', '02-10', '01-05', '15-08') CONNECT BY LEVEL < sysdate - to_date(t1.disbursed_date, 'dd-mm-yyyy')) c left outer join lms_tw.TBL_OPS_OVERSITE_MASTER ops on t1.application_id=ops.application_id left outer join lms_tw.branch_master t2 on t1.branch_id = t2.branch_id left outer join lms_tw.oversite_status_master s on s.ovrstatusid = t1.cr_verification_status where (/*(t1.verification_status not in (1, 2, 4, 5, 6, 11, 13)) AND*/ (t1.nho_sendback = 1 or ops.roh_sendback in (1))) " + Qblock + " order by c.num_weekdays desc");

                        }

                    }
                    if (role_cvbcm_cnt > 0 && role_bco ==0)//BCM
                    {
                        DashBoardProperties Dashboardpro = new DashBoardProperties();

                        if (!string.IsNullOrEmpty(request.status.ToString()))
                        {
                            if (request.status == "10" || request.status == "3" || request.status == "14")
                            {
                                Dashboardlist = new OracleHelper().GetRecords<DashBoardProperties>("select to_char(t1.Loan_Id) LoanId,to_char(t1.application_id) ApplicationNo,t1.customer_name cusName,to_char(t1.disbursed_date, 'dd-mm-yyyy') disDate,t2.branch_name branch,to_char(t1.loan_amount) as loanAmount,case nvl(t1.cr_verification_status, 0) when 2 then 'Verify By NCH' when 3 then case nvl(t1.nch_sendback, 0) when 2 then 'Recaptured' when 1 then 'Sendback by NCH' end when 8 then 'Verify By RCH' when 9 then 'Verify By BCM' when 10 then case nvl(t1.rch_sendback, 0) when 2 then 'Recaptured' when 1 then 'Sendback by RCH' end else 'Not Verified' end status,to_char(s.ovrstatusid) as statusId,'1' as redirectFlag,'19' roleid,c.num_weekdays TAT " +
                                    "from TBL_OVERSITE_MASTER t1 CROSS APPLY ( SELECT COUNT (*) AS num_weekdays FROM dual WHERE TO_CHAR ( to_date(t1.disbursed_date,'dd-mm-yyyy') + LEVEL , 'DY' , 'NLS_DATE_LANGUAGE=ENGLISH' ) NOT IN ('SUN') AND TO_CHAR ( to_date(t1.disbursed_date,'dd-mm-yyyy') + LEVEL , 'DD-MM' , 'NLS_DATE_LANGUAGE=ENGLISH' ) NOT IN ('26-01','02-10','01-05','15-08') CONNECT BY LEVEL < sysdate - to_date(t1.disbursed_date,'dd-mm-yyyy') ) c left outer join branch_master t2 on t1.branch_id = t2.branch_id left outer join oversite_status_master s on s.ovrstatusid = t1.cr_verification_status where (( t1.nho_sendback = 1 or t1.rch_sendback in(1) or t1.application_id in(select distinct(tt.application_id) from TBL_OVERSITE_DOCUMENTS_MST tt where tt.nch_sendback_to='B'  and tt.nch_sendback_flag=1))) " + Qblock + " order by c.num_weekdays desc");
                            }
                            //else if(request.status == "3" || request.status == "7" || request.status == "5" || request.status == "4")
                            //{
                            //    Dashboardlist = new OracleHelper().GetRecords<DashBoardProperties>("select to_char(t1.Loan_Id) LoanId,to_char(t1.application_id) ApplicationNo,t1.customer_name cusName,to_char(t1.disbursed_date, 'dd-mm-yyyy HH24:mi:ss') disDate,t2.branch_name branch,to_char(t1.loan_amount) as loanAmount,case nvl(t1.nch_sendback, 0)when 1 then case nvl(t1.nho_sendback, 0) when 1 then 'NHO & NCH sendback' else 'NCH Sendback' end else case nvl(t1.nho_sendback, 0) WHEN 1 then 'NHO sendback' end end status,to_char(s.ovrstatusid) as statusId,'1' as redirectFlag,'19' roleid from TBL_OVERSITE_MASTER t1 left outer join branch_master t2 on t1.branch_id = t2.branch_id left outer join oversite_status_master s on s.ovrstatusid = t1.verification_status where ((t1.verification_status not in (6)) or(t1.nch_sendback = 1 or t1.nho_sendback = 1 or t1.verification_status in(1,3,5,7,4))) " + Qblock + "");
                            //}
                            else
                            {
                                Dashboardlist = new OracleHelper().GetRecords<DashBoardProperties>("select to_char(t1.Loan_Id) LoanId,to_char(t1.application_id) ApplicationNo,t1.customer_name cusName,to_char(t1.disbursed_date, 'dd-mm-yyyy') disDate,t2.branch_name branch,to_char(t1.loan_amount) as loanAmount,case nvl(t1.cr_verification_status, 0) when 2 then 'Verify By NCH' when 3 then case nvl(t1.nch_sendback, 0) when 2 then 'Recaptured' when 1 then 'Sendback by NCH' end when 8 then 'Verify By RCH' when 9 then 'Verify By BCM' when 10 then case nvl(t1.rch_sendback, 0) when 2 then 'Recaptured' when 1 then 'Sendback by RCH' end else 'Not Verified' end status,to_char(s.ovrstatusid) as statusId,'0' as redirectFlag,'19' roleid,c.num_weekdays TAT " +
                                    "from TBL_OVERSITE_MASTER t1 CROSS APPLY ( SELECT COUNT (*) AS num_weekdays FROM dual WHERE TO_CHAR ( to_date(t1.disbursed_date,'dd-mm-yyyy') + LEVEL , 'DY' , 'NLS_DATE_LANGUAGE=ENGLISH' ) NOT IN ('SUN') AND TO_CHAR ( to_date(t1.disbursed_date,'dd-mm-yyyy') + LEVEL , 'DD-MM' , 'NLS_DATE_LANGUAGE=ENGLISH' ) NOT IN ('26-01','02-10','01-05','15-08') CONNECT BY LEVEL < sysdate - to_date(t1.disbursed_date,'dd-mm-yyyy') ) c left outer join branch_master t2 on t1.branch_id = t2.branch_id left outer join oversite_status_master s on s.ovrstatusid = t1.cr_verification_status where (t1.cr_verification_status not in (6))" + Qblock + " order by c.num_weekdays desc");

                            }
                        }
                        else
                        {
                            Dashboardlist = new OracleHelper().GetRecords<DashBoardProperties>("select to_char(t1.Loan_Id) LoanId,to_char(t1.application_id) ApplicationNo,t1.customer_name cusName,to_char(t1.disbursed_date, 'dd-mm-yyyy') disDate,t2.branch_name branch,to_char(t1.loan_amount) as loanAmount,case nvl(t1.cr_verification_status, 0) when 2 then 'Verify By NCH' when 3 then case nvl(t1.nch_sendback, 0) when 2 then 'Recaptured' when 1 then 'Sendback by NCH' end when 8 then 'Verify By RCH' when 9 then 'Verify By BCM' when 10 then case nvl(t1.rch_sendback, 0) when 2 then 'Recaptured' when 1 then 'Sendback by RCH' end else 'Not Verified' end status,to_char(s.ovrstatusid) as statusId,'1' as redirectFlag,'19' roleid,c.num_weekdays TAT " +
                                "from TBL_OVERSITE_MASTER t1 CROSS APPLY ( SELECT COUNT (*) AS num_weekdays FROM dual WHERE TO_CHAR ( to_date(t1.disbursed_date,'dd-mm-yyyy') + LEVEL , 'DY' , 'NLS_DATE_LANGUAGE=ENGLISH' ) NOT IN ('SUN') AND TO_CHAR ( to_date(t1.disbursed_date,'dd-mm-yyyy') + LEVEL , 'DD-MM' , 'NLS_DATE_LANGUAGE=ENGLISH' ) NOT IN ('26-01','02-10','01-05','15-08') CONNECT BY LEVEL < sysdate - to_date(t1.disbursed_date,'dd-mm-yyyy') ) c left outer join branch_master t2 on t1.branch_id = t2.branch_id left outer join oversite_status_master s on s.ovrstatusid = t1.cr_verification_status where ((t1.cr_verification_status not in(1,2,4,5, 6, 8, 9)) AND ( t1.nho_sendback = 1 or t1.rch_sendback in(1) or t1.application_id in(select distinct(tt.application_id) from TBL_OVERSITE_DOCUMENTS_MST tt where tt.nch_sendback_to='B'  and tt.nch_sendback_flag=1)))" + Qblock + " order by c.num_weekdays desc");

                        }

                    }


                    if ( (role_cvnho_cnt > 0  || role_cvnho_ass_cnt > 0)&& role_cvnch_cnt ==0)//NHO
                    {
                        DashBoardProperties Dashboardpro = new DashBoardProperties();

                        if (!string.IsNullOrEmpty(request.status.ToString()))
                        {
                            if (request.status == "1" || request.status == "2")
                            {
                                Dashboardlist = new OracleHelper().GetRecords<DashBoardProperties>("select to_char(t1.Loan_Id) LoanId,to_char(t1.application_id) ApplicationNo,t1.customer_name cusName,to_char(t1.disbursed_date,'dd-mm-yyyy') disDate,t2.branch_name branch,to_char(t1.loan_amount) as loanAmount,s.ovrdescription || decode(t1.NHO_sendback,2,' RECAPTURED') as status,to_char(s.ovrstatusid) as statusId,'1' as redirectFlag,c.num_weekdays tat from TBL_OVERSITE_MASTER t1  left outer join tbl_ops_oversite_master o on t1.application_id = o.application_id and t1.loan_id = o.loan_id CROSS APPLY ( SELECT COUNT (*) AS num_weekdays FROM dual WHERE TO_CHAR ( to_date(o.roh_verify_date,'dd-mm-yyyy') + LEVEL , 'DY' , 'NLS_DATE_LANGUAGE=ENGLISH' ) NOT IN ('SUN') AND TO_CHAR ( to_date(o.roh_verify_date,'dd-mm-yyyy') + LEVEL , 'DD-MM' , 'NLS_DATE_LANGUAGE=ENGLISH' ) NOT IN ('26-01','02-10','01-05','15-08') CONNECT BY LEVEL < sysdate - to_date(o.roh_verify_date,'dd-mm-yyyy') ) c left outer join branch_master t2 on t1.branch_id = t2.branch_id " +
                                " left outer join oversite_status_master s on s.ovrstatusid = t1.verification_status where ( nvl(t1.verification_status,0) not in (6,7)) " + Qblock + "");
                                //Dashboardlist.Add(Dashboardpro);

                            }
                            else if (request.status == "5")
                            {
                                Dashboardlist = new OracleHelper().GetRecords<DashBoardProperties>("select to_char(t1.Loan_Id) LoanId,to_char(t1.application_id) ApplicationNo,t1.customer_name cusName,to_char(t1.disbursed_date,'dd-mm-yyyy') disDate,t2.branch_name branch,to_char(t1.loan_amount) as loanAmount,s.ovrdescription || decode(t1.NHO_sendback,2,' RECAPTURED') as status,/*to_char(s.ovrstatusid)*/ '1'  as statusId, '1' as redirectFlag,c.num_weekdays tat from TBL_OVERSITE_MASTER t1 left outer join tbl_ops_oversite_master o on t1.application_id = o.application_id and t1.loan_id = o.loan_id CROSS APPLY ( SELECT COUNT (*) AS num_weekdays FROM dual WHERE TO_CHAR ( to_date(o.roh_verify_date,'dd-mm-yyyy') + LEVEL , 'DY' , 'NLS_DATE_LANGUAGE=ENGLISH' ) NOT IN ('SUN') AND TO_CHAR ( to_date(o.roh_verify_date,'dd-mm-yyyy') + LEVEL , 'DD-MM' , 'NLS_DATE_LANGUAGE=ENGLISH' ) NOT IN ('26-01','02-10','01-05','15-08') CONNECT BY LEVEL < sysdate - to_date(o.roh_verify_date,'dd-mm-yyyy') ) c left outer join branch_master t2 on t1.branch_id = t2.branch_id left outer join oversite_status_master s on s.ovrstatusid = t1.verification_status " +
                                  "  where t1.verification_status=5 and  t1.NHO_sendback=2 " + Qblock + " union select to_char(t1.Loan_Id) LoanId,to_char(t1.application_id) ApplicationNo,t1.customer_name cusName,to_char(t1.disbursed_date,'dd-mm-yyyy HH24:mi:ss') disDate,t2.branch_name branch,to_char(t1.loan_amount) as loanAmount,s.ovrdescription || decode(t1.NHO_sendback,2,' RECAPTURED') as status,to_char(s.ovrstatusid) as statusId, '0' as redirectFlag,c.num_weekdays tat from TBL_OVERSITE_MASTER t1  left outer join tbl_ops_oversite_master o on t1.application_id = o.application_id and t1.loan_id = o.loan_id CROSS APPLY ( SELECT COUNT (*) AS num_weekdays FROM dual WHERE TO_CHAR ( to_date(o.roh_verify_date,'dd-mm-yyyy') + LEVEL , 'DY' , 'NLS_DATE_LANGUAGE=ENGLISH' ) NOT IN ('SUN') AND TO_CHAR ( to_date(o.roh_verify_date,'dd-mm-yyyy') + LEVEL , 'DD-MM' , 'NLS_DATE_LANGUAGE=ENGLISH' ) NOT IN ('26-01','02-10','01-05','15-08') CONNECT BY LEVEL < sysdate - to_date(o.roh_verify_date,'dd-mm-yyyy') ) c left outer join branch_master t2 on t1.branch_id = t2.branch_id left outer join oversite_status_master s on s.ovrstatusid = t1.verification_status where (t1.verification_status=5  and  t1.NHO_sendback=1) " + Qblock + "");
                                //Dashboardlist.Add(Dashboardpro);

                            }
                            else
                            {
                                Dashboardlist = new OracleHelper().GetRecords<DashBoardProperties>("select to_char(t1.Loan_Id) LoanId,to_char(t1.application_id) ApplicationNo,t1.customer_name cusName,to_char(t1.disbursed_date,'dd-mm-yyyy') disDate,t2.branch_name branch,to_char(t1.loan_amount) as loanAmount,s.ovrdescription || decode(t1.NHO_sendback,2,' RECAPTURED') as status,to_char(s.ovrstatusid),'0' as redirectFlag,c.num_weekdays TAT from lms_tw.TBL_OVERSITE_MASTER t1 left outer join lms_tw.TBL_OPS_OVERSITE_MASTER ops on t1.application_id = ops.application_id CROSS APPLY (SELECT COUNT(*) AS num_weekdays FROM dual WHERE TO_CHAR(to_date(ops.roh_verify_date, 'dd-mm-yyyy') + LEVEL, 'DY', 'NLS_DATE_LANGUAGE=ENGLISH') NOT IN ('SUN') AND TO_CHAR(to_date(ops.roh_verify_date, 'dd-mm-yyyy') + LEVEL, 'DD-MM', 'NLS_DATE_LANGUAGE=ENGLISH') NOT IN ('26-01', '02-10', '01-05', '15-08') CONNECT BY LEVEL < sysdate - to_date(ops.roh_verify_date, 'dd-mm-yyyy')) c left outer join lms_tw.branch_master t2 on t1.branch_id = t2.branch_id" +
                                     " left outer join lms_tw.oversite_status_master s on s.ovrstatusid = t1.verification_status  where (nvl(t1.verification_status,0) not in (6,7))" + Qblock + "");
                                //Dashboardlist.Add(Dashboardpro);
                            }
                        }
                        else
                        {
                            Dashboardlist = new OracleHelper().GetRecords<DashBoardProperties>("select to_char(t1.Loan_Id) LoanId, to_char(t1.application_id) ApplicationNo, t1.customer_name cusName, to_char(t1.disbursed_date, 'dd-mm-yyyy') disDate, t2.branch_name branch, to_char(t1.loan_amount) as loanAmount, case nvl(t1.verification_status, 0) when 4 then 'Verify By NHO' when 5 then case nvl(t1.Nho_Sendback, 0) when 2 then 'Recaptured' when 1 then 'Sendback by NHO' end when 11 then 'Verify By RHO' when 13 then 'Verify By BHO' when 12 then case nvl(ops.roh_sendback, 0) when 2 then 'Recaptured' when 1 then 'Sendback by RHO' end else 'Not Verified' end status, '1' as statusId,/*case when ops.roh_verify is null then '0' else '1' end*/  '1' as redirectFlag,c.num_weekdays tat from TBL_OVERSITE_MASTER t1  left outer join tbl_ops_oversite_master ops on t1.application_id = ops.application_id and t1.loan_id = ops.loan_id CROSS APPLY ( SELECT COUNT (*) AS num_weekdays FROM dual WHERE TO_CHAR ( to_date(ops.roh_verify_date,'dd-mm-yyyy') + LEVEL , 'DY' , 'NLS_DATE_LANGUAGE=ENGLISH' ) NOT IN ('SUN') AND TO_CHAR ( to_date(ops.roh_verify_date,'dd-mm-yyyy') + LEVEL , 'DD-MM' , 'NLS_DATE_LANGUAGE=ENGLISH' ) NOT IN ('26-01','02-10','01-05','15-08') CONNECT BY LEVEL < sysdate - to_date(ops.roh_verify_date,'dd-mm-yyyy') ) c left outer join branch_master t2 on t1.branch_id = t2.branch_id " +
                            " left outer join oversite_status_master s on s.ovrstatusid = t1.verification_status where  (t1.verification_status not in (5,4) and nvl(t1.nho_verify,0)=0 or (t1.verification_status=5  and t1.NHO_sendback in(2))or (t1.verification_status = 13 and ops.roh_sendback in(2))) " + Qblock + "");

                            //Dashboardlist = new OracleHelper().GetRecords<DashBoardProperties>("select to_char(t1.Loan_Id) LoanId,to_char(t1.application_id) ApplicationNo,t1.customer_name cusName,to_char(t1.disbursed_date,'dd-mm-yyyy') disDate,t2.branch_name branch,to_char(t1.loan_amount) as loanAmount, case nvl(t1.nho_verify, 0) when 1 then 'Verified' else case nvl(t1.nho_sendback, 0) when 1 then 'Sendback' when 2 then 'Sendback Recaptured' else 'Not Verified' end end status,/*s.ovrdescription || decode(t1.NHO_sendback,2,' RECAPTURED') as status,*//*to_char(s.ovrstatusid)*/ '1' as statusId,'1' as redirectFlag,c.num_weekdays tat from TBL_OVERSITE_MASTER t1 CROSS APPLY ( SELECT COUNT (*) AS num_weekdays FROM dual WHERE TO_CHAR ( to_date(t1.disbursed_date,'dd-mm-yyyy') + LEVEL , 'DY' , 'NLS_DATE_LANGUAGE=ENGLISH' ) NOT IN ('SUN') AND TO_CHAR ( to_date(t1.disbursed_date,'dd-mm-yyyy') + LEVEL , 'DD-MM' , 'NLS_DATE_LANGUAGE=ENGLISH' ) NOT IN ('26-01','02-10','01-05','15-08') CONNECT BY LEVEL < sysdate - to_date(t1.disbursed_date,'dd-mm-yyyy') ) c    left outer join tbl_ops_oversite_master o on t1.application_id=o.application_id and t1.loan_id=o.loan_id left outer join branch_master t2 on t1.branch_id = t2.branch_id " +
                            //" left outer join oversite_status_master s on s.ovrstatusid = t1.verification_status where  (t1.verification_status in(1,2,3,5,13) and nvl(t1.nho_verify,0)=0 or (t1.verification_status=5  and t1.NHO_sendback=2)or (t1.verification_status = 13 and o.roh_sendback=2))" + Qblock + "");
                            //Dashboardlist.Add(Dashboardpro);

                        }

                    }
                    if (role_cvnch_cnt > 0 && role_cvnho_cnt ==0) //NCH
                    {
                        DashBoardProperties Dashboardpro = new DashBoardProperties();

                        if (!string.IsNullOrEmpty(request.status.ToString()))
                        {

                            if (request.status == "3")
                            {
                                Dashboardlist = new OracleHelper().GetRecords<DashBoardProperties>("select to_char(t1.Loan_Id) LoanId,to_char(t1.application_id) ApplicationNo,t1.customer_name cusName,to_char(t1.disbursed_date,'dd-mm-yyyy') disDate,t2.branch_name branch,to_char(t1.loan_amount) as loanAmount,case nvl(t1.cr_verification_status, 0) when 2 then 'Verify By NCH' when 3 then case nvl(t1.nch_sendback, 0) when 2 then 'Recaptured' when 1 then 'Sendback by NCH' end when 8 then 'Verify By RCH' when 9 then 'Verify By BCM' when 10 then case nvl(t1.rch_sendback, 0) when 2 then 'Recaptured' when 1 then 'Sendback by RCH' end else 'Not Verified' end status,to_char(s.ovrstatusid) as statusId,case t1.nch_sendback when 1 then '0' else '1' end as redirectFlag,'50' roleid,c.num_weekdays TAT from TBL_OVERSITE_MASTER t1 CROSS APPLY ( SELECT COUNT (*) AS num_weekdays FROM dual WHERE TO_CHAR ( to_date(t1.disbursed_date,'dd-mm-yyyy') + LEVEL , 'DY' , 'NLS_DATE_LANGUAGE=ENGLISH' ) NOT IN ('SUN') AND TO_CHAR ( to_date(t1.disbursed_date,'dd-mm-yyyy') + LEVEL , 'DD-MM' , 'NLS_DATE_LANGUAGE=ENGLISH' ) NOT IN ('26-01','02-10','01-05','15-08') CONNECT BY LEVEL < sysdate - to_date(t1.disbursed_date,'dd-mm-yyyy') ) c left outer join branch_master t2 on t1.branch_id = t2.branch_id " +
                                " left outer join oversite_status_master s on s.ovrstatusid = t1.cr_verification_status where  t1.cr_verification_status is not null " + Qblock + " order by c.num_weekdays desc");
                            }
                            else
                            {
                                Dashboardlist = new OracleHelper().GetRecords<DashBoardProperties>("select to_char(t1.Loan_Id) LoanId,to_char(t1.application_id) ApplicationNo,t1.customer_name cusName,to_char(t1.disbursed_date,'dd-mm-yyyy') disDate,t2.branch_name branch,to_char(t1.loan_amount) as loanAmount,case nvl(t1.cr_verification_status, 0) when 2 then 'Verify By NCH' when 3 then case nvl(t1.nch_sendback, 0) when 2 then 'Recaptured' when 1 then 'Sendback by NCH' end when 8 then 'Verify By RCH' when 9 then 'Verify By BCM' when 10 then case nvl(t1.rch_sendback, 0) when 2 then 'Recaptured' when 1 then 'Sendback by RCH' end else 'Not Verified' end status,case t1.nch_sendback when 2 then '8' else to_char(s.ovrstatusid) end as statusId,'1' as redirectFlag,'50' roleid,c.num_weekdays TAT from TBL_OVERSITE_MASTER t1 CROSS APPLY ( SELECT COUNT (*) AS num_weekdays FROM dual WHERE TO_CHAR ( to_date(t1.disbursed_date,'dd-mm-yyyy') + LEVEL , 'DY' , 'NLS_DATE_LANGUAGE=ENGLISH' ) NOT IN ('SUN') AND TO_CHAR ( to_date(t1.disbursed_date,'dd-mm-yyyy') + LEVEL , 'DD-MM' , 'NLS_DATE_LANGUAGE=ENGLISH' ) NOT IN ('26-01','02-10','01-05','15-08') CONNECT BY LEVEL < sysdate - to_date(t1.disbursed_date,'dd-mm-yyyy') ) c left outer join branch_master t2 on t1.branch_id = t2.branch_id" +
                                     " left outer join oversite_status_master s on s.ovrstatusid = t1.cr_verification_status where t1.cr_verification_status is not null " + Qblock + " order by c.num_weekdays desc");
                                //Dashboardlist.Add(Dashboardpro);
                            }
                        }
                        else
                        {
                            Dashboardlist = new OracleHelper().GetRecords<DashBoardProperties>("select to_char(t1.Loan_Id) LoanId,to_char(t1.application_id) ApplicationNo,t1.customer_name cusName,to_char(t1.disbursed_date,'dd-mm-yyyy') disDate,t2.branch_name branch,to_char(t1.loan_amount) as loanAmount,case nvl(t1.cr_verification_status, 0) when 2 then 'Verify By NCH' when 3 then case nvl(t1.nch_sendback, 0) when 2 then 'Recaptured' when 1 then 'Sendback by NCH' end when 8 then 'Verify By RCH' when 9 then 'Verify By BCM' when 10 then case nvl(t1.rch_sendback, 0) when 2 then 'Recaptured' when 1 then 'Sendback by RCH' end else 'Not Verified' end status,case t1.nch_sendback when 2 then '8' else to_char(s.ovrstatusid) end as statusId,case t1.nch_sendback when 1 then '0' else '1' end as redirectFlag,'50' roleid,c.num_weekdays TAT from TBL_OVERSITE_MASTER t1 CROSS APPLY ( SELECT COUNT (*) AS num_weekdays FROM dual WHERE TO_CHAR ( to_date(t1.disbursed_date,'dd-mm-yyyy') + LEVEL , 'DY' , 'NLS_DATE_LANGUAGE=ENGLISH' ) NOT IN ('SUN') AND TO_CHAR ( to_date(t1.disbursed_date,'dd-mm-yyyy') + LEVEL , 'DD-MM' , 'NLS_DATE_LANGUAGE=ENGLISH' ) NOT IN ('26-01','02-10','01-05','15-08') CONNECT BY LEVEL < sysdate - to_date(t1.disbursed_date,'dd-mm-yyyy') ) c left outer join branch_master t2 on t1.branch_id = t2.branch_id " +
                            " left outer join oversite_status_master s on s.ovrstatusid = t1.cr_verification_status where  (t1.cr_verification_status  not in(3) and nvl(t1.nch_verify,0)=0 or (t1.NCH_sendback=2))" + Qblock + " order by c.num_weekdays desc");
                            //Dashboardlist.Add(Dashboardpro);

                        }


                    }
                    if (/*role_rm_cnt > 0||*/ role_cvrm_cnt > 0)//RM
                    {
                        DashBoardProperties Dashboardpro = new DashBoardProperties();

                        if (!string.IsNullOrEmpty(request.status.ToString()))
                        {
                            if (request.status == "3" || request.status == "5")
                            {
                                Dashboardlist = new OracleHelper().GetRecords<DashBoardProperties>("select to_char(t1.Loan_Id) LoanId,to_char(t1.application_id) ApplicationNo,t1.customer_name cusName,to_char(t1.disbursed_date,'dd-mm-yyyy HH24:mi:ss') disDate,t2.branch_name branch,to_char(t1.loan_amount) as loanAmount,s.ovrdescription || decode(t1.NCH_sendback,2,' RECAPTURED') as status,to_char(s.ovrstatusid) as statusId,'1' as redirectFlag,c.num_weekdays tat from TBL_OVERSITE_MASTER t1 CROSS APPLY ( SELECT COUNT (*) AS num_weekdays FROM dual WHERE TO_CHAR ( to_date(t1.disbursed_date,'dd-mm-yyyy') + LEVEL , 'DY' , 'NLS_DATE_LANGUAGE=ENGLISH' ) NOT IN ('SUN') AND TO_CHAR ( to_date(t1.disbursed_date,'dd-mm-yyyy') + LEVEL , 'DD-MM' , 'NLS_DATE_LANGUAGE=ENGLISH' ) NOT IN ('26-01','02-10','01-05','15-08') CONNECT BY LEVEL < sysdate - to_date(t1.disbursed_date,'dd-mm-yyyy') ) c left outer join branch_master t2 on t1.branch_id = t2.branch_id" +
                                " left outer join oversite_status_master s on s.ovrstatusid = t1.verification_status where  (t1.verification_status not in(6) and  t1.nch_sendback =1 or t1.nho_sendback=1) " + Qblock + "");
                                //Dashboardlist.Add(Dashboardpro);

                            }
                            else if (request.status == "7")
                            {
                                Dashboardlist = new OracleHelper().GetRecords<DashBoardProperties>("select to_char(t1.Loan_Id) LoanId,to_char(t1.application_id) ApplicationNo,t1.customer_name cusName,to_char(t1.disbursed_date,'dd-mm-yyyy') disDate,t2.branch_name branch,to_char(t1.loan_amount) as loanAmount,s.ovrdescription || decode(t1.NCH_sendback,2,' RECAPTURED') as status,to_char(s.ovrstatusid) as statusId,'1' as redirectFlag,c.num_weekdays tat from TBL_OVERSITE_MASTER t1 CROSS APPLY ( SELECT COUNT (*) AS num_weekdays FROM dual WHERE TO_CHAR ( to_date(t1.disbursed_date,'dd-mm-yyyy') + LEVEL , 'DY' , 'NLS_DATE_LANGUAGE=ENGLISH' ) NOT IN ('SUN') AND TO_CHAR ( to_date(t1.disbursed_date,'dd-mm-yyyy') + LEVEL , 'DD-MM' , 'NLS_DATE_LANGUAGE=ENGLISH' ) NOT IN ('26-01','02-10','01-05','15-08') CONNECT BY LEVEL < sysdate - to_date(t1.disbursed_date,'dd-mm-yyyy') ) c left outer join branch_master t2 on t1.branch_id = t2.branch_id" +
                                " left outer join oversite_status_master s on s.ovrstatusid = t1.verification_status where  (t1.verification_status  in(7)) " + Qblock + "");

                            }
                            else
                            {
                                //Dashboardlist = new OracleHelper().GetRecords<DashBoardProperties>(" select to_char(t1.Loan_Id) LoanId,to_char(t1.application_id) ApplicationNo,t1.customer_name cusName,to_char(t1.disbursed_date,'dd-mm-yyyy HH24:mi:ss') disDate,t2.branch_name branch,to_char(t1.loan_amount) as loanAmount,s.ovrdescription || decode(t1.NCH_sendback,2,' RECAPTURED') as status,to_char(s.ovrstatusid) as statusId,'1' as redirectFlag from TBL_OVERSITE_MASTER t1 left outer join branch_master t2 on t1.branch_id = t2.branch_id" +
                                //" left outer join oversite_status_master s on s.ovrstatusid = t1.verification_status where (nvl(t1.verification_status, 0) not in (6)) " + Qblock + "");
                                //Dashboardlist.Add(Dashboardpro);
                                Dashboardlist = new OracleHelper().GetRecords<DashBoardProperties>("select to_char(t1.Loan_Id) LoanId,to_char(t1.application_id) ApplicationNo,t1.customer_name cusName,to_char(t1.disbursed_date,'dd-mm-yyyy') disDate,t2.branch_name branch,to_char(t1.loan_amount) as loanAmount,s.ovrdescription || decode(t1.NCH_sendback,2,' RECAPTURED') as status,to_char(s.ovrstatusid) as statusId,'0' as redirectFlag,c.num_weekdays tat from TBL_OVERSITE_MASTER t1 CROSS APPLY ( SELECT COUNT (*) AS num_weekdays FROM dual WHERE TO_CHAR ( to_date(t1.disbursed_date,'dd-mm-yyyy') + LEVEL , 'DY' , 'NLS_DATE_LANGUAGE=ENGLISH' ) NOT IN ('SUN') AND TO_CHAR ( to_date(t1.disbursed_date,'dd-mm-yyyy') + LEVEL , 'DD-MM' , 'NLS_DATE_LANGUAGE=ENGLISH' ) NOT IN ('26-01','02-10','01-05','15-08') CONNECT BY LEVEL < sysdate - to_date(t1.disbursed_date,'dd-mm-yyyy') ) c left outer join branch_master t2 on t1.branch_id = t2.branch_id" +
                                " left outer join oversite_status_master s on s.ovrstatusid = t1.verification_status where (nvl(t1.verification_status,0) not in (6)) " + Qblock + "");

                            }
                        }

                        else
                        {
                            Dashboardlist = new OracleHelper().GetRecords<DashBoardProperties>("select to_char(t1.Loan_Id) LoanId,to_char(t1.application_id) ApplicationNo,t1.customer_name cusName,to_char(t1.disbursed_date,'dd-mm-yyyy') disDate,t2.branch_name branch,to_char(t1.loan_amount) as loanAmount, case nvl(t1.nch_sendback, 0) when 1 then case nvl(t1.nho_sendback, 0) when 1 then 'NHO & NCH sendback' else 'NCH Sendback' end else case nvl(t1.nho_sendback, 0) WHEN 1 then 'NHO sendback' end end status,/*s.ovrdescription || decode(t1.NCH_sendback,2,' RECAPTURED') as status,*/to_char(s.ovrstatusid) as statusId,'1' as redirectFlag,c.num_weekdays tat from TBL_OVERSITE_MASTER t1 CROSS APPLY ( SELECT COUNT (*) AS num_weekdays FROM dual WHERE TO_CHAR ( to_date(t1.disbursed_date,'dd-mm-yyyy') + LEVEL , 'DY' , 'NLS_DATE_LANGUAGE=ENGLISH' ) NOT IN ('SUN') AND TO_CHAR ( to_date(t1.disbursed_date,'dd-mm-yyyy') + LEVEL , 'DD-MM' , 'NLS_DATE_LANGUAGE=ENGLISH' ) NOT IN ('26-01','02-10','01-05','15-08') CONNECT BY LEVEL < sysdate - to_date(t1.disbursed_date,'dd-mm-yyyy') ) c left outer join branch_master t2 on t1.branch_id = t2.branch_id" +
                        " left outer join oversite_status_master s on s.ovrstatusid = t1.verification_status where (t1.verification_status not in (6) and  t1.nch_sendback =1 or t1.nho_sendback=1 or t1.verification_status=7) " + Qblock + "  ");
                            //Dashboardlist.Add(Dashboardpro);

                        }
                    }
                     if (/*role_rch_cnt > 0 ||*/ role_cvrch_cnt > 0 && role_cvroh_cnt ==0)//RCH
                    {
                        DashBoardProperties Dashboardpro = new DashBoardProperties();

                        if (!string.IsNullOrEmpty(request.status.ToString()))
                        {
                            if (request.status == "10")
                            {
                                Dashboardlist = new OracleHelper().GetRecords<DashBoardProperties>("select to_char(t1.Loan_Id) LoanId,to_char(t1.application_id) ApplicationNo,t1.customer_name cusName,to_char(t1.disbursed_date, 'dd-mm-yyyy') disDate,t2.branch_name branch,to_char(t1.loan_amount) as loanAmount,case nvl(t1.cr_verification_status, 0) when 2 then 'Verified By NCH' when 3 then case nvl(t1.nch_sendback, 0) when 2 then 'Recaptured' when 1 then 'Sendback by NCH' end when 8 then 'Verified By RCH' when 9 then 'Verified By BCM' when 10 then case nvl(t1.rch_sendback, 0) when 2 then 'Recaptured' when 1 then 'Sendback by RCH' end else 'Not Verified' end status,case t1.rch_sendback when 2 then case t1.nch_sendback when 1 then '3' else '9' end when 1 then to_char(s.ovrstatusid)else case t1.nch_sendback when 1 then '3' else to_char(s.ovrstatusid) end end  as statusId,case t1.rch_sendback when 1 then '0' else '1' end  as redirectFlag,'55' roleid,c.num_weekdays TAT from TBL_OVERSITE_MASTER t1 CROSS APPLY ( SELECT COUNT (*) AS num_weekdays FROM dual WHERE TO_CHAR ( to_date(t1.disbursed_date,'dd-mm-yyyy') + LEVEL , 'DY' , 'NLS_DATE_LANGUAGE=ENGLISH' ) NOT IN ('SUN') AND TO_CHAR ( to_date(t1.disbursed_date,'dd-mm-yyyy') + LEVEL , 'DD-MM' , 'NLS_DATE_LANGUAGE=ENGLISH' ) NOT IN ('26-01','02-10','01-05','15-08') CONNECT BY LEVEL < sysdate - to_date(t1.disbursed_date,'dd-mm-yyyy') ) c left outer join branch_master t2 on t1.branch_id = t2.branch_id left outer join oversite_status_master s on s.ovrstatusid = t1.cr_verification_status where (t1.verification_status not in (6) or   t1.application_id in(select distinct(tt.application_id) from TBL_OVERSITE_DOCUMENTS_MST tt where tt.nch_sendback_to='R') )" + Qblock + " order by c.num_weekdays desc");
                            }
                            //else if (request.status == "3")

                            //{
                            //    Dashboardlist = new OracleHelper().GetRecords<DashBoardProperties>("select to_char(t1.Loan_Id) LoanId,to_char(t1.application_id) ApplicationNo,t1.customer_name cusName,to_char(t1.disbursed_date, 'dd-mm-yyyy') disDate,t2.branch_name branch,to_char(t1.loan_amount) as loanAmount,case nvl(t1.cr_verification_status, 0) when 2 then 'Verified By NCH' when 3 then case nvl(t1.nch_sendback, 0) when 2 then 'Recaptured' when 1 then 'Sendback by NCH' end when 8 then 'Verified By RCH' when 9 then 'Verified By BCM' when 10 then case nvl(t1.rch_sendback, 0) when 2 then 'Recaptured' when 1 then 'Sendback by RCH' end else 'Not Verified' end status,case t1.rch_sendback when 2 then case t1.nch_sendback when 1 then '3' else '9' end when 1 then to_char(s.ovrstatusid)else case t1.nch_sendback when 1 then '3'  when 2 then '1' else to_char(s.ovrstatusid) end end  as statusId, CASE WHEN  t1.nch_sendback =1 THEN '0' ELSE '1' END as redirectFlag,'55' roleid,c.num_weekdays TAT  from TBL_OVERSITE_MASTER t1 CROSS APPLY ( SELECT COUNT (*) AS num_weekdays FROM dual WHERE TO_CHAR ( to_date(t1.disbursed_date,'dd-mm-yyyy') + LEVEL , 'DY' , 'NLS_DATE_LANGUAGE=ENGLISH' ) NOT IN ('SUN') AND TO_CHAR ( to_date(t1.disbursed_date,'dd-mm-yyyy') + LEVEL , 'DD-MM' , 'NLS_DATE_LANGUAGE=ENGLISH' ) NOT IN ('26-01','02-10','01-05','15-08') CONNECT BY LEVEL < sysdate - to_date(t1.disbursed_date,'dd-mm-yyyy') ) c left outer join branch_master t2 on t1.branch_id = t2.branch_id left outer join oversite_status_master s on s.ovrstatusid = t1.cr_verification_status  where ((t1.verification_status not in (6)) and(t1.nho_sendback = 1 or t1.cr_verification_status is not null or t1.application_id in(select distinct(tt.application_id) from TBL_OVERSITE_DOCUMENTS_MST tt where tt.nch_sendback_to='R'))) " + Qblock + " order by c.num_weekdays desc ");

                            //}
                            else
                            {
                                Dashboardlist = new OracleHelper().GetRecords<DashBoardProperties>("select to_char(t1.Loan_Id) LoanId,to_char(t1.application_id) ApplicationNo,t1.customer_name cusName,to_char(t1.disbursed_date, 'dd-mm-yyyy') disDate,t2.branch_name branch,to_char(t1.loan_amount) as loanAmount,case nvl(t1.cr_verification_status, 0) when 2 then 'Verified By NCH' when 3 then case nvl(t1.nch_sendback, 0) when 2 then 'Recaptured' when 1 then 'Sendback by NCH' end when 8 then 'Verified By RCH' when 9 then 'Verified By BCM' when 10 then case nvl(t1.rch_sendback, 0) when 2 then 'Recaptured' when 1 then 'Sendback by RCH' end else 'Not Verified' end status,case t1.rch_sendback when 2 then case t1.nch_sendback when 1 then '3' else '9' end when 1 then to_char(s.ovrstatusid)else case t1.nch_sendback when 1 then '3' else to_char(s.ovrstatusid) end end  as statusId,'1' as redirectFlag,'55' roleid,c.num_weekdays TAT  from TBL_OVERSITE_MASTER t1 CROSS APPLY ( SELECT COUNT (*) AS num_weekdays FROM dual WHERE TO_CHAR ( to_date(t1.disbursed_date,'dd-mm-yyyy') + LEVEL , 'DY' , 'NLS_DATE_LANGUAGE=ENGLISH' ) NOT IN ('SUN') AND TO_CHAR ( to_date(t1.disbursed_date,'dd-mm-yyyy') + LEVEL , 'DD-MM' , 'NLS_DATE_LANGUAGE=ENGLISH' ) NOT IN ('26-01','02-10','01-05','15-08') CONNECT BY LEVEL < sysdate - to_date(t1.disbursed_date,'dd-mm-yyyy') ) c left outer join branch_master t2 on t1.branch_id = t2.branch_id left outer join oversite_status_master s on s.ovrstatusid = t1.cr_verification_status  where ((t1.verification_status not in (6)) and(t1.nho_sendback = 1 or t1.cr_verification_status is not null or t1.application_id in(select distinct(tt.application_id) from TBL_OVERSITE_DOCUMENTS_MST tt where tt.nch_sendback_to='R'))) " + Qblock + " order by c.num_weekdays desc ");

                            }
                        }

                        else
                        {
                            Dashboardlist = new OracleHelper().GetRecords<DashBoardProperties>("select to_char(t1.Loan_Id) LoanId,to_char(t1.application_id) ApplicationNo,t1.customer_name cusName,to_char(t1.disbursed_date, 'dd-mm-yyyy') disDate,t2.branch_name branch,to_char(t1.loan_amount) as loanAmount,case nvl(t1.cr_verification_status, 0) when 2 then 'Verified By NCH' when 3 then case nvl(t1.nch_sendback, 0) when 2 then 'Recaptured' when 1 then 'Sendback by NCH' end when 8 then 'Verified By RCH' when 9 then 'Verified By BCM' when 10 then case nvl(t1.rch_sendback, 0) when 2 then 'Recaptured' when 1 then 'Sendback by RCH' end else 'Not Verified' end status,case t1.rch_sendback when 2 then case t1.nch_sendback when 1 then '3' else '9' end when 1 then to_char(s.ovrstatusid)else case t1.nch_sendback when 1 then '3' else to_char(s.ovrstatusid) end end  as statusId,case t1.rch_sendback when 1 then '0' else '1' end as redirectFlag,'55' roleid,c.num_weekdays TAT  from TBL_OVERSITE_MASTER t1 CROSS APPLY ( SELECT COUNT (*) AS num_weekdays FROM dual WHERE TO_CHAR ( to_date(t1.disbursed_date,'dd-mm-yyyy') + LEVEL , 'DY' , 'NLS_DATE_LANGUAGE=ENGLISH' ) NOT IN ('SUN') AND TO_CHAR ( to_date(t1.disbursed_date,'dd-mm-yyyy') + LEVEL , 'DD-MM' , 'NLS_DATE_LANGUAGE=ENGLISH' ) NOT IN ('26-01','02-10','01-05','15-08') CONNECT BY LEVEL < sysdate - to_date(t1.disbursed_date,'dd-mm-yyyy') ) c left outer join branch_master t2 on t1.branch_id = t2.branch_id left outer join oversite_status_master s on s.ovrstatusid = t1.cr_verification_status where ((t1.verification_status not in (6)) and( t1.nho_sendback = 1 or t1.rch_sendback=2 or t1.cr_verification_status not in(3,8) )) " + Qblock + "" +
                                "union select to_char(t1.Loan_Id) LoanId, to_char(t1.application_id) ApplicationNo, t1.customer_name cusName, to_char(t1.disbursed_date, 'dd-mm-yyyy') disDate, t2.branch_name branch, to_char(t1.loan_amount) as loanAmount, case nvl(t1.cr_verification_status, 0) when 2 then 'Verified By NCH' when 3 then case nvl(t1.nch_sendback, 0) when 2 then 'Recaptured' when 1 then 'Sendback by NCH' end when 8 then 'Verified By RCH' when 9 then 'Verified By BCM' when 10 then case nvl(t1.rch_sendback, 0) when 2 then 'Recaptured' when 1 then 'Sendback by RCH' end else 'Not Verified' end status, case t1.rch_sendback when 2 then case t1.nch_sendback when 1 then '3' else '9' end when 1 then to_char(s.ovrstatusid) else case t1.nch_sendback when 1 then '3' when 2 then '1' else to_char(s.ovrstatusid) end end as statusId, case t1.rch_sendback when 1 then '0' else '1' end as redirectFlag, '55' roleid, c.num_weekdays TAT from TBL_OVERSITE_MASTER t1 CROSS APPLY ( SELECT COUNT (*) AS num_weekdays FROM dual WHERE TO_CHAR ( to_date(t1.disbursed_date,'dd-mm-yyyy') + LEVEL , 'DY' , 'NLS_DATE_LANGUAGE=ENGLISH' ) NOT IN ('SUN') AND TO_CHAR ( to_date(t1.disbursed_date,'dd-mm-yyyy') + LEVEL , 'DD-MM' , 'NLS_DATE_LANGUAGE=ENGLISH' ) NOT IN ('26-01','02-10','01-05','15-08') CONNECT BY LEVEL < sysdate - to_date(t1.disbursed_date,'dd-mm-yyyy') ) c left outer join branch_master t2 on t1.branch_id = t2.branch_id left outer join oversite_status_master s on s.ovrstatusid = t1.cr_verification_status where t1.verification_status not in (6) and t1.nch_sendback is not null and t1.rch_verify is null /*( t1.nch_sendback IS NULL OR t1.nch_sendback =2) */and t1.rch_verify is null and t1.application_id in(select distinct(tt.application_id) from TBL_OVERSITE_DOCUMENTS_MST tt where tt.nch_sendback_to='R' " + Qblock + ") " +
                                "order by tat desc");

                        }
                    }
                    else if (role_cvroh_cnt > 0 && role_cvrch_cnt ==0)//ROH
                    {
                        DashBoardProperties Dashboardpro = new DashBoardProperties();

                        if (!string.IsNullOrEmpty(request.status.ToString()))
                        {
                            if (request.status == "13")
                            {
                                Dashboardlist = new OracleHelper().GetRecords<DashBoardProperties>("select to_char(t1.Loan_Id) LoanId, to_char(t1.application_id) ApplicationNo, t1.customer_name cusName, to_char(t1.disbursed_date, 'dd-mm-yyyy') disDate, t2.branch_name branch, to_char(t1.loan_amount) as loanAmount,  case nvl(t1.verification_status, 0) when 4 then 'Verified By NOH' when 5 then case nvl(t1.nho_Sendback, 0) when 2 then 'Recaptured' when 1 then 'Sendback by NOH' end when 11 then 'Verified By ROH' when 13 then 'Verified By BCO' when 10 then case nvl(ops.roh_sendback, 0) when 2 then 'Recaptured' when 1 then 'Sendback by ROH' end else 'Not Verified' end status, to_char(s.ovrstatusid) as statusId, case ops.roh_sendback when 2 then case t1.nho_sendback when 1 then '3' else '9' end when 1 then to_char(s.ovrstatusid) else case t1.nho_sendback when 1 then '3' else to_char(s.ovrstatusid) end end as statusId, '0' as redirectFlag, case ops.roh_sendback when 1 then '0' else '1' end as redirectFlag, '52' roleid, c.num_weekdays tat from TBL_OVERSITE_MASTER t1 CROSS APPLY (SELECT COUNT(*) AS num_weekdays FROM dual WHERE TO_CHAR(to_date(t1.disbursed_date, 'dd-mm-yyyy') + LEVEL, 'DY', 'NLS_DATE_LANGUAGE=ENGLISH') NOT IN ('SUN') AND TO_CHAR(to_date(t1.disbursed_date, 'dd-mm-yyyy') + LEVEL, 'DD-MM', 'NLS_DATE_LANGUAGE=ENGLISH') NOT IN ('26-01', '02-10', '01-05', '15-08') CONNECT BY LEVEL < sysdate - to_date(t1.disbursed_date, 'dd-mm-yyyy')) c " +
                                    "left outer join TBL_OPS_OVERSITE_MASTER ops on t1.application_id=ops.application_id left outer join branch_master t2 on t1.branch_id = t2.branch_id left outer join oversite_status_master s on s.ovrstatusid = t1.verification_status where (nvl(t1.verification_status, 0) not in (6)) " + Qblock + " order by TAT desc");
                                //Dashboardlist.Add(Dashboardpro);

                            }

                            else
                            {

                                Dashboardlist = new OracleHelper().GetRecords<DashBoardProperties>("select to_char(t1.Loan_Id) LoanId, to_char(t1.application_id) ApplicationNo, t1.customer_name cusName, to_char(t1.disbursed_date, 'dd-mm-yyyy') disDate, t2.branch_name branch, to_char(t1.loan_amount) as loanAmount, /*--s.ovrdescription || decode(t1.NCH_sendback, 2, ' RECAPTURED') as status,*/ case nvl(t1.verification_status, 0) when 4 then 'Verified By NOH' when 5 then case nvl(t1.nho_Sendback, 0) when 2 then 'Recaptured' when 1 then 'Sendback by NOH' end when 11 then 'Verified By ROH' when 13 then 'Verified By BCO' when 10 then case nvl(ops.roh_sendback, 0) when 2 then 'Recaptured' when 1 then 'Sendback by ROH' end else 'Not Verified' end status, /*--to_char(s.ovrstatusid) as statusId*/ case ops.roh_sendback when 2 then case t1.nho_sendback when 1 then '3' else '9' end when 1 then to_char(s.ovrstatusid) else case t1.nho_sendback when 1 then '3' else to_char(s.ovrstatusid) end end as statusId, '1' as redirectFlag, '52' roleid, c.num_weekdays tat from TBL_OVERSITE_MASTER t1 CROSS APPLY (SELECT COUNT(*) AS num_weekdays FROM dual WHERE TO_CHAR(to_date(t1.disbursed_date, 'dd-mm-yyyy') + LEVEL, 'DY', 'NLS_DATE_LANGUAGE=ENGLISH') NOT IN ('SUN') AND TO_CHAR(to_date(t1.disbursed_date, 'dd-mm-yyyy') + LEVEL, 'DD-MM', 'NLS_DATE_LANGUAGE=ENGLISH') NOT IN ('26-01', '02-10', '01-05', '15-08') CONNECT BY LEVEL < sysdate - to_date(t1.disbursed_date, 'dd-mm-yyyy')) c " +
                                    "left outer join TBL_OPS_OVERSITE_MASTER ops on t1.application_id=ops.application_id left outer join branch_master t2 on t1.branch_id = t2.branch_id left outer join oversite_status_master s on s.ovrstatusid = t1.verification_status where (t1.verification_status not in (6) and (t1.cr_verification_status is not null or t1.nho_sendback = 1)) " + Qblock + " order by TAT desc");

                            }
                        }

                        else
                        {

                            Dashboardlist = new OracleHelper().GetRecords<DashBoardProperties>("select to_char(t1.Loan_Id) LoanId, to_char(t1.application_id) ApplicationNo, t1.customer_name cusName, to_char(t1.disbursed_date, 'dd-mm-yyyy') disDate, t2.branch_name branch, to_char(t1.loan_amount) as loanAmount, case nvl(t1.verification_status, 0) when 4 then 'Verified By National Ops' when 5 then case nvl(t1.nho_sendback, 0) when 2 then 'Recaptured' when 1 then 'Sendback by National Ops' end when 11 then 'Verified By Regianol Ops' when 13 then 'Verified By Branch Ops' when 12 then case nvl(ops.roh_sendback, 0) when 2 then 'Recaptured' when 1 then 'Sendback by Regianol Ops' end else case nvl(ops.roh_sendback, 0) when 2 then 'Recaptured' when 1 then 'Sendback by Regianol Ops' else 'Not Verified' end end status, case ops.roh_sendback when 2 then case t1.nho_sendback when 1 then '5' else '13' end when 1 then to_char(s.ovrstatusid) else case t1.nho_sendback when 1 then '5' else to_char(s.ovrstatusid) end end as statusId, case ops.roh_sendback when 1 then '0' else '1' end as redirectFlag, '52' roleid, c.num_weekdays tat from TBL_OVERSITE_MASTER t1 CROSS APPLY (SELECT COUNT(*) AS num_weekdays FROM dual WHERE TO_CHAR(to_date(t1.disbursed_date, 'dd-mm-yyyy') + LEVEL, 'DY', 'NLS_DATE_LANGUAGE=ENGLISH') NOT IN ('SUN') AND TO_CHAR(to_date(t1.disbursed_date, 'dd-mm-yyyy') + LEVEL, 'DD-MM', 'NLS_DATE_LANGUAGE=ENGLISH') NOT IN ('26-01', '02-10', '01-05', '15-08') CONNECT BY LEVEL < sysdate - to_date(t1.disbursed_date, 'dd-mm-yyyy')) c left outer join TBL_OPS_OVERSITE_MASTER ops on t1.application_id=ops.application_id left outer join branch_master t2 on t1.branch_id = t2.branch_id left outer join oversite_status_master s on s.ovrstatusid = t1.verification_status where (t1.verification_status not in (6) and t1.nho_sendback = 1) " + Qblock + " " +
                                   "union select to_char(t1.Loan_Id) LoanId, to_char(t1.application_id) ApplicationNo, t1.customer_name cusName, to_char(t1.disbursed_date, 'dd-mm-yyyy') disDate, t2.branch_name branch, to_char(t1.loan_amount) as loanAmount, case nvl(t1.verification_status, 0) when 4 then 'Verified By National Ops' when 5 then case nvl(t1.nho_sendback, 0) when 2 then 'Recaptured' when 1 then 'Sendback by National Ops' end when 11 then 'Verified By Regianol Ops' when 13 then 'Verified By Branch Ops' when 12 then case nvl(ops.roh_sendback, 0) when 2 then 'Recaptured' when 1 then 'Sendback by Regianol Ops' end else case nvl(ops.roh_sendback, 0) when 2 then 'Recaptured' when 1 then 'Sendback by Regianol Ops' else 'Not Verified' end end status, case ops.roh_sendback when 2 then case t1.nho_sendback when 1 then '5' else '13' end when 1 then to_char(s.ovrstatusid) else case t1.nho_sendback when 1 then '5' when 2 then '1' else to_char(s.ovrstatusid) end end as statusId,'1'  as redirectFlag, '52' roleid, c.num_weekdays tat from TBL_OVERSITE_MASTER t1 CROSS APPLY (SELECT COUNT(*) AS num_weekdays FROM dual WHERE TO_CHAR(to_date(t1.disbursed_date, 'dd-mm-yyyy') + LEVEL, 'DY', 'NLS_DATE_LANGUAGE=ENGLISH') NOT IN ('SUN') AND TO_CHAR(to_date(t1.disbursed_date, 'dd-mm-yyyy') + LEVEL, 'DD-MM', 'NLS_DATE_LANGUAGE=ENGLISH') NOT IN ('26-01', '02-10', '01-05', '15-08') CONNECT BY LEVEL < sysdate - to_date(t1.disbursed_date, 'dd-mm-yyyy')) c left outer join TBL_OPS_OVERSITE_MASTER ops on t1.application_id=ops.application_id left outer join branch_master t2 on t1.branch_id = t2.branch_id left outer join oversite_status_master s on s.ovrstatusid = t1.verification_status where (t1.verification_status not in (6) and (t1.nho_sendback is not null or ops.roh_verify is null or  ops.roh_sendback=2)) " + Qblock + " order by TAT desc");


                        }
                    }
                    if (role_ro_cnt > 0)//RO
                    {
                        DashBoardProperties Dashboardpro = new DashBoardProperties();

                        if (!string.IsNullOrEmpty(request.status.ToString()))
                        {
                            if (request.status == "7")
                            {
                                Dashboardlist = new OracleHelper().GetRecords<DashBoardProperties>("select t1.Loan_Id LoanId, t1.application_id ApplicationNo, t1.customer_name CustomerName, t1.disbursed_date DisbursementDate, t2.branch_name Location, t1.loan_amount Amount,'1' as redirectFlag,c.num_weekdays tat from TBL_OVERSITE_MASTER t1 CROSS APPLY ( SELECT COUNT (*) AS num_weekdays FROM dual WHERE TO_CHAR ( to_date(t1.disbursed_date,'dd-mm-yyyy') + LEVEL , 'DY' , 'NLS_DATE_LANGUAGE=ENGLISH' ) NOT IN ('SUN') AND TO_CHAR ( to_date(t1.disbursed_date,'dd-mm-yyyy') + LEVEL , 'DD-MM' , 'NLS_DATE_LANGUAGE=ENGLISH' ) NOT IN ('26-01','02-10','01-05','15-08') CONNECT BY LEVEL < sysdate - to_date(t1.disbursed_date,'dd-mm-yyyy') ) c left outer join branch_master t2 on t1.branch_id = t2.branch_id " +
                                " left outer join oversite_status_master s on s.ovrstatusid = t1.verification_status where  (t1.verification_status not in(6))" + Qblock + "");
                                //Dashboardlist.Add(Dashboardpro);

                            }
                            else
                            {
                                Dashboardlist = new OracleHelper().GetRecords<DashBoardProperties>("select t1.Loan_Id LoanId, t1.application_id ApplicationNo, t1.customer_name CustomerName, t1.disbursed_date DisbursementDate, t2.branch_name Location, t1.loan_amount Amount,'0' as redirectFlag,c.num_weekdays tat from TBL_OVERSITE_MASTER t1 CROSS APPLY ( SELECT COUNT (*) AS num_weekdays FROM dual WHERE TO_CHAR ( to_date(t1.disbursed_date,'dd-mm-yyyy') + LEVEL , 'DY' , 'NLS_DATE_LANGUAGE=ENGLISH' ) NOT IN ('SUN') AND TO_CHAR ( to_date(t1.disbursed_date,'dd-mm-yyyy') + LEVEL , 'DD-MM' , 'NLS_DATE_LANGUAGE=ENGLISH' ) NOT IN ('26-01','02-10','01-05','15-08') CONNECT BY LEVEL < sysdate - to_date(t1.disbursed_date,'dd-mm-yyyy') ) c left outer join branch_master t2 on t1.branch_id = t2.branch_id " +
                                     " left outer join oversite_status_master s on s.ovrstatusid = t1.verification_status where  (t1.verification_status not in(6)) " + Qblock + "");
                                //Dashboardlist.Add(Dashboardpro);
                            }
                        }
                        else
                        {
                            Dashboardlist = new OracleHelper().GetRecords<DashBoardProperties>("select to_char(t1.Loan_Id) LoanId,to_char(t1.application_id) ApplicationNo,t1.customer_name cusName,to_char(t1.disbursed_date,'dd-mm-yyyy HH24:mi:ss') disDate,t2.branch_name branch,to_char(t1.loan_amount) as loanAmount,s.ovrdescription || decode(t1.NCH_sendback,2,' RECAPTURED') as status,to_char(s.ovrstatusid) as statusId,'1' as redirectFlag,c.num_weekdays tat from TBL_OVERSITE_MASTER t1 CROSS APPLY ( SELECT COUNT (*) AS num_weekdays FROM dual WHERE TO_CHAR ( to_date(t1.disbursed_date,'dd-mm-yyyy') + LEVEL , 'DY' , 'NLS_DATE_LANGUAGE=ENGLISH' ) NOT IN ('SUN') AND TO_CHAR ( to_date(t1.disbursed_date,'dd-mm-yyyy') + LEVEL , 'DD-MM' , 'NLS_DATE_LANGUAGE=ENGLISH' ) NOT IN ('26-01','02-10','01-05','15-08') CONNECT BY LEVEL < sysdate - to_date(t1.disbursed_date,'dd-mm-yyyy') ) c left outer join branch_master t2 on t1.branch_id = t2.branch_id" +
                        " left outer join oversite_status_master s on s.ovrstatusid = t1.verification_status where (t1.verification_status=7) " + Qblock + "  ");
                            //Dashboardlist.Add(Dashboardpro);


                        }

                    }

                    if (role_report > 0)
                    {

                            Dashboardlist = new OracleHelper().GetRecords<DashBoardProperties>("select to_char(t1.Loan_Id) LoanId, to_char(t1.application_id) ApplicationNo, t1.customer_name cusName, to_char(t1.disbursed_date, 'dd-mm-yyyy') disDate, t2.branch_name branch, to_char(t1.loan_amount) as loanAmount, case nvl(t1.verification_status, 0) when 4 then 'Verify By NHO' when 5 then case nvl(t1.Nho_Sendback, 0) when 2 then 'Recaptured' when 1 then 'Sendback by NHO' end when 11 then 'Verify By RHO' when 13 then 'Verify By BHO' when 12 then case nvl(o.roh_sendback, 0) when 2 then 'Recaptured' when 1 then 'Sendback by RHO' end else 'Not Verified' end status,'1' as statusId,'2' as redirectFlag,c.num_weekdays tat from TBL_OVERSITE_MASTER t1 CROSS APPLY ( SELECT COUNT (*) AS num_weekdays FROM dual WHERE TO_CHAR ( to_date(t1.disbursed_date,'dd-mm-yyyy') + LEVEL , 'DY' , 'NLS_DATE_LANGUAGE=ENGLISH' ) NOT IN ('SUN') AND TO_CHAR ( to_date(t1.disbursed_date,'dd-mm-yyyy') + LEVEL , 'DD-MM' , 'NLS_DATE_LANGUAGE=ENGLISH' ) NOT IN ('26-01','02-10','01-05','15-08') CONNECT BY LEVEL < sysdate - to_date(t1.disbursed_date,'dd-mm-yyyy') ) c    left outer join tbl_ops_oversite_master ops on t1.application_id=ops.application_id and t1.loan_id=ops.loan_id left outer join branch_master t2 on t1.branch_id = t2.branch_id " +
                            " left outer join oversite_status_master s on s.ovrstatusid = t1.verification_status where  (t1.verification_status !=1 and t1.cr_verification_status !=1)" + Qblock + "");
                    }

                    dashBoardResponse.DataList = Dashboardlist;
                }






                //query = "select t1.Loan_Id LoanId,t1.application_id ApplicationNo,t1.customer_name CustomerName,t1.disbursed_date DisbursementDate,t2.branch_name Location,t1.loan_amount Amount from TBL_OVERSITE_MASTER t1 inner join branch_master t2 on t1.branch_id=t2.branch_id where" +
                //    " t1.region_id='" + request.regionId + "' and t1.product_id='" + request.productId + "' and to_char(disbursed_date, 'mm')='" + request.MonthId + "' and (t1.application_id='" + request.searchValue + "' or t1.Loan_Id='" + request.searchValue + "'";
                if (dashBoardResponse.DataList.Count > 0)
                {
                    response.Data = dashBoardResponse;
                    response.responseMsg = "Success";
                    response.apiStatus = ResponseTypeContants.SUCCESS;
                    response.status = ApiStatusConstants.COMPLETED;
                }
                else
                {
                    response.responseMsg = "No data Found!";
                    response.Data = null;
                    response.apiStatus = ResponseTypeContants.FAIL;
                    response.status = ApiStatusConstants.NOT_COMPLETED;

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
        public Response<MonthListResponse> FillMonth(MonthListRequest MonRequest)
        {
            MonthListResponse monthListResponse = new MonthListResponse();
            Response<MonthListResponse> response = new Response<MonthListResponse>();
            List<MonthListProperties> Month = new List<MonthListProperties>();
            string output = JsonConvert.SerializeObject(MonRequest);
            string query = string.Empty;

            try
            {
                if (MonRequest.Flag == 1)
                {
                    query = "select * from TBL_OVERSITE_MONTH ";
                    Month = new OracleHelper().GetRecords<MonthListProperties>(query);
                    monthListResponse.MonthList = Month;
                    response.Data = monthListResponse;
                    response.responseMsg = "Success";
                    response.apiStatus = ResponseTypeContants.SUCCESS;
                    response.status = ApiStatusConstants.COMPLETED;
                    
                }

            }
            catch (Exception ex)
            {
                ErrorLogRequest errorLogRequest = new Oversite.DTO.ErrorLog.Request.ErrorLogRequest();
                errorLogRequest.Function_Name = "Documents Update-Get";
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
        public Response<RegionListResponse> FillRegion(RegionListRequest RegRequest)
        {
            RegionListResponse regionListResponse = new RegionListResponse();
            Response<RegionListResponse> response = new Response<RegionListResponse>();
            List<RegionListProperties> Region = new List<RegionListProperties>();
            string query = string.Empty;
            string output = JsonConvert.SerializeObject(RegRequest);

            try
            {
                if (RegRequest.Flag == 1)
                {
                    query = "select distinct(r.region_id) region_id,r.region_name from cv_los.user_type_link u inner join cv_los.branch_master b on u.link_value=b.branch_id " +
                        "inner join cv_los.region_master r on b.region_id=r.region_id where u.user_id='"+RegRequest.empcode+"'";
                    //query = "select region_id,region_name from cv_los.region_master";
                    Region = new OracleHelper().GetRecords<RegionListProperties>(query);
                    regionListResponse.RegionList = Region;
                    response.Data = regionListResponse;
                    response.responseMsg = "Success";
                    response.apiStatus = ResponseTypeContants.SUCCESS;
                    response.status = ApiStatusConstants.COMPLETED;
                }

            }
            catch (Exception ex)
            {
                ErrorLogRequest errorLogRequest = new Oversite.DTO.ErrorLog.Request.ErrorLogRequest();
                errorLogRequest.Function_Name = "Documents Update-Get";
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
        public Response<ProductListResponse> FillProduct(ProductListRequest ProdRequest)
        {
            ProductListResponse productListResponse = new ProductListResponse();
            Response<ProductListResponse> response = new Response<ProductListResponse>();
            List<RegionListProperties> Region = new List<RegionListProperties>();
            string query = string.Empty;
            string query1 = string.Empty;

            string output = JsonConvert.SerializeObject(ProdRequest);
            List<ProductListProperties> Product = new List<ProductListProperties>();

            try
            {
                if (ProdRequest.Flag == 1)
                {
                    decimal rolecnt = new OracleHelper().ExecuteScalar<decimal>("select count(*) from user_master WHERE emp_code = '" + ProdRequest.enterBy + "' ");
                    ProductListProperties prop1 = new ProductListProperties();
                    decimal rolecnt1 = new OracleHelper().ExecuteScalar<decimal>("select count(*) from VW_CV_USER_MASTER WHERE emp_code = '" + ProdRequest.enterBy + "' ");

                    if (rolecnt > 0)
                    {
                        //query = "select xx.product_id,xx.product_name,case when xx.product_id=43 then 1 /*when xx.product_id=13 then 2*/ else 3 end proid from (select p.product_id, p.product_name from product_master p where p.product_id in (select u.product_id from user_master u WHERE u.emp_code =  '" + ProdRequest.enterBy + "') union select p.product_id, p.product_name from product_master p where p.product_id in (select u.product_id from vw_cv_user_master u WHERE u.emp_code = '" + ProdRequest.enterBy + "'))xx order by proid asc";
                        query = "select xx.product_id,xx.product_name,case when xx.product_id=43 then 1 /*when xx.product_id=13 then 2*/ else 3 end proid from (select p.product_id, p.product_name from product_master p where p.product_id in (select u.product_id from user_master u WHERE u.emp_code =  '" + ProdRequest.enterBy + "'))xx order by proid asc";

                        Product = new OracleHelper().GetRecords<ProductListProperties>(query);
                        //Product.Add(prop1);

                    }
                    if(rolecnt1 > 0)
                    {
                        query = "select xx.product_id,xx.product_name,case when xx.product_id=13 then 1 /*when xx.product_id=13 then 2*/ else 3 end proid from (select p.product_id, p.product_name from product_master p where p.product_id in (select u.product_id from vw_cv_user_master u WHERE u.emp_code = '" + ProdRequest.enterBy + "'))xx order by proid asc ";
                        Product = new OracleHelper().GetRecords<ProductListProperties>(query);

                    }
                    //if (rolecnt1 > 0)
                    //{
                    //    query1 = "select p.product_id,p.product_name from product_master p where p.product_id in(select u.product_id from vw_cv_user_master u WHERE u.emp_code = '" + ProdRequest.enterBy + "'   )";
                    //    prop1 = new OracleHelper().GetRecord<ProductListProperties>(query1);
                    //    //Product.Add(prop1);

                    //}
                    productListResponse.ProductList = Product;
                    response.Data = productListResponse;
                    response.responseMsg = "Success";
                    response.apiStatus = ResponseTypeContants.SUCCESS;
                    response.status = ApiStatusConstants.COMPLETED;


                }

            }
            catch (Exception ex)
            {
                ErrorLogRequest errorLogRequest = new Oversite.DTO.ErrorLog.Request.ErrorLogRequest();
                errorLogRequest.Function_Name = "Documents Update-Get";
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
        public Response<StatusListResponse> FillStatus(StatusListRequest strequest)
        {
            StatusListResponse statusListResponse = new StatusListResponse();
            Response<StatusListResponse> response = new Response<StatusListResponse>();
            string query = string.Empty;
            string output = JsonConvert.SerializeObject(strequest);
            List<StatusListProperties> status = new List<StatusListProperties>();
            try
            {
                if (strequest.Flag == 1)
                {
                    decimal role_ro_cnt = new OracleHelper().ExecuteScalar<decimal>("select count(*) from vw_cv_User_Role_Details t1 left outer join vw_cv_user_master t2 ON  t1.user_id = t2.user_id  WHERE t2.emp_code = '" + strequest.enterBy + "' and t1.Role_id=7");
                    if (role_ro_cnt > 0)
                    {
                        query = "select OvrStatusID as statusID, OvrDescription as description from Oversite_Status_Master where CV_Is_Default = 1 and OVRSTATUSID=7";
                        status = new OracleHelper().GetRecords<StatusListProperties>(query);

                    }
                    else
                    {
                        query = "select OvrStatusID as statusID, OvrDescription as description from Oversite_Status_Master where CV_Is_Default = 1";
                        status = new OracleHelper().GetRecords<StatusListProperties>(query);


                    }


                }
                statusListResponse.StatusList = status;
                response.Data = statusListResponse;
                response.responseMsg = "Success";
                response.apiStatus = ResponseTypeContants.SUCCESS;
                response.status = ApiStatusConstants.COMPLETED;
            }
            catch (Exception ex)
            {
                ErrorLogRequest errorLogRequest = new Oversite.DTO.ErrorLog.Request.ErrorLogRequest();
                errorLogRequest.Function_Name = "Dashboardstatusfill-Get";
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

        public Response<BranchListResponse> FillBranch(BranchListRequest RegRequest)
        {
            BranchListResponse branchListResponse = new BranchListResponse();
            Response<BranchListResponse> response = new Response<BranchListResponse>();
            List<BranchListProperties> Region = new List<BranchListProperties>();
            string query = string.Empty;
            string output = JsonConvert.SerializeObject(RegRequest);

            try
            {
                if (RegRequest.Flag == 1)
                {
                    query = "select t.branch_id,t.branch_name,t.Region_Id region_id from CV_LOS.Branch_Master t where t.status_id=1 and t.Region_Id='"+RegRequest.regionid+"'";
                    Region = new OracleHelper().GetRecords<BranchListProperties>(query);
                    if(Region.Count>0)
                    {
                        branchListResponse.BranchList = Region;
                        response.Data = branchListResponse;
                        response.responseMsg = "Success";
                        response.apiStatus = ResponseTypeContants.SUCCESS;
                        response.status = ApiStatusConstants.COMPLETED;
                    }
                    else
                    {
                        response.responseMsg = "No Data to show";
                        response.apiStatus = ResponseTypeContants.SUCCESS;
                        response.status = ApiStatusConstants.COMPLETED;
                    }


                }

            }
            catch (Exception ex)
            {
                ErrorLogRequest errorLogRequest = new Oversite.DTO.ErrorLog.Request.ErrorLogRequest();
                errorLogRequest.Function_Name = "DashBoard Fill Branch-Update";
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
    }
}

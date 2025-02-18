using Dapper;
using Newtonsoft.Json;
using Oversite.DTO.CommonMaster.Request;
using Oversite.DTO.Documents.Properties;
using Oversite.DTO.Documents.Response;
using Oversite.DTO.ErrorLog.Request;
using Oversite.DTO.Response;
using Oversite.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oversite.Core.DataSource.Documents
{
    public class GetDocumentsDataSource
    {
        public GetDocumentResponse GetDocument(GetDocumentRequest request)
        {
            GetDocumentResponse response = new GetDocumentResponse();
            string query = string.Empty;
            string query1 = string.Empty;
            //string query2 = string.Empty;

            string output = JsonConvert.SerializeObject(request);
            try
            {
                decimal cvrolenho = new OracleHelper().ExecuteScalar<decimal>("select count(*) from vw_cv_user_role_details  t1 left outer join vw_cv_user_master t2 ON  t1.user_id = t2.user_id  WHERE t2.emp_code = '" + request.enterBy + "' and t1.role_id=81"); //oversite nho
                decimal cvrolench = new OracleHelper().ExecuteScalar<decimal>("select count(*) from vw_cv_user_role_details  t1 left outer join vw_cv_user_master t2 ON  t1.user_id = t2.user_id WHERE t2.emp_code = '" + request.enterBy + "' and t1.role_id=77"); //oversite nch


                //Credit team new flow 100890
                decimal cvrolebcm = new OracleHelper().ExecuteScalar<decimal>("select count(*) from vw_cv_user_role_details  t1 left outer join vw_cv_user_master t2 ON  t1.user_id = t2.user_id WHERE t2.emp_code = '" + request.enterBy + "' and t1.role_id=79");//oversite bch//bcm role count
                decimal cvrolerch = new OracleHelper().ExecuteScalar<decimal>("select count(*) from vw_cv_user_role_details  t1 left outer join vw_cv_user_master t2 ON  t1.user_id = t2.user_id WHERE t2.emp_code = '" + request.enterBy + "' and t1.role_id=78");//oversite rch//rch role count

                decimal cvroleroh = new OracleHelper().ExecuteScalar<decimal>("select count(*) from vw_cv_user_role_details  t1 left outer join vw_cv_user_master t2 ON  t1.user_id = t2.user_id WHERE t2.emp_code = '" + request.enterBy + "' and t1.role_id=80");//oversite roh//roh role count
                decimal role_cvnho_ass_cnt = new OracleHelper().ExecuteScalar<decimal>("select count(*) from vw_cv_user_role_details  t1 left outer join vw_cv_user_master t2 ON  t1.user_id = t2.user_id  WHERE t2.emp_code = '" + request.enterBy + "' and t1.role_id=84"); //oversite nho

                decimal app = new OracleHelper().ExecuteScalar<decimal>("select count(*) from tbl_oversite_master  t1 where (t1.application_id = '" + request.searchType + "' or t1.loan_id = '" + request.searchType + "')");
                if (app > 0)
                {
                    List<DocumentProperties> documents = new List<DocumentProperties>();
                    List<DocumentProperties> cibilDocuments = new List<DocumentProperties>();

                     if (/*rolenho > 0 ||*/ (cvrolenho > 0 || role_cvnho_ass_cnt >0) && cvrolench ==0)//nho
                    {
                        decimal validcnt = new OracleHelper().ExecuteScalar<decimal>("select count(*) from tbl_oversite_master  t1 where (t1.application_id = '" + request.searchType + "' or t1.loan_id = '" + request.searchType + "') and t1.nho_verify=1");
                        if (validcnt > 0)
                        {
                            response.isDataAvailable = false;
                            response.message = "Application Already Verified";
                            return response;
                        }

                        else
                        {
                           // decimal rhodcnt = new OracleHelper().ExecuteScalar<decimal>("select count(*) from tbl_ops_oversite_master  t1 where (t1.application_id = '" + request.searchType + "' or t1.loan_id = '" + request.searchType + "') ");
                           // if (rhodcnt > 0)
                            //{
                                decimal rhovalidcnt = new OracleHelper().ExecuteScalar<decimal>("select count(*) from tbl_ops_oversite_master  t1 where (t1.application_id = '" + request.searchType + "' or t1.loan_id = '" + request.searchType + "') and t1.roh_verify is not null");
                                if (validcnt > 0)
                                {
                                    response.isDataAvailable = false;
                                    response.message = "RHO verification  completed";
                                    return response;
                                }
                                decimal rhosndcnt = new OracleHelper().ExecuteScalar<decimal>("select count(*) from tbl_ops_oversite_master  t1 where (t1.application_id = '" + request.searchType + "' or t1.loan_id = '" + request.searchType + "') and t1.roh_verify =1 and t1.roh_sendback =1");
                                if (rhosndcnt > 0)
                                {
                                    response.isDataAvailable = false;
                                    response.message = "This file Already sentback by ROH";
                                    return response;
                                }
                            //}
                           // else
                            //{
                               // response.isDataAvailable = false;
                                //response.message = "RHO verification not completed";
                               // return response;

                            //}
                            decimal validcntT = new OracleHelper().ExecuteScalar<decimal>("select count(*) from tbl_oversite_master  t1 where (t1.application_id = '" + request.searchType + "' or t1.loan_id = '" + request.searchType + "') and t1.nho_sendback=1");
                            if (validcntT > 0)
                            {
                                response.isDataAvailable = false;
                                response.message = "This file Already sentback by NHO,Can't Verify Without Recapturing";
                                return response;
                            }

                            var documentList = new List<DocumentProperties>();

                            decimal cvcount = new OracleHelper().ExecuteScalar<decimal>("select count(*) from cv_los.loan_master  t where (t.application_id = '" + request.searchType + "' or t.loan_id = '" + request.searchType + "')");
                            if (cvcount == 0)
                            {
                                query = "select p.customer_name customerName, to_char(p.product_id) as productId, to_char(p.application_id) as applicationId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, case p.applicant_type when 'P' then 1 when 'G' then 2 when 'C' then 3 end as applicantTypeId, p.customerid, to_char(p.document_type) documentType, vw.document_description documentName,/* case nvl(p.verify_flag, 0) when 1 then 'Not Verified' when 4 then 'Verified' else case nvl(p.nho_sendback_flag, 0) when 1 then 'Sendback' when 2 then 'Recaptured' else 'Not Verified' end end documentStatus*/case nvl(p.nho_verify_by, 0) when '0' then case nvl(p.nho_sendback_flag, 0) when 1 then 'Sendback' when 2 then 'Recaptured' else  'Not Verified' end else  'Verified' end documentStatus, to_char(p.old_document_no) as docNo, case p.nho_sendback_flag when 1 then p.sendback_remarks else p.nho_verified_remarks end remarks, '0' as his, p.document_no as oldDoc from TBL_OVERSITE_MASTER t left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id left outer join document_master vw on p.document_type = vw.document_id and t.product_id = vw.product_id where (t.application_id = '" + request.searchType + "' or t.loan_id = '" + request.searchType + "') and p.document_no is null and vw.noh_mandate = 1 " +
                                    "union all select p.customer_name customerName, to_char(p.product_id) as productId, to_char(p.application_id) as applicationId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, case p.applicant_type when 'P' then 1 when 'G' then 2 when 'C' then 3 end as applicantTypeId, p.customerid, to_char(p.document_type) documentType, vw.document_description documentName, /*case nvl(p.verify_flag, 0) when 1 then 'Not Verified' when 4 then 'Verified' else case nvl(p.nho_sendback_flag, 0) when 1 then 'Sendback' when 2 then 'Recaptured' else 'Not Verified' end end documentStatus*/case nvl(p.nho_verify_by, 0) when '0' then case nvl(p.nho_sendback_flag, 0) when 1 then 'Sendback' when 2 then 'Recaptured' else 'Not Verified' end else 'Verified' end documentStatus, to_char(p.document_no) as docNo, case p.nho_sendback_flag when 1 then p.sendback_remarks else p.nho_verified_remarks end remarks, '1' as his, p.old_document_no as old_Doc from TBL_OVERSITE_MASTER t left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id left outer join document_master vw on p.document_type = vw.document_id and t.product_id = vw.product_id where (t.application_id = '" + request.searchType + "' or t.loan_id = '" + request.searchType + "') and p.document_no is not null and vw.noh_mandate = 1 order by docNo asc";

                                documents = new OracleHelper().GetRecords<DocumentProperties>(query);
                                documentList = documents;
                                string cibilquery = "select p.customer_name customerName, to_char(p.product_id) as productId, to_char(p.application_id) as applicationId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, case p.applicant_type when 'P' then 1 when 'G' then 2 when 'C' then 3 end as applicantTypeId, p.customerid, to_char(p.document_type) documentType, case to_char(p.document_type) when '-52' then 'Cibil Report' end documentName,/* case nvl(p.verify_flag, 0) when 1 then 'Not Verified' when 4 then 'Verified' else case nvl(p.nho_sendback_flag, 0) when 1 then 'Sendback' when 2 then 'Recaptured' else 'Not Verified' end end documentStatus*/case nvl(p.nho_verify_by, 0)when '0' then case nvl(p.nho_sendback_flag, 0) when 1 then 'Sendback'  when 2 then  'Recaptured' else  'Not Verified' end else 'Verified' end documentStatus, to_char(p.old_document_no) as docNo, case p.nho_sendback_flag when 1 then p.sendback_remarks else p.nho_verified_remarks end remarks,'0' as his,p.document_no as oldDoc from TBL_OVERSITE_MASTER t left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id where (t.application_id = '" + request.searchType + "' or t.loan_id = '" + request.searchType + "') and p.document_no is null  and p.document_type=-52" +
                                "union all select p.customer_name customerName,to_char(p.product_id) as productId, to_char(p.application_id) as applicationId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, case p.applicant_type when 'P' then 1 when 'G' then 2 when 'C' then 3 end as applicantTypeId, p.customerid, to_char(p.document_type) documentType, case to_char(p.document_type) when '-52' then 'Cibil Report' end documentName, /*case nvl(p.verify_flag, 0) when 1 then 'Not Verified' when 4 then 'Verified' else case nvl(p.nho_sendback_flag, 0) when 1 then 'Sendback' when 2 then 'Recaptured' else 'Not Verified' end end documentStatus*/case nvl(p.nho_verify_by, 0) when '0' then case nvl(p.nho_sendback_flag, 0) when 1 then 'Sendback'  when 2 then  'Recaptured' else  'Not Verified'  end else 'Verified' end documentStatus, to_char(p.document_no) as docNo, case p.nho_sendback_flag when 1 then p.sendback_remarks else p.nho_verified_remarks end remarks,'1' as his,p.old_document_no as old_Doc from TBL_OVERSITE_MASTER t left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id where (t.application_id = '" + request.searchType + "' or t.loan_id = '" + request.searchType + "') and p.document_no is not null and  p.document_type=-52 order by docNo asc";

                                cibilDocuments = new OracleHelper().GetRecords<DocumentProperties>(cibilquery);
                                documentList.AddRange(cibilDocuments);

                                var sortedList = documentList.OrderByDescending(n => n.docNo).ToList();
                                response.documentList = sortedList;
                                query1 = "select t.customer_name customerName,to_char(t.application_id) applicationId,to_char(t.loan_id) loanID, p.product_name productName, t.product_id productId , s.scheme_name schemeName, to_char(t.scheme_id) schemeId , to_char(t.loandate, 'dd-mm-yyyy HH24:mi:ss') loanDate, to_char(t.loan_amount) loanAmount, b.branch_name branch,t.decision_flag  decisionId,t.risk_category categoryId from TBL_OVERSITE_MASTER t left outer join product_master p on t.product_id = p.product_id left outer join scheme_master s on t.scheme_id = s.scheme_id and t.product_id=s.product_id left outer join branch_master b on t.branch_id = b.branch_id where (t.application_id = '" + request.searchType + "' or t.loan_id = '" + request.searchType + "')";
                                response.LoanList = new OracleHelper().GetRecords<DocumentDataProperties>(query1);

                            }
                            else
                            {
                                query = "select p.customer_name customerName, to_char(p.product_id) as productId, to_char(p.application_id) as applicationId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, case p.applicant_type when 'P' then 1 when 'G' then 2 when 'C' then 3 end as applicantTypeId, p.customerid, to_char(p.document_type) documentType, vw.document_description documentName,/* case nvl(p.nho_verify_by, 0) when '0' then 'Not Verified' else case nvl(p.nho_sendback_flag, 0) when 1 then 'Sendback' when 2 then 'Recaptured' else 'Verified' end end documentStatus*/case nvl(p.nho_verify_by, 0) when '0' then case nvl(p.nho_sendback_flag, 0) when 1 then 'Sendback' when 2 then 'Recaptured' else 'Not Verified' end else 'Verified' end documentStatus, to_char(p.old_document_no) as docNo, case p.nho_sendback_flag when 1 then p.sendback_remarks else p.nho_verified_remarks end remarks, '0' as his, p.document_no as oldDoc from TBL_OVERSITE_MASTER t left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id left outer join CV_LOS.document_master vw on p.document_type = vw.document_id and t.product_id = vw.product_id where (t.application_id = '" + request.searchType + "' or t.loan_id = '" + request.searchType + "') and p.document_no is null and vw.noh_mandate = 1 " +
                                    "union all select p.customer_name customerName, to_char(p.product_id) as productId, to_char(p.application_id) as applicationId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, case p.applicant_type when 'P' then 1 when 'G' then 2 when 'C' then 3 end as applicantTypeId, p.customerid, to_char(p.document_type) documentType, vw.document_description documentName, /*case nvl(p.verify_flag, 0) when 1 then 'Not Verified' when 4 then 'Verified' else case nvl(p.nho_sendback_flag, 0) when 1 then 'Sendback' when 2 then 'Recaptured' else 'Verified' end end documentStatus*/case nvl(p.nho_verify_by, 0) when '0' then case nvl(p.nho_sendback_flag, 0) when 1 then 'Sendback' when 2 then 'Recaptured' else 'Not Verified' end else 'Verified' end documentStatus, to_char(p.document_no) as docNo, case p.nho_sendback_flag when 1 then p.sendback_remarks else p.nho_verified_remarks end remarks, '1' as his, p.old_document_no as old_Doc from TBL_OVERSITE_MASTER t left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id left outer join CV_LOS.document_master vw on p.document_type = vw.document_id and t.product_id = vw.product_id where (t.application_id = '" + request.searchType + "' or t.loan_id = '" + request.searchType + "') and p.document_no is not null and vw.noh_mandate = 1 order by docNo asc";

                                documents = new OracleHelper().GetRecords<DocumentProperties>(query);
                                documentList = documents;
                                string cibilquery = "select p.customer_name customerName, to_char(p.product_id) as productId, to_char(p.application_id) as applicationId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, case p.applicant_type when 'P' then 1 when 'G' then 2 when 'C' then 3 end as applicantTypeId, p.customerid, to_char(p.document_type) documentType, case to_char(p.document_type) when '-52' then 'Cibil Report' end documentName, /*case nvl(p.verify_flag, 0) when 1 then 'Not Verified' when 4 then 'Verified' else case nvl(p.nho_sendback_flag, 0) when 1 then 'Sendback' when 2 then 'Recaptured' else 'Not Verified' end end documentStatus*/case nvl(p.nho_verify_by, 0) when '0' then case nvl(p.nho_sendback_flag, 0) when 1 then 'Sendback' when 2 then 'Recaptured' else 'Not Verified' end else 'Verified' end documentStatus, to_char(p.old_document_no) as docNo, case p.nho_sendback_flag when 1 then p.sendback_remarks else p.nho_verified_remarks end remarks,'0' as his,p.document_no as oldDoc from TBL_OVERSITE_MASTER t left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id where (t.application_id = '" + request.searchType + "' or t.loan_id = '" + request.searchType + "') and p.document_no is null  and p.document_type=-52" +
                                "union all select p.customer_name customerName,to_char(p.product_id) as productId, to_char(p.application_id) as applicationId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, case p.applicant_type when 'P' then 1 when 'G' then 2 when 'C' then 3 end as applicantTypeId, p.customerid, to_char(p.document_type) documentType, case to_char(p.document_type) when '-52' then 'Cibil Report' end documentName,/* case nvl(p.verify_flag, 0) when 1 then 'Not Verified' when 4 then 'Verified' else case nvl(p.nho_sendback_flag, 0) when 1 then 'Sendback' when 2 then 'Recaptured' else 'Not Verified' end end documentStatus*/case nvl(p.nho_verify_by, 0) when '0' then case nvl(p.nho_sendback_flag, 0) when 1 then 'Sendback' when 2 then 'Recaptured' else 'Not Verified' end else 'Verified' end documentStatus, to_char(p.document_no) as docNo, case p.nho_sendback_flag when 1 then p.sendback_remarks else p.nho_verified_remarks end remarks,'1' as his,p.old_document_no as old_Doc from TBL_OVERSITE_MASTER t left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id where (t.application_id = '" + request.searchType + "' or t.loan_id = '" + request.searchType + "') and p.document_no is not null and  p.document_type=-52 order by docNo asc";

                                cibilDocuments = new OracleHelper().GetRecords<DocumentProperties>(cibilquery);
                                documentList.AddRange(cibilDocuments);

                                var sortedList = documentList.OrderByDescending(n => n.docNo).ToList();
                                response.documentList = sortedList;
                                query1 = "select t.customer_name customerName,to_char(t.application_id) applicationId,to_char(t.loan_id) loanID, p.product_name productName, t.product_id productId , s.scheme_name schemeName, to_char(t.scheme_id) schemeId , to_char(t.loandate, 'dd-mm-yyyy HH24:mi:ss') loanDate, to_char(t.loan_amount) loanAmount, b.branch_name branch,t.decision_flag  decisionId,t.risk_category categoryId from TBL_OVERSITE_MASTER t left outer join product_master p on t.product_id = p.product_id left outer join cv_los.scheme_master s on t.scheme_id = s.scheme_id and t.product_id=s.product_id left outer join cv_los.branch_master b on t.branch_id = b.branch_id where (t.application_id = '" + request.searchType + "' or t.loan_id = '" + request.searchType + "')";
                                response.LoanList = new OracleHelper().GetRecords<DocumentDataProperties>(query1);
                            }
                        }
                        if (response.documentList.Count > 0 || response.LoanList.Count > 0)
                        {
                            response.isDataAvailable = true;
                            response.message = ResponseTypeContants.SUCCESS;
                        }
                        else
                        {
                            response.isDataAvailable = false;
                            response.message = "No Data Found";
                        }

                    }
                    else if (/*rolecnch > 0 ||*/ cvrolench > 0)//nch
                    {
                        decimal validcnt = new OracleHelper().ExecuteScalar<decimal>("select count(*) from tbl_oversite_master  t1 where (t1.application_id = '" + request.searchType + "' or t1.loan_id = '" + request.searchType + "') and (t1.nch_verify=1 or t1.cr_verification_status=6)");
                        string sndbckqry = "select count(*) vrycnt from tbl_oversite_master t1 where (t1.application_id = '" + request.searchType + "' or t1.loan_id = '" + request.searchType + "') and t1.nch_verify is null  and t1.nch_sendback = 1";
                        //string sndbckqry = "select sum(vrycnt) from( select count(*) vrycnt from tbl_oversite_master t1 where (t1.application_id = '"+request.searchType+ "' or t1.loan_id = '" + request.searchType + "') and t1.nch_verify is null  and t1.nch_sendback = 1 " +
                        //    "union all select count(*) vrycnt from tbl_oversite_master t1 where (t1.application_id = '" + request.searchType + "' or t1.loan_id = '" + request.searchType + "') and t1.rch_verify is null and t1.bcm_verify is null)";
                        decimal sndbckcnt = new OracleHelper().ExecuteScalar<decimal>(sndbckqry);
                        if (sndbckcnt > 0)
                        {
                            response.isDataAvailable = false;
                            response.message = "Can't Verify Without Recapturing";
                            return response;
                        }
                        //decimal validcnt = new OracleHelper().ExecuteScalar<decimal>("select count(*) from tbl_oversite_master  t1 where (t1.application_id = '" + request.searchType + "' or t1.loan_id = '" + request.searchType + "') and t1.nch_verify=1");
                        //if (validcnt > 0)
                        //{
                        //    response.isDataAvailable = false;
                        //    response.message = "Application Already Verified";
                        //    return response;
                        //}

                        else
                        {
                            string sndbckqryrch = "select count(*) vrycnt from tbl_oversite_master t1 where (t1.application_id = '" + request.searchType + "' or t1.loan_id = '" + request.searchType + "') and t1.rch_verify is null  and t1.rch_sendback = 1";
                            decimal sndbckcntrch = new OracleHelper().ExecuteScalar<decimal>(sndbckqryrch);
                            if (sndbckcntrch > 0)
                            {
                                response.isDataAvailable = false;
                                response.message = "This file Already sentback by RCH,Can't Verify Without Recapturing";
                                return response;
                                response.popupflag = 1;
                            }
                            else
                            {
                                response.popupflag = 0;
                            }
                       
                            var documentList = new List<DocumentProperties>();

                            query = "select p.customer_name customerName,  to_char(p.product_id) as productId, to_char(p.application_id) as applicationId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, case p.applicant_type when 'P' then 1 when 'G' then 2 when 'C' then 3 end as applicantTypeId, p.customerid, to_char(p.document_type) documentType,case to_char(p.document_type) /*when '165' then 'Cibil' when '167' then 'Cibil'*/  when '153' then 'Credit approval' else vw.document_description end documentName, case nvl(p.verify_flag, 0) when 3 then case nvl(p.nch_sendback_flag, 0)when 2 then 'Recaptured' when 1 then 'Sendback by NCH' end when 10 then case nvl(p.rch_sendback_flag, 0) when 2 then 'Recaptured' when 1 then 'Sendback by NCH' end when 2 then 'Verify By NCH' when 8 then 'Verify By RCH' when 9 then 'Verify By BCM' else 'Not Verified' end documentStatus , to_char(p.old_document_no) as docNo, case p.nch_sendback_flag when 1 then p.nch_sendback_remarks else p.nch_verify_remarks end remarks,'0' as his,p.old_document_no as oldDoc,to_number(p.nch_sendback_flag) sendback_flag from TBL_OVERSITE_MASTER t left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id left outer join vw_document_master vw on p.document_type = vw.document_id and t.product_id = vw.product_id where (t.application_id = '" + request.searchType + "' or t.loan_id = '" + request.searchType + "') and p.document_no is null  and vw.NCH_MANDATE=1" +
                          "union all select p.customer_name customerName,  to_char(p.product_id) as productId, to_char(p.application_id) as applicationId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, case p.applicant_type when 'P' then 1 when 'G' then 2 when 'C' then 3 end as applicantTypeId, p.customerid, to_char(p.document_type) documentType,case to_char(p.document_type)/* when '165' then 'Cibil' when '167' then 'Cibil'*/  when '153' then 'Credit approval' else vw.document_description end documentName, case nvl(p.verify_flag, 0) when 3 then case nvl(p.nch_sendback_flag, 0)when 2 then 'Recaptured' when 1 then 'Sendback by NCH' end when 10 then case nvl(p.rch_sendback_flag, 0) when 2 then 'Recaptured' when 1 then 'Sendback by NCH' end when 2 then 'Verify By NCH' when 8 then 'Verify By RCH' when 9 then 'Verify By BCM' else 'Not Verified' end documentStatus , to_char(p.document_no) as docNo, case p.nch_sendback_flag when 1 then p.nch_sendback_remarks else p.nch_verify_remarks end remarks,'1' as his,p.old_document_no as oldDoc,to_number(p.nch_sendback_flag) sendback_flag from TBL_OVERSITE_MASTER t left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id left outer join vw_document_master vw on p.document_type = vw.document_id and t.product_id = vw.product_id where (t.application_id = '" + request.searchType + "' or t.loan_id = '" + request.searchType + "') and p.document_no is not null  and vw.NCH_MANDATE=1 order by docNo asc";
                            documents = new OracleHelper().GetRecords<DocumentProperties>(query);
                            documentList = documents;

                            string cibilquery = "select p.customer_name customerName,  to_char(p.product_id) as productId, to_char(p.application_id) as applicationId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, case p.applicant_type when 'P' then 1 when 'G' then 2 when 'C' then 3 end as applicantTypeId, p.customerid, to_char(p.document_type) documentType,case to_char(p.document_type) when '-52' then 'Cibil Report' end documentName, case nvl(p.verify_flag, 0) when 3 then case nvl(p.nch_sendback_flag, 0)when 2 then 'Recaptured' when 1 then 'Sendback by NCH' end when 10 then case nvl(p.rch_sendback_flag, 0) when 2 then 'Recaptured' when 1 then 'Sendback by NCH' end when 2 then 'Verify By NCH' when 8 then 'Verify By RCH' when 9 then 'Verify By BCM' else 'Not Verified' end documentStatus , to_char(p.old_document_no) as docNo, case p.nch_sendback_flag when 1 then p.nch_sendback_remarks else p.nch_verify_remarks end remarks,'0' as his,p.old_document_no as oldDoc,p.nch_sendback_flag sendback_flag from TBL_OVERSITE_MASTER t left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id where (t.application_id = '" + request.searchType + "' or t.loan_id = '" + request.searchType + "') and p.document_no is null and p.document_type=-52" +
                          "union all select p.customer_name customerName,  to_char(p.product_id) as productId, to_char(p.application_id) as applicationId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, case p.applicant_type when 'P' then 1 when 'G' then 2 when 'C' then 3 end as applicantTypeId, p.customerid, to_char(p.document_type) documentType,case to_char(p.document_type) when '-52' then 'Cibil Report' end documentName, case nvl(p.verify_flag, 0) when 3 then case nvl(p.nch_sendback_flag, 0)when 2 then 'Recaptured' when 1 then 'Sendback by NCH' end when 10 then case nvl(p.rch_sendback_flag, 0) when 2 then 'Recaptured' when 1 then 'Sendback by NCH' end when 2 then 'Verify By NCH' when 8 then 'Verify By RCH' when 9 then 'Verify By BCM' else 'Not Verified' end documentStatus , to_char(p.document_no) as docNo, case p.nch_sendback_flag when 1 then p.nch_sendback_remarks else p.nch_verify_remarks end remarks,'1' as his,p.old_document_no as oldDoc,p.nch_sendback_flag sendback_flag from TBL_OVERSITE_MASTER t left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id where (t.application_id = '" + request.searchType + "' or t.loan_id = '" + request.searchType + "') and p.document_no is not null  and p.document_type=-52 order by docNo asc";
                            cibilDocuments = new OracleHelper().GetRecords<DocumentProperties>(cibilquery);
                            documentList.AddRange(cibilDocuments);
                            var sortedList = documentList.OrderByDescending(n => n.docNo).ToList();
                            response.documentList = sortedList;

                            decimal cvcount = new OracleHelper().ExecuteScalar<decimal>("select count(*) from cv_los.loan_master  t where (t.application_id = '" + request.searchType + "' or t.loan_id = '" + request.searchType + "')");
                            if (cvcount == 0)
                            {
                                query1 = "select t.customer_name customerName,to_char(t.application_id) applicationId,to_char(t.loan_id) loanID, p.product_name productName, t.product_id productId , s.scheme_name schemeName, to_char(t.scheme_id) schemeId , to_char(t.loandate, 'dd-mm-yyyy HH24:mi:ss') loanDate, to_char(t.loan_amount) loanAmount, b.branch_name branch,t.decision_flag  decisionId,t.risk_category categoryId  from TBL_OVERSITE_MASTER t left outer join product_master p on t.product_id = p.product_id left outer join scheme_master s on t.scheme_id = s.scheme_id and t.product_id=s.product_id left outer join branch_master b on t.branch_id = b.branch_id where (t.application_id = '" + request.searchType + "' or t.loan_id = '" + request.searchType + "')";
                                response.LoanList = new OracleHelper().GetRecords<DocumentDataProperties>(query1);
                            }
                            else
                            {
                                query1 = "select t.customer_name customerName,to_char(t.application_id) applicationId,to_char(t.loan_id) loanID, p.product_name productName, t.product_id productId , s.scheme_name schemeName, to_char(t.scheme_id) schemeId , to_char(t.loandate, 'dd-mm-yyyy HH24:mi:ss') loanDate, to_char(t.loan_amount) loanAmount, b.branch_name branch,t.decision_flag  decisionId,t.risk_category categoryId  from TBL_OVERSITE_MASTER t left outer join product_master p on t.product_id = p.product_id left outer join cv_los.scheme_master s on t.scheme_id = s.scheme_id and t.product_id=s.product_id left outer join cv_los.branch_master b on t.branch_id = b.branch_id where (t.application_id = '" + request.searchType + "' or t.loan_id = '" + request.searchType + "')";
                                response.LoanList = new OracleHelper().GetRecords<DocumentDataProperties>(query1);
                            }
                            if (validcnt > 0)
                            {
                                response.enableFlag = 0;
                            }
                            else
                            {
                                response.enableFlag = 1;
                            }
                        }

                        if (response.documentList.Count > 0 || response.LoanList.Count > 0)
                        {
                            response.isDataAvailable = true;
                            response.message = ResponseTypeContants.SUCCESS;
                        }
                        else
                        {
                            response.isDataAvailable = false;
                            response.message = "No Data Found";
                        }

                    }


                    else if (cvroleroh > 0)//roh
                    {
                        string sndbckqry = "select count(*) vrycnt from tbl_ops_oversite_master t1 where (t1.application_id = '" + request.searchType + "' or t1.loan_id = '" + request.searchType + "') and t1.roh_verify is null  and t1.roh_sendback = 1";
                        decimal sndbckcnt = new OracleHelper().ExecuteScalar<decimal>(sndbckqry);
                        if (sndbckcnt > 0)
                        {
                            response.isDataAvailable = false;
                            response.message = "Can't Verify Without Recapturing";
                            return response;
                        }
                        decimal validcnt = new OracleHelper().ExecuteScalar<decimal>("select count(*) from tbl_ops_oversite_master t1 left outer join tbl_oversite_master t on t1.application_id=t.application_id where (t1.application_id = '" + request.searchType + "' or t1.loan_id = '" + request.searchType + "') and t1.roh_verify = 1 and (t.nho_sendback is null and t1.roh_sendback is null )");
                        if (validcnt > 0)
                        {
                            response.isDataAvailable = false;
                            response.message = "Application Already Verified";
                            return response;
                        }
                        decimal cn = new OracleHelper().ExecuteScalar<decimal>("select count(*) from tbl_ops_oversite_master t1 left outer join tbl_oversite_master t on t1.application_id=t.application_id where (t1.application_id = '" + request.searchType + "' or t1.loan_id = '" + request.searchType + "') and t1.roh_verify = 1 and (t.nho_sendback=1 or t1.roh_sendback=1)");
                        if (cn > 0)
                        {
                            response.isDataAvailable = false;
                            response.message = "Application Is In Sendback Stage";
                            return response;
                        }
                        else
                        {
                            string sndbckqrynch = "select count(*) vrycnt from tbl_oversite_master t1 where (t1.application_id = '" + request.searchType + "' or t1.loan_id = '" + request.searchType + "') and t1.nho_verify is null  and t1.nho_sendback = 1";
                            decimal sndbckcntnch = new OracleHelper().ExecuteScalar<decimal>(sndbckqrynch);
                            if (sndbckcntnch > 0)
                            {
                                response.isDataAvailable = false;
                                response.message = "This file Already sentback by NOH";
                                return response;
                                response.popupflag = 1;
                                //response.message = "This file already sendbacked by NCH";
                            }
                            else
                            {
                                response.popupflag = 0;
                                // response.message = ResponseTypeContants.SUCCESS;
                            }
                            string nhosndbckqry = "select count(*) vrycnt from tbl_oversite_master t1 where (t1.application_id = '" + request.searchType + "' or t1.loan_id = '" + request.searchType + "') and( t1.nho_verify is not null)";
                            decimal nhosndbckcnt = new OracleHelper().ExecuteScalar<decimal>(sndbckqry);
                            if (nhosndbckcnt > 0)
                            {
                                response.isDataAvailable = false;
                                response.message = "NHO verification completed";
                                return response;
                            }
                            var documentList = new List<DocumentProperties>();
                           
                            decimal cvcount = new OracleHelper().ExecuteScalar<decimal>("select count(*) from cv_los.loan_master  t where (t.application_id = '" + request.searchType + "' or t.loan_id = '" + request.searchType + "')");
                            if (cvcount == 0)
                            {
                                query = "select p.customer_name customerName, to_char(p.product_id) as productId, to_char(p.application_id) as applicationId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, case p.applicant_type when 'P' then 1 when 'G' then 2 when 'C' then 3 end as applicantTypeId, p.customerid, to_char(p.document_type) documentType, case to_char(p.document_type) when '153' then 'Credit approval' else vw.document_description end documentName, case nvl(p.verify_flag, 0) when 5 then case nvl(p.nho_sendback_flag, 0) when 2 then 'Recaptured' when 1 then 'Sendback by NOH' end when 12 then case nvl(p.rho_sendback_flag, 0) when 2 then 'Recaptured' when 1 then 'Sendback by ROH' end when 4 then 'Verify By NOH' when 11 then 'Verify By ROH' when 13 then 'Verify By BCM' else 'Not Verified' end documentStatus, to_char(p.old_document_no) as docNo, case p.rho_sendback_flag when 1 then p.RHO_SENDBACK_REMARKS else p.RHO_REMARKS end remarks, '0' as his, p.document_no as oldDoc from lms_tw.TBL_OVERSITE_MASTER t left outer join lms_tw.TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id left outer join lms_tw.document_master vw on p.document_type = vw.document_id and t.product_id = vw.product_id where (t.application_id = '" + request.searchType + "' or t.loan_id = '" + request.searchType + "') and p.document_no is null and vw.noh_mandate = 1 " +
                                    "union all select p.customer_name customerName, to_char(p.product_id) as productId, to_char(p.application_id) as applicationId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, case p.applicant_type when 'P' then 1 when 'G' then 2 when 'C' then 3 end as applicantTypeId, p.customerid, to_char(p.document_type) documentType, case to_char(p.document_type) /* when '165' then 'Cibil' when '167' then 'Cibil'*/ when '153' then 'Credit approval' else vw.document_description end documentName, case nvl(p.verify_flag, 0) when 5 then case nvl(p.nho_sendback_flag, 0) when 2 then 'Recaptured' when 1 then 'Sendback by NOH' end when 12 then case nvl(p.rho_sendback_flag, 0) when 2 then 'Recaptured' when 1 then 'Sendback by ROH' end when 4 then 'Verify By NOH' when 11 then 'Verify By ROH' when 13 then 'Verify By BCM' else 'Not Verified' end documentStatus, to_char(p.document_no) as docNo, case p.rho_sendback_flag when 1 then p.RHO_SENDBACK_REMARKS else p.RHO_REMARKS end remarks, '1' as his, p.old_document_no as oldDoc from lms_tw.TBL_OVERSITE_MASTER t left outer join lms_tw.TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id left outer join lms_tw.document_master vw on p.document_type = vw.document_id and t.product_id = vw.product_id where (t.application_id = '" + request.searchType + "' or t.loan_id = '" + request.searchType + "') and p.document_no is not null and vw.noh_mandate = 1 order by docNo asc";
                                documents = new OracleHelper().GetRecords<DocumentProperties>(query);
                                documentList = documents;
                                string cibilquery = "select p.customer_name customerName, to_char(p.product_id) as productId, to_char(p.application_id) as applicationId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, case p.applicant_type when 'P' then 1 when 'G' then 2 when 'C' then 3 end as applicantTypeId, p.customerid, to_char(p.document_type) documentType, case to_char(p.document_type) when '-52' then 'Cibil Report' when '153' then 'Credit approval' else vw.document_description end documentName, case nvl(p.verify_flag, 0) when 5 then case nvl(p.nho_sendback_flag, 0) when 2 then 'Recaptured' when 1 then 'Sendback by NOH' end when 12 then case nvl(p.rho_sendback_flag, 0) when 2 then 'Recaptured' when 1 then 'Sendback by ROH' end when 4 then 'Verify By NOH' when 11 then 'Verify By ROH' when 13 then 'Verify By BCO' else 'Not Verified' end documentStatus, to_char(p.old_document_no) as docNo, case p.rho_sendback_flag when 1 then p.RHO_SENDBACK_REMARKS else p.RHO_REMARKS end remarks, '0' as his, p.document_no as oldDoc from TBL_OVERSITE_MASTER t left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id    left outer join document_master vw on p.document_type = vw.document_id and t.product_id = vw.product_id where(t.application_id = '" + request.searchType + "' or t.loan_id = '" + request.searchType + "') and p.document_no is null and p.document_type = -52 " +
                                "union all select p.customer_name customerName, to_char(p.product_id) as productId, to_char(p.application_id) as applicationId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, case p.applicant_type when 'P' then 1 when 'G' then 2 when 'C' then 3 end as applicantTypeId, p.customerid, to_char(p.document_type) documentType, case to_char(p.document_type) when '-52' then 'Cibil Report' when '153' then 'Credit approval' else vw.document_description end documentName, case nvl(p.verify_flag, 0) when 5 then case nvl(p.nho_sendback_flag, 0) when 2 then 'Recaptured' when 1 then 'Sendback by NOH' end when 12 then case nvl(p.rho_sendback_flag, 0) when 2 then 'Recaptured' when 1 then 'Sendback by ROH' end when 4 then 'Verify By NOH' when 11 then 'Verify By ROH' when 13 then 'Verify By BCO' else 'Not Verified' end documentStatus, to_char(p.document_no) as docNo, case p.rho_sendback_flag when 1 then p.RHO_SENDBACK_REMARKS else p.RHO_REMARKS end remarks, '1' as his, p.old_document_no as oldDoc from TBL_OVERSITE_MASTER t left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id  left outer join document_master vw on p.document_type = vw.document_id and t.product_id = vw.product_id where (t.application_id = '" + request.searchType + "' or t.loan_id = '" + request.searchType + "') and p.document_no is not null and p.document_type = -52 order by docNo asc";
                                cibilDocuments = new OracleHelper().GetRecords<DocumentProperties>(cibilquery);
                                //response.documentList.AddRange(cibilDocuments);

                                documentList.AddRange(cibilDocuments);
                                var sortedList = documentList.OrderByDescending(n => n.docNo).ToList();
                                response.documentList = sortedList;
                                query1 = "select t.customer_name customerName,to_char(t.application_id) applicationId,to_char(t.loan_id) loanID, p.product_name productName, t.product_id productId , s.scheme_name schemeName, to_char(t.scheme_id) schemeId , to_char(t.loandate, 'dd-mm-yyyy HH24:mi:ss') loanDate, to_char(t.loan_amount) loanAmount, b.branch_name branch,t.decision_flag  decisionId,t.risk_category categoryId  from TBL_OVERSITE_MASTER t left outer join product_master p on t.product_id = p.product_id left outer join scheme_master s on t.scheme_id = s.scheme_id and t.product_id=s.product_id left outer join branch_master b on t.branch_id = b.branch_id where (t.application_id = '" + request.searchType + "' or t.loan_id = '" + request.searchType + "')";
                                response.LoanList = new OracleHelper().GetRecords<DocumentDataProperties>(query1);
                            }
                            else
                            {
                                query = "select p.customer_name customerName, to_char(p.product_id) as productId, to_char(p.application_id) as applicationId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, case p.applicant_type when 'P' then 1 when 'G' then 2 when 'C' then 3 end as applicantTypeId, p.customerid, to_char(p.document_type) documentType, case to_char(p.document_type) when '153' then 'Credit approval' else vw.document_description end documentName, case nvl(p.verify_flag, 0) when 5 then case nvl(p.nho_sendback_flag, 0) when 2 then 'Recaptured' when 1 then 'Sendback by NOH' end when 12 then case nvl(p.rho_sendback_flag, 0) when 2 then 'Recaptured' when 1 then 'Sendback by ROH' end when 4 then 'Verify By NOH' when 11 then 'Verify By ROH' when 13 then 'Verify By BCM' else 'Not Verified' end documentStatus, to_char(p.old_document_no) as docNo, case p.rho_sendback_flag when 1 then p.RHO_SENDBACK_REMARKS else p.RHO_REMARKS end remarks, '0' as his, p.document_no as oldDoc from lms_tw.TBL_OVERSITE_MASTER t left outer join lms_tw.TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id left outer join cv_los.document_master vw on p.document_type = vw.document_id and t.product_id = vw.product_id where (t.application_id = '" + request.searchType + "' or t.loan_id = '" + request.searchType + "') and p.document_no is null and vw.noh_mandate = 1 " +
                                    "union all select p.customer_name customerName, to_char(p.product_id) as productId, to_char(p.application_id) as applicationId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, case p.applicant_type when 'P' then 1 when 'G' then 2 when 'C' then 3 end as applicantTypeId, p.customerid, to_char(p.document_type) documentType, case to_char(p.document_type) /* when '165' then 'Cibil' when '167' then 'Cibil'*/ when '153' then 'Credit approval' else vw.document_description end documentName, case nvl(p.verify_flag, 0) when 5 then case nvl(p.nho_sendback_flag, 0) when 2 then 'Recaptured' when 1 then 'Sendback by NOH' end when 12 then case nvl(p.rho_sendback_flag, 0) when 2 then 'Recaptured' when 1 then 'Sendback by ROH' end when 4 then 'Verify By NOH' when 11 then 'Verify By ROH' when 13 then 'Verify By BCM' else 'Not Verified' end documentStatus, to_char(p.document_no) as docNo, case p.rho_sendback_flag when 1 then p.RHO_SENDBACK_REMARKS else p.RHO_REMARKS end remarks, '1' as his, p.old_document_no as oldDoc from lms_tw.TBL_OVERSITE_MASTER t left outer join lms_tw.TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id left outer join cv_los.document_master vw on p.document_type = vw.document_id and t.product_id = vw.product_id where (t.application_id = '" + request.searchType + "' or t.loan_id = '" + request.searchType + "') and p.document_no is not null and vw.noh_mandate = 1 order by docNo asc";
                                documents = new OracleHelper().GetRecords<DocumentProperties>(query);
                                documentList = documents;
                                string cibilquery = "select p.customer_name customerName, to_char(p.product_id) as productId, to_char(p.application_id) as applicationId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, case p.applicant_type when 'P' then 1 when 'G' then 2 when 'C' then 3 end as applicantTypeId, p.customerid, to_char(p.document_type) documentType, case to_char(p.document_type) when '-52' then 'Cibil Report' when '153' then 'Credit approval' else vw.document_description end documentName, case nvl(p.verify_flag, 0) when 5 then case nvl(p.nho_sendback_flag, 0) when 2 then 'Recaptured' when 1 then 'Sendback by NOH' end when 12 then case nvl(p.rho_sendback_flag, 0) when 2 then 'Recaptured' when 1 then 'Sendback by ROH' end when 4 then 'Verify By NOH' when 11 then 'Verify By ROH' when 13 then 'Verify By BCO' else 'Not Verified' end documentStatus, to_char(p.old_document_no) as docNo, case p.rho_sendback_flag when 1 then p.RHO_SENDBACK_REMARKS else p.RHO_REMARKS end remarks, '0' as his, p.document_no as oldDoc from TBL_OVERSITE_MASTER t left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id    left outer join CV_LOS.document_master vw on p.document_type = vw.document_id and t.product_id = vw.product_id where(t.application_id = '" + request.searchType + "' or t.loan_id = '" + request.searchType + "') and p.document_no is null and p.document_type = -52 " +
                                "union all select p.customer_name customerName, to_char(p.product_id) as productId, to_char(p.application_id) as applicationId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, case p.applicant_type when 'P' then 1 when 'G' then 2 when 'C' then 3 end as applicantTypeId, p.customerid, to_char(p.document_type) documentType, case to_char(p.document_type) when '-52' then 'Cibil Report' when '153' then 'Credit approval' else vw.document_description end documentName, case nvl(p.verify_flag, 0) when 5 then case nvl(p.nho_sendback_flag, 0) when 2 then 'Recaptured' when 1 then 'Sendback by NOH' end when 12 then case nvl(p.rho_sendback_flag, 0) when 2 then 'Recaptured' when 1 then 'Sendback by ROH' end when 4 then 'Verify By NOH' when 11 then 'Verify By ROH' when 13 then 'Verify By BCO' else 'Not Verified' end documentStatus, to_char(p.document_no) as docNo, case p.rho_sendback_flag when 1 then p.RHO_SENDBACK_REMARKS else p.RHO_REMARKS end remarks, '1' as his, p.old_document_no as oldDoc from TBL_OVERSITE_MASTER t left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id  left outer join CV_LOS.document_master vw on p.document_type = vw.document_id and t.product_id = vw.product_id where (t.application_id = '" + request.searchType + "' or t.loan_id = '" + request.searchType + "') and p.document_no is not null and p.document_type = -52 order by docNo asc";
                                cibilDocuments = new OracleHelper().GetRecords<DocumentProperties>(cibilquery);
                                //response.documentList.AddRange(cibilDocuments);

                                documentList.AddRange(cibilDocuments);
                                var sortedList = documentList.OrderByDescending(n => n.docNo).ToList();
                                response.documentList = sortedList;
                                query1 = "select t.customer_name customerName,to_char(t.application_id) applicationId,to_char(t.loan_id) loanID, p.product_name productName, t.product_id productId , s.scheme_name schemeName, to_char(t.scheme_id) schemeId , to_char(t.loandate, 'dd-mm-yyyy HH24:mi:ss') loanDate, to_char(t.loan_amount) loanAmount, b.branch_name branch,t.decision_flag  decisionId,t.risk_category categoryId  from TBL_OVERSITE_MASTER t left outer join product_master p on t.product_id = p.product_id left outer join cv_los.scheme_master s on t.scheme_id = s.scheme_id and t.product_id=s.product_id left outer join cv_los.branch_master b on t.branch_id = b.branch_id where (t.application_id = '" + request.searchType + "' or t.loan_id = '" + request.searchType + "')";
                                response.LoanList = new OracleHelper().GetRecords<DocumentDataProperties>(query1);
                            }
                        }

                        if (response.documentList.Count > 0 || response.LoanList.Count > 0)
                        {
                            response.isDataAvailable = true;
                            response.message = ResponseTypeContants.SUCCESS;
                        }
                        else
                        {
                            response.isDataAvailable = false;
                            response.message = "No Data Found";
                        }

                    }
                    else if (cvrolerch > 0)//rch
                    {
                        string sndbckqry = "select count(*) vrycnt from tbl_oversite_master t1 where (t1.application_id = '" + request.searchType + "' or t1.loan_id = '" + request.searchType + "') and t1.rch_verify is null  and t1.rch_sendback = 1";
                        decimal sndbckcnt = new OracleHelper().ExecuteScalar<decimal>(sndbckqry);
                        if (sndbckcnt > 0)
                        {
                            response.isDataAvailable = false;
                            response.message = "Can't Verify Without Recapturing";
                            return response;
                        }
                        decimal validcnt = new OracleHelper().ExecuteScalar<decimal>("select count(*) from tbl_oversite_master  t1 where (t1.application_id = '" + request.searchType + "' or t1.loan_id = '" + request.searchType + "') and t1.RCH_VERIFY=1");
                        if (validcnt > 0)
                        {
                            response.isDataAvailable = false;
                            response.message = "Application Already Verified";
                            return response;
                        }
                        else
                        {
                            string sndbckqrynch = "select count(*) vrycnt from tbl_oversite_master t1 where (t1.application_id = '" + request.searchType + "' or t1.loan_id = '" + request.searchType + "') and t1.nch_verify is null  and t1.nch_sendback = 1";
                            decimal sndbckcntnch = new OracleHelper().ExecuteScalar<decimal>(sndbckqrynch);
                            if (sndbckcntnch > 0)
                            {
                                response.isDataAvailable = false;
                                response.message = "This file Already sentback by NCH,Can't Verify Without Recapturing";
                                return response;
                                response.popupflag = 1;
                            }
                            else
                            {
                                response.popupflag = 0;
                            }
                            var documentList = new List<DocumentProperties>();

                            query = "select p.customer_name customerName,  to_char(p.product_id) as productId, to_char(p.application_id) as applicationId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, case p.applicant_type when 'P' then 1 when 'G' then 2 when 'C' then 3 end as applicantTypeId, p.customerid, to_char(p.document_type) documentType,case to_char(p.document_type) /*when '165' then 'Cibil' when '167' then 'Cibil' */ when '153' then 'Credit approval' else vw.document_description end documentName, case nvl(p.verify_flag, 0) when 3 then case nvl(p.nch_sendback_flag, 0)when 2 then 'Recaptured' when 1 then 'Sendback by NCH' end when 10 then case nvl(p.rch_sendback_flag, 0) when 2 then 'Recaptured' when 1 then 'Sendback by RCH' end when 2 then 'Verify By NCH' when 8 then 'Verify By RCH' when 9 then 'Verify By BCM' else 'Not Verified' end documentStatus, to_char(p.old_document_no) as docNo, case p.rch_sendback_flag when 1 then p.rch_sendback_remarks else p.rch_verify_remarks end remarks,'0' as his,p.old_document_no as oldDoc from TBL_OVERSITE_MASTER t left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id left outer join vw_document_master vw on p.document_type = vw.document_id and t.product_id = vw.product_id where (t.application_id = '" + request.searchType + "' or t.loan_id = '" + request.searchType + "') and p.document_no is null  and vw.NCH_MANDATE=1" +
                            "union all select p.customer_name customerName,  to_char(p.product_id) as productId, to_char(p.application_id) as applicationId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, case p.applicant_type when 'P' then 1 when 'G' then 2 when 'C' then 3 end as applicantTypeId, p.customerid, to_char(p.document_type) documentType,case to_char(p.document_type) /*when '165' then 'Cibil' when '167' then 'Cibil'*/  when '153' then 'Credit approval' else vw.document_description end documentName, case nvl(p.verify_flag, 0) when 3 then case nvl(p.nch_sendback_flag, 0)when 2 then 'Recaptured' when 1 then 'Sendback by NCH' end when 10 then case nvl(p.rch_sendback_flag, 0) when 2 then 'Recaptured' when 1 then 'Sendback by RCH' end when 2 then 'Verify By NCH' when 8 then 'Verify By RCH' when 9 then 'Verify By BCM' else 'Not Verified' end documentStatus, to_char(p.document_no) as docNo, case p.rch_sendback_flag when 1 then p.rch_sendback_remarks else p.rch_verify_remarks end remarks,'1' as his,p.old_document_no as oldDoc from TBL_OVERSITE_MASTER t left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id left outer join vw_document_master vw on p.document_type = vw.document_id and t.product_id = vw.product_id where (t.application_id = '" + request.searchType + "' or t.loan_id = '" + request.searchType + "') and p.document_no is not null  and vw.NCH_MANDATE=1 order by docNo asc";
                            documents = new OracleHelper().GetRecords<DocumentProperties>(query);
                            documentList = documents;

                            string cibilquery = "select p.customer_name customerName, to_char(p.product_id) as productId, to_char(p.application_id) as applicationId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, case p.applicant_type when 'P' then 1 when 'G' then 2 when 'C' then 3 end as applicantTypeId, p.customerid, to_char(p.document_type) documentType,case to_char(p.document_type) when '-52' then 'Cibil Report'  when '153' then 'Credit approval' end documentName, case nvl(p.verify_flag, 0) when 3 then case nvl(p.nch_sendback_flag, 0)when 2 then 'Recaptured' when 1 then 'Sendback by NCH' end when 10 then case nvl(p.rch_sendback_flag, 0) when 2 then 'Recaptured' when 1 then 'Sendback by RCH' end when 2 then 'Verify By NCH' when 8 then 'Verify By RCH' when 9 then 'Verify By BCM' else 'Not Verified' end documentStatus, to_char(p.old_document_no) as docNo, case p.rch_sendback_flag when 1 then p.rch_sendback_remarks else p.rch_verify_remarks end remarks,'0' as his,p.old_document_no as oldDoc from TBL_OVERSITE_MASTER t left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id where(t.application_id = '" + request.searchType + "' or t.loan_id = '" + request.searchType + "') and p.document_no is null and p.document_type=-52 " +
                            "union all select p.customer_name customerName,  to_char(p.product_id) as productId, to_char(p.application_id) as applicationId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, case p.applicant_type when 'P' then 1 when 'G' then 2 when 'C' then 3 end as applicantTypeId, p.customerid, to_char(p.document_type) documentType,case to_char(p.document_type) when '-52' then 'Cibil Report'  when '153' then 'Credit approval' end documentName, case nvl(p.verify_flag, 0) when 3 then case nvl(p.nch_sendback_flag, 0)when 2 then 'Recaptured' when 1 then 'Sendback by NCH' end when 10 then case nvl(p.rch_sendback_flag, 0) when 2 then 'Recaptured' when 1 then 'Sendback by RCH' end when 2 then 'Verify By NCH' when 8 then 'Verify By RCH' when 9 then 'Verify By BCM' else 'Not Verified' end documentStatus, to_char(p.document_no) as docNo, case p.rch_sendback_flag when 1 then p.rch_sendback_remarks else p.rch_verify_remarks end remarks,'1' as his,p.old_document_no as oldDoc from TBL_OVERSITE_MASTER t left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id where (t.application_id = '" + request.searchType + "' or t.loan_id = '" + request.searchType + "') and p.document_no is not null and p.document_type=-52  order by docNo asc";
                            cibilDocuments = new OracleHelper().GetRecords<DocumentProperties>(cibilquery);
                            documentList.AddRange(cibilDocuments);
                            var sortedList = documentList.OrderByDescending(n => n.docNo).ToList();
                            response.documentList = sortedList;
                            decimal cvcount = new OracleHelper().ExecuteScalar<decimal>("select count(*) from cv_los.loan_master  t where (t.application_id = '" + request.searchType + "' or t.loan_id = '" + request.searchType + "')");
                            if (cvcount == 0)
                            {
                                query1 = "select t.customer_name customerName,to_char(t.application_id) applicationId,to_char(t.loan_id) loanID, p.product_name productName, t.product_id productId , s.scheme_name schemeName, to_char(t.scheme_id) schemeId , to_char(t.loandate, 'dd-mm-yyyy HH24:mi:ss') loanDate, to_char(t.loan_amount) loanAmount, b.branch_name branch,t.decision_flag  decisionId,t.risk_category categoryId  from TBL_OVERSITE_MASTER t left outer join product_master p on t.product_id = p.product_id left outer join scheme_master s on t.scheme_id = s.scheme_id and t.product_id=s.product_id left outer join branch_master b on t.branch_id = b.branch_id where (t.application_id = '" + request.searchType + "' or t.loan_id = '" + request.searchType + "')";
                                response.LoanList = new OracleHelper().GetRecords<DocumentDataProperties>(query1);
                            }
                            else
                            {
                                query1 = "select t.customer_name customerName,to_char(t.application_id) applicationId,to_char(t.loan_id) loanID, p.product_name productName, t.product_id productId , s.scheme_name schemeName, to_char(t.scheme_id) schemeId , to_char(t.loandate, 'dd-mm-yyyy HH24:mi:ss') loanDate, to_char(t.loan_amount) loanAmount, b.branch_name branch,t.decision_flag  decisionId,t.risk_category categoryId  from TBL_OVERSITE_MASTER t left outer join product_master p on t.product_id = p.product_id left outer join cv_los.scheme_master s on t.scheme_id = s.scheme_id and t.product_id=s.product_id left outer join cv_los.branch_master b on t.branch_id = b.branch_id where (t.application_id = '" + request.searchType + "' or t.loan_id = '" + request.searchType + "')";
                                response.LoanList = new OracleHelper().GetRecords<DocumentDataProperties>(query1);
                            }
                        }

                        if (response.documentList.Count > 0 || response.LoanList.Count > 0)
                        {
                            response.isDataAvailable = true;
                            response.message = ResponseTypeContants.SUCCESS;
                        }
                        else
                        {
                            response.isDataAvailable = false;
                            response.message = "No Data Found";
                        }

                    }
                    else
                    {
                        response.isDataAvailable = false;
                        response.message = "You Have No Permission To View";
                    }
                }
                else
                {
                    response.isDataAvailable = false;
                    response.message = "Invalid Loan Id/Application Id";
                }

            }

            catch (Exception ex)
            {
                ErrorLogRequest errorLogRequest = new Oversite.DTO.ErrorLog.Request.ErrorLogRequest();
                errorLogRequest.Function_Name = "Documents Get - GetDocument";
                errorLogRequest.V_Data = output;
                errorLogRequest.V_Exception = ex.Message;
                new ErrorLog.ErrorLogDataSource().ErrorLog(errorLogRequest);
                response.message = ex.Message;


            }
            return response;
        }
        public DocumentResponse ViewDocument(DocumentRequest request)
        {
            MongoDbConnectionHandler mongoDbConnectionHandler = new MongoDbConnectionHandler();
            DocumentResponse response = new DocumentResponse();
            GetDocumentProperties applicationDocument = new GetDocumentProperties();
            GetDocumentPropertiesfinal getDocumentPropertiesfinal = new GetDocumentPropertiesfinal();
            GetDocumentsPropertiesnew applicationDocumentnew = new GetDocumentsPropertiesnew();
            GetDocumentsPropertiesnewfin findoc = new GetDocumentsPropertiesnewfin();
            DocumentRemarksProperties documentRemarksProperties = new DocumentRemarksProperties();
            string query = string.Empty;
            string qry = string.Empty;
            string output = JsonConvert.SerializeObject(request);

            try
            {
                var log1 = WriteLog("View document Data");
                log1 = WriteLog(JsonConvert.SerializeObject(request));
                qry = "select count(*) from lms_tw.TBL_OVERSITE_master t where (t.application_id='" + request.applicationId + "' or t.loan_id='" + request.applicationId + "') and t.lms=0 ";
                decimal cnt = new OracleHelper().ExecuteScalar<decimal>(qry);
                log1 = WriteLog(JsonConvert.SerializeObject(cnt));
                if (cnt > 0)
                {
                    int DocumentNo = Convert.ToInt32(request.DocumentNo);
                    findoc = mongoDbConnectionHandler.Select<GetDocumentsPropertiesnewfin>("Finnone_CV_documents", "DocumentNo", DocumentNo);
                    log1 = WriteLog(JsonConvert.SerializeObject(findoc));
                    getDocumentPropertiesfinal.DocumentNo = findoc.DocumentNo.ToString();
                    getDocumentPropertiesfinal.ApplicationId = findoc.ApplicationId;
                    getDocumentPropertiesfinal.Data = findoc.Data;
                    getDocumentPropertiesfinal.DocID = findoc.DocID;
                    getDocumentPropertiesfinal.Extension = findoc.Extension;
                    getDocumentPropertiesfinal.Id = findoc.Id;
                    getDocumentPropertiesfinal.NewDocID = findoc.NewDocID;
                    response.GetDocument = getDocumentPropertiesfinal;
                    log1 = WriteLog(JsonConvert.SerializeObject(getDocumentPropertiesfinal));

                    query = "select case t.verify_flag when 5 then t.sendback_remarks when 4 then t.nho_verified_remarks end nhoRemarks, case t.verify_flag when 3 then t.sendback_remarks when 2 then t.nch_verify_remarks end nchRemarks, t.doc_remarks, t.recaptured_remarks rchRemarks from lms_tw.tbl_oversite_documents_mst t " +
                   " where t.APPLICATION_ID = " + request.applicationId + " and t.DOCUMENT_TYPE = " + request.documentType + " and customerid = '" + request.customerId + "'";
                    response.Remarklist = new OracleHelper().GetRecords<DocumentRemarksProperties>(query);
                    log1 = WriteLog(JsonConvert.SerializeObject(response.Remarklist));

                    if (response.GetDocument.Data != null)
                    {
                        response.isDataAvailable = true;
                        response.message = ResponseTypeContants.SUCCESS;
                    }
                    else
                    {
                        response.isDataAvailable = false;
                        response.message = "No Data Found";
                    }
                }
                else
                {
                    qry = "select count(*) from vw_cv_application_doc_dtl t  where t.document_no='" + request.DocumentNo + "'";

                    if (request.documentType == "-52")
                    {
                        applicationDocument = new OracleHelper().GetRecord<GetDocumentProperties>("select t.reportdata Data,t.EXTENSION Extension  from cv_los.TBL_CREDIBUREAUDOCS t where t.fileid = '" + request.DocumentNo + "' and t.applicationid='"+request.applicationId+"'");
                        if (applicationDocument.Data != null)
                        {
                            getDocumentPropertiesfinal.Data = applicationDocument.Data;
                            getDocumentPropertiesfinal.Extension = applicationDocument.Extension;
                        }
                        else
                        {
                            string DocumentNo = request.DocumentNo.ToString();
                            applicationDocument = mongoDbConnectionHandler.select<GetDocumentProperties>("CV_Application_Documents", "DocumentNo", DocumentNo);
                            getDocumentPropertiesfinal.DocumentNo = applicationDocument.DocumentNo;
                            getDocumentPropertiesfinal.ApplicationId = applicationDocument.ApplicationId.ToString();
                            getDocumentPropertiesfinal.Data = applicationDocument.Data;
                            getDocumentPropertiesfinal.DocID = applicationDocument.DocID;
                            getDocumentPropertiesfinal.Extension = applicationDocument.Extension;
                            getDocumentPropertiesfinal.Id = applicationDocument.Id;
                            getDocumentPropertiesfinal.NewDocID = applicationDocument.NewDocID;
                        }

                    }
                    else
                    {
                        if (request.productId == 10)
                        {
                            string prdctqry = "select count(*) from lms_tw.APPLICATION_MST t where t.application_id='" + request.applicationId + "'";
                            decimal prdctcnt = new OracleHelper().ExecuteScalar<decimal>(prdctqry);
                            if (prdctcnt > 0)
                            {
                                string DocumentNo = request.DocumentNo;
                                applicationDocumentnew = mongoDbConnectionHandler.Select<GetDocumentsPropertiesnew>("Application_Documents", "DocumentNo", Convert.ToInt32(DocumentNo));

                                getDocumentPropertiesfinal.DocumentNo = applicationDocumentnew.DocumentNo.ToString();
                                getDocumentPropertiesfinal.ApplicationId = applicationDocumentnew.ApplicationId.ToString();
                                getDocumentPropertiesfinal.Data = applicationDocumentnew.Data;
                                getDocumentPropertiesfinal.DocID = applicationDocumentnew.DocID;
                                getDocumentPropertiesfinal.Extension = applicationDocumentnew.Extension;
                                getDocumentPropertiesfinal.Id = applicationDocumentnew.Id;
                                applicationDocument.NewDocID = applicationDocumentnew.NewDocID;
                            }
                            else
                            {
                              

                                string DocumentNo = request.DocumentNo.ToString();
                                applicationDocument = mongoDbConnectionHandler.select<GetDocumentProperties>("CV_Application_Documents", "DocumentNo", DocumentNo);
                                getDocumentPropertiesfinal.DocumentNo = applicationDocument.DocumentNo.ToString();
                                getDocumentPropertiesfinal.ApplicationId = applicationDocument.ApplicationId.ToString();
                                getDocumentPropertiesfinal.Data = applicationDocument.Data;
                                getDocumentPropertiesfinal.DocID = applicationDocument.DocID;
                                getDocumentPropertiesfinal.Extension = applicationDocument.Extension;
                                getDocumentPropertiesfinal.Id = applicationDocument.Id;
                                getDocumentPropertiesfinal.NewDocID = applicationDocument.NewDocID;
                            }
                        }
                        else
                        {
                            string DocumentNo = request.DocumentNo.ToString();
                            applicationDocument = mongoDbConnectionHandler.select<GetDocumentProperties>("CV_Application_Documents", "DocumentNo", DocumentNo);
                            getDocumentPropertiesfinal.DocumentNo = applicationDocument.DocumentNo.ToString();
                            getDocumentPropertiesfinal.ApplicationId = applicationDocument.ApplicationId.ToString();
                            getDocumentPropertiesfinal.Data = applicationDocument.Data;
                            getDocumentPropertiesfinal.DocID = applicationDocument.DocID;
                            getDocumentPropertiesfinal.Extension = applicationDocument.Extension;
                            getDocumentPropertiesfinal.Id = applicationDocument.Id;
                            getDocumentPropertiesfinal.NewDocID = applicationDocument.NewDocID;
                        }
                    }
                    response.GetDocument = getDocumentPropertiesfinal;




                    query = "select t.nho_verified_remarks nhoRemarks, t.nch_verify_remarks nchRemarks, t.rho_remarks rhoRemarks, t.rch_verify_remarks rchRemarks from lms_tw.tbl_oversite_documents_mst t " +
                     " where t.APPLICATION_ID = " + request.applicationId + " and t.DOCUMENT_TYPE = " + request.documentType + " and customerid = '" + request.customerId + "'";
                    response.Remarklist = new OracleHelper().GetRecords<DocumentRemarksProperties>(query);
                    

                    if (response.GetDocument != null || response.Remarklist != null)
                    {
                        response.isDataAvailable = true;
                        response.message = ResponseTypeContants.SUCCESS;
                    }
                    else
                    {
                        response.isDataAvailable = false;
                        response.message = "No Data Found";
                    }

                }
            }

            catch (Exception ex)
            {
                var log1 = WriteLog("View document Data");
                log1 = WriteLog(JsonConvert.SerializeObject(ex.Message));
                response.message = ex.Message;


            }
            return response;


        }


        public async Task WriteLog(string context)
        {
            try
            {
                string FolderPath = @"C:\Tracer\Oversite";// configuration.GetSection("MySettings").GetSection("ExceptionPath").Value;//ConfigurationManager.AppSettings["ExceptionPath"];
                if (!Directory.Exists(FolderPath))
                {
                    Directory.CreateDirectory(FolderPath);
                }

                string fileName = FolderPath + "\\" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                string dateTimeFormat = "MM/dd/yyyy hh:mm tt";
                StringBuilder content = new StringBuilder();
                content.AppendLine("_________________________________________________");
                content.AppendLine("[" + DateTime.Now.ToString(dateTimeFormat) + "]");


                if (context != null)
                {
                    content.AppendLine("Context: " + context);
                }

                //if (Page != null)
                //{
                //    content.AppendLine("Page: " + Page);
                //}

                //content.AppendLine(ex.ToString());
                await System.IO.File.AppendAllTextAsync(fileName, content.ToString());
                content.AppendLine("_________________________________________________");
            }
            finally
            {
                ///
            }
        }
        public Response<UpdateDocumentResponse> GetDocDetails(UpdateDocumentRequest request)
        {
            UpdateDocumentResponse updateDocumentResponse = new UpdateDocumentResponse();
            Response<UpdateDocumentResponse> response = new Response<UpdateDocumentResponse>();
            List<string> cmd = new List<string>();
            string query = string.Empty;
            string query1 = string.Empty;
            string query2 = string.Empty;
            string output = JsonConvert.SerializeObject(request);
            try
            {           
                decimal app = new OracleHelper().ExecuteScalar<decimal>("select count(*) from lms_tw.tbl_oversite_master  t1 where (t1.application_id = '" + request.searchValue + "' or t1.loan_id = '" + request.searchValue + "')");
                if (app > 0)
                {
                    string query3 = "select count(*) from lms_tw.TBL_OVERSITE_MASTER t where (t.application_id = '" + request.searchValue + "' or t.loan_id = '" + request.searchValue + "') and nvl(t.lms,0)=0 and nvl(t.verification_status,0)=7";
                    decimal count2 = new OracleHelper().ExecuteScalar<decimal>(query3);

                    if (count2 > 0)
                    {
                        query = "select p.customer_name customerName, to_char(t.application_id) as applicationId, to_char(t.loan_id) as loanId, P.CUSTOMERID customerId, p.applicant_type as applicantTypeId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, to_char(p.document_type) docType, d.document_description docName, p.sendback_remarks remarks, to_char(p.old_document_no) as docNo, pr.product_name as productName, to_char(t.product_id) as productId, sc.scheme_name as schemeName,t.scheme_id schemeId, t.LOANDATE, to_char(t.loan_amount) as loanAmount, br.branch_name as branch from TBL_OVERSITE_MASTER t " +
                                                  "left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id /*and (p.nch_sendback_flag = 1 or p.nho_sendback_flag = 1)*/ left outer join vw_document_master d on p.document_type = d.document_id and t.product_id = d.product_id left outer join product_master pr on pr.product_id = t.product_id left outer join TBL_CHECKLIST_MASTER sc on sc.scheme_id = t.scheme_id  left outer join branch_master br on br.branch_id = t.branch_id where (to_char(t.application_id) = '" + request.searchValue + "' or to_char(t.loan_id) = '" + request.searchValue + "') and p.document_no is null union all select p.customer_name customerName, to_char(t.application_id) as applicationId, " +
                                                  "to_char(t.loan_id) as loanId, P.CUSTOMERID customerId, p.applicant_type as applicantTypeId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, to_char(p.document_type) docType, d.document_description docName, p.sendback_remarks remarks, to_char(p.document_no) as docNo, pr.product_name as productName, to_char(t.product_id) as productId, sc.scheme_name as schemeName, t.scheme_id schemeId, t.LOANDATE, to_char(t.loan_amount) as loanAmount, br.branch_name as branch from TBL_OVERSITE_MASTER t left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id and (p.nch_sendback_flag = 1 or p.nho_sendback_flag = 1) " +
                                                  "left outer join vw_document_master d on p.document_type = d.document_id and t.product_id = d.product_id left outer join product_master pr on pr.product_id = t.product_id left outer join TBL_CHECKLIST_MASTER sc on sc.scheme_id = t.scheme_id  left outer join branch_master br on br.branch_id = t.branch_id where (to_char(t.application_id) = '" + request.searchValue + "' or to_char(t.loan_id) = '" + request.searchValue + "') and p.document_no is not null";
                        updateDocumentResponse.PrimaryDataList = new OracleHelper().GetRecords<PrimaryDataProperties>(query);
                        string queryy44 = "select t.Applicant_Type as applicantType ,decode(t.applicant_type,'P','PrimaryCustomer','C','Co-Applicant','G','Guarantor') as applicantTypedesc, T.CUSTOMER_NAME as customerName ,T.CUSTOMERID as customerId from TBL_OVERSITE_DOCUMENTS_MST t where (to_char(t.application_id) = '" + request.searchValue + "' or to_char(t.loan_id) = '" + request.searchValue + "') group by t.applicant_type,T.CUSTOMER_NAME,T.CUSTOMERID";
                        updateDocumentResponse.Finnonecustlist = new OracleHelper().GetRecords<FinnonecustProperties>(queryy44);
                        updateDocumentResponse.LMS = 0;
                        response.Data = updateDocumentResponse;
                        response.responseMsg = "Success";
                        response.apiStatus = ResponseTypeContants.SUCCESS;
                        response.status = ApiStatusConstants.COMPLETED;

                    }
                    else
                    {

                        decimal cvrolerch = new OracleHelper().ExecuteScalar<decimal>("select count(*) from lms_tw.vw_cv_user_role_details  t1 left outer join lms_tw.vw_cv_user_master t2 ON  t1.user_id = t2.user_id WHERE t2.emp_code = '" + request.enterBy + "' and t1.role_id=78");//oversite rch
                        decimal cvrolerho = new OracleHelper().ExecuteScalar<decimal>("select count(*) from lms_tw.vw_cv_user_role_details  t1 left outer join lms_tw.vw_cv_user_master t2 ON  t1.user_id = t2.user_id  WHERE t2.emp_code = '" + request.enterBy + "' and t1.role_id=80");//oversite rho
                        decimal cvrolerm = new OracleHelper().ExecuteScalar<decimal>("select count(*) from lms_tw.vw_cv_user_role_details  t1 left outer join lms_tw.vw_cv_user_master t2 ON  t1.user_id = t2.user_id WHERE t2.emp_code = '" + request.enterBy + "' and t1.role_id=48");

                        //bcm branch team 100890
                        decimal cvrolebcm = new OracleHelper().ExecuteScalar<decimal>("select count(*) from lms_tw.vw_cv_user_role_details  t1 left outer join lms_tw.vw_cv_user_master t2 ON  t1.user_id = t2.user_id WHERE t2.emp_code = '" + request.enterBy + "' and t1.role_id=79");//oversite bch

                        decimal cvrolebco = new OracleHelper().ExecuteScalar<decimal>("select count(*) from lms_tw.vw_cv_user_role_details  t1 left outer join lms_tw.vw_cv_user_master t2 ON  t1.user_id = t2.user_id WHERE t2.emp_code = '" + request.enterBy + "' and t1.role_id=82");//oversite bho

                        decimal cvloancount = new OracleHelper().ExecuteScalar<decimal>("select count(tt.application_id) from cv_los.disbursement_details tt where (tt.application_id = '" + request.searchValue + "' or tt.loan_id = '" + request.searchValue + "')");

                        //cvloan
                        if (cvloancount > 0)
                        {
                            if (cvrolerm > 0 /*|| rolecrm > 0*/)
                            {
                                query = "select distinct p.customer_name customerName, to_char(t.application_id) as applicationId, to_char(t.loan_id) as loanId, P.CUSTOMERID customerId, p.applicant_type as applicantTypeId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, to_char(p.document_type) docType, d.document_description docName, p.sendback_remarks remarks, to_char(p.old_document_no) as docNo, pr.product_name as productName, to_char(t.product_id) as productId, sc.scheme_name as schemeName,t.scheme_id schemeId, t.LOANDATE, to_char(t.loan_amount) as loanAmount, " +
                               "br.branch_name as branch from TBL_OVERSITE_MASTER t left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id and (p.nch_sendback_flag = 1 or p.nho_sendback_flag = 1 ) left outer join vw_document_master d on p.document_type = d.document_id and t.product_id = d.product_id left outer join product_master pr on pr.product_id = t.product_id left outer join vw_scheme_master sc on sc.scheme_id = t.scheme_id and sc.product_id=t.product_id left outer join branch_master br on br.branch_id = t.branch_id where (to_char(t.application_id) = '" + request.searchValue + "' or to_char(t.loan_id) = '" + request.searchValue + "') " +
                              "and p.document_no is null union all select distinct p.customer_name customerName, to_char(t.application_id) as applicationId, to_char(t.loan_id) as loanId, P.CUSTOMERID customerId, p.applicant_type as applicantTypeId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, to_char(p.document_type) docType, d.document_description docName, p.sendback_remarks remarks, to_char(p.document_no) as docNo, pr.product_name as productName, to_char(t.product_id) as productId, sc.scheme_name as schemeName,t.scheme_id schemeId, t.LOANDATE, to_char(t.loan_amount) as loanAmount, br.branch_name as branch " +
                               "from TBL_OVERSITE_MASTER t left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id and (p.nch_sendback_flag = 1 or p.nho_sendback_flag = 1) left outer join vw_document_master d on p.document_type = d.document_id and t.product_id = d.product_id left outer join product_master pr on pr.product_id = t.product_id left outer join vw_scheme_master sc on sc.scheme_id = t.scheme_id and sc.product_id=t.product_id left outer join branch_master br on br.branch_id = t.branch_id where (to_char(t.application_id) = '" + request.searchValue + "' or to_char(t.loan_id) = '" + request.searchValue + "') and p.document_no is not null";
                                updateDocumentResponse.PrimaryDataList = new OracleHelper().GetRecords<PrimaryDataProperties>(query);
                                updateDocumentResponse.LMS = 1;
                                response.Data = updateDocumentResponse;
                                response.responseMsg = "Success";
                                response.apiStatus = ResponseTypeContants.SUCCESS;
                                response.status = ApiStatusConstants.COMPLETED;
                                return response;
                            }
                            else if (cvrolerho > 0)
                            {
                                //query = "select t.customer_name customerName, case t.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, p.document_type documentType, d.document_description documentStatus, p.doc_remarks remarks from TBL_OVERSITE_MASTER t left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id left outer join document_master d on p.document_type = d.document_id and t.product_id = d.product_id where (t.application_id = " + request.searchValue + " or t.loan_id = " + request.searchValue + ")";
                                query = "select distinct p.customer_name customerName, to_char(t.application_id) as applicationId, to_char(t.loan_id) as loanId, P.CUSTOMERID customerId, p.applicant_type as applicantTypeId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, to_char(p.document_type) docType, d.document_description docName, p.sendback_remarks remarks, to_char(p.old_document_no) as docNo, pr.product_name as productName, to_char(t.product_id) as productId, sc.scheme_name as schemeName,t.scheme_id schemeId, t.LOANDATE, to_char(t.loan_amount) as loanAmount, br.branch_name as branch from TBL_OVERSITE_MASTER t " +
                                    "left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id and ( p.nho_sendback_flag = 1) left outer join vw_document_master d on p.document_type = d.document_id and t.product_id = d.product_id left outer join product_master pr on pr.product_id = t.product_id left outer join vw_scheme_master sc on sc.scheme_id = t.scheme_id and sc.product_id=t.product_id left outer join branch_master br on br.branch_id = t.branch_id where (to_char(t.application_id) = '" + request.searchValue + "' or to_char(t.loan_id) = '" + request.searchValue + "') and p.document_no is null union all select distinct p.customer_name customerName, to_char(t.application_id) as applicationId, " +
                                    "to_char(t.loan_id) as loanId, P.CUSTOMERID customerId, p.applicant_type as applicantTypeId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, to_char(p.document_type) docType, d.document_description docName, p.sendback_remarks remarks, to_char(p.document_no) as docNo, pr.product_name as productName, to_char(t.product_id) as productId, sc.scheme_name as schemeName,t.scheme_id schemeId, t.LOANDATE, to_char(t.loan_amount) as loanAmount, br.branch_name as branch from TBL_OVERSITE_MASTER t left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id and (p.nho_sendback_flag = 1) " +
                                    "left outer join vw_document_master d on p.document_type = d.document_id and t.product_id = d.product_id left outer join product_master pr on pr.product_id = t.product_id left outer join vw_scheme_master sc on sc.scheme_id = t.scheme_id and sc.product_id=t.product_id left outer join branch_master br on br.branch_id = t.branch_id where (to_char(t.application_id) = '" + request.searchValue + "' or to_char(t.loan_id) = '" + request.searchValue + "') and p.document_no is not null";
                                updateDocumentResponse.PrimaryDataList = new OracleHelper().GetRecords<PrimaryDataProperties>(query);
                                //PrimaryDataList.Add(PrimaryDataList1);
                                updateDocumentResponse.LMS = 1;
                                response.Data = updateDocumentResponse;
                                response.responseMsg = "Success";
                                response.apiStatus = ResponseTypeContants.SUCCESS;
                                response.status = ApiStatusConstants.COMPLETED;
                                return response;
                            }
                            else if (cvrolerch > 0 /*|| rolecrch > 0*/)
                            {
                                query = "select distinct p.customer_name customerName, to_char(t.application_id) as applicationId, to_char(t.loan_id) as loanId, P.CUSTOMERID customerId, p.applicant_type as applicantTypeId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, to_char(p.document_type) docType, d.document_description docName, p.nch_sendback_remarks remarks, to_char(p.old_document_no) as docNo, pr.product_name as productName, to_char(t.product_id) as productId, sc.scheme_name as schemeName,t.scheme_id schemeId, t.LOANDATE, to_char(t.loan_amount) as loanAmount, " +
                                    "br.branch_name as branch from TBL_OVERSITE_MASTER t left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id and (p.nch_sendback_flag = 1) left outer join vw_document_master d on p.document_type = d.document_id and t.product_id = d.product_id left outer join product_master pr on pr.product_id = t.product_id left outer join cv_los.scheme_master sc on sc.scheme_id = t.scheme_id and sc.product_id=t.product_id left outer join branch_master br on br.branch_id = t.branch_id where (to_char(t.application_id) = '" + request.searchValue + "' or to_char(t.loan_id) = '" + request.searchValue + "') " +
                                    "and p.document_no is null and p.nch_sendback_to='R' union all select distinct p.customer_name customerName, to_char(t.application_id) as applicationId, to_char(t.loan_id) as loanId, P.CUSTOMERID customerId, p.applicant_type as applicantTypeId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, to_char(p.document_type) docType, d.document_description docName, p.nch_sendback_remarks remarks, to_char(p.document_no) as docNo, pr.product_name as productName, to_char(t.product_id) as productId, sc.scheme_name as schemeName,t.scheme_id schemeId, t.LOANDATE, to_char(t.loan_amount) as loanAmount, br.branch_name as branch " +
                                    "from TBL_OVERSITE_MASTER t left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id and (p.nch_sendback_flag = 1) left outer join vw_document_master d on p.document_type = d.document_id and t.product_id = d.product_id left outer join product_master pr on pr.product_id = t.product_id left outer join cv_los.scheme_master sc on sc.scheme_id = t.scheme_id and sc.product_id=t.product_id left outer join branch_master br on br.branch_id = t.branch_id where (to_char(t.application_id) = '" + request.searchValue + "' or to_char(t.loan_id) = '" + request.searchValue + "') and p.document_no is not null and p.nch_sendback_to='R'";
                                updateDocumentResponse.PrimaryDataList = new OracleHelper().GetRecords<PrimaryDataProperties>(query);
                                if (updateDocumentResponse.PrimaryDataList.Count > 0)
                                {
                                    updateDocumentResponse.LMS = 1;
                                    response.Data = updateDocumentResponse;
                                    response.responseMsg = "Success";
                                    response.apiStatus = ResponseTypeContants.SUCCESS;
                                    response.status = ApiStatusConstants.COMPLETED;
                                    return response;
                                }
                                else
                                {
                                    updateDocumentResponse.LMS = 1;
                                    response.Data = updateDocumentResponse;
                                    response.responseMsg = "You Have No Permission! ";
                                    response.apiStatus = ResponseTypeContants.FAIL;
                                    response.status = ApiStatusConstants.NOT_COMPLETED;
                                    return response;
                                }

                            }
                            //100823
                            else if (cvrolebcm>0 && cvrolebco>0)
                            {
                                decimal rchsenbackflag = new OracleHelper().ExecuteScalar<decimal>("select tt.rch_sendback from TBL_OVERSITE_MASTER tt where (tt.application_id = '" + request.searchValue + "' or tt.loan_id = '" + request.searchValue + "')");
                                decimal rhosenbackflag = new OracleHelper().ExecuteScalar<decimal>("select tt.roh_sendback from tbl_ops_oversite_master tt where (tt.application_id = '" + request.searchValue + "' or tt.loan_id = '" + request.searchValue + "')");
                                decimal nchsenbackflag = new OracleHelper().ExecuteScalar<decimal>("select tt.nch_sendback from TBL_OVERSITE_MASTER tt where (tt.application_id = '" + request.searchValue + "' or tt.loan_id = '" + request.searchValue + "')");
                                decimal nhosenbackflag = new OracleHelper().ExecuteScalar<decimal>("select tt.nho_sendback from TBL_OVERSITE_MASTER tt where (tt.application_id = '" + request.searchValue + "' or tt.loan_id = '" + request.searchValue + "')");
                                if (rchsenbackflag == 1 && rhosenbackflag == 1)
                                {
                                    query = "select distinct p.customer_name customerName, to_char(t.application_id) as applicationId, to_char(t.loan_id) as loanId, P.CUSTOMERID customerId, p.applicant_type as applicantTypeId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, to_char(p.document_type) docType, d.document_description docName, p.rch_sendback_remarks rchsendbackremarks," +
                                        " p.rho_sendback_remarks rhosendbackremarks, case when to_char(p.old_document_no) is null then '0' else to_char(p.old_document_no) end as docNo, pr.product_name as productName, to_char(t.product_id) as productId, sc.scheme_name as schemeName, t.scheme_id schemeId, t.LOANDATE, to_char(t.loan_amount) as loanAmount, br.branch_name as branch,p.rho_sendback_remarks remarks from TBL_OVERSITE_MASTER t left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id" +
                                        " and t.loan_id = p.loan_id and (p.nch_sendback_flag = 1 OR p.nho_sendback_flag=1 OR P.RCH_SENDBACK_FLAG = 1 OR p.rho_sendback_flag=1) left outer join vw_document_master d on p.document_type = d.document_id and t.product_id = d.product_id left outer join product_master pr on pr.product_id = t.product_id left outer join cv_los.scheme_master sc on sc.scheme_id = t.scheme_id and sc.product_id = t.product_id left outer join branch_master br on " +
                                        "br.branch_id = t.branch_id where (to_char(t.application_id) = '" + request.searchValue + "' or to_char(t.loan_id) = '" + request.searchValue + "') and p.document_no is null union all select distinct p.customer_name customerName, to_char(t.application_id) as applicationId, to_char(t.loan_id) as loanId, P.CUSTOMERID customerId, p.applicant_type as applicantTypeId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType," +
                                        " to_char(p.document_type) docType, case to_char(p.document_type) when '-52' then 'Cibil Report' else d.document_description end docName, p.rch_sendback_remarks rchsendbackremarks, p.rho_sendback_remarks rhosendbackremarks, to_char(p.document_no) as docNo, pr.product_name as productName, to_char(t.product_id) as productId, sc.scheme_name as schemeName, t.scheme_id schemeId, t.LOANDATE, to_char(t.loan_amount) as loanAmount, br.branch_name as branch,p.rho_sendback_remarks remarks from " +
                                        "TBL_OVERSITE_MASTER t left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id and (p.nch_sendback_flag = 1 OR p.nho_sendback_flag=1 OR P.RCH_SENDBACK_FLAG = 1 OR p.rho_sendback_flag = 1) left outer join vw_document_master d on p.document_type = d.document_id and t.product_id = d.product_id left outer join product_master pr on pr.product_id = t.product_id left outer join cv_los.scheme_master sc on sc.scheme_id = t.scheme_id and sc.product_id = t.product_id" +
                                        " left outer join branch_master br on br.branch_id = t.branch_id where (to_char(t.application_id) = '" + request.searchValue + "' or to_char(t.loan_id) = '" + request.searchValue + "') and p.document_no is not null";
                                    updateDocumentResponse.PrimaryDataList = new OracleHelper().GetRecords<PrimaryDataProperties>(query);

                                }
                                else if (rchsenbackflag == 1  )
                                {
                                    query = "select distinct p.customer_name customerName, to_char(t.application_id) as applicationId, to_char(t.loan_id) as loanId, P.CUSTOMERID customerId, p.applicant_type as applicantTypeId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, to_char(p.document_type) docType, d.document_description docName, p.rch_sendback_remarks remarks, case when to_char(p.old_document_no) is null then '0' else to_char(p.old_document_no) end as docNo, pr.product_name as productName, to_char(t.product_id) as productId, sc.scheme_name as schemeName,t.scheme_id schemeId, t.LOANDATE, to_char(t.loan_amount) as loanAmount, " +
                                            "br.branch_name as branch from TBL_OVERSITE_MASTER t left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id and (p.nch_sendback_flag = 1  OR P.RCH_SENDBACK_FLAG=1) left outer join vw_document_master d on p.document_type = d.document_id and t.product_id = d.product_id left outer join product_master pr on pr.product_id = t.product_id left outer join cv_los.scheme_master sc on sc.scheme_id = t.scheme_id and sc.product_id=t.product_id left outer join branch_master br on br.branch_id = t.branch_id where (to_char(t.application_id) = '" + request.searchValue + "' or to_char(t.loan_id) = '" + request.searchValue + "') " +
                                            "and p.document_no is null union all select distinct p.customer_name customerName, to_char(t.application_id) as applicationId, to_char(t.loan_id) as loanId, P.CUSTOMERID customerId, p.applicant_type as applicantTypeId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, to_char(p.document_type) docType, d.document_description docName, p.rch_sendback_remarks remarks, to_char(p.document_no) as docNo, pr.product_name as productName, to_char(t.product_id) as productId, sc.scheme_name as schemeName,t.scheme_id schemeId, t.LOANDATE, to_char(t.loan_amount) as loanAmount, br.branch_name as branch " +
                                            "from TBL_OVERSITE_MASTER t left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id and (p.nch_sendback_flag = 1  OR P.RCH_SENDBACK_FLAG=1) left outer join vw_document_master d on p.document_type = d.document_id and t.product_id = d.product_id left outer join product_master pr on pr.product_id = t.product_id left outer join cv_los.scheme_master sc on sc.scheme_id = t.scheme_id and sc.product_id=t.product_id left outer join branch_master br on br.branch_id = t.branch_id where (to_char(t.application_id) = '" + request.searchValue + "' or to_char(t.loan_id) = '" + request.searchValue + "') and p.document_no is not null";
                                    updateDocumentResponse.PrimaryDataList = new OracleHelper().GetRecords<PrimaryDataProperties>(query);

                                }
                                else if (rhosenbackflag == 1)
                                {
                                    query = "select distinct p.customer_name customerName, to_char(t.application_id) as applicationId, to_char(t.loan_id) as loanId, P.CUSTOMERID customerId, p.applicant_type as applicantTypeId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, to_char(p.document_type) docType, case to_char(p.document_type) when '-52' then 'Cibil Report' else d.document_description end docName, p.rho_sendback_remarks remarks, case when to_char(p.old_document_no) is null then '0' else to_char(p.old_document_no) end as docNo, pr.product_name as productName, to_char(t.product_id) as productId, sc.scheme_name as schemeName,t.scheme_id schemeId, t.LOANDATE, to_char(t.loan_amount) as loanAmount, " +
                                            "br.branch_name as branch from TBL_OVERSITE_MASTER t left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id and p.rho_sendback_flag=1 left outer join vw_document_master d on p.document_type = d.document_id and t.product_id = d.product_id left outer join product_master pr on pr.product_id = t.product_id left outer join cv_los.scheme_master sc on sc.scheme_id = t.scheme_id and sc.product_id=t.product_id left outer join branch_master br on br.branch_id = t.branch_id where (to_char(t.application_id) = '" + request.searchValue + "' or to_char(t.loan_id) = '" + request.searchValue + "') " +
                                            "and p.document_no is null union all select distinct p.customer_name customerName, to_char(t.application_id) as applicationId, to_char(t.loan_id) as loanId, P.CUSTOMERID customerId, p.applicant_type as applicantTypeId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, to_char(p.document_type) docType, case to_char(p.document_type) when '-52' then 'Cibil Report' else d.document_description end docName, p.rho_sendback_remarks remarks, to_char(p.document_no) as docNo, pr.product_name as productName, to_char(t.product_id) as productId, sc.scheme_name as schemeName,t.scheme_id schemeId, t.LOANDATE, to_char(t.loan_amount) as loanAmount, br.branch_name as branch " +
                                            "from TBL_OVERSITE_MASTER t left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id and p.rho_sendback_flag=1 left outer join vw_document_master d on p.document_type = d.document_id and t.product_id = d.product_id left outer join product_master pr on pr.product_id = t.product_id left outer join cv_los.scheme_master sc on sc.scheme_id = t.scheme_id and sc.product_id=t.product_id left outer join branch_master br on br.branch_id = t.branch_id where (to_char(t.application_id) = '" + request.searchValue + "' or to_char(t.loan_id) = '" + request.searchValue + "') and p.document_no is not null";
                                    updateDocumentResponse.PrimaryDataList = new OracleHelper().GetRecords<PrimaryDataProperties>(query);

                                }

                                if (nhosenbackflag == 1 && nchsenbackflag == 1)
                                {
                                    query = "select distinct p.customer_name customerName, to_char(t.application_id) as applicationId, to_char(t.loan_id) as loanId, P.CUSTOMERID customerId, p.applicant_type as applicantTypeId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, to_char(p.document_type) docType, case to_char(p.document_type) when '-52' then 'Cibil Report' else d.document_description end docName, p.nch_sendback_remarks remarks, case when to_char(p.old_document_no) is null then '0' else to_char(p.old_document_no) end as docNo, pr.product_name as productName, to_char(t.product_id) as productId, sc.scheme_name as schemeName, t.scheme_id schemeId, t.LOANDATE, to_char(t.loan_amount) as loanAmount, " +
                                        "br.branch_name as branch from TBL_OVERSITE_MASTER t left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id and (p.nch_sendback_flag = 1 OR p.nho_sendback_flag=1 OR p.rho_sendback_flag=1 OR P.RCH_SENDBACK_FLAG = 1) left outer join vw_document_master d on p.document_type = d.document_id and t.product_id = d.product_id left outer join product_master pr on pr.product_id = t.product_id left outer join cv_los.scheme_master sc on sc.scheme_id = t.scheme_id and sc.product_id = t.product_id left outer join branch_master br on br.branch_id = t.branch_id where (to_char(t.application_id) = '" + request.searchValue + "' or to_char(t.loan_id) = '" + request.searchValue + "') and p.document_no is null " +
                                        " union all select distinct p.customer_name customerName, to_char(t.application_id) as applicationId, to_char(t.loan_id) as loanId, P.CUSTOMERID customerId, p.applicant_type as applicantTypeId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, to_char(p.document_type) docType, case to_char(p.document_type) when '-52' then 'Cibil Report' else d.document_description end docName, p.nch_sendback_remarks remarks, to_char(p.document_no) as docNo, pr.product_name as productName, to_char(t.product_id) as productId, sc.scheme_name as schemeName, t.scheme_id schemeId, t.LOANDATE, to_char(t.loan_amount) as loanAmount, br.branch_name as branch from" +
                                        " TBL_OVERSITE_MASTER t left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id and (p.nch_sendback_flag = 1 OR p.nho_sendback_flag=1 OR p.rho_sendback_flag=1 OR P.RCH_SENDBACK_FLAG = 1) left outer join vw_document_master d on p.document_type = d.document_id and t.product_id = d.product_id left outer join product_master pr on pr.product_id = t.product_id left outer join cv_los.scheme_master sc on sc.scheme_id = t.scheme_id and sc.product_id = t.product_id left outer join branch_master br on br.branch_id = t.branch_id where (to_char(t.application_id) = '" + request.searchValue + "' or to_char(t.loan_id) = '" + request.searchValue + "') and p.document_no is not null ";
                                    updateDocumentResponse.PrimaryDataList = new OracleHelper().GetRecords<PrimaryDataProperties>(query);

                                }
                                else if(nhosenbackflag == 1)
                                {
                                    query = "select distinct p.customer_name customerName, to_char(t.application_id) as applicationId, to_char(t.loan_id) as loanId, P.CUSTOMERID customerId, p.applicant_type as applicantTypeId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, to_char(p.document_type) docType, case to_char(p.document_type) when '-52' then 'Cibil Report' else d.document_description end docName, p.sendback_remarks remarks,case when to_char(p.old_document_no) is null then '0' else to_char(p.old_document_no) end as docNo, pr.product_name as productName, to_char(t.product_id) as productId, sc.scheme_name as schemeName,t.scheme_id schemeId, t.LOANDATE, to_char(t.loan_amount) as loanAmount, " +
                                       "br.branch_name as branch from TBL_OVERSITE_MASTER t left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id and (p.nho_sendback_flag = 1 OR P.rho_Sendback_Flag = 1) left outer join vw_document_master d on p.document_type = d.document_id and t.product_id = d.product_id left outer join product_master pr on pr.product_id = t.product_id left outer join cv_los.scheme_master sc on sc.scheme_id = t.scheme_id and sc.product_id=t.product_id left outer join branch_master br on br.branch_id = t.branch_id where (to_char(t.application_id) = '" + request.searchValue + "' or to_char(t.loan_id) = '" + request.searchValue + "') " +
                                       "and p.document_no is null  union all select distinct p.customer_name customerName, to_char(t.application_id) as applicationId, to_char(t.loan_id) as loanId, P.CUSTOMERID customerId, p.applicant_type as applicantTypeId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, to_char(p.document_type) docType, case to_char(p.document_type) when '-52' then 'Cibil Report' else d.document_description end docName, p.sendback_remarks remarks, to_char(p.document_no) as docNo, pr.product_name as productName, to_char(t.product_id) as productId, sc.scheme_name as schemeName,t.scheme_id schemeId, t.LOANDATE, to_char(t.loan_amount) as loanAmount, br.branch_name as branch " +
                                       "from TBL_OVERSITE_MASTER t left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id and (p.nho_sendback_flag = 1 OR P.rho_Sendback_Flag = 1) left outer join vw_document_master d on p.document_type = d.document_id and t.product_id = d.product_id left outer join product_master pr on pr.product_id = t.product_id left outer join cv_los.scheme_master sc on sc.scheme_id = t.scheme_id and sc.product_id=t.product_id left outer join branch_master br on br.branch_id = t.branch_id where (to_char(t.application_id) = '" + request.searchValue + "' or to_char(t.loan_id) = '" + request.searchValue + "') and p.document_no is not null ";
                                    updateDocumentResponse.PrimaryDataList = new OracleHelper().GetRecords<PrimaryDataProperties>(query);

                                }
                                else if(nchsenbackflag == 1)
                                {
                                    query = "select distinct p.customer_name customerName, to_char(t.application_id) as applicationId, to_char(t.loan_id) as loanId, P.CUSTOMERID customerId, p.applicant_type as applicantTypeId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, to_char(p.document_type) docType, d.document_description docName, p.nch_sendback_remarks remarks,case when to_char(p.old_document_no) is null then '0' else to_char(p.old_document_no) end as docNo, pr.product_name as productName, to_char(t.product_id) as productId, sc.scheme_name as schemeName,t.scheme_id schemeId, t.LOANDATE, to_char(t.loan_amount) as loanAmount, " +
                                       "br.branch_name as branch from TBL_OVERSITE_MASTER t left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id and (p.nch_sendback_flag = 1  OR P.RCH_SENDBACK_FLAG=1) left outer join vw_document_master d on p.document_type = d.document_id and t.product_id = d.product_id left outer join product_master pr on pr.product_id = t.product_id left outer join cv_los.scheme_master sc on sc.scheme_id = t.scheme_id and sc.product_id=t.product_id left outer join branch_master br on br.branch_id = t.branch_id where (to_char(t.application_id) = '" + request.searchValue + "' or to_char(t.loan_id) = '" + request.searchValue + "') " +
                                       "and p.document_no is null and p.nch_sendback_to='B' union all select distinct p.customer_name customerName, to_char(t.application_id) as applicationId, to_char(t.loan_id) as loanId, P.CUSTOMERID customerId, p.applicant_type as applicantTypeId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, to_char(p.document_type) docType, d.document_description docName, p.nch_sendback_remarks remarks, to_char(p.document_no) as docNo, pr.product_name as productName, to_char(t.product_id) as productId, sc.scheme_name as schemeName,t.scheme_id schemeId, t.LOANDATE, to_char(t.loan_amount) as loanAmount, br.branch_name as branch " +
                                       "from TBL_OVERSITE_MASTER t left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id and (p.nch_sendback_flag = 1  OR P.RCH_SENDBACK_FLAG=1) left outer join vw_document_master d on p.document_type = d.document_id and t.product_id = d.product_id left outer join product_master pr on pr.product_id = t.product_id left outer join cv_los.scheme_master sc on sc.scheme_id = t.scheme_id and sc.product_id=t.product_id left outer join branch_master br on br.branch_id = t.branch_id where (to_char(t.application_id) = '" + request.searchValue + "' or to_char(t.loan_id) = '" + request.searchValue + "') and p.document_no is not null and p.nch_sendback_to='B'";
                                    updateDocumentResponse.PrimaryDataList = new OracleHelper().GetRecords<PrimaryDataProperties>(query);

                                }
                                if (updateDocumentResponse.PrimaryDataList != null)
                                {
                                    if (updateDocumentResponse.PrimaryDataList.Count > 0)
                                    {
                                        updateDocumentResponse.LMS = 1;
                                        response.Data = updateDocumentResponse;
                                        response.responseMsg = "Success";
                                        response.apiStatus = ResponseTypeContants.SUCCESS;
                                        response.status = ApiStatusConstants.COMPLETED;
                                        return response;
                                    }
                                    else
                                    {
                                        updateDocumentResponse.LMS = 1;
                                        response.Data = updateDocumentResponse;
                                        response.responseMsg = "You Have No Permission! ";
                                        response.apiStatus = ResponseTypeContants.FAIL;
                                        response.status = ApiStatusConstants.NOT_COMPLETED;
                                        return response;
                                    }
                                }
                                else
                                {
                                    updateDocumentResponse.LMS = 1;
                                    response.Data = updateDocumentResponse;
                                    response.responseMsg = "Please check Application ID !!!";
                                    response.apiStatus = ResponseTypeContants.FAIL;
                                    response.status = ApiStatusConstants.NOT_COMPLETED;
                                    return response;
                                }



                            }
                            //100823

                            else if (cvrolebcm > 0 && cvrolebco==0)
                            {
                                decimal rchsenbackflag = new OracleHelper().ExecuteScalar<decimal>("select tt.rch_sendback from TBL_OVERSITE_MASTER tt where (tt.application_id = '" + request.searchValue + "' or tt.loan_id = '" + request.searchValue + "')");
                                decimal nchsenbackflag = new OracleHelper().ExecuteScalar<decimal>("select tt.nch_sendback from TBL_OVERSITE_MASTER tt where (tt.application_id = '" + request.searchValue + "' or tt.loan_id = '" + request.searchValue + "')");

                                if (rchsenbackflag == 1)
                                {
                                    query = "select distinct p.customer_name customerName, to_char(t.application_id) as applicationId, to_char(t.loan_id) as loanId, P.CUSTOMERID customerId, p.applicant_type as applicantTypeId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, to_char(p.document_type) docType, d.document_description docName, p.rch_sendback_remarks remarks, case when to_char(p.old_document_no) is null then '0' else to_char(p.old_document_no) end as docNo, pr.product_name as productName, to_char(t.product_id) as productId, sc.scheme_name as schemeName,t.scheme_id schemeId, t.LOANDATE, to_char(t.loan_amount) as loanAmount, " +
                                            "br.branch_name as branch from TBL_OVERSITE_MASTER t left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id and (p.nch_sendback_flag = 1  OR P.RCH_SENDBACK_FLAG=1) left outer join vw_document_master d on p.document_type = d.document_id and t.product_id = d.product_id left outer join product_master pr on pr.product_id = t.product_id left outer join cv_los.scheme_master sc on sc.scheme_id = t.scheme_id and sc.product_id=t.product_id left outer join branch_master br on br.branch_id = t.branch_id where (to_char(t.application_id) = '" + request.searchValue + "' or to_char(t.loan_id) = '" + request.searchValue + "') " +
                                            "and p.document_no is null union all select distinct p.customer_name customerName, to_char(t.application_id) as applicationId, to_char(t.loan_id) as loanId, P.CUSTOMERID customerId, p.applicant_type as applicantTypeId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, to_char(p.document_type) docType, d.document_description docName, p.rch_sendback_remarks remarks, to_char(p.document_no) as docNo, pr.product_name as productName, to_char(t.product_id) as productId, sc.scheme_name as schemeName,t.scheme_id schemeId, t.LOANDATE, to_char(t.loan_amount) as loanAmount, br.branch_name as branch " +
                                            "from TBL_OVERSITE_MASTER t left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id and (p.nch_sendback_flag = 1  OR P.RCH_SENDBACK_FLAG=1) left outer join vw_document_master d on p.document_type = d.document_id and t.product_id = d.product_id left outer join product_master pr on pr.product_id = t.product_id left outer join cv_los.scheme_master sc on sc.scheme_id = t.scheme_id and sc.product_id=t.product_id left outer join branch_master br on br.branch_id = t.branch_id where (to_char(t.application_id) = '" + request.searchValue + "' or to_char(t.loan_id) = '" + request.searchValue + "') and p.document_no is not null";
                                    updateDocumentResponse.PrimaryDataList = new OracleHelper().GetRecords<PrimaryDataProperties>(query);
                                }
                                if (nchsenbackflag == 1)
                                {
                                    query = "select distinct p.customer_name customerName, to_char(t.application_id) as applicationId, to_char(t.loan_id) as loanId, P.CUSTOMERID customerId, p.applicant_type as applicantTypeId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, to_char(p.document_type) docType, d.document_description docName, p.nch_sendback_remarks remarks,case when to_char(p.old_document_no) is null then '0' else to_char(p.old_document_no) end as docNo, pr.product_name as productName, to_char(t.product_id) as productId, sc.scheme_name as schemeName,t.scheme_id schemeId, t.LOANDATE, to_char(t.loan_amount) as loanAmount, " +
                                        "br.branch_name as branch from TBL_OVERSITE_MASTER t left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id and (p.nch_sendback_flag = 1  OR P.RCH_SENDBACK_FLAG=1) left outer join vw_document_master d on p.document_type = d.document_id and t.product_id = d.product_id left outer join product_master pr on pr.product_id = t.product_id left outer join cv_los.scheme_master sc on sc.scheme_id = t.scheme_id and sc.product_id=t.product_id left outer join branch_master br on br.branch_id = t.branch_id where (to_char(t.application_id) = '" + request.searchValue + "' or to_char(t.loan_id) = '" + request.searchValue + "') " +
                                        "and p.document_no is null and p.nch_sendback_to='B' union all select distinct p.customer_name customerName, to_char(t.application_id) as applicationId, to_char(t.loan_id) as loanId, P.CUSTOMERID customerId, p.applicant_type as applicantTypeId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, to_char(p.document_type) docType, d.document_description docName, p.nch_sendback_remarks remarks, to_char(p.document_no) as docNo, pr.product_name as productName, to_char(t.product_id) as productId, sc.scheme_name as schemeName,t.scheme_id schemeId, t.LOANDATE, to_char(t.loan_amount) as loanAmount, br.branch_name as branch " +
                                        "from TBL_OVERSITE_MASTER t left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id and (p.nch_sendback_flag = 1  OR P.RCH_SENDBACK_FLAG=1) left outer join vw_document_master d on p.document_type = d.document_id and t.product_id = d.product_id left outer join product_master pr on pr.product_id = t.product_id left outer join cv_los.scheme_master sc on sc.scheme_id = t.scheme_id and sc.product_id=t.product_id left outer join branch_master br on br.branch_id = t.branch_id where (to_char(t.application_id) = '" + request.searchValue + "' or to_char(t.loan_id) = '" + request.searchValue + "') and p.document_no is not null and p.nch_sendback_to='B'";
                                    updateDocumentResponse.PrimaryDataList = new OracleHelper().GetRecords<PrimaryDataProperties>(query);
                                }
                                if (updateDocumentResponse.PrimaryDataList == null)
                                {

                                    response.Data = null;
                                    response.responseMsg = "This File Is Sendbacked By NHO So BCH Cant Access This File";
                                    response.apiStatus = ResponseTypeContants.FAIL;
                                    response.status = ApiStatusConstants.NOT_COMPLETED;
                                    return response;


                                }
                                if (updateDocumentResponse.PrimaryDataList.Count > 0)
                                {
                                    updateDocumentResponse.LMS = 1;
                                    response.Data = updateDocumentResponse;
                                    response.responseMsg = "Success";
                                    response.apiStatus = ResponseTypeContants.SUCCESS;
                                    response.status = ApiStatusConstants.COMPLETED;
                                    return response;
                                }
                                else
                                {
                                    updateDocumentResponse.LMS = 1;
                                    response.Data = updateDocumentResponse;
                                    response.responseMsg = "You Have No Permission! ";
                                    response.apiStatus = ResponseTypeContants.FAIL;
                                    response.status = ApiStatusConstants.NOT_COMPLETED;
                                    return response;
                                }

                            }
                            else if (cvrolebco > 0 && cvrolebcm==0)
                            {
                                decimal rhosenbackflag = new OracleHelper().ExecuteScalar<decimal>("select tt.roh_sendback from tbl_ops_oversite_master tt where (tt.application_id = '" + request.searchValue + "' or tt.loan_id = '" + request.searchValue + "')");
                                decimal nhosenbackflag = new OracleHelper().ExecuteScalar<decimal>("select tt.nho_sendback from TBL_OVERSITE_MASTER tt where (tt.application_id = '" + request.searchValue + "' or tt.loan_id = '" + request.searchValue + "')");
                                if (rhosenbackflag == 1)
                                {
                                    query = "select distinct p.customer_name customerName, to_char(t.application_id) as applicationId, to_char(t.loan_id) as loanId, P.CUSTOMERID customerId, p.applicant_type as applicantTypeId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, to_char(p.document_type) docType, case to_char(p.document_type) when '-52' then 'Cibil Report' else d.document_description end docName, p.rho_sendback_remarks remarks, case when to_char(p.old_document_no) is null then '0' else to_char(p.old_document_no) end as docNo, pr.product_name as productName, to_char(t.product_id) as productId, sc.scheme_name as schemeName,t.scheme_id schemeId, t.LOANDATE, to_char(t.loan_amount) as loanAmount, " +
                                            "br.branch_name as branch from TBL_OVERSITE_MASTER t left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id and p.rho_sendback_flag=1 left outer join vw_document_master d on p.document_type = d.document_id and t.product_id = d.product_id left outer join product_master pr on pr.product_id = t.product_id left outer join cv_los.scheme_master sc on sc.scheme_id = t.scheme_id and sc.product_id=t.product_id left outer join branch_master br on br.branch_id = t.branch_id where (to_char(t.application_id) = '" + request.searchValue + "' or to_char(t.loan_id) = '" + request.searchValue + "') " +
                                            "and p.document_no is null union all select distinct p.customer_name customerName, to_char(t.application_id) as applicationId, to_char(t.loan_id) as loanId, P.CUSTOMERID customerId, p.applicant_type as applicantTypeId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, to_char(p.document_type) docType, case to_char(p.document_type) when '-52' then 'Cibil Report' else d.document_description end docName, p.rho_sendback_remarks remarks, to_char(p.document_no) as docNo, pr.product_name as productName, to_char(t.product_id) as productId, sc.scheme_name as schemeName,t.scheme_id schemeId, t.LOANDATE, to_char(t.loan_amount) as loanAmount, br.branch_name as branch " +
                                            "from TBL_OVERSITE_MASTER t left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id and p.rho_sendback_flag=1 left outer join vw_document_master d on p.document_type = d.document_id and t.product_id = d.product_id left outer join product_master pr on pr.product_id = t.product_id left outer join cv_los.scheme_master sc on sc.scheme_id = t.scheme_id and sc.product_id=t.product_id left outer join branch_master br on br.branch_id = t.branch_id where (to_char(t.application_id) = '" + request.searchValue + "' or to_char(t.loan_id) = '" + request.searchValue + "') and p.document_no is not null";
                                    updateDocumentResponse.PrimaryDataList = new OracleHelper().GetRecords<PrimaryDataProperties>(query);
                                }
                                if (nhosenbackflag == 1)
                                {
                                    query = "select distinct p.customer_name customerName, to_char(t.application_id) as applicationId, to_char(t.loan_id) as loanId, P.CUSTOMERID customerId, p.applicant_type as applicantTypeId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, to_char(p.document_type) docType, case to_char(p.document_type) when '-52' then 'Cibil Report' else d.document_description end docName, p.sendback_remarks remarks,case when to_char(p.old_document_no) is null then '0' else to_char(p.old_document_no) end as docNo, pr.product_name as productName, to_char(t.product_id) as productId, sc.scheme_name as schemeName,t.scheme_id schemeId, t.LOANDATE, to_char(t.loan_amount) as loanAmount, " +
                                        "br.branch_name as branch from TBL_OVERSITE_MASTER t left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id and (p.nho_sendback_flag = 1 OR P.rho_Sendback_Flag = 1) left outer join vw_document_master d on p.document_type = d.document_id and t.product_id = d.product_id left outer join product_master pr on pr.product_id = t.product_id left outer join cv_los.scheme_master sc on sc.scheme_id = t.scheme_id and sc.product_id=t.product_id left outer join branch_master br on br.branch_id = t.branch_id where (to_char(t.application_id) = '" + request.searchValue + "' or to_char(t.loan_id) = '" + request.searchValue + "') " +
                                        "and p.document_no is null union all select distinct p.customer_name customerName, to_char(t.application_id) as applicationId, to_char(t.loan_id) as loanId, P.CUSTOMERID customerId, p.applicant_type as applicantTypeId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, to_char(p.document_type) docType, case to_char(p.document_type) when '-52' then 'Cibil Report' else d.document_description end docName, p.sendback_remarks remarks, to_char(p.document_no) as docNo, pr.product_name as productName, to_char(t.product_id) as productId, sc.scheme_name as schemeName,t.scheme_id schemeId, t.LOANDATE, to_char(t.loan_amount) as loanAmount, br.branch_name as branch " +
                                        "from TBL_OVERSITE_MASTER t left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id and (p.nho_sendback_flag = 1 OR P.rho_Sendback_Flag = 1) left outer join vw_document_master d on p.document_type = d.document_id and t.product_id = d.product_id left outer join product_master pr on pr.product_id = t.product_id left outer join cv_los.scheme_master sc on sc.scheme_id = t.scheme_id and sc.product_id=t.product_id left outer join branch_master br on br.branch_id = t.branch_id where (to_char(t.application_id) = '" + request.searchValue + "' or to_char(t.loan_id) = '" + request.searchValue + "') and p.document_no is not null ";
                                    updateDocumentResponse.PrimaryDataList = new OracleHelper().GetRecords<PrimaryDataProperties>(query);
                                }
                                if (updateDocumentResponse.PrimaryDataList == null)
                                {

                                    response.Data = null;
                                    response.responseMsg = "This File Is Sendbacked By NCH So BCO Cant Access This File";
                                    response.apiStatus = ResponseTypeContants.FAIL;
                                    response.status = ApiStatusConstants.NOT_COMPLETED;
                                    return response;



                                }
                                if (updateDocumentResponse.PrimaryDataList.Count > 0)
                                {
                                    updateDocumentResponse.LMS = 1;
                                    response.Data = updateDocumentResponse;
                                    response.responseMsg = "Success";
                                    response.apiStatus = ResponseTypeContants.SUCCESS;
                                    response.status = ApiStatusConstants.COMPLETED;
                                    return response;
                                }
                                else
                                {
                                    updateDocumentResponse.LMS = 1;
                                    response.Data = updateDocumentResponse;
                                    response.responseMsg = "You Have No Permission! ";
                                    response.apiStatus = ResponseTypeContants.FAIL;
                                    response.status = ApiStatusConstants.NOT_COMPLETED;
                                    return response;
                                }

                            }

                            else
                            {
                                response.Data = null;
                                response.responseMsg = "You Have No Permission!";
                                response.apiStatus = ResponseTypeContants.FAIL;
                                response.status = ApiStatusConstants.NOT_COMPLETED;
                            }
                        }
                        else
                        {
                            if (cvrolerm > 0 /*|| rolecrm > 0*/)
                            {
                                query = "select distinct p.customer_name customerName, to_char(t.application_id) as applicationId, to_char(t.loan_id) as loanId, P.CUSTOMERID customerId, p.applicant_type as applicantTypeId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, to_char(p.document_type) docType, d.document_description docName, p.sendback_remarks remarks, to_char(p.old_document_no) as docNo, pr.product_name as productName, to_char(t.product_id) as productId, sc.scheme_name as schemeName,t.scheme_id schemeId, t.LOANDATE, to_char(t.loan_amount) as loanAmount, " +
                               "br.branch_name as branch from TBL_OVERSITE_MASTER t left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id and (p.nch_sendback_flag = 1 or p.nho_sendback_flag = 1 ) left outer join vw_document_master d on p.document_type = d.document_id and t.product_id = d.product_id left outer join product_master pr on pr.product_id = t.product_id left outer join vw_scheme_master sc on sc.scheme_id = t.scheme_id and sc.product_id=t.product_id left outer join branch_master br on br.branch_id = t.branch_id where (to_char(t.application_id) = '" + request.searchValue + "' or to_char(t.loan_id) = '" + request.searchValue + "') " +
                              "and p.document_no is null union all select distinct p.customer_name customerName, to_char(t.application_id) as applicationId, to_char(t.loan_id) as loanId, P.CUSTOMERID customerId, p.applicant_type as applicantTypeId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, to_char(p.document_type) docType, d.document_description docName, p.sendback_remarks remarks, to_char(p.document_no) as docNo, pr.product_name as productName, to_char(t.product_id) as productId, sc.scheme_name as schemeName,t.scheme_id schemeId, t.LOANDATE, to_char(t.loan_amount) as loanAmount, br.branch_name as branch " +
                               "from TBL_OVERSITE_MASTER t left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id and (p.nch_sendback_flag = 1 or p.nho_sendback_flag = 1) left outer join vw_document_master d on p.document_type = d.document_id and t.product_id = d.product_id left outer join product_master pr on pr.product_id = t.product_id left outer join vw_scheme_master sc on sc.scheme_id = t.scheme_id and sc.product_id=t.product_id left outer join branch_master br on br.branch_id = t.branch_id where (to_char(t.application_id) = '" + request.searchValue + "' or to_char(t.loan_id) = '" + request.searchValue + "') and p.document_no is not null";
                                updateDocumentResponse.PrimaryDataList = new OracleHelper().GetRecords<PrimaryDataProperties>(query);
                                updateDocumentResponse.LMS = 1;
                                response.Data = updateDocumentResponse;
                                response.responseMsg = "Success";
                                response.apiStatus = ResponseTypeContants.SUCCESS;
                                response.status = ApiStatusConstants.COMPLETED;
                                return response;
                            }
                            //else if (countnho > 0)//nho
                            else if (/*rolerho > 0 ||*/ cvrolerho > 0)
                            {
                                //query = "select t.customer_name customerName, case t.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, p.document_type documentType, d.document_description documentStatus, p.doc_remarks remarks from TBL_OVERSITE_MASTER t left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id left outer join document_master d on p.document_type = d.document_id and t.product_id = d.product_id where (t.application_id = " + request.searchValue + " or t.loan_id = " + request.searchValue + ")";
                                query = "select distinct p.customer_name customerName, to_char(t.application_id) as applicationId, to_char(t.loan_id) as loanId, P.CUSTOMERID customerId, p.applicant_type as applicantTypeId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, to_char(p.document_type) docType, d.document_description docName, p.sendback_remarks remarks, to_char(p.old_document_no) as docNo, pr.product_name as productName, to_char(t.product_id) as productId, sc.scheme_name as schemeName,t.scheme_id schemeId, t.LOANDATE, to_char(t.loan_amount) as loanAmount, br.branch_name as branch from TBL_OVERSITE_MASTER t " +
                                    "left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id and ( p.nho_sendback_flag = 1) left outer join vw_document_master d on p.document_type = d.document_id and t.product_id = d.product_id left outer join product_master pr on pr.product_id = t.product_id left outer join vw_scheme_master sc on sc.scheme_id = t.scheme_id and sc.product_id=t.product_id left outer join branch_master br on br.branch_id = t.branch_id where (to_char(t.application_id) = '" + request.searchValue + "' or to_char(t.loan_id) = '" + request.searchValue + "') and p.document_no is null union all select distinct p.customer_name customerName, to_char(t.application_id) as applicationId, " +
                                    "to_char(t.loan_id) as loanId, P.CUSTOMERID customerId, p.applicant_type as applicantTypeId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, to_char(p.document_type) docType, d.document_description docName, p.sendback_remarks remarks, to_char(p.document_no) as docNo, pr.product_name as productName, to_char(t.product_id) as productId, sc.scheme_name as schemeName,t.scheme_id schemeId, t.LOANDATE, to_char(t.loan_amount) as loanAmount, br.branch_name as branch from TBL_OVERSITE_MASTER t left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id and (p.nho_sendback_flag = 1) " +
                                    "left outer join vw_document_master d on p.document_type = d.document_id and t.product_id = d.product_id left outer join product_master pr on pr.product_id = t.product_id left outer join vw_scheme_master sc on sc.scheme_id = t.scheme_id and sc.product_id=t.product_id left outer join branch_master br on br.branch_id = t.branch_id where (to_char(t.application_id) = '" + request.searchValue + "' or to_char(t.loan_id) = '" + request.searchValue + "') and p.document_no is not null";
                                updateDocumentResponse.PrimaryDataList = new OracleHelper().GetRecords<PrimaryDataProperties>(query);
                                //PrimaryDataList.Add(PrimaryDataList1);
                                updateDocumentResponse.LMS = 1;
                                response.Data = updateDocumentResponse;
                                response.responseMsg = "Success";
                                response.apiStatus = ResponseTypeContants.SUCCESS;
                                response.status = ApiStatusConstants.COMPLETED;
                                return response;
                            }
                            // else if (count1 > 0)//nch
                            else if (cvrolerch > 0 /*|| rolecrch > 0*/)
                            {
                                query = "select distinct p.customer_name customerName, to_char(t.application_id) as applicationId, to_char(t.loan_id) as loanId, P.CUSTOMERID customerId, p.applicant_type as applicantTypeId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, to_char(p.document_type) docType, d.document_description docName, p.sendback_remarks remarks, to_char(p.old_document_no) as docNo, pr.product_name as productName, to_char(t.product_id) as productId, sc.scheme_name as schemeName,t.scheme_id schemeId, t.LOANDATE, to_char(t.loan_amount) as loanAmount, " +
                                    "br.branch_name as branch from TBL_OVERSITE_MASTER t left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id and (p.nch_sendback_flag = 1) left outer join vw_document_master d on p.document_type = d.document_id and t.product_id = d.product_id left outer join product_master pr on pr.product_id = t.product_id left outer join vw_scheme_master sc on sc.scheme_id = t.scheme_id and sc.product_id=t.product_id left outer join branch_master br on br.branch_id = t.branch_id where (to_char(t.application_id) = '" + request.searchValue + "' or to_char(t.loan_id) = '" + request.searchValue + "') " +
                                    "and p.document_no is null and p.nch_sendback_to='R' union all select distinct p.customer_name customerName, to_char(t.application_id) as applicationId, to_char(t.loan_id) as loanId, P.CUSTOMERID customerId, p.applicant_type as applicantTypeId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, to_char(p.document_type) docType, d.document_description docName, p.sendback_remarks remarks, to_char(p.document_no) as docNo, pr.product_name as productName, to_char(t.product_id) as productId, sc.scheme_name as schemeName,t.scheme_id schemeId, t.LOANDATE, to_char(t.loan_amount) as loanAmount, br.branch_name as branch " +
                                    "from TBL_OVERSITE_MASTER t left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id and (p.nch_sendback_flag = 1) left outer join vw_document_master d on p.document_type = d.document_id and t.product_id = d.product_id left outer join product_master pr on pr.product_id = t.product_id left outer join vw_scheme_master sc on sc.scheme_id = t.scheme_id and sc.product_id=t.product_id left outer join branch_master br on br.branch_id = t.branch_id where (to_char(t.application_id) = '" + request.searchValue + "' or to_char(t.loan_id) = '" + request.searchValue + "') and p.document_no is not null and p.nch_sendback_to='R'";
                                updateDocumentResponse.PrimaryDataList = new OracleHelper().GetRecords<PrimaryDataProperties>(query);
                                if (updateDocumentResponse.PrimaryDataList.Count > 0)
                                {
                                    updateDocumentResponse.LMS = 1;
                                    response.Data = updateDocumentResponse;
                                    response.responseMsg = "Success";
                                    response.apiStatus = ResponseTypeContants.SUCCESS;
                                    response.status = ApiStatusConstants.COMPLETED;
                                    return response;
                                }
                                else
                                {
                                    updateDocumentResponse.LMS = 1;
                                    response.Data = updateDocumentResponse;
                                    response.responseMsg = "You Have No Permission! ";
                                    response.apiStatus = ResponseTypeContants.FAIL;
                                    response.status = ApiStatusConstants.NOT_COMPLETED;
                                    return response;
                                }

                            }

                            //100823
                            else if (cvrolebcm >0 && cvrolebco>0)
                            {
                                decimal rchsenbackflag = new OracleHelper().ExecuteScalar<decimal>("select tt.rch_sendback from TBL_OVERSITE_MASTER tt where (tt.application_id = '" + request.searchValue + "' or tt.loan_id = '" + request.searchValue + "')");
                                decimal rhosenbackflag = new OracleHelper().ExecuteScalar<decimal>("select tt.roh_sendback from tbl_ops_oversite_master tt where (tt.application_id = '" + request.searchValue + "' or tt.loan_id = '" + request.searchValue + "')");

                                decimal nchsenbackflag = new OracleHelper().ExecuteScalar<decimal>("select tt.nch_sendback from TBL_OVERSITE_MASTER tt where (tt.application_id = '" + request.searchValue + "' or tt.loan_id = '" + request.searchValue + "')");
                                decimal nhosenbackflag = new OracleHelper().ExecuteScalar<decimal>("select tt.nho_sendback from TBL_OVERSITE_MASTER tt where (tt.application_id = '" + request.searchValue + "' or tt.loan_id = '" + request.searchValue + "')");

                                if (rchsenbackflag == 1 && rhosenbackflag == 1)
                                {
                                    query = "select distinct p.customer_name customerName, to_char(t.application_id) as applicationId, to_char(t.loan_id) as loanId, P.CUSTOMERID customerId, p.applicant_type as applicantTypeId," +
                                        " case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, to_char(p.document_type) docType, d.document_description docName, " +
                                        "p.sendback_remarks remarks, p.rho_sendback_remarks rhosendbackremarks,p.rch_sendback_remarks rchsendbackremarks,to_char(p.old_document_no) as docNo, pr.product_name as productName, to_char(t.product_id) as productId, sc.scheme_name as schemeName," +
                                        " t.scheme_id schemeId, t.LOANDATE, to_char(t.loan_amount) as loanAmount, br.branch_name as branch from TBL_OVERSITE_MASTER t left outer join TBL_ops_OVERSITE_MASTER ops on t.application_id=ops.application_id " +
                                        "left outer join TBL_OVERSITE_DOCUMENTS_MST p on (t.application_id = p.application_id or ops.application_id=p.application_id) and (t.loan_id = p.loan_id OR ops.Loan_Id=p.Loan_Id) and " +
                                        "(p.nch_sendback_flag = 1 OR  p.nho_sendback_flag=1 OR P.RCH_SENDBACK_FLAG = 1 OR P.Rho_Sendback_Flag = 1) left outer join vw_document_master d on p.document_type = d.document_id and " +
                                        "(t.product_id = d.product_id ) left outer join product_master pr on pr.product_id = t.product_id left outer join vw_scheme_master sc on sc.scheme_id = t.scheme_id and sc.product_id = t.product_id left outer join branch_master br on br.branch_id = t.branch_id " +
                                        "where (to_char(t.application_id) = '" + request.searchValue + "' or to_char(t.loan_id) = '" + request.searchValue + "') and p.document_no is null union all select distinct p.customer_name customerName, to_char(t.application_id) as applicationId, to_char(t.loan_id) as loanId," +
                                        " P.CUSTOMERID customerId, p.applicant_type as applicantTypeId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, to_char(p.document_type) docType, d.document_description docName, p.sendback_remarks remarks," +
                                        " p.rho_sendback_remarks rhosendbackremarks,p.rch_sendback_remarks rchsendbackremarks,to_char(p.document_no) as docNo, pr.product_name as productName, to_char(t.product_id) as productId, sc.scheme_name as schemeName, t.scheme_id schemeId, t.LOANDATE, to_char(t.loan_amount) as loanAmount, " +
                                        "br.branch_name as branch from TBL_OVERSITE_MASTER t left outer join tbl_ops_oversite_master ops on t.application_id=ops.application_id left outer join TBL_OVERSITE_DOCUMENTS_MST p on (t.application_id = p.application_id or ops.application_id=p.application_id) and (t.loan_id = p.loan_id or ops.loan_id=p.loan_id) and " +
                                        "(p.nch_sendback_flag = 1 OR p.nho_sendback_flag=1 OR P.RCH_SENDBACK_FLAG = 1 OR P.Rho_Sendback_Flag = 1) left outer join vw_document_master d on p.document_type = d.document_id and t.product_id = d.product_id left outer join product_master pr on pr.product_id = t.product_id left outer join vw_scheme_master sc on sc.scheme_id = t.scheme_id and" +
                                        " sc.product_id = t.product_id left outer join branch_master br on br.branch_id = t.branch_id where (to_char(t.application_id) = '" + request.searchValue + "' or to_char(t.loan_id) = '" + request.searchValue + "') and p.document_no is not null";
                                    updateDocumentResponse.PrimaryDataList = new OracleHelper().GetRecords<PrimaryDataProperties>(query);
                                }
                                else if(rchsenbackflag ==1)
                                {
                                    query = "select distinct p.customer_name customerName, to_char(t.application_id) as applicationId, to_char(t.loan_id) as loanId, P.CUSTOMERID customerId, p.applicant_type as applicantTypeId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, to_char(p.document_type) docType, d.document_description docName, p.sendback_remarks remarks, to_char(p.old_document_no) as docNo, pr.product_name as productName, to_char(t.product_id) as productId, sc.scheme_name as schemeName,t.scheme_id schemeId, t.LOANDATE, to_char(t.loan_amount) as loanAmount, " +
                                           "br.branch_name as branch from TBL_OVERSITE_MASTER t left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id and (p.nch_sendback_flag = 1  OR P.RCH_SENDBACK_FLAG=1) left outer join vw_document_master d on p.document_type = d.document_id and t.product_id = d.product_id left outer join product_master pr on pr.product_id = t.product_id left outer join vw_scheme_master sc on sc.scheme_id = t.scheme_id and sc.product_id=t.product_id left outer join branch_master br on br.branch_id = t.branch_id where (to_char(t.application_id) = '" + request.searchValue + "' or to_char(t.loan_id) = '" + request.searchValue + "') " +
                                           "and p.document_no is null union all select distinct p.customer_name customerName, to_char(t.application_id) as applicationId, to_char(t.loan_id) as loanId, P.CUSTOMERID customerId, p.applicant_type as applicantTypeId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, to_char(p.document_type) docType, d.document_description docName, p.sendback_remarks remarks, to_char(p.document_no) as docNo, pr.product_name as productName, to_char(t.product_id) as productId, sc.scheme_name as schemeName,t.scheme_id schemeId, t.LOANDATE, to_char(t.loan_amount) as loanAmount, br.branch_name as branch " +
                                           "from TBL_OVERSITE_MASTER t left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id and (p.nch_sendback_flag = 1  OR P.RCH_SENDBACK_FLAG=1) left outer join vw_document_master d on p.document_type = d.document_id and t.product_id = d.product_id left outer join product_master pr on pr.product_id = t.product_id left outer join vw_scheme_master sc on sc.scheme_id = t.scheme_id and sc.product_id=t.product_id left outer join branch_master br on br.branch_id = t.branch_id where (to_char(t.application_id) = '" + request.searchValue + "' or to_char(t.loan_id) = '" + request.searchValue + "') and p.document_no is not null";
                                    updateDocumentResponse.PrimaryDataList = new OracleHelper().GetRecords<PrimaryDataProperties>(query);

                                }
                                else if(rhosenbackflag ==1)
                                {
                                    query = "select distinct p.customer_name customerName, to_char(t.application_id) as applicationId, to_char(t.loan_id) as loanId, P.CUSTOMERID customerId, p.applicant_type as applicantTypeId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, to_char(p.document_type) docType, case to_char(p.document_type) when '-52' then 'Cibil Report' else d.document_description end docName, p.rho_sendback_remarks remarks, to_char(p.old_document_no) as docNo, pr.product_name as productName, to_char(tt.product_id) as productId, sc.scheme_name as schemeName, tt.scheme_id schemeId, tt.LOANDATE, to_char(tt.loan_amount) as loanAmount, br.branch_name as branch from TBL_ops_OVERSITE_MASTER t left outer join TBL_OVERSITE_MASTER tt on t.application_id=tt.application_id left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id and (P.Rho_Sendback_Flag = 1) " +
                                        "left outer join vw_document_master d on p.document_type = d.document_id and tt.product_id = d.product_id left outer join product_master pr on pr.product_id = tt.product_id left outer join vw_scheme_master sc on sc.scheme_id = tt.scheme_id and sc.product_id = tt.product_id left outer join branch_master br on br.branch_id = tt.branch_id where (to_char(t.application_id) = '" + request.searchValue + "' or to_char(t.loan_id) = '" + request.searchValue + "') and p.document_no is null " +
                                        "union all select distinct p.customer_name customerName, to_char(t.application_id) as applicationId, to_char(t.loan_id) as loanId, P.CUSTOMERID customerId, p.applicant_type as applicantTypeId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, to_char(p.document_type) docType, case to_char(p.document_type) when '-52' then 'Cibil Report' else d.document_description end docName, p.rho_sendback_remarks remarks, to_char(p.document_no) as docNo, pr.product_name as productName, to_char(tt.product_id) as productId, sc.scheme_name as schemeName, tt.scheme_id schemeId, tt.LOANDATE, to_char(tt.loan_amount) as loanAmount, br.branch_name as branch from TBL_ops_OVERSITE_MASTER t left outer join TBL_OVERSITE_MASTER tt on t.application_id=tt.application_id left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id and ( P.Rho_Sendback_Flag = 1) " +
                                        "left outer join vw_document_master d on p.document_type = d.document_id and tt.product_id = d.product_id left outer join product_master pr on pr.product_id = tt.product_id left outer join vw_scheme_master sc on sc.scheme_id = tt.scheme_id and sc.product_id = tt.product_id left outer join branch_master br on br.branch_id = tt.branch_id where (to_char(t.application_id) = '" + request.searchValue + "' or to_char(t.loan_id) = '" + request.searchValue + "') and p.document_no is not null";
                                    updateDocumentResponse.PrimaryDataList = new OracleHelper().GetRecords<PrimaryDataProperties>(query);

                                }

                                if(nhosenbackflag == 1 && nchsenbackflag == 1)
                                {
                                    query = "select distinct p.customer_name customerName, to_char(t.application_id) as applicationId, to_char(t.loan_id) as loanId, P.CUSTOMERID customerId, p.applicant_type as applicantTypeId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, to_char(p.document_type) docType, d.document_description docName, p.sendback_remarks remarks, to_char(p.old_document_no) as docNo, pr.product_name as productName, to_char(t.product_id) as productId, sc.scheme_name as schemeName, t.scheme_id schemeId, t.LOANDATE, to_char(t.loan_amount) as loanAmount, br.branch_name as branch from TBL_OVERSITE_MASTER t left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id and (p.nch_sendback_flag = 1 OR p.nho_sendback_flag=1 OR P.RCH_SENDBACK_FLAG = 1 OR p.rho_sendback_flag=1) left outer join vw_document_master d on p.document_type = d.document_id and t.product_id = d.product_id " +
                                        "left outer join product_master pr on pr.product_id = t.product_id left outer join vw_scheme_master sc on sc.scheme_id = t.scheme_id and sc.product_id = t.product_id left outer join branch_master br on br.branch_id = t.branch_id where (to_char(t.application_id) = '" + request.searchValue + "' or to_char(t.loan_id) = '" + request.searchValue + "') and p.document_no is null and p.nch_sendback_to = 'B' union all select distinct p.customer_name customerName, to_char(t.application_id) as applicationId, to_char(t.loan_id) as loanId, P.CUSTOMERID customerId, p.applicant_type as applicantTypeId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, to_char(p.document_type) docType, d.document_description docName, p.sendback_remarks remarks, to_char(p.document_no) as docNo, pr.product_name as productName, to_char(t.product_id) as productId, sc.scheme_name as schemeName, t.scheme_id schemeId, t.LOANDATE, " +
                                        "to_char(t.loan_amount) as loanAmount, br.branch_name as branch from TBL_OVERSITE_MASTER t left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id and (p.nch_sendback_flag = 1 OR p.nho_sendback_flag=1 OR P.RCH_SENDBACK_FLAG = 1 OR p.rho_sendback_flag=1) left outer join vw_document_master d on p.document_type = d.document_id and t.product_id = d.product_id left outer join product_master pr on pr.product_id = t.product_id left outer join vw_scheme_master sc on sc.scheme_id = t.scheme_id and sc.product_id = t.product_id left outer join branch_master br on br.branch_id = t.branch_id where (to_char(t.application_id) = '" + request.searchValue + "' or to_char(t.loan_id) = '" + request.searchValue + "') and p.document_no is not null and p.nch_sendback_to = 'B'";


                                }
                                else if (nhosenbackflag == 1)
                                {
                                    query = "select distinct p.customer_name customerName, to_char(t.application_id) as applicationId, to_char(t.loan_id) as loanId, P.CUSTOMERID customerId, p.applicant_type as applicantTypeId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, to_char(p.document_type) docType, d.document_description docName, p.sendback_remarks remarks, to_char(p.old_document_no) as docNo, pr.product_name as productName, to_char(t.product_id) as productId, sc.scheme_name as schemeName,t.scheme_id schemeId, t.LOANDATE, to_char(t.loan_amount) as loanAmount, " +
                                        "br.branch_name as branch from TBL_OVERSITE_MASTER t left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id and (p.nch_sendback_flag = 1  OR P.RCH_SENDBACK_FLAG=1) left outer join vw_document_master d on p.document_type = d.document_id and t.product_id = d.product_id left outer join product_master pr on pr.product_id = t.product_id left outer join vw_scheme_master sc on sc.scheme_id = t.scheme_id and sc.product_id=t.product_id left outer join branch_master br on br.branch_id = t.branch_id where (to_char(t.application_id) = '" + request.searchValue + "' or to_char(t.loan_id) = '" + request.searchValue + "') " +
                                        "and p.document_no is null and p.nch_sendback_to='B' union all select distinct p.customer_name customerName, to_char(t.application_id) as applicationId, to_char(t.loan_id) as loanId, P.CUSTOMERID customerId, p.applicant_type as applicantTypeId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, to_char(p.document_type) docType, d.document_description docName, p.sendback_remarks remarks, to_char(p.document_no) as docNo, pr.product_name as productName, to_char(t.product_id) as productId, sc.scheme_name as schemeName,t.scheme_id schemeId, t.LOANDATE, to_char(t.loan_amount) as loanAmount, br.branch_name as branch " +
                                        "from TBL_OVERSITE_MASTER t left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id and (p.nch_sendback_flag = 1  OR P.RCH_SENDBACK_FLAG=1) left outer join vw_document_master d on p.document_type = d.document_id and t.product_id = d.product_id left outer join product_master pr on pr.product_id = t.product_id left outer join vw_scheme_master sc on sc.scheme_id = t.scheme_id and sc.product_id=t.product_id left outer join branch_master br on br.branch_id = t.branch_id where (to_char(t.application_id) = '" + request.searchValue + "' or to_char(t.loan_id) = '" + request.searchValue + "') and p.document_no is not null and p.nch_sendback_to='B'";
                                    updateDocumentResponse.PrimaryDataList = new OracleHelper().GetRecords<PrimaryDataProperties>(query);
                                }



                                else if (nchsenbackflag == 1)
                                {
                                    query = "select distinct p.customer_name customerName, to_char(t.application_id) as applicationId, to_char(t.loan_id) as loanId, P.CUSTOMERID customerId, p.applicant_type as applicantTypeId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, to_char(p.document_type) docType, d.document_description docName, p.sendback_remarks remarks, to_char(p.old_document_no) as docNo, pr.product_name as productName, to_char(t.product_id) as productId, sc.scheme_name as schemeName,t.scheme_id schemeId, t.LOANDATE, to_char(t.loan_amount) as loanAmount, " +
                                        "br.branch_name as branch from TBL_OVERSITE_MASTER t left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id and (p.nch_sendback_flag = 1  OR P.RCH_SENDBACK_FLAG=1) left outer join vw_document_master d on p.document_type = d.document_id and t.product_id = d.product_id left outer join product_master pr on pr.product_id = t.product_id left outer join vw_scheme_master sc on sc.scheme_id = t.scheme_id and sc.product_id=t.product_id left outer join branch_master br on br.branch_id = t.branch_id where (to_char(t.application_id) = '" + request.searchValue + "' or to_char(t.loan_id) = '" + request.searchValue + "') " +
                                        "and p.document_no is null and p.nch_sendback_to='B' union all select distinct p.customer_name customerName, to_char(t.application_id) as applicationId, to_char(t.loan_id) as loanId, P.CUSTOMERID customerId, p.applicant_type as applicantTypeId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, to_char(p.document_type) docType, d.document_description docName, p.sendback_remarks remarks, to_char(p.document_no) as docNo, pr.product_name as productName, to_char(t.product_id) as productId, sc.scheme_name as schemeName,t.scheme_id schemeId, t.LOANDATE, to_char(t.loan_amount) as loanAmount, br.branch_name as branch " +
                                        "from TBL_OVERSITE_MASTER t left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id and (p.nch_sendback_flag = 1  OR P.RCH_SENDBACK_FLAG=1) left outer join vw_document_master d on p.document_type = d.document_id and t.product_id = d.product_id left outer join product_master pr on pr.product_id = t.product_id left outer join vw_scheme_master sc on sc.scheme_id = t.scheme_id and sc.product_id=t.product_id left outer join branch_master br on br.branch_id = t.branch_id where (to_char(t.application_id) = '" + request.searchValue + "' or to_char(t.loan_id) = '" + request.searchValue + "') and p.document_no is not null and p.nch_sendback_to='B'";
                                    updateDocumentResponse.PrimaryDataList = new OracleHelper().GetRecords<PrimaryDataProperties>(query);
                                }

                                if (updateDocumentResponse.PrimaryDataList.Count > 0)
                                {
                                    updateDocumentResponse.LMS = 1;
                                    response.Data = updateDocumentResponse;
                                    response.responseMsg = "Success";
                                    response.apiStatus = ResponseTypeContants.SUCCESS;
                                    response.status = ApiStatusConstants.COMPLETED;
                                    return response;
                                }
                                else
                                {
                                    updateDocumentResponse.LMS = 1;
                                    response.Data = updateDocumentResponse;
                                    response.responseMsg = "You Have No Permission! ";
                                    response.apiStatus = ResponseTypeContants.FAIL;
                                    response.status = ApiStatusConstants.NOT_COMPLETED;
                                    return response;
                                }

                            }
                            //100823

                            else if (cvrolebcm > 0 && cvrolebco==0)
                            {
                                decimal rchsenbackflag = new OracleHelper().ExecuteScalar<decimal>("select tt.rch_sendback from TBL_OVERSITE_MASTER tt where (tt.application_id = '" + request.searchValue + "' or tt.loan_id = '" + request.searchValue + "')");
                                decimal nchsenbackflag = new OracleHelper().ExecuteScalar<decimal>("select tt.nch_sendback from TBL_OVERSITE_MASTER tt where (tt.application_id = '" + request.searchValue + "' or tt.loan_id = '" + request.searchValue + "')");

                                if (rchsenbackflag == 1)
                                {
                                    query = "select distinct p.customer_name customerName, to_char(t.application_id) as applicationId, to_char(t.loan_id) as loanId, P.CUSTOMERID customerId, p.applicant_type as applicantTypeId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, to_char(p.document_type) docType, d.document_description docName, p.sendback_remarks remarks, to_char(p.old_document_no) as docNo, pr.product_name as productName, to_char(t.product_id) as productId, sc.scheme_name as schemeName,t.scheme_id schemeId, t.LOANDATE, to_char(t.loan_amount) as loanAmount, " +
                                            "br.branch_name as branch from TBL_OVERSITE_MASTER t left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id and (p.nch_sendback_flag = 1  OR P.RCH_SENDBACK_FLAG=1) left outer join vw_document_master d on p.document_type = d.document_id and t.product_id = d.product_id left outer join product_master pr on pr.product_id = t.product_id left outer join vw_scheme_master sc on sc.scheme_id = t.scheme_id and sc.product_id=t.product_id left outer join branch_master br on br.branch_id = t.branch_id where (to_char(t.application_id) = '" + request.searchValue + "' or to_char(t.loan_id) = '" + request.searchValue + "') " +
                                            "and p.document_no is null union all select distinct p.customer_name customerName, to_char(t.application_id) as applicationId, to_char(t.loan_id) as loanId, P.CUSTOMERID customerId, p.applicant_type as applicantTypeId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, to_char(p.document_type) docType, d.document_description docName, p.sendback_remarks remarks, to_char(p.document_no) as docNo, pr.product_name as productName, to_char(t.product_id) as productId, sc.scheme_name as schemeName,t.scheme_id schemeId, t.LOANDATE, to_char(t.loan_amount) as loanAmount, br.branch_name as branch " +
                                            "from TBL_OVERSITE_MASTER t left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id and (p.nch_sendback_flag = 1  OR P.RCH_SENDBACK_FLAG=1) left outer join vw_document_master d on p.document_type = d.document_id and t.product_id = d.product_id left outer join product_master pr on pr.product_id = t.product_id left outer join vw_scheme_master sc on sc.scheme_id = t.scheme_id and sc.product_id=t.product_id left outer join branch_master br on br.branch_id = t.branch_id where (to_char(t.application_id) = '" + request.searchValue + "' or to_char(t.loan_id) = '" + request.searchValue + "') and p.document_no is not null";
                                    updateDocumentResponse.PrimaryDataList = new OracleHelper().GetRecords<PrimaryDataProperties>(query);
                                }
                                if (nchsenbackflag == 1)
                                {
                                    query = "select distinct p.customer_name customerName, to_char(t.application_id) as applicationId, to_char(t.loan_id) as loanId, P.CUSTOMERID customerId, p.applicant_type as applicantTypeId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, to_char(p.document_type) docType, d.document_description docName, p.sendback_remarks remarks, to_char(p.old_document_no) as docNo, pr.product_name as productName, to_char(t.product_id) as productId, sc.scheme_name as schemeName,t.scheme_id schemeId, t.LOANDATE, to_char(t.loan_amount) as loanAmount, " +
                                        "br.branch_name as branch from TBL_OVERSITE_MASTER t left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id and (p.nch_sendback_flag = 1  OR P.RCH_SENDBACK_FLAG=1) left outer join vw_document_master d on p.document_type = d.document_id and t.product_id = d.product_id left outer join product_master pr on pr.product_id = t.product_id left outer join vw_scheme_master sc on sc.scheme_id = t.scheme_id and sc.product_id=t.product_id left outer join branch_master br on br.branch_id = t.branch_id where (to_char(t.application_id) = '" + request.searchValue + "' or to_char(t.loan_id) = '" + request.searchValue + "') " +
                                        "and p.document_no is null and p.nch_sendback_to='B' union all select distinct p.customer_name customerName, to_char(t.application_id) as applicationId, to_char(t.loan_id) as loanId, P.CUSTOMERID customerId, p.applicant_type as applicantTypeId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, to_char(p.document_type) docType, d.document_description docName, p.sendback_remarks remarks, to_char(p.document_no) as docNo, pr.product_name as productName, to_char(t.product_id) as productId, sc.scheme_name as schemeName,t.scheme_id schemeId, t.LOANDATE, to_char(t.loan_amount) as loanAmount, br.branch_name as branch " +
                                        "from TBL_OVERSITE_MASTER t left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id and (p.nch_sendback_flag = 1  OR P.RCH_SENDBACK_FLAG=1) left outer join vw_document_master d on p.document_type = d.document_id and t.product_id = d.product_id left outer join product_master pr on pr.product_id = t.product_id left outer join vw_scheme_master sc on sc.scheme_id = t.scheme_id and sc.product_id=t.product_id left outer join branch_master br on br.branch_id = t.branch_id where (to_char(t.application_id) = '" + request.searchValue + "' or to_char(t.loan_id) = '" + request.searchValue + "') and p.document_no is not null and p.nch_sendback_to='B'";
                                    updateDocumentResponse.PrimaryDataList = new OracleHelper().GetRecords<PrimaryDataProperties>(query);
                                }

                                if (updateDocumentResponse.PrimaryDataList == null)
                                {

                                    response.Data = null;
                                    response.responseMsg = "This File Is Sendbacked By NHO So BCH Cant Access This File";
                                    response.apiStatus = ResponseTypeContants.FAIL;
                                    response.status = ApiStatusConstants.NOT_COMPLETED;
                                    return response;


                                }
                                if (updateDocumentResponse.PrimaryDataList.Count > 0)
                                {
                                    updateDocumentResponse.LMS = 1;
                                    response.Data = updateDocumentResponse;
                                    response.responseMsg = "Success";
                                    response.apiStatus = ResponseTypeContants.SUCCESS;
                                    response.status = ApiStatusConstants.COMPLETED;
                                    return response;
                                }
                                else
                                {
                                    updateDocumentResponse.LMS = 1;
                                    response.Data = updateDocumentResponse;
                                    response.responseMsg = "You Have No Permission! ";
                                    response.apiStatus = ResponseTypeContants.FAIL;
                                    response.status = ApiStatusConstants.NOT_COMPLETED;
                                    return response;
                                }
                                

                            }
                            else if (cvrolebco > 0 && cvrolebcm==0)
                            {
                                decimal rhosenbackflag = new OracleHelper().ExecuteScalar<decimal>("select tt.roh_sendback from tbl_ops_oversite_master tt where (tt.application_id = '" + request.searchValue + "' or tt.loan_id = '" + request.searchValue + "')");
                                decimal nhosenbackflag = new OracleHelper().ExecuteScalar<decimal>("select tt.nho_sendback from TBL_OVERSITE_MASTER tt where (tt.application_id = '" + request.searchValue + "' or tt.loan_id = '" + request.searchValue + "')");

                                if (rhosenbackflag == 1)
                                {
                                    query = "select distinct p.customer_name customerName, to_char(t.application_id) as applicationId, to_char(t.loan_id) as loanId, P.CUSTOMERID customerId, p.applicant_type as applicantTypeId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, to_char(p.document_type) docType, case to_char(p.document_type) when '-52' then 'Cibil Report' else d.document_description end docName, p.rho_sendback_remarks remarks, to_char(p.old_document_no) as docNo, pr.product_name as productName, to_char(tt.product_id) as productId, sc.scheme_name as schemeName, tt.scheme_id schemeId, tt.LOANDATE, to_char(tt.loan_amount) as loanAmount, br.branch_name as branch from TBL_ops_OVERSITE_MASTER t left outer join TBL_OVERSITE_MASTER tt on t.application_id=tt.application_id left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id and (P.Rho_Sendback_Flag = 1) " +
                                        "left outer join vw_document_master d on p.document_type = d.document_id and tt.product_id = d.product_id left outer join product_master pr on pr.product_id = tt.product_id left outer join vw_scheme_master sc on sc.scheme_id = tt.scheme_id and sc.product_id = tt.product_id left outer join branch_master br on br.branch_id = tt.branch_id where (to_char(t.application_id) = '" + request.searchValue + "' or to_char(t.loan_id) = '" + request.searchValue + "') and p.document_no is null " +
                                        "union all select distinct p.customer_name customerName, to_char(t.application_id) as applicationId, to_char(t.loan_id) as loanId, P.CUSTOMERID customerId, p.applicant_type as applicantTypeId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, to_char(p.document_type) docType, case to_char(p.document_type) when '-52' then 'Cibil Report' else d.document_description end docName, p.rho_sendback_remarks remarks, to_char(p.document_no) as docNo, pr.product_name as productName, to_char(tt.product_id) as productId, sc.scheme_name as schemeName, tt.scheme_id schemeId, tt.LOANDATE, to_char(tt.loan_amount) as loanAmount, br.branch_name as branch from TBL_ops_OVERSITE_MASTER t left outer join TBL_OVERSITE_MASTER tt on t.application_id=tt.application_id left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id and ( P.Rho_Sendback_Flag = 1) " +
                                        "left outer join vw_document_master d on p.document_type = d.document_id and tt.product_id = d.product_id left outer join product_master pr on pr.product_id = tt.product_id left outer join vw_scheme_master sc on sc.scheme_id = tt.scheme_id and sc.product_id = tt.product_id left outer join branch_master br on br.branch_id = tt.branch_id where (to_char(t.application_id) = '" + request.searchValue + "' or to_char(t.loan_id) = '" + request.searchValue + "') and p.document_no is not null";
                                    updateDocumentResponse.PrimaryDataList = new OracleHelper().GetRecords<PrimaryDataProperties>(query);
                                }
                                if (nhosenbackflag == 1)
                                {
                                    query = "select distinct p.customer_name customerName, to_char(t.application_id) as applicationId, to_char(t.loan_id) as loanId, P.CUSTOMERID customerId, p.applicant_type as applicantTypeId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, to_char(p.document_type) docType, d.document_description docName, p.sendback_remarks remarks, to_char(p.old_document_no) as docNo, pr.product_name as productName, to_char(t.product_id) as productId, sc.scheme_name as schemeName,t.scheme_id schemeId, t.LOANDATE, to_char(t.loan_amount) as loanAmount, " +
                                        "br.branch_name as branch from TBL_OVERSITE_MASTER t left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id and (p.nch_sendback_flag = 1  OR P.RCH_SENDBACK_FLAG=1) left outer join vw_document_master d on p.document_type = d.document_id and t.product_id = d.product_id left outer join product_master pr on pr.product_id = t.product_id left outer join vw_scheme_master sc on sc.scheme_id = t.scheme_id and sc.product_id=t.product_id left outer join branch_master br on br.branch_id = t.branch_id where (to_char(t.application_id) = '" + request.searchValue + "' or to_char(t.loan_id) = '" + request.searchValue + "') " +
                                        "and p.document_no is null and p.nch_sendback_to='B' union all select distinct p.customer_name customerName, to_char(t.application_id) as applicationId, to_char(t.loan_id) as loanId, P.CUSTOMERID customerId, p.applicant_type as applicantTypeId, case p.applicant_type when 'P' then 'Primary Customer' when 'G' then 'Guarantor' when 'C' then 'Co-Applicant' end as applicantType, to_char(p.document_type) docType, d.document_description docName, p.sendback_remarks remarks, to_char(p.document_no) as docNo, pr.product_name as productName, to_char(t.product_id) as productId, sc.scheme_name as schemeName,t.scheme_id schemeId, t.LOANDATE, to_char(t.loan_amount) as loanAmount, br.branch_name as branch " +
                                        "from TBL_OVERSITE_MASTER t left outer join TBL_OVERSITE_DOCUMENTS_MST p on t.application_id = p.application_id and t.loan_id = p.loan_id and (p.nch_sendback_flag = 1  OR P.RCH_SENDBACK_FLAG=1) left outer join vw_document_master d on p.document_type = d.document_id and t.product_id = d.product_id left outer join product_master pr on pr.product_id = t.product_id left outer join vw_scheme_master sc on sc.scheme_id = t.scheme_id and sc.product_id=t.product_id left outer join branch_master br on br.branch_id = t.branch_id where (to_char(t.application_id) = '" + request.searchValue + "' or to_char(t.loan_id) = '" + request.searchValue + "') and p.document_no is not null and p.nch_sendback_to='B'";
                                    updateDocumentResponse.PrimaryDataList = new OracleHelper().GetRecords<PrimaryDataProperties>(query);
                                }
                                if (updateDocumentResponse.PrimaryDataList == null)
                                {

                                    response.Data = null;
                                    response.responseMsg = "This File Is Sendbacked By NCH So BCO Cant Access This File";
                                    response.apiStatus = ResponseTypeContants.FAIL;
                                    response.status = ApiStatusConstants.NOT_COMPLETED;
                                    return response;



                                }
                                if (updateDocumentResponse.PrimaryDataList.Count > 0)
                                {
                                    updateDocumentResponse.LMS = 1;
                                    response.Data = updateDocumentResponse;
                                    response.responseMsg = "Success";
                                    response.apiStatus = ResponseTypeContants.SUCCESS;
                                    response.status = ApiStatusConstants.COMPLETED;
                                    return response;
                                }
                                else
                                {
                                    updateDocumentResponse.LMS = 1;
                                    response.Data = updateDocumentResponse;
                                    response.responseMsg = "You Have No Permission! ";
                                    response.apiStatus = ResponseTypeContants.FAIL;
                                    response.status = ApiStatusConstants.NOT_COMPLETED;
                                    return response;
                                }

                            }

                            else
                            {
                                response.Data = null;
                                response.responseMsg = "You Have No Permission!";
                                response.apiStatus = ResponseTypeContants.FAIL;
                                response.status = ApiStatusConstants.NOT_COMPLETED;
                            }
                        }


                    }
                }
                else
                {
                    response.Data = null;
                    response.responseMsg = "Invalid Loan Id/Application Id";
                    return response;
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
        public Response<AddDocumentResponse> AddDoc(AddDocumentRequest request)
        {
            AddDocumentResponse addDocumentResponse = new AddDocumentResponse();
            Response<AddDocumentResponse> response = new Response<AddDocumentResponse>();
            string output = JsonConvert.SerializeObject(request);
            string query1 = string.Empty;
            string query2 = string.Empty;
            string query3 = string.Empty;
            string query4 = string.Empty;
            List<ApplicationDocumentProperties> applicationDocumentList = new List<ApplicationDocumentProperties>();
            List<ApplicationDocumentPropertiesnew> applicationsDocumentList = new List<ApplicationDocumentPropertiesnew>();


            try
            {
                List<string> cmd = new List<string>();

                int stnum = request.OversiteDocumentList.Count;


                List<string> cmd1 = new List<string>();
                DynamicParameters[] dynamicParameterList = new DynamicParameters[1];
                DynamicParameters dynamicParameters = new DynamicParameters();

                for (int i = 0; i <= stnum - 1; i++)
                {
                    DocUploadProperties documents = request.OversiteDocumentList[i];

                    query1 = "select  nvl(t.lms,0) as lms,t.verification_status as status,t.PRODUCT_ID as product_id from TBL_OVERSITE_MASTER t where to_char(t.application_id) = '" + documents.applicationId + "' ";
                    Adddocproperties doc = new OracleHelper().GetRecord<Adddocproperties>(query1);
                    if (doc.lms == 0)//finnone
                    {
                        if (string.IsNullOrEmpty(documents.Document))
                        {
                            response.apiStatus = ResponseTypeContants.FAIL;
                            response.responseMsg = "Please Provide A Valid Document";
                            response.status = ApiStatusConstants.NOT_COMPLETED;
                            return response;
                        }
                        if (doc.status == 7)//upload pending
                        {
                            ApplicationDocumentProperties applicationDocument = new ApplicationDocumentProperties();
                            applicationDocument.ApplicationId = documents.applicationId;
                            applicationDocument.Extension = documents.DocExtension;
                            //applicationDocument.DocID= documents.documentType

                            try
                            {
                                applicationDocument.Data = Convert.FromBase64String(documents.Document);
                            }
                            catch (Exception e)
                            {
                                string xx = e.Message;
                                response.Data = null;
                                response.responseMsg = "Please Provide A Valid Document Format[Should Be Base64] ";
                                response.apiStatus = ResponseTypeContants.FAIL;
                                response.status = ApiStatusConstants.NOT_COMPLETED;
                            }
                            string query31 = "";
                            string query41 = "";

                            object docuNo = "";

                            string newvalue = "333";
                            query31 = "select value from key_master  where firm_id = 1  and branch_id =  0   and module_id= 3  and key_id= 1020  and product_id = 43";
                            docuNo = new OracleHelper().ExecuteScalar<object>(query31);
                            query41 = "update key_master set value=  value+1 where firm_id = 1  and branch_id =  0   and module_id= 3  and key_id= 1020  and product_id = 43";
                            new OracleHelper().ExecuteNonQuery(query41);
                            int proid = Convert.ToInt32(documents.ProductID);
                            string docuNo1 = newvalue + docuNo;
                            int documentNo = Convert.ToInt32(docuNo1);
                            applicationDocument.DocumentNo = documentNo;
                            applicationDocumentList.Add(applicationDocument);

                            string query222 = "select count(*) from TBL_OVERSITE_DOCUMENTS_MST t where t.application_id = " + documents.applicationId + " and t.document_type= " + documents.documentType + " and t.CUSTOMERID='" + documents.customerId + "'";
                            decimal cnt1 = new OracleHelper().ExecuteScalar<decimal>(query222);
                            if (cnt1 == 0)
                            {
                                object custname;
                                custname = new OracleHelper().ExecuteScalar<object>("select distinct t.customer_name as custname from TBL_OVERSITE_DOCUMENTS_MST t where  t.application_id= '" + documents.applicationId + "'  and t.CUSTOMERID='" + documents.customerId + "' ");
                                docuNo = new OracleHelper().ExecuteScalar<object>(query31);
                                cmd.Add("insert into TBL_OVERSITE_DOCUMENTS_MST(APPLICATION_ID,LOAN_ID,OLD_DOCUMENT_NO,DOCUMENT_TYPE,PRODUCT_ID,VERIFY_FLAG,APPLICANT_TYPE,CUSTOMERID,CUSTOMER_NAME) values('" + documents.applicationId + "','" + documents.loanId + "','" + docuNo1 + "','" + documents.documentType + "','" + documents.ProductID + "',1,'" + documents.applicantType + "','" + documents.customerId + "','" + custname + "')");
                            }
                            else
                            {
                                query2 = "select count(*) from TBL_OVERSITE_DOCUMENTS_MST t where t.application_id = " + documents.applicationId + " and t.document_type= " + documents.documentType + " and t.CUSTOMERID='" + documents.customerId + "' and t.old_document_no is null";
                                decimal cnt = new OracleHelper().ExecuteScalar<decimal>(query2);
                                if (cnt > 0)
                                {

                                    cmd.Add("Update TBL_OVERSITE_DOCUMENTS_MST t set t.old_document_no= '" + docuNo1 + "',t.upload_by='" + documents.EnterBy + "',upload_date=sysdate,VERIFY_FLAG=1 where t.application_id= " + documents.applicationId + " and t.document_type=" + documents.documentType + " and t.CUSTOMERID='" + documents.customerId + "'");
                                }
                                else
                                {

                                    decimal docSeq = new OracleHelper().GetRecord<decimal>("select nvl(max(slno),0) from TBL_OVERSITE_DOCUMENTS_MST_his where application_id = " + documents.applicationId);
                                    cmd.Add("insert into TBL_OVERSITE_DOCUMENTS_MST_his(select t.*, " + docSeq + " from TBL_OVERSITE_DOCUMENTS_MST t where  t.application_id = " + documents.applicationId + " and t.document_type= " + documents.documentType + " and t.CUSTOMERID='" + documents.customerId + "')");
                                    cmd.Add("Update TBL_OVERSITE_DOCUMENTS_MST t set t.old_document_no= '" + docuNo1 + "',t.upload_by='" + documents.EnterBy + "',upload_date=sysdate,VERIFY_FLAG=1 where t.application_id= " + documents.applicationId + " and t.document_type=" + documents.documentType + " and t.CUSTOMERID='" + documents.customerId + "'");

                                }

                            }
                            for (int k1 = 0; k1 < applicationDocumentList.Count; k1++)
                            {
                                int Docflag = new OracleHelper().ExecuteScalar<int>("select t.parmtr_value id from general_parameter t where t.parmtr_id=111 ");

                                ApplicationDocumentProperties applicationDocumentMongo = applicationDocumentList[k1];
                                if (Docflag == 0)
                                {
                                    UpdateDocumentsnew(1, applicationDocumentMongo);
                                }

                            }

                            addDocumentResponse.DocNo = applicationDocument.DocumentNo.ToString();

                        }
                        if (doc.status == 3 || doc.status == 5 || doc.status == 10)//query
                        {
                            ApplicationDocumentProperties applicationDocument = new ApplicationDocumentProperties();
                            applicationDocument.ApplicationId = documents.applicationId;
                            applicationDocument.Extension = documents.DocExtension;
                            OvrsiteDocUpadteProperties rq1 = new OvrsiteDocUpadteProperties();
                            rq1.applicationId = documents.applicationId;
                            rq1.customerId = documents.customerId;
                            rq1.documentType = documents.documentType;
                            rq1.productId = documents.ProductID.ToString();
                            rq1.productId = documents.ProductID.ToString();
                            rq1.enterBy = documents.EnterBy;
                            int documentNo = OvrsiteDocUpdate(rq1);
                            applicationDocument.DocumentNo = documentNo;
                            applicationDocumentList.Add(applicationDocument);

                            try
                            {
                                applicationDocument.Data = Convert.FromBase64String(documents.Document);
                            }
                            catch (Exception e)
                            {
                                string xx = e.Message;
                                response.Data = null;
                                response.responseMsg = "Please Provide A Valid Document Format[Should Be Base64] ";
                                response.apiStatus = ResponseTypeContants.FAIL;
                                response.status = ApiStatusConstants.NOT_COMPLETED;
                            }
                            for (int k1 = 0; k1 < applicationDocumentList.Count; k1++)
                            {
                                int Docflag = new OracleHelper().ExecuteScalar<int>("select t.parmtr_value id from general_parameter t where t.parmtr_id=111 ");

                                ApplicationDocumentProperties applicationDocumentMongo = applicationDocumentList[k1];
                                if (Docflag == 0)
                                {
                                    UpdateDocumentsnew(1, applicationDocumentMongo);
                                }
                                else
                                {
                                    //docManger.AddApplicationDoc(applicationDocumentMongo);
                                }

                            }

                            addDocumentResponse.DocNo = applicationDocument.DocumentNo.ToString();
                        }


                    }
                    else
                    {

                        if (documents.ProductID == 10)
                        {
                            //tw documents
                            query2 = "select count(*) from application_documents_dtl t where t.application_id = " + documents.applicationId + "";
                            decimal cnt3 = new OracleHelper().ExecuteScalar<decimal>(query2);

                            if (cnt3 > 0)//tw
                            {

                                if (string.IsNullOrEmpty(documents.Document))
                                {
                                    response.apiStatus = ResponseTypeContants.FAIL;
                                    response.responseMsg = "Please Provide A Valid Document";
                                    response.status = ApiStatusConstants.NOT_COMPLETED;
                                    return response;
                                }
                                decimal Docflag1 = new OracleHelper().ExecuteScalar<decimal>("select t.parmtr_value id from general_parameter t where t.parmtr_id=111 ");


                                OvrsiteDocUpadteProperties rq1 = new OvrsiteDocUpadteProperties();
                                rq1.applicationId = documents.applicationId;
                                rq1.customerId = documents.customerId;
                                rq1.documentType = documents.documentType;
                                rq1.productId = documents.ProductID.ToString();
                                rq1.enterBy = documents.EnterBy;
                                int documentNo = OvrsiteDocUpdate(rq1);
                                ApplicationDocumentProperties applicationDocument = new ApplicationDocumentProperties();
                                applicationDocument.ApplicationId = documents.applicationId;
                                applicationDocument.DocumentNo = documentNo;
                                applicationDocument.Extension = documents.DocExtension;
                                try
                                {
                                    applicationDocument.Data = Convert.FromBase64String(documents.Document);
                                }
                                catch (Exception e)
                                {
                                    string xx = e.Message;
                                    response.Data = null;
                                    response.responseMsg = "Please Provide A Valid Document Format[Should Be Base64] ";
                                    response.apiStatus = ResponseTypeContants.FAIL;
                                    response.status = ApiStatusConstants.NOT_COMPLETED;
                                }
                                applicationDocumentList.Add(applicationDocument);


                                for (int k1 = 0; k1 < applicationDocumentList.Count; k1++)
                                {
                                    ApplicationDocumentProperties applicationDocumentMongo = applicationDocumentList[k1];
                                    if (Docflag1 == 0)
                                    {
                                        UpdateDocuments(1, applicationDocumentMongo);
                                    }

                                }

                                addDocumentResponse.DocNo = documentNo.ToString();

                            }
                            //tw documents end 
                            else//cv
                            {
                                query2 = "select count(*) from vw_cv_application_doc_dtl t where t.application_id = " + documents.applicationId + "";
                                decimal cnt5 = new OracleHelper().ExecuteScalar<decimal>(query2);
                                if (cnt5 > 0)
                                {
                                    if (string.IsNullOrEmpty(documents.Document))
                                    {
                                        response.apiStatus = ResponseTypeContants.FAIL;
                                        response.responseMsg = "Please Provide A Valid Document";
                                        response.status = ApiStatusConstants.NOT_COMPLETED;
                                        return response;
                                    }
                                    query2 = "select count(*) from TBL_OVERSITE_DOCUMENTS_MST t where t.application_id = " + documents.applicationId + " and t.document_type= " + documents.documentType + "";
                                    decimal count3 = new OracleHelper().ExecuteScalar<decimal>(query2);
                                    decimal Docflag1 = new OracleHelper().ExecuteScalar<decimal>("select t.parmtr_value id from general_parameter t where t.parmtr_id=111 ");

                                    if (count3 > 0)
                                    {
                                        OvrsiteDocUpadteProperties rq1 = new OvrsiteDocUpadteProperties();
                                        rq1.applicationId = documents.applicationId;
                                        rq1.customerId = documents.customerId;
                                        rq1.documentType = documents.documentType;
                                        rq1.productId = documents.ProductID.ToString();
                                        rq1.enterBy = documents.EnterBy;
                                        int documentNo = OvrsiteDocUpdate(rq1);
                                        ApplicationDocumentPropertiesnew applicationDocument = new ApplicationDocumentPropertiesnew();
                                        applicationDocument.ApplicationId = documents.applicationId;
                                        applicationDocument.DocumentNo = documentNo.ToString();
                                        applicationDocument.Extension = documents.DocExtension;
                                        try
                                        {
                                            applicationDocument.Data = Convert.FromBase64String(documents.Document);
                                        }
                                        catch (Exception e)
                                        {
                                            string xx = e.Message;
                                            response.Data = null;
                                            response.responseMsg = "Please Provide A Valid Document Format[Should Be Base64]";
                                            response.apiStatus = ResponseTypeContants.FAIL;
                                            response.status = ApiStatusConstants.NOT_COMPLETED;
                                        }
                                        applicationsDocumentList.Add(applicationDocument);


                                        decimal docSeq = new OracleHelper().ExecuteScalar<decimal>("select nvl(max(slno),0)+1 from vw_application_doc_dtl_his where   application_id = '" + documents.applicationId + "' ");

                                        cmd.Add("insert into vw_application_doc_dtl_his (select t.*," + docSeq + " from vw_cv_application_doc_dtl t where t.application_id=" + documents.applicationId + " and t.document_type=" + documents.documentType + " and t.applicant_type='" + documents.applicantType + "')");
                                        cmd.Add("update vw_cv_application_doc_dtl t set  t.document_no=" + documentNo + ",t.MODIFIED_BY='" + documents.EnterBy + "',t.MODIFIED_DATE=sysdate where t.application_id=" + documents.applicationId + " and t.document_type=" + documents.documentType + " and t.applicant_type='" + documents.applicantType + "'");

                                        for (int k1 = 0; k1 < applicationsDocumentList.Count; k1++)
                                        {
                                            ApplicationDocumentPropertiesnew applicationDocumentMongo = applicationsDocumentList[k1];
                                            if (Docflag1 == 0)
                                            {
                                                UpdateDocument(1, applicationDocumentMongo);
                                            }
                                            else
                                            {
                                            }

                                        }
                                        addDocumentResponse.DocNo = documentNo.ToString();
                                    }
                                }
                            }
                        }
                        else if (documents.ProductID == 11 || documents.ProductID == 12 || documents.ProductID == 13 || documents.ProductID==14)
                        {
                            query2 = "select count(*) from vw_cv_application_doc_dtl t where t.application_id = " + documents.applicationId + "";
                            decimal cnt5 = new OracleHelper().ExecuteScalar<decimal>(query2);
                            if (cnt5 > 0)
                            {
                                if (string.IsNullOrEmpty(documents.Document))
                                {
                                    response.apiStatus = ResponseTypeContants.FAIL;
                                    response.responseMsg = "Please Provide A Valid Document";
                                    response.status = ApiStatusConstants.NOT_COMPLETED;
                                    return response;
                                }
                                query2 = "select count(*) from TBL_OVERSITE_DOCUMENTS_MST t where t.application_id = " + documents.applicationId + " and t.document_type = " + documents.documentType + " and t.CUSTOMERID = '" + documents.customerId + "'";
                                decimal count3 = new OracleHelper().ExecuteScalar<decimal>(query2);
                                decimal Docflag1 = new OracleHelper().ExecuteScalar<decimal>("select t.parmtr_value id from general_parameter t where t.parmtr_id=111 ");

                                if (count3 > 0)
                                {
                                    OvrsiteDocUpadteProperties rq1 = new OvrsiteDocUpadteProperties();
                                    rq1.applicationId = documents.applicationId;
                                    rq1.customerId = documents.customerId;
                                    rq1.documentType = documents.documentType;
                                    rq1.productId = documents.ProductID.ToString();
                                    //rq1.productId = documents.ProductID.ToString();
                                    rq1.enterBy = documents.EnterBy;
                                    int documentNo = OvrsiteDocUpdate(rq1);
                                    if(documentNo ==0)
                                    {
                                        response.apiStatus = ResponseTypeContants.FAIL;
                                        response.responseMsg = "Document No generation failed.Please try again !!!";
                                        response.status = ApiStatusConstants.NOT_COMPLETED;
                                        return response;
                                    }
                                    ApplicationDocumentPropertiesnew applicationDocument = new ApplicationDocumentPropertiesnew();
                                    applicationDocument.ApplicationId = documents.applicationId;
                                    applicationDocument.DocumentNo = documentNo.ToString();
                                    applicationDocument.Extension = documents.DocExtension;

                                    try
                                    {
                                        applicationDocument.Data = Convert.FromBase64String(documents.Document);
                                    }
                                    catch (Exception e)
                                    {
                                        string xx = e.Message;
                                        response.Data = null;
                                        response.responseMsg = "Please Provide A Valid Document Format[Should Be Base64]";
                                        response.apiStatus = ResponseTypeContants.FAIL;
                                        response.status = ApiStatusConstants.NOT_COMPLETED;
                                    }
                                    applicationsDocumentList.Add(applicationDocument);

                                    decimal docSeq = new OracleHelper().ExecuteScalar<decimal>("select nvl(max(slno),0)+1 from vw_application_doc_dtl_his where   application_id = '" + documents.applicationId + "' ");

                                    cmd.Add("insert into vw_application_doc_dtl_his (select t.*," + docSeq + " from vw_cv_application_doc_dtl t where t.application_id=" + documents.applicationId + " and t.document_type=" + documents.documentType + " and t.applicant_type='" + documents.applicantType + "')");
                                    cmd.Add("update vw_cv_application_doc_dtl t set  t.document_no=" + documentNo + ",t.MODIFIED_BY='" + documents.EnterBy + "',t.MODIFIED_DATE=sysdate  where t.application_id=" + documents.applicationId + " and t.document_type=" + documents.documentType + " and t.applicant_type='" + documents.applicantType + "'");


                                    for (int k1 = 0; k1 < applicationsDocumentList.Count; k1++)
                                    {
                                        ApplicationDocumentPropertiesnew applicationDocumentMongo = applicationsDocumentList[k1];
                                        if (Docflag1 == 0)
                                        {

                                           UpdateDocument(1, applicationDocumentMongo);
                                        }

                                    }
                                    addDocumentResponse.DocNo = documentNo.ToString();
                                }
                                else
                                {
                                    response.apiStatus = ResponseTypeContants.FAIL;
                                    response.responseMsg = "Please Check Document";
                                    response.status = ApiStatusConstants.NOT_COMPLETED;
                                    return response;
                                }
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(documents.Document))
                            {
                                response.apiStatus = ResponseTypeContants.FAIL;
                                response.responseMsg = "Please Provide A Valid Document";
                                response.status = ApiStatusConstants.NOT_COMPLETED;
                                return response;
                            }
                            decimal count2 = new OracleHelper().ExecuteScalar<decimal>(query2);
                            decimal Docflag = new OracleHelper().ExecuteScalar<decimal>("select t.parmtr_value id from general_parameter t where t.parmtr_id=111 ");

                            //if (count2 > 0)
                            {
                                OvrsiteDocUpadteProperties rq1 = new OvrsiteDocUpadteProperties();
                                rq1.applicationId = documents.applicationId;
                                rq1.customerId = documents.customerId;
                                rq1.documentType = documents.documentType;
                                rq1.productId = documents.ProductID.ToString();
                                rq1.enterBy = documents.EnterBy;
                                int documentNo = OvrsiteDocUpdate(rq1);
                                decimal docSeq = new OracleHelper().ExecuteScalar<decimal>("select nvl(max(slno),0)+1 from application_documents_dtl_his where   application_id = '" + documents.applicationId + "' ");

                                cmd.Add("insert into application_documents_dtl_his (select t.*," + docSeq + " from Application_documents_dtl t where t.application_id=" + documents.applicationId + " and t.document_type=" + documents.documentType + " and t.applicant_type='" + documents.applicantType + "')");
                                cmd.Add("update application_documents_dtl t set  t.document_no=" + documentNo + ",t.MODIFIED_BY='" + documents.EnterBy + "',t.MODIFIED_DATE=sysdate  where t.application_id=" + documents.applicationId + " and t.document_type=" + documents.documentType + " and t.applicant_type='" + documents.applicantType + "'");

                                ApplicationDocumentProperties applicationDocument = new ApplicationDocumentProperties();
                                applicationDocument.ApplicationId = documents.applicationId;
                                applicationDocument.DocumentNo = documentNo;
                                applicationDocument.Extension = documents.DocExtension;
                                try
                                {
                                    applicationDocument.Data = Convert.FromBase64String(documents.Document);
                                }
                                catch (Exception e)
                                {
                                    string xx = e.Message;
                                    response.Data = null;
                                    response.responseMsg = "Please Provide A Valid Document Format[Should Be Base64]";
                                    response.apiStatus = ResponseTypeContants.FAIL;
                                    response.status = ApiStatusConstants.NOT_COMPLETED;
                                }
                                applicationDocumentList.Add(applicationDocument);

                                for (int k1 = 0; k1 < applicationDocumentList.Count; k1++)
                                {
                                    ApplicationDocumentProperties applicationDocumentMongo = applicationDocumentList[k1];
                                    if (Docflag == 0)
                                    {

                                        UpdateDocuments(1, applicationDocumentMongo);
                                    }

                                }
                                addDocumentResponse.DocNo = documentNo.ToString();

                            }
                        }
                    }

                }
                int queryresult = new OracleHelper().ExecuteNonQuerynew(cmd.ToArray(), OracleHelper.SQLMode.Query, null);
                if(queryresult<0)
                {
                    response.apiStatus = ResponseTypeContants.FAIL;
                    response.responseMsg = "Please Provide A Valid Document";
                    response.status = ApiStatusConstants.NOT_COMPLETED;
                    return response;
                }


            }
            catch (Exception ex)
            {
                ErrorLogRequest errorLogRequest = new Oversite.DTO.ErrorLog.Request.ErrorLogRequest();
                errorLogRequest.Function_Name = "Add Doc";
                errorLogRequest.V_Data = output;
                errorLogRequest.V_Exception = ex.Message;
                new ErrorLog.ErrorLogDataSource().ErrorLog(errorLogRequest);
                response.responseMsg = "Failed!Please Contact The Support Team.";
                response.Data = null;
                response.apiStatus = ResponseTypeContants.FAIL;
                response.status = ApiStatusConstants.NOT_COMPLETED;

                return response;
            }
            response.Data = addDocumentResponse;
            response.responseMsg = "Success";
            response.apiStatus = ApiStatusConstants.COMPLETED;
            response.status = ResponseTypeContants.SUCCESS;
            return response;
        }

        private int OvrsiteDocUpdate(OvrsiteDocUpadteProperties req)
        {
            string query3 = "";
            string query4 = "";

            object docuNo = "";
            List<string> cmd = new List<string>();
            string newvalue = "333";
            query3 = "select value from key_master  where firm_id = 1  and branch_id =  0   and module_id= 3  and key_id= 1020  and product_id = 43";
            docuNo = new OracleHelper().ExecuteScalar<object>(query3);
            //string Doc_NO = documentNo.ToString();
            query4 = "update key_master set value=  value+1 where firm_id = 1  and branch_id =  0   and module_id= 3  and key_id= 1020  and product_id = 43";
            new OracleHelper().ExecuteNonQuery(query4);
            //documentNo = new OracleHelper().ExecuteScalar<decimal>(query3);
            //new OracleHelper().ExecuteScalar<decimal>(query4);
            int proid = Convert.ToInt32(req.productId);
            //int documentNo = int.Parse(req.productId + docuNo);
            string docuNo1 = newvalue + docuNo;
            int documentNo = Convert.ToInt32(docuNo1);
            //string docuNo1 = documentNo.ToString();
            string quer444 = "select  verify_flag from TBL_OVERSITE_DOCUMENTS_MST t where t.application_id = " + req.applicationId + " and t.document_type = " + req.documentType + " and t.CUSTOMERID = '" + req.customerId + "'";
            decimal curstatus = new OracleHelper().ExecuteScalar<decimal>(quer444);
            query4 = "select  nch_sendback_flag from TBL_OVERSITE_DOCUMENTS_MST t where t.application_id= " + req.applicationId + " and t.document_type=" + req.documentType + " and t.CUSTOMERID='" + req.customerId + "'";
            decimal sendbacknchflag = new OracleHelper().ExecuteScalar<decimal>(query4);
            string quer44 = "select  nho_sendback_flag from TBL_OVERSITE_DOCUMENTS_MST t where t.application_id= " + req.applicationId + " and t.document_type=" + req.documentType + " and t.CUSTOMERID='" + req.customerId + "'";
            decimal sendbacknhoflag = new OracleHelper().ExecuteScalar<decimal>(quer44);
            string quer99 = "select  count(*) from TBL_OVERSITE_DOCUMENTS_MST t where t.application_id= " + req.applicationId + " and t.document_type=" + req.documentType + " and t.CUSTOMERID='" + req.customerId + "' and t.document_no is not null";
            decimal docflag = new OracleHelper().ExecuteScalar<decimal>(quer99);
            string oldnewdoc = "select  count(*) from TBL_OVERSITE_DOCUMENTS_MST t where t.application_id= " + req.applicationId + " and t.document_type=" + req.documentType + " and t.CUSTOMERID='" + req.customerId + "' and t.document_no is null and t.old_document_no is null";
            decimal oldnewdocflag = new OracleHelper().ExecuteScalar<decimal>(oldnewdoc);

            string queryrch = "select  t.rch_sendback_flag from TBL_OVERSITE_DOCUMENTS_MST t where t.application_id= " + req.applicationId + " and t.document_type=" + req.documentType + " and t.CUSTOMERID='" + req.customerId + "'";
            decimal sendbackrchflag = new OracleHelper().ExecuteScalar<decimal>(queryrch);

            string Roh = "select  t.RHO_SENDBACK_FLAG from TBL_OVERSITE_DOCUMENTS_MST t where t.application_id= " + req.applicationId + " and t.document_type=" + req.documentType + " and t.CUSTOMERID='" + req.customerId + "'";
            decimal Rohflag = new OracleHelper().ExecuteScalar<decimal>(Roh);

            if (oldnewdocflag > 0)
            {
                cmd.Add("Update TBL_OVERSITE_DOCUMENTS_MST t set  t.old_document_no= '" + docuNo1 + "' where t.application_id= " + req.applicationId + " and t.document_type=" + req.documentType + " and t.CUSTOMERID='" + req.customerId + "'");
            }
            else
            {
                if (docflag > 0)
                {
                    object olddocuNo = "";
                    object preolddocuNo = "";
                    string query312 = "select  t.document_no from TBL_OVERSITE_DOCUMENTS_MST t where t.application_id= " + req.applicationId + " and t.document_type=" + req.documentType + " and t.CUSTOMERID='" + req.customerId + "'";
                    olddocuNo = new OracleHelper().ExecuteScalar<object>(query312);
                    string query3112 = "select  t.old_document_no from TBL_OVERSITE_DOCUMENTS_MST t where t.application_id= " + req.applicationId + " and t.document_type=" + req.documentType + " and t.CUSTOMERID='" + req.customerId + "'";
                    preolddocuNo = new OracleHelper().ExecuteScalar<object>(query3112);
                    decimal seq_ID1 = new OracleHelper().ExecuteScalar<decimal>("select nvl(max(sequence_id ),0)+1  from TBL_OVERSITE_DOCUMENTS_DTL where  application_id = '" + req.applicationId + "' ");
                    cmd.Add("insert into TBL_OVERSITE_DOCUMENTS_dtl (APPLICATION_ID,LOAN_ID,customerid,DOCUMENT_NO,DOCUMENT_TYPE,VERIFICATION_STATUS,VERIFY_DATE,VERIFY_BY,SENDBACK_FLAG,PRODUCT_ID,SEQUENCE_ID)" +
                        "values (" + req.applicationId + ",'" + req.loanId + "','" + req.customerId + "','" + preolddocuNo + "'," + req.documentType + "," + curstatus + ",sysdate,'" + req.enterBy + "',1," + proid + "," + seq_ID1 + ")");

                    cmd.Add("Update TBL_OVERSITE_DOCUMENTS_MST t set  t.document_no= '" + docuNo1 + "',t.old_document_no= '" + olddocuNo + "' where t.application_id= " + req.applicationId + " and t.document_type=" + req.documentType + " and t.CUSTOMERID='" + req.customerId + "'");

                }
                else
                {
                    cmd.Add("Update TBL_OVERSITE_DOCUMENTS_MST t set  t.document_no= '" + docuNo1 + "' where t.application_id= " + req.applicationId + " and t.document_type=" + req.documentType + " and t.CUSTOMERID='" + req.customerId + "'");
                }
            }



            if (sendbacknchflag == 1)
            {
                cmd.Add("Update TBL_OVERSITE_DOCUMENTS_MST t set /*t.document_no= '" + docuNo1 + "',*/t.recaptured_by='" + req.enterBy + "',t.recaptured_dt=sysdate/*,t.nch_sendback_flag=2*/ where t.application_id= " + req.applicationId + " and t.document_type=" + req.documentType + " and t.CUSTOMERID='" + req.customerId + "'");

            }
            if (sendbacknhoflag == 1)
            {
                cmd.Add("Update TBL_OVERSITE_DOCUMENTS_MST t set /*t.document_no= '" + docuNo1 + "',*/t.recaptured_by='" + req.enterBy + "',t.recaptured_dt=sysdate/*,t.nho_sendback_flag=2*/ where t.application_id= " + req.applicationId + " and t.document_type=" + req.documentType + " and t.CUSTOMERID='" + req.customerId + "'");

            }

            if (sendbackrchflag == 1)
            {
                cmd.Add("Update TBL_OVERSITE_DOCUMENTS_MST t set /*t.document_no= '" + docuNo1 + "',*/t.recaptured_by='" + req.enterBy + "',t.recaptured_dt=sysdate where t.application_id= " + req.applicationId + " and t.document_type=" + req.documentType + " and t.CUSTOMERID='" + req.customerId + "'");

            }
            if (Rohflag == 1)
            {
                cmd.Add("Update TBL_OVERSITE_DOCUMENTS_MST t set /* VERIFY_FLAG=13,t.document_no= '" + docuNo1 + "',*/t.recaptured_by='" + req.enterBy + "',t.recaptured_dt=sysdate where t.application_id= " + req.applicationId + " and t.document_type=" + req.documentType + " and t.CUSTOMERID='" + req.customerId + "'");

            }
            int result =new OracleHelper().ExecuteNonQuerynew(cmd.ToArray(), OracleHelper.SQLMode.Query, null);
            if(result>0)
            {
                return documentNo;

            }
            else
            {
                return 0;
            }

            return documentNo;
        }
        public int UpdateDocuments(int transactionType, ApplicationDocumentProperties applicationDocumentProperties)
        {
            int retval = -1;
            MongoDbConnectionHandler mongoDbConnectionHandler = new MongoDbConnectionHandler();
            try
            {
                if (transactionType == 1)
                {
                    mongoDbConnectionHandler.Insert("Application_Documents", applicationDocumentProperties);
                }
                else
                {
                    //mongoDbConnectionHandler.Update("Application_Documents", applicationDocumentProperties, "DocumentNo", applicationDocumentProperties.DocumentNo);
                }
                retval = 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retval;
        }

        private int UpdateDocumentsnew(int transactionType, ApplicationDocumentProperties applicationDocumentProperties)
        {
            int retval = -1;
            MongoDbConnectionHandler mongoDbConnectionHandler = new MongoDbConnectionHandler();
            try
            {
                if (transactionType == 1)
                {
                    mongoDbConnectionHandler.Insert("Finnone_CV_documents", applicationDocumentProperties);
                }
                else
                {
                    mongoDbConnectionHandler.Update("Finnone_CV_documents", applicationDocumentProperties, "DocumentNo", applicationDocumentProperties.DocumentNo);
                }
                retval = 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retval;
        }

        private int UpdateDocument(int transactionType, ApplicationDocumentPropertiesnew applicationDocumentProperties)
        {
            int retval = -1;
            MongoDbConnectionHandler mongoDbConnectionHandler = new MongoDbConnectionHandler();
            try
            {
                if (transactionType == 1)
                {
                    mongoDbConnectionHandler.Insert("CV_Application_Documents", applicationDocumentProperties);
                }
                else
                {
                    //mongoDbConnectionHandler.Update("Application_Documents", applicationDocumentProperties, "DocumentNo", applicationDocumentProperties.DocumentNo);
                }
                retval = 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retval;
        }

        public GetVerifiedDocumentResponse VerifiedDocumentDetails(GetDocumentRequest request)
        {
            GetVerifiedDocumentResponse response = new GetVerifiedDocumentResponse();

            List<VerifiedDocumentProperties> viewDocumentProperties = new List<VerifiedDocumentProperties>();
            DocumentDataProperties loandetails = new DocumentDataProperties();

            decimal rolecnt = new OracleHelper().ExecuteScalar<decimal>("select count(*) from vw_cv_user_role_details  t1 left outer join vw_cv_user_master t2 ON  t1.user_id = t2.user_id WHERE t2.emp_code = '" + request.enterBy + "' and t1.role_id=83");

            if (rolecnt > 0)
            {

                viewDocumentProperties = new OracleHelper().GetRecords<VerifiedDocumentProperties>("select dm.customer_name, case dm.applicant_type when 'P' then 'Primary Applicant' when 'C' then 'Co-Applicant' when 'G' then 'Guarator' end applicant_type, vw.document_description, case when dm.document_no is null then dm.old_document_no else dm.document_no end docno," +
                    " dm.nho_verify_by, to_char(dm.nho_verified_date) nho_verified_date, dm.nho_verified_remarks, dm.rho_verify_by, to_char(dm.rho_verified_date) rho_verified_date, dm.rho_remarks, dm.sendback_by, to_char(dm.sendback_dt) sendback_dt, dm.sendback_remarks,dm.rho_sendback_by,to_char(dm.rho_sendback_date) rho_sendback_date,dm.rho_sendback_remarks, dm.recaptured_by, to_char(dm.recaptured_dt) recaptured_dt, dm.recaptured_remarks from tbl_oversite_master om left outer join tbl_oversite_documents_mst dm on om.application_id = dm.application_id " +
                    "left outer join tbl_ops_oversite_master oom on om.application_id = oom.application_id left outer join vw_document_master vw on dm.document_type = vw.document_id and om.product_id = vw.product_id where (om.application_id = '" + request.searchType + "' or om.loan_id = '" + request.searchType + "')  order by /*docno,*/nho_verified_date,rho_verified_date,sendback_dt,rho_sendback_date,recaptured_dt");


                loandetails = new OracleHelper().GetRecord<DocumentDataProperties>("select t.customer_name customerName,to_char(t.application_id) applicationId,to_char(t.loan_id) loanID, p.product_name productName, t.product_id productId , s.scheme_name schemeName, to_char(t.scheme_id) schemeId , to_char(t.loandate, 'dd-mm-yyyy HH24:mi:ss') loanDate, to_char(t.loan_amount) loanAmount," +
                    " b.branch_name branch,t.decision_flag  decisionId,t.risk_category categoryId from TBL_OVERSITE_MASTER t left outer join product_master p on t.product_id = p.product_id left outer join cv_los.scheme_master s on t.scheme_id = s.scheme_id and t.product_id=s.product_id left outer join cv_los.branch_master b on t.branch_id = b.branch_id " +
                    "where (t.application_id = '" + request.searchType + "' or t.loan_id = '" + request.searchType + "')");
                if (viewDocumentProperties.Count > 0 && loandetails !=null)
                {
                    response.documentList = viewDocumentProperties;
                    response.LoanList = loandetails;
                    response.isDataAvailable = true;
                }
                else
                {
                    response.message = "Please check entered data "+request.searchType+" !!!";

                }

            }
            else
            {
                response.message = "You have no permission !!!";
            }

            return response;
        }
    }

}


using Oracle.ManagedDataAccess.Client;
using Oversite.DTO.Email.Properties;
using Oversite.DTO.Email.Request;
using Oversite.DTO.Email.Response;
using Oversite.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Oversite.Core.DataSource.Email
{
    public class QueryEmailDataSource
    {
        MailManager manager = new MailManager();
        public QueryEmailResponse QueryMail()
        {
            QueryEmailResponse response = new QueryEmailResponse();
            QueryEmailRequest request = new QueryEmailRequest();
            List<QueryEmailProperties> mails = new List<QueryEmailProperties>();

            List<RMProperties> rMs = new List<RMProperties>();
            DataTable dt = new DataTable();
            OracleDataAdapter da = new OracleDataAdapter();
            List<string> cmd = new List<string>();
                  

            try
            {
                int m = 0;
                int j = 0;
                int k = 0;
                TimeSpan remaindate;
                string subject = "DOCUMENT RECTIFICATION";
                string data = ExporttoHtml();
                string maildata = string.Empty;
                string body = "";
                string mailto = "vishnu.r@mactech.net.in";
                //request.queryEmails = new OracleHelper().GetRecords<QueryEmailProperties>("select to_number(t.sendback_flag) SendBackflag,to_date(to_char(t.sendback_dt)) Sendback_date,to_number(t.application_id) Application_id,to_number(t.loan_id) Loan_id from TBL_OVERSITE_DOCUMENTS_MST t where t.sendback_flag = 1 and t.doc_status in (3 , 5)");

                request.queryEmails = new OracleHelper().GetRecords<QueryEmailProperties>("select to_number(t.nch_sendback_flag) SendBackflag, to_date(to_char(t.sendback_dt)) Sendback_date, to_number(t.application_id) Application_id, to_number(t.loan_id) Loan_id from TBL_OVERSITE_DOCUMENTS_MST t where t.nch_sendback_flag = 1  union all select to_number(t.nho_sendback_flag) SendBackflag, to_date(to_char(t.sendback_dt)) Sendback_date, to_number(t.application_id) Application_id, to_number(t.loan_id) Loan_id from TBL_OVERSITE_DOCUMENTS_MST t where t.nho_sendback_flag = 1");

                request.RMsMails = new OracleHelper().GetRecords<RMProperties>("select to_number(t.emp_code) emp_code,to_char(t.emailid) email_id From user_master t left outer join user_role_details r on r.user_id = t.user_id where r.role_id = 48 and t.product_id = 43");

                //string query = "select to_number(t.application_id) Application_id from TBL_OVERSITE_DOCUMENTS_MST t where t.nch_sendback_flag = 1 union all select to_number(t.application_id) Application_id from TBL_OVERSITE_DOCUMENTS_MST t where t.nho_sendback_flag = 1";

                for ( m = 0; m < request.queryEmails.Count; m++)
                {

                    DateTime sendb = request.queryEmails[m].Sendback_date;
                    DateTime dd = DateTime.Now;

                    //var body[] 
                   // List<string> termsList = new List<string>();
                    remaindate = dd - sendb;
                    if (remaindate.Days == 3 || remaindate.Days == 5 || remaindate.Days == 7)
                    {
                        
                        for (int c = 0; c < request.RMsMails.Count; c++)
                        {
                            decimal Log_id = new OracleHelper().ExecuteScalar<decimal>("select to_number(value) from key_master where key_id =1043 and firm_id = 1 and branch_id = 0 and module_id = 3 and product_id=0");
                            cmd.Add("update key_master t set t.value= t.value+1  where t.firm_id=1 and t.branch_id=0 and t.module_id=3 and t.key_id=1043 and t.product_id=0");

                            //termsList.Add(request.queryEmails[m].Application_id.ToString() + " ," + "");
                            body += request.queryEmails[m].Application_id.ToString() + "," + "";
                            
                            //string maildata = "Dear sir , \r\n Please Verify And Close the Query Raised By NCH/NHO on " + request.queryEmails[m].Application_id + "";
                             //maildata = data + " ," + request.queryEmails[m].Sendback_date.ToString("dd-MM-yyyy") + "";
                            //string mailto = request.RMsMails[k].email_id;
                            cmd.Add("insert into OVERSITE_EMAIL_LOG(LOG_ID,APPLICATION_ID,SENDBACK_DATE,EMAIL_FLAG)values(" + Log_id + "," + request.queryEmails[m].Application_id + ",'" + request.queryEmails[m].Sendback_date.ToString("dd-MMM-yyyy") + "',1)");
                            new OracleHelper().ExecuteNonQuerynew(cmd.ToArray(), OracleHelper.SQLMode.Query, null);

                        }
                        
                         //body  +=  " ," + termsList.ToString() + "";
                       // maildata += data + " ," + termsList.ToString() + "";



                      response.isDataAvailable = true;
                        response.message = "Alert Message Send Successfully";
                    }
                    else
                    {
                        response.isDataAvailable = true;
                        response.message = "No data ";
                    }

                    
                    //return response;
                }
                maildata = data + body + ".";
                manager.SendMail(subject, maildata, mailto);

                response.isDataAvailable = true;
                response.message = "There Are No SendBack Documents";
                return response;


               
            }
            catch (Exception ex)
            {
                response.isDataAvailable = false;
                response.message = ex.Message;

            }
            return response;
        }



        public static string ExporttoHtml()

        {
            string html = "<tr><th> Dear sir ,  </th></tr> <br>  <tr><th><tr><th> Please Verify And Close the Query Raised By NCH/NHO for Applications : </th></tr></th></tr>";

            return html;
        }
        //return html


        public TotalDisResponse TotalDisb()
        {
            TotalDisResponse response = new TotalDisResponse();
            TotalDisbRequest request = new TotalDisbRequest();
            List<TotalDisbProperties> disb = new List<TotalDisbProperties>();

            try
            {
                decimal count = new OracleHelper().ExecuteScalar<decimal>("select count(*) from disbursement_details t  left outer join application_mst m on t.application_id = m.application_id and m.application_status = 7 where to_char(t.disbursed_date) = to_char(sysdate)");
                request.totalDisbs = new OracleHelper().GetRecords<TotalDisbProperties>("select to_number(t.emp_code) emp_code, to_char(t.emailid) email_id From user_master t left outer join user_role_details r on r.user_id = t.user_id where r.role_id in (49, 50) and t.product_id = 43");

                for (int i = 0; i < request.totalDisbs.Count - 1; i++)
                {
                    string subject1 = "TOTAL DISBURSEMENT";
                    // string data = "Total Disbursbed on" + DateTime.Now + " :" + count + "";
                    string data1 = ExporttoHtml1();

                    string dd = DateTime.Now.ToString("dd-MM-yyyy");
                    string data = data1 + dd + "     Count = " + count + "";


                    string to = request.totalDisbs[i].email_id;
                    manager.SendMail(subject1, data, to);
                }
                response.isDataAvailable = true;
                response.message = "SUCCESS";
                return response;
            }
            catch (Exception e)
            {
                response.isDataAvailable = false;
                response.message = e.Message;
            }
            return response;
        }

        public static string ExporttoHtml1()

        {
            string html = "<tr><th> Dear sir ,  </th></tr> <br>  <tr><th> Total Disbursbed on : </th></tr>";

            return html;
        }



        public NCHResponse NCHMail()
        {
            NCHResponse response = new NCHResponse();
            NCHRequest request1 = new NCHRequest();
            //List<NCHProperties> nCHes = new List<NCHProperties>();
            //List<NCHApplicationProperties> applicationList = new List<NCHApplicationProperties>();
            TotalDisbRequest request = new TotalDisbRequest();
            DataTable dt = new DataTable();


            try
            {
                //request.nCHProperties = new OracleHelper().GetRecords<NCHProperties>("select to_number(t.nch_sendback_flag) SendBackflag, to_date(to_char(t.sendback_dt)) Sendback_date, to_number(t.application_id) Application_id, to_number(t.loan_id) Loan_id from TBL_OVERSITE_DOCUMENTS_MST t where t.nch_sendback_flag = 2 union all select to_number(t.nho_sendback_flag) SendBackflag, to_date(to_char(t.sendback_dt)) Sendback_date, to_number(t.application_id) Application_id, to_number(t.loan_id) Loan_id from TBL_OVERSITE_DOCUMENTS_MST t where t.nho_sendback_flag = 2");

                //string query = "select to_number(t.nch_sendback_flag) SendBackflag, to_date(to_char(t.sendback_dt)) Sendback_date, to_number(t.application_id) Application_id, to_number(t.loan_id) Loan_id from TBL_OVERSITE_DOCUMENTS_MST t where t.nch_sendback_flag = 2 union all select to_number(t.nho_sendback_flag) SendBackflag, to_date(to_char(t.sendback_dt)) Sendback_date, to_number(t.application_id) Application_id, to_number(t.loan_id) Loan_id from TBL_OVERSITE_DOCUMENTS_MST t where t.nho_sendback_flag = 2";

                // applicationList = new OracleHelper().GetRecords<NCHApplicationProperties>("select t.application_id Application_id from TBL_OVERSITE_DOCUMENTS_MST t where t.nch_sendback_flag = 2 union all select t.application_id from TBL_OVERSITE_DOCUMENTS_MST t where t.nho_sendback_flag = 2");

                string query1 = "select t.application_id Application_id from TBL_OVERSITE_DOCUMENTS_MST t where t.nch_sendback_flag = 2 union all select t.application_id from TBL_OVERSITE_DOCUMENTS_MST t where t.nho_sendback_flag = 2";
                string header = "Application ID";
                request.totalDisbs = new OracleHelper().GetRecords<TotalDisbProperties>("select to_number(t.emp_code) emp_code, to_char(t.emailid) email_id From user_master t left outer join user_role_details r on r.user_id = t.user_id where r.role_id in (49, 50) and t.product_id = 43");

                for (int i = 0; i < request.totalDisbs.Count - 1; i++)
                {


                    dt = new OracleHelper().ExecuteDataSet(query1).Tables[0];

                    string subject = "Updated Sendback Documents";
                    string bodyn = ConvertDataTableToHTML(dt, header);
                    string maildata = bodyn;
                    string to = request.totalDisbs[i].email_id;
                    //string to1 = request.totalDisbs[1].email_id;
                    manager.SendMail(subject, maildata, to);


                }
                // manager.SendMail(subject, maildata, to1);


                //for (int j = 0; j<request.nCHProperties.Count - 1; j++)
                //{

                //}
                response.isDataAvailable = true;
                response.message = "Success";
            }
              
            catch (Exception ex)
            {
                response.isDataAvailable = false;
                response.message = ex.Message;
            }
            return response;
        }
        public static string ConvertDataTableToHTML(DataTable dt, string header)
        {
            string html = "<b><h4>" + header + "</h4></b> ";
            //add header row
            html += " <table border = 1>";
            html += "<tr> <th>";
            for (int i = 0; i < dt.Columns.Count; i++)
                html += "<td><th>" + dt.Columns[i].ColumnName + "</th></td>";
            html += "</th></tr>";
            //add rows
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                html += "<tr><td>";
                for (int j = 0; j < dt.Columns.Count; j++)
                    html += "<td><th>" + dt.Rows[i][j].ToString() + "</th></td>";
                html += "</td></tr>";

            }
            html += "</table>";
            html += "<br>";
            return html;
        }
    }
}

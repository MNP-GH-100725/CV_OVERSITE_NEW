
using Newtonsoft.Json;
using Oversite.DTO.ErrorLog.Request;
using Oversite.DTO.Reports.Properties;
using Oversite.DTO.Reports.Request;
using Oversite.DTO.Reports.Response;
using Oversite.DTO.Response;
using Oversite.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Oversite.Core.DataSource.Reports
{
    public class ProductReportDataSource
    {


        public ProductDataResponse product (ProductDataRequest request)
        {
            ProductDataResponse response = new ProductDataResponse();
            List<ProductDataProperties> productlist = new List<ProductDataProperties>();
            string query = string.Empty;
            try
            {
                //query = "select t.product_id productId, u.role_id roleId from cv_los.user_master t left outer join cv_los.user_role_details u on t.user_id=u.user_id where t.emp_code = '" + request.enterBy+ "'and u.role_id in(48,49,50,52,55,7,19,74,73)";
                query = "select distinct t.product_id productId, u.role_id roleId from cv_los.user_master t left outer join cv_los.user_role_details u on t.user_id=u.user_id INNER JOIN cv_los.oversite_role_master r on u.role_id=r.role_id where t.emp_code = '" + request.enterBy+ "'";

                productlist = new OracleHelper().GetRecords<ProductDataProperties>(query);
                if(productlist.Count==0)
                {
                    query = "select t.product_id productId, u.role_id roleId from user_master t left outer join user_role_details u on t.user_id=u.user_id where t.emp_code = '" + request.enterBy + "'and t.product_id not in(42,43,44,45,46) and u.role_id in(48,49,50,52,55,7,19,74,73)";
                    response.ProductData = new OracleHelper().GetRecords<ProductDataProperties>(query);
                }
                else
                {  query = "select t.product_id productId, u.role_id roleId from cv_los.user_master t left outer join cv_los.user_role_details u on t.user_id=u.user_id  INNER JOIN cv_los.oversite_role_master r on u.role_id=r.role_id  where t.emp_code = '" + request.enterBy + "'";
                    //query = "select t.product_id productId, u.role_id roleId from cv_los.user_master t left outer join cv_los.user_role_details u on t.user_id=u.user_id where t.emp_code = '" + request.enterBy + "'and u.role_id in(48,49,50,52,55,7,19,74,73)";
                    response.ProductData = new OracleHelper().GetRecords<ProductDataProperties>(query);
                }
                if (response.ProductData.Count>0 )
                {
                    response.IsDataAvailable = true;
                    response.message = ResponseTypeContants.SUCCESS;
                }
                else
                {
                    response.IsDataAvailable = false;
                    response.message = "You have no permission";
                }
            }
            catch (Exception ex)
            {
                response.IsDataAvailable = false;
                response.message = ex.Message;
                return response;
            }
            return response;
        }



        public VerificationStatusReportResponse VerificationStatusReport(VerificationStatusReportRequest request)
        {
            VerificationStatusReportResponse response = new VerificationStatusReportResponse();
            string query = string.Empty;
            try
            {

                //rch report
                if (request.roleid == "78")
                {
                    query = "SELECT t.region_id, rm.region_name,'Verify 100% files disbursed within 72 Hours of disbursement' as Norm, COUNT(DISTINCT t.loan_id) AS no_of_disbursed, COUNT(DISTINCT CASE WHEN t.rch_verify = 1 THEN t.loan_id END) AS no_of_rch_verified, COUNT(DISTINCT CASE WHEN ot.statusid = 8 AND ot.tat <= 3 THEN ot.loanid END) AS no_of_rch_verified_asper_TAT, Round((COUNT(DISTINCT CASE WHEN ot.statusid = 8 AND ot.tat <= 3 THEN ot.loanid END) / COUNT(DISTINCT t.loan_id)) * 100) ||'%' AS Achivement_Percentage," +
                        " COUNT(DISTINCT CASE WHEN t.rch_sendback IS NULL AND t.rch_verify = 1 THEN t.loan_id END) AS No_Of_Files_Without_Observ, COUNT(DISTINCT CASE WHEN t.rch_sendback IS NOT NULL THEN t.loan_id END) AS No_Of_Files_With_Observ, COUNT(DISTINCT CASE WHEN ot.decision_flag = 2 AND ot.risk_category = 1 AND ot.statusid = 10 AND t.rch_verify is null AND ot.senbackflag = 1 THEN t.loan_id END) AS MTD_No_Of_Files_WithHigh, COUNT(DISTINCT CASE WHEN ot.decision_flag = 2 AND ot.risk_category = 3 AND ot.statusid = 10 AND t.rch_verify is null AND ot.senbackflag = 1 THEN t.loan_id END) AS MTD_No_Of_Files_WithMedium, COUNT(DISTINCT CASE WHEN ot.decision_flag = 2 AND ot.risk_category = 2 AND ot.statusid = 10 AND t.rch_verify is null AND ot.senbackflag = 1 THEN t.loan_id END) AS MTD_No_Of_Files_WithLow," +
                        " COUNT(DISTINCT CASE WHEN ot.decision_flag = 2 AND ot.risk_category = 1 AND ot.statusid = 10 AND t.rch_verify=1 AND ot.senbackflag = 1 THEN t.loan_id END) AS No_Of_Files_WithHigh, COUNT(DISTINCT CASE WHEN ot.decision_flag = 2 AND ot.risk_category = 3 AND ot.statusid = 10 AND t.rch_verify=1 AND ot.senbackflag = 1 THEN t.loan_id END) AS No_Of_Files_WithMedium, COUNT(DISTINCT CASE WHEN ot.decision_flag = 2 AND ot.risk_category = 2 AND ot.statusid = 10 AND t.rch_verify=1 AND ot.senbackflag = 1 THEN t.loan_id END) AS No_Of_Files_WithLow" +
                        " FROM tbl_oversite_master t LEFT OUTER JOIN tbl_oversite_TATDetails ot ON t.loan_id = ot.loanid LEFT OUTER JOIN cv_los.region_master rm ON t.region_id = rm.region_id WHERE t.disbursed_date BETWEEN to_date('" + request.FromDate + "','DD-MM-yyyy') AND to_date('" + request.Todate + "','DD-MM-yyyy') and t.product_id=" + request.productid + " GROUP BY t.region_id, rm.region_name";
                    response.verificationStatusReports = new OracleHelper().GetRecords<VerificationStatusReportProperties>(query);
                }
                //nch report
                else if (request.roleid == "77")
                {
                    query = "SELECT t.region_id, rm.region_name, 'Verify 20% of the files verified by Regional Team' as Norm, COUNT(DISTINCT t.loan_id) AS no_of_disbursed, COUNT(DISTINCT CASE WHEN t.nch_verify = 1 THEN t.loan_id END) AS no_of_rch_verified, COUNT(DISTINCT CASE WHEN ot.statusid = 2 AND ot.tat <= 3 THEN ot.loanid END) AS no_of_rch_verified_asper_TAT, Round((COUNT(DISTINCT CASE WHEN ot.statusid = 2 AND ot.tat <= 3 THEN ot.loanid END) / COUNT(DISTINCT t.loan_id)) * 100) ||'%' AS Achivement_Percentage," +
                        " COUNT(DISTINCT CASE WHEN t.nch_sendback IS NULL AND t.nch_verify = 1 THEN t.loan_id END) AS No_Of_Files_Without_Observ, COUNT(DISTINCT CASE WHEN t.nch_sendback IS NOT NULL THEN t.loan_id END) AS No_Of_Files_With_Observ, COUNT(DISTINCT CASE WHEN ot.decision_flag = 2 AND ot.risk_category = 1 AND ot.statusid = 3 AND t.nch_verify is null AND ot.senbackflag = 1 THEN t.loan_id END) AS MTD_No_Of_Files_WithHigh, COUNT(DISTINCT CASE WHEN ot.decision_flag = 2 AND ot.risk_category = 3 AND ot.statusid = 3 AND t.nch_verify is null AND ot.senbackflag = 1 THEN t.loan_id END) AS MTD_No_Of_Files_WithMedium, COUNT(DISTINCT CASE WHEN ot.decision_flag = 2 AND ot.risk_category = 2 AND ot.statusid = 3 AND t.nch_verify is null AND ot.senbackflag = 1 THEN t.loan_id END) AS MTD_No_Of_Files_WithLow," +
                        " COUNT(DISTINCT CASE WHEN ot.decision_flag = 2 AND ot.risk_category = 1 AND ot.statusid = 3 AND t.nch_verify = 1 AND ot.senbackflag = 1 THEN t.loan_id END) AS No_Of_Files_WithHigh, COUNT(DISTINCT CASE WHEN ot.decision_flag = 2 AND ot.risk_category = 3 AND ot.statusid = 3 AND t.nch_verify = 1 AND ot.senbackflag = 1 THEN t.loan_id END) AS No_Of_Files_WithMedium, COUNT(DISTINCT CASE WHEN ot.decision_flag = 2 AND ot.risk_category = 2 AND ot.statusid = 3 AND t.nch_verify = 1 AND ot.senbackflag = 1 THEN t.loan_id END) AS No_Of_Files_WithLow" +
                        " FROM tbl_oversite_master t LEFT OUTER JOIN tbl_oversite_TATDetails ot ON t.loan_id = ot.loanid LEFT OUTER JOIN cv_los.region_master rm ON t.region_id = rm.region_id WHERE t.disbursed_date BETWEEN to_date('" + request.FromDate + "', 'DD-MM-yyyy') AND to_date('" + request.Todate + "', 'DD-MM-yyyy') /*and t.product_id in(10,11,12,13) */ and t.product_id=" + request.productid + "  GROUP BY t.region_id, rm.region_name";
                    response.verificationStatusReports = new OracleHelper().GetRecords<VerificationStatusReportProperties>(query);
                }
                else if (request.roleid == "80")
                {
                    query = "SELECT t.region_id, rm.region_name, to_char(COUNT(DISTINCT t.loan_id)) as Norm, COUNT(DISTINCT t.loan_id) AS no_of_disbursed, COUNT(DISTINCT CASE WHEN (op.roh_verify is not null or op.ROH_SENDBACK is not null) THEN t.loan_id END) AS no_of_rch_verified, COUNT(DISTINCT CASE WHEN (ot.statusid = 11 or ot.statusid = 12) THEN ot.loanid END) AS no_of_rch_verified_asper_TAT, Round((COUNT(DISTINCT CASE WHEN (op.roh_verify is not null or op.ROH_SENDBACK is not null) THEN t.loan_id END) / COUNT(DISTINCT t.loan_id) * 100)) || '%' AS Achivement_Percentage, COUNT(DISTINCT CASE WHEN op.roh_sendback IS NULL AND op.roh_verify = 1 THEN t.loan_id END) AS No_Of_Files_Without_Observ, " +
                            "COUNT(DISTINCT CASE WHEN op.roh_sendback IS NOT NULL THEN t.loan_id END) AS No_Of_Files_With_Observ, COUNT(DISTINCT CASE WHEN op.roh_decision_flag = 2 AND op.roh_risk_category = 1 AND ot.statusid = 12 AND op.roh_verify =1 AND op.roh_sendback = 1 THEN t.loan_id END) AS MTD_No_Of_Files_WithHigh, COUNT(DISTINCT CASE WHEN op.roh_decision_flag = 2 AND op.roh_risk_category = 3 AND ot.statusid = 12 AND op.roh_verify =1 AND op.roh_sendback = 1 THEN t.loan_id END) AS MTD_No_Of_Files_WithMedium, COUNT(DISTINCT CASE WHEN op.roh_decision_flag = 2 AND op.roh_risk_category = 2 AND ot.statusid = 12 AND op.roh_verify =1 AND op.roh_sendback = 1 THEN t.loan_id END) AS MTD_No_Of_Files_WithLow, " +
                            "COUNT(DISTINCT CASE WHEN op.roh_decision_flag = 2 AND op.roh_risk_category = 1 AND ot.statusid = 12 AND op.roh_verify = 1 AND op.roh_sendback=2 THEN t.loan_id END) AS No_Of_Files_WithHigh, COUNT(DISTINCT CASE WHEN op.roh_decision_flag = 2 AND op.roh_risk_category = 3 AND ot.statusid = 12 AND op.roh_verify = 1 AND op.roh_sendback=2 THEN t.loan_id END) AS No_Of_Files_WithMedium, COUNT(DISTINCT CASE WHEN op.roh_decision_flag = 2 AND op.roh_risk_category = 2 AND ot.statusid = 12 AND op.roh_verify = 1 AND op.roh_sendback=2 THEN t.loan_id END) AS No_Of_Files_WithLow FROM lms_tw.tbl_oversite_master t left outer join lms_tw.tbl_ops_oversite_master op on t.application_id=op.application_id and t.loan_id=op.loan_id " +
                            "LEFT OUTER JOIN lms_tw.tbl_oversite_TATDetails ot ON t.loan_id = ot.loanid LEFT OUTER JOIN cv_los.region_master rm ON t.region_id = rm.region_id WHERE to_date(t.disbursed_date) BETWEEN to_date('" + request.FromDate + "', 'DD-MM-yyyy') AND to_date('" + request.Todate + "', 'DD-MM-yyyy')/* and t.product_id in (10, 11, 12, 13)*/ and t.product_id=" + request.productid + " GROUP BY t.region_id, rm.region_name";
                    response.verificationStatusReports = new OracleHelper().GetRecords<VerificationStatusReportProperties>(query);
                }
                else if (request.roleid == "81")
                {
                    if (request.flag == 1)
                    {
                        query = "SELECT t.region_id, rm.region_name, to_char(round(COUNT(DISTINCT t.loan_id) * (20 / 100))) as Norm, COUNT(DISTINCT t.loan_id) AS no_of_disbursed, COUNT(DISTINCT CASE WHEN (t.nho_verify is not null or t.nho_sendback is not null) THEN t.loan_id END) AS no_of_rch_verified, COUNT(DISTINCT CASE WHEN (ot.statusid = 4 or ot.statusid = 5) THEN ot.loanid END) AS no_of_rch_verified_asper_TAT, Round((COUNT(DISTINCT CASE WHEN (t.nho_verify is not null or t.nho_sendback is not null) THEN t.loan_id END) / (COUNT(DISTINCT t.loan_id) * (20 / 100)) * 100)) || '%' AS Achivement_Percentage, COUNT(DISTINCT CASE WHEN t.nho_sendback IS NULL and t.nho_verify = 1 THEN t.loan_id END) AS No_Of_Files_Without_Observ, COUNT(DISTINCT CASE WHEN t.nho_sendback IS NOT NULL THEN t.loan_id END) AS No_Of_Files_With_Observ," +
                            " COUNT(DISTINCT CASE WHEN t.nho_decision_flag = 2 AND t.nho_risk_category = 1 AND ot.statusid = 5 AND t.nho_verify is null AND t.nho_sendback is not null THEN t.loan_id END) AS MTD_No_Of_Files_WithHigh, COUNT(DISTINCT CASE WHEN t.nho_decision_flag = 2 AND t.nho_risk_category = 3 AND ot.statusid = 5 AND t.nho_verify is null AND t.nho_sendback is not null THEN t.loan_id END) AS MTD_No_Of_Files_WithMedium, COUNT(DISTINCT CASE WHEN t.nho_decision_flag = 2 AND t.nho_risk_category = 2 AND ot.statusid = 5 AND t.nho_verify is null AND t.nho_sendback is not null THEN t.loan_id END) AS MTD_No_Of_Files_WithLow, COUNT(DISTINCT CASE WHEN t.nho_decision_flag = 1 AND t.nho_risk_category = 1 AND ot.statusid = 4 AND t.nho_verify =1 AND t.nho_sendback is not null THEN t.loan_id END) AS No_Of_Files_WithHigh, " +
                            "COUNT(DISTINCT CASE WHEN t.nho_decision_flag = 1 AND t.nho_risk_category = 3 AND ot.statusid = 4 AND t.nho_verify =1 AND t.nho_sendback is not null THEN t.loan_id END) AS No_Of_Files_WithMedium, COUNT(DISTINCT CASE WHEN t.nho_decision_flag = 1 AND t.nho_risk_category = 2 AND ot.statusid = 4 AND t.nho_verify =1 AND t.nho_sendback is not null THEN t.loan_id END) AS No_Of_Files_WithLow FROM lms_tw.tbl_oversite_master t LEFT OUTER JOIN lms_tw.tbl_oversite_TATDetails ot ON t.loan_id = ot.loanid LEFT OUTER JOIN " +
                            "cv_los.region_master rm ON t.region_id = rm.region_id WHERE to_date(t.disbursed_date) BETWEEN to_date('" + request.FromDate + "', 'DD-MM-yyyy') AND to_date('" + request.Todate + "', 'DD-MM-yyyy') and t.product_id=" + request.productid + " GROUP BY t.region_id, rm.region_name";
                        response.verificationStatusReports = new OracleHelper().GetRecords<VerificationStatusReportProperties>(query);
                    }
                    if (request.flag == 2)
                    {
                        query = "SELECT t.region_id, rm.region_name, to_char(COUNT(DISTINCT t.loan_id)) as Norm, COUNT(DISTINCT t.loan_id) AS no_of_disbursed, COUNT(DISTINCT CASE WHEN (op.roh_verify is not null or op.ROH_SENDBACK is not null) THEN t.loan_id END) AS no_of_rch_verified, COUNT(DISTINCT CASE WHEN (ot.statusid = 11 or ot.statusid = 12) THEN ot.loanid END) AS no_of_rch_verified_asper_TAT, Round((COUNT(DISTINCT CASE WHEN (op.roh_verify is not null or op.ROH_SENDBACK is not null) THEN t.loan_id END) / COUNT(DISTINCT t.loan_id) * 100)) || '%' AS Achivement_Percentage, COUNT(DISTINCT CASE WHEN op.roh_sendback IS NULL AND op.roh_verify = 1 THEN t.loan_id END) AS No_Of_Files_Without_Observ, " +
                            "COUNT(DISTINCT CASE WHEN op.roh_sendback IS NOT NULL THEN t.loan_id END) AS No_Of_Files_With_Observ, COUNT(DISTINCT CASE WHEN op.roh_decision_flag = 2 AND op.roh_risk_category = 1 AND ot.statusid = 12 AND op.roh_verify =1 AND op.roh_sendback = 1 THEN t.loan_id END) AS MTD_No_Of_Files_WithHigh, COUNT(DISTINCT CASE WHEN op.roh_decision_flag = 2 AND op.roh_risk_category = 3 AND ot.statusid = 12 AND op.roh_verify =1 AND op.roh_sendback = 1 THEN t.loan_id END) AS MTD_No_Of_Files_WithMedium, COUNT(DISTINCT CASE WHEN op.roh_decision_flag = 2 AND op.roh_risk_category = 2 AND ot.statusid = 12 AND op.roh_verify =1 AND op.roh_sendback = 1 THEN t.loan_id END) AS MTD_No_Of_Files_WithLow, " +
                            "COUNT(DISTINCT CASE WHEN op.roh_decision_flag = 2 AND op.roh_risk_category = 1 AND ot.statusid = 12 AND op.roh_verify = 1 AND op.roh_sendback=2 THEN t.loan_id END) AS No_Of_Files_WithHigh, COUNT(DISTINCT CASE WHEN op.roh_decision_flag = 2 AND op.roh_risk_category = 3 AND ot.statusid = 12 AND op.roh_verify = 1 AND op.roh_sendback=2 THEN t.loan_id END) AS No_Of_Files_WithMedium, COUNT(DISTINCT CASE WHEN op.roh_decision_flag = 2 AND op.roh_risk_category = 2 AND ot.statusid = 12 AND op.roh_verify = 1 AND op.roh_sendback=2 THEN t.loan_id END) AS No_Of_Files_WithLow FROM lms_tw.tbl_oversite_master t left outer join lms_tw.tbl_ops_oversite_master op on t.application_id=op.application_id and t.loan_id=op.loan_id " +
                            "LEFT OUTER JOIN lms_tw.tbl_oversite_TATDetails ot ON t.loan_id = ot.loanid LEFT OUTER JOIN cv_los.region_master rm ON t.region_id = rm.region_id WHERE to_date(t.disbursed_date) BETWEEN to_date('" + request.FromDate + "', 'DD-MM-yyyy') AND to_date('" + request.Todate + "', 'DD-MM-yyyy')/* and t.product_id in (10, 11, 12, 13)*/ and t.product_id=" + request.productid + " GROUP BY t.region_id, rm.region_name";
                        response.verificationStatusReports = new OracleHelper().GetRecords<VerificationStatusReportProperties>(query);
                    }
                }
                if (response.verificationStatusReports == null)
                {
                    response.IsDataAvailable = false;
                    response.message = "You have no permission";
                }
                else
                {
                    if (response.verificationStatusReports.Count > 0)
                    {
                        response.IsDataAvailable = true;
                        response.message = ResponseTypeContants.SUCCESS;
                    }
                    else
                    {
                        response.IsDataAvailable = false;
                        response.message = "No Data to show";
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsDataAvailable = false;
                response.message = ex.Message;
                return response;
            }
            return response;
        }
    }
}

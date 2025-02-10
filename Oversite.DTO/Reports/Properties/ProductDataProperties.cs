using System;
using System.Collections.Generic;
using System.Text;

namespace Oversite.DTO.Reports.Properties
{
    public class ProductDataProperties
    {
        
        public int productId { get; set; }
        public int roleId { get; set; }
    }

    public class VerificationStatusReportProperties
    {
        public string region_name { get; set; }
        public string Norm { get; set; }
        public decimal no_of_disbursed { get; set; }
        public decimal no_of_rch_verified { get; set; }
        public decimal no_of_rch_verified_asper_TAT { get; set; }
        public string Achivement_Percentage { get; set; }
        public decimal No_Of_Files_Without_Observ { get; set; }
        public decimal No_Of_Files_With_Observ { get; set; }
        public decimal MTD_No_Of_Files_WithHigh { get; set; }
        public decimal MTD_No_Of_Files_WithMedium { get; set; }
        public decimal MTD_No_Of_Files_WithLow { get; set; }
        public decimal No_Of_Files_WithHigh { get; set; }
        public decimal No_Of_Files_WithMedium { get; set; }
        public decimal No_Of_Files_WithLow { get; set; }

    }
    public class OpVerificationReportProperties

    {
        public string roh_verdate { get; set; }
        public string roh_sendbdate { get; set; }
        public string branch_name { get; set; }
        public string emp_name { get; set; }
        public string emp_code { get; set; }
        public string state_name { get; set; }
        public string loan_id { get; set; }
        public string application_id { get; set; }
        public string customer_name { get; set; }
        public string loan_amount { get; set; }
        public string dismonth { get; set; }
        public string disdate { get; set; }
        public string risk_categ { get; set; }
        public string status { get; set; }
        public string roh_remark { get; set; }
        public decimal above_tat { get; set; }
        public string noh_senbackdate { get; set; }
        public string noh_verifydate { get; set; }
        public string noh_overall_remark { get; set; }


    }
}

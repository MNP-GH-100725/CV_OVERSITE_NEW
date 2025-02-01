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
}

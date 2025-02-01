using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oversite.DTO.Reports.Request
{
    public class GenerateReportRequests
    {
        public int reportId { get; set; }
        public string type { get; set; }
        public string param1 { get; set; }
        public string param2 { get; set; }
        public string param3 { get; set; }
        public string param4 { get; set; }
        public string param5 { get; set; }

        public string param6 { get; set; }
        public string param7 { get; set; }
        public int firmId { get; set; }
        public int productId { get; set; }
        public string userId { get; set; }
    }
    public class GetNewDataReportRequest
    {
        public int reportId { get; set; }
        public int DtlreportId { get; set; }
        public int loan_id { get; set; }
        public string type { get; set; }
        public int FirmID { get; set; }
        public int ProductId { get; set; }
    }
    public class DrilldownReportsRequest
    {
        public string type { get; set; }
        public string report_id { get; set; }
        public string level_no { get; set; }
        public string parms { get; set; }
        public string link_field { get; set; }
        public string link_value { get; set; }
        public int productId { get; set; }
        public int firmId { get; set; }
    }
    public class DataByIdrequest
    {
        public string type { get; set; }
        public string report_id { get; set; }
        //    public string subreport_id { get; set; }
        public string parms { get; set; }
    }
    public class ComboFillRequest
    {
        public int combo_id { get; set; }
        public int report_id { get; set; }
        public int flag { get; set; }// 1 if 1st combo and 2nd combo depends 
        public string id { get; set; }
        public int productId { get; set; }
        public int firmId { get; set; }
    }
    public class ReportsDetailsRequest
    {
        public int productId { get; set; }
        public int firmId { get; set; }
    }

    public class TATDataRequest
    {

        public int FIRM_ID { get; set; }
        public int PRODUCT_ID { get; set; }
        public string APPLICATION_ID { get; set; }
        public int FLAG { get; set; }
        public string EMP_CODE { get; set; }

    }

    public class GenerateReportRequestsOne
    {
        public string reportId { get; set; }
        public string type { get; set; }
        public string param1 { get; set; }
        public string param2 { get; set; }
        public string param3 { get; set; }
        public string param4 { get; set; }
        public string param5 { get; set; }

        public string param6 { get; set; }
        public string param7 { get; set; }
        public string firmId { get; set; }
        public string productId { get; set; }
        public string userId { get; set; }
    }
    public class GeneralParameterRequest
    {

        [Required(ErrorMessage = "Parameter Id is required")]

        [JsonProperty("ParameterID")]
        public int ParameterID { get; set; }

        [Required(ErrorMessage = "Module Id is required")]
        [JsonProperty("ModuleID")]
        public int ModuleID { get; set; }
        [JsonProperty("productId")]
        public int productId { get; set; }

    }
}


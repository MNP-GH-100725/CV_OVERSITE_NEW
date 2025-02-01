using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oversite.DTO.Reports.Properties
{
    public class GetReportsProperties
    {
        public string resultset { get; set; }
    }
    public class ReportPropertyNames
    {
        public string Header { get; set; }
        public string PropertyName { get; set; }
        public string PropertyType { get; set; }

    }
    public class ReportExecuter
    {
        public string output { get; set; }


    }

    public class ReportParamsProperties
    {
        public string report_parm1 { get; set; }
        public string report_parm2 { get; set; }
        public string report_parm22 { get; set; }
        public string report_parm3 { get; set; }
        public string report_parm41 { get; set; }
        public string report_parm42 { get; set; }
        public string report_parm5 { get; set; }
        public string chart_type { get; set; }

    }
    public class QueryProperties
    {
        public string query { get; set; }
        public string header { get; set; }
        public string url { get; set; }
        public string Url_Params { get; set; }
        public string link_fields { get; set; }
        public string x_param { get; set; }
        public string y_param { get; set; }
        // public string parameters { get; set; }
    }
    public class QueryNewReportProperties
    {
        public string query { get; set; }
        public string header { get; set; }
        public string url { get; set; }
        public string Url_Params { get; set; }
        public string link_fields { get; set; }
        public string view_model { get; set; }
        public string main_header { get; set; }
        public string align_status { get; set; }
        public int sub_rpt_id { get; set; }
        public string Style_Format { get; set; }
        public int querry_format { get; set; }
        public int report_dtl_id { get; set; }
        // public string parameters { get; set; }
    }


    public class ComboFillProperties
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
    public class DataByIdProperties
    {
        public string loan_id { get; set; }
        public string installment_no { get; set; }
        public string installment_amount { get; set; }
        public string interest_amount { get; set; }
        public string closing_balance { get; set; }

    }
    public class ReportsDetailsProperties
    {
        public int REPORT_ID { get; set; }

        public string REPORT_NAME { get; set; }

        public string REPORT_PARM1 { get; set; }

        public string REPORT_PARM2 { get; set; }

        public string REPORT_PARM22 { get; set; }

        public string REPORT_PARM3 { get; set; }

        public string REPORT_PARM41 { get; set; }

        public string REPORT_PARM42 { get; set; }

        public string REPORT_PARM5 { get; set; }

        public int STATUS { get; set; }

        public string UPDATED_DATE { get; set; }
    }
    public class GeneralParameterProperties
    {
        [JsonProperty("ParameterValue")]
        public double parameterValue { get; set; }
    }
    public class GeneralParamProperties
    {
        [JsonProperty("ParameterValue")]
        public string parameterValue { get; set; }
    }
}


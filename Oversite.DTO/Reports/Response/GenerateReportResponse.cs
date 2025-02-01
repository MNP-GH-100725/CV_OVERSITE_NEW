using Oversite.DTO.Reports.Properties;
using Oversite.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oversite.DTO.Reports.Response
{
    public class GenerateReportResponse : BaseResponse
    {
        public List<GetReportsProperties> resulset { get; set; }
        public string header { get; set; }
        public string Urls { get; set; }
        public string Url_Params { get; set; }
        public string link_fields { get; set; }
        public int last_level { get; set; }
        public string ChartType { get; set; }
        public string x_param { get; set; }
        public string y_param { get; set; }
        public string message { get; set; }
        public bool isDataAvailable { get; set; }
        public GenerateReportResponse()
        {
            isDataAvailable = false;
        }

        public class GenerateNewReportResponse
        {
            public string message { get; set; }
            public bool isDataAvailable { get; set; }
            public GenerateNewReportResponse()
            {
                isDataAvailable = false;
            }
            public List<GetReportsProperties> resulset { get; set; }
            public List<string> paragraph { get; set; }
            public string header { get; set; }
            public string Urls { get; set; }
            public string Url_Params { get; set; }
            public string link_fields { get; set; }
            public int last_level { get; set; }
            public string view_model { get; set; }
            public string main_header { get; set; }
            public string align_status { get; set; }
            public int sub_rpt_id { get; set; }
            public string Style_Format { get; set; }
            public string DisplayImage { get; set; }
            public string buttonHeader { get; set; }
            public string ChartType { get; set; }
        }

        public class GenerateAllReportResponse
        {
            public string message { get; set; }
            public bool isDataAvailable { get; set; }
            public GenerateAllReportResponse()
            {
                isDataAvailable = false;
            }
            public List<GetReportsProperties> resulset { get; set; }
            public List<string> paragraph { get; set; }
            public List<string> header { get; set; }
            public List<string> Urls { get; set; }
            public List<string> Url_Params { get; set; }
            public string link_fields { get; set; }
            public int last_level { get; set; }
            public List<string> view_model { get; set; }
            public List<string> main_header { get; set; }
            public List<string> align_status { get; set; }
            public List<string> sub_rpt_id { get; set; }
            public string Style_Format { get; set; }
            public string DisplayImage { get; set; }
            public string buttonHeader { get; set; }
            public string ChartType { get; set; }
        }

        public class ComboFillResponse
        {
            public string message { get; set; }
            public bool isDataAvailable { get; set; }
            public ComboFillResponse()
            {
                isDataAvailable = false;
            }
            public List<ComboFillProperties> resultset { get; set; }
        }

        public class ReportsDetailsResponse
        {
            public string message { get; set; }
            public bool isDataAvailable { get; set; }
            public ReportsDetailsResponse()
            {
                isDataAvailable = false;
            }
            public List<ReportsDetailsProperties> reportDetailsList { get; set; }
        }
        public class TATReportResponse
        {
            public List<TATReportProperties> TATData { get; set; }
            public string message { get; set; }
            public bool isDataAvailable { get; set; }
            public TATReportResponse()
            {
                isDataAvailable = false;
            }
        }

        
    }

    public class GenerateReportResponse1 : BaseResponse
    {
        public List<GetReportsProperties> resulset { get; set; }
        public string header { get; set; }
        public string Urls { get; set; }
        public string Url_Params { get; set; }
        public string link_fields { get; set; }
        public int last_level { get; set; }
        public string ChartType { get; set; }
        //public List<ReportChartProperties> chartdata { get; set; }

    }
    public class GeneralParameterResponse : BaseResponse
    {

        public GeneralParameterProperties param_Value { get; set; }
        public GeneralParamProperties param_Values { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oversite.DTO.Reports.Properties
{
    public class GetReportProperties
    {
        public string FUNCTION_ID { get; set; }
        public string FUNCTION_NAME { get; set; }
        public string ROUTER_LINK { get; set; }
        public string HREF { get; set; }
        public string HAS_SUB_MENU { get; set; }
        public string PARENT_ID { get; set; }
        public string ICON { get; set; }
        public string REPORT_ID { get; set; }
        public string REPORT_NAME { get; set; }
        public string REPORT_PARM1 { get; set; }
        public string REPORT_PARM2 { get; set; }
        public string REPORT_PARM22 { get; set; }
        public string REPORT_PARM3 { get; set; }
        public string REPORT_PARM41 { get; set; }
        public string REPORT_PARM42 { get; set; }
        public string REPORT_PARM5 { get; set; }
        public string STATUS { get; set; }
        public string UPDATED_DATE { get; set; }
    }
    public class TATReportProperties
    {
        public string APPLICATION_ID { get; set; }
        public string FUNCTION_NAME { get; set; }
        public string WORK_START_TIME { get; set; }
        public string ASSIGNED_TIME { get; set; }
        public string END_TIME { get; set; }
        public string Empcode { get; set; }
        public string emp_name { get; set; }

    }
}

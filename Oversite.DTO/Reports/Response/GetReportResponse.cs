using Oversite.DTO.Reports.Properties;
using Oversite.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oversite.DTO.Reports.Response
{
    public class GetReportResponse : BaseResponse
    {
        public string message { get; set; }
        public bool isDataAvailable { get; set; }
        public GetReportResponse()
        {
            isDataAvailable = false;
        }
        //public List<GetReportProperties> resultset { get; set; }
        public List<GetReportProperties> resultset = new List<GetReportProperties>();
    }
    public class GetReportExecuterData
    {
        public List<ReportExecuter> dataList { get; set; }


    }
}

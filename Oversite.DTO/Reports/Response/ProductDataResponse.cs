
using Oversite.DTO.Reports.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oversite.DTO.Reports.Response
{
    public class ProductDataResponse 
    {
        public ProductDataResponse()
        {
            IsDataAvailable = false;
        }
        public List<ProductDataProperties> ProductData { get; set; }
  
        public bool IsDataAvailable { get; set; }
        public string message { get; set; }
    }

    public class VerificationStatusReportResponse
    {
        public VerificationStatusReportResponse()
        {
            IsDataAvailable = false;
        }
        public List<VerificationStatusReportProperties> verificationStatusReports { get; set; }

        public bool IsDataAvailable { get; set; }
        public string message { get; set; }
    }
}

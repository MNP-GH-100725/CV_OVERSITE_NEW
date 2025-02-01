using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Oversite.DTO.Reports.Request
{
    public class ProductDataRequest
    {
        public string enterBy { get; set; }
    }

    public class VerificationStatusReportRequest
    {
        public string roleid { get; set; }
        public string FromDate { get; set; }
        public string Todate { get; set; }
        public int productid { get; set; }
        public int flag { get; set; }
    }
    public class OpVerificationReportRequest
    {
        public string roleid { get; set; }
        public string FromDate { get; set; }
        public string Todate { get; set; }
        public int productid { get; set; }
    }
}

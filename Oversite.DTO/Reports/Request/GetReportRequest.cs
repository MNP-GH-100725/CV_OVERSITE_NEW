using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oversite.DTO.Reports.Request
{
    public class GetReportRequest
    {
        public int FirmID { get; set; }
        public int UserID { get; set; }
        public int ProductID { get; set; }
    }
}

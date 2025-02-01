using System;
using System.Collections.Generic;
using System.Text;

namespace Oversite.DTO.ErrorLog.Request
{
    public class ErrorLogRequest
    {
        public string Function_Name { get; set; }
        public string V_Exception { get; set; }
        public string V_Data { get; set; }
        public int Firm_ID { get; set; }
        public int Product_ID { get; set; }
    }
}

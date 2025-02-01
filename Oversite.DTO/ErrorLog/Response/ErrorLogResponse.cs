using System;
using System.Collections.Generic;
using System.Text;

namespace Oversite.DTO.ErrorLog.Response
{
    public class ErrorLogResponse
    {
        public string message { get; set; }
        public string errorStatus { get; set; }
        public bool isDataAvailable { get; set; }
        public string outputmsg { get; set; }
    }
}

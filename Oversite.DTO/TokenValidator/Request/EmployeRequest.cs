using System;
using System.Collections.Generic;
using System.Text;

namespace Oversite.DTO.TokenValidator.Request
{
    public class EmployeRequest
    {
        public string employeeId { get; set; }
        public string type { get; set; }
    }
    public class PreauthRequest
    {
        public string domain { get; set; }
        public string module { get; set; }
        public string environment { get; set; }

    }

    public class GeneratePreauthRequest
    {
        public string request { get; set; }
        public string token { get; set; }
        public string tokenType { get; set; }

    }

    public class ResponsePreauthRequest
    {
        public string request { get; set; }
        public string hash { get; set; }

    }
    public class AddpreauthRequest
    {
        public string domain { get; set; }
        public string module { get; set; }
        public string environment { get; set; }
    }
}

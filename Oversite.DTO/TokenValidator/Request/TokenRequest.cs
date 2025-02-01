using System;
using System.Collections.Generic;
using System.Text;

namespace Oversite.DTO.TokenValidator.Request
{
    public class TokenRequest
    {
        public string employeeId { get; set; }
        public string branchID { get; set; }
        public string token { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Oversite.DTO.TokenValidator.Request
{
    public class SmsRequest
    {
        public string sender { get; set; }
        public string mobileNo { get; set; }
        //public string message { get; set; }

        public string clientID { get; set; }
        public string receipientNo { get; set; }
        public string message { get; set; }
        public string messageType { get; set; }
    }
}

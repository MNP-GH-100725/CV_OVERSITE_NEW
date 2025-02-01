using System;
using System.Collections.Generic;
using System.Text;

namespace Oversite.DTO.TokenValidator.Response
{
    public class TokenResponse
    {
        public bool isValidToken { get; set; }
    }
    public class EmptokenResponse
    {
        public string responseMsg;
        public bool isDataAvailable;
        public string apiStatus;

        public string login_branch_id { get; set; }
        public string token { get; set; }
        public string emp_code { get; set; }

        public string duration { get; set; }
        public string status { get; set; }
        public string ipaddress { get; set; }
        public DateTime starttime { get; set; }
        public DateTime limitstarttime { get; set; }
        public int limit { get; set; }
        public DateTime endtime { get; set; }
        public string login_key { get; set; }
        public DateTime tokenupdatetime { get; set; }
        public string FIRM_ID { get; set; }
        public string product_id { get; set; }
    }
}


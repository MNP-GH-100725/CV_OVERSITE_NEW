using System;
using System.Collections.Generic;
using System.Text;

namespace Oversite.DTO.Email.Properties
{
   public class QueryEmailProperties
    {
        public decimal SendBackflag { get; set; }
        public DateTime Sendback_date { get; set; }
        public decimal Application_id { get; set; }
        public decimal Loan_id { get; set; }
    }

    public class RMProperties
    {
        public decimal emp_code { get; set; }
        public string email_id { get; set; }
    }

    public class TotalDisbProperties
    {
        public decimal emp_code { get; set; }
        public string email_id { get; set; }
    }
}

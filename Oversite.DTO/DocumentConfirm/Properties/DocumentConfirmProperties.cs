using System;
using System.Collections.Generic;
using System.Text;

namespace Oversite.DTO.DocumentConfirm.Properties
{
   public  class DocumentConfirmProperties
    {
    }
    public class DocReConfirmProperties
    {
        public string applicationId { get; set; }
        public string loanId { get; set; }
        public int docType { get; set; }
        public string docNo { get; set; }
        public int productId { get; set; }
        public string customerid { get; set; }
       
    }
    public class TATDetailsProperties
    {
        public string application_id { get; set; }
        public string loan_id { get; set; }
        public string disbursed_date { get; set; }
        public decimal TAT { get; set; }
        public int product_id { get; set; }
        public int firm_id { get; set; }
        public int region_id { get; set; }

    }
}

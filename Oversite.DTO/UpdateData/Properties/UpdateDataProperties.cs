using System;
using System.Collections.Generic;
using System.Text;

namespace Oversite.DTO.UpdateData
{
    public class UpdateDataProperties
    {


        public int applicationId { get; set; }
        public int loanId { get; set; }
        public int documentNo { get; set; }

        public string documentType { get; set; }
        public int oldDocumentNo { get; set; }
        public DateTime nchVerifyDate { get; set; }

        public string nchVerifyBy { get; set; }
        public string nchVerifyRemarks { get; set; }
        public DateTime nhoVerifiedDate { get; set; }

        public string nhoVerifyBy { get; set; }
        public string nhoVerifiedRemarks { get; set; }
        public int verifyFlag { get; set; }
        public int docStatus { get; set; }
        public string docRemarks { get; set; }
        public string sendbackRemarks { get; set; }

        public string sendbackBy { get; set; }
        public DateTime sendbackDt { get; set; }
        public string recapturedRemarks { get; set; }

        public string recapturedBy { get; set; }
        public DateTime recapturedDt { get; set; }
        public int sendbackFlag { get; set; }
        public int productId { get; set; }
    }
    public class DocupdateProperties
    {
        public int applicationId { get; set; }
        public int loanId { get; set; }
        public string documentNo { get; set; }
        public string documentType { get; set; }
        public string applicantType { get; set; }
        public string customerId { get; set; }
        public string sendbackto { get; set; }
    }
}

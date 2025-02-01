using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Oversite.DTO.Documents.Request
{
    public class CommonMasterRequest
    {
        public int commonDataTypeID { get; set; }
    }
    public class GetdocMasterRequest
    {
        public int productId { get; set; }
        public int schemeId { get; set; }
        public String enterBy { get; set; }
        public String applicationId { get; set; }
    }
    public class AccRepaymentScheduleRequest
    {

        [JsonProperty("Loan_Id")]
        public string Loan_Id { get; set; }
    }
    public class LoanDetailsRequest
    {
        [JsonProperty("Loan_Id")]
        public string Loan_Id { get; set; }
    }
    public class SaveRepayDetailsRequest
    {
        public string loanId { get; set; }
        public string repaydecisionflag { get; set; }
        public string repayremarks { get; set; }
    }
}

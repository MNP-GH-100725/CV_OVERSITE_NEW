using Oversite.DTO.CommonMaster.Properties;
using System;
using System.Collections.Generic;
using System.Text;

namespace Oversite.DTO.CommonMaster.Response
{
    public class CommonMasterReponse
    {
        public bool isDataAvailable { get; set; }
        public CommonMasterReponse()
        {
            isDataAvailable = false;
        }
        public string message { get; set; }
        public List<CommonMasterProperties> commonDataList { get; set; }
    }
    public class GetdocMasterReponse
    {
        public List<GetdocProperties> GetDocList { get; set; }

    }
    public class RepaymentScheduleResponse
    {
        public List<RepaymentScheduleProperties> repaymentschList { get; set; }
        public List<RepaymentSumProperties> repaymentTotalList { get; set; }

    }
    public class LoanDetailsResponse
    {
        public List<CustomerDetailsProperties> customerDtlsList { get; set; }
        public List<LoanotherDataPropeties> Customerotherdatalist { get; set; }
        public List<Repaydecisionproperties> Repaylist { get; set; }
        public List<repayemiproperties> instDetailsList { get; set; }

    }
    public class SaveRepayDetailsResponse
    {

    }
}

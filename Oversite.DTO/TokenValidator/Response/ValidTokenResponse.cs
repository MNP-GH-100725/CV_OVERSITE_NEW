using System;
using System.Collections.Generic;
using System.Text;

namespace Oversite.DTO.TokenValidator.Response
{
    public class ValidTokenResponse
    {
        public ValidTokenResponse()
        {
            isDataAvailable = false;
        }
        public bool isDataAvailable { get; set; }
        public int flag { get; set; }
    }
    public class TokenData
    {
        public DateTime ENDTIME { get; set; }

        public DateTime TOKENUPDATETIME { get; set; }
    }
    public class EmployeResponse
    {
        public EmployeResponse()
        {
            IsDataAvailable = false;
            employeeData = new EmployeeData();
        }




        public EmployeeData employeeData { get; set; }
        public List<EmployeeData> employeeDatas { get; set; }
        public BranchMasterResponse branchMasters
        {
            get; set;
        }
        public bool IsDataAvailable { get; set; }

        public string errorStatus { get; set; }
        public string msg { get; set; }
        public int concurrentCheck { get; set; }
        public string concurrentCheckmsg { get; set; }

    }
    public class EmployeeData
    {

        public string userName { get; set; }
        public string userId { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public string branchId { get; set; }
        public string branchName { get; set; }

        public string stateId { get; set; }
        public string stateName { get; set; }

        public string zoneId { get; set; }
        public string zoneName { get; set; }
        public string otpFlag { get; set; }

    }
    public class BranchMasterResponse
    {
        public BranchMasterResponse()
        {
            isDataAvailable = false;
        }
        public bool isDataAvailable { get; set; }
        public List<BranchMasterData> branchMasterProperties { get; set; }
    }
    public class BranchMasterData
    {
        public string branch { get; set; }
        public long branchId { get; set; }
    }
    public class PreauthResponse
    {

        public string message;

        public string token { get; set; }

        public DateTime tokentime { get; set; }

        public List<Getpreauth> getpreauths { get; set; }
    }
    public class Getpreauth
    {
        public string domain { get; set; }
        public string module { get; set; }
        public string environment { get; set; }
        public string hashkey { get; set; }

    }
    public class AddpreauthResponse
    {
        public string responseMsg { get; set; }
        public string apiStatus { get; set; }
        public string status { get; set; }
        public bool isDataAvailable { get; set; }
    }
}

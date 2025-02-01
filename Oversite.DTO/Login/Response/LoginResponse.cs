using Oversite.DTO.Login.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oversite.DTO.Login.Response
{
    public class LoginResponse
    {
        public LoginResponse()
        {
            IsDataAvailable = false;
            loginData = new LoginProperties();
        }
        public LoginProperties loginData { get; set; }
        public List<DatasProperties> Data { get; set; }
        public List<DocumentsDataProperties> RoleList { get; set; }
        public bool IsDataAvailable { get; set; }
        public string message { get; set; }
        public string token { get; set; }
        public int Is_Pass_Expired { get; set; }
        public List<RegionProperties> regionData { get; set; }

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
    public class GetLoginRequests
    {
        public string product_id { get; set; }
        public string Employee_code { get; set; }
        public string function_id { get; set; }
        public string token { get; set; }
    }
    public class EncryptedResponse
    {
        public string message { get; set; }
    }

    public class PreauthResponse
    {

        public string message;

        public string token { get; set; }

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

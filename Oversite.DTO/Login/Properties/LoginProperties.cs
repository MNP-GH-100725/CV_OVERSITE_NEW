using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oversite.DTO.Login.Properties
{
    public class LoginProperties
    {
        public LoginStatus loginStatus { get; set; }
        public int firmID { get; set; }
        public int productID { get; set; }
        public int branchID { get; set; }
        public int stateID { get; set; }
        public string firmName { get; set; }
        public string employeeName { get; set; }
        public string branchName { get; set; }
        public string token { get; set; }
        public string loginKey { get; set; }
        public string empCode { get; set; }

    }
    public enum LoginStatus
    {
        validUser = 1,
        inValidUser = 0

    }
    public class DocumentsDataProperties
    {
        public string statusId { get; set; }
        public string functionId { get; set; }
        public string functionName { get; set; }
        public string roleId { get; set; }
        public string routerLink { get; set; }
    }
    public class DatasProperties
    {

        public string roieId { get; set; } //100823
    }

    public class RegionProperties
    {

        public int region_id { get; set; }
    }
    public class RoleFunctionProperties
    {
        [JsonProperty("functioN_ID")]
        public string FUNCTION_ID { get; set; }
        [JsonProperty("product_id")]
        public string product_id { get; set; }
        public string emp_code { get; set; }
        [JsonProperty("functioN_NAME")]
        public string FUNCTION_NAME { get; set; }
        [JsonProperty("routeR_LINK")]
        public string ROUTER_LINK { get; set; }
        [JsonProperty("href")]
        public string HREF { get; set; }
        [JsonProperty("haS_SUB_MENU")]
        public string HAS_SUB_MENU { get; set; }
        [JsonProperty("parenT_ID")]
        public string PARENT_ID { get; set; }
        [JsonProperty("icon")]
        public string ICON { get; set; }
    }
}

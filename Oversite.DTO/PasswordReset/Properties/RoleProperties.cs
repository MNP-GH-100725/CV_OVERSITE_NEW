using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace Oversite.DTO.PasswordReset.Properties

{
    public class RoleProperties
    {
        [JsonProperty("FirmID")]
        public int FIRM_ID { get; set; }
        [JsonProperty("RoleID")]
        public int ROLE_ID { get; set; }
        [JsonProperty("RoleName")]
        public string ROLE_NAME { get; set; }
        [JsonProperty("RoleDescription")]
        public string ROLE_DESCRIPTION { get; set; }
        [JsonProperty("StatusID")]
        public int STATUS_ID { get; set; }
        [JsonProperty("IsActive")]
        public string IS_ACTIVE { get; set; }       

    }
}

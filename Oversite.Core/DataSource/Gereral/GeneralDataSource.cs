using System;
using System.Collections.Generic;
using System.Text;


using Oracle.ManagedDataAccess.Client;
using CVHelpers;


using System.Linq;


using Newtonsoft.Json;


using Oversite.Helpers;
using Oversite.DTO.Documents.Request;
using Oversite.DTO.CommonMaster.Response;
using Oversite.DTO.Response;
using Oversite.DTO.CommonMaster.Properties;

namespace Oversite.Core.DataSource.General
{
    public class GeneralDataSource
    {
        public DateTime SysDate()
        {
            DateTime dt;

            string query = "select get_sysdate from dual";
            dt = new OracleHelper().ExecuteScalar<DateTime>(query);
            return dt;
        }

        public KeyValueResponse GetKeyValue(int firmId, int branchId, int moduleId, int keyId, int productId)

        {
            KeyValueResponse keyValueResponse = new KeyValueResponse();
            try
            {
                string query = string.Empty;
                if (productId != 0)
                    query = "select value keyvalue from key_master where firm_id = " + firmId + " and branch_id = " + branchId + " and module_id=" + moduleId + "  and key_id=" + keyId + " and product_id=" + productId + "";
                else
                    query = "select value keyvalue from key_master where firm_id = " + firmId + " and branch_id = " + branchId + " and module_id=" + moduleId + "  and key_id=" + keyId + "";

                keyValueResponse.keyvalue = new OracleHelper().ExecuteScalar<object>(query).ToString();
                if (keyValueResponse.keyvalue != null)
                {
                    keyValueResponse.isDataAvailable = true;
                    keyValueResponse.message = ResponseTypeContants.SUCCESS;
                }
                else
                {
                    keyValueResponse.isDataAvailable = false;
                    keyValueResponse.message = "no data found";

                }
            }
            catch (Exception ex)
            {
                keyValueResponse.message = ex.Message;

            }
            return keyValueResponse;
        }
        


        public string UpdateKeyValue(int firmId, int branchId, int moduleId, int keyId, int productId)
        {
            string query = string.Empty;
            if (productId != 0)
                query = "update key_master set value=value+1 where firm_id = " + firmId + " and branch_id = " + branchId + "  and module_id = " +
                moduleId + "  and key_id = " + keyId + " and product_id=" + productId;
            else
                query = "update key_master set value=value+1 where firm_id = " + firmId + " and branch_id = " + branchId + "  and module_id = " +
               moduleId + "  and key_id = " + keyId + "";
            return query;
        }

    }
}
        
           
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Oversite.Helpers
//{
//    class GeneralManager
//    {
//        DbConnectionHandler objDbConnectionHandler = new DbConnectionHandler();

//        public string GetParameter(int firmId, int moduleId, int parameterId, int productId)
//        {
//            string value = objDbConnectionHandler.GetRecord<string>("select parmtr_value from general_parameter  where firm_id=" + firmId + "and module_id=" +
//                moduleId + "  and parmtr_id=" + parameterId + " and product_id=" + productId + "", DbConnectionHandler.SQLMode.Query, null);
//            return value;
//        }


//        public int GetKeyValue(int firmId, int branchId, int moduleId, int keyId, int productId)
//        {

//            int value = 0;
//            if (productId != 0)
//                value = objDbConnectionHandler.GetRecord<int>("select value from key_master  where firm_id=" + firmId + " and branch_id=" + branchId +
//                "  and module_id=" + moduleId + "  and key_id=" + keyId + " and product_id=" + productId, DbConnectionHandler.SQLMode.Query, null);
//            else
//                value = objDbConnectionHandler.GetRecord<int>("select value from key_master  where firm_id=" + firmId + " and branch_id=" + branchId +
//               "  and module_id=" + moduleId + "  and key_id=" + keyId + "", DbConnectionHandler.SQLMode.Query, null);
//            return value;
//        }
//        public int GetandUpdateKeyValue(int firmId, int branchId, int moduleId, int keyId, int productId)
//        {

//            int value = 0;
//            string query = string.Empty;


//            int check = objDbConnectionHandler.GetRecord<int>("select t.parmtr_value from GENERAL_PARAMETER t where t.parmtr_id=601", DbConnectionHandler.SQLMode.Query, null);
//            #region "Queryy old"
//            if (check == 0)
//            {

//                if (productId != 0)
//                {
//                    if (keyId != 1002)
//                    {
//                        value = dbConnectionHandler.GetRecord<int>("select value from key_master  where firm_id=" + firmId + " and branch_id=" + branchId +
//                                    "  and module_id=" + moduleId + "  and key_id=" + keyId + " and product_id=" + productId, DbConnectionHandler.SQLMode.Query, null);
//                        value = objDbConnectionHandler.GetRecord<int>("select value from key_master  where firm_id=" + firmId + " and branch_id=" + branchId +
//                                    "  and module_id=" + moduleId + "  and key_id=" + keyId + " and product_id=" + productId + " for update of value", DbConnectionHandler.SQLMode.Query, null);
//                        query = "update key_master set value=value+1 where firm_id = " + firmId + " and branch_id = " + branchId + "  and module_id = " +
//                        moduleId + "  and key_id = " + keyId + " and product_id=" + productId;

//                        objDbConnectionHandler.ExecuteNonQuery(query, DbConnectionHandler.SQLMode.Query, null);
//                    }
//                    else
//                    {
//                        value = dbConnectionHandler.GetRecord<int>("select value from key_master  where firm_id=" + firmId + " and branch_id=" + branchId +
//                                    "  and module_id=" + moduleId + "  and key_id=" + keyId + " and product_id=" + productId, DbConnectionHandler.SQLMode.Query, null);
//                        value = objDbConnectionHandler.GetRecord<int>("select value from key_master  where firm_id=" + firmId + " and branch_id=" + branchId +
//                                    "  and module_id=" + moduleId + "  and key_id=" + keyId + " and product_id=" + productId + " for update of value", DbConnectionHandler.SQLMode.Query, null);
//                        query = "update key_master set value=value+1 where firm_id = " + firmId + " and branch_id = " + branchId + "  and module_id = " +
//                        moduleId + "  and key_id = " + keyId + " and product_id=" + productId;

//                        objDbConnectionHandler.ExecuteNonQuery(query, DbConnectionHandler.SQLMode.Query, null);
//                    }



//                }
//                else
//                {
//                    value = objDbConnectionHandler.GetRecord<int>("select value from key_master  where firm_id=" + firmId + " and branch_id=" + branchId +
//                   "  and module_id=" + moduleId + "  and key_id=" + keyId + " for update of value", DbConnectionHandler.SQLMode.Query, null);
//                    query = "update key_master set value=value+1 where firm_id = " + firmId + " and branch_id = " + branchId + "  and module_id = " +
//                   moduleId + "  and key_id = " + keyId + "";

//                    objDbConnectionHandler.ExecuteNonQuery(query, DbConnectionHandler.SQLMode.Query, null);
//                }

//            }
//            #endregion
//            #region "function doc id"
//            else
//            {


//                DynamicParameters dynamicParameters = new DynamicParameters();
//                dynamicParameters.Add("firmId", firmId, DbType.Int32, ParameterDirection.Input, 2);
//                dynamicParameters.Add("branchId", branchId, DbType.Int32, ParameterDirection.Input, 4);
//                dynamicParameters.Add("moduleId", moduleId, DbType.Int32, ParameterDirection.Input, 4);
//                dynamicParameters.Add("keyId", keyId, DbType.Int32, ParameterDirection.Input, 4);
//                dynamicParameters.Add("productId", productId, DbType.Int32, ParameterDirection.Input, 4);
//                dynamicParameters.Add("outval", productId, DbType.String, ParameterDirection.ReturnValue, 20);
//                objDbConnectionHandler.ExecuteNonQuery("fn_GET_AND_UPDATE", DbConnectionHandler.SQLMode.StoredProcedure, dynamicParameters);
//                value = Convert.ToInt32(dynamicParameters.Get<string>("outval"));

//            }

//            #endregion
//            return value;

//        }

//        public string UpdateKeyValue(int firmId, int branchId, int moduleId, int keyId, int productId)
//        {
//            string query = string.Empty;
//            if (productId != 0)
//                query = "update key_master set value=value+1 where firm_id = " + firmId + " and branch_id = " + branchId + "  and module_id = " +
//                moduleId + "  and key_id = " + keyId + " and product_id=" + productId;
//            else
//                query = "update key_master set value=value+1 where firm_id = " + firmId + " and branch_id = " + branchId + "  and module_id = " +
//               moduleId + "  and key_id = " + keyId + "";
//            return query;
//        }
//        public int GenerateSMSID(int firmId, int sequence)
//        {
//            int smsId;
//            smsId = Convert.ToInt32(firmId.ToString() + sequence.ToString());
//            return smsId;
//        }

//    }
//}


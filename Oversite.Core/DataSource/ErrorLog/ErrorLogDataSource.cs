using Oracle.ManagedDataAccess.Client;
using Oversite.DTO.ErrorLog.Request;
using Oversite.DTO.ErrorLog.Response;
using Oversite.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Oversite.Core.DataSource.ErrorLog
{
    class ErrorLogDataSource
    {
        public ErrorLogResponse ErrorLog(ErrorLogRequest errorLogRequest)
        {
            ErrorLogResponse errorLogResponse = new ErrorLogResponse();
            try
            {

                OracleParameter[] parm_c = new OracleParameter[6];
                parm_c[0] = new OracleParameter("function_name", OracleDbType.Varchar2, 4000);
                parm_c[0].Value = errorLogRequest.Function_Name;
                parm_c[0].Direction = ParameterDirection.Input;

                parm_c[1] = new OracleParameter("v_exception", OracleDbType.Varchar2, 4000);
                parm_c[1].Value = errorLogRequest.V_Exception;
                parm_c[1].Direction = ParameterDirection.Input;

                parm_c[2] = new OracleParameter("v_data", OracleDbType.Varchar2, 4000);
                parm_c[2].Value = errorLogRequest.V_Data;
                parm_c[2].Direction = ParameterDirection.Input;

                parm_c[3] = new OracleParameter("firmId", OracleDbType.Long);
                parm_c[3].Value = errorLogRequest.Firm_ID;
                parm_c[3].Direction = ParameterDirection.Input;

                parm_c[4] = new OracleParameter("productid", OracleDbType.Long);
                parm_c[4].Value = errorLogRequest.Product_ID;
                parm_c[4].Direction = ParameterDirection.Input;

                parm_c[5] = new OracleParameter("ErrorStatus", OracleDbType.Int16, 9);
                parm_c[5].Direction = ParameterDirection.Output;

                new OracleHelper().ExecuteNonQuery("sp_error_log_create", parm_c);

                errorLogResponse.errorStatus = parm_c[5].Value.ToString();

            }
            catch (Exception ex)
            {
                errorLogResponse.isDataAvailable = false;
                errorLogResponse.message = ex.Message;
            }
            return errorLogResponse;
        }
    }
}

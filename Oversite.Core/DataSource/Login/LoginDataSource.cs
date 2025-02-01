using CVHelpers;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Oracle.ManagedDataAccess.Client;
using Oversite.DTO.Login.Properties;
using Oversite.DTO.Login.Request;
using Oversite.DTO.Login.Response;
using Oversite.DTO.PasswordReset.Properties;
using Oversite.DTO.Response;
using Oversite.Helpers;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Oversite.Core.DataSource.Login
{/// <summary>
/// 
/// </summary>
    public class LoginDataSource
    {
        PassswordUtility objPassword = new PassswordUtility();

        //public LoginResponse userLogin(LoginRequest request, IDistributedCache distributedCache)
        //{
        //    //Response<LoginResponse> response = new Response<LoginResponse>();
        //    LoginResponse response = new LoginResponse();
        //    try
        //    {
        //        RedisManager redisManager = new RedisManager(distributedCache);
        //        RedisManagerDto redisManagerDto = new RedisManagerDto();
        //        JwtTokenManager jwtToken = new JwtTokenManager();
        //        string token = string.Empty;
        //        token = jwtToken.CreateToken(0, request.employeeId, request.siganture);
        //        string password = string.Empty;
        //        if (!String.IsNullOrEmpty(token))
        //        {
        //            try
        //            {
        //                decimal time = new OracleHelper().ExecuteScalar<decimal>("select to_number(PARMTR_VALUE) PARMTR_VALUE from GENERAL_PARAMETER where PARMTR_ID = 617 and MODULE_ID = 2");
        //                DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
        //                {
        //                    AbsoluteExpiration = DateTime.Now.AddMinutes(Convert.ToDouble(time)),
        //                    SlidingExpiration = TimeSpan.FromMinutes(Convert.ToDouble(time)),
        //                };
        //                string passwrd = string.Empty;
        //                string qry12 = "select count(*)  from user_master t  where t.emp_code ='" + request.employeeId + "'";
        //                decimal count1 = new OracleHelper().ExecuteScalar<decimal>(qry12);
        //                if (count1 > 0)
        //                {
        //                    passwrd = objPassword.Encrypt(request.password);
        //                }
        //                else
        //                {
        //                    passwrd = objPassword.Encrypt(request.password);
        //                }

        //                //password expire
        //                string datequery, rolequery;
        //                decimal logindifference;

        //                rolequery = "select t.role_id role from vw_user_role_details_CV t inner join vw_cv_user_master um on t.user_id=um.user_id where um.emp_code='" + request.employeeId + "' and t.role_id in(1,6)";
        //                List<RoleProperties> roleProperties = new OracleHelper().GetRecords<RoleProperties>(rolequery);
        //                decimal expiredays1 = new OracleHelper().ExecuteScalar<decimal>("select to_number(PARMTR_VALUE)PARMTR_VALUE  from lms_tw.GENERAL_PARAMETER where PARMTR_ID=614 and MODULE_ID=3");
        //                decimal expiredays2 = new OracleHelper().ExecuteScalar<decimal>("select to_number(PARMTR_VALUE)PARMTR_VALUE  from lms_tw.GENERAL_PARAMETER where PARMTR_ID=615 and MODULE_ID=3");
        //                //decimal expiredays1 = new OracleHelper().ExecuteScalar<decimal>("select to_number(t.parmtr_value) parmtr_value  from GENERAL_PARAMETER t where PARMTR_ID=1015 and MODULE_ID=3 and product_id=10");
        //                //decimal expiredays2 = new OracleHelper().ExecuteScalar<decimal>("select to_number(t.parmtr_value) parmtr_value  from GENERAL_PARAMETER t where PARMTR_ID=1016 and MODULE_ID=3 and product_id=10");


        //                datequery = "select to_date(sysdate,'dd-mon-yyyy')-to_date((case when t.modified_date is null then t.entered_date else t.modified_date end),'dd-mon-yyyy')from user_master t where emp_code = '" + request.employeeId + "'";
        //                logindifference = new OracleHelper().ExecuteScalar<decimal>(datequery);
        //                foreach (RoleProperties roleProperties1 in roleProperties)
        //                {
        //                    if (roleProperties1.ROLE_ID == 1 || roleProperties1.ROLE_ID == 6)
        //                    {
        //                        if (logindifference > expiredays1)
        //                        {
        //                            response.message = "Your Password Expired...Please Change Your Password and login!!!";
        //                            return response;
        //                        }
        //                    }
        //                }

        //                if (logindifference > expiredays2)
        //                {
        //                    response.message = "Your Password Expired...Please Change Your Password and login!!!";
        //                    return response;
        //                }




        //                //int date_flag = objDbConnectionHandler.GetRecord<Int32>("select count(*) from user_master_authentication u" +
        //                //                " where u.user_id = '" + request.employeeID + "'" +
        //                //                " and (sysdate- u.ph_auth_date)> 45 ", DbConnectionHandler.SQLMode.Query, null);
        //                //decimal user_flag = new OracleHelper().ExecuteScalar<decimal>("select to_number(count(*)) from user_master_authentication u" +
        //                //                    "
        //                //                    where u.user_id in ( '" + request.employeeId + "')");
        //                //    if (date_flag > 0 || user_flag == 0)
        //                //    {
        //                //        response.Is_Pass_Expired = 1;
        //                //        response.IsDataAvailable = false;
        //                //        response.message = "Your Password Expired...Please Change Your Password and login!!!";
        //                //        return response;
        //                //    }
        //                //    else
        //                //        response.Is_Pass_Expired = 0;
        //                //}


        //                string strings;
        //                OracleParameter[] parm_c = new OracleParameter[7];
        //                parm_c[0] = new OracleParameter("var_employeeid", OracleDbType.Varchar2);
        //                parm_c[0].Value = request.employeeId.Trim();
        //                parm_c[0].Direction = ParameterDirection.Input;
        //                parm_c[1] = new OracleParameter("var_passwd", OracleDbType.Varchar2);
        //                parm_c[1].Value = passwrd;
        //                parm_c[1].Direction = ParameterDirection.Input;
        //                parm_c[2] = new OracleParameter("var_branchid", OracleDbType.Decimal);
        //                parm_c[2].Value = 0;
        //                parm_c[2].Direction = ParameterDirection.Input;

        //                parm_c[3] = new OracleParameter("var_token", OracleDbType.Varchar2);
        //                parm_c[3].Value = "CVOS_" + token;
        //                parm_c[3].Direction = ParameterDirection.Input;

        //                parm_c[4] = new OracleParameter("var_token_duration", OracleDbType.Decimal);
        //                parm_c[4].Value = time;
        //                parm_c[4].Direction = ParameterDirection.Input;

        //                parm_c[5] = new OracleParameter("var_signature_data", OracleDbType.Varchar2);
        //                parm_c[5].Value = request.siganture;
        //                parm_c[5].Direction = ParameterDirection.Input;

        //                //parm_c[6] = new OracleParameter("var_productid", OracleDbType.Decimal);
        //                //parm_c[6].Value = 0;
        //                //parm_c[6].Direction = ParameterDirection.Input;

        //                parm_c[6] = new OracleParameter("error_msg", OracleDbType.Varchar2, 4000);

        //                parm_c[6].Direction = ParameterDirection.Output;
        //                new OracleHelper().ExecuteNonQuery("proc_oversiteemployee", parm_c);
        //                strings = parm_c[6].Value.ToString();
        //                string[] data = strings.Split('^');

        //                if (Convert.ToInt64(data[1]) == 1)
        //                {

        //                    response.loginData.loginStatus = LoginStatus.validUser;
        //                    //response.loginData.firmID = Convert.ToInt32(data[2]);
        //                    //response.loginData.firmName = Convert.ToString(data[3]);
        //                    //response.loginData.employeeName = Convert.ToString(data[5]);
        //                    ///response.loginData.branchName = Convert.ToString(data[6]);
        //                    response.loginData.empCode = Convert.ToString(request.employeeId);
        //                    ///response.loginData.productID = Convert.ToInt32(0);
        //                    response.loginData.branchID = 0;
        //                    response.loginData.token = "CVOS_" + token;
        //                    EmpTokenRequest empTokenRequest = new EmpTokenRequest()
        //                    {
        //                        emp_code = response.loginData.empCode,
        //                        token = "CVOS_" + token
        //                    };
        //                    DTO.Login.Response.EmptokenResponse emptokenResponse = EmpTokenDetail(empTokenRequest);
        //                    emptokenResponse.limitstarttime = DateTime.Now;
        //                    emptokenResponse.limit = 0;
        //                    redisManagerDto.options = options;
        //                    redisManagerDto.key = "CVOS_" + token;
        //                    redisManagerDto.value = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(emptokenResponse));
        //                    redisManager.SetCache(redisManagerDto);
        //                    //	response.departId = Convert.ToString(data[7]);
        //                    //decimal checkToken = 0;
        //                    //DataTable dt = new DataTable();
        //                    //string query = "select count(*)  from oversite_login_token_handler t  where t.emp_code =  " + request.employeeId + "  and t.status = 1 and endtime <= sysdate ";
        //                    //checkToken = new OracleHelper().ExecuteScalar<decimal>(query);
        //                    //if (checkToken == 1)
        //                    //{
        //                    //    response.loginData.token = token;
        //                    //}
        //                    //else
        //                    //{
        //                    //    query = "select t.token  from oversite_login_token_handler t  where t.emp_code = " + request.employeeId + "  and t.status = 1 ";
        //                    //    object tokens = new OracleHelper().ExecuteScalar<object>(query);
        //                    //    response.loginData.token = tokens.ToString();
        //                    //}

        //                    string regionqry = "select distinct(b.region_id) from cv_los.user_type_link u inner join cv_los.branch_master b on u.link_value=b.branch_id  where u.user_id='" + request.employeeId + "'";
        //                    response.regionData = new OracleHelper().GetRecords<RegionProperties>(regionqry);
        //                    //response.IsDataAvailable = true;
        //                    //response.message = "Success";

        //                    string qry = "select count(*)  from vw_cv_user_master t  where t.emp_code = '" + request.employeeId + "'";
        //                    decimal cnt = new OracleHelper().ExecuteScalar<decimal>(qry);

        //                    string qry1 = "select count(*)  from user_master t  where t.emp_code = '" + request.employeeId + "'";
        //                    decimal count = new OracleHelper().ExecuteScalar<decimal>(qry1);
        //                    if (cnt >= 1)
        //                    {
        //                        string qry2 = "select t.emp_name employeeName   from vw_cv_user_master t  where t.emp_code = '" + request.employeeId + "'";
        //                        object name = new OracleHelper().ExecuteScalar<object>(qry2);
        //                        response.loginData.employeeName = Convert.ToString(name);
        //                        string qury = "select Distinct u.role_id roleId from vw_cv_user_master t left outer join vw_cv_user_role_details u on t.user_id=u.user_id where t.emp_code = '" + request.employeeId + "'and u.role_id in (48,49,50,52,55,7,19,2,74,73)";
        //                        //response.Data = new OracleHelper().GetRecords<DatasProperties>(qury);
        //                        object role = new OracleHelper().ExecuteScalar<object>(qury);
        //                        //for (int i = 0; i < response.Data.Count(); i++)
        //                        //{
        //                        //string qry3 = "select to_char(t.role_id) from vw_cv_User_Role_Details t where t.user_id =" + response.Data[i].userId + "";
        //                        //rolelist = new OracleHelper().GetRecords<int>(qry3);
        //                        ////object role = new OracleHelper().ExecuteScalar<object>(qry3);
        //                        //for (int k = 0; k < rolelist.Count; k++)
        //                        //{
        //                        string query1 = "select  t.status_id statusId, t.function_id functionId, t.function_name  functionName,t.role_id roleId ,t.ROUTER_LINK routerLink from OVERSITE_ROLE_FUNCTION_DTL t where t.role_id=" + role + "";
        //                        response.RoleList = new OracleHelper().GetRecords<DocumentsDataProperties>(query1);
        //                        //}
        //                        //}

        //                    }
        //                    else if (count >= 1)
        //                    {
        //                        string qry2 = "select t.emp_name employeeName   from user_master t  where t.emp_code = '" + request.employeeId + "'";
        //                        object name = new OracleHelper().ExecuteScalar<object>(qry2);
        //                        response.loginData.employeeName = Convert.ToString(name);

        //                        ///string qury = "select to_char(t.user_id) userId from User_master t  where t.emp_code ='" + request.employeeId +"'";

        //                        string qury = "select Distinct u.role_id roleId from User_master t left outer join User_Role_Details u on t.user_id=u.user_id where t.emp_code = '" + request.employeeId + "'and u.role_id in (48,49,50,52,55,7,19,2,74,73)";
        //                        //rolelist = new OracleHelper().GetRecords<int>(qury);
        //                        object role = new OracleHelper().ExecuteScalar<object>(qury);
        //                        //for (int i = 0; i < rolelist.Count(); i++)
        //                        ////foreach (var item in rolelist)
        //                        //{
        //                        //string qry3 = "select to_char(t.role_id) from User_Role_Details t where t.user_id =" + response.Data[i].userId + "";
        //                        //rolelist = new OracleHelper().GetRecords<int>(qry3);
        //                        ////object role = new OracleHelper().ExecuteScalar<object>(qry3);
        //                        //for (int k = 0; k < rolelist.Count; k++)
        //                        //{

        //                        string query1 = "select  t.status_id statusId, t.function_id functionId, t.function_name  functionName,t.role_id roleId from OVERSITE_ROLE_FUNCTION_DTL t where t.role_id=" + role + "";
        //                        response.RoleList = new OracleHelper().GetRecords<DocumentsDataProperties>(query1);
        //                        //}
        //                        //}

        //                    }
        //                    if (response.RoleList.Count > 0)
        //                    {
        //                        response.IsDataAvailable = true;
        //                        response.message = ResponseTypeContants.SUCCESS;
        //                    }
        //                    else
        //                    {
        //                        response.IsDataAvailable = false;
        //                        response.message = "No Data Found";
        //                    }

        //                }
        //                else if (Convert.ToInt64(data[1]) == 0)
        //                {

        //                    response.message = Convert.ToString(data[0]);
        //                    return response;
        //                }

        //            }

        //            catch (Exception ex)
        //            {
        //                response.message = "Exception :- " + ex.Message;
        //            }
        //        }
        //        else
        //        {
        //            response.message = "Failed To Generate Token";
        //        }
        //    }
        //    catch (Exception e)
        //    {

        //    }
        //    return response;
        //}

        public LoginResponse userLogin(LoginRequest request, IDistributedCache distributedCache)
        {
            //Response<LoginResponse> response = new Response<LoginResponse>();
            List<DatasProperties> DatasProperties = new List<DatasProperties>();//100823
            LoginResponse response = new LoginResponse();
            decimal roleid = 0;


            try
            {
                RedisManager redisManager = new RedisManager(distributedCache);
                RedisManagerDto redisManagerDto = new RedisManagerDto();
                JwtTokenManager jwtToken = new JwtTokenManager();
                string token = string.Empty;
                token = jwtToken.CreateToken(0, request.employeeId, request.siganture);
                string password = string.Empty;
                if (!String.IsNullOrEmpty(token))
                {
                    try
                    {
                        decimal time = new OracleHelper().ExecuteScalar<decimal>("select to_number(PARMTR_VALUE) PARMTR_VALUE from GENERAL_PARAMETER where PARMTR_ID = 617 and MODULE_ID = 2");
                        DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
                        {
                            AbsoluteExpiration = DateTime.Now.AddMinutes(Convert.ToDouble(time)),
                            SlidingExpiration = TimeSpan.FromMinutes(Convert.ToDouble(time)),
                        };
                        string passwrd = string.Empty;
                        string qry12 = "select count(*)  from user_master t  where t.emp_code ='" + request.employeeId + "'";
                        decimal count1 = new OracleHelper().ExecuteScalar<decimal>(qry12);
                        if (count1 > 0)
                        {
                            passwrd = objPassword.Encrypt(request.password);
                        }
                        else
                        {
                            passwrd = objPassword.Encrypt(request.password);
                        }

                        //password expire
                        string datequery, rolequery;
                        decimal logindifference;

                        rolequery = "select t.role_id role from user_role_details t inner join user_master um on t.user_id=um.user_id where um.emp_code='" + request.employeeId + "' and t.role_id in(1,6)";
                        List<RoleProperties> roleProperties = new OracleHelper().GetRecords<RoleProperties>(rolequery);
                        decimal expiredays1 = new OracleHelper().ExecuteScalar<decimal>("select to_number(PARMTR_VALUE)PARMTR_VALUE  from lms_tw.GENERAL_PARAMETER where PARMTR_ID=614 and MODULE_ID=3");
                        decimal expiredays2 = new OracleHelper().ExecuteScalar<decimal>("select to_number(PARMTR_VALUE)PARMTR_VALUE  from lms_tw.GENERAL_PARAMETER where PARMTR_ID=615 and MODULE_ID=3");
                        //decimal expiredays1 = new OracleHelper().ExecuteScalar<decimal>("select to_number(t.parmtr_value) parmtr_value  from GENERAL_PARAMETER t where PARMTR_ID=1015 and MODULE_ID=3 and product_id=10");
                        //decimal expiredays2 = new OracleHelper().ExecuteScalar<decimal>("select to_number(t.parmtr_value) parmtr_value  from GENERAL_PARAMETER t where PARMTR_ID=1016 and MODULE_ID=3 and product_id=10");

                        datequery = "select to_date(sysdate,'dd-mon-yyyy')-to_date((case when t.modified_date is null then t.entered_date else t.modified_date end),'dd-mon-yyyy')from user_master t where emp_code = '" + request.employeeId + "'";
                        logindifference = new OracleHelper().ExecuteScalar<decimal>(datequery);
                        foreach (RoleProperties roleProperties1 in roleProperties)
                        {
                            if (roleProperties1.ROLE_ID == 1 || roleProperties1.ROLE_ID == 6)
                            {
                                if (logindifference > expiredays1)
                                {
                                    response.message = "Your Password Expired...Please Change Your Password and login!!!";
                                    return response;
                                }
                            }
                        }

                        if (logindifference > expiredays2)
                        {
                            response.message = "Your Password Expired...Please Change Your Password and login!!!";
                            return response;
                        }

                        string strings;
                        OracleParameter[] parm_c = new OracleParameter[7];
                        parm_c[0] = new OracleParameter("var_employeeid", OracleDbType.Varchar2);
                        parm_c[0].Value = request.employeeId.Trim();
                        parm_c[0].Direction = ParameterDirection.Input;
                        parm_c[1] = new OracleParameter("var_passwd", OracleDbType.Varchar2);
                        parm_c[1].Value = passwrd;
                        parm_c[1].Direction = ParameterDirection.Input;
                        parm_c[2] = new OracleParameter("var_branchid", OracleDbType.Decimal);
                        parm_c[2].Value = 0;
                        parm_c[2].Direction = ParameterDirection.Input;

                        parm_c[3] = new OracleParameter("var_token", OracleDbType.Varchar2);
                        parm_c[3].Value = "CVOS_" + token;
                        parm_c[3].Direction = ParameterDirection.Input;

                        parm_c[4] = new OracleParameter("var_token_duration", OracleDbType.Decimal);
                        parm_c[4].Value = time;
                        parm_c[4].Direction = ParameterDirection.Input;

                        parm_c[5] = new OracleParameter("var_signature_data", OracleDbType.Varchar2);
                        parm_c[5].Value = request.siganture;
                        parm_c[5].Direction = ParameterDirection.Input;

                        //parm_c[6] = new OracleParameter("var_productid", OracleDbType.Decimal);
                        //parm_c[6].Value = 0;
                        //parm_c[6].Direction = ParameterDirection.Input;

                        parm_c[6] = new OracleParameter("error_msg", OracleDbType.Varchar2, 4000);

                        parm_c[6].Direction = ParameterDirection.Output;
                        new OracleHelper().ExecuteNonQuery("proc_oversiteemployee", parm_c);
                        strings = parm_c[6].Value.ToString();
                        string[] data = strings.Split('^');

                        if (Convert.ToInt64(data[1]) == 1)
                        {

                            response.loginData.loginStatus = LoginStatus.validUser;
                            response.loginData.empCode = Convert.ToString(request.employeeId);
                            response.loginData.branchID = 0;
                            response.loginData.token = "CVOS_" + token;
                            EmpTokenRequest empTokenRequest = new EmpTokenRequest()
                            {
                                emp_code = response.loginData.empCode,
                                token = "CVOS_" + token
                            };
                            DTO.Login.Response.EmptokenResponse emptokenResponse = EmpTokenDetail(empTokenRequest);
                            emptokenResponse.limitstarttime = DateTime.Now;
                            emptokenResponse.limit = 0;
                            redisManagerDto.options = options;
                            redisManagerDto.key = "CVOS_" + token;
                            redisManagerDto.value = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(emptokenResponse));
                            redisManager.SetCache(redisManagerDto);
                            //	response.departId = Convert.ToString(data[7]);
                            //decimal checkToken = 0;
                            //DataTable dt = new DataTable();
                            //string query = "select count(*)  from oversite_login_token_handler t  where t.emp_code =  " + request.employeeId + "  and t.status = 1 and endtime <= sysdate ";
                            //checkToken = new OracleHelper().ExecuteScalar<decimal>(query);
                            //if (checkToken == 1)
                            //{
                            //    response.loginData.token = token;
                            //}
                            //else
                            //{
                            //    query = "select t.token  from oversite_login_token_handler t  where t.emp_code = " + request.employeeId + "  and t.status = 1 ";
                            //    object tokens = new OracleHelper().ExecuteScalar<object>(query);
                            //    response.loginData.token = tokens.ToString();
                            //}
                            string regionqry = "select distinct(b.region_id) from user_type_link u inner join branch_master b on u.link_value=b.branch_id  where u.user_id='" + request.employeeId + "'";
                            response.regionData = new OracleHelper().GetRecords<RegionProperties>(regionqry);

                            //response.IsDataAvailable = true;
                            //response.message = "Success";

                            string qry = "select count(*)  from vw_cv_user_master t  where t.emp_code = '" + request.employeeId + "'";
                            decimal cnt = new OracleHelper().ExecuteScalar<decimal>(qry);

                            string qry1 = "select count(*)  from user_master t  where t.emp_code = '" + request.employeeId + "' and t.product_id=10";
                            decimal count = new OracleHelper().ExecuteScalar<decimal>(qry1);
                            //100823
                            if (cnt >= 1)
                            {
                                string qry2 = "select t.emp_name employeeName   from vw_cv_user_master t  where t.emp_code = '" + request.employeeId + "'";
                                object name = new OracleHelper().ExecuteScalar<object>(qry2);
                                response.loginData.employeeName = Convert.ToString(name);
                                string qury = "select  distinct(to_char(u.role_id)) roieId from vw_cv_user_master t left outer join vw_cv_user_role_details u on t.user_id=u.user_id INNER JOIN cv_los.oversite_role_master r on u.role_id=r.role_id where t.emp_code = '" + request.employeeId + "'";
                                DatasProperties = new OracleHelper().GetRecords<DatasProperties>(qury);
                                response.Data = DatasProperties;
                                string roles = "";

                                for (int i = 0; i < DatasProperties.Count(); i++)
                                {
                                    if (roles == "")
                                    {
                                        roles = DatasProperties[i].roieId;

                                    }
                                    else
                                    {
                                        roles += "," + DatasProperties[i].roieId;
                                    }
                                }
                                string query1 = "select  t.status_id statusId, t.function_id functionId, t.function_name  functionName,t.role_id roleId,t.ROUTER_LINK routerLink from OVERSITE_ROLE_FUNCTION_DTL t where t.role_id in(" + roles + ")";
                                response.RoleList = new OracleHelper().GetRecords<DocumentsDataProperties>(query1);


                            }
                            else
                            if (count >= 1)
                            {
                                string qry2 = "select t.emp_name employeeName   from user_master t  where t.emp_code = '" + request.employeeId + "'";
                                object name = new OracleHelper().ExecuteScalar<object>(qry2);
                                response.loginData.employeeName = Convert.ToString(name);

                                ///string qury = "select to_char(t.user_id) userId from User_master t  where t.emp_code ='" + request.employeeId +"'";

                                string qury = "select Distinct u.role_id roleId from User_master t left outer join User_Role_Details u on t.user_id=u.user_id where t.emp_code = '" + request.employeeId + "'and u.role_id in (48,49,50,52,55,16,69)";
                                //rolelist = new OracleHelper().GetRecords<int>(qury);
                                object role = new OracleHelper().ExecuteScalar<object>(qury);
                                //for (int i = 0; i < rolelist.Count(); i++)
                                ////foreach (var item in rolelist)
                                //{
                                //string qry3 = "select to_char(t.role_id) from User_Role_Details t where t.user_id =" + response.Data[i].userId + "";
                                //rolelist = new OracleHelper().GetRecords<int>(qry3);
                                ////object role = new OracleHelper().ExecuteScalar<object>(qry3);
                                //for (int k = 0; k < rolelist.Count; k++)
                                //{

                                string query1 = "select  t.status_id statusId, t.function_id functionId, t.function_name  functionName,t.role_id roleId,t.ROUTER_LINK routerLink from OVERSITE_ROLE_FUNCTION_DTL t where t.role_id=" + role + "";
                                response.RoleList = new OracleHelper().GetRecords<DocumentsDataProperties>(query1);
                                //}
                                //}

                            }
                            if (response.RoleList.Count > 0)
                            {
                                response.IsDataAvailable = true;
                                response.message = ResponseTypeContants.SUCCESS;
                            }
                            else
                            {
                                response.IsDataAvailable = false;
                                response.message = "No Data Found";
                            }

                        }
                        else if (Convert.ToInt64(data[1]) == 0)
                        {

                            response.message = Convert.ToString(data[0]);
                            return response;
                        }

                    }

                    catch (Exception ex)
                    {
                        response.message = "Exception :- " + ex.Message;
                    }
                }
                else
                {
                    response.message = "Failed To Generate Token";
                }
            }
            catch (Exception e)
            {

            }
            return response;
        }


        public DTO.Login.Response.EmptokenResponse EmpTokenDetail(EmpTokenRequest request)
        {
            DTO.Login.Response.EmptokenResponse response = new DTO.Login.Response.EmptokenResponse();
            try
            {
                string query = "select to_char(t.login_branch_id) login_branch_id,t.token,t.emp_code,to_char(t.duration) duration,to_char(t.status) status,to_char(t.ipaddress) ipaddress,t.starttime,t.endtime,to_char(t.login_key) login_key,t.tokenupdatetime,to_char(t.product_id) product_id from oversite_login_token_handler t where t.emp_code='" + request.emp_code + "'and t.token='" + request.token + "'";
                response = new OracleHelper().GetRecord<DTO.Login.Response.EmptokenResponse>(query);

            }
            catch (Exception ex)
            {

            }
            return response;
        }
    }
}

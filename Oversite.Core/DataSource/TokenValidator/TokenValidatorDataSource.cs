using CVHelpers;
using Maibro.Helper;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Oversite.DTO.ErrorLog.Request;
using Oversite.DTO.Response;
using Oversite.DTO.TokenValidator.Request;
using Oversite.DTO.TokenValidator.Response;
using Oversite.Helpers;
using RSA_Angular_.NET_CORE.RSA;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;

namespace Oversite.Core.DataSource.TokenValidator
{
    public class TokenValidatorDataSource
    {

        AppConfigManager appConfigManager = new AppConfigManager();
        public ValidTokenResponse ValidToken(ValidTokenRequest Request)
        {
            bool isValidToken = false;
            ValidTokenResponse validTokenResponse = new ValidTokenResponse();
            try
            {
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

                var SecurityToken = handler.ReadToken(Request.userContextObj) as JwtSecurityToken;

                var jti = SecurityToken.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;
                string[] value = new RsaHelper().Decrypt(jti).Split(',');

                TokenRequest request = new TokenRequest();
                request.branchID = value[1];
                request.employeeId = value[2];
                request.token = Request.userContextObj;
                TokenResponse tokenResponse = ValidateToken(request);
                if (tokenResponse.isValidToken)
                {
                    validTokenResponse.isDataAvailable = true;
                    isValidToken = true;
                    validTokenResponse.flag = 1;
                }
                else
                {
                    validTokenResponse.flag = 0;
                }
            }
            catch (Exception ex)
            {
                isValidToken = false;
            }
            return validTokenResponse;
        }
        public TokenResponse ValidateToken(TokenRequest request)
        {
            TokenResponse response = new TokenResponse();
            int _CheckTokenCount = 0;
            int _okenCountCheck = 0;
            while (_CheckTokenCount <= 5)
            {
                string _tokenCount = "select count(*)  from oversite_login_token_handler t where t.token = '" + request.token + "' and t.login_branch_id = '" + request.branchID + "' and t.emp_code = '" + request.employeeId + "' and   t.status = 1";
                decimal _varTokenCount = new OracleHelper().ExecuteScalar<decimal>(_tokenCount);

                if (_varTokenCount > 0)
                {
                    _okenCountCheck = 1;
                    _CheckTokenCount = 6;

                }
                else
                {
                    _CheckTokenCount++;
                    _okenCountCheck = 0;
                }
            }


            if (_okenCountCheck == 1)
            {
                string tokenDataQuery = "select endtime ENDTIME,TOKENUPDATETIME TOKENUPDATETIME from oversite_login_token_handler t where  t.token = '" + request.token + "' and t.login_branch_id = '" + request.branchID + "' and t.emp_code = '" + request.employeeId + "' and  t.status = 1";

                TokenData tokenData = new OracleHelper().GetRecord<TokenData>(tokenDataQuery);
                if (tokenData.ENDTIME <= DateTime.Now)
                {
                    response.isValidToken = false;
                }
                else
                {
                    if (DateTime.Now <= tokenData.TOKENUPDATETIME)
                    {
                        string updateToken = "update oversite_login_token_handler t set t.endtime=sysdate + (.000694 * 30),t.TOKENUPDATETIME=sysdate + (.000694 * (30-2)) where  token = '" + request.token + "' and login_branch_id = '" + request.branchID + "' and emp_code = '" + request.employeeId + "' and  status = 1";


                        new OracleHelper().ExecuteNonQuery(updateToken);
                        response.isValidToken = true;
                    }
                    else
                    {
                        response.isValidToken = false;
                    }
                }
            }
            else
            {
                response.isValidToken = false;
            }

            return response;
        }
        public ValidTokenResponse ValidateTokenWithEmpCode(ValidateTokenWithEmpcodeRequest request)
        {
            ValidTokenResponse response = new ValidTokenResponse();

            try
            {
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                var SecurityToken = handler.ReadToken(request.userContextObj.Replace("CVOS_", "")) as JwtSecurityToken;
                var jti = SecurityToken.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;
                string[] value = new RsaHelper().Decrypt(jti).Split(',');
                string branchID = value[1];
                string employeeId = value[2];
                if (employeeId == request.empCode)
                {
                    string _tokenCount = "select count(*)  from oversite_login_token_handler t where t.token = '" + request.userContextObj + "'  and t.emp_code = '" + employeeId + "'  and   t.status = 1";
                    decimal _varTokenCount = new OracleHelper().ExecuteScalar<decimal>(_tokenCount);
                    if (_varTokenCount > 0)
                    {
                        //TokenRequest request1 = new TokenRequest();
                        //request1.branchID = value[0];
                        //request1.employeeId = value[1];
                        //request1.token = request.userContextObj;
                        //TokenResponse tokenResponse = ValidateToken(request1);
                        //if (tokenResponse.isValidToken)
                        //{
                        response.isDataAvailable = true;
                        response.flag = 1;
                    }
                    else
                    {
                        response.isDataAvailable = false;
                        response.flag = 0;
                    }


                }
                else
                {
                    response.isDataAvailable = false;
                    response.flag = 0;
                }


            }
            catch (Exception ex)
            {

            }

            return response;
        }
        public updateTokenWhenLogoutResponse updateTokenWhenLogout(ValidateTokenWithEmpcodeRequest request, IDistributedCache distributedCache)
        {
            updateTokenWhenLogoutResponse response = new updateTokenWhenLogoutResponse();
            string output = JsonConvert.SerializeObject(request);

            try
            {
                string _tokenCount = "update oversite_login_token_handler t set t.status=0 where  t.emp_code = '" + request.empCode + "' and t.token='"+request.userContextObj+"'";
                new OracleHelper().ExecuteNonQuery(_tokenCount);
                DeleteLogOutToken(request.empCode,request.userContextObj, distributedCache);

                response.isDataAvailable = true;
                response.flag = 1;

            }
            catch (Exception ex)
            {
                ErrorLogRequest errorLogRequest = new Oversite.DTO.ErrorLog.Request.ErrorLogRequest();
                errorLogRequest.Function_Name = "updateTokenWhenLogout";
                errorLogRequest.V_Data = output;
                errorLogRequest.V_Exception = ex.Message;
                new ErrorLog.ErrorLogDataSource().ErrorLog(errorLogRequest);
                response.message = ex.Message;

            }

            return response;
        }

        public EmployeResponse GetotpemployeeData(EmployeRequest request, IDistributedCache distributedCache)
        {
            EmployeResponse response = new EmployeResponse();
            EmployeeData EmployeeData = new EmployeeData();
            if (request.type == "1")
            {
                string qq = "update oversite_login_token_handler t set t.status=0 where t.emp_code='" + request.employeeId + "' and t.status=1 ";
                new OracleHelper().ExecuteNonQuery(qq);
                DeleteToken(request.employeeId, distributedCache);
                response.IsDataAvailable = true;

            }
            else
            {

                string q = "select count(*) from oversite_login_token_handler t where t.emp_code='" + request.employeeId + "' and t.status=1";
                decimal count = new OracleHelper().ExecuteScalar<decimal>(q);
                if (count > 0)
                {
                    response.concurrentCheck = 1;
                    response.concurrentCheckmsg = "User is active in another session. Do you want to continue?";
                    //response.IsDataAvailable = false;
                    response.IsDataAvailable = true;

                }
                else
                {
                    response.IsDataAvailable = false;
                    response.concurrentCheckmsg = "Please Login";
                    return response;

                }

            }

            // response.IsDataAvailable = true;
            return response;

        }
        public void DeleteToken(string employeeId, IDistributedCache distributedCache)
        {
            string query = "select TOKEN token from  oversite_login_token_handler t where t.EMP_CODE= " + employeeId + "";
            List<EmptokenResponse> response = new OracleHelper().GetRecords<EmptokenResponse>(query);
            if (response.Any())
            {
                foreach (var item in response)
                {
                    if (new RedisManager(distributedCache).CheckCache(item.token))
                    {
                        new RedisManager(distributedCache).Remove(item.token);
                    }
                }
            }
        }
        public void DeleteLogOutToken(string employeeId,string token, IDistributedCache distributedCache)
        {
            string query = "select TOKEN token from  oversite_login_token_handler t where t.EMP_CODE= '" + employeeId + "' and t.token='"+token+"'";
            List<EmptokenResponse> response = new OracleHelper().GetRecords<EmptokenResponse>(query);
            if (response.Any())
            {
                foreach (var item in response)
                {
                    if (new RedisManager(distributedCache).CheckCache(item.token))
                    {
                        new RedisManager(distributedCache).Remove(item.token);
                    }
                }
            }
        }
        public BranchMasterResponse Get_Branch(string empId)
        {
            BranchMasterResponse response = new BranchMasterResponse();
            List<BranchMasterData> branchMasterProperties = new List<BranchMasterData>();
            //string query = "select t.branch_Id BranchId, t.branch_name Branch from home_user_branches ub, BRANCH_MASTER t where ub.branch_id = t.branch_id and t.status_id = 1 and ub.user_id = " + empId + "";
            string query = "select t.branch_id BranchId, t.branch_name Branch from user_master ub, BRANCH_MASTER t where ub.branch_id = t.branch_id and t.status_id = 1 and ub.emp_code = '" + empId + "'";

            branchMasterProperties = new OracleHelper().GetRecords<BranchMasterData>(query);

            if (branchMasterProperties.Count > 0)
            {
                response.isDataAvailable = true;
                response.branchMasterProperties = branchMasterProperties;
            }
            return response;

        }
        public PreauthResponse Preauth(PreauthRequest request, IDistributedCache distributedCache)
        {
            PreauthResponse response = new PreauthResponse();
            List<Getpreauth> getpreauths = new List<Getpreauth>();
            try
            {
                RedisManagerDto redisManagerDto = new RedisManagerDto();
                RedisManager redisManager = new RedisManager(distributedCache);
                JwtTokenManager jwtToken = new JwtTokenManager();
                string token = string.Empty;
                //string time = "3";
                decimal time = new OracleHelper().ExecuteScalar<decimal>("select to_number(PARMTR_VALUE) PARMTR_VALUE from GENERAL_PARAMETER where PARMTR_ID = 618 and MODULE_ID = 2");
                DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(Convert.ToDouble(time)),
                    SlidingExpiration = TimeSpan.FromMinutes(Convert.ToDouble(time))
                };
                string que = "select * from PRE_AUTH_LOG t where t.domain='" + request.domain + "' and t.module='" + request.module + "' and t.environment='" + request.environment + "'and t.status=1";
                getpreauths = new OracleHelper().GetRecords<Getpreauth>(que);
                if (getpreauths.Count > 0)
                {
                    token = jwtToken.CreatePreToken(request.domain, request.environment, request.module, getpreauths[0].hashkey);
                    redisManagerDto.options = options;
                    redisManagerDto.key = "CVOSpre_" + token;
                    redisManagerDto.value = Encoding.ASCII.GetBytes("CVOSpre_" + token);
                    redisManager.SetCache(redisManagerDto);
                    response.token = "CVOSpre_" + token;
                    //response.getpreauths = getpreauths;
                }
                else
                {
                    response.message = "Failed";
                }

            }
            catch (Exception ex)
            {

            }
            return response;
        }
        public AddpreauthResponse Add(AddpreauthRequest request)
        {
            AddpreauthResponse response = new AddpreauthResponse();
            try
            {
                string qry = "insert into PRE_AUTH_LOG(Domain,Module,ENVIRONMENT,Status,HASHKEY)values('" + request.domain + "','" + request.module + "','" + request.environment + "',1,'" + (new EncryptDecryptUtil().ToMD5(request.domain + "" + request.module + "" + request.environment)) + "')";
                new OracleHelper().ExecuteNonQuery(qry);
                response.isDataAvailable = true;
                response.responseMsg = "SUCCESS";
            }
            catch (Exception ex)
            {

            }
            return response;
        }


    }
}

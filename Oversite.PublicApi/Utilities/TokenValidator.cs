using CVHelpers;
using Maibro.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Oversite.DTO.Login.Properties;
using Oversite.DTO.Login.Response;
using Oversite.DTO.Request;
using Oversite.Helpers;
using RSA_Angular_.NET_CORE.RSA;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace Oversite.PublicApi.Utilities
{
    public class PreTokenValidator : ActionFilterAttribute
    {
        ApiManager generalUtility = new ApiManager();
        AppConfigManager appConfiguration = new AppConfigManager();
        IConfiguration configuration;
        RedisManager redisManager;
        private readonly IDistributedCache _distributedCache;

        //public TokenValidator(IDistributedCache distributedCache)
        //{

        //    _distributedCache = distributedCache;

        //}
        public PreTokenValidator(IDistributedCache distributedCache)
        {

            _distributedCache = distributedCache;
            redisManager = new RedisManager(_distributedCache);
        }

        private bool tokenValidationRequired = true;
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            bool validUser = false;

            RedisManagerDto redisManagerDto = new RedisManagerDto();
            if (isServiceAvailable())   /////////// return true if the api service is ready to do
            {
                if (isDBServerAvailable())   /////////// return true if the DB server is available
                {

                    StringValues authorizationToken;
                    StringValues PreAuthorization;
                    StringValues preauthrequest;
                    StringValues FunctionId;
                    StringValues ProductId;
                    filterContext.HttpContext.Request.Headers.TryGetValue("Authorization", out authorizationToken);
                    filterContext.HttpContext.Request.Headers.TryGetValue("preauthorization", out PreAuthorization);
                    filterContext.HttpContext.Request.Headers.TryGetValue("preauthrequest", out preauthrequest);
                    filterContext.HttpContext.Request.Headers.TryGetValue("functionid", out FunctionId);
                    filterContext.HttpContext.Request.Headers.TryGetValue("productid", out ProductId);
                    string controllerName = filterContext.RouteData.Values["controller"].ToString();
                    string actionName = filterContext.RouteData.Values["action"].ToString();
                    var Method = filterContext.HttpContext.Request.Method.ToString();

                    if (Method == "POST")
                    {
                        if (filterContext.HttpContext.Request.QueryString.HasValue)
                        {
                            filterContext.Result = new StatusCodeResult(400);
                            return;
                        }
                    }


                    string[] cname = { "employee", "login" };

                    if (isTokenAvailable(authorizationToken))   /////////// return true if the token is available in the token
                    {
                        if (isTokenAvailable(PreAuthorization))   /////////// return true if the token is available in the token
                        {
                            if (isTokenAvailable(preauthrequest))   /////////// return true if the token is available in the token
                            {
                                if (isPreAutherized(authorizationToken))   /////////// return true if the token is valid 
                                {
                                    if (PreCheckRateLimit(authorizationToken))   /////////// return true if the token is valid
                                    {

                                        validUser = true;
                                    }
                                    else
                                    {
                                        filterContext.Result = new StatusCodeResult(429);
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            //validUser = true;
            if (!validUser)
            {
                filterContext.Result = new StatusCodeResult(403);
                return;
            }
        }

        private bool isTokenAvailable(string userContextObj)
        {
            bool checkToken = false;
            if (userContextObj != null)
            {
                checkToken = true;
            }
            return checkToken;
        }

        private bool isAutherized(string userContextObj)
        {
            return true;
        }

        private bool isPreAutherized(string userContextObj)//---tocken expire
        {
            bool isValidToken = false;
            if (redisManager.CheckCache(userContextObj))
            {
                isValidToken = true;
            }
            return isValidToken;
        }
        private bool isValidToken(EmptokenResponse tokenData)
        {
            bool isValidToken = true;

            try
            {

                if (tokenData.endtime <= DateTime.Now)
                {
                    isValidToken = false;
                }
                else
                {
                    if (DateTime.Now <= tokenData.tokenupdatetime)
                    {
                        RedisManagerDto redisManagerDto = new RedisManagerDto();
                        DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
                        {
                            AbsoluteExpiration = DateTime.Now.AddMinutes(Convert.ToDouble(tokenData.duration)),
                            SlidingExpiration = TimeSpan.FromMinutes(Convert.ToDouble(tokenData.duration)),
                        };
                        redisManager.Remove(tokenData.token);
                        tokenData.endtime = DateTime.Now.AddMinutes(Convert.ToDouble(tokenData.duration));
                        tokenData.tokenupdatetime = DateTime.Now.AddMinutes(Convert.ToDouble(tokenData.duration));
                        redisManagerDto.options = options;
                        redisManagerDto.key = tokenData.token;
                        redisManagerDto.value = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(tokenData));
                        redisManager.SetCache(redisManagerDto);
                        isValidToken = true;
                    }
                    else
                    {
                        isValidToken = false;
                    }
                }

            }
            catch (Exception ex)
            {
                isValidToken = false;
            }
            return isValidToken;
        }
        private bool PreCheckRateLimit(string tokenData)
        {
            bool isValidToken = true;
            if (tokenData != null)
            {
                RateLimit rateLimit = new RateLimit() { limitstarttime = DateTime.Now.AddSeconds(10) };

                RedisManagerDto redisManagerDto = new RedisManagerDto();
                DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(10),
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                    SlidingExpiration = TimeSpan.FromMinutes(10),
                };
                List<RateLimit> lstRate = redisManager.GetCache<List<RateLimit>>("PreRateLimit_" + tokenData);
                if (lstRate != null)
                {
                    lstRate.Add(rateLimit);
                }
                else
                {
                    lstRate = new List<RateLimit>();
                    lstRate.Add(rateLimit);
                }
                redisManager.Remove("PreRateLimit_" + tokenData);
                redisManagerDto.options = options;
                redisManagerDto.key = "PreRateLimit_" + tokenData;
                redisManagerDto.value = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(lstRate));
                redisManager.SetCache(redisManagerDto);

                var count = lstRate.Where(a => a.limitstarttime > DateTime.Now).ToList();

                if (count.Count() > 10)
                {
                    isValidToken = false;
                }
            }

            return isValidToken;
        }
        private bool isValidFunctionId(GetLoginRequests requests)
        {
            bool isValidToken = true;
            try
            {
                List<RoleFunctionProperties> rolefunctionList = redisManager.GetCache<List<RoleFunctionProperties>>("RolefunctionList_" + requests.token);

                var _role = rolefunctionList.Where(a => a.FUNCTION_ID == requests.function_id && a.product_id == requests.product_id && a.emp_code == requests.Employee_code).ToList();

                if (_role != null && _role.Count() > 0)
                {
                    isValidToken = true;
                }
                //string baseUrl = appConfiguration.GetBaseUrl;
                //CVDTO.Response.Response<GetLoginResponses> response = new CVHelpers.ApiManager().InvokePostHttpClientt<Response<GetLoginResponses>, GetLoginRequests>(requests, baseUrl + "/api/Login/UserFunctionCheck").Item1;                //ValidTokenResponse validTokenResponse = ValidToken(validTokenRequest);
                //isValidToken = response.isDataAvailable;


            }
            catch (Exception ex)
            {
                isValidToken = false;
            }
            return isValidToken;
        }
        private bool isDBServerAvailable()
        {
            return true;
        }

        private bool isServiceAvailable()
        {
            return true;
        }
    }
    public class TokenValidator : ActionFilterAttribute
    {
        ApiManager generalUtility = new ApiManager();
        AppConfigManager appConfiguration = new AppConfigManager();
        IConfiguration configuration;
        RedisManager redisManager;
        private readonly IDistributedCache _distributedCache;

        //public TokenValidator(IDistributedCache distributedCache)
        //{

        //    _distributedCache = distributedCache;

        //}
        public TokenValidator(IDistributedCache distributedCache)
        {

            _distributedCache = distributedCache;
            redisManager = new RedisManager(_distributedCache);
        }


        private bool tokenValidationRequired = true;
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            bool validUser = false;

            RedisManagerDto redisManagerDto = new RedisManagerDto();
            if (isServiceAvailable())   /////////// return true if the api service is ready to do
            {
                if (isDBServerAvailable())   /////////// return true if the DB server is available
                {

                    StringValues authorizationToken;
                    StringValues PreAuthorization;
                    StringValues preauthrequest;
                    StringValues FunctionId;
                    StringValues ProductId;
                    filterContext.HttpContext.Request.Headers.TryGetValue("Authorization", out authorizationToken);
                    filterContext.HttpContext.Request.Headers.TryGetValue("preauthorization", out PreAuthorization);
                    filterContext.HttpContext.Request.Headers.TryGetValue("preauthrequest", out preauthrequest);
                    filterContext.HttpContext.Request.Headers.TryGetValue("functionid", out FunctionId);
                    filterContext.HttpContext.Request.Headers.TryGetValue("productid", out ProductId);
                    string controllerName = filterContext.RouteData.Values["controller"].ToString();
                    string actionName = filterContext.RouteData.Values["action"].ToString();
                    var Method = filterContext.HttpContext.Request.Method.ToString();

                    if (Method == "POST")
                    {
                        if (!(controllerName == "NewDoc" && actionName == "WebAllocation"))
                        {
                            if (filterContext.HttpContext.Request.QueryString.HasValue)
                            {
                                filterContext.Result = new StatusCodeResult(400);
                                return;
                            }
                        }
                    }


                    string[] cname = { "employee", "login" };

                    if (isTokenAvailable(authorizationToken))   /////////// return true if the token is available in the token
                    {

                        if (isTokenAvailable(PreAuthorization))   /////////// return true if the token is available in the token
                        {
                            if (isTokenAvailable(preauthrequest))   /////////// return true if the token is available in the token
                            {
                                EmptokenResponse EmptokenResponse = redisManager.GetCache<EmptokenResponse>(authorizationToken);


                                //EmptokenResponse EmptokenResponse = new EmptokenResponse();
                                if (isTokenAvailable(FunctionId))   /////////// return true if the FunctionId is available in the FunctionId
                                {
                                    if (isTokenAvailable(ProductId))   /////////// return true if the token is available in the token
                                    {
                                        if (isValidToken(EmptokenResponse))   /////////// return true if the token is valid
                                        {
                                            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                                            var SecurityToken = handler.ReadToken(authorizationToken.ToString().Replace("CVOS_", "")) as JwtSecurityToken;
                                            var jti = SecurityToken.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;
                                            string[] value = new RsaEncHelper().Decrypt(jti).Split(',');

                                            GetLoginRequests _LoginRequests = new GetLoginRequests()
                                            {
                                                Employee_code = value[2],
                                                function_id = (FunctionId.ToString()),
                                                product_id = (ProductId.ToString()),
                                                token = authorizationToken
                                            };
                                            if (isValidFunctionId(_LoginRequests))
                                            {
                                                if (CheckRateLimit(EmptokenResponse))
                                                {


                                                    validUser = true;

                                                }
                                                else
                                                {
                                                    filterContext.Result = new StatusCodeResult(429);
                                                    return;
                                                }
                                            }
                                            else
                                            {
                                                filterContext.Result = new StatusCodeResult(403);
                                                return;
                                            }
                                        }
                                        else
                                        {

                                            filterContext.Result = new StatusCodeResult(403);
                                            return;

                                        }
                                    }
                                    else
                                    {
                                        filterContext.Result = new StatusCodeResult(403);
                                        return;
                                    }
                                }
                                else
                                {
                                    filterContext.Result = new StatusCodeResult(403);
                                    return;
                                }
                            }
                            else
                            {

                                filterContext.Result = new StatusCodeResult(403);
                                return;

                            }
                        }
                        else
                        {

                            filterContext.Result = new StatusCodeResult(403);
                            return;

                        }

                    }
                    else
                    {

                        filterContext.Result = new StatusCodeResult(403);
                        return;

                    }
                }
                else
                {

                    filterContext.Result = new StatusCodeResult(403);
                    return;

                }
            }
            else
            {

                filterContext.Result = new StatusCodeResult(403);
                return;

            }
            //validUser = true;
            //if (!validUser)
            //{
            //    filterContext.Result = new StatusCodeResult(403);
            //    return;
            //}
        }

        private bool isTokenAvailable(string userContextObj)
        {
            bool checkToken = false;
            if (userContextObj != null)
            {
                checkToken = true;
            }
            return checkToken;
        }

        private bool isAutherized(string userContextObj)
        {
            return true;
        }

        private bool CheckRateLimit(EmptokenResponse tokenData)
        {

            RateLimit rateLimit = new RateLimit() { limitstarttime = DateTime.Now.AddSeconds(10) };
            bool isValidToken = true;
            RedisManagerDto redisManagerDto = new RedisManagerDto();
            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(10),
                SlidingExpiration = TimeSpan.FromMinutes(10),
            };
            List<RateLimit> lstRate = redisManager.GetCache<List<RateLimit>>("RateLimit_" + tokenData.token);
            if (lstRate.Any())
            {
                lstRate.Add(rateLimit);
            }
            else
            {
                lstRate = new List<RateLimit>();
                lstRate.Add(rateLimit);
            }
            redisManager.Remove("RateLimit_" + tokenData.token);
            redisManagerDto.options = options;
            redisManagerDto.key = "RateLimit_" + tokenData.token;
            redisManagerDto.value = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(lstRate));
            redisManager.SetCache(redisManagerDto);

            var count = lstRate.Where(a => a.limitstarttime > DateTime.Now).ToList();

            if (count.Count() > 25)
            {
                isValidToken = false;
            }
            //var diffInSeconds = ( DateTime.Now- tokenData.limitstarttime).TotalSeconds;
            //RedisManagerDto redisManagerDto = new RedisManagerDto();
            //DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
            //{
            //    AbsoluteExpiration = DateTime.Now.AddMinutes(Convert.ToDouble(tokenData.duration)),
            //    SlidingExpiration = TimeSpan.FromMinutes(Convert.ToDouble(tokenData.duration)),
            //};
            //if (diffInSeconds <= 5)
            //{
            //    tokenData.limitstarttime = DateTime.Now;
            //    tokenData.limit = tokenData.limit + 1;
            //    redisManager.Remove(tokenData.token);
            //    tokenData.endtime = DateTime.Now.AddMinutes(Convert.ToDouble(tokenData.duration));
            //    tokenData.tokenupdatetime = DateTime.Now.AddMinutes(Convert.ToDouble(tokenData.duration));
            //    redisManagerDto.options = options;
            //    redisManagerDto.key = tokenData.token;
            //    redisManagerDto.value = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(tokenData));
            //    redisManager.SetCache(redisManagerDto);

            //    if (tokenData.limit == 5)
            //    {
            //        return false;
            //    }
            //    else
            //    {

            //        tokenData.limit = 0;
            //        tokenData.limitstarttime = DateTime.Now;

            //        redisManager.Remove(tokenData.token);
            //        tokenData.endtime = DateTime.Now.AddMinutes(Convert.ToDouble(tokenData.duration));
            //        tokenData.tokenupdatetime = DateTime.Now.AddMinutes(Convert.ToDouble(tokenData.duration));
            //        redisManagerDto.options = options;
            //        redisManagerDto.key = tokenData.token;
            //        redisManagerDto.value = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(tokenData));
            //        redisManager.SetCache(redisManagerDto);
            //        return true;
            //    }
            //}
            //else
            //{
            //    tokenData.limitstarttime = DateTime.Now;
            //    tokenData.limit = 0;
            //    redisManager.Remove(tokenData.token);
            //    tokenData.endtime = DateTime.Now.AddMinutes(Convert.ToDouble(tokenData.duration));
            //    tokenData.tokenupdatetime = DateTime.Now.AddMinutes(Convert.ToDouble(tokenData.duration));
            //    redisManagerDto.options = options;
            //    redisManagerDto.key = tokenData.token;
            //    redisManagerDto.value = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(tokenData));
            //    redisManager.SetCache(redisManagerDto);
            //}

            return isValidToken;
        }
        private bool isValidToken(EmptokenResponse tokenData)
        {
            bool isValidToken = false;

            try
            {
                if (tokenData.status == "1")
                {

                    if (tokenData.endtime <= DateTime.Now)
                    {
                        isValidToken = false;
                    }
                    else
                    {
                        if (redisManager.CheckCache(tokenData.token))
                        {
                            RedisManagerDto redisManagerDto = new RedisManagerDto();
                            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
                            {
                                AbsoluteExpiration = DateTime.Now.AddMinutes(Convert.ToDouble(tokenData.duration)),
                                SlidingExpiration = TimeSpan.FromMinutes(Convert.ToDouble(tokenData.duration))
                            };
                            redisManager.Remove(tokenData.token);
                            tokenData.endtime = DateTime.Now.AddMinutes(Convert.ToDouble(tokenData.duration));
                            tokenData.tokenupdatetime = DateTime.Now.AddMinutes(Convert.ToDouble(tokenData.duration));
                            redisManagerDto.options = options;
                            redisManagerDto.key = tokenData.token;
                            redisManagerDto.value = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(tokenData));
                            redisManager.SetCache(redisManagerDto);
                            isValidToken = true;
                        }
                        else
                        {
                            isValidToken = false;
                        }
                    }
                }
                else
                {
                    isValidToken = false;
                }
            }
            catch (Exception ex)
            {
                isValidToken = false;
            }
            return isValidToken;
        }
        private bool isValidFunctionId(GetLoginRequests requests)
        {
            bool isValidToken = true;
            //try
            //{
            //    if (requests.function_id == "0")
            //    {
            //        isValidToken = true;
            //    }
            //    else if (requests.function_id == "-1")
            //    {
            //        isValidToken = false;
            //    }
            //    else
            //    {
            //        List<RoleFunctionProperties> rolefunctionList = redisManager.GetCache<List<RoleFunctionProperties>>("CVRolefunctionList_" + requests.token.Replace("CV_", ""));

            //        var _role = rolefunctionList.Where(a => a.FUNCTION_ID == requests.function_id && a.product_id == requests.product_id && a.emp_code == requests.Employee_code).ToList();

            //        if (_role != null && _role.Count() > 0)
            //        {
            //            isValidToken = true;
            //            RedisManagerDto redisManagerDto = new RedisManagerDto();
            //            redisManager.Remove("CVRolefunctionList_" + requests.token.Replace("CV_", ""));
            //            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
            //            {
            //                AbsoluteExpiration = DateTime.Now.AddMinutes(15),
            //                SlidingExpiration = TimeSpan.FromMinutes(15)
            //            };
            //            redisManagerDto.options = options;
            //            redisManagerDto.key = "CVRolefunctionList_" + requests.token.Replace("CV_", "");
            //            redisManagerDto.value = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(rolefunctionList));
            //            redisManager.SetCache(redisManagerDto);
            //        }
            //        else
            //        {
            //            isValidToken = false;
            //            RedisManagerDto redisManagerDto = new RedisManagerDto();
            //            redisManager.Remove("CVRolefunctionList_" + requests.token.Replace("CV_", ""));
            //        }

            //    }

            //}
            //catch (Exception ex)
            //{
            //    isValidToken = false;
            //}
            return isValidToken;
        }
        private bool isDBServerAvailable()
        {
            return true;
        }

        private bool isServiceAvailable()
        {
            return true;
        }
    }

    public class ResultDetailFilter : ResultFilterAttribute//----response encryption
    {
        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var options = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            var result = context.Result as ObjectResult;
            var guid = Guid.NewGuid().ToString();

            var jsonString = JsonConvert.SerializeObject(result.Value, options);
            StringValues authorizationToken;
            StringValues preauthrequestKey;
            context.HttpContext.Request.Headers.TryGetValue("Authorization", out authorizationToken);
            context.HttpContext.Request.Headers.TryGetValue("preauthrequestKey", out preauthrequestKey);

            string controllerName = context.RouteData.Values["controller"].ToString();
            string actionName = context.RouteData.Values["action"].ToString();
            string apiRequest = Base64Encode(guid);

            if (controllerName == "FormValidation" && actionName == "PREFormControlData")
            {
                apiRequest = GETRESPONSEASH(jsonString, authorizationToken, "CVOSpre_");
            }
            else if (controllerName == "Login" && actionName == "ProductList")
            {
                apiRequest = GETRESPONSEASH(jsonString, authorizationToken, "CVOSpre_");
            }
            else if (controllerName == "PasswordReset" && actionName == "ChangePassword")
            {
                apiRequest = GETRESPONSEASH(jsonString, authorizationToken, "CVOSpre_");
            }
            else if (controllerName == "Login" && actionName == "PreBranchList")
            {
                apiRequest = GETRESPONSEASH(jsonString, authorizationToken, "CVOSpre_");
            }
            else if (controllerName.ToUpper() == ("Login").ToUpper() && actionName.ToUpper() == ("UserLogin").ToUpper())
            {
                apiRequest = GETRESPONSEASH(jsonString, authorizationToken, "CVOSpre_");
            }
            else if (controllerName == "TokenValidator" && actionName == "ValidateTokenWithEmpCode")
            {
                apiRequest = GETRESPONSEASH(jsonString, authorizationToken, "CVOSpre_");
            }
            else if (controllerName == "TokenValidator" && actionName == "GetotpemployeeData")
            {
                apiRequest = GETRESPONSEASH(jsonString, authorizationToken, "CVOSpre_");
            }
            else if (controllerName == "Reports" && actionName == "PreReportProductData")
            {
                apiRequest = GETRESPONSEASH(jsonString, authorizationToken, "CVOSpre_");
            }
            else
            {
                if (controllerName != "Login" && actionName != "Preauth")
                {
                    apiRequest = GETRESPONSEASH(jsonString, authorizationToken, "CVOS_");
                }

            }
            //else if (controllerName == "TokenValidator" && actionName == "updateTokenWhenLogout")
            //{
            //    apiRequest = GETRESPONSEASH(jsonString, authorizationToken);
            //}
            WebApiRequest webApiRequest = new WebApiRequest()
            {
                apiResponse = new EncryptDecryptUtil().EncryptProperty(jsonString),
                apiRequest = apiRequest,
                apiResponseKey = preauthrequestKey
            };
            //string md = new EncryptDecryptUtil().ToMD5("hello");
            //string ip = new EncryptDecryptUtil().GetIPAddress();
            result.Value = webApiRequest;
            await next();

        }
        public EncryptedResponse InvokePostHttpLogin(string obj, string url)
        {
            PassswordUtility objPassword = new PassswordUtility();

            HttpResponseMessage response = new HttpResponseMessage() { StatusCode = HttpStatusCode.BadRequest };
            EncryptedResponse nwresponse = new EncryptedResponse();
            // object[] inputArray = new object[1];
            // inputArray[0]= obj;
            string[] str = new string[1];

            //// str[0]=inputArray[0];
            // string[] resultArray = Array.ConvertAll(inputArray, x => x.ToString());
            string contents = objPassword.Decrypt(obj);
            string emp = contents.Substring(15, 6);
            //string contents = JsonConvert.SerializeObject(obj);
            // string contents = DecryptPassword(input);
            var jsonResponse = string.Empty;
            // Object ob = new object();
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Clear();
                response = httpClient.PostAsync(url, new StringContent(contents, Encoding.UTF8, "application/json")).Result;
                if (response.IsSuccessStatusCode)
                {

                    jsonResponse = response.Content.ReadAsStringAsync().Result.ToString();
                    //ob = JsonConvert.DeserializeObject<T>(jsonResponse); 
                    string ob = objPassword.Encrypt(jsonResponse.ToString());
                    string ob1 = objPassword.Encrypt(emp.ToString());

                    var MyString1 = ob1;
                    var first1 = MyString1.Substring(0, (int)(MyString1.Length / 2));
                    var last1 = MyString1.Substring((int)(MyString1.Length / 2), (int)(MyString1.Length / 2));

                    var MyString = ob;
                    var first = MyString.Substring(0, (int)(MyString.Length / 2));
                    var last = MyString.Substring((int)(MyString.Length / 2), (int)(MyString.Length / 2));

                    Random random = new Random();
                    int randomNumber = random.Next(1, 11);

                    if (randomNumber % 2 == 0)
                    {
                        ob = first1 + first + ob1 + last + last1;
                    }
                    else
                    {
                        ob = last1 + first + ob1 + last + first1;
                    }


                    nwresponse.message = ob;

                }

            }

            return nwresponse;
        }
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        //public static string CreateResponse(object objectResult, string key)
        //{
        //    //JavaScriptSerializer json_serializer = new JavaScriptSerializer();
        //    //int encCount = 3;
        //    //string encStr1 = Base64Encode(json_serializer.Serialize(objectResult));
        //    //string encStr2 = string.Empty;
        //    //return key + encStr1 + key;

        //}

        public string GETRESPONSEASH(string response, string token, string tokenType)
        {
            string _resHash = string.Empty;
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            var SecurityToken = handler.ReadToken(token.Replace(tokenType, "")) as JwtSecurityToken;
            var jti = SecurityToken.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;
            string[] value = new RsaEncHelper().Decrypt(jti).Split(',');
            var keyVa = response + "" + value[0] + "" + token;
            var keyVa1 = response + value[0] + token;
            _resHash = new EncryptDecryptUtil().ToMD5(keyVa);

            return _resHash;
        }
        public byte[] getEdata(string response, string key)
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            byte[] hashedDataBytes;
            UTF8Encoding encoder = new UTF8Encoding();
            hashedDataBytes = md5Hasher.ComputeHash(encoder.GetBytes(response));
            return hashedDataBytes;
        }
    }

    public class RateLimit
    {
        public DateTime limitstarttime { get; set; }
        public int limit { get; set; }

    }

}

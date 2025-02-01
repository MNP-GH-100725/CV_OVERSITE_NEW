using Microsoft.Extensions.Caching.Distributed;
using Oversite.Core.DataSource.TokenValidator;
using Oversite.DTO.Response;
using Oversite.DTO.TokenValidator.Request;
using Oversite.DTO.TokenValidator.Response;
using System;

namespace Oversite.Core.BLL.TokenValidator
{
    public class TokenValidatorBLL
    {
        private readonly static Lazy<TokenValidatorBLL> m_instance;
        public static TokenValidatorBLL instance
        {
            get
            {
                return TokenValidatorBLL.m_instance.Value;
            }
        }

        static TokenValidatorBLL()
        {
            TokenValidatorBLL.m_instance = new Lazy<TokenValidatorBLL>(() => new TokenValidatorBLL());

        }
        public Response<ValidTokenResponse> ValidToken(ValidTokenRequest request)

        {
            ValidTokenResponse ValidTokenResponse = new ValidTokenResponse();
            Response<ValidTokenResponse> response = new Response<ValidTokenResponse>();
            try
            {
                ValidTokenResponse = new TokenValidatorDataSource().ValidToken(request);

                if (ValidTokenResponse.isDataAvailable)

                {
                    response.Data = ValidTokenResponse;
                    response.status = ResponseTypeContants.SUCCESS;
                    response.apiStatus = ApiStatusConstants.COMPLETED;
                    response.responseMsg = ResponseTypeContants.SUCCESS;
                }

                else
                {
                    response.apiStatus = ApiStatusConstants.NOT_COMPLETED;
                    response.responseMsg = "No Data Found";
                }
            }
            catch (Exception ex)
            {
                Exception exception = ex;
                response.status = "exception";
                response.responseMsg = "internal server error";
                response.SetExceptionError(ex.Message);
            }
            return response;
        }

        public Response<ValidTokenResponse> ValidateTokenWithEmpCode(ValidateTokenWithEmpcodeRequest request)

        {
            ValidTokenResponse ValidTokenResponse = new ValidTokenResponse();
            Response<ValidTokenResponse> response = new Response<ValidTokenResponse>();
            try
            {
                ValidTokenResponse = new TokenValidatorDataSource().ValidateTokenWithEmpCode(request);

                if (ValidTokenResponse.isDataAvailable)

                {
                    response.Data = ValidTokenResponse;
                    response.status = ResponseTypeContants.SUCCESS;
                    response.apiStatus = ApiStatusConstants.COMPLETED;
                    response.responseMsg = ResponseTypeContants.SUCCESS;
                }

                else
                {
                    response.apiStatus = ApiStatusConstants.NOT_COMPLETED;
                    response.responseMsg = "No Data Found";
                }
            }
            catch (Exception ex)
            {
                Exception exception = ex;
                response.status = "exception";
                response.responseMsg = "internal server error";
                response.SetExceptionError(ex.Message);
            }
            return response;
        }

        public Response<updateTokenWhenLogoutResponse> updateTokenWhenLogout(ValidateTokenWithEmpcodeRequest request, IDistributedCache distributedCache)

        {
            updateTokenWhenLogoutResponse ValidTokenResponse = new updateTokenWhenLogoutResponse();
            Response<updateTokenWhenLogoutResponse> response = new Response<updateTokenWhenLogoutResponse>();
            try
            {
                ValidTokenResponse = new TokenValidatorDataSource().updateTokenWhenLogout(request, distributedCache);

                if (ValidTokenResponse.isDataAvailable)

                {
                    response.Data = ValidTokenResponse;
                    response.status = ResponseTypeContants.SUCCESS;
                    response.apiStatus = ApiStatusConstants.COMPLETED;
                    response.responseMsg = ResponseTypeContants.SUCCESS;
                }

                else
                {
                    response.apiStatus = ApiStatusConstants.NOT_COMPLETED;
                    response.responseMsg = "No Data Found";
                }
            }
            catch (Exception ex)
            {
                Exception exception = ex;
                response.status = "exception";
                response.responseMsg = "internal server error";
                response.SetExceptionError(ex.Message);
            }
            return response;
        }

        public Response<EmployeResponse> GetotpemployeeData(EmployeRequest request, IDistributedCache distributedCache)
        {
            EmployeResponse ValidTokenResponse = new EmployeResponse();
            Response<EmployeResponse> response = new Response<EmployeResponse>();
            try
            {
                ValidTokenResponse = new TokenValidatorDataSource().GetotpemployeeData(request, distributedCache);

                if (ValidTokenResponse.IsDataAvailable)

                {
                    response.Data = ValidTokenResponse;
                    response.status = ResponseTypeContants.SUCCESS;
                    response.apiStatus = ApiStatusConstants.COMPLETED;
                    response.responseMsg = ResponseTypeContants.SUCCESS;
                }

                else
                {
                    response.Data = ValidTokenResponse;
                    response.responseMsg = "Session Logout";
                }

            }
            catch (Exception ex)
            {
                Exception exception = ex;
                response.status = "exception";
                response.responseMsg = "internal server error";
                response.SetExceptionError(ex.Message);
            }
            return response;
        }
        public Response<PreauthResponse> Preauth(PreauthRequest request, IDistributedCache distributedCache)
        {
            PreauthResponse preauthResponse = new PreauthResponse();
            Response<PreauthResponse> response = new Response<PreauthResponse>();
            try
            {
                preauthResponse = new TokenValidatorDataSource().Preauth(request, distributedCache);

                if (preauthResponse.token != null)
                {
                    response.Data = preauthResponse;
                    response.status = ResponseTypeContants.SUCCESS;
                    response.apiStatus = ApiStatusConstants.COMPLETED;
                    response.responseMsg = ResponseTypeContants.SUCCESS;
                }

                else
                {
                    response.apiStatus = ApiStatusConstants.NOT_COMPLETED;
                    response.responseMsg = "No Data Found";

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
                response = new TokenValidatorDataSource().Add(request);
                if (response.isDataAvailable)
                {
                    response.status = ResponseTypeContants.SUCCESS;
                    response.apiStatus = ApiStatusConstants.COMPLETED;
                    response.responseMsg = ResponseTypeContants.SUCCESS;
                }
                else
                {
                    response.responseMsg = "Failed";
                }
            }
            catch (Exception ex)
            {

            }
            return response;
        }
    }
}

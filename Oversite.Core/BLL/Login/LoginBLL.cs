using Microsoft.Extensions.Caching.Distributed;
using Oversite.Core.DataSource;
using Oversite.Core.DataSource.Login;
using Oversite.DTO.Login.Request;
using Oversite.DTO.Login.Response;
using Oversite.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oversite.Core.BLL.Login
{
    public class LoginBLL
    {
        private readonly static Lazy<LoginBLL> m_instance;

        public static LoginBLL Instance
        {
            get
            {
                return m_instance.Value;
            }
        }

        static LoginBLL()
        {
            m_instance = new Lazy<LoginBLL>(() => new LoginBLL());

        }
        public Response<LoginResponse> userLogin(LoginRequest request, IDistributedCache distributedCache)
        {

            Response<LoginResponse> response = new Response<LoginResponse>();
            LoginResponse loginResponse = new LoginResponse();
            try
            {
                loginResponse = new LoginDataSource().userLogin(request, distributedCache);

                if (loginResponse.IsDataAvailable)
                {

                    response.Data = loginResponse;
                    response.status = ResponseTypeContants.SUCCESS;
                    response.apiStatus = ApiStatusConstants.COMPLETED;
                    response.responseMsg = loginResponse.message;
                }
                else
                {
                    response.apiStatus = ApiStatusConstants.COMPLETED;
                    response.responseMsg = loginResponse.message;
                }
            }
            catch (Exception ex)
            {
                Exception exception = ex;
                response.status = "Exception";
                response.responseMsg = "Internal Server Error";
                response.SetExceptionError(ex.Message);
            }

            return response;
        }

    }
}

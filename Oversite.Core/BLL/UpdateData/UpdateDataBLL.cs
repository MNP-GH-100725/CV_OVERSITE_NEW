using Oversite.Core.DataSource.UpdateData;
using Oversite.DTO.Response;
using Oversite.DTO.UpdateData;
using System;
using System.Collections.Generic;
using System.Text;

namespace Oversite.Core.BLL.UpdateData
{
    public class UpdateDataBLL
    {
        private readonly static Lazy<UpdateDataBLL> m_instance;

        public static UpdateDataBLL Instance
        {
            get
            {
                return m_instance.Value;
            }
        }

        static UpdateDataBLL()
        {
            m_instance = new Lazy<UpdateDataBLL>(() => new UpdateDataBLL());

        }
        public Response<UpdateDataResponse> updateData(UpdateDataRequest request)
        {
 
                Response<UpdateDataResponse> updateDataResponse = new Response<UpdateDataResponse>();
                updateDataResponse = new UpdateDataSource().update(request);
                return updateDataResponse;


           
        }
    }
}

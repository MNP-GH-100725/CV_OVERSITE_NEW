using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using static Oversite.DTO.Response.GlobalVariables;

namespace Oversite.DTO.Response
{
    public class BaseResponse
    {
        public BaseResponse()
        {
            status = new ResponseStatus();
            status.flag = ProcessStatus.failed;
            status.code = APIStatus.failed;
            status.message = "Failed";
            status.timeStamp = DateTime.Now.ToString(Oversite.DTO.Response.GlobalVariables.Constants.Strings.dateTimeFormat);
        }
        public ResponseStatus status { get; set; }

    }
}

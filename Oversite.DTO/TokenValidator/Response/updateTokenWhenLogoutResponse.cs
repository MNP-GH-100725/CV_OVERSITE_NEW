using System;
using System.Collections.Generic;
using System.Text;

namespace Oversite.DTO.TokenValidator.Response
{
    public class updateTokenWhenLogoutResponse
    {

        public updateTokenWhenLogoutResponse()
        {
            isDataAvailable = false;
        }
        public string message { get; set; }
        public bool isDataAvailable { get; set; }
        public int flag { get; set; }
    }

}



using System;
using System.Collections.Generic;
using System.Text;

namespace Oversite.DTO.Response
{
    public class Error
    {
        public string ErrorCode
        {
            get;
            set;
        }

        public string ErrorMessage
        {
            get;
            set;
        }

        public string ErrorType
        {
            get;
            set;
        }

        public Error()
        {
        }
    }
}

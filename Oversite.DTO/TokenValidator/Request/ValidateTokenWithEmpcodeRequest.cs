using System;
using System.Collections.Generic;
using System.Text;

namespace Oversite.DTO.TokenValidator.Request
{
   public  class ValidateTokenWithEmpcodeRequest
    {
        public string userContextObj { get; set; }
        public string empCode { get; set; }
    }
}

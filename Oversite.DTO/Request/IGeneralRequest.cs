using System;
using System.Collections.Generic;
using System.Text;

namespace Oversite.DTO.Request
{
	public class GeneralRequest
	{
		
	
		public int firmId { get; set; }
		public int productId { get; set; }
	}

    public class EncryptRequest
    {
        public string request { get; set; }

    }
    public class WebApiRequest
    {
        public string apiRequest { get; set; }
        public string apiResponseKey { get; set; }
        public string apiResponse { get; set; }
    }
}

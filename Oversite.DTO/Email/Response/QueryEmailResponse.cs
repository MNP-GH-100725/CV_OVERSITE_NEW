using System;
using System.Collections.Generic;
using System.Text;

namespace Oversite.DTO.Email.Response
{
  public  class QueryEmailResponse
    {
        public bool isDataAvailable { get; set; }
        public QueryEmailResponse()
        {
            isDataAvailable = false;
        }
        public string message { get; set; }
    }

    public class TotalDisResponse
    {
        public bool isDataAvailable { get; set; }
        public TotalDisResponse()
        {
            isDataAvailable = false;
        }
        public string message { get; set; }
    }

    public class NCHResponse
    {
        public bool isDataAvailable { get; set; }
        public NCHResponse()
        {
            isDataAvailable = false;
        }
        public string message { get; set; }
    }
}

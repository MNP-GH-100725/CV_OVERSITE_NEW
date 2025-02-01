using System;
using System.Collections.Generic;
using System.Text;

namespace Oversite.DTO.Response
{
    public class KeyValueResponse
    {
        public bool isDataAvailable { get; set; }
        public KeyValueResponse()
        {
            isDataAvailable = false;

        }
        public string keyvalue { get; set; }
        public string message { get; set; }
        public KeyValueProperties KeyValueProperties { get; set; }
    }
    public class KeyValueProperties
    {
        public string keyvalue { get; set; }

    }
}

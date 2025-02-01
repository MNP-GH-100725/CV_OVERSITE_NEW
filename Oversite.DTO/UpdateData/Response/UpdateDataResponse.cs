using System;
using System.Collections.Generic;
using System.Text;

namespace Oversite.DTO.UpdateData
{
    public class UpdateDataResponse
    {
        public UpdateDataResponse()
        {
            IsDataAvailable = false;
           
        }
        
        public List<UpdateDataProperties> UpdateData { get; set; }

        public bool IsDataAvailable { get; set; }
        public string message { get; set; }
        public string token { get; set; }
    }
   
}


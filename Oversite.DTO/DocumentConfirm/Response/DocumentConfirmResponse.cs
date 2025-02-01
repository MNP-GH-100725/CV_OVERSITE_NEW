using System;
using System.Collections.Generic;
using System.Text;

namespace Oversite.DTO.DocumentConfirm.Response
{
    public class DocumentConfirmResponse
    {
        public bool isDataAvailable { get; set; }
        public DocumentConfirmResponse()
        {
            isDataAvailable = false;
        }
        public string message { get; set; }
        //public List<CommonMasterProperties> commonDataList { get; set; }
    }
}

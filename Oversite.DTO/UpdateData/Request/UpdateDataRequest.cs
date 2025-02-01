using System;
using System.Collections.Generic;
using System.Text;

namespace Oversite.DTO.UpdateData
{
    public class UpdateDataRequest
    {
        //public int firmId { get; set; }
        
        //public string documentId { get; set; }
        //public string searchType { get; set; }
        public string docStatus { get; set; }
        public string docRemarks { get; set; }
        public string enterBy { get; set; }
        public int productId { get; set; }
        //public int docType { get; set; }
        public List<DocupdateProperties>docupdatelist { get; set; }


    }
}

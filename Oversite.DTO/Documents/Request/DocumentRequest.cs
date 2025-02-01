using Oversite.DTO.Documents.Properties;
using System;
using System.Collections.Generic;
using System.Text;

namespace Oversite.DTO.CommonMaster.Request
{
    public class DocumentRequest
    {
        public string DocumentNo { get; set; }
        public int productId { get; set; }
        public string documentType { get; set; }
        public string customerId { get; set; }
        public string enterBy { get; set; }
        public string applicationId { get; set; }
    }
    public class GetDocumentRequest
    {
        public string searchType { get; set; }
        public string enterBy { get; set; }
    }
    public class UpdateDocumentRequest
    {
        public string searchValue { get; set; }
        public string enterBy { get; set; }
    }
    public class AddDocumentRequest
    {
        public List<DocUploadProperties> OversiteDocumentList { get; set; }

    }
} 
 
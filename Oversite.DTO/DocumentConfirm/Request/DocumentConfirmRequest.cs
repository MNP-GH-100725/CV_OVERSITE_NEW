using Oversite.DTO.DocumentConfirm.Properties;
using System;
using System.Collections.Generic;
using System.Text;

namespace Oversite.DTO.DocumentConfirm.Request
{
    public class DocumentConfirmRequest
    {
        public string remarks { get; set; }
        public int schemeId { get; set; }
        public string searchType { get; set; }
        public string verifyStatus { get; set; }
        public string enterBy { get; set; }
        public string enterDate { get; set; }
        public int  roleId { get; set; }
        public int flag { get; set; }
        public string rmRemarks { get; set; }

        public int decisionFlag { get; set; }

        public int categoryId { get; set; }
        public string sendbackto { get; set; }

        public List<DocReConfirmProperties> confirmlist { get; set; }
    }
}

using Oversite.DTO.Email.Properties;
using System;
using System.Collections.Generic;
using System.Text;

namespace Oversite.DTO.Email.Request
{
   public class QueryEmailRequest
    {
       public List<QueryEmailProperties> queryEmails { get; set; }
        public List<RMProperties> RMsMails { get; set; }

    }

    public class TotalDisbRequest
    {
        public List<TotalDisbProperties> totalDisbs { get; set; }
    }

    public class NCHRequest
    {
        //public List<NCHProperties> nCHProperties { get; set; }
    }
}

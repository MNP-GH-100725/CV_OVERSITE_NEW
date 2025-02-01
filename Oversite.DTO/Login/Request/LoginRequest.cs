using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oversite.DTO.Login.Request
{
    public class LoginRequest
    {
        public int firmId { get; set; }
        //public int productId { get; set; } 
        public string siganture { get; set; }
        public string employeeId { get; set; }
        public string password { get; set; }
    }
    public class EmpTokenRequest
    {
        public string emp_code { get; set; }
        public string token { get; set; }
    }
}

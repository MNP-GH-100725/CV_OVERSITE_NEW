using System;
using System.Collections.Generic;
using System.Text;

namespace Oversite.DTO.DashBoard.Properties
{
    public class DashBoardProperties
    {
        public string loanId { get; set; }
        public string applicationNo { get; set; }
        public string cusName { get; set; }

        public string disDate { get; set; }
        public string branch { get; set; }
        public string loanAmount { get; set; }
        public string statusId { get; set; }
        public string status { get; set; }
        public string redirectFlag { get; set; }
        //new by 100890
        public string roleid { get; set; }
        public decimal TAT { get; set; }
    }
    public class MonthListProperties
    {
        public int MonthId { get; set; }
        public string MonthName { get; set; }
    }
    public class RegionListProperties
    {
        public int region_id { get; set; }
        public string region_name { get; set; }
    }
    public class ProductListProperties
    {
        public int product_id { get; set; }
        public string product_name { get; set; }
    }
    public class StatusListProperties
    {
        public decimal statusID { get; set; }
        public string description { get; set; }
    }

    public class BranchListProperties
    {
        public int branch_id { get; set; }
        public int region_id { get; set; }
        public string branch_name { get; set; }
    }

}

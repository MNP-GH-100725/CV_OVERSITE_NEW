using System;
using System.Collections.Generic;
using System.Text;

namespace Oversite.DTO.DashBoard.Request
{
    public  class DashBoardRequest
    {
        public string productId { get; set; }
        public string enterBy { get; set; }
        public string regionId { get; set; }
        public string roleId { get; set; }
        public string MonthId { get; set; }
        public string searchValue { get; set; }
        public string status { get; set; }
        public string ToDate { get; set; }
        public string branchId { get; set; }



    }
    public class MonthListRequest
    {
        public int Flag { get; set; }
    }
    public class RegionListRequest
    {
        public int Flag { get; set; }
        public string empcode { get; set; }
    }

    public class ProductListRequest
    {
        public int Flag { get; set; }
        public int enterBy { get; set; }

    }
    public class StatusListRequest
    {
        public int Flag { get; set; }
        public string  enterBy { get; set; }
    }
    public class BranchListRequest
    {
        public int Flag { get; set; }
        public int regionid { get; set; }
    }
}

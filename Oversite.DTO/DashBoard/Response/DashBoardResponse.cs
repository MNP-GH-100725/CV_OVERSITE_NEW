using Oversite.DTO.DashBoard.Properties;
using System;
using System.Collections.Generic;
using System.Text;

namespace Oversite.DTO.DashBoard.Response
{
    public class DashBoardResponse
    {
        //public bool isDataAvailable { get; set; }
        //public DashBoardResponse()
        //{
        //    isDataAvailable = false;
        //}
        //public string message { get; set; }
        public List<DashBoardProperties> DataList { get; set; }
    }
    public class MonthListResponse
    {
        
        public List<MonthListProperties> MonthList { get; set; }
    }
    public class RegionListResponse
    {
        public List<RegionListProperties> RegionList { get; set; }
    }
    public class ProductListResponse
    {
        public List<ProductListProperties> ProductList { get; set; }
    }
    public class StatusListResponse
    {
        public List<StatusListProperties> StatusList { get; set; }

    }
    public class BranchListResponse
    {
        public List<BranchListProperties> BranchList { get; set; }
    }

}

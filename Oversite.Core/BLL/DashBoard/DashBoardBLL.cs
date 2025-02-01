using Oversite.Core.DataSource.Dashboard;
using Oversite.DTO.DashBoard.Request;
using Oversite.DTO.DashBoard.Response;
using Oversite.DTO.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Oversite.Core.BLL.DashBoard
{
    public  class DashBoardBLL
    {
        public readonly static Lazy<DashBoardBLL> m_instance;
        public static DashBoardBLL Instance
        {
            get
            {
                return DashBoardBLL.m_instance.Value;
            }
        }
        static DashBoardBLL()
        {
            DashBoardBLL.m_instance = new Lazy<DashBoardBLL>(() => new DashBoardBLL());
        }
        public Response<DashBoardResponse> GetData(DashBoardRequest request)
        {
            Response<DashBoardResponse> dashBoardResponse = new Response<DashBoardResponse>();
            dashBoardResponse = new DashBoardDataSource().Data(request);
            return dashBoardResponse;


        }
        public Response<MonthListResponse> FillMonth(MonthListRequest request)
        {
            Response<MonthListResponse> monthListResponse = new Response<MonthListResponse>();
            monthListResponse = new DashBoardDataSource().FillMonth(request);
            return monthListResponse;
        }
        public Response<RegionListResponse> FillRegion(RegionListRequest request)
        {
            Response<RegionListResponse> regionListResponse = new Response<RegionListResponse>();
            regionListResponse = new DashBoardDataSource().FillRegion(request);
            return regionListResponse;
        }

        public Response<ProductListResponse> FillProduct(ProductListRequest request)
        {
            Response<ProductListResponse> productListResponse = new Response<ProductListResponse>();
            productListResponse = new DashBoardDataSource().FillProduct(request);
            return productListResponse;
        }
        public Response<StatusListResponse> FillStatus(StatusListRequest request)
        {
            Response<StatusListResponse> statusListResponse = new Response<StatusListResponse>();
            statusListResponse = new DashBoardDataSource().FillStatus(request);
            return statusListResponse;
        }
        public Response<BranchListResponse> FillBranch(BranchListRequest request)
        {
            Response<BranchListResponse> branchlistResponse = new Response<BranchListResponse>();
            branchlistResponse = new DashBoardDataSource().FillBranch(request);
            return branchlistResponse;
        }

    }
}

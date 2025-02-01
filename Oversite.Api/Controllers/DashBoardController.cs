using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Oversite.Core.BLL.DashBoard;
using Oversite.DTO.DashBoard.Request;
using Oversite.DTO.DashBoard.Response;
using Oversite.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oversite.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashBoardController : ControllerBase
    {
        [HttpPost("DashBoardData")]
        public ActionResult<Response<DashBoardResponse>> Get([FromBody] DashBoardRequest request)
        {
            if (ModelState.IsValid)
            {
                return DashBoardBLL.Instance.GetData(request);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        [HttpPost("FillMonth")]
        public ActionResult<Response<MonthListResponse>> Month([FromBody] MonthListRequest MonRequest)
        {
            return DashBoardBLL.Instance.FillMonth(MonRequest);
            
        }

       
        [HttpPost("FillRegion")]
        public ActionResult<Response<RegionListResponse>> Region([FromBody] RegionListRequest RegRequest)
        {
            return DashBoardBLL.Instance.FillRegion(RegRequest);
           
        }

        [HttpPost("FillProduct")]
        public ActionResult<Response<ProductListResponse>> Product([FromBody] ProductListRequest ProdRequest)
        {
            return DashBoardBLL.Instance.FillProduct(ProdRequest);
        }

        [HttpPost("FillStatus")]
        public ActionResult<Response<StatusListResponse>> Status([FromBody] StatusListRequest StRequest)
        {
            return DashBoardBLL.Instance.FillStatus(StRequest);
        }
        [HttpPost("FillBranch")]
        public ActionResult<Response<BranchListResponse>> Branch([FromBody] BranchListRequest branchRequest)
        {
            return DashBoardBLL.Instance.FillBranch(branchRequest);

        }

    }
}

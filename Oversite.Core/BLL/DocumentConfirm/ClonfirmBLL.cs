using Oversite.Core.DataSource.DocumentConfirm;
using Oversite.DTO.CommonMaster.Response;
using Oversite.DTO.DocumentConfirm.Request;
using Oversite.DTO.DocumentConfirm.Response;
using Oversite.DTO.Documents.Request;
using Oversite.DTO.Request;
using Oversite.DTO.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Oversite.Core.BLL.DocumentConfirm
{
    public class ClonfirmBLL
    {
		private readonly static Lazy<ClonfirmBLL> m_instance;
		public static ClonfirmBLL instance
		{
			get
			{
				return ClonfirmBLL.m_instance.Value;
			}
		}

		static ClonfirmBLL()
		{
			ClonfirmBLL.m_instance = new Lazy<ClonfirmBLL>(() => new ClonfirmBLL());

		}
		public Response<DocumentConfirmResponse> Confirm(DocumentConfirmRequest request)
		{

			Response<DocumentConfirmResponse> documentConfirmResponse = new Response<DocumentConfirmResponse>();
			documentConfirmResponse = new ConfirmDataSource().confirm(request);
				return documentConfirmResponse;
			
		}
		public Response<RepaymentScheduleResponse> Repay(AccRepaymentScheduleRequest request)
		{

			Response<RepaymentScheduleResponse> repayResponse = new Response<RepaymentScheduleResponse>();
			repayResponse = new ConfirmDataSource().Repay(request);
			return repayResponse;

		}
		public Response<LoanDetailsResponse> FetchData(LoanDetailsRequest request)
		{

			Response<LoanDetailsResponse> loanDetailsResponse = new Response<LoanDetailsResponse>();
			loanDetailsResponse = new ConfirmDataSource().FetchData(request);
			return loanDetailsResponse;

		}
		public Response<SaveRepayDetailsResponse> SaveData(SaveRepayDetailsRequest request)
		{

			Response<SaveRepayDetailsResponse> saveRepayDetailsResponse = new Response<SaveRepayDetailsResponse>();
			saveRepayDetailsResponse = new ConfirmDataSource().SaveData(request);
			return saveRepayDetailsResponse;

		}
	}
}

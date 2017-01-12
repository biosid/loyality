using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RapidSoft.Loaylty.BonusGateway.BonusGateway;

namespace RapidSoft.VTB24.BankConnector.Tests.StubServices
{
	public class BonusGatewayStub: StubBase, BonusGateway
	{
		public GetBalanceResponse GetBalance(GetBalanceRequest request)
		{
			throw new NotImplementedException();
		}

		public Task<GetBalanceResponse> GetBalanceAsync(GetBalanceRequest request)
		{
			throw new NotImplementedException();
		}

		public GetAccountsByClientResponse GetAccountsByClient(GetAccountsByClientRequest request)
		{
			throw new NotImplementedException();
		}

		public Task<GetAccountsByClientResponse> GetAccountsByClientAsync(GetAccountsByClientRequest request)
		{
			throw new NotImplementedException();
		}

		public ApplyDiscountResponse ApplyDiscount(ApplyDiscountRequest request)
		{
			throw new NotImplementedException();
		}

		public Task<ApplyDiscountResponse> ApplyDiscountAsync(ApplyDiscountRequest request)
		{
			throw new NotImplementedException();
		}

		public ApplyWithdrawByAccountResponse ApplyWithdrawByAccount(ApplyWithdrawByAccountRequest request)
		{
			throw new NotImplementedException();
		}

		public Task<ApplyWithdrawByAccountResponse> ApplyWithdrawByAccountAsync(ApplyWithdrawByAccountRequest request)
		{
			throw new NotImplementedException();
		}

		public RefundPointsByChequeResponse RefundPointsByCheque(RefundPointsByChequeRequest request)
		{
			throw new NotImplementedException();
		}

		public Task<RefundPointsByChequeResponse> RefundPointsByChequeAsync(RefundPointsByChequeRequest request)
		{
			throw new NotImplementedException();
		}

		public RefundPointsResponse RefundPoints(RefundPointsRequest request)
		{
			throw new NotImplementedException();
		}

		public Task<RefundPointsResponse> RefundPointsAsync(RefundPointsRequest request)
		{
			throw new NotImplementedException();
		}

		public RollbackPointsResponse RollbackPoints(RollbackPointsRequest request)
		{
			if (string.IsNullOrEmpty(request.OriginalRequestId))
			{
				throw new ArgumentException("request.OriginalRequestId is null or empty");
			}

			if (string.IsNullOrEmpty(request.RequestId))
			{
				throw new ArgumentException("request.RequestId is null or empty");
			}

			if (string.IsNullOrEmpty(request.PartnerId))
			{
				throw new ArgumentException("request.PartnerId is null or empty");
			}

			if (string.IsNullOrEmpty(request.PosId))
			{
				throw new ArgumentException("request.PosId is null or empty");
			}

			if (string.IsNullOrEmpty(request.TerminalId))
			{
				throw new ArgumentException("request.TerminalId is null or empty");
			}

			return new RollbackPointsResponse
			{
				DateTime = DateTime.Now,
				Error = this.GetStubDescription(),
				Status = 0,
				UtcDateTime = DateTime.UtcNow,
			};
		}

		public Task<RollbackPointsResponse> RollbackPointsAsync(RollbackPointsRequest request)
		{
			throw new NotImplementedException();
		}

		public ConfirmPointsResponse ConfirmPoints(ConfirmPointsRequest request)
		{
			if (string.IsNullOrEmpty(request.OriginalRequestId))
			{
				throw new ArgumentException("request.OriginalRequestId is null or empty");
			}

			if (string.IsNullOrEmpty(request.RequestId))
			{
				throw new ArgumentException("request.RequestId is null or empty");
			}

			if (string.IsNullOrEmpty(request.PartnerId))
			{
				throw new ArgumentException("request.PartnerId is null or empty");
			}

			if (string.IsNullOrEmpty(request.PosId))
			{
				throw new ArgumentException("request.PosId is null or empty");
			}

			if (string.IsNullOrEmpty(request.TerminalId))
			{
				throw new ArgumentException("request.TerminalId is null or empty");
			}

			return new ConfirmPointsResponse
				       {
					       DateTime = DateTime.Now,
					       Error = this.GetStubDescription(),
					       Status = 0,
					       UtcDateTime = DateTime.UtcNow,
				       };
		}

		public Task<ConfirmPointsResponse> ConfirmPointsAsync(ConfirmPointsRequest request)
		{
			throw new NotImplementedException();
		}

		public GetCurrenciesByLoyaltyProgramResponse GetCurrenciesByLoyaltyProgram(GetCurrenciesByLoyaltyProgramRequest request)
		{
			throw new NotImplementedException();
		}

		public Task<GetCurrenciesByLoyaltyProgramResponse> GetCurrenciesByLoyaltyProgramAsync(GetCurrenciesByLoyaltyProgramRequest request)
		{
			throw new NotImplementedException();
		}
	}
}

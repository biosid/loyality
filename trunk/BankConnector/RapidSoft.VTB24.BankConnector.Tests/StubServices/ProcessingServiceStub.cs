using RapidSoft.Loaylty.Processing.ProcessingService;

namespace RapidSoft.VTB24.BankConnector.Tests.StubServices
{
	using System;
	using System.Linq;
	using System.Threading.Tasks;

	public class ProcessingServiceStub : StubBase, ProcessingService
	{
		public BatchDepositByCardResponse BatchDepositByCard(BatchDepositByCardRequest request)
		{
			throw new NotImplementedException();
		}

		public Task<BatchDepositByCardResponse> BatchDepositByCardAsync(BatchDepositByCardRequest request)
		{
			throw new NotImplementedException();
		}

		public BatchActivateClientsResponse BatchActivateClients(BatchActivateClientsRequest request)
		{
			var response = new BatchActivateClientsResponseType
			{
				StatusCode = 0,
			};
			response.ClientActivationResults =
				request.Request.ActivationFacts.Select(
					x => new BatchActivateClientsResponseTypeClientActivationResult
					{
						ClientExternalId = x.ClientExternalId,
						ClientId = x.ClientExternalId,
						Error = this.GetStubDescription(),
						StatusCode = 0,
					}).ToArray();
			return new BatchActivateClientsResponse(response);
		}

		public Task<BatchActivateClientsResponse> BatchActivateClientsAsync(BatchActivateClientsRequest request)
		{
			throw new NotImplementedException();
		}

		public BatchCreateCardsResponse BatchCreateCards(BatchCreateCardsRequest request)
		{
			throw new NotImplementedException();
		}

		public Task<BatchCreateCardsResponse> BatchCreateCardsAsync(BatchCreateCardsRequest request)
		{
			throw new NotImplementedException();
		}

		public BatchCreateClientsResponse BatchCreateClients(BatchCreateClientsRequest request)
		{
			var responseProcessingList = request.Request.ClientRegistrationFacts.Select(
				x => new BatchCreateClientsResponseTypeClientRegistrationResult
				{
					ClientExternalId = x.ClientExternalId,
					ClientId = x.ClientId,
					Error = this.GetStubDescription(),
					StatusCode = 0
				}).ToList();
			var responseProcessing = new BatchCreateClientsResponseType
			{
				StatusCode = 0,
				ClientRegistrationResults = responseProcessingList.ToArray()
			};
			return new BatchCreateClientsResponse(responseProcessing);
		}

		public Task<BatchCreateClientsResponse> BatchCreateClientsAsync(BatchCreateClientsRequest request)
		{
			throw new NotImplementedException();
		}

		public AuthorizeDiscountByChequeResponse AuthorizeDiscountByCheque(AuthorizeDiscountByChequeRequest request)
		{
			throw new NotImplementedException();
		}

		public Task<AuthorizeDiscountByChequeResponse> AuthorizeDiscountByChequeAsync(AuthorizeDiscountByChequeRequest request)
		{
			throw new NotImplementedException();
		}

		public RollbackTransactionByIdResponse RollbackTransactionById(RollbackTransactionByIdRequest request)
		{
			throw new NotImplementedException();
		}

		public Task<RollbackTransactionByIdResponse> RollbackTransactionByIdAsync(RollbackTransactionByIdRequest request)
		{
			throw new NotImplementedException();
		}

		public RefundByChequeResponse RefundByCheque(RefundByChequeRequest request)
		{
			throw new NotImplementedException();
		}

		public Task<RefundByChequeResponse> RefundByChequeAsync(RefundByChequeRequest request)
		{
			throw new NotImplementedException();
		}

		public RefundByTransactionExternalIdResponse RefundByTransactionExternalId(RefundByTransactionExternalIdRequest request)
		{
			throw new NotImplementedException();
		}

		public Task<RefundByTransactionExternalIdResponse> RefundByTransactionExternalIdAsync(RefundByTransactionExternalIdRequest request)
		{
			throw new NotImplementedException();
		}

		public GetBalanceByCardResponse GetBalanceByCard(GetBalanceByCardRequest request)
		{
			throw new NotImplementedException();
		}

		public Task<GetBalanceByCardResponse> GetBalanceByCardAsync(GetBalanceByCardRequest request)
		{
			throw new NotImplementedException();
		}

		public BatchDeactivateClientsResponse BatchDeactivateClients(BatchDeactivateClientsRequest request)
		{
			throw new NotImplementedException();
		}

		public Task<BatchDeactivateClientsResponse> BatchDeactivateClientsAsync(BatchDeactivateClientsRequest request)
		{
			throw new NotImplementedException();
		}

		public BatchLockClientsResponse BatchLockClients(BatchLockClientsRequest request)
		{
		    var result = new BatchLockClientsResponseType();
		    result.StatusCode = 0;
		    result.ClientLockResults =
		        request.Request.LockFacts.Select(
		            x =>
		            new BatchLockClientsResponseTypeClientLockResult
		                {
		                    ClientExternalId = x.ClientExternalId,
		                    ClientId = x.ClientExternalId,
		                    Error = this.GetStubDescription(),
		                    StatusCode = 0,
		               }).ToArray();
            return new BatchLockClientsResponse(result);
		}

		public Task<BatchLockClientsResponse> BatchLockClientsAsync(BatchLockClientsRequest request)
		{
			throw new NotImplementedException();
		}

		public BatchDeleteClientsResponse BatchDeleteClients(BatchDeleteClientsRequest request)
		{
			var processingResponse = new BatchDeleteClientsResponseType();
			processingResponse.StatusCode = 0;
			processingResponse.ClientDeletionResults =
				request.Request.DeletionFacts.Select(
					x =>
					new BatchDeleteClientsResponseTypeClientDeletionResult
					{
						ClientExternalId = x.ClientExternalId,
						ClientId = x.ClientExternalId,
						StatusCode = 0,
						Error = this.GetStubDescription(),
					}).ToArray();
			return new BatchDeleteClientsResponse(processingResponse);
		}

		public Task<BatchDeleteClientsResponse> BatchDeleteClientsAsync(BatchDeleteClientsRequest request)
		{
			throw new NotImplementedException();
		}

		public BatchDepositByClientsResponse BatchDepositByClients(BatchDepositByClientsRequest request)
		{
			var processingResponse = new BatchDepositByClientsResponseType();
			processingResponse.StatusCode = 0;
			processingResponse.DepositByClientResults =
				request.Request.DepositTransactions.Select(
					x =>
					new BatchDepositByClientsResponseTypeDepositByClientResult
					{
						BonusSum = x.BonusSum,
						Error = this.GetStubDescription(),
						StatusCode = 0,
						TransactionDateTime = DateTime.Now,
						TransactionDateTimeUTC = DateTime.UtcNow,
						TransactionExternalId = x.TransactionExternalId,
						TransactionId = x.GetHashCode(),
					}).ToArray();
			return new BatchDepositByClientsResponse(processingResponse);
		}

		public Task<BatchDepositByClientsResponse> BatchDepositByClientsAsync(BatchDepositByClientsRequest request)
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

		public AuthorizeWithdrawByAccountResponse AuthorizeWithdrawByAccount(AuthorizeWithdrawByAccountRequest request)
		{
			throw new NotImplementedException();
		}

		public Task<AuthorizeWithdrawByAccountResponse> AuthorizeWithdrawByAccountAsync(AuthorizeWithdrawByAccountRequest request)
		{
			throw new NotImplementedException();
		}

		public GetOperationHistoryResponse GetOperationHistory(GetOperationHistoryRequest request)
		{
			throw new NotImplementedException();
		}

		public Task<GetOperationHistoryResponse> GetOperationHistoryAsync(GetOperationHistoryRequest request)
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

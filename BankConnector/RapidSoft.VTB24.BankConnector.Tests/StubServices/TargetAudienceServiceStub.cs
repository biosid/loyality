using System.Threading.Tasks;

using RapidSoft.Loaylty.PromoAction.WsClients.TargetAudienceService;

namespace RapidSoft.VTB24.BankConnector.Tests.StubServices
{
	using System;
	using System.Linq;

	using RapidSoft.VTB24.BankConnector.Processors;

	using ResultBase = RapidSoft.Loaylty.PromoAction.WsClients.TargetAudienceService.ResultBase;

    public class TargetAudienceServiceStub
		: StubBase, ITargetAudienceService
	{
        public string Echo(string message)
        {
            throw new NotImplementedException();
        }

        public Task<string> EchoAsync(string message)
        {
            throw new NotImplementedException();
        }

        public GetClientTargetAudiencesResult GetClientTargetAudiences(string clientId)
        {
            return new GetClientTargetAudiencesResult
                   {
                       Success = true,
                       ResultCode = 0,
                       ClientTargetAudiences =
                           new[]
                           {
                               new TargetAudience
                               {
                                   Id = SegmentExtensions.StandartSegmentId,
                                   IsSegment = true,
                                   Name = SegmentExtensions.StandartSegmentId
                               }
                           }
                   };
        }

		public Task<GetClientTargetAudiencesResult> GetClientTargetAudiencesAsync(string clientId)
		{
			throw new NotImplementedException();
		}

		public AssignClientAudienceResult AssignClientTargetAudience(AssignClientTargetAudienceParameters parameters)
		{
			var assignResult = new AssignClientAudienceResult();
			assignResult.Success = true;
			assignResult.ResultCode = 0;
			assignResult.ClientTargetAudienceRelations =
				parameters.ClientAudienceRelations.Select(
					x =>
					new ClientTargetAudienceRelationResult
					{
						ClientId = x.ClientId,
						PromoActionId = x.PromoActionId,
						AssignResultCode = 1,
						ResultDescription = this.GetStubDescription(),
					}).ToArray();
			return assignResult;
		}

		public Task<AssignClientAudienceResult> AssignClientTargetAudienceAsync(AssignClientTargetAudienceParameters parameters)
		{
			throw new NotImplementedException();
		}

	    public ResultBase AssignClientSegment(AssignClientSegmentParameters parameters)
	    {
	        return new ResultBase { Success = true, ResultCode = 0 };
	    }

	    public Task<ResultBase> AssignClientSegmentAsync(AssignClientSegmentParameters parameters)
	    {
	        throw new NotImplementedException();
	    }

        public GetTargetAudiencesResult GetTargetAudiences(bool? isSegment)
        {
            throw new NotImplementedException();
        }

        public Task<GetTargetAudiencesResult> GetTargetAudiencesAsync(bool? isSegment)
        {
            throw new NotImplementedException();
        }
	}
}

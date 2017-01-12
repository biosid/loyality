namespace RapidSoft.Loaylty.PartnersConnector.Interfaces.Entities
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;

	[DataContract]
	public class UpdateOrdersStatusResult : ResultBase
	{
        public UpdateOrderStatusResult[] UpdateOrderStatusResults { get; set; }

		public static UpdateOrdersStatusResult BuildSuccess(IEnumerable<UpdateOrderStatusResult> orderStatusResults)
		{
		    var asArray = orderStatusResults.ToArray();
            return new UpdateOrdersStatusResult
                       {
                            UpdateOrderStatusResults = asArray,
                            ResultCode = (int)ResultCodes.Success
                       };
		}

		public static UpdateOrdersStatusResult BuildUnknownFail(string error)
		{
			return new UpdateOrdersStatusResult { ResultCode = (int)ResultCodes.UnknownError, ResultDescription = error };
		}
	}

    public class UpdateOrderStatusResult : ResultBase
    {
        public string ExternalOrderId { get; set; }

        public int? OrderId { get; set; }
    }
}
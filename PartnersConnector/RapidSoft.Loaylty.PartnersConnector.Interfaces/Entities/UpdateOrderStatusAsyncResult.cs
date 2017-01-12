namespace RapidSoft.Loaylty.PartnersConnector.Interfaces.Entities
{
	using System.Runtime.Serialization;

	[DataContract]
	public class UpdateOrderStatusAsyncResult : ResultBase
	{
		public static UpdateOrderStatusAsyncResult BuildSuccess()
		{
			return new UpdateOrderStatusAsyncResult { ResultCode = (int)ResultCodes.Success, ResultDescription = null };
		}
	}
}
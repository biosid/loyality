namespace RapidSoft.Loaylty.PartnersConnector.Interfaces.Entities
{
	using System.Runtime.Serialization;

	[DataContract]
	public class CommitOrderAsyncResult : ResultBase
	{
		public static CommitOrderAsyncResult BuildSuccess()
		{
			return new CommitOrderAsyncResult { ResultCode = (int)ResultCodes.Success, ResultDescription = null };
		}
	}
}
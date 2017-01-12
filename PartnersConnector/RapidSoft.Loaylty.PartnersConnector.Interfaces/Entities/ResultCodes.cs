namespace RapidSoft.Loaylty.PartnersConnector.Interfaces.Entities
{
	public enum ResultCodes
	{
        InvalidArgument = -1,
		Success = 0,
		UnknownError = 1,

		InvalidOrderItemPrice = 1,
		InvalidOrderUpdateStatus = 2,

		InvalidUserTicket = 1,
		InvalidSignature = 41,

		UnknowCancelPaymentError = 1,

		InvalidPartnerConfig = 10,
		CommunicationFail = 11,
		InvalidPartnerResponse = 12,

        OrderIsNotPaidOrCancelled = 1,
        OrderNotFound = 2,
        UnknownGetPaymentStatusError = 3,

        UnknownCustomCommitMethod = 51,
        CustomCommitFailed = 52
	}
}
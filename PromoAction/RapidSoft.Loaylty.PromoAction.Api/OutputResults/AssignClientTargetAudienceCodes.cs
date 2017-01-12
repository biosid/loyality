namespace RapidSoft.Loaylty.PromoAction.Api.OutputResults
{
    public enum AssignClientTargetAudienceCodes
    {
        //клиент успешно добавлен в участники компании в Системе лояльности;
        Success = 1,
        
        //кампания с указанным идентификатором не найдена;
        TargetAudienceNotFound = 2,

		////3 – клиент с указанным идентификатором не найден.
		//ClientNotFound = 3
    }
}
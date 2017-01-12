using RapidSoft.Loaylty.PromoAction.Api.InputParameters;

namespace RapidSoft.Loaylty.PromoAction.Api
{
    using System.ServiceModel;

    using Monitoring;

    using RapidSoft.Loaylty.PromoAction.Api.OutputResults;

    /// <summary>
    /// Интерфейс-контракт компонента "Промо акции".
    /// </summary>
    [ServiceContract]
    public interface ITargetAudienceService : ISupportService
    {
        /// <summary>
        /// Операция возвращает список ЦА к которым принадлежит клиент согласно хранимому реестру клиентов целевых аудиторий.
        /// </summary>
        /// <param name="clientId">
        /// Идентификатор клиента.
        /// </param>
        /// <returns>
        /// Массив записей типа Целевая аудитория.
        /// </returns>
        [OperationContract]
        GetClientTargetAudiencesResult GetClientTargetAudiences(string clientId);

        [OperationContract]
        AssignClientAudienceResult AssignClientTargetAudience(AssignClientTargetAudienceParameters parameters);

        [OperationContract]
        ResultBase AssignClientSegment(AssignClientSegmentParameters parameters);

        [OperationContract]
        GetTargetAudiencesResult GetTargetAudiences(bool? isSegment);
    }
}

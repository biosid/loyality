namespace RapidSoft.Loaylty.PromoAction.Api.OutputResults
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    using RapidSoft.Loaylty.PromoAction.Api.Entities;

    [DataContract]
    public class GetMetadataByDomainIdResult : ResultBase
    {
        [DataMember]
        public IList<EntityMetadata> Entities { get; set; }

        public static GetMetadataByDomainIdResult BuidSuccess(EntityMetadata[] entities)
        {
            return new GetMetadataByDomainIdResult
                       {
                           ResultCode = ResultCodes.SUCCESS,
                           Success = true,
                           ResultDescription = null,
                           Entities = entities
                       };
        }

        public static GetMetadataByDomainIdResult BuidSuccess(EntitiesMetadata entitiesMetadata)
        {
            return entitiesMetadata == null
                       ? BuidSuccess((EntityMetadata[])null)
                       : BuidSuccess(entitiesMetadata.Entities);
        }

        public static new GetMetadataByDomainIdResult BuildFail(int code, string message)
        {
            return new GetMetadataByDomainIdResult { ResultCode = code, Success = false, ResultDescription = message };
        }
    }
}
namespace RapidSoft.VTB24.BankConnector.API.Entities
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    
    using DataModels;

    [DataContract]
    public class BankOffersServiceResponse
    {
        [DataMember]
        public int TotalCount { get; set; }
        
        [DataMember]
        public List<BankOffer> Offers { get; set; }

        public BankOffersServiceResponse(int totalCount, List<BankOffer> offers)
        {
            TotalCount = totalCount;
            Offers = offers;
        }
    }
}

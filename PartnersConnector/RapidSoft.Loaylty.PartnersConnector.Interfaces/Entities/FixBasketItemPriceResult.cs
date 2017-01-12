namespace RapidSoft.Loaylty.PartnersConnector.Interfaces.Entities
{
    using System;

    public class FixBasketItemPriceResult : ResultBase
    {
        public decimal? ActualPrice
        {
            get;
            set;
        }

        // TODO: удалить, больше не используется
        public string ErrorMessage
        {
            get;
            set;
        }

        public DateTime UtcDateTime
        {
            get;
            set;
        }

        public int Confirmed
        {
            get;
            set;
        }

        public string Reason
        {
            get;
            set;
        }

        public string ReasonCode
        {
            get;
            set;
        }
    }
}
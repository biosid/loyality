using System;

namespace RapidSoft.Loaylty.PartnersConnector.Litres.DataAccess.Entities
{
    public class LitresDownloadCode
    {
        public long Id { get; set; }

        public string PartnerProductId { get; set; }

        public string Code { get; set; }

        public DateTime InsertedDate { get; set; }

        public int? OrderId { get; set; }
    }
}

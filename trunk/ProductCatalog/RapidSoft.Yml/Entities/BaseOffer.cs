using System.Collections.Generic;

namespace RapidSoft.YML.Entities
{
    public abstract class BaseOffer : IOffer
    {
        public string Id { get; set; }

        abstract public string DisplayName { get; }

        public virtual string[] Picture { get; set; }

        public virtual string Url { get; set; }

        public virtual decimal Price { get; set; }

        public string[] Categories { get; set; }

        public string Description { get; set; }

        public virtual bool? Delivery { get; set; }

        public virtual decimal? LocalDeliveryCost { get; set; }

        public string SalesNotes { get; set; }

        public bool? ManufacturerWarranty { get; set; }

        public string CountryOfOrigin { get; set; }

        public bool? Downloadable { get; set; }

        public IList<OfferParam> Params { get; set; }

        public override string ToString()
        {
            return DisplayName;
        }
    }
}
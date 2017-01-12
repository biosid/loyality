namespace RapidSoft.YML.Entities.OfferTypes
{
    /// <summary>
    /// Класс, представляющий упрощённый тип предложения
    /// </summary>
    public class SimpleOffer : BaseOffer
    {
        public string Name { get; set; }

        public string Vendor { get; set; }

        public string VendorCode { get; set; }

        public override string DisplayName
        {
            get { return Name; }
        }
    }
}
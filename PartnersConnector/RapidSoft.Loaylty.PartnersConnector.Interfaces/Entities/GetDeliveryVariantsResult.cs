namespace RapidSoft.Loaylty.PartnersConnector.Interfaces.Entities
{
    public class GetDeliveryVariantsResult : ResultBase
    {
        public VariantsLocation Location
        {
            get;
            set;
        }

        public DeliveryGroup[] DeliveryGroups
        {
            get;
            set;
        }
    }
}
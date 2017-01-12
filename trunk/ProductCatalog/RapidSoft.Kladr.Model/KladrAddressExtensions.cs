namespace RapidSoft.Kladr.Model
{
    public static class KladrAddressExtensions
    {

        public static KladrAddress GetAddressByAddressElement(this AddressElement addressElement)
        {
            var address = new KladrAddress();
            address.AddressLevel = addressElement.Level;

            switch (addressElement.Level)
            {
                case AddressLevel.Street:
                    address.Street = addressElement;
                    break;
                case AddressLevel.Town:
                    address.Town = addressElement;
                    break;
                case AddressLevel.City:
                    address.City = addressElement;
                    break;
                case AddressLevel.District:
                    address.District = addressElement;
                    break;
                case AddressLevel.Region:
                    address.Region = addressElement;
                    break;
            }

            return address;
        }

        public static AddressLevel GetAddressLevel(this KladrAddress address)
        {
            if (address.Street != null)
            {
                return AddressLevel.Street;
            }
            if (address.Town != null)
            {
                return AddressLevel.Town;
            }
            if (address.City != null)
            {
                return AddressLevel.City;
            }
            if (address.District != null)
            {
                return AddressLevel.District;
            }
            if (address.Region != null)
            {
                return AddressLevel.Region;
            }
            return 0;
        }
    }
}
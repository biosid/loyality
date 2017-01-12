using System;

namespace RapidSoft.Kladr.Model
{
    [Serializable]
    public enum AddressLevel
    {
        None = 0,
        Region = 1,
        District = 2,
        City = 3,
        Town = 4,
        Street = 5,
        House = 6,
        Flat = 7,
    }
}

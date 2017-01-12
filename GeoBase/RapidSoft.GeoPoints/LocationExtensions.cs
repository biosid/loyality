using System;
using System.Text.RegularExpressions;

using RapidSoft.GeoPoints.Entities;
using RapidSoft.Kladr.Model;

namespace RapidSoft.GeoPoints
{
    public static class LocationExtensions
    {
        public const string KladrRegex =
            @"^(?<Region>\d{2})\s?(?<District>\d{3})\s?(?<City>\d{3})\s?(?<Town>\d{3})\s?(?<Actual>\d{2})+$";

        public static AddressElement GetRegionElement(this Location location)
        {
            if (location == null)
            {
                throw new ArgumentNullException("location");
            }

            AddressElement addressElement = null;
            if (location.RegionName != null)
            {
                var kladrCode = location.GetKladrCode(AddressLevel.Region);
                addressElement = new AddressElement
                                    {
                                        Code = kladrCode,
                                        Level = AddressLevel.Region,
                                        Name = location.RegionName,
                                        Prefix = location.RegionToponym
                                    };
            }

            return addressElement;
        }

        public static AddressElement GetDistrictElement(this Location location)
        {
            if (location == null)
            {
                throw new ArgumentNullException("location");
            }

            AddressElement addressElement = null;
            if (location.DistrictName != null)
            {
                var kladrCode = location.GetKladrCode(AddressLevel.District);
                addressElement = new AddressElement
                                     {
                                         Code = kladrCode,
                                         Level = AddressLevel.District,
                                         Name = location.DistrictName,
                                         Prefix = location.DistrictToponym
                                     };
            }

            return addressElement;
        }

        public static AddressElement GetCityElement(this Location location)
        {
            if (location == null)
            {
                throw new ArgumentNullException("location");
            }

            AddressElement addressElement = null;
            if (location.CityName != null)
            {
                var kladrCode = location.GetKladrCode(AddressLevel.City);
                addressElement = new AddressElement
                                     {
                                         Code = kladrCode,
                                         Level = AddressLevel.City,
                                         Name = location.CityName,
                                         Prefix = location.CityToponym
                                     };
            }

            return addressElement;
        }

        public static AddressElement GetTownElement(this Location location)
        {
            if (location == null)
            {
                throw new ArgumentNullException("location");
            }

            AddressElement addressElement = null;
            if (location.TownName != null)
            {
                var kladrCode = location.GetKladrCode(AddressLevel.Town);
                addressElement = new AddressElement
                                     {
                                         Code = kladrCode,
                                         Level = AddressLevel.Town,
                                         Name = location.TownName,
                                         Prefix = location.TownToponym
                                     };
            }

            return addressElement;
        }

        public static KladrAddress ToKladrAddress(this Location location)
        {
            if (location == null)
            {
                throw new ArgumentNullException("location");
            }

            var retVal = new KladrAddress
                             {
                                 Region = location.GetRegionElement(),
                                 District = location.GetDistrictElement(),
                                 City = location.GetCityElement(),
                                 Town = location.GetTownElement(),
                                 AddressLevel = location.CalcAddressLevel()
                             };
            return retVal;
        }

        public static string GetKladrCode(this Location location, AddressLevel level)
        {
            if (location == null)
            {
                throw new ArgumentNullException("location");
            }

            var kladr = location.KladrCode;

            var regex = new Regex(KladrRegex);

            var m = regex.Match(kladr);

            if (!m.Success)
            {
                throw new ApplicationException("Код КЛАДР не соответствует формату");
            }

            switch (level)
            {
                case AddressLevel.Region:
                    {
                        return m.Groups["Region"].Value + new string('0', 11);
                    }

                case AddressLevel.District:
                    {
                        return m.Groups["Region"].Value + m.Groups["District"].Value + new string('0', 8);
                    }

                case AddressLevel.City:
                    {
                        return m.Groups["Region"].Value + m.Groups["District"].Value + m.Groups["City"].Value + new string('0', 5);
                    }

                case AddressLevel.Town:
                    {
                        return m.Groups["Region"].Value + m.Groups["District"].Value + m.Groups["City"].Value + m.Groups["Town"].Value + new string('0', 2);
                    }

                default:
                    {
                        throw new NotSupportedException(string.Format("Тип {0} не поддерживается", level));
                    }
            }
        }

        public static AddressLevel CalcAddressLevel(this Location location)
        {
            if (location == null)
            {
                throw new ArgumentNullException("location");
            }

            var kladr = location.KladrCode;

            var regex = new Regex(KladrRegex);

            var m = regex.Match(kladr);

            if (!m.Success)
            {
                throw new ApplicationException("Код КЛАДР не соответствует формату");
            }

            if (m.Groups["Town"].Value != "000")
            {
                return AddressLevel.Town;
            }

            if (m.Groups["City"].Value != "000")
            {
                return AddressLevel.City;
            }

            if (m.Groups["District"].Value != "000")
            {
                return AddressLevel.District;
            }

            if (m.Groups["Region"].Value != "00")
            {
                return AddressLevel.Region;
            }

            return AddressLevel.None;
        }
    }
}
using System;
using System.Globalization;
using RapidSoft.Loaders.Geocoder.Entities;
using RapidSoft.Loaders.Geocoder.Entities.Yandex;

namespace RapidSoft.Loaders.Geocoder.Service.Yandex
{
    public static class ParsingHelper
    {
        public static AddressKind ParseAddressKind(string kind)
        {
            switch (kind)
            {
                case "house":
                    return AddressKind.House;
                case "street":
                    return AddressKind.Street;
                case "metro":
                    return AddressKind.Metro;
                case "district":
                    return AddressKind.District;
                case "locality":
                    return AddressKind.City;
                case "area":
                    return AddressKind.Area;
                case "province":
                    return AddressKind.Province;
                case "country":
                    return AddressKind.Country;
                case "hydro":
                    return AddressKind.Hydro;
                case "railway":
                    return AddressKind.Railway;
                case "route":
                    return AddressKind.Route;
                case "vegetation":
                    return AddressKind.Vegetation;
                case "cemetery":
                    return AddressKind.Cemetery;
                case "bridge":
                    return AddressKind.Bridge;
                case "km":
                    return AddressKind.Km;
                case "other":
                    return AddressKind.Other;
            }

            throw new ArgumentException(String.Format("Неизвестный AddressKind: {0}", kind));
        }

        public static GeocodingPrecision ParseGeocodingPrecision(string precision)
        {
            switch (precision)
            {
                case "exact":
                    return GeocodingPrecision.Exact;
                case "number":
                    return GeocodingPrecision.Number;
                case "near":
                    return GeocodingPrecision.Near;
                case "street":
                    return GeocodingPrecision.Street;
                case "other":
                    return GeocodingPrecision.Other;
            }

            throw new ArgumentException(String.Format("Неизвестный GeocodingPrecision: {0}", precision));
        }

        public static GeoCoordinate ParsePositionString(string pos)
        {
            var parsedCoords = pos.Split(' ');

            var formatProvider = new NumberFormatInfo
                                     {
                                         NumberDecimalSeparator = "."
                                     };

            var lat = decimal.Parse(parsedCoords[1], formatProvider);
            var lng = decimal.Parse(parsedCoords[0], formatProvider);

            return new GeoCoordinate(lat, lng);
        }
    }
}
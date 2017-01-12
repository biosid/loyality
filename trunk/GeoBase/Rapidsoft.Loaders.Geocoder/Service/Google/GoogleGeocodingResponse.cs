using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using RapidSoft.Loaders.Geocoder.Entities;
using RapidSoft.Loaders.Geocoder.Entities.Google;
using RapidSoft.Loaders.Geocoder.Exceptions;

namespace RapidSoft.Loaders.Geocoder.Service.Google
{
    public class GoogleGeocodingResponse : IGeocodingResponse
    {
        private readonly StatusCode _code;
        private readonly ReadOnlyCollection<GoogleResolvedAddress> _addresses;
        private readonly string _rawResponse;

        public GoogleGeocodingResponse(StatusCode code, IList<GoogleResolvedAddress> adresses, string rawResponse)
        {
            _code = code;
            _addresses = new ReadOnlyCollection<GoogleResolvedAddress>(adresses);
            _rawResponse = rawResponse;
        }

        public GoogleGeocodingResponse(StatusCode code, IList<GoogleResolvedAddress> addresses) : this(code, addresses, null)
        {
        }

        public StatusCode StatusCode
        {
            get
            {
                return _code;
            }
        }

        public ReadOnlyCollection<GoogleResolvedAddress> Addresses
        {
            get
            {
                return _addresses;
            }
        }

        IResolvedAddress[] IGeocodingResponse.Addresses
        {
            get
            {
                return _addresses.ToArray();
            }
        }

        public string RawResponse
        {
            get
            {
                return _rawResponse;
            }
        }

        public static GoogleGeocodingResponse FromXMLString(string xml)
        {
            var resolvedAddresses = new List<GoogleResolvedAddress>();

            var doc = XDocument.Parse(xml);
            var codeCode = int.Parse(
                doc.Root.Descendants()
                    .Where(n => n.Name == "{http://earth.google.com/kml/2.0}code")
                    .First()
                    .Value);
            
            if (!Enum.IsDefined(typeof(StatusCode), codeCode))
            {
                throw new GoogleGeocodingException(String.Format("Неизвестный код статуса в ответе GoogleMaps: {0}", codeCode));
            }

            var code =  (StatusCode)codeCode;

            var adresses = doc.Root.Descendants().Where(n => n.Name == "{http://earth.google.com/kml/2.0}Placemark");

            foreach (var addrNode in adresses)
            {
                //// ReSharper disable PossibleNullReferenceException
                var accuracyCode = int.Parse(addrNode.Element("{urn:oasis:names:tc:ciq:xsdschema:xAL:2.0}AddressDetails").Attribute("Accuracy").Value);
                //// ReSharper restore PossibleNullReferenceException
                var coordsData =
                    addrNode.Descendants().Where(n => n.Name == "{http://earth.google.com/kml/2.0}coordinates").First().Value.Split(',');
                var address = addrNode.Descendants().Where(n => n.Name == "{http://earth.google.com/kml/2.0}address").First().Value.Trim('\n', ' ');

                var accuracy = (GeocodingAccuracy) accuracyCode;
                var lng = decimal.Parse(coordsData[0].Replace('.', ','));
                var lat = decimal.Parse(coordsData[1].Replace('.', ','));

                resolvedAddresses.Add(new GoogleResolvedAddress(address, new GeoCoordinate(lat, lng), accuracy));
            }

            return new GoogleGeocodingResponse(code, resolvedAddresses, xml);
        }
    }
}
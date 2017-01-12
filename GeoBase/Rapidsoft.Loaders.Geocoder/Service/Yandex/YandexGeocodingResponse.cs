using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;

namespace RapidSoft.Loaders.Geocoder.Service.Yandex
{
    public class YandexGeocodingResponse : IGeocodingResponse
    {
        private readonly string _request;
        private readonly YandexResolvedAddress[] _addresses;
        private readonly int _found;
        private readonly string _rawResponse;
        private readonly Excerpt _excerpt;

        public YandexGeocodingResponse(string request, IEnumerable<YandexResolvedAddress> addresses, int found, Excerpt resultsExcerptInfo) : this(request, addresses, found, resultsExcerptInfo, null)
        {    
        }

        public YandexGeocodingResponse(string request, IEnumerable<YandexResolvedAddress> addresses, int found, Excerpt resultsExcerptInfo, string rawResponse)
        {
            _request = request;
            _addresses = addresses.ToArray();
            _found = found;
            _excerpt = resultsExcerptInfo ?? new Excerpt(10, 0);
            _rawResponse = rawResponse;
        }

        public string Request
        {
            get
            {
                return _request;
            }
        }

        public YandexResolvedAddress[] Addresses
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
                return _addresses;
            }
        }

        public int Found
        {
            get
            {
                return _found;
            }
        }

        public Excerpt ResultsExcerpt
        {
            get
            {
                return _excerpt;
            }
        }

        public string RawResponse
        {
            get
            {
                return _rawResponse;
            }
        }

        public static YandexGeocodingResponse FromXmlString(string xml)
        {
            var root = XDocument.Parse(xml).Root;
            if (root == null)
            {
                throw new ArgumentException("Пустой xml", "xml");
            }

            var metadataNode = root.Descendants("{http://maps.yandex.ru/geocoder/1.x}GeocoderResponseMetaData").First();
            var requestNode = metadataNode.Element("{http://maps.yandex.ru/geocoder/1.x}request");
            var foundNode = metadataNode.Element("{http://maps.yandex.ru/geocoder/1.x}found");
            var resultsNode = metadataNode.Element("{http://maps.yandex.ru/geocoder/1.x}results");
            var skipNode = metadataNode.Element("{http://maps.yandex.ru/geocoder/1.x}skip");
            var addrrNodes = root.Descendants("{http://maps.yandex.ru/ymaps/1.x}GeoObject");

            var request = requestNode == null ? String.Empty : requestNode.Value;
            var found = foundNode == null ? 0 : int.Parse(foundNode.Value);
            var results = resultsNode == null ? 10 : int.Parse(resultsNode.Value);
            var skipped = skipNode == null ? 0 : int.Parse(skipNode.Value);
            var addresses = addrrNodes.Select<XElement, YandexResolvedAddress>(ParseAddress);

            return new YandexGeocodingResponse(request, addresses, found, new Excerpt(results, skipped), xml);
        }

        private static YandexResolvedAddress ParseAddress(XElement addrNode)
        {
            var metaDataNode = addrNode.Descendants().First(n => n.Name == "{http://maps.yandex.ru/geocoder/1.x}GeocoderMetaData");

            var kindNode = metaDataNode.Element("{http://maps.yandex.ru/geocoder/1.x}kind");
            var precisionNode = metaDataNode.Element("{http://maps.yandex.ru/geocoder/1.x}precision");
            var addressNode = metaDataNode.Element("{http://maps.yandex.ru/geocoder/1.x}text");
            var posNode = addrNode.Descendants("{http://www.opengis.net/gml}pos").First();

            if (addressNode == null || kindNode == null || precisionNode == null)
            {
                throw new InvalidDataException("XML не содержит необходимых элементов: text, kind или precision");
            }

            var address = addressNode.Value;
            var coord = ParsingHelper.ParsePositionString(posNode.Value);
            var kind = ParsingHelper.ParseAddressKind(kindNode.Value);
            var precision = ParsingHelper.ParseGeocodingPrecision(precisionNode.Value);

            return new YandexResolvedAddress(address, coord, kind, precision);
        }
    }

    public class Excerpt
    {
        private readonly int _size;
        private readonly int _offset;

        public Excerpt(int size) : this(size, 0)
        {
        }

        public Excerpt(int size, int offset)
        {
            _size = size;
            _offset = offset;
        }

        public int Size
        {
            get
            {
                return _size;
            }
        }

        public int Offset
        {
            get
            {
                return _offset;
            }
        }
    }
}
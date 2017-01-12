namespace RapidSoft.YML
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    using Entities;

    using Extensions;

    public class GenericOffer
    {
        private readonly string _url;
        private readonly decimal _price;
        private readonly XElement _offer;
        private string _vendor;
        private string _model;
        private string _name;
        private string _title;
        private string _included;
        private string _transport;
        private string _place;
        private DateTime? _date;

        internal GenericOffer(string url, decimal price, XElement offerNode)
        {
            _offer = offerNode;
            _url = url;
            _price = price;
        }

        #region Общие для всех предложения свойства

        public string Id
        {
            get
            {
                return _offer.Attribute("id").TryGetValue();
            }
        }

        public bool? IsAvailible
        {
            get
            {
                return _offer.Attribute("available").TryGetValue().ParseNullableBool();
            }
        }

        public string Type
        {
            get
            {
                return _offer.Attribute("type").TryGetValue();
            }
        }

        public int? Bid
        {
            get
            {
                return _offer.Attribute("bid").TryGetValue().ParseNullableInt();
            }
        }

        public int? CBid
        {
            get
            {
                return _offer.Attribute("cbid").TryGetValue().ParseNullableInt();
            }
        }

        public string Url
        {
            get
            {
                return _url;
            }
        }

        public decimal Price
        {
            get
            {
                return _price;
            }
        }

        public string CurrencyId
        {
            get
            {
                return _offer.Elements("currencyId").Single().Value;
            }
        }

        public IEnumerable<string> Categories
        {
            get
            {
                return _offer.Elements("categoryId").Where(e => !String.IsNullOrEmpty(e.Value)).Select(e => e.Value);
            }
        }

        public string[] Picture
        {
            get
            {
                var strings = _offer.Elements("picture").Select(e => e.Value).ToArray();
                return strings.Length == 0 ? null : strings;
            }
        }

        public bool? Delivery
        {
            get
            {
                return _offer.Element("delivery").TryGetValue().ParseNullableBool();
            }
        }

        public string[] Barcode
        {
            get
            {
                var strings = _offer.Elements("barcode").Select(e => e.Value).ToArray();
                return strings.Length == 0 ? null : strings;
            }
        }

        public decimal? LocalDeliveryCost
        {
            get
            {
                return _offer.Element("local_delivery_cost").TryGetValue().ParseNullableDecimal(Builder.NumberFormat);
            }
        }

        public string Description
        {
            get
            {
                return _offer.Element("description").TryGetValue();
            }
        }

        public bool? ManufacturerWarranty
        {
            get
            {
                return _offer.Element("manufacturer_warranty").TryGetValue().ParseNullableBool();
            }
        }

        public string CountryOfOrigin
        {
            get
            {
                return _offer.Element("country_of_origin").TryGetValue();
            }
        }

        public bool? Downloadable
        {
            get
            {
                return _offer.Element("downloadable").TryGetValue().ParseNullableBool();
            }
        }

        public IEnumerable<OfferParam> Params
        {
            get
            {
                var offerParams = _offer.Elements("param").Select(e => Builder.GetOfferParam(e)).ToArray();
                return offerParams.Length == 0 ? null : offerParams;
            }
        }

        public int? Weight
        {
            get
            {
                var parameters = this.Params;

                if (parameters == null)
                {
                    return null;
                }

                var paramName = YmlSettings.YmlParamWeightName;

                var weightParameter = parameters.Where(x => x.Name == paramName).ToArray();

                if (weightParameter.Length != 1)
                {
                    return null;
                }

                return weightParameter[0] == null ? (int?)null : Convert.ToInt32(weightParameter[0].Value);
            }
        }

        public decimal? BasePriceRUR
        {
            get
            {
                return _offer.Element(YmlSettings.YmlParamBasePriceName).TryGetValue().ParseNullableDecimal(Builder.NumberFormat);
            }
        }

        public bool IsDeliveredByEmail
        {
            get
            {
                var parameters = Params;
                if (parameters == null)
                {
                    return false;
                }

                var paramName = YmlSettings.YmlParamIsDeliveredByEmailName;

                var isDeliveredByEmailParameter = parameters.FirstOrDefault(x => x.Name == paramName);
                if (isDeliveredByEmailParameter == null)
                {
                    return false;
                }

                return Convert.ToBoolean(isDeliveredByEmailParameter.Value);
            }
        }

        #endregion

        #region Cвойства, зависящие от типа торгового предложения (поэтому все могут принимать значение null)

        public string TypePrefix
        {
            get
            {
                return _offer.Element("typePrefix").TryGetValue();
            }
        }

        public string Vendor
        {
            get
            {
                return _vendor ?? (_vendor = _offer.Element("vendor").TryGetValue());
            }
        }

        public string VendorCode
        {
            get
            {
                return _offer.Element("vendorCode").TryGetValue();
            }
        }

        public string Model
        {
            get
            {
                return _model ?? (_model = _offer.Element("model").TryGetValue());
            }
        }

        public string Provider
        {
            get
            {
                return _offer.Element("provider").TryGetValue();
            }
        }

        public string TarifPlan
        {
            get
            {
                return _offer.Element("tarifplan").TryGetValue();
            }
        }

        public string Author
        {
            get
            {
                return _offer.Element("author").TryGetValue();
            }
        }

        public string Name
        {
            get
            {
                return _name ?? (_name = _offer.Element("name").TryGetValue());
            }
        }

        public string Publisher
        {
            get
            {
                return _offer.Element("publisher").TryGetValue();
            }
        }

        public string Series
        {
            get
            {
                return _offer.Element("series").TryGetValue();
            }
        }

        public int? Year
        {
            get
            {
                return _offer.Element("year").TryGetValue().ParseNullableInt();
            }
        }

        public string Isbn
        {
            get
            {
                return _offer.Element("ISBN").TryGetValue();
            }
        }

        public int? Volumes
        {
            get
            {
                return _offer.Element("volume").TryGetValue().ParseNullableInt();
            }
        }

        public int? Part
        {
            get
            {
                return _offer.Element("part").TryGetValue().ParseNullableInt();
            }
        }

        public string Language
        {
            get
            {
                return _offer.Element("language").TryGetValue();
            }
        }

        public string Binding
        {
            get
            {
                return _offer.Element("binding").TryGetValue();
            }
        }

        public int? PageExtent
        {
            get
            {
                return _offer.Element("page_extent").TryGetValue().ParseNullableInt();
            }
        }

        public string TableOfContents
        {
            get
            {
                return _offer.Element("table_of_contents").TryGetValue();
            }
        }

        public string PerformedBy
        {
            get
            {
                return _offer.Element("performed_by").TryGetValue();
            }
        }

        public string PerformaceType
        {
            get
            {
                return _offer.Element("performace_type").TryGetValue();
            }
        }

        public string Storage
        {
            get
            {
                return _offer.Element("storage").TryGetValue();
            }
        }

        public string Format
        {
            get
            {
                return _offer.Element("format").TryGetValue();
            }
        }

        public string RecordingLenght
        {
            get
            {
                return _offer.Element("recording_lenght").TryGetValue();
            }
        }

        public string Artist
        {
            get
            {
                return _offer.Element("artist").TryGetValue();
            }
        }

        public string Title
        {
            get
            {
                return _title ?? (_title = _offer.Element("title").TryGetValue());
            }
        }

        public string Media
        {
            get
            {
                return _offer.Element("media").TryGetValue();
            }
        }

        public string Starring
        {
            get
            {
                return _offer.Element("starring").TryGetValue();
            }
        }

        public string Director
        {
            get
            {
                return _offer.Element("director").TryGetValue();
            }
        }

        public string OriginalName
        {
            get
            {
                return _offer.Element("originalName").TryGetValue();
            }
        }

        public string Country
        {
            get
            {
                return _offer.Element("country").TryGetValue();
            }
        }

        public string WorldRegion
        {
            get
            {
                return _offer.Element("worldRegion").TryGetValue();
            }
        }

        public string Region
        {
            get
            {
                return _offer.Element("region").TryGetValue();
            }
        }

        public int? Days
        {
            get
            {
                return _offer.Element("days").TryGetValue().ParseNullableInt();
            }
        }

        public IEnumerable<string> TourDates
        {
            get
            {
                return _offer.Elements("dataTour").Select(e => e.Value);
            }
        }

        public string HotelStars
        {
            get
            {
                return _offer.Element("hotel_stars").TryGetValue();
            }
        }

        public string Room
        {
            get
            {
                return _offer.Element("room").TryGetValue();
            }
        }

        public string Meal
        {
            get
            {
                return _offer.Element("meal").TryGetValue();
            }
        }

        public string Included
        {
            get
            {
                return _included ?? (_included = _offer.Element("included").TryGetValue());
            }
        }

        public string Transport
        {
            get
            {
                return _transport ?? (_transport = _offer.Element("transport").TryGetValue());
            }
        }

        public decimal? PriceMin
        {
            get
            {
                return _offer.Element("price_min").TryGetValue().ParseNullableDecimal(Builder.NumberFormat);
            }
        }

        public decimal? PriceMax
        {
            get
            {
                return _offer.Element("price_max").TryGetValue().ParseNullableDecimal(Builder.NumberFormat);
            }
        }

        public string Options
        {
            get
            {
                return _offer.Element("options").TryGetValue();
            }
        }

        public string Place
        {
            get
            {
                return _place ?? (_place = _offer.Element("place").TryGetValue());
            }
        }

        public Hall Hall
        {
            get
            {
                var node = _offer.Element("hall");
                return node == null ? null : Builder.GetHall(node);
            }
        }

        public string HallPart
        {
            get
            {
                return _offer.Element("hall_part").TryGetValue();
            }
        }

        public string SalesNotes
        {
            get
            {
                return _offer.Element("sales_notes").TryGetValue();
            }
        }

        public DateTime? Date
        {
            get
            {
                return _date ?? (_date = _offer.Element("date").TryGetValue().ParseNullableDateTime());
            }
        }

        public bool? IsPremiere
        {
            get
            {
                return _offer.Element("is_premiere").TryGetValue().ParseNullableBool();
            }
        }

        public bool? IsKids
        {
            get
            {
                return _offer.Element("is_kids").TryGetValue().ParseNullableBool();
            }
        }

        public bool? Store
        {
            get
            {
                return _offer.Element("store").TryGetValue().ParseNullableBool();
            }
        }

        public bool? Pickup
        {
            get
            {
                return _offer.Element("pickup").TryGetValue().ParseNullableBool();
            }
        }

        public bool? Available
        {
            get
            {
                return _offer.Attribute("available").TryGetValue().ParseNullableBool();
            }
        }

        public bool? Adult
        {
            get
            {
                return _offer.Element("adult").TryGetValue().ParseNullableBool();
            }
        }

        public string ISBN
        {
            get
            {
                return _offer.Element("sales_notes").TryGetValue();
            }
        }

        public int? Volume
        {
            get
            {
                return _offer.Element("volume").TryGetValue().ParseNullableInt();
            }
        }

        public string PerformanceType
        {
            get
            {
                return _offer.Element("performance_type").TryGetValue();
            }
        }

        public string RecordingLength
        {
            get
            {
                return _offer.Element("recording_length").TryGetValue();
            }
        }

        public string DataTour
        {
            get
            {
                return _offer.Element("dataTour").TryGetValue();
            }
        }

        public string HallPlan
        {
            get
            {
                return _offer.Element("hall").TryGetValue();
            }
        }

        #endregion

        #region Методы

        public IOffer ToConcreteOffer()
        {
            return Builder.GetConcreteOffer(this);
        }

        public XElement GetUnderlyingElement()
        {
            return _offer;
        }

        #endregion
    }
}
namespace RapidSoft.YML
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Xml.Linq;
    
    using Entities;

    using Entities.OfferTypes;

    using Extensions;

    public static class Builder
    {
        internal static readonly NumberFormatInfo NumberFormat;

        static Builder()
        {
            NumberFormat = CultureInfo.GetCultureInfo("en-US").NumberFormat;
        }

        public static GenericOffer GetGenericOffer(XElement offerNode, bool throwException = false)
        {
            var url = offerNode.Element("url").TryGetValue();
            var price = offerNode.Element("price").TryGetValue().ParseNullableDecimal(NumberFormat);
            var categoriesNodes = offerNode.Elements("categoryId");

            if (price == null || !categoriesNodes.Any())
            {
                return null;
            }

            var offer = new GenericOffer(url, price.Value, offerNode);
            return IsValid(offer, throwException) ? offer : null;
        }

        public static IOffer GetConcreteOffer(GenericOffer genericOffer)
        {
            BaseOffer offer;

            switch (genericOffer.Type)
            {
                case "vendor.model":
                    offer = new VendorModel
                    {
                        TypePrefix = genericOffer.TypePrefix,
                        Vendor = genericOffer.Vendor,
                        VendorCode = genericOffer.VendorCode,
                        Model = genericOffer.Model
                    };
                    break;
                case "book":
                    offer = new Book
                    {
                        Author = genericOffer.Author,
                        Name = genericOffer.Name,
                        Publisher = genericOffer.Publisher,
                        Series = genericOffer.Series,
                        Year = genericOffer.Year,
                        Isbn = genericOffer.Isbn,
                        Volumes = genericOffer.Volumes,
                        Part = genericOffer.Part,
                        Language = genericOffer.Language,
                        Binding = genericOffer.Binding,
                        PageExtent = genericOffer.PageExtent,
                        TableOfContents = genericOffer.TableOfContents
                    };
                    break;
                case "audiobook":
                    offer = new Audiobook
                    {
                        Author = genericOffer.Author,
                        Name = genericOffer.Name,
                        Publisher = genericOffer.Publisher,
                        Series = genericOffer.Series,
                        Year = genericOffer.Year,
                        Isbn = genericOffer.Isbn,
                        Volume = genericOffer.Volumes,
                        Part = genericOffer.Part,
                        Language = genericOffer.Language,
                        TableOfContents = genericOffer.TableOfContents,
                        PerformedBy = genericOffer.PerformedBy,
                        PerformanceType = genericOffer.PerformaceType,
                        Storage = genericOffer.Storage,
                        Format = genericOffer.Format,
                        RecordingLength = genericOffer.RecordingLenght
                    };
                    break;
                case "artist.title":
                    offer = new ArtistTitle
                    {
                        Artist = genericOffer.Artist,
                        Title = genericOffer.Title,
                        Year = genericOffer.Year,
                        Media = genericOffer.Media,
                        Starring = genericOffer.Starring,
                        Director = genericOffer.Director,
                        OriginalName = genericOffer.OriginalName,
                        Country = genericOffer.Country
                    };
                    break;
                case "tour":
                    offer = new Tour
                    {
                        WorldRegion = genericOffer.WorldRegion,
                        Country = genericOffer.Country,
                        Region = genericOffer.Region,
// ReSharper disable PossibleInvalidOperationException
                        Days = genericOffer.Days.Value,
// ReSharper restore PossibleInvalidOperationException
                        TourDates = genericOffer.TourDates.ToList(),
                        Name = genericOffer.Name,
                        HotelStars = genericOffer.HotelStars,
                        Room = genericOffer.Room,
                        Meal = genericOffer.Meal,
                        Included = genericOffer.Included,
                        Transport = genericOffer.Transport
                    };
                    break;
                case "ticket":
                case "event-ticket":
                    offer = new EventTicket
                    {
                        Name = genericOffer.Name,
                        Place = genericOffer.Place,
                        Hall = genericOffer.Hall,
                        HallPart = genericOffer.HallPart,
// ReSharper disable PossibleInvalidOperationException
                        Date = genericOffer.Date.Value,
// ReSharper restore PossibleInvalidOperationException
                        IsPremiere = genericOffer.IsPremiere,
                        IsKids = genericOffer.IsKids
                    };
                    break;
                default:
                    offer = new SimpleOffer
                    {
                        Name = genericOffer.Name,
                        Vendor = genericOffer.Vendor,
                        VendorCode = genericOffer.VendorCode

                    };
                    break;
            }

            // IYmlOffer
            offer.Id = genericOffer.Id;
            offer.Url = genericOffer.Url;
            offer.Price = genericOffer.Price;
            offer.Picture = genericOffer.Picture;
            offer.Categories = genericOffer.Categories.ToArray();
            offer.Description = genericOffer.Description;
            // BaseOffer
            offer.CountryOfOrigin = genericOffer.CountryOfOrigin;
            offer.Delivery = genericOffer.Delivery;
            offer.Downloadable = genericOffer.Downloadable;
            offer.LocalDeliveryCost = genericOffer.LocalDeliveryCost;
            offer.ManufacturerWarranty = genericOffer.ManufacturerWarranty;
            offer.Params = genericOffer.Params.ToList();
            offer.SalesNotes = genericOffer.SalesNotes;
            

            return offer;
        }

        public static OfferParam GetOfferParam(XElement paramNode)
        {
            return new OfferParam
            {
                Name = paramNode.Attributes("name").Single().Value,
                Unit = paramNode.Attribute("unit").TryGetValue(),
                Value = paramNode.Value
            };
        }

        public static Category GetCategory(XElement categoryNode)
        {
            return new Category
            {
                Id = categoryNode.Attributes("id").Single().Value,
                ParentId = categoryNode.Attribute("parentId").TryGetValue(),
                Name = categoryNode.Value
            };
        }

        public static Hall GetHall(XElement hallNode)
        {
            return new Hall
                       {
                           Name = hallNode.Value,
                           PlanUrl = hallNode.Attribute("plan").TryGetValue()
                       };
        }

        public static bool IsValid(GenericOffer offer, bool throwException = false)
        {
            if (offer == null)
            {
                throw new ArgumentNullException("offer");
            }
            
            switch (offer.Type)
            {
                case "vendor.model":
                    if (string.IsNullOrWhiteSpace(offer.Vendor) || string.IsNullOrWhiteSpace(offer.Model))
                    {
                        if (throwException)
                        {
                            var mess =
                                string.Format(
                                    "ѕредложение id=\"{0}\" с type=\"{1}\" не имеет значений дл€ атрибутов \"vendor\" или \"model\"",
                                    offer.Id,
                                    offer.Type);
                            throw new Exception(mess);
                        }

                        return false;
                    }

                    break;
                case "book":
                case "audiobook":
                    if (string.IsNullOrWhiteSpace(offer.Name))
                    {
                        if (throwException)
                        {
                            var mess =
                                string.Format(
                                    "ѕредложение id=\"{0}\" с type=\"{1}\" не имеет значений дл€ атрибута \"name\"",
                                    offer.Id,
                                    offer.Type);
                            throw new Exception(mess);
                        }

                        return false;
                    }

                    break;
                case "artist.title":
                    if (string.IsNullOrWhiteSpace(offer.Title))
                    {
                        if (throwException)
                        {
                            var mess =
                                string.Format(
                                    "ѕредложение id=\"{0}\" с type=\"{1}\" не имеет значений дл€ атрибута \"title\"",
                                    offer.Id,
                                    offer.Type);
                            throw new Exception(mess);
                        }

                        return false;
                    }

                    break;
                case "tour":
                    if (offer.Days == null || string.IsNullOrWhiteSpace(offer.Name) || string.IsNullOrWhiteSpace(offer.Included) || string.IsNullOrWhiteSpace(offer.Transport))
                    {
                        if (throwException)
                        {
                            var mess =
                                string.Format(
                                    "ѕредложение id=\"{0}\" с type=\"{1}\" не имеет значений дл€ атрибутов \"days\", \"name\", \"included\" или \"transport\"",
                                    offer.Id,
                                    offer.Type);
                            throw new Exception(mess);
                        }

                        return false;
                    }

                    break;
                case "ticket":
                case "event-ticket":
                    if (string.IsNullOrWhiteSpace(offer.Name) || string.IsNullOrWhiteSpace(offer.Place) || offer.Date == null)
                    {
                        if (throwException)
                        {
                            var mess =
                                string.Format(
                                    "ѕредложение id=\"{0}\" с type=\"{1}\" не имеет значений дл€ атрибутов \"place\", \"name\" или \"date\"",
                                    offer.Id,
                                    offer.Type);
                            throw new Exception(mess);
                        }

                        return false;
                    }

                    break;
                default:
                    if (string.IsNullOrWhiteSpace(offer.Name) &&
                        string.IsNullOrWhiteSpace(offer.Title) &&
                        (string.IsNullOrWhiteSpace(offer.Vendor) || string.IsNullOrWhiteSpace(offer.Model)))
                    {
                        if (throwException)
                        {
                            var mess =
                                string.Format(
                                    "ѕредложение id=\"{0}\" с type=\"{1}\" не имеет значений дл€ атрибутов \"name\", \"title\", \"vendor\", \"model\"",
                                    offer.Id,
                                    offer.Type);
                            throw new Exception(mess);
                        }

                        return false;
                    }
                    

                    break;
            }
            return true;
        }
    }
}
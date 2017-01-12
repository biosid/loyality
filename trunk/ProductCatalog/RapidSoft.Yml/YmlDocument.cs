using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace RapidSoft.YML
{
    using Categories;

    using Entities;

    public class YmlDocument : IDisposable
    {
        private readonly XDocument _doc;
        private CategoryTree _categoriesTree;
        private Category[] _categories;
        private XmlReader _reader;

        public YmlDocument(Stream yml)
        {
            _reader = XmlReader.Create(yml, new XmlReaderSettings { ProhibitDtd = false, XmlResolver = null });
            _doc = XDocument.Load(_reader);
        }

        public YmlDocument(string url)
        {
            _doc = XDocument.Load(url);
        }

        public DateTime CatalogDate
        {
            get
            {
                return DateTime.Parse(_doc.Descendants("yml_catalog").Single().Attributes("date").Single().Value);
            }
        }

        public IEnumerable<Category> Categories
        {
            get
            {
                return _categories ?? (_categories = _doc.Descendants("category").Select(c => Builder.GetCategory(c)).ToArray());
            }
        }

        public IEnumerable<GenericOffer> Offers
        {
            get
            {
                return _doc.Descendants("offer").Select(n => Builder.GetGenericOffer(n)).Where(o => o != null);
            }
        }

        public IEnumerable<XElement> GetInvalidOfferElements()
        {
            return _doc.Descendants("offer").Where(n => Builder.GetGenericOffer(n) == null);
        }

        public CategoryTree GetCategoryTree()
        {
            return _categoriesTree ?? (_categoriesTree = new CategoryTree(Categories));
        }

        public void Dispose()
        {
            if (_reader != null)
            {
                _reader.Close();
                _reader = null;
            }
        }
    }
}
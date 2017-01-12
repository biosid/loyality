using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

using RapidSoft.YML.Entities;

namespace RapidSoft.YML
{
    public class YmlReader
    {
        #region Constructors

        public YmlReader(string filePath)
        {
            _filePath = filePath;
        }

        #endregion

        #region Fields

        private readonly string _filePath;

        #endregion

        #region Properties

        public IEnumerable<GenericOffer> Offers
        {
            get
            {
                using (var reader = CreateReader())
                {
                    while (reader.Read())
                    {
                        if (reader.Name == "offer" && reader.NodeType == XmlNodeType.Element)
                        {
                            yield return ReadOffer(reader);
                        }
                    }
                }
            }
        }

        public IEnumerable<Category> Categories
        {
            get
            {
                using (var reader = CreateReader())
                {
                    while (reader.Read())
                    {
                        if (reader.Name == "category" && reader.NodeType == XmlNodeType.Element)
                        {
                            yield return ReadCategory(reader);
                        }
                    }
                }
            }
        }

        #endregion

        #region Methods

        public IEnumerable<IEnumerable<Category>> ReadCategoriesBatches(int size)
        {
            using (var reader = this.CreateReader())
            {
                do
                {
                    var batch = this.ReadCategoryBatch(reader, size);
                    if (batch.Count > 0)
                    {
                        yield return batch;
                    }
                }
                while (reader.ReadState == ReadState.Interactive && !this.IsEndOfCategories(reader));
            }
        }

        public IEnumerable<IEnumerable<GenericOffer>> ReadOffersBatches(int size, bool throwException = false)
        {
            using (var reader = this.CreateReader())
            {
                do
                {
                    var batch = this.ReadOfferBatch(reader, size, throwException);
                    if (batch.Count > 0)
                    {
                        yield return batch;
                    }
                }
                while (reader.ReadState == ReadState.Interactive && !this.IsEndOfOffers(reader));
            }
        }

        private bool IsEndOfCategories(XmlReader reader)
        {
            return reader.NodeType == XmlNodeType.EndElement && reader.Name == "categories";
        }

        private bool IsEndOfOffers(XmlReader reader)
        {
            return reader.NodeType == XmlNodeType.EndElement && reader.Name == "offers";
        }

        private IList<Category> ReadCategoryBatch(XmlReader reader, int size)
        {
            var batch = new List<Category>(size);
            while (batch.Count < size)
            {
                if (!reader.Read())
                {
                    break;
                }

                if (this.IsEndOfCategories(reader))
                {
                    break;
                }

                if (reader.Name == "category" && reader.NodeType == XmlNodeType.Element)
                {
                    var category = this.ReadCategory(reader);
                    batch.Add(category);
                }
            }

            return batch;
        }

        private IList<GenericOffer> ReadOfferBatch(XmlReader reader, int size, bool throwException = false)
        {
            var batch = new List<GenericOffer>(size);
            while (batch.Count < size)
            {
                if (!reader.Read())
                {
                    break;
                }

                if (this.IsEndOfOffers(reader))
                {
                    break;
                }

                if (reader.Name == "offer" && reader.NodeType == XmlNodeType.Element)
                {
                    var category = this.ReadOffer(reader, throwException);
                    batch.Add(category);
                }
            }

            return batch;
        }

        private Category ReadCategory(XmlReader reader)
        {
            var node = XNode.ReadFrom(reader);
            return Builder.GetCategory((XElement) node);
        }

        private GenericOffer ReadOffer(XmlReader reader, bool throwException = false)
        {
            var node = XNode.ReadFrom(reader);
            return Builder.GetGenericOffer((XElement)node, throwException);
        }

        private XmlReader CreateReader()
        {
            return XmlReader.Create(_filePath, new XmlReaderSettings
            {
                ProhibitDtd = false,
                XmlResolver = null
            });
        }

        #endregion
    }
}
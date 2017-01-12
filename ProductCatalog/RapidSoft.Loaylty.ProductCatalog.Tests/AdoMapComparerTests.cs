using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RapidSoft.Loaylty.ProductCatalog.Tests.Import
{
    using API.Entities;

    using Extensions;

    using ProductCatalog.DataSources;

    [TestClass]
    public class AdoMapComparerTests
    {
        private readonly AdoMapperColumn[] testProductColumns = new[]
            {
                NewColumn("ProductId", typeof(string), "[ProductId] [nvarchar](256) NOT NULL,", 256, true, isSystem: true), 
                NewColumn("PartnerId", typeof(int), "[PartnerId] [int] NOT NULL,", isSystem: true),
                NewColumn("Pictures", typeof(string), "[Pictures] [xml] NULL,", null, true, true, XmlSerializer.Serialize, a => XmlSerializer.Deserialize<string[]>((string)a)),
                NewColumn("Bid", typeof(int), "[Bid] [int] NULL,"), 
                NewColumn("SalesNotes", typeof(string), "[SalesNotes] [nvarchar](50) NULL,", 50)
            };

        [TestMethod]
        public void ShouldCompareProducts()
        {
            var existsProd = Tests.TestHelper.BuildTestProduct();

            var newProd = Tests.TestHelper.BuildTestProduct();

            AdoMapComparer<Product> comparer = new AdoMapComparer<Product>(testProductColumns, new[] { "Pictures" });

            var isEqual = comparer.Equals(existsProd, newProd);
            Assert.AreEqual(true, isEqual);

            newProd.PartnerId = -105;

            isEqual = comparer.Equals(existsProd, newProd);
            Assert.AreEqual(true, isEqual, "Так как PartnerId системное поле, то по нему НЕ проверяем экиввалентность");

            newProd.Pictures = new[]
                               {
                                   "раз", "два"
                               };

            isEqual = comparer.Equals(existsProd, newProd);
            Assert.AreEqual(true, isEqual, "Так как Pictures указано как skip поле, то по нему НЕ проверяем экиввалентность");

            newProd.SalesNotes = newProd.SalesNotes.GetFirst(50);

            isEqual = comparer.Equals(existsProd, newProd);
            Assert.AreEqual(true, isEqual, "Перед сравнением поля должны обрзаться в соответствии с их длинной");

            newProd.Bid = newProd.Bid - 500;

            isEqual = comparer.Equals(existsProd, newProd);
            Assert.AreEqual(false, isEqual, "Так как Bid НЕ системное и не skip поле, то по нему проверяем экиввалентность");
        }

        private static AdoMapperColumn NewColumn(
            string columnName,
            Type dotNetType,
            string columnSqlDeclare,
            int? columnLen = null,
            bool? isInsert = null,
            bool? isSelect = null,
            Func<object, object> objToDBMapFunc = null,
            Func<object, object> databaseToObjMapFunc = null,
            bool isSystem = false)
        {
            return new AdoMapperColumn
            {
                ColumnName = columnName,
                DotNetType = dotNetType,
                ColumnSqlDeclare = columnSqlDeclare,
                ColumnLen = columnLen,
                IsInsert = isInsert,
                IsSelect = isSelect,
                ObjToDBMapFunc = objToDBMapFunc,
                DBToObjMapFunc = databaseToObjMapFunc,
                IsSystem = isSystem
            };
        }

    }
}

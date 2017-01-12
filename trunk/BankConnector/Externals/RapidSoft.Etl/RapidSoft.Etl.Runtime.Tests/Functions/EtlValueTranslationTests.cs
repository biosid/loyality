using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using RapidSoft.Etl.Runtime.Functions;

namespace RapidSoft.Etl.Runtime.Tests.Functions
{
    [TestClass]
    public class EtlValueTranslationTests
    {
        #region Initialization

        [TestInitialize]
        public void Initialize()
        {
        }

        #endregion

        #region Tests

        [TestMethod()]
        public void CanTranslateWithNestedFunctions()
        {
            var record = new FakeDataRecord(new Dictionary<string, object>
            {
                {"Id", new Guid("506A78BC-9AB9-4DB9-9371-08334060F5C9")}, 
                {"Null", null},
                {"Boolean", true},
                {"Byte", 255},
                {"DateTime", DateTime.Parse("2010-01-15")},
                {"Decimal", Decimal.Parse("79228162514264337593543950335")},
                {"Double", 100000000.1},
                {"Guid", new Guid("00000000-0000-0000-0000-000000000000")},
                {"Int16", 32767},
                {"Int32", 2147483638},
                {"Int64", 9223372036854775807},
                {"Single", 10000000},
                {"String", "Name:A"},
            });

            var translation = new EtlValueTranslation
            (
                new EtlValueFunction[]
                {
                    new EtlParseFunction(@".*:(?<g>\w)+", "g"),
                    new EtlDecodeFunction
                    (
                        new EtlDecodeRule(@"A", "Name is A"),
                        new EtlDecodeRule(@"B", "Name is B")
                    ),
                    new EtlConcatenateFunction(null, null, ", "),
                    new EtlConcatenateFunction("Byte", null, null),
                    new EtlConcatenateFunction(null, null, ", "),
                    new EtlConcatenateFunction
                    (
                        "Id", 
                         new EtlValueTranslation
                         (
                            new EtlReplaceFunction("-", "_")
                         ),
                         null
                    )
                }
            );

            var expectedResult = "Name is A, 255, 506a78bc_9ab9_4db9_9371_08334060f5c9";
            var result = EtlValueTranslation.Evaluate("String", translation, record);
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult, result);
        }

        #endregion
    }
}

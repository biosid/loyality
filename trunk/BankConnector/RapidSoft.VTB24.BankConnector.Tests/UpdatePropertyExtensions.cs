using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RapidSoft.VTB24.BankConnector.Tests
{
    using System.Linq;

    using RapidSoft.VTB24.BankConnector.API.Entities;
    using RapidSoft.VTB24.BankConnector.DataModels;

    [TestClass]
    public class UpdatePropertyExtensions
    {
        [TestMethod]
        public void ShouldPack()
        {
            var pros = new[] { UpdateProperty.Email, UpdateProperty.LastName };

            var retVal = pros.Pack();

            Assert.AreEqual((int)UpdateProperty.Email + (int)UpdateProperty.LastName, retVal);

            pros = new[] { UpdateProperty.Email, UpdateProperty.LastName, UpdateProperty.Email };

            retVal = pros.Pack();

            Assert.AreEqual((int)UpdateProperty.Email + (int)UpdateProperty.LastName, retVal);

            pros = Enum.GetValues(typeof(UpdateProperty)).Cast<int>().Select(x => (UpdateProperty)x).ToArray();

            retVal = pros.Pack();

            Assert.AreEqual(511, retVal);
        }

        [TestMethod]
        public void ShouldUnpack()
        {
            var t = new[] { UpdateProperty.Email, UpdateProperty.LastName }.Pack();

            var retVal = t.Unpack().ToArray();

            Assert.IsTrue(retVal.Any(x => x == UpdateProperty.Email));
            Assert.IsTrue(retVal.Any(x => x == UpdateProperty.LastName));

            retVal = 255.Unpack().ToArray();
            Assert.IsTrue(retVal.Count() == 8);
        }
    }
}

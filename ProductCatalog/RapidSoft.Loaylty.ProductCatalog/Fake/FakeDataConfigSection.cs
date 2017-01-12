namespace RapidSoft.Loaylty.ProductCatalog.Fake
{
    using System.Configuration;

    public class FakeDataConfigSection : ConfigurationSection
    {
        public static readonly FakeDataConfigSection Current =
        (FakeDataConfigSection)ConfigurationManager.GetSection("fakeData");

        [ConfigurationProperty("priceMechanic")]
        public FakeMechanic PriceMechanic
        {
            get { return (FakeMechanic)base["priceMechanic"]; }
            set { base["priceMechanic"] = value; }
        }

        [ConfigurationProperty("calcSingleValue")]
        public FakeCalcSingleValMechanic CalcSingleValue
        {
            get { return (FakeCalcSingleValMechanic)base["calcSingleValue"]; }
            set { base["calcSingleValue"] = value; }
        }

        [ConfigurationProperty("checkOrderResult")]
        public FakeCheckOrderResult CheckOrderResult
        {
            get { return (FakeCheckOrderResult)base["checkOrderResult"]; }
            set { base["checkOrderResult"] = value; }
        }

        [ConfigurationProperty("commitOrdersAsyncResult")]
        public FakeCommitOrdersResult CommitOrdersAsyncResult
        {
            get { return (FakeCommitOrdersResult)base["commitOrdersAsyncResult"]; }
            set { base["commitOrdersAsyncResult"] = value; }
        }

        [ConfigurationProperty("fixBasketItemPrice")]
        public FakeFixBasketItemPriceResult FixBasketItemPrice
        {
            get { return (FakeFixBasketItemPriceResult)base["fixBasketItemPrice"]; }
            set { base["fixBasketItemPrice"] = value; }
        }

        [ConfigurationProperty("fakeGeopoints")]
        public FakeGeopoints FakeGeopoints
        {
            get { return (FakeGeopoints)base["fakeGeopoints"]; }
            set { base["fakeGeopoints"] = value; }
        }
    }
}
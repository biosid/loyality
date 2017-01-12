namespace RapidSoft.VTB24.BankConnector.Acquiring
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;

    public class UnitellerAuthInfo
    {
        public string Password
        {
            get;
            set;
        }

        public string ShopId
        {
            get;
            set;
        }

        public string OrderId
        {
            get;
            set;
        }

        public string CustomerId
        {
            get;
            set;
        }

        public decimal Subtotal
        {
            get;
            set;
        }

        public string SubtotalString 
        {
            get
            {
                return Math.Round(Subtotal, 2, MidpointRounding.AwayFromZero).ToString(CultureInfo.CreateSpecificCulture("en-US"));
            }
        }

        public string ReturnUrl
        {
            get;
            set;
        }

        public void Validate()
        {
            Contract.Assert(this.ShopId != null);
            Contract.Assert(this.OrderId != null);
            Contract.Assert(this.CustomerId != null);
            Contract.Assert(this.Password != null);
            Contract.Assert(this.Subtotal >= 0);
            Contract.Assert(!string.IsNullOrEmpty(this.ReturnUrl));
        }
    }
}

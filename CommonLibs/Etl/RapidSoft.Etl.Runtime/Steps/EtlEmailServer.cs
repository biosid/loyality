namespace RapidSoft.Etl.Runtime.Steps
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Net;

    [Serializable]
    [TypeConverter(typeof(EtlEmailServerObjectConverter))]
    public sealed class EtlEmailServer
    {
        #region Properties

        public string Host { get; set; }

        public string Port { get; set; }

        public string UseSSL { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
        
        #endregion

        public void ThrowIfInvalid()
        {
            if (string.IsNullOrEmpty(this.Host))
            {
                throw new ArgumentException("Property \"Host\" cannot be null or empty");
            }

            if (this.IntPort < IPEndPoint.MinPort || this.IntPort > IPEndPoint.MaxPort)
            {
                var errorMessage = string.Format("Property \"Port\" should be greater then {0} and less then {1}", IPEndPoint.MinPort, IPEndPoint.MaxPort);
                throw new ArgumentException(errorMessage);
            }

            if (string.IsNullOrEmpty(this.UserName))
            {
                throw new ArgumentException("Property \"UserName\" cannot be null or empty");
            }

            if (string.IsNullOrEmpty(this.Password))
            {
                throw new ArgumentException("Property \"Password\" cannot be null or empty", "step");
            }
        }

        public int IntPort
        {
            get
            {
                int res;
                int.TryParse(Port, out res);
                return res;
            }
        }

        public bool BoolUseSSL
        {
            get
            {
                if (string.IsNullOrEmpty(UseSSL))
                {
                    return false;
                }

                return this.ParseBool(this.UseSSL);
            }
        }

        private bool ParseBool(string val)
        {
            bool res;
            bool.TryParse(val, out res);
            return res;
        }

        #region Nested classes

        public sealed class EtlEmailServerObjectConverter : ExpandableObjectConverter
        {
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                if (sourceType == typeof(string))
                {
                    return true;
                }

                return false;
            }

            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                if (destinationType == typeof(string))
                {
                    return true;
                }

                return false;
            }

            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                if (destinationType == typeof(string))
                {
                    var obj = (EtlEmailServer)value;

                    if (obj != null)
                    {
                        return obj.Host;
                    }
                    else
                    {
                        return null;
                    }
                }

                return null;
            }

            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                if (value == null)
                {
                    return null;
                }

                var str = value.ToString();
                if (string.IsNullOrEmpty(str))
                {
                    return null;
                }

                return new EtlEmailServer { Host = str };
            }
        }

        #endregion
    }
}
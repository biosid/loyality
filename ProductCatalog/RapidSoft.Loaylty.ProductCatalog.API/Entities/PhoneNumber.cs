using System;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;

namespace RapidSoft.Loaylty.ProductCatalog.API.Entities
{
    [DataContract]
    [Serializable]
    public class PhoneNumber : ICloneable, IEquatable<PhoneNumber>
    {
        private static readonly Regex DigitsRx = new Regex("^\\d+$", RegexOptions.Multiline);

        public PhoneNumber()
        {
            
        }

        private PhoneNumber(PhoneNumber phoneNumber)
        {
            this.LocalNumber = phoneNumber.LocalNumber;
            this.CityCode = phoneNumber.CityCode;
            this.CountryCode = phoneNumber.CountryCode;
        }

        [DataMember]
        public string LocalNumber
        {
            get;
            set;
        }

        [DataMember]
        public string CityCode
        {
            get;
            set;
        }

        [DataMember]
        public string CountryCode
        {
            get;
            set;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(this.CountryCode))
            {
                sb.AppendFormat("+{0}", this.CountryCode);
            }
            if (!string.IsNullOrWhiteSpace(this.CountryCode) && !string.IsNullOrWhiteSpace(this.CityCode))
            {
                sb.Append(" ");
            }
            if (!string.IsNullOrWhiteSpace(this.CityCode))
            {
                sb.AppendFormat("({0})", this.CityCode);
            }
            if (!string.IsNullOrWhiteSpace(this.CityCode) && !string.IsNullOrWhiteSpace(this.LocalNumber))
            {
                sb.Append(" ");
            }
            if (!string.IsNullOrWhiteSpace(this.LocalNumber))
            {
                sb.Append(this.LocalNumber);
            }
            return sb.ToString();
        }


        public string GetSimpleFormatString(
            bool writeCountryCode = true,
            bool writeCityCode = true,
            bool writeLocalNumber = true)
        {
            var sb = new StringBuilder();
            if (writeCountryCode && !string.IsNullOrWhiteSpace(this.CountryCode))
            {
                sb.AppendFormat("{0}", this.CountryCode);
            }
            if (writeCityCode && !string.IsNullOrWhiteSpace(this.CountryCode) && !string.IsNullOrWhiteSpace(this.CityCode))
            {
                sb.Append("");
            }
            if (writeCityCode && !string.IsNullOrWhiteSpace(this.CityCode))
            {
                sb.AppendFormat("{0}", this.CityCode);
            }
            if (writeCityCode && !string.IsNullOrWhiteSpace(this.CityCode) && !string.IsNullOrWhiteSpace(this.LocalNumber))
            {
                sb.Append("");
            }
            if (writeLocalNumber && !string.IsNullOrWhiteSpace(this.LocalNumber))
            {
                sb.Append(this.LocalNumber);
            }
            return sb.ToString();
        }

        public object Clone()
        {
            return new PhoneNumber(this);
        }

        public bool IsWellFormed()
        {
            var isIncomplete = string.IsNullOrEmpty(this.LocalNumber) || string.IsNullOrEmpty(this.CityCode) || string.IsNullOrEmpty(this.CountryCode);
            
            if (isIncomplete)
            {
                return false;
            }

            return Regex.IsMatch(CountryCode, @"^[+-]?\d+$") && DigitsRx.IsMatch(this.CityCode) && DigitsRx.IsMatch(this.LocalNumber);
        }

        public bool IsEmpty()
        {
            return string.IsNullOrWhiteSpace(this.CountryCode)
                && string.IsNullOrWhiteSpace(this.CityCode)
                && string.IsNullOrWhiteSpace(this.LocalNumber);
        }

        public bool Equals(PhoneNumber other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return Equals(other.LocalNumber, this.LocalNumber) && Equals(other.CityCode, this.CityCode) && Equals(other.CountryCode, this.CountryCode);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != typeof (PhoneNumber))
            {
                return false;
            }
            return this.Equals((PhoneNumber) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = (this.LocalNumber != null ? this.LocalNumber.GetHashCode() : 0);
                result = (result*397) ^ (this.CityCode != null ? this.CityCode.GetHashCode() : 0);
                result = (result*397) ^ (this.CountryCode != null ? this.CountryCode.GetHashCode() : 0);
                return result;
            }
        }

        public static bool operator ==(PhoneNumber left, PhoneNumber right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(PhoneNumber left, PhoneNumber right)
        {
            return !Equals(left, right);
        }
    }
}

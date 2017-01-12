using System;
using System.Text;

namespace RapidSoft.Kladr.Model
{
    public static class AddressStringConverter
    {
        #region Constants

        private const char DELIMITER = '#';

        #endregion

        #region Methods

        public static string GetString(AddressElement addressElement)
        {
            if (addressElement == null)
            {
                return null;
            }
            return string.Concat(addressElement.Code, DELIMITER, addressElement.Level, DELIMITER, addressElement.Name, DELIMITER, addressElement.Prefix);
        }

        public static AddressElement Parse(string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException();
            }

            var strs = str.Split(DELIMITER);
            if (strs.Length != 4)
            {
                throw new FormatException("Input string was not in a correct format.");
            }

            return new AddressElement
            {
                Code = strs[0],
                Level = (AddressLevel)Enum.Parse(typeof(AddressLevel), strs[1]),
                Name = strs[2],
                Prefix = strs[3]
            };
        }

		public static string GetAddressText(KladrAddress address)
		{
			return GetAddressText(address, AddressLevel.Flat);
		}

        /// <summary>
        /// Returns the address string representation for the address specified.
        /// </summary>
        /// <param name="address">Point to get address.</param>
        /// <param name="maxAccuracyLevel">Maximum level of address accuracy.</param>
        public static string GetAddressText(KladrAddress address, AddressLevel maxAccuracyLevel)
        {
            if (address == null)
            {
                return "";
            }

            var text = new StringBuilder();
            var empty = true;

            foreach (var elem in address.GetNotNullElements(maxAccuracyLevel))
            {
                var elemText = GetAddressText(elem, true, true);

                if (!string.IsNullOrEmpty(elemText))
                {
                    if (!empty)
                    {
                        text.Append(", ");
                    }

                    text.Append(elemText);
                    empty = false;
                }
            }

            return text.ToString();
        }

		/// <summary>
        /// Returns the address string representation for the address specified.
        /// </summary>
        /// <param name="addressElement">Address element format to.</param>
        /// <param name="isPrefixDotNeeded">Indicates if need place dot after prefix.</param>
        /// <param name="isPrefixBeforeName">Indicates if element prefix follows before element name.</param>
        public static string GetAddressText(AddressElement addressElement, bool isPrefixDotNeeded = true, bool isPrefixBeforeName = false)
        {
            if (addressElement == null)
            {
                return "";
            }

			if (addressElement.Level == AddressLevel.House)
			{
				return GetHouseText((House) addressElement);
			}

			if (addressElement.Level == AddressLevel.Flat)
        	{
				return GetFlatText((Flat) addressElement);
        	}

            var format = "{0}{1}{2}{3}";

            if (!isPrefixBeforeName)
            {
                format = "{3}{2}{0}{1}";
            }
			
            var prefix = NormalizePrefixCase(addressElement.Prefix);
            var dot = isPrefixDotNeeded ? NormalizePrefixDot(addressElement.Prefix) : "";
            var name = addressElement.Name;

            return String.Format
            (
                format,
                prefix,
                dot,
                string.IsNullOrEmpty(prefix) ? "" : " ",
                name
            );
        }

    	private static string GetHouseText(House house)
    	{
			var sb = new StringBuilder();
			var needComma = false;
			if (!string.IsNullOrEmpty(house.HouseNumberBase))
    		{
				sb.Append("д. ");
				sb.Append(house.HouseNumberBase);
    			needComma = true;
			}

			if (!string.IsNullOrEmpty(house.HouseFraction))
			{
				sb.Append("/");
				sb.Append(house.HouseFraction);
				needComma = true;
            }

			if (!string.IsNullOrEmpty(house.HouseBlock))
			{
				if (needComma)
				{
					sb.Append(", ");
				}
				sb.Append("корп. ");
				sb.Append(house.HouseBlock);
				needComma = true;
            }

			if (!string.IsNullOrEmpty(house.HouseBuilding))
			{
				if (needComma)
				{
					sb.Append(", ");
				}
				sb.Append("стр. ");
				sb.Append(house.HouseBuilding);
            }

			return sb.ToString();
    	}

    	private static string GetFlatText(Flat flat)
    	{
			var sb = new StringBuilder();
			var needComma = false;
			if (flat.HouseEntrance.HasValue)
			{	
				sb.Append("подъезд ");
                sb.Append(flat.HouseEntrance);
				needComma = true;
			}

			if (flat.Floor.HasValue)
			{
				if (needComma)
				{
					sb.Append(", ");
				}
				sb.Append("этаж ");
				sb.Append(flat.Floor);
				needComma = true;
			}

			if (!string.IsNullOrEmpty(flat.FlatNumber))
			{
				if (needComma)
				{
					sb.Append(", ");
				}
				sb.Append("кв. ");
				sb.Append(flat.FlatNumber);
			}

			return sb.ToString();
    	}

        private static string NormalizePrefixDot(string prefix)
        {
            if (string.IsNullOrEmpty(prefix))
            {
                return "";
            }

            if (prefix.EndsWith("."))
            {
                return "";
            }

            if (prefix == "край")
            {
                return "";
            }

            if (prefix.Length > 5 || prefix.IndexOf('-') != -1)
            {
                return "";
            }

            var upperPrefix = prefix.ToUpper();

            if (upperPrefix.Equals(prefix))
            {
                return "";
            }

            return ".";
        }

        private static string NormalizePrefixCase(string prefix)
        {
            if (string.IsNullOrEmpty(prefix))
            {
                return "";
            }

            if (prefix.Length > 1)
            {
                if (char.IsUpper(prefix[0]) && char.IsLower(prefix[1]))
                {
                    return prefix.ToLower();
                }
                else
                {
                    return prefix;
                }
            }

            return prefix;
        }

        #endregion
    }
}

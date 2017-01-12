namespace RapidSoft.Etl.Runtime.Steps
{
	using System;
	using System.ComponentModel;
	using System.Globalization;

    [Serializable]
	[TypeConverter(typeof(EtlReceiveMailFilterObjectConverter))]
	public sealed class EtlEmailReceiveFilter
	{
        // TODO: Может сделать регулярку?
		public string From { get; set; }
		
		// TODO: Может сделать регулярку? 
		public string SubjectContains { get; set; }

		public string SubjectStartsWith { get; set; }

		#region Nested classes

		public sealed class EtlReceiveMailFilterObjectConverter : ExpandableObjectConverter
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
					var obj = (EtlEmailReceiveFilter)value;

					if (obj != null)
					{
						return obj.From;
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

				return new EtlEmailReceiveFilter
				{
					From = str,
				};
			}
		}

		#endregion
	}
}

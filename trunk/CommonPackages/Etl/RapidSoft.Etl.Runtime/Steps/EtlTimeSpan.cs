using System;
using System.Collections.Generic;
using System.Text;

namespace RapidSoft.Etl.Runtime.Steps
{
	using System.ComponentModel;
	using System.Globalization;

	[Serializable]
	[TypeConverter(typeof(EtlTimeSpan.EtlTimeSpanObjectConverter))]
	public sealed class EtlTimeSpan
	{
		#region Constants

		private const EtlTimeSpanType DefaultType = EtlTimeSpanType.Days;

		private const int DefaultValue = 1;

		#endregion

		#region Constructors

		public EtlTimeSpan(EtlTimeSpanType timeSpanType, int value)
			: this()
		{
			this.TimeSpanType = timeSpanType;
			this.Value = value;
		}

		public EtlTimeSpan()
		{
			this.TimeSpanType = DefaultType;
			this.Value = DefaultValue;
		}

		#endregion

		#region Properties

		public EtlTimeSpanType TimeSpanType
		{
			get;
			set;
		}

		public int Value
		{
			get;
			set;
		}

		#endregion

		#region Methods

		public static TimeSpan ToSystemTimeSpan(EtlTimeSpan etlTimeSpan)
		{
			switch (etlTimeSpan.TimeSpanType)
			{
				case EtlTimeSpanType.Days:
					return new TimeSpan(etlTimeSpan.Value, 0, 0, 0);
				case EtlTimeSpanType.Hours:
					return new TimeSpan(0, etlTimeSpan.Value, 0, 0);
				default:
					throw new InvalidOperationException(string.Format("Unknown value \"{0}\" of type {1}", etlTimeSpan.Value, etlTimeSpan.GetType().FullName));
			}
		}

		#endregion

		#region Nested classes

		public sealed class EtlTimeSpanObjectConverter : ExpandableObjectConverter
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
					var obj = (EtlTimeSpan)value;

					if (obj != null)
					{
						return obj.Value.ToString();
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

				int v;
				if (!int.TryParse(str, out v))
				{
					return null;
				}

				return new EtlTimeSpan
				{
					Value = v
				};
			}
		}

		#endregion

	}
}

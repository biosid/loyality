namespace RapidSoft.Loaylty.Logging.Wcf
{
    using System;
    using System.Configuration;

    using RapidSoft.Extensions;

    public abstract class ParameterSerializer
    {
        public static ParameterSerializer Build()
        {
            var serializeType = ConfigurationManager.AppSettings["ParamLogSerializer"] ?? string.Empty;

            switch (serializeType)
            {
                case "JSON":
                    return new JsonParameterSerializer();
                case "XML":
                    return new XmlParameterSerializer();
                default:
                    return new ToStringParameterSerializer();
            }
        }

        public abstract string Serialize<T>(T param);

        internal class JsonParameterSerializer : ParameterSerializer
        {
            public override string Serialize<T>(T param)
            {
                if (param == null)
                {
                    return "null";
                }

                string retVal;
                try
                {
                    retVal = param.JsonSerialize();
                }
                catch (Exception ex)
                {
                    retVal = "Ошибка сериализации параметра: " + ex.StackTrace;
                }

                return retVal;
            }
        }

        internal class XmlParameterSerializer : ParameterSerializer
        {
            public override string Serialize<T>(T param)
            {
                if (param == null)
                {
                    return "null";
                }

                var retVal = param.Serialize();

                return retVal;
            }
        }

        internal class ToStringParameterSerializer : ParameterSerializer
        {
            public override string Serialize<T>(T param)
            {
                if (param == null)
                {
                    return "null";
                }

                var retVal = param.ToString();

                return retVal;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace RapidSoft.Loaders.KLADR.Exceptions
{
    public class ConfigurationException : ApplicationException
    {
        public ConfigurationException()
        {}

        public ConfigurationException(string message) : base(message)
        {}

        public ConfigurationException(string message, Exception innerException) : base(message, innerException)
        {}

        protected ConfigurationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {}
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace RapidSoft.Loaders.KLADR.Exceptions
{
    public class DownloadException : Exception
    {
        public DownloadException()
        {}

        public DownloadException(string message) : base(message)
        {}

        public DownloadException(string message, Exception innerException) : base(message, innerException)
        {}

        protected DownloadException(SerializationInfo info, StreamingContext context) : base(info, context)
        {}
    }
}

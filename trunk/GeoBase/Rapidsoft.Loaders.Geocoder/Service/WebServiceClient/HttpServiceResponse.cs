using System;
using System.IO;
using System.Net;

namespace RapidSoft.Loaders.Geocoder.Service.WebServiceClient
{
    public class HttpServiceResponse : IHttpServiceResponse
    {
        private readonly HttpWebResponse _response;

        public HttpStatusCode StatusCode
        {
            get
            {
                return _response.StatusCode;
            }
        }

        public string StatusDescription
        {
            get
            {
                return _response.StatusDescription;
            }
        }

        public Uri ResponseUri
        {
            get
            {
                return _response.ResponseUri;
            }
        }

        public string ContentEncoding
        {
            get
            {
                return _response.ContentEncoding;
            }
        }

        public long ContentLength
        {
            get
            {
                return _response.ContentLength;
            }
        }

        public string ContentType
        {
            get
            {
                return _response.ContentType;
            }
        }

        public CookieCollection Cookies
        {
            get
            {
                return _response.Cookies;
            }
        }

        public WebHeaderCollection Headers
        {
            get
            {
                return _response.Headers;
            }
        }

        public string CharacterSet
        {
            get
            {
                return _response.CharacterSet;
            }
        }

        public HttpServiceResponse(HttpWebResponse response)
        {
            _response = response;
        }

        public Stream GetResponseStream()
        {
            return _response.GetResponseStream();
        }

        public string GetResponseString()
        {
            using(var stream = GetResponseStream())
            {
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
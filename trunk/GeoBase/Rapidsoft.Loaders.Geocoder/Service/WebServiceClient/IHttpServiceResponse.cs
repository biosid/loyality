using System;
using System.IO;
using System.Net;

namespace RapidSoft.Loaders.Geocoder.Service.WebServiceClient
{
    public interface IHttpServiceResponse {
        HttpStatusCode StatusCode { get; }
        string StatusDescription { get; }
        Uri ResponseUri { get; }
        string ContentEncoding { get; }
        long ContentLength { get; }
        string ContentType { get; }
        CookieCollection Cookies { get; }
        WebHeaderCollection Headers { get; }
        string CharacterSet { get; }
        Stream GetResponseStream();
        string GetResponseString();
    }
}
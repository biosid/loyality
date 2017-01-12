using System;
using System.Collections.Specialized;

namespace RapidSoft.Loaders.Geocoder.Service.WebServiceClient
{
    public class UrlBuilder
    {
        #region Private fields

        private readonly QueryString _qs = new QueryString();
        private readonly UriBuilder _ub;
        private bool _isRelative;

        #endregion

        #region Constructors

        public UrlBuilder()
        {
            _ub = new UriBuilder();
        }

        public UrlBuilder(string url)
        {
            if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                _ub = new UriBuilder(url);
            }
            else
            {
                // временно превращаем url в абсолютный
                _isRelative = true;
                _ub = new UriBuilder("http://localhost" + url);
            }

            _qs.Add(_ub.Query);
        }

        public UrlBuilder(string url, NameValueCollection queryString)
            : this(url)
        {
            _qs = new QueryString(queryString);
        }

        public UrlBuilder(Uri uri)
        {
            _ub = new UriBuilder(uri);
            _qs.Add(uri.Query);
        }

        public UrlBuilder(string scheme, string host)
        {
            _ub = new UriBuilder(scheme, host);
        }

        public UrlBuilder(string scheme, string host, int port)
        {
            _ub = new UriBuilder(scheme, host, port);
        }

        public UrlBuilder(string scheme, string host, int port, string path, string extra)
        {
            _ub = new UriBuilder(scheme, host, port, path, extra);
        }

        #endregion

        #region Public properties

        public string Fragment
        {
            get
            {
                return _ub.Fragment;
            }

            set
            {
                _ub.Fragment = value;
            }
        }

        public string Host
        {
            get
            {
                return _isRelative ? null : _ub.Host;
            }

            set
            {
                _ub.Host = value;
                _isRelative = value == null ? true : false;
            }
        }

        public string UserName
        {
            get
            {
                return _ub.UserName;
            }

            set
            {
                _ub.UserName = value;
            }
        }

        public string Password
        {
            get
            {
                return _ub.Password;
            }

            set
            {
                _ub.Password = value;
            }
        }

        public string Path
        {
            get
            {
                return _ub.Path;
            }

            set
            {
                _ub.Path = value;
            }
        }

        public int Port
        {
            get
            {
                return _ub.Port;
            }

            set
            {
                _ub.Port = value;
            }
        }
            
        public QueryString Query
        {
            get
            {
                return _qs;
            }
        }

        public string Scheme
        {
            get
            {
                return _ub.Scheme;
            }

            set
            {
                _ub.Scheme = value;
            }
        }

        public Uri Uri
        {
            get
            {
                _ub.Query = Query.ToString();
                var uri = _ub.Uri;
                
                // если url предполагается относительным, превращаем его обратно в таковой
                if (_isRelative)
                {
                    uri = new Uri(uri.PathAndQuery, UriKind.Relative);
                }

                return uri;
            }
        }

        public string Url
        {
            get
            {
                return _isRelative ? Uri.OriginalString : Uri.AbsoluteUri;
            }
        }

        #endregion
        
        public string GetRelativeUrl()
        {
            return String.Format("{0}?{1}", Path, Query);
        }
    }
}
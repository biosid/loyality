using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web;

namespace RapidSoft.Loaders.Geocoder.Service.WebServiceClient
{
    public class QueryString : NameValueCollection
    {
        #region Constructors

        public QueryString(string query)
        {
            Add(query);
        }

        public QueryString()
        {
        }

        public QueryString(NameValueCollection clone) : base(clone)
        {
        }

        #endregion



        #region Public methods

        public virtual void Add(string key, int value)
        {
            Add(key, value.ToString());
        }

        public virtual void Add(IDictionary<string, string> query)
        {
            foreach (var key in query.Keys)
            {
                Add(key, query[key]);       
            }
        }

        public virtual void Add(string query)
        {
            var collection = HttpUtility.ParseQueryString(query);
            Add(collection);
        }

        public virtual void Add(IDictionary query)
        {
            foreach (var key in query.Keys)
            {
                Add(key.ToString(), query[key].ToString());
            }
        }

        public virtual bool Has(string key)
        {
            return GetValues(key) != null;
        }

        public virtual void ClearExcept(string except)
        {
            ClearExcept(new[] { except });
        }

        public virtual void ClearExcept(string[] except)
        {
            var rem = new ArrayList();
            foreach (string s in AllKeys)
            {
                foreach (string e in except)
                {
                    if (e != s)
                    {
                        continue;
                    }

                    rem.Add(s);
                    break;
                }
            }

            foreach (string s in rem)
            {
                Remove(s);
            }
        }

        public override string ToString()
        {
            // capacity расчитана на самый распространённый случай: каждый ключ имеет одно значение
            var capacity = (AllKeys.Length * 4) - 1; 

            var sb = capacity > 0 ? new StringBuilder(capacity) : new StringBuilder();
            for (var i = 0; i < Count; i++)
            {
                if (i > 0)
                {
                    sb.Append('&');
                }

                var key = GetKey(i);
                var values = GetValues(i);

                if (values != null && values.Length > 0)
                {
                    for (var j = 0; j < values.Length; j++)
                    {
                        if (j > 0)
                        {
                            sb.Append('&');
                        }

                        sb.Append(key);
                        sb.Append('=');
                        sb.Append(HttpUtility.UrlEncode(values[j]));
                    }      
                } 
                else
                {
                    sb.Append(key);
                }
            }

            return sb.ToString();
        }

        #endregion

        #region Static members

        public static QueryString FromUrl(string url)
        {
            var parts = url.Split('?');
            return parts.Length == 1 ? new QueryString() : new QueryString(parts[1]);
        }

        #endregion
    }
}
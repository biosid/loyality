using System.Web;
using Newtonsoft.Json;
using Vtb24.Site.Services.ClientService.Models;
using Vtb24.Site.Services.Models;

namespace Vtb24.Site.Services.ClientService
{
    public class CookieSessionStorage
    {
        public const string SESSION_COOKIE_NAME = "session-data";

        public CookieSessionStorage(HttpContextBase context)
        {
            _context = context;
            _cookie = _context.Request.Cookies[SESSION_COOKIE_NAME];
        }

        private readonly HttpContextBase _context;
        private HttpCookie _cookie;
        private readonly object _lock = new object();


        #region API

        public void SetLocation(UserLocation location)
        {
            InsureCookie();

            var serialized = JsonConvert.SerializeObject(location, Formatting.None);
            _cookie["location"] = HttpUtility.UrlEncode(serialized);
        }

        public UserLocation GetUserLocation()
        {
            if (_cookie == null || string.IsNullOrEmpty(_cookie["location"]))
            {
                return null;
            }

            var serialized = HttpUtility.UrlDecode(_cookie["location"]);
            var location = JsonConvert.DeserializeObject<UserLocation>(serialized);
            return location;
        }

        #endregion


        protected void InsureCookie()
        {
            lock (_lock)
            {
                _cookie = _cookie ?? new HttpCookie(SESSION_COOKIE_NAME) { HttpOnly = true };
                _context.Response.SetCookie(_cookie);   
            }
        }
    }
}
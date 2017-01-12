namespace RapidSoft.Loaders.Geocoder.Service.WebServiceClient
{
    public class HttpServiceRequestFactory : IHttpServiceRequestFactory
    {
        private readonly string _url;

        public HttpServiceRequestFactory(string url)
        {
            _url = url;
        }

        public string Url
        {
            get
            {
                return _url;
            }
        }

        public IHttpServiceRequest Create()
        {
            return new HttpServiceRequest(_url);
        }
    }
}
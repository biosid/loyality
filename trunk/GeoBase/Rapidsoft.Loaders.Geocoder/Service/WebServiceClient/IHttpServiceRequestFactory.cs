namespace RapidSoft.Loaders.Geocoder.Service.WebServiceClient
{
    public interface IHttpServiceRequestFactory 
    {
        string Url { get; }

        IHttpServiceRequest Create();
    }
}
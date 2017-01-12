namespace RapidSoft.Loaders.Geocoder.Service
{
    public interface IGeocodingService
    {
        IGeocodingResponse ResolveAddress(string address);

        string ServiceName { get; }

        IGeocodingResponse FromXmlString(string xml);
    }
}
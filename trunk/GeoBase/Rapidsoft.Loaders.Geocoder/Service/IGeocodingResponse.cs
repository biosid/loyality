namespace RapidSoft.Loaders.Geocoder.Service
{
    public interface IGeocodingResponse
    {
        IResolvedAddress[] Addresses { get; }

        string RawResponse { get; }
    }
}
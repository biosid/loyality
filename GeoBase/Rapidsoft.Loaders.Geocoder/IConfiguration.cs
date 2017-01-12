namespace RapidSoft.Loaders.Geocoder
{
    public interface IConfiguration
    {
        string ConnectionString { get; }

        string ProviderName { get; }

        string EtlPackageId { get; }

        int CountInPackage { get; }
    }
}

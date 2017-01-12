using System;
using System.Data;
using System.Data.SqlClient;
using RapidSoft.GeoPoints.Entities;
using RapidSoft.Loaders.Geocoder.Entities;
using RapidSoft.Loaders.Geocoder.Service;
using RapidSoft.Loaders.Geocoder.Service.Yandex;

namespace RapidSoft.Geo.Points.Geocoder.Service
{
    public class ServiceCachingDecorator : IGeocodingService
    {
        private readonly IGeocodingService geocodingService;

        public ServiceCachingDecorator(IGeocodingService geocodingService, SqlConnection connection, string tableName)
        {
            if (geocodingService == null)
            {
                throw new ArgumentNullException("geocodingService");    
            }
            this.geocodingService = geocodingService;
            Connection = connection;
            TableName = tableName;
        }

        public SqlConnection Connection { get; private set; }

        public string TableName { get; private set; }

        public string ServiceName
        {
            get { return geocodingService.ServiceName; }
        }

        public IGeocodingResponse FromXmlString(string xml)
        {
            return geocodingService.FromXmlString(xml);
        }

        public IGeocodingResponse ResolveAddress(string address)
        {
            if (String.IsNullOrEmpty(address))
            {
                throw new ArgumentNullException("address");
            }

            var cache = FindAddress(address);
            if (cache != null)
            {
                return FromXmlString(cache.RawResponse);
            }

            var response = geocodingService.ResolveAddress(address);
            var newCache = new GeocodingCache(address, response.RawResponse);
            SaveCache(newCache);

            return response;
        }

        private void SaveCache(GeocodingCache newCache)
        {
            var command = new SqlCommand(string.Format(@"INSERT INTO {0} ([Address] ,[RawResponse]) VALUES (@Address, @RawResponse)", TableName), Connection);
            command.Parameters.Add("Address", SqlDbType.NVarChar).Value = newCache.Address;
            command.Parameters.Add("RawResponse", SqlDbType.NVarChar).Value = newCache.RawResponse;
            command.ExecuteNonQuery();
        }

        private GeocodingCache FindAddress(string address)
        {
            var command = new SqlCommand(string.Format(@"SELECT TOP 1 [ID], [Address], [RawResponse] FROM {0} WITH(NOLOCK) WHERE [Address] = '{1}'", TableName, address), Connection);
            using (var reader = command.ExecuteReader())
            {                           
                while (reader.Read())
                {
                    return new GeocodingCache((string) reader["Address"], (string) reader["RawResponse"]);
                }
            }
            return null;
        }
    }
}
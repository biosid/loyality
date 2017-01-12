using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

using RapidSoft.GeoPoints.Entities;
using RapidSoft.Loaders.Geocoder;
using RapidSoft.Loaders.Geocoder.Entities;

namespace RapidSoft.Loaders.Geocoder.DataSources
{
    [DataSourceInfo(Name = "BankServicePoints")]
    public class BankServicePointsDataSource : IDataSource
    {
        Configuration config = new Configuration();

        public IEnumerable<ILocation> GetLocations()
        {
            using (SqlConnection conn = new SqlConnection(config.ConnectionString))
            {
                conn.Open();
                SqlCommand command = conn.CreateCommand();
                command.CommandText = @"
                    SELECT  [Id],[City],[Address]
                    FROM    [bankdict].[BankServicePoint_VIEW] p WITH(NOLOCK) where Id ='B33B30D9-7133-45D0-9D7C-1B0F67499143'";
                    //WHERE   p.Id not in (select Id from [bankdict].BankServicePointGeoInfo_VIEW where GeoSystem = @GeoSystem)";

                command.Parameters.AddWithValue("@GeoSystem", config.GeoCodingService);
                command.CommandTimeout *= 10;
                command.CommandType = System.Data.CommandType.Text;

                List<ILocation> locations = new List<ILocation>();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var result = string.Empty;
                        var loc = new BankServicePoint()
                        {
                            Id = reader["Id"],
                            City = (string)GetValue(reader["City"]),
                            SourceAddress = (string)GetValue(reader["Address"]),
                        };
                        loc.Address = GetAddress(loc);
                        locations.Add(loc);
                    }
                }

                return locations;
            }
        }

        private object GetValue(object value)
        {
            if (value is DBNull)
            {
                return null;
            }
            return value;
        }

        public void UpdateGeoInfo(object key, LocationGeoInfo geoInfo)
        {
            using (SqlConnection conn = new SqlConnection(config.ConnectionString))
            {
                conn.Open();

                SqlCommand command = conn.CreateCommand();
                command.CommandText = @"INSERT INTO [bankdict].[BankServicePointGeoInfo_VIEW] ([Id], [GeoSystem], [Lat], [Lng], [GeoCodingStatus], [GeoCodingAccuracy], [GeoDateTime], [EtlPackageId], [EtlSessionId], [CreatedDateTime], [CreatedUtcDateTime], [ModifiedDateTime], [ModifiedUtcDateTime])
                                        VALUES (@Id, @GeoSystem, @Lat, @Lng, @GeoCodingStatus, @GeoCodingAccuracy, @GeoDateTime, @EtlPackageId, @EtlSessionId, @CreatedDateTime, @CreatedUtcDateTime, @ModifiedDateTime, @ModifiedUtcDateTime)";

                command.CommandTimeout *= 10;

                command.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = (Guid)key;
                command.Parameters.Add("@GeoSystem", SqlDbType.NVarChar).Value = geoInfo.GeoSystem;
                command.Parameters.Add("@Lat", SqlDbType.Decimal).Value = geoInfo.Lat;
                command.Parameters.Add("@Lng", SqlDbType.Decimal).Value = geoInfo.Lng;
                command.Parameters.Add("@GeoCodingStatus", SqlDbType.Int).Value = (int)geoInfo.GeoCodingStatus;
                command.Parameters.Add("@GeoCodingAccuracy", SqlDbType.Int).Value = (int)geoInfo.GeoCodingAccuracy;
                command.Parameters.Add("@GeoDateTime", SqlDbType.DateTime).Value = geoInfo.GeoDateTime;
                command.Parameters.Add("@EtlPackageId", SqlDbType.UniqueIdentifier).Value = geoInfo.EtlPackageId;
                command.Parameters.Add("@EtlSessionId", SqlDbType.UniqueIdentifier).Value = geoInfo.EtlSessionId;
                command.Parameters.Add("@CreatedDateTime", SqlDbType.DateTime).Value = DateTime.Now;
                command.Parameters.Add("@CreatedUtcDateTime", SqlDbType.DateTime).Value = DateTime.Now;
                command.Parameters.Add("@ModifiedDateTime", SqlDbType.DateTime).Value = DateTime.Now;
                command.Parameters.Add("@ModifiedUtcDateTime", SqlDbType.DateTime).Value = DateTime.Now;

                command.ExecuteNonQuery();
            }
        }

        private string GetAddress(BankServicePoint location)
        {
            return String.Format("{0}, {1}", location.City, location.SourceAddress);
        }
    }
}

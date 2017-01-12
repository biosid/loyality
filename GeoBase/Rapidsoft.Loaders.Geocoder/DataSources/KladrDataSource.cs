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
    [DataSourceInfo(Name = "Kladr")]
    public class KladrDataSource : IDataSource
    {
        Configuration config = new Configuration();

        //public string Name
        //{ 
        //    get 
        //    { 
        //        return "KladrDataSource"; 
        //    } 
        //}

        public IEnumerable<ILocation> GetLocations()
        {
            using (SqlConnection conn = new SqlConnection(config.ConnectionString))
            {
                conn.Open();
                SqlCommand command = conn.CreateCommand();
                command.CommandText = @"
                    SELECT [Id],[LocationType],[Name],[RegionName], [RegionToponym], [DistrictName], [DistrictToponym], [CityName], [CityToponym], [Toponym], [TownName], [TownToponym]
                    FROM [Geopoints].[Location_VIEW] WITH(NOLOCK)
                    WHERE [IsCity] = 1 AND [Id] NOT IN (SELECT [Id] FROM [Geopoints].[LocationGeoInfo_VIEW] WHERE GeoSystem = @GeoSystem)";

                command.Parameters.AddWithValue("@GeoSystem", config.GeoCodingService);
                command.CommandTimeout *= 10;
                command.CommandType = System.Data.CommandType.Text;

                List<ILocation> locations = new List<ILocation>();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var result = string.Empty;
                        var loc = new KladrLocation()
                        {
                            Id = reader["Id"],
                            LocationType = (int)GetValue(reader["LocationType"]),
                            Name = (string)GetValue(reader["Name"]),
                            RegionName = (string)GetValue(reader["RegionName"]),
                            RegionToponym = (string)GetValue(reader["RegionToponym"]),
                            DistrictName = (string)GetValue(reader["DistrictName"]),
                            DistrictToponym = (string)GetValue(reader["DistrictToponym"]),
                            CityName = (string)GetValue(reader["CityName"]),
                            CityToponym = (string)GetValue(reader["CityToponym"]),
                            Toponym = (string)GetValue(reader["Toponym"]),
                            TownName = (string)GetValue(reader["TownName"]),
                            TownToponym = (string)GetValue(reader["TownToponym"]),
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
                command.CommandText = @"INSERT INTO [Geopoints].[LocationGeoInfo_VIEW] ([Id], [GeoSystem], [Lat], [Lng], [GeoCodingStatus], [GeoCodingAccuracy], [GeoDateTime], [EtlPackageId], [EtlSessionId], [CreatedDateTime], [CreatedUtcDateTime], [ModifiedDateTime], [ModifiedUtcDateTime])
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

        private string GetAddress(KladrLocation location)
        {
            var result = string.Empty;
            AddStringToAddress(location.RegionName, location.RegionToponym, ref result);
            AddStringToAddress(location.DistrictName, location.DistrictToponym, ref result);
            AddStringToAddress(location.CityName, location.CityToponym, ref result);
            AddStringToAddress(location.TownName, location.TownToponym, ref result);
            AddStringToAddress(location.Name, location.Toponym, ref result);
            return result;
        }

        private void AddStringToAddress(string name, string toponym, ref string result)
        {
            if (!string.IsNullOrEmpty(name))
            {
                if (!string.IsNullOrEmpty(result))
                {
                    result += ", ";
                }

                result += (string.IsNullOrEmpty(toponym) ? string.Empty : toponym + ". ") +
                    name;
            }
        }

    }
}

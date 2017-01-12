namespace RapidSoft.GeoPoints.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;

    using RapidSoft.Extensions;
    using RapidSoft.GeoPoints.Entities;

    public class LocationRepository
    {
        /// <summary>
        /// Загружает страны по буквенному коду страны альфа-2 по ОКСМ (двухбуквеннный код).
        /// </summary>
        /// <param name="alpha2Codes">
        /// Набор буквенных кодов страны альфа-2.
        /// </param>
        /// <returns>
        /// Список найденных стран.
        /// </returns>
        public IList<Country> GetCountriesByAlpha2Codes(IList<string> alpha2Codes)
        {
            // TODO: Если запросов к таблице "[Geopoints].[OKSMCountries]" станет много следует вынести в отдельный репозиторий
            if (alpha2Codes == null || alpha2Codes.Count == 0)
            {
                return new List<Country>();
            }

            var codes = string.Join(",", alpha2Codes);
            using (var connection = new SqlConnection(RepositoriesConfig.ConnectionString))
            {
                connection.Open();
                var procedureName = string.Format("[Geopoints].[GetCountriesByAlpha2Codes]", RepositoriesConfig.SchemaToken);

                using (var command = new SqlCommand(procedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    RepositoriesUtils.AddParameter(command, "@codes", codes);
                    using (var reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        var retVal = ReadCountries(reader);

                        return retVal;
                    }
                }
            }
        }

        public List<Location> GetCountries(string Locale, string NameSearchPattern, int? Skip, int? Top)
        {
            using (SqlConnection connection = new SqlConnection(RepositoriesConfig.ConnectionString))
            {
                connection.Open();
                String sProcedureName = String.Format("[geopoints].[GetCountries]", RepositoriesConfig.SchemaToken);

                SqlCommand command = new SqlCommand(sProcedureName, connection);
                RepositoriesUtils.AddParameter(command, "@locale", Locale);
                RepositoriesUtils.AddParameter(command, "@nameSearchPattern", NameSearchPattern);
                RepositoriesUtils.AddParameter(command, "@skip", Skip);
                RepositoriesUtils.AddParameter(command, "@top", Top);


                command.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                return ReadItems(reader);
            }
        }

        public List<Location> GetLocationsByParent(Guid? ParentId, int? LocationType, string NameSearchPattern, string Locale, int? Skip, int? Top)
        {
            using (SqlConnection connection = new SqlConnection(RepositoriesConfig.ConnectionString))
            {
                connection.Open();
                String sProcedureName = String.Format("[geopoints].[GetLocationsByParent]", RepositoriesConfig.SchemaToken);

                SqlCommand command = new SqlCommand(sProcedureName, connection);
                RepositoriesUtils.AddParameter(command, "@parentID", ParentId);
                RepositoriesUtils.AddParameter(command, "@locationType", LocationType);
                RepositoriesUtils.AddParameter(command, "@nameSearchPattern", NameSearchPattern);
                RepositoriesUtils.AddParameter(command, "@locale", Locale);
                RepositoriesUtils.AddParameter(command, "@skip", Skip);
                RepositoriesUtils.AddParameter(command, "@top", Top);

                command.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                return ReadItems(reader);
            }
        }

        public List<string> GetExistKladrCodes(IEnumerable<string> kladrs, int len = 13)
        {
            // NOTE: максимальная строка в базе 4000 символов, длинна кладр кода len символов + запятая
            var batchSize = 4000 / (len + 1);

            var batches = kladrs.Batch(batchSize);

            var retVal = new List<string>();
            foreach (var batch in batches)
            {
                var result = InternalGetExistKladrCodes(batch);
                retVal.AddRange(result);
            }

            return retVal;
        }

        public List<Location> GetLocations(string parentKladrCode, int[] locationTypes, string[] toponyms, string nameSearchPattern, bool? regionIsCityOnly, int? skip, int? top)
        {
            // NOTE: если параметр regionIcCityOnly равен true,
            //       то исключаются все результаты, у которых LocationType равен 1 (Region) и IsCity не равен 1

            using (var connection = new SqlConnection(RepositoriesConfig.ConnectionString))
            {
                connection.Open();

                var paramValueLocationTypes = locationTypes == null ? null : string.Join(",", locationTypes);
                var paramValueToponyms = toponyms == null ? null : string.Join(",", toponyms);

                const string SProcedureName = "[geopoints].[GetLocations]";

                var command = new SqlCommand(SProcedureName, connection);
                RepositoriesUtils.AddParameter(command, "@parentKladrCode", parentKladrCode);
                RepositoriesUtils.AddParameter(command, "@locationType", paramValueLocationTypes);
                RepositoriesUtils.AddParameter(command, "@toponyms", paramValueToponyms);
                RepositoriesUtils.AddParameter(command, "@nameSearchPattern", nameSearchPattern);
                RepositoriesUtils.AddParameter(command, "@regionIsCityOnly", regionIsCityOnly);
                
                RepositoriesUtils.AddParameter(command, "@skip", skip);
                RepositoriesUtils.AddParameter(command, "@top", top);

                command.CommandType = CommandType.StoredProcedure;

                var reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                return ReadItems(reader);
            }
        }

        public List<Location> GetLocationsByKladrCode(string ParentKladrCode, int[] LocationTypes, string NameSearchPattern, string Locale, int? Skip, int? Top)
        {
            using (var connection = new SqlConnection(RepositoriesConfig.ConnectionString))
            {
                connection.Open();

                var paramValueLocationTypes = LocationTypes == null ? null : string.Join(",", LocationTypes);

                const string SProcedureName = "[geopoints].[GetLocationsByKladrCode]";

                var command = new SqlCommand(SProcedureName, connection);
                RepositoriesUtils.AddParameter(command, "@kladrCode", ParentKladrCode);
                RepositoriesUtils.AddParameter(command, "@locationType", paramValueLocationTypes);
                RepositoriesUtils.AddParameter(command, "@nameSearchPattern", NameSearchPattern);
                RepositoriesUtils.AddParameter(command, "@locale", Locale);
                RepositoriesUtils.AddParameter(command, "@skip", Skip);
                RepositoriesUtils.AddParameter(command, "@top", Top);

                command.CommandType = CommandType.StoredProcedure;

                var reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                return ReadItems(reader);
            }
        }

        public static long IP2Long(string ip)
        {
            string[] ipBytes;
            double num = 0;
            if (!string.IsNullOrEmpty(ip))
            {
                ipBytes = ip.Split('.');
                for (int i = ipBytes.Length - 1; i >= 0; i--)
                {
                    num += ((int.Parse(ipBytes[i]) % 256) * Math.Pow(256, (3 - i)));
                }
            }
            return (long)num;
        }

        public Location GetLocationByIP(string IP)
        {
            var ipInt = IP2Long(IP);

            using (var connection = new SqlConnection(RepositoriesConfig.ConnectionString))
            {
                connection.Open();
                const string SProcedureName = "[geopoints].[GetLocationByIP]";

                using (var command = new SqlCommand(SProcedureName, connection))
                {
                    RepositoriesUtils.AddParameter(command, "@ipINT", ipInt);
                    command.CommandType = CommandType.StoredProcedure;

                    using (var reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        var locations = ReadItems(reader);
                        return locations.FirstOrDefault();
                    }
                }
            }
        }

        public List<Location> GetLocationsByIP(string IP, int LocationType, string Locale, int? Skip, int? Top)
        {

            long ipInt = IP2Long(IP);


            using (SqlConnection connection = new SqlConnection(RepositoriesConfig.ConnectionString))
            {
                connection.Open();
                String sProcedureName = String.Format("[geopoints].[GetLocationsByIP]", RepositoriesConfig.SchemaToken);

                SqlCommand command = new SqlCommand(sProcedureName, connection);
                RepositoriesUtils.AddParameter(command, "@ipINT", ipInt);
                RepositoriesUtils.AddParameter(command, "@locationType", LocationType);
                RepositoriesUtils.AddParameter(command, "@locale", Locale);
                RepositoriesUtils.AddParameter(command, "@skip", Skip);
                RepositoriesUtils.AddParameter(command, "@top", Top);



                command.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                return ReadItems(reader);
            }
        }

        public Location GetLocationById(Guid Id, string Locale)
        {

            using (SqlConnection connection = new SqlConnection(RepositoriesConfig.ConnectionString))
            {
                connection.Open();
                String sProcedureName = String.Format("[geopoints].[GetLocationById]", RepositoriesConfig.SchemaToken);

                SqlCommand command = new SqlCommand(sProcedureName, connection);
                RepositoriesUtils.AddParameter(command, "@Id", Id);
                RepositoriesUtils.AddParameter(command, "@locale", Locale);

                command.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                List<Location> ll = ReadItems(reader);
                return (ll.Count == 0 ? null : ll[0]);
            }
        }

        public Location GetLocationByExternalId(string Locale, string ExternalId)
        {
            using (SqlConnection connection = new SqlConnection(RepositoriesConfig.ConnectionString))
            {
                connection.Open();
                String sProcedureName = String.Format("[geopoints].[GetLocationByExternalId]", RepositoriesConfig.SchemaToken);

                SqlCommand command = new SqlCommand(sProcedureName, connection);
                RepositoriesUtils.AddParameter(command, "@locale", Locale);
                RepositoriesUtils.AddParameter(command, "@ExternalId", ExternalId);

                command.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                List<Location> ll = ReadItems(reader);
                return (ll.Count == 0 ? null : ll[0]);
            }
        }

        public Location GetLocationByKladrCode(string KladrCode, string Locale)
        {

            using (SqlConnection connection = new SqlConnection(RepositoriesConfig.ConnectionString))
            {
                connection.Open();
                String sProcedureName = String.Format("[geopoints].[GetLocationByKladrCode]", RepositoriesConfig.SchemaToken);

                SqlCommand command = new SqlCommand(sProcedureName, connection);
                RepositoriesUtils.AddParameter(command, "@KladrCode", KladrCode);
                //RepositoriesUtils.AddParameter(command, "@LocationType", LocationType);
                RepositoriesUtils.AddParameter(command, "@locale", Locale);

                command.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                List<Location> ll = ReadItems(reader);
                return (ll.Count == 0 ? null : ll[0]);
            }
        }

        public Location GetLocationByKladrCode(string KladrCode)
        {
            // NOTE: LocationType и Locale не используются, но почему-то поленились убрать...
            const int fakeLocationType = -1;
            var fakeLocale = string.Empty;
            return this.GetLocationByKladrCode(KladrCode, fakeLocale);
        }

        public Location GetLocationByCoordinates(string Locale, string GeoSystem, int? LocationType, int? Radius, decimal Lat, decimal Lng)
        {
            using (SqlConnection connection = new SqlConnection(RepositoriesConfig.ConnectionString))
            {
                connection.Open();
                String sProcedureName = "[Geopoints].[GetLocationByCoordinates]";

                SqlCommand command = new SqlCommand(sProcedureName, connection);
                RepositoriesUtils.AddParameter(command, "@locale", Locale);
                RepositoriesUtils.AddParameter(command, "@GeoSystem", GeoSystem);
                RepositoriesUtils.AddParameter(command, "@LocationType", LocationType);
                RepositoriesUtils.AddParameter(command, "@Radius", Radius);
                RepositoriesUtils.AddParameter(command, "@Lat", Lat);
                RepositoriesUtils.AddParameter(command, "@Lng", Lng);

                command.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                List<Location> ll = ReadItems(reader);
                return (ll.Count == 0 ? null : ll[0]);
            }
        }

        private static List<Location> ReadItems(SqlDataReader reader)
        {
            List<Location> res = new List<Location>();

            while (reader.Read())
            {
                object obj = null;

                switch (Convert.ToInt32(reader["LocationType"]))
                {
                    case 8:
                        obj = new ServicePoint();
                        break;
                    default:
                        obj = new Location();
                        break;
                }

                var loc = (obj as Location);

                loc.Id = new Guid(reader["Id"].ToString());
                loc.ParentId = RepositoriesUtils.GetNullableGuidValue(reader["ParentId"]);
                loc.LocationType = Convert.ToInt32(reader["LocationType"]);
                loc.Name = reader["Name"] as string;
                loc.Toponym = reader["Toponym"] as string;
                loc.KladrCode = reader["KladrCode"] as string;
                loc.RegionName = reader["RegionName"] as string;
                loc.RegionId = RepositoriesUtils.GetNullableGuidValue (reader["RegionId"]);
                loc.RegionToponym = reader["RegionToponym"] as string;
                loc.DistrictName = reader["DistrictName"] as string;
                loc.DistrictId = RepositoriesUtils.GetNullableGuidValue(reader["DistrictId"]);
                loc.DistrictToponym = reader["DistrictToponym"] as string;
                loc.CityName = reader["CityName"] as string;
                loc.CityId = RepositoriesUtils.GetNullableGuidValue(reader["CityId"]);
                loc.CityToponym = reader["CityToponym"] as string;
                loc.TownName = reader["TownName"] as string;
                loc.TownId = RepositoriesUtils.GetNullableGuidValue(reader["TownId"]);
                loc.TownToponym = reader["TownToponym"] as string;

                if (HasColumn(reader, "ExternalId"))
                    loc.ExternalId = reader["ExternalId"] as string;

                if (HasColumn(reader, "CountryId"))
                    loc.CountryId = RepositoriesUtils.GetNullableGuidValue(reader["CountryId"]);

                if (HasColumn(reader, "Index"))
                    loc.Index = reader["Index"] as string;

                if (HasColumn(reader, "Address"))
                    loc.Address = reader["Address"] as string;

                ServicePoint sp = obj as ServicePoint;
                if (sp != null)
                {
                    if (HasColumn(reader, "InstantTransferSystem"))
                        sp.InstantTransferSystem = reader["InstantTransferSystem"] as string;

                    if (HasColumn(reader, "Unaddressed"))
                        sp.Unaddressed = RepositoriesUtils.ConvertToInt(reader["Unaddressed"]);

                    if (HasColumn(reader, "Code"))
                        sp.Code = reader["Code"] as string;

                    if (HasColumn(reader, "PhoneNumber"))
                        sp.PhoneNumber = reader["PhoneNumber"] as string;

                    if (HasColumn(reader, "Schedule"))
                        sp.Schedule = reader["Schedule"] as string;

                    if (HasColumn(reader, "Currency"))
                        sp.Currency = reader["Currency"] as string;

                    if (HasColumn(reader, "Summa"))
                        sp.Summa = reader["Summa"] as string;

                    if (HasColumn(reader, "MaxSumma"))
                        sp.MaxSumma = reader["MaxSumma"] as string;

                    if (HasColumn(reader, "Description"))
                        sp.Description = reader["Description"] as string;
                }

                res.Add(obj as Location);
            }

            return res;
        }

        private static IList<Country> ReadCountries(SqlDataReader reader)
        {
            var res = new List<Country>();

            while (reader.Read())
            {
                var country = new Country
                {
                    NumberCode = Convert.ToInt32(reader["NumberCode"]),
                    Name = reader["Name"].ToString(),
                    Description = reader["Description"].ToString(),
                    Alpha2Code = reader["Alpha2Code"].ToString(),
                    Alpha3Code = reader["Alpha3Code"].ToString()
                };

                res.Add(country);
            }

            return res;
        }

        private static List<string> InternalGetExistKladrCodes(IEnumerable<string> kladrs)
        {
            var kladrCodes = string.Join(",", kladrs);
            using (var connection = new SqlConnection(RepositoriesConfig.ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[geopoints].[GetLocationsByKladrCodes]";

                    RepositoriesUtils.AddParameter(command, "@kladrCodes", kladrCodes);

                    using (var reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        var retVal = new List<string>();
                        while (reader.Read())
                        {
                            retVal.Add(reader["KladrCode"] as string);
                        }

                        return retVal;
                    }
                }
            }
        }

        private static bool HasColumn(SqlDataReader reader, string columnName)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                string s = reader.GetName(i);
                if (s.Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }

            return false;
        }
    }
}
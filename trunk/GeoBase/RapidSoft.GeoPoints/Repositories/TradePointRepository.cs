using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RapidSoft.GeoPoints.Entities;
using System.Data.SqlClient;
using System.Data;


namespace RapidSoft.GeoPoints.Repositories
{
    public class TradePointRepository
    {

		public List<TradePoint> GetTradePointsByLocation(Guid LocationId, string Locale, int? Skip, int? Top)
		{
			using (SqlConnection connection = new SqlConnection(RepositoriesConfig.ConnectionString))
			{
				connection.Open();
                String sProcedureName = String.Format("[geopoints].[GetTradePointsByLocation]", RepositoriesConfig.SchemaToken);

				SqlCommand command = new SqlCommand(sProcedureName, connection);
				RepositoriesUtils.AddParameter(command, "@LocationId", LocationId);
				RepositoriesUtils.AddParameter(command, "@locale", Locale);
				RepositoriesUtils.AddParameter(command, "@skip", Skip);
				RepositoriesUtils.AddParameter(command, "@top", Top);


				command.CommandType = CommandType.StoredProcedure;

				SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);
				return ReadItems(reader);
			}
		}

        private List<TradePoint> ReadItems(SqlDataReader reader)
        {
			List<TradePoint> res = new List<TradePoint>();

            while (reader.Read())
            {
				var n = new TradePoint
				{
					LocationId = new Guid(reader["LocationId"].ToString()),
					TypeId = new Guid(reader["TypeId"].ToString()),
					ExternalId = reader["ExternalId"] as string,
					Name = reader["Name"] as string,
					Description = reader["Description"] as string
				};
                res.Add(n);
            }
            return res;
        }
    }
}
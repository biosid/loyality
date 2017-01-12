namespace RapidSoft.VTB24.BankConnector.DataModels
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Data;
	using System.Data.Entity.Infrastructure;
	using System.Data.Metadata.Edm;
	using System.Data.SqlClient;
	using System.Linq;
	using System.Reflection;
	using System.Text.RegularExpressions;

	public partial class BankConnectorDBContext
	{
		public void BulkInsertAll<T>(T[] entities) where T : class
		{
			var conn = (SqlConnection)Database.Connection;

			conn.Open();

			Type t = typeof(T);
			Set(t).ToString();
			var objectContext = ((IObjectContextAdapter)this).ObjectContext;
			var workspace = objectContext.MetadataWorkspace;
			var mappings = GetMappings(workspace, objectContext.DefaultContainerName, typeof(T).Name);

			var tableName = GetTableName<T>();
			var bulkCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.CheckConstraints | SqlBulkCopyOptions.UseInternalTransaction, null) { DestinationTableName = tableName };

			// Foreign key relations show up as virtual declared 
			// properties and we want to ignore these.
			var properties = t.GetProperties().Where(p => !p.GetGetMethod().IsVirtual).ToArray();
			var table = new DataTable();
			foreach (var property in properties)
			{
				Type propertyType = property.PropertyType;

				// Nullable properties need special treatment.
				if (propertyType.IsGenericType &&
					propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
				{
					propertyType = Nullable.GetUnderlyingType(propertyType);
				}

				// Since we cannot trust the CLR type properties to be in the same order as
				// the table columns we use the SqlBulkCopy column mappings.
				table.Columns.Add(new DataColumn(property.Name, propertyType));
				var clrPropertyName = property.Name;
				var tableColumnName = mappings[property.Name];
				bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping(clrPropertyName, tableColumnName));
			}

			// Add all our entities to our data table
			foreach (var entity in entities)
			{
				var e = entity;
				table.Rows.Add(properties.Select(property => GetPropertyValue(property.GetValue(e, null))).ToArray());
			}

			// send it to the server for bulk execution
			bulkCopy.BulkCopyTimeout = 5 * 60;
			bulkCopy.WriteToServer(table);

			conn.Close();
		}

		private string GetTableName<T>() where T : class
		{
			var dbSet = Set<T>();
			var sql = dbSet.ToString();
			var regex = new Regex(@"FROM (?<table>.*) AS");
			var match = regex.Match(sql);
			return match.Groups["table"].Value;
		}

		private object GetPropertyValue(object o)
		{
			if (o == null)
				return DBNull.Value;
			return o;
		}

		private Dictionary<string, string> GetMappings(MetadataWorkspace workspace, string containerName, string entityName)
		{
			var mappings = new Dictionary<string, string>();
			var storageMapping = workspace.GetItem<GlobalItem>(containerName, DataSpace.CSSpace);
			dynamic entitySetMaps = storageMapping.GetType().InvokeMember(
				"EntitySetMaps",
				BindingFlags.GetProperty | BindingFlags.NonPublic | BindingFlags.Instance,
				null, storageMapping, null);

			foreach (var entitySetMap in entitySetMaps)
			{
				var typeMappings = GetArrayList("TypeMappings", entitySetMap);
				dynamic typeMapping = typeMappings[0];
				dynamic types = GetArrayList("Types", typeMapping);

				if (types[0].Name == entityName)
				{
					var fragments = GetArrayList("MappingFragments", typeMapping);
					var fragment = fragments[0];
					var properties = GetArrayList("AllProperties", fragment);
					foreach (var property in properties)
					{
						var edmProperty = GetProperty("EdmProperty", property);
						var columnProperty = GetProperty("ColumnProperty", property);
						mappings.Add(edmProperty.Name, columnProperty.Name);
					}
				}
			}

			return mappings;
		}

		private ArrayList GetArrayList(string property, object instance)
		{
			var type = instance.GetType();
			var objects = (IEnumerable)type.InvokeMember(property, BindingFlags.GetProperty | BindingFlags.NonPublic | BindingFlags.Instance, null, instance, null);
			var list = new ArrayList();
			foreach (var o in objects)
			{
				list.Add(o);
			}
			return list;
		}

		private dynamic GetProperty(string property, object instance)
		{
			var type = instance.GetType();
			return type.InvokeMember(property, BindingFlags.GetProperty | BindingFlags.NonPublic | BindingFlags.Instance, null, instance, null);
		}
	}
}

namespace RapidSoft.Loaylty.ProductCatalog.DataSources
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Text;

    using API.Entities;

    using Extensions;

    public class AdoMapper<T> where T : new()
    {
        private readonly AdoMapperColumn[] columns;

        private readonly PropertyDescriptorCollection props;

        public AdoMapper(AdoMapperColumn[] columns)
        {
            this.columns = columns;
            props = TypeDescriptor.GetProperties(typeof(T));
        }

        public string GetCreateTableColumns()
        {
            return string.Join(string.Empty, columns.SelectMany(c => c.ColumnSqlDeclare));
        }

        public string GetCommaSeparatedColumnNames(bool? isInsert = null, bool? isSelect = null)
        {
            return string.Join(",", columns.Select(c => c.ColumnName));
        }

        public string GetCommaSeparatedColumnParameters(bool? isInsert = null, bool? isSelect = null)
        {
            return "@" + string.Join(",@", columns.Select(c => c.ColumnName));
        }

        public string GetCommaSeparatedColumnUpdateParameters()
        {
            return string.Join(", ", columns.Select(c => c.ColumnName + " = @" + c.ColumnName));
        }

        public DataRow[] MapToDataRows(IEnumerable<T> entities)
        {
            var table = new DataTable();

            foreach (var adoMapperRow in columns)
            {
                if (adoMapperRow.IsInsert.HasValue && !adoMapperRow.IsInsert.Value)
                {
                    continue;
                }

                var column = new DataColumn();
                column.ColumnName = adoMapperRow.ColumnName;

                if (adoMapperRow.DotNetType.IsGenericType &&
                    adoMapperRow.DotNetType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    column.DataType = adoMapperRow.DotNetType.GetType();
                    column.AllowDBNull = true;
                }
                else
                {
                    column.DataType = adoMapperRow.DotNetType;
                }

                table.Columns.Add(column);
            }

            table.PrimaryKey = new[]
            {
                table.Columns["ProductId"]
            };

            foreach (var entity in entities)
            {
                var row = table.NewRow();

                foreach (var column in columns)
                {
                    if (column.IsInsert.HasValue && !column.IsInsert.Value)
                    {
                        continue;
                    }

                    var propertyDescriptor = props[column.ColumnName];

                    var value = propertyDescriptor.GetValue(entity);

                    if (value == null)
                    {
                        continue;
                    }
                    else
                    {
                        if (column.ObjToDBMapFunc != null)
                        {
                            value = column.ObjToDBMapFunc(value);
                        }
                        else
                        {
                            if (column.DotNetType == typeof(string) && column.ColumnLen != null)
                            {
                                var s = (string)value;
                                if (s.Length > column.ColumnLen)
                                {
                                    value = s.GetFirst(column.ColumnLen.Value);
                                }
                            }
                        }

                        row[column.ColumnName] = value;
                    }
                }

                table.Rows.Add(row);
            }

            return table.Select();
        }

        public SqlParameter[] GetInsertSqlParameters(Product product)
        {
            var parameters = new List<SqlParameter>();
            foreach (var column in columns)
            {
                if (column.IsInsert.HasValue && !column.IsInsert.Value)
                {
                    continue;
                }

                var propertyDescriptor = props[column.ColumnName];

                if (propertyDescriptor == null)
                {
                    throw new InvalidOperationException(string.Format("Property with name {0} in type {1} not found", column.ColumnName, typeof(T)));
                }

                var value = propertyDescriptor.GetValue(product);

                SqlParameter parameter;

                if (value == null)
                {
                    parameter = new SqlParameter(column.ColumnName, DBNull.Value);
                }
                else
                {
                    if (column.ObjToDBMapFunc != null)
                    {
                        value = column.ObjToDBMapFunc(value);
                    }
                    else
                    {
                        if (column.DotNetType == typeof(string) && column.ColumnLen != null)
                        {
                            var s = (string)value;
                            if (s.Length > column.ColumnLen)
                            {
                                value = s.GetFirst(column.ColumnLen.Value);

                                // throw new InvalidOperationException(string.Format("Column {0} limited by {1} len but value is {2}", column.ColumnName, column.ColumnLen, s));
                            }
                        }
                    }

                    parameter = new SqlParameter(column.ColumnName, value);
                }

                parameters.Add(parameter);
            }

            return parameters.ToArray();
        }

        public T GetEntity(SqlDataReader reader)
        {
            var entity = new T();

            foreach (var column in columns)
            {
                if (column.IsSelect.HasValue && !column.IsSelect.Value)
                {
                    continue;
                }

                var propertyDescriptor = props[column.ColumnName];

                if (propertyDescriptor == null)
                {
                    throw new InvalidOperationException(string.Format("Property with name {0} in type {1} not found", column.ColumnName, typeof(T)));
                }

                var ordinal = reader.GetOrdinal(column.ColumnName);
                var value = reader.GetValue(ordinal);

                if (value == DBNull.Value)
                {
                    propertyDescriptor.SetValue(entity, null);
                }
                else
                {
                    if (column.DBToObjMapFunc != null)
                    {
                        value = column.DBToObjMapFunc(value);
                    }

                    propertyDescriptor.SetValue(entity, value);
                }
            }

            return entity;
        }

        public AdoMapperColumn NewColumn(
            string columnName, 
            Type dotNetType, 
            string columnSqlDeclare = null, 
            bool? isInsert = null, 
            bool? isSelect = null, 
            Func<object, object> objToDBMapFunc = null, 
            Func<object, object> databaseToObjMapFunc = null)
        {
            return new AdoMapperColumn
            {
                ColumnName = columnName, 
                DotNetType = dotNetType, 
                ColumnSqlDeclare = columnSqlDeclare, 
                IsInsert = isInsert, 
                IsSelect = isSelect, 
                ObjToDBMapFunc = objToDBMapFunc, 
                DBToObjMapFunc = databaseToObjMapFunc
            };
        }
    }
}
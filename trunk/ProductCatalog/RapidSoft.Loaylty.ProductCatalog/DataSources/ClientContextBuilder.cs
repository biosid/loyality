namespace RapidSoft.Loaylty.ProductCatalog.DataSources
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    using Extensions;

    public class ClientContextBuilder<T>
    {
        private AdoMapperColumn[] columns;
        private string[] skips;
        private Tuple<Func<T, object>, AdoMapperColumn>[] getters;

        public ClientContextBuilder(AdoMapperColumn[] columns, params string[] skips)
        {
            columns.ThrowIfNull("columns");

            this.columns = columns;
            this.skips = skips;

            this.BuildGetters();
        }

        public Dictionary<string, string> GetProductContext(T product, string fieldNamePrefix)
        {
            var res = new Dictionary<string, string>();

            foreach (var getter in this.getters)
            {
                object val1 = getter.Item1(product);
                res.Add(fieldNamePrefix + getter.Item2.ColumnName, val1 == null ? null : val1.ToString());
            }

            return res;
        }

        private void BuildGetters()
        {
            var type = typeof(T);
            PropertyInfo[] allProperties = type.GetProperties();

            var propertiesWithoutSkipped = this.skips == null
                ? allProperties
                : allProperties.Where(x => !this.skips.Contains(x.Name));

            this.getters = propertiesWithoutSkipped.Join(
                columns,
                info => info.Name,
                column => column.ColumnName,
                this.MakeValueGetter).ToArray();
        }

        private Tuple<Func<T, object>, AdoMapperColumn> MakeValueGetter(PropertyInfo propertyInfo, AdoMapperColumn column)
        {
            ParameterExpression objParam = Expression.Parameter(typeof(T), "obj");

            Expression value;

            if (column.ObjToDBMapFunc == null)
            {
                value = Expression.PropertyOrField(objParam, propertyInfo.Name);

                if (column.ColumnLen > 0 && column.DotNetType == typeof(string))
                {
                    MemberExpression typedAccessor = Expression.PropertyOrField(objParam, propertyInfo.Name);
                    Expression<Func<object, string>> bind = x => ((string)x).GetFirst(column.ColumnLen.Value).SafeTrim();
                    value = Expression.Invoke(bind, typedAccessor);
                }
            }
            else
            {
                MemberExpression typedAccessor = Expression.PropertyOrField(objParam, propertyInfo.Name);
                Expression<Func<object, object>> bind = x => column.ObjToDBMapFunc(x);

                value = Expression.Invoke(bind, typedAccessor);
            }

            UnaryExpression castToObject = Expression.Convert(value, typeof(object));
            LambdaExpression lambdaExpr = Expression.Lambda<Func<T, object>>(castToObject, objParam);

            var retVal = (Func<T, object>)lambdaExpr.Compile();

            return new Tuple<Func<T, object>, AdoMapperColumn>(retVal, column);
        }
    }
}
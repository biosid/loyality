namespace RapidSoft.Loaylty.ProductCatalog.DataSources
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    using Extensions;
    using RapidSoft.Loaylty.Logging;

    /// <summary>
    /// Сравниватель объектов на основе коллекции <see cref="AdoMapperColumn"/>
    /// </summary>
    /// <typeparam name="T">Тип сравниваемых объектов</typeparam>
    internal class AdoMapComparer<T>
    {
        private readonly ILog log = LogManager.GetLogger(typeof(AdoMapComparer<T>));
        private readonly AdoMapperColumn[] columns;
        private readonly string[] skips;

        private Tuple<Func<T, object>, AdoMapperColumn, string>[] getters;

        public AdoMapComparer(AdoMapperColumn[] columns, params string[] skips)
        {
            columns.ThrowIfNull("columns");

            this.columns = columns;
            this.skips = skips;

            this.BuildGetters();
        }

        public bool Equals(T x, T y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (ReferenceEquals(x, null))
            {
                log.Debug("Не эквиваленты, так как первый экзепляр равен null");
                return false;
            }

            if (ReferenceEquals(y, null))
            {
                log.Debug("Не эквиваленты, так как второй экзепляр равен null");
                return false;
            }

            foreach (var getter in this.getters)
            {
                object val1 = getter.Item1(x);
                object val2 = getter.Item1(y);

                if (val1 == null && val2 == null)
                {
                    continue;
                }

                if (ReferenceEquals(val1, null))
                {
                    var mess = string.Format("Не эквиваленты, так как значение свойства {0} первого экзепляра равно null", getter.Item3);
                    log.Debug(mess);
                    return false;
                }

                if (ReferenceEquals(val2, null))
                {
                    var mess = string.Format("Не эквиваленты, так как значение свойства {0} второго экзепляра равно null", getter.Item3);
                    log.Debug(mess);
                    return false;
                }

                if (!val1.Equals(val2))
                {
                    var mess = string.Format(
                        "Не эквиваленты, так как значение свойства {0} не эквиваленты {1} и {2}",
                        getter.Item3,
                        val1,
                        val2);
                    log.Debug(mess);
                    return false;
                }
            }

            return true;
        }

        private void BuildGetters()
        {
            var type = typeof(T);
            PropertyInfo[] allProperties = type.GetProperties();

            var propertiesWithoutSkipped = this.skips == null
                ? allProperties
                : allProperties.Where(x => !this.skips.Contains(x.Name));

            var columnsWithoutSystem = this.columns.Where(x => !x.IsSystem);

            this.getters = propertiesWithoutSkipped.Join(
                columnsWithoutSystem,
                info => info.Name,
                column => column.ColumnName,
                this.MakeValueGetter).ToArray();
        }

        private Tuple<Func<T, object>, AdoMapperColumn, string> MakeValueGetter(PropertyInfo propertyInfo, AdoMapperColumn column)
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

            return new Tuple<Func<T, object>, AdoMapperColumn, string>(retVal, column, propertyInfo.Name);
        }
    }
}
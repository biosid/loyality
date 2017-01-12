namespace RapidSoft.Loaylty.ProductCatalog.DataSources
{
    using System;

    public class AdoMapperColumn
    {
        public string ColumnName
        {
            get;
            set;
        }

        public Type DotNetType
        {
            get;
            set;
        }

        public string ColumnSqlDeclare
        {
            get;
            set;
        }

        public bool? IsInsert
        {
            get;
            set;
        }

        public Func<object, object> ObjToDBMapFunc
        {
            get;
            set;
        }

        public Func<object, object> DBToObjMapFunc
        {
            get;
            set;
        }

        public bool? IsSelect
        {
            get;
            set;
        }

        public int? ColumnLen
        {
            get;
            set;
        }
        
        /// <summary>
        /// Признак указывающий что столбец является системным (ид пользователя или дата изменения и т.д.)
        /// <c>false</c> - поле является значимым  не системным.
        /// </summary>
        public bool IsSystem
        {
            get;
            set;
        }
    }
}
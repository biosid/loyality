using RapidSoft.Etl.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using RapidSoft.Etl.Monitor.Resources.Views.Shared;

namespace RapidSoft.Etl.Monitor.Models
{
    public class EtlPackageMonitorModel
    {
        #region Properties

        public int ColumnCount
        {
            get;
            set;
        }

        public int MaxItemCountInColumn
        {
            get;
            set;
        }

        public List<EtlPackageMonitorItem>[] ItemsByColumns
        {
            get;
            set;
        }

        public IEnumerable<EtlPackageMonitorItem> Items
        {
            get
            {
                if (ItemsByColumns != null && ItemsByColumns.Count() > 0)
                {
                    var items = (IEnumerable<EtlPackageMonitorItem>)ItemsByColumns[0];
                    for (int i = 1; i < ItemsByColumns.Length; i++)
                        items = items.Union(ItemsByColumns[i]);

                    return items;
                }

                return new EtlPackageMonitorItem[0];
            }
        }

        public bool HasErrors
        {
            get;
            set;
        }

        public DateTime LastUpdated
        {
            get;
            set;
        }

        #endregion

        #region Methods

        public static string ToIntelliDateTimeText(DateTime value, string format)
        {
            var today = DateTime.Today;
            var text = value.ToString(format);

            if (value.Date == today)
            {
                text = "сегодня " + text;
            }
            else if (value.Date == today.AddDays(-1))
            {
                text = "вчера " + text;
            }
            else if (value.Date == today.AddDays(-2))
            {
                text = "позавчера " + text;
            }

            return text;
        }

        public static string GetEtlStatusClass(EtlPackageMonitorItemStatus status)
        {
            switch (status)
            {
                case EtlPackageMonitorItemStatus.Succeeded:
                    return "greenbox";
                case EtlPackageMonitorItemStatus.Running:
                    return "bluebox";
                case EtlPackageMonitorItemStatus.Warn:
                    return "yellowbox";
                default:
                    return "redbox";
            }
        }

        #endregion
    }
}
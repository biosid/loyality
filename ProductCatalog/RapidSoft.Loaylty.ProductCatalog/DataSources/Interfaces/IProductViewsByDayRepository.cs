namespace RapidSoft.Loaylty.ProductCatalog.DataSources.Interfaces
{
    using System;
    using System.Collections.Generic;

    public interface IProductViewsByDayRepository
    {
        void SaveViews(DateTime date, KeyValuePair<string, int>[] views);
    }
}
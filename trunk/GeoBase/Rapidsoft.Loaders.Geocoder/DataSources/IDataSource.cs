using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RapidSoft.GeoPoints.Entities;
using RapidSoft.Loaders.Geocoder.Entities;

namespace RapidSoft.Loaders.Geocoder.DataSources
{
    public interface IDataSource
    {
        //string Name { get; }

        IEnumerable<ILocation> GetLocations();

        void UpdateGeoInfo(object key, LocationGeoInfo geoInfo);
    }
}

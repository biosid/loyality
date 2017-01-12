using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RapidSoft.Loaders.Geocoder.Logic
{
    public interface IYandexConfiguration
    {
        string YandexKey { get; }

        int RequestInterval { get; }
    }
}

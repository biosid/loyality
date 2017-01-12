using System.Collections.Generic;
using Vtb24.Site.Services.GeoService.Models;

namespace Vtb24.Site.Services.GeoService.Stubs
{
    public static class GeoServiceStubData
    {
        static GeoServiceStubData()
        {
            InitDefaultCity();
            InitRegions();
            InitCities();
        }

        public static GeoLocation DefaultCity { get; private set; }

        public static List<GeoLocation> Regions { get; private set; }

        public static List<GeoLocation> Cities { get; private set; } 


        private static void InitDefaultCity()
        {
            // именно так :)
            DefaultCity = new GeoLocation
            {
                Name = "Москва",
                KladrCode = "7700000000000",
                Toponym = "г",
                RegionName = "Москва",
                RegionToponym = "г",
                Type = GeoLocationType.City
            }; 
        }

        private static void InitRegions()
        {
            Regions = new List<GeoLocation>
            {
                // TODO: заполнить RegionName, RegionToponim
                new GeoLocation {Name = "Адыгея", KladrCode = "0100000000000", Toponym = "Респ", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Алтай", KladrCode = "0400000000000", Toponym = "Респ", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Алтайский", KladrCode = "2200000000000", Toponym = "край", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Амурская", KladrCode = "2800000000000", Toponym = "обл", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Архангельская", KladrCode = "2900000000000", Toponym = "обл", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Астраханская", KladrCode = "3000000000000", Toponym = "обл", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Байконур", KladrCode = "9900000000000", Toponym = "г", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Башкортостан", KladrCode = "0200000000000", Toponym = "Респ", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Белгородская", KladrCode = "3100000000000", Toponym = "обл", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Брянская", KladrCode = "3200000000000", Toponym = "обл", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Бурятия", KladrCode = "0300000000000", Toponym = "Респ", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Владимирская", KladrCode = "3300000000000", Toponym = "обл", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Волгоградская", KladrCode = "3400000000000", Toponym = "обл", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Вологодская", KladrCode = "3500000000000", Toponym = "обл", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Воронежская", KladrCode = "3600000000000", Toponym = "обл", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Дагестан", KladrCode = "0500000000000", Toponym = "Респ", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Еврейская", KladrCode = "7900000000000", Toponym = "Аобл", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Забайкальский", KladrCode = "7500000000000", Toponym = "край", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Ивановская", KladrCode = "3700000000000", Toponym = "обл", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Ингушетия", KladrCode = "0600000000000", Toponym = "Респ", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Иркутская", KladrCode = "3800000000000", Toponym = "обл", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Кабардино-Балкарская", KladrCode = "0700000000000", Toponym = "Респ", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Калининградская", KladrCode = "3900000000000", Toponym = "обл", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Калмыкия", KladrCode = "0800000000000", Toponym = "Респ", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Калужская", KladrCode = "4000000000000", Toponym = "обл", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Камчатский", KladrCode = "4100000000000", Toponym = "край", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Карачаево-Черкесская", KladrCode = "0900000000000", Toponym = "Респ", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Карелия", KladrCode = "1000000000000", Toponym = "Респ", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Кемеровская", KladrCode = "4200000000000", Toponym = "обл", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Кировская", KladrCode = "4300000000000", Toponym = "обл", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Коми", KladrCode = "1100000000000", Toponym = "Респ", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Костромская", KladrCode = "4400000000000", Toponym = "обл", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Краснодарский", KladrCode = "2300000000000", Toponym = "край", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Красноярский", KladrCode = "2400000000000", Toponym = "край", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Курганская", KladrCode = "4500000000000", Toponym = "обл", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Курская", KladrCode = "4600000000000", Toponym = "обл", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Ленинградская", KladrCode = "4700000000000", Toponym = "обл", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Липецкая", KladrCode = "4800000000000", Toponym = "обл", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Магаданская", KladrCode = "4900000000000", Toponym = "обл", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Марий Эл", KladrCode = "1200000000000", Toponym = "Респ", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Мордовия", KladrCode = "1300000000000", Toponym = "Респ", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Москва", KladrCode = "7700000000000", Toponym = "г", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Московская", KladrCode = "5000000000000", Toponym = "обл", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Мурманская", KladrCode = "5100000000000", Toponym = "обл", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Ненецкий", KladrCode = "8300000000000", Toponym = "АО", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Нижегородская", KladrCode = "5200000000000", Toponym = "обл", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Новгородская", KladrCode = "5300000000000", Toponym = "обл", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Новосибирская", KladrCode = "5400000000000", Toponym = "обл", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Омская", KladrCode = "5500000000000", Toponym = "обл", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Оренбургская", KladrCode = "5600000000000", Toponym = "обл", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Орловская", KladrCode = "5700000000000", Toponym = "обл", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Пензенская", KladrCode = "5800000000000", Toponym = "обл"},
                new GeoLocation {Name = "Пермский", KladrCode = "5900000000000", Toponym = "край", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Приморский", KladrCode = "2500000000000", Toponym = "край", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Псковская", KladrCode = "6000000000000", Toponym = "обл", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Ростовская", KladrCode = "6100000000000", Toponym = "обл", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Рязанская", KladrCode = "6200000000000", Toponym = "обл", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Самарская", KladrCode = "6300000000000", Toponym = "обл", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Санкт-Петербург", KladrCode = "7800000000000", Toponym = "г", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Саратовская", KladrCode = "6400000000000", Toponym = "обл", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Саха /Якутия/", KladrCode = "1400000000000", Toponym = "Респ", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Сахалинская", KladrCode = "6500000000000", Toponym = "обл", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Свердловская", KladrCode = "6600000000000", Toponym = "обл", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Северная Осетия - Алания", KladrCode = "1500000000000", Toponym = "Респ", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Смоленская", KladrCode = "6700000000000", Toponym = "обл", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Ставропольский", KladrCode = "2600000000000", Toponym = "край", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Тамбовская", KladrCode = "6800000000000", Toponym = "обл", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Татарстан", KladrCode = "1600000000000", Toponym = "Респ", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Тверская", KladrCode = "6900000000000", Toponym = "обл", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Томская", KladrCode = "7000000000000", Toponym = "обл", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Тульская", KladrCode = "7100000000000", Toponym = "обл", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Тыва", KladrCode = "1700000000000", Toponym = "Респ", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Тюменская", KladrCode = "7200000000000", Toponym = "обл", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Удмуртская", KladrCode = "1800000000000", Toponym = "Респ", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Ульяновская", KladrCode = "7300000000000", Toponym = "обл", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Хабаровский", KladrCode = "2700000000000", Toponym = "край", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Хакасия", KladrCode = "1900000000000", Toponym = "Респ", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Ханты-Мансийский Автономный округ - Югра", KladrCode = "8600000000000", Toponym = "АО", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Челябинская", KladrCode = "7400000000000", Toponym = "обл", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Чеченская", KladrCode = "2000000000000", Toponym = "Респ", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Чувашская Республика", KladrCode = "2100000000000", Toponym = "Чувашия", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Чукотский", KladrCode = "8700000000000", Toponym = "АО", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Ямало-Ненецкий", KladrCode = "8900000000000", Toponym = "АО", Type = GeoLocationType.Region},
                new GeoLocation {Name = "Ярославская", KladrCode = "7600000000000", Toponym = "обл", Type = GeoLocationType.Region}
            };
        }

        private static void InitCities()
        {
            Cities = new List<GeoLocation>
            {
                new GeoLocation { Name = "Зеленоград", KladrCode = "7700000200000", Toponym = "г", RegionName = "Москва", RegionToponym = "г", Type = GeoLocationType.City}, 
                new GeoLocation { Name = "Москва", KladrCode = "7700000000000", Toponym = "г", RegionName = "Москва", RegionToponym = "г", Type = GeoLocationType.City }, 
                new GeoLocation { Name = "Московский", KladrCode = "7701100100000", Toponym = "г", RegionName = "Москва", RegionToponym = "г", Type = GeoLocationType.City }, 
                new GeoLocation { Name = "Троицк", KladrCode = "7700000500000", Toponym = "г", RegionName = "Москва", RegionToponym = "г", Type = GeoLocationType.City }, 
                new GeoLocation { Name = "Щербинка", KladrCode = "7700000300000", Toponym = "г", RegionName = "Москва", RegionToponym = "г", Type = GeoLocationType.City }
            };
        }
    }
}
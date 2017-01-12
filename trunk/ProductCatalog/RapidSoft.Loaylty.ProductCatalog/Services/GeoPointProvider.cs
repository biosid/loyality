namespace RapidSoft.Loaylty.ProductCatalog.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using Extensions;

    using Fake;

    using GeoPoints.WsClients.GeoPointService;

    using Interfaces;

    using AddressElement = Kladr.Model.AddressElement;
    using AddressLevel = Kladr.Model.AddressLevel;
    using Country = API.Entities.Country;
    using KladrAddress = Kladr.Model.KladrAddress;

    public class GeoPointProvider : IGeoPointProvider
    {
        static GeoPointProvider()
        {
            Mapper.CreateMap<GeoPoints.WsClients.GeoPointService.Country, Country>()
                  .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Alpha2Code))
                  .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
            Mapper.CreateMap<GeoPoints.WsClients.GeoPointService.AddressElement, AddressElement>();
            Mapper.CreateMap<GeoPoints.WsClients.GeoPointService.KladrAddress, KladrAddress>();
        }

        public virtual KladrAddress GetAddressByKladrCode(string kladrCode)
        {
            kladrCode.ThrowIfNull("kladrCode");

            if (FakeDataConfigSection.Current != null &&
                FakeDataConfigSection.Current.FakeGeopoints != null &&
                FakeDataConfigSection.Current.FakeGeopoints.UseFake)
            {
                return new KladrAddress
                {
                    AddressLevel = AddressLevel.Region,
                    Region = new AddressElement()
                    {
                        Code = "7700000000000",
                        Prefix = "г",
                        Name = "Москва",
                        Level = AddressLevel.Region
                    }
                };
            }

            var result =
                WebClientCaller.CallService<GeoPointServiceClient, KladrAddressResult>(
                    s => s.GetAddressByKladrCode(kladrCode));

            if (!result.Success)
            {
                throw new OperationException("Не удалось получить информацию по адресу: " + result.ResultDescription, result.ResultCode);
            }

            var retVal = Mapper.Map<GeoPoints.WsClients.GeoPointService.KladrAddress, KladrAddress>(result.KladrAddress);

            return retVal;
        }

        public bool IsKladrCodeExists(string kladrCode)
        {
            kladrCode.ThrowIfNull("kladrCode");

            var param = kladrCode.MakeArray();
            var result =
                WebClientCaller.CallService<GeoPointServiceClient, CheckKladrCodeResult>(
                    s => s.GetExistKladrCodes(param));

            if (!result.Success)
            {
                throw new OperationException("Не удалось проверить существование кода КЛАДР: " + result.ResultDescription, result.ResultCode);
            }

            return result.ExistKladrCodes.Any(x => x == kladrCode);
        }

        public IList<string> GetExistKladrCodes(IList<string> codes)
        {
            codes.ThrowIfNull("codes");

            if (FakeDataConfigSection.Current != null &&
                FakeDataConfigSection.Current.FakeGeopoints != null &&
                FakeDataConfigSection.Current.FakeGeopoints.UseFake)
            {
                // NOTE: Все существуют кроме 1111111111111 и 1111111111112
                return codes.Where(x => x != "1111111111111" && x != "1111111111112").ToList();
            }

            if (codes.Count == 0)
            {
                return new List<string>(0);
            }

            var asArray = codes.ToArray();
            var result =
                WebClientCaller.CallService<GeoPointServiceClient, CheckKladrCodeResult>(
                    s => s.GetExistKladrCodes(asArray));

            if (!result.Success)
            {
                throw new OperationException("Не удалось проверить существование кодов КЛАДР: " + result.ResultDescription, result.ResultCode);
            }

            return result.ExistKladrCodes;
        }
    }
}
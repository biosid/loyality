namespace RapidSoft.Loaylty.ProductCatalog.Interfaces
{
    using System.Collections.Generic;

    using KladrAddress = RapidSoft.Kladr.Model.KladrAddress;

    public interface IGeoPointProvider
    {
        KladrAddress GetAddressByKladrCode(string kladrCode);

        bool IsKladrCodeExists(string kladrCode);

        IList<string> GetExistKladrCodes(IList<string> codes);
    }
}
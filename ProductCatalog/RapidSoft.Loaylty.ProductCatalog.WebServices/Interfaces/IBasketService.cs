namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Interfaces
{
    using System;
    using System.ServiceModel;

    using Monitoring;

    using RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Basket.Input;
    using RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Basket.Output;
    using RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog;
    using RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Output;

    [ServiceContract]
    public interface IBasketService : ISupportService
    {
        [OperationContract]
        ValueResult<Guid> Add(AddParameters parameters);

        [OperationContract]
        ValueResult<Guid> SetQuantity(SetQuantityParameters parameters);

        [OperationContract]
        ResultBase Remove(RemoveParameters parameters);

        [OperationContract]
        ValueResult<ClientItem> GetItem(GetItemParameters parameters);

        [OperationContract]
        BasketResult Get(GetParameters parameters);
    }
}

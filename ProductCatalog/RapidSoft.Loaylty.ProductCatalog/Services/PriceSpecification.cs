namespace RapidSoft.Loaylty.ProductCatalog.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using API.Entities;

    using Extensions;

    using Interfaces;

    using PromoAction.WsClients.MechanicsService;

    public class PriceSpecification : IPriceSpecification
    {
        public static void FillBasketItemPrice(BasketItem item, IMechanicsProvider mechanicsProvider, Dictionary<string, string> clientContext)
        {
            item.ThrowIfNull("item");

            if (item.FixedPrice != null)
            {
                var price = XmlSerializer.Deserialize<FixedPrice>(item.FixedPrice);
                var actualPrice = mechanicsProvider.CalculateProductPrice(clientContext, price.PriceRUR, item.Product);

                item.ItemPrice = actualPrice.PromoResult.Round();
                item.TotalPrice = item.ItemPrice * item.ProductsQuantity;
                item.TotalPriceRur = price.PriceRUR * item.ProductsQuantity;
            }
            else
            {
                item.TotalPrice = GetProductsBonusPrice(item.Product, item.ProductsQuantity);
                item.TotalPriceRur = item.Product.PriceRUR * item.ProductsQuantity;
                item.ItemPrice = GetProductsBonusPrice(item.Product, 1);
            }
        }

        public static void FillWishListItemPrice(API.OutputResults.WishListItem item)
        {
            item.ThrowIfNull("item");

            item.TotalPrice = GetProductsBonusPrice(item.Product, item.ProductsQuantity);
            item.ItemPrice = GetProductsBonusPrice(item.Product, 1);
        }

        public static decimal CalcBonusPrice(FactorsResult factors, decimal deliveryPrice)
        {
            var price = (((deliveryPrice * factors.BaseMultiplicationFactor) + factors.BaseAdditionFactor) * factors.MultiplicationFactor) + factors.AdditionFactor;
            return price.Round();
        }

        public static decimal GetProductsBonusPrice(Product product, int quantity)
        {
            product.ThrowIfNull("product");
            return product.Price * quantity;
        }

        public void FillOrderPrice(Order order, OrderItemPrice[] orderPrices, decimal deliveryCost, decimal bonusDeliveryCost, decimal deliveryAdvance, decimal itemsAdvance)
        {
            decimal orderItemsCost = 0;
            decimal orderBonusItemsCost = 0;
            
            foreach (var orderItem in order.Items)
            {
                var price = GetPrice(orderPrices, orderItem.Product.ProductId);

                // Calc order item prices
                orderItem.PriceRur = price.ProductPriceRur;
                orderItem.PriceBonus = price.ProductPrice;
                orderItem.AmountPriceRur = price.ProductPriceRur * orderItem.Amount;
                orderItem.AmountPriceBonus = price.ProductPrice * orderItem.Amount;
                
                orderItemsCost += orderItem.AmountPriceRur;
                orderBonusItemsCost += orderItem.AmountPriceBonus;
            }

            // Calc order prices
            order.ItemsCost = orderItemsCost;
            order.BonusItemsCost = orderBonusItemsCost; 

            order.DeliveryCost = deliveryCost;
            order.BonusDeliveryCost = bonusDeliveryCost;
            order.DeliveryAdvance = deliveryAdvance;
            order.ItemsAdvance = itemsAdvance;
            order.TotalAdvance = deliveryAdvance + itemsAdvance;

            order.TotalCost = order.ItemsCost + order.DeliveryCost;
            order.BonusTotalCost = (order.BonusItemsCost - (orderItemsCost != 0 ? orderBonusItemsCost * itemsAdvance / orderItemsCost : 0).Round()) + order.BonusDeliveryCost; // вычитаем из общей цены в бонусах оплаченный аванс
        }

        private static OrderItemPrice GetPrice(IEnumerable<OrderItemPrice> prices, string productId)
        {
            var price = prices.FirstOrDefault(i => i.ProductId == productId);

            if (price == null)
            {
                throw new InvalidOperationException(string.Format("Can not calc order price, deliveryPrice with productId:{0} not found", productId));
            }

            return price;
        }
    }
}
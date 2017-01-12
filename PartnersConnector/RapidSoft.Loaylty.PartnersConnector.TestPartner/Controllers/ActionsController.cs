using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;

using RapidSoft.Extensions;

namespace RapidSoft.Loaylty.PartnersConnector.TestPartner.Controllers
{
    using Common.DTO;
    using Common.DTO.CheckOrder;
    using Common.DTO.CommitOrder;
    using Common.DTO.GetDeliveryVariants;
    using Common.Services;

    public class ActionsController : Controller
    {
        #region Data
        
        private const string CheckOrderErrorXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<CheckOrderResult xmlns=""http://tempuri.org/XMLSchema.xsd"">
  <Checked>0</Checked>
  <Reason>{0}</Reason>
</CheckOrderResult>";

        private const string FixBasketItemPriceErrorXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<FixPriceResult xmlns=""http://tempuri.org/XMLSchema.xsd"">
  <ActualPrice>0</ActualPrice>
  <Confirmed>0</Confirmed>
  <Reason>Не удалось зафиксировать цену товара</Reason>
</FixPriceResult>";

        private const string FixBasketItemPriceOkXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<FixPriceResult xmlns=""http://tempuri.org/XMLSchema.xsd"">
  <ActualPrice>8</ActualPrice>
  <UtcDateTime>2013-01-15T12:30:00</UtcDateTime>
  <Confirmed>1</Confirmed>
</FixPriceResult>";

        private const string CheckOrderOkXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<CheckOrderResult xmlns=""http://tempuri.org/XMLSchema.xsd"">
  <Checked>1</Checked>
</CheckOrderResult>";

        private const string ConfirmOrderErrorXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<CommitOrderResult xmlns=""http://tempuri.org/XMLSchema.xsd"">
  <InternalOrderId>{1}</InternalOrderId>
  <Confirmed>0</Confirmed>
<Reason>{0}</Reason>
</CommitOrderResult>";
        private const string ConfirmOrderOkXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<CommitOrderResult xmlns=""http://tempuri.org/XMLSchema.xsd"">
    <InternalOrderId>{0}</InternalOrderId>
    <Confirmed>1</Confirmed>
</CommitOrderResult>";

        private const string OrderXml = @"<Order>
      <OrderId>{0}</OrderId>
      <InternalOrderId>{0}</InternalOrderId>
      <Confirmed>1</Confirmed>
    </Order>";
        private const string OrderErrorXml = @"<Order>
      <OrderId>{0}</OrderId>
      <InternalOrderId>{0}</InternalOrderId>
      <Confirmed>0</Confirmed>
      <Reason>{1}</Reason>
    </Order>";

        private const string GetDeliveryVariantsErrorXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<GetDeliveryVariantsResult xmlns=""http://tempuri.org/XMLSchema.xsd"">
    <ResultCode>1</ResultCode>
    <Reason>{0}</Reason>
</GetDeliveryVariantsResult>";

        private const string GetDeliveryVariantsEmptyResponce = @"<?xml version=""1.0"" encoding=""utf-8""?>
<GetDeliveryVariantsResult xmlns=""http://tempuri.org/XMLSchema.xsd"">
  <ResultCode>0</ResultCode>  
  <Location>
    <LocationName>г. Москва</LocationName>
    <PostCode>105187</PostCode>
    <ExternalLocationId>FakePartnerExternalLocationId</ExternalLocationId>
  </Location>
</GetDeliveryVariantsResult>";

        private const string GetDeliveryVariantsOkResponce = @"<?xml version=""1.0"" encoding=""utf-8""?>
<GetDeliveryVariantsResult xmlns=""http://tempuri.org/XMLSchema.xsd"">
  <ResultCode>0</ResultCode>  
  <Location>
    <LocationName>г. Москва</LocationName>
    <PostCode>105187</PostCode>
    <ExternalLocationId>FakePartnerExternalLocationId</ExternalLocationId>
  </Location>
  <DeliveryGroups>
    <DeliveryGroup>
      <GroupName>Курьерская доставка</GroupName>
      <DeliveryVariants>
        <DeliveryVariant>
          <DeliveryVariantName>Обычная</DeliveryVariantName>
          <ExternalDeliveryVariantId>123</ExternalDeliveryVariantId>
          <Description>Скрок доставки 3 рабочих дня</Description>
          <ItemsCost>2400</ItemsCost>
          <DeliveryCost>100</DeliveryCost>
          <TotalCost>2500</TotalCost>
        </DeliveryVariant>
        <DeliveryVariant>
          <DeliveryVariantName>Медленная</DeliveryVariantName>
          <ExternalDeliveryVariantId>124</ExternalDeliveryVariantId>
          <Description>Скрок доставки от двух недель, в зависимости от времени года</Description>
          <ItemsCost>2400</ItemsCost>
          <DeliveryCost>10</DeliveryCost>
          <TotalCost>2410</TotalCost>
        </DeliveryVariant>
      </DeliveryVariants>
    </DeliveryGroup>
    <DeliveryGroup>
      <GroupName>Пункты самовывоза</GroupName>
      <DeliveryVariants>
        <DeliveryVariant>
          <DeliveryVariantName>Экспесс самовывоз</DeliveryVariantName>
          <ExternalDeliveryVariantId>126</ExternalDeliveryVariantId>
          <PickupPoints>
            <PickupPoint>
              <Name>г. Москва, ул. Победы 5. MYCOMPANY</Name>
              <ExternalPickupPointId>234234234</ExternalPickupPointId>
              <DeliveryVariantName>Экспесс самовывоз</DeliveryVariantName>
              <ExternalDeliveryVariantId>126</ExternalDeliveryVariantId>
              <Address>г. Москва, ул. Победы 5</Address>
              <Phones>
                <Phone>7 916 321 54 76</Phone>
                <Phone>7 495 876 55 55 доб. 5</Phone>
              </Phones>
              <OperatingHours>
                <OperatingHour>пн-вс с 9:00 по 23:00</OperatingHour>
              </OperatingHours>
              <Description>Срок доставки: 3 дня, вы будете оповещены по SMS Срок хранения: неделя</Description>
              <ItemsCost>2400</ItemsCost>
              <DeliveryCost>1000</DeliveryCost>
              <TotalCost>3400</TotalCost>
            </PickupPoint>
          </PickupPoints>
          <ItemsCost>2400</ItemsCost>
          <DeliveryCost>1000</DeliveryCost>
          <TotalCost>3400</TotalCost>
        </DeliveryVariant>
      </DeliveryVariants>
    </DeliveryGroup>
  </DeliveryGroups>
</GetDeliveryVariantsResult>";

        #endregion

        [HttpPost]
        public ContentResult GetDeliveryVariants()
        {
            try
            {
                var body = this.ReadBody();

                var request = body.Deserialize<GetDeliveryVariantsMessage>(Encoding.UTF8);

                MessageValidator.Validate(request);

                if (request.Location.PostCode == "000000")
                {
                    return this.Content(GetDeliveryVariantsEmptyResponce, "text/xml");
                }
                else
                {
                    return this.Content(GetDeliveryVariantsOkResponce, "text/xml");
                }
            }
            catch (AssertException ex)
            {
                var retXml = string.Format(GetDeliveryVariantsErrorXml, this.FunEscape(ex.Message));

               return this.Content(retXml, "text/xml");
            }
            catch (Exception ex)
            {
                var retXml = string.Format(GetDeliveryVariantsErrorXml, this.FunEscape(ex.ToString()));

                return this.Content(retXml, "text/xml");
            }
        }

        [HttpPost]
        public ContentResult FixPrice()
        {
            try
            {
                var body = this.ReadBody();
                this.ValidateFixBasketItemPriceRequest(body);
                return this.Content(FixBasketItemPriceOkXml, "text/xml");
            }
            catch (AssertException ex)
            {
                var retXml = string.Format(FixBasketItemPriceErrorXml, this.FunEscape(ex.Message));

               return this.Content(retXml, "text/xml");
            }
            catch (Exception ex)
            {
                var retXml = string.Format(FixBasketItemPriceErrorXml, this.FunEscape(ex.ToString()));

                return this.Content(retXml, "text/xml");
            }
        }

        [HttpPost]
        public ContentResult CheckOrder()
        {
            try
            {
                var body = this.ReadBody();
                this.ValidateCheckOrderRequest(body);
                return this.Content(CheckOrderOkXml, "text/xml");
            }
            catch (AssertException ex)
            {
                var retXml = string.Format(CheckOrderErrorXml, this.FunEscape(ex.Message));

               return this.Content(retXml, "text/xml");
            }
            catch (Exception ex)
            {
                var retXml = string.Format(CheckOrderErrorXml, this.FunEscape(ex.ToString()));

                return this.Content(retXml, "text/xml");
            }
        }

        [HttpPost]
        public ContentResult ConfirmOrder()
        {
            string orderId = null;
            try
            {
                var body = this.ReadBody();
                this.ValidateCommitOrderRequest(body, out orderId);
                var retXml = string.Format(ConfirmOrderOkXml, this.FunEscape(orderId));
                return this.Content(retXml, "text/xml");
            }
            catch (AssertException ex)
            {
                var retXml = string.Format(ConfirmOrderErrorXml, this.FunEscape(ex.Message), orderId);
                return this.Content(retXml, "text/xml");
            }
            catch (Exception ex)
            {
                var retXml = string.Format(ConfirmOrderErrorXml, this.FunEscape(ex.ToString()), orderId);
                return this.Content(retXml, "text/xml");
            }
        }

        [HttpPost]
        public ContentResult BatchConfirmOrder()
        {
            Common.DTO.CommitOrder.Order[] orders;
            try
            {
                var body = this.ReadBody();

                this.ValidateCommitOrdersRequest(body, out orders);
            }
            catch (AssertException)
            {
                // TODO: А как сообщить о том что проблема не в данныъх заказа, а вообще присланном содержании.
                throw;
            }
            catch (Exception)
            {
                // TODO: А как сообщить о том что проблема не в данныъх заказа, а вообще присланном содержании.
                throw;
            }

            var sb = new StringBuilder();

            sb.Append(@"<?xml version=""1.0"" encoding=""utf-8""?>
<CommitOrdersResult xmlns=""http://tempuri.org/XMLSchema.xsd"">
  <Orders>");
  
            foreach (var order in orders)
            {
                string orderId = null;
                try
                {
                    sb.AppendFormat(OrderXml, orderId);
                }
                catch (AssertException ex)
                {
                    sb.AppendFormat(OrderErrorXml, orderId, ex.Message);
                }
                catch (Exception ex)
                {
                    sb.AppendFormat(OrderErrorXml, orderId, ex.Message);
                }
            }

            sb.Append(@"  </Orders>
</CommitOrdersResult>");
            return this.Content(sb.ToString(), "text/xml");
        }

        public ContentResult Ping()
        {
            var data = this.ReadBody();

            return this.Content(data, "text/xml");
        }

        public string FunEscape(string input)
        {
            return
                input.Serialize(Encoding.UTF8)
                     .Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<string>", string.Empty)
                     .Replace("</string>", string.Empty);
        }

        private void ValidateFixBasketItemPriceRequest(string body)
        {
            var message = body.Deserialize<FixPriceMessage>(Encoding.UTF8);

            MessageValidator.Validate(message);

            this.Assert(message.BasketItemId != null, "BasketItemId is null");
            this.Assert(message.ClientId != null, "ClientId is null");
            this.Assert(message.OfferId != null, "OfferId is null");
            this.Assert(message.OfferName != null, "OfferName is null");
            this.Assert(message.Price != 0, "Price less than 0");
            this.Assert(message.Amount != 0, "Amount less than 0");            
        }

        private void ValidateCheckOrderRequest(string body)
        {
            var message = body.Deserialize<CheckOrderMessage>(Encoding.UTF8);

            MessageValidator.Validate(message);

            this.Assert(message.Order != null, "Запрос должен иметь один элемент \"Order\"");
        }

        private void ValidateCommitOrderRequest(string body, out string orderId)
        {
            var message = body.Deserialize<CommitOrderMessage>(Encoding.UTF8);

            MessageValidator.Validate(message);

            orderId = message.Order.OrderId;

            this.Assert(message.Order != null, "Запрос должен иметь один элемент \"Order\"");
        }

        private void ValidateCommitOrdersRequest(string body, out Common.DTO.CommitOrder.Order[] orders)
        {
            var message = body.Deserialize<CommitOrdersMessage>(Encoding.UTF8);

            MessageValidator.Validate(message);

            this.Assert(message.Orders != null, "Пакетное подтверждение заказов должно иметь заказы");

            this.Assert(message.Orders.Any(), "Пакетное подтверждение заказов должно иметь как минимум один заказ");

            orders = message.Orders;
        }

        public string ReadBody()
        {
            Request.InputStream.Position = 0;
            using (var ms = new MemoryStream(HttpContext.Request.BinaryRead(HttpContext.Request.ContentLength)))
            {
                ms.Position = 0;
                using (var sr = new StreamReader(ms))
                {
                    var myStr = sr.ReadToEnd();
                    return myStr;
                }
            }
        }

        private void Assert(bool condition, string description)
        {
            if (!condition)
            {
                throw new AssertException(description);
            }
        }             
    }
}

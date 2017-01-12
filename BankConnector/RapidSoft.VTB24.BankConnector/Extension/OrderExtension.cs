namespace RapidSoft.VTB24.BankConnector.Extension
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using RapidSoft.Loaylty.ProductCatalog.WsClients.CatalogAdminService;
    using RapidSoft.VTB24.BankConnector.DataModels;
    using RapidSoft.VTB24.BankConnector.Infrastructure.Configuration;

    using DeliveryInfo = RapidSoft.Loaylty.ProductCatalog.WsClients.OrderManagementService.DeliveryInfo;
    using DeliveryTypes = RapidSoft.Loaylty.ProductCatalog.WsClients.OrderManagementService.DeliveryTypes;
    using Order = RapidSoft.Loaylty.ProductCatalog.WsClients.OrderManagementService.Order;
    using PartnerType = RapidSoft.Loaylty.ProductCatalog.WsClients.CatalogAdminService.PartnerType;

    public static class OrderExtension
    {
        public static void Validate(this Order order, Partner partner)
        {
            var errors = new List<Tuple<string, string>>();
            
            // Проверка полей заказа
            if (!string.IsNullOrEmpty(order.ExternalOrderId) && order.ExternalOrderId.Length > FieldLenght.ExternalOrderIdMaxLen)
            {
                errors.Add(new Tuple<string, string>("order.ExternalOrderId", string.Format("более {0} символов", FieldLenght.ExternalOrderIdMaxLen)));
            }

            if (!string.IsNullOrEmpty(order.ExternalOrderId) && order.ExternalOrderId.Length > FieldLenght.ExternalOrderIdMaxLen)
            {
                errors.Add(new Tuple<string, string>("order.ExternalOrderId", string.Format("более {0} символов", FieldLenght.ExternalOrderIdMaxLen)));
            }

            if (string.IsNullOrEmpty(order.ClientId))
            {
                errors.Add(new Tuple<string, string>("order.ClientId", "В заказе должен быть передан идентификатор клиента"));
            }
            else if (order.ClientId.Length > FieldLenght.ClientIdMaxLen)
            {
                errors.Add(new Tuple<string, string>("order.ClientId", string.Format("более {0} символов", FieldLenght.ClientIdMaxLen)));
            }

            if (order.Items == null || order.Items.Length == 0)
            {
                errors.Add(new Tuple<string, string>("Items", "В заказе должен находиться список позиций"));
            }
            else
            {
                if (!string.IsNullOrEmpty(order.Items[0].Product.ProductId) && order.Items[0].Product.ProductId.Length > FieldLenght.ProductIdMaxLen)
                {
                    errors.Add(new Tuple<string, string>("Items[0].Product.ProductId", string.Format("более {0} символов", FieldLenght.ProductIdMaxLen)));
                }

                // Проверка полей товара
                if (string.IsNullOrEmpty(order.Items[0].Product.Name))
                {
                    errors.Add(new Tuple<string, string>("Items[0].Product.Name", "В позиции заказа должно быть указано наименование продукта"));
                }
                else if (order.Items[0].Product.Name.Length > FieldLenght.ProductNameMaxLen)
                {
                    errors.Add(new Tuple<string, string>("Items[0].Product.Name", string.Format("более {0} символов", FieldLenght.ProductNameMaxLen)));
                }
            }

            if ((partner.Type == PartnerType.Direct || partner.Type == PartnerType.Offline) &&
                partner.Id != ConfigHelper.BankProductsPartnerId)
            {
                var isOnlineDeliveryVariantsSupported = partner.IsOnlineDeliveryVariantsSupported();

                ValidateDeliveryInfo(order.DeliveryInfo, isOnlineDeliveryVariantsSupported, errors);
            }

            if (errors.Any())
            {
                var description = new StringBuilder();
                description.AppendLine(string.Format("Неверный заказ ({0}):", order.Id));
                foreach (var keyValuePair in errors)
                {
                    description.AppendLine(string.Format("поле:{0}, ошибка {1}", keyValuePair.Item1, keyValuePair.Item2));
                }

                throw new Exception(description.ToString());
            }
        }

        private static void ValidateDeliveryInfo(DeliveryInfo deliveryInfo, bool isOnlineDeliveryVariantsSupported, List<Tuple<string, string>> errors)
        {
            if (deliveryInfo == null)
            {
                errors.Add(new Tuple<string, string>("order.DeliveryInfo", string.Format("order.DeliveryInfo должно быть заполнено для заказов")));
                return;
            }

            if (deliveryInfo.DeliveryType == DeliveryTypes.Delivery)
            {
                var hasRegionTitle = !string.IsNullOrEmpty(deliveryInfo.Address.RegionTitle);

                if (!isOnlineDeliveryVariantsSupported && !hasRegionTitle)
                {
                    errors.Add(new Tuple<string, string>("DeliveryInfo.RegionTitle", "В данных заказа должен быть заполнен регион доставки (партнер не поддерживает получение вариантов доставки)"));
                }
                else if (hasRegionTitle && deliveryInfo.Address.RegionTitle.Length > FieldLenght.DeliveryRegionMaxLen)
                {
                    errors.Add(new Tuple<string, string>("order.DeliveryInfo.RegionTitle", string.Format("более {0} символов", FieldLenght.DeliveryRegionMaxLen)));
                }

                if (!string.IsNullOrEmpty(deliveryInfo.Address.CityTitle) && deliveryInfo.Address.CityTitle.Length > FieldLenght.DeliveryCityMaxLen)
                {
                    errors.Add(new Tuple<string, string>("order.DeliveryInfo.CityTitle", string.Format("более {0} символов", FieldLenght.DeliveryCityMaxLen)));
                }

                if (string.IsNullOrEmpty(deliveryInfo.Address.AddressText))
                {
                    errors.Add(new Tuple<string, string>("DeliveryInfo.AddressText", "В данных заказа должен быть заполнен адрес доставки"));
                }
                else if (deliveryInfo.Address.AddressText.Length > FieldLenght.DeliveryAddressMaxLen)
                {
                    errors.Add(new Tuple<string, string>("order.DeliveryInfo.AddressText", string.Format("более {0} символов", FieldLenght.DeliveryAddressMaxLen)));
                }
            }
            else if (deliveryInfo.DeliveryType == DeliveryTypes.Pickup)
            {
                if (deliveryInfo.PickupPoint == null)
                {
                    errors.Add(new Tuple<string, string>("DeliveryInfo.PickupPoint", "Для DeliveryTypes.PickUp должен быть заполнен deliveryInfo.PickupPoint"));
                }
                else if (string.IsNullOrEmpty(deliveryInfo.PickupPoint.Address))
                {
                    errors.Add(new Tuple<string, string>("DeliveryInfo.PickupPoint", "Для DeliveryTypes.PickUp должен быть заполнен deliveryInfo.PickupPoint.Address"));
                }                
            }
            else if (deliveryInfo.DeliveryType == DeliveryTypes.Email)
            {
                if (deliveryInfo.Contact == null)
                {
                    errors.Add(new Tuple<string, string>("DeliveryInfo.Contact", "Для DeliveryTypes.Email должен быть заполнен deliveryInfo.Contact"));
                }
                else if (string.IsNullOrEmpty(deliveryInfo.Contact.Email))
                {
                    errors.Add(new Tuple<string, string>("DeliveryInfo.Contact.Email", "Для DeliveryTypes.Email должен быть заполнен deliveryInfo.Contact.Email"));
                }
            }
            else
            {
                throw new NotSupportedException(string.Format("deliveryInfo.DeliveryType {0} not supported", deliveryInfo.DeliveryType));
            }

            if (deliveryInfo.Contact == null)
            {
                errors.Add(new Tuple<string, string>("DeliveryInfo.Contact", "В данных заказа должен быть указан получатель"));
            }
            else
            {
                var contact = deliveryInfo.Contact;

                var contactName = string.Format("{0} {1} {2}", contact.LastName, contact.FirstName, contact.MiddleName);

                if (string.IsNullOrEmpty(contactName))
                {
                    errors.Add(new Tuple<string, string>("DeliveryInfo.Contact.LastName", "В данных заказа должна быть указана фамилия получателя"));
                }
                else if (contactName.Length > FieldLenght.ContactNameMaxLen)
                {
                    errors.Add(new Tuple<string, string>("contactName", string.Format("более {0} символов", FieldLenght.ContactNameMaxLen)));
                }

                var contactPhone = Entity.PhoneNumber.FromServicePhone(contact.Phone).GlobalNumber;

                if (contactPhone == null)
                {
                    errors.Add(new Tuple<string, string>("DeliveryInfo.Contact.Phone", "В данных заказа должен быть указан телефон получателя"));
                }
                else if (contactPhone.Length > FieldLenght.ContactPhoneMaxLen)
                {
                    errors.Add(new Tuple<string, string>("contact.Phone", string.Format("более {0} символов", FieldLenght.ContactPhoneMaxLen)));
                }
            }
        }
    }
}

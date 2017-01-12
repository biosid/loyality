namespace RapidSoft.Loaylty.ProductCatalog
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using API.Entities;

    using Extensions;

    using Kladr.Model;

    /// <summary>
    ///     The order extensions.
    /// </summary>
    internal static class OrderExtensions
    {
        #region Methods

        private const string Separator = ", ";
        
        /// <summary>
        /// The get client order status.
        /// </summary>
        /// <param name="order">
        /// The order.
        /// </param>
        /// <returns>
        /// The <see cref="PublicOrderStatuses"/>.
        /// </returns>
        public static PublicOrderStatuses GetClientOrderStatus(this Order order)
        {
            order.ThrowIfNull("order");
            OrderStatuses status = order.Status;
            switch (status)
            {
                case OrderStatuses.Draft:
                case OrderStatuses.Registration:
                {
                    return PublicOrderStatuses.Registration;
                }

                case OrderStatuses.Processing:
                {
                    return PublicOrderStatuses.Processing;
                }

                case OrderStatuses.DeliveryWaiting:
                {
                    return PublicOrderStatuses.DeliveryWaiting;
                }

                case OrderStatuses.Delivery:
                {
                    return PublicOrderStatuses.Delivery;
                }

                case OrderStatuses.Delivered:
                case OrderStatuses.DeliveredWithDelay:
                {
                    return PublicOrderStatuses.Delivered;
                }

                case OrderStatuses.CancelledByPartner:
                {
                    return PublicOrderStatuses.Cancelled;
                }

                case OrderStatuses.NotDelivered:
                {
                    return PublicOrderStatuses.NotDelivered;
                }

                default:
                {
                    throw new NotSupportedException(string.Format("Статус {0} не поддерживается", status));
                }
            }
        }

        /// <summary>
        /// The get order statuses.
        /// </summary>
        /// <param name="publicOrderStatus">
        /// The public order status.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        public static IEnumerable<OrderStatuses> GetOrderStatuses(this PublicOrderStatuses publicOrderStatus)
        {
            switch (publicOrderStatus)
            {
                case PublicOrderStatuses.Registration:
                {
                    yield return OrderStatuses.Draft;
                    yield return OrderStatuses.Registration;
                    yield break;
                }

                case PublicOrderStatuses.Processing:
                {
                    yield return OrderStatuses.Processing;
                    yield break;
                }

                case PublicOrderStatuses.DeliveryWaiting:
                {
                    yield return OrderStatuses.DeliveryWaiting;
                    yield break;
                }

                case PublicOrderStatuses.Delivery:
                {
                    yield return OrderStatuses.Delivery;
                    yield break;
                }

                case PublicOrderStatuses.Delivered:
                {
                    yield return OrderStatuses.Delivered;
                    yield return OrderStatuses.DeliveredWithDelay;
                    yield break;
                }

                case PublicOrderStatuses.Cancelled:
                {
                    yield return OrderStatuses.CancelledByPartner;
                    yield break;
                }

                case PublicOrderStatuses.NotDelivered:
                {
                    yield return OrderStatuses.NotDelivered;
                    yield break;
                }

                default:
                {
                    throw new NotSupportedException(string.Format("Статус {0} не поддерживается", publicOrderStatus));
                }
            }
        }

        /// <summary>
        /// The reset titles.
        /// </summary>
        /// <param name="deliveryAddress">
        /// The delivery info.
        /// </param>
        /// <param name="kladrAddress">
        /// The kladr address.
        /// </param>
        public static void ResetTitles(this DeliveryAddress deliveryAddress, KladrAddress kladrAddress)
        {
            deliveryAddress.RegionTitle = kladrAddress.Region != null
                ? AddressStringConverter.GetAddressText(kladrAddress.Region, true, true)
                : deliveryAddress.RegionTitle;
            deliveryAddress.RegionKladrCode = kladrAddress.Region.Maybe(r => r.Code);

            deliveryAddress.DistrictTitle = kladrAddress.District != null
                ? AddressStringConverter.GetAddressText(kladrAddress.District, true, true)
                : deliveryAddress.DistrictTitle;
            deliveryAddress.DistrictKladrCode = kladrAddress.District.Maybe(r => r.Code);

            deliveryAddress.CityTitle = kladrAddress.City != null
                ? AddressStringConverter.GetAddressText(kladrAddress.City, true, true)
                : deliveryAddress.CityTitle;
            deliveryAddress.CityKladrCode = kladrAddress.City.Maybe(r => r.Code);

            deliveryAddress.TownTitle = kladrAddress.Town != null
                ? AddressStringConverter.GetAddressText(kladrAddress.Town, true, true)
                : deliveryAddress.TownTitle;
            deliveryAddress.TownKladrCode = kladrAddress.Town.Maybe(r => r.Code);
        }

        /// <summary>
        /// The build address text.
        /// </summary>
        /// <param name="deliveryAddress">
        /// The delivery info.
        /// </param>
        /// <param name="kladrAddress">
        /// The kladr address.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string BuildAddressText(this DeliveryAddress deliveryAddress, KladrAddress kladrAddress)
        {
            if (kladrAddress == null)
            {
                throw new ArgumentNullException("kladrAddress");
            }

            var address = new StringBuilder();

            // Возвращает полный адресс указанного уровня
            address.Append(kladrAddress.FullText).Append(Separator);

            switch (kladrAddress.AddressLevel)
            {
                case AddressLevel.Town:
                    break;
                case AddressLevel.City:
                {
                    address.AppendIfNotEmpty(deliveryAddress.TownTitle);
                    break;
                }

                case AddressLevel.District:
                {
                    address.AppendIfNotEmpty(deliveryAddress.CityTitle);
                    address.AppendIfNotEmpty(deliveryAddress.TownTitle);
                    break;
                }

                case AddressLevel.Region:
                {
                    address.AppendIfNotEmpty(deliveryAddress.DistrictTitle);
                    address.AppendIfNotEmpty(deliveryAddress.CityTitle);
                    address.AppendIfNotEmpty(deliveryAddress.TownTitle);
                    break;
                }

                default:
                {
                    throw new NotSupportedException(
                        string.Format("Уровень КЛАДР кода {0} не поддерживается", kladrAddress.AddressLevel));
                }
            }

            address.AppendIfNotEmpty(deliveryAddress.StreetTitle);
            address.AppendIfNotEmpty(deliveryAddress.House);
            address.AppendIfNotEmpty(deliveryAddress.Flat);

            string fullAddress = address.ToString(0, address.Length - 2);
            return fullAddress;
        }

        /// <summary>
        /// The build address text.
        /// </summary>
        /// <param name="deliveryAddress">
        /// The delivery info.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string BuildAddressText(this DeliveryAddress deliveryAddress)
        {        
            var address = new StringBuilder();

            address.AppendIfNotEmpty(deliveryAddress.DistrictTitle);
            address.AppendIfNotEmpty(deliveryAddress.CityTitle);
            address.AppendIfNotEmpty(deliveryAddress.TownTitle);
            address.AppendIfNotEmpty(deliveryAddress.StreetTitle);
            address.AppendIfNotEmpty(deliveryAddress.House);
            address.AppendIfNotEmpty(deliveryAddress.Flat);

            string fullAddress = address.ToString(0, address.Length - 2);
            return fullAddress;
        }

        /// <summary>
        /// The append if not is null or white space.
        /// </summary>
        /// <param name="stringBuilder">
        ///     The string builder.
        /// </param>
        /// <param name="check">
        ///     The check.
        /// </param>
        /// <returns>
        /// The <see cref="StringBuilder"/>.
        /// </returns>
        private static StringBuilder AppendIfNotEmpty(this StringBuilder stringBuilder, string check)
        {
            if (!string.IsNullOrWhiteSpace(check))
            {
                stringBuilder.Append(check);
                stringBuilder.Append(Separator);
            }

            return stringBuilder;
        }

        #endregion
    }
}
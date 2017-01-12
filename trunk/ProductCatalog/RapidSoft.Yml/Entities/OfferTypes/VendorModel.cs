using System;

namespace RapidSoft.YML.Entities.OfferTypes
{
    /// <summary>
    /// Класс, представляющий тип предложения vendor.model (произвольный товар) 
    /// </summary>
    public class VendorModel : BaseOffer
    {
        /// <summary>
        /// Группа товаров \ категория
        /// </summary>
        public string TypePrefix { get; set; }

        /// <summary>
        /// Производитель
        /// </summary>
        public string Vendor { get; set; }

        /// <summary>
        /// Код товара (указывается код производителя)
        /// </summary>
        public string VendorCode { get; set; }

        /// <summary>
        /// Модель
        /// </summary>
        public string Model { get; set; }

        public override string DisplayName
        {
            get
            {
                var vendorModel = String.Format("{0} {1}", Vendor, Model);
                return String.IsNullOrEmpty(TypePrefix)
                           ? vendorModel
                           : String.Format("{0} {1}", TypePrefix, vendorModel);
            }
        }
    }
}
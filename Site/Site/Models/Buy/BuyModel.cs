using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Vtb24.Site.Models.Basket;
using Vtb24.Site.Services.GiftShop.Catalog.Models;

namespace Vtb24.Site.Models.Buy
{
    public class BuyModel
    {
        public string[] BasketItemIds { get; set; }

        public string[] ProductIds { get; set; }

        public int PartnerId { get; set; }

        public ContactModel Contact { get; set; }

        [Required(ErrorMessage = "Не выбран способ доставки")]
        public string DeliveryVariantId { get; set; }

        [Required(ErrorMessage = "Не выбран способ доставки")]
        public int DeliveryType { get; set; }

        public string PickupPointId { get; set; }

        public DeliveryAddressModel Address { get; set; }

        [AllowHtml]
        [StringLength(500, ErrorMessage = "Превышена допустимая длина дополнительной информации (500 символов)")]
        public string Comment { get; set; }

        public int BonusPayment { get; set; }

        public decimal AdvancePayment { get; set; }

        #region Справочные поля

        public bool ShowItemsWarning { get; set; }

        public bool DisableButton { get; set; }

        public int Balance { get; set; }
        
        public BasketItemModel[] Items { get; set; }

        public bool ShowFormalOffer { get { return !string.IsNullOrWhiteSpace(FormalOfferUrl); } }

        public string FormalOfferUrl { get; set; }

        public bool OnlineDeliveryVariantsEnabled { get; set; }

        public bool IsEmailRequired { get; set; }

        public bool ProposeToSaveEmail { get; set; }

        public AdvancePaymentSupportMode AdvancePaymentSupport { get; set; }

        public int MaxAdvanceFraction { get; set; }

        public bool ShowSavedAddresses { get { return SavedAddresses != null && SavedAddresses.Any(); } }

        public DeliveryAddressModel[] SavedAddresses { get; set; }

        public SelectListItem[] Regions { get; set; }

        public DeliveryVariantsModel Variants { get; set; }

        public decimal ItemsPriceBonus { get; set; }

        public decimal ItemsPriceRur { get; set; }

        #endregion
    }
}
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Vtb24.Site.Services.GiftShop.Orders.Models;

namespace Vtb24.Site.Models.Buy
{
    public class DeliveryAddressModel
    {
        public string Location { get; set; }

        [Required(ErrorMessage = "Укажите населённый пункт")]
        public string LocationKladr { get; set; }

        public string Region { get; set; }

        public string RegionKladr { get; set; }

        [Required(ErrorMessage = "Укажите почтовый индекс")]
        [RegularExpression("\\d{6}", ErrorMessage = "Почтовый индекс должен состоять из 6 цифр")]
        public string PostCode { get; set; }

        [Required(ErrorMessage = "Укажите улицу")]
        [StringLength(50, ErrorMessage = "Превышена допустимая длина улицы (50 символов)")]
        public string Street { get; set; }

        [Required(ErrorMessage = "Укажите номер дома")]
        [StringLength(20, ErrorMessage = "Превышена допустимая длина номера дома (20 символов)")]
        public string House { get; set; }

        [StringLength(10, ErrorMessage = "Превышена допустимая длина квартиры (10 символов)")]
        public string Flat { get; set; }
 
        public string Title
        {
            get
            {
                var buff = new List<string>();

                if (!string.IsNullOrWhiteSpace(Street))
                {
                    buff.Add(Street);
                }

                if (!string.IsNullOrWhiteSpace(House))
                {
                    buff.Add("д." + House);
                }

                if (!string.IsNullOrWhiteSpace(Flat))
                {
                    buff.Add("к." + Flat);    
                }

                if (!string.IsNullOrWhiteSpace(Location))
                {
                    buff.Add(Location);
                }

                return string.Join(", ", buff);
            }
        }

        public static DeliveryAddressModel Map(DeliveryAddress original, string kladr)
        {
            if (original == null)
            {
                return null;
            }

            string location;
            if (!string.IsNullOrEmpty(original.Town))
            {
                location = original.Town;
            }
            else if (!string.IsNullOrEmpty(original.City))
            {
                location = original.City;
            }
            else
            {
                location = original.Region;
            }

            var model = new DeliveryAddressModel
            {
                Region = original.Region,
                RegionKladr = string.IsNullOrEmpty(kladr) ? null : kladr.Substring(0, 2) + "00000000000",
                Location = location,
                LocationKladr = kladr,
                PostCode = original.PostCode,
                Street = original.Street,
                House = original.House,
                Flat = original.Flat
            };

            return model;
        }
    }
}
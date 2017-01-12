using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Vtb24.Arms.Areas.Catalog.Models.Delivery
{
    public class UpdateBindingModel
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Укажите регион, район или город")]
        public string KladrCode { get; set; }

        public string Region { get; set; }

        public string District { get; set; }

        public string City { get; set; }
    }
}
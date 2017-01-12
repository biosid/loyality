using System;
using System.ComponentModel.DataAnnotations;

namespace Vtb24.Arms.Site.Models.News
{
    public class NewsModelListQueryModel
    {
        public DateTime? from { get; set; }

        public DateTime? to { get; set; }

        public bool hideunpublished { get; set; }

        [StringLength(100)]
        public string keyword { get; set; }
    }
}

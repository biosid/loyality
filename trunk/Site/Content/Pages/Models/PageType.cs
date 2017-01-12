using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vtb24.Site.Content.Pages.Models
{
    public enum PageType
    {
        [Description("Простая")]
        Plain,

        [Description("Оферта")]
        Offer,

        [Description("Неизвестно")]
        Unknown
    }
}

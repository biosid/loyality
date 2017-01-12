using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Vtb24.Arms.Areas.Catalog.Models.Delivery
{
    public enum PartnerLocationStatus
    {
        [Description("Привязан")]
        Binded,

        [Description("Не привязан")]
        NotBinded,

        [Description("Неверная привязка")]
        WrongBinding
    }
}
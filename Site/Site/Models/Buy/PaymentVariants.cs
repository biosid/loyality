using System.ComponentModel;

namespace Vtb24.Site.Models.Buy
{
    public enum PaymentVariants
    {
        // ReSharper disable InconsistentNaming

        [Description("Оплата бонусами программы «Коллекция»")]
        bonus,

        [Description("Оплата банковской картой ВТБ 24")]
        advance,

        // ReSharper restore InconsistentNaming
    }
}
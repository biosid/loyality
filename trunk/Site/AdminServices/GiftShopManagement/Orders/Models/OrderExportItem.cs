using System;
using LINQtoCSV;

namespace Vtb24.Arms.AdminServices.GiftShopManagement.Orders.Models
{
    public class OrderExportItem
    {
        [CsvColumn(FieldIndex = 1, Name = "Дата/Время", OutputFormat = "dd.MM.yyyy HH:mm")]
        public DateTime OrderTime { get; set; }

        [CsvColumn(FieldIndex = 2, Name = "№ Заказа(Коллекция)")]
        public int Id { get; set; }

        [CsvColumn(FieldIndex = 3, Name = "№ Заказа(партнер)")]
        public string ExternalId { get; set; }

        [CsvColumn(FieldIndex = 4, Name = "Статус")]
        public string Status { get; set; }

        [CsvColumn(FieldIndex = 5, Name = "Оплата товара")]
        public string ProductPaymentStatus { get; set; }

        [CsvColumn(FieldIndex = 6, Name = "Оплата доставки")]
        public string DeliveryPaymentStatus { get; set; }

        [CsvColumn(FieldIndex = 7, Name = "Артикул")]
        public string ProductId { get; set; }

        [CsvColumn(FieldIndex = 8, Name = "Контактное лицо")]
        public string ContactName { get; set; }

        [CsvColumn(FieldIndex = 9, Name = "Телефон")]
        public string ContactPhone { get; set; }

        [CsvColumn(FieldIndex = 10, Name = "E-mail")]
        public string ContactEmail { get; set; }

        [CsvColumn(FieldIndex = 11, Name = "Наименование")]
        public string Title { get; set; }

        [CsvColumn(FieldIndex = 12, Name = "К-во")]
        public int? Quantity { get; set; }

        [CsvColumn(FieldIndex = 13, Name = "Цена", OutputFormat = "F2")]
        public decimal? Price { get; set; }

        [CsvColumn(FieldIndex = 14, Name = "Оплачено бонусами", OutputFormat = "F0")]
        public decimal? BonusPayment { get; set; }

        [CsvColumn(FieldIndex = 15, Name = "Доплачено рублями", OutputFormat = "F2")]
        public decimal? RurPayment { get; set; }
    }
}

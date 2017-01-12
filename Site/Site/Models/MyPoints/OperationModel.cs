using System;
using Vtb24.Site.Services.Processing.Models;
using System.Linq;

namespace Vtb24.Site.Models.MyPoints
{
    public class OperationModel
    {
        public DateTime Date { get; set; }

        public decimal Amount { get; set; }

        public bool IsRecieved { get; set; }

        public string OrderId { get; set; }

        public string Description { get; set; }

        public bool IsHighlighted { get; set; }

        public string ImagePath { get; set; }

        public bool ShowImage
        {
            get { return !string.IsNullOrWhiteSpace(ImagePath); }
        }

        public static OperationModel Map(ProcessingOperationInfo info)
        {
            var model = new OperationModel
            {
                Date = info.ProcessingTime,
                Amount = Math.Abs(info.Sum),
                IsRecieved = info.Type == ProcessingOperationType.Deposit,
                Description = info.Desc,
                IsHighlighted = info.BankAccrualType.HasValue && info.BankAccrualType != 0,
                ImagePath = info.ImagePath
            };
            
            if (info.Cheque != null)
            {
                if (string.IsNullOrEmpty(model.Description))
                {
                    model.Description = string.Join("; ", info.Cheque.Items.Select(i => i.Name).ToArray());
                }

                if (info.Type == ProcessingOperationType.Withdraw)
                {
                    model.Description = string.Format("Заказ №{0}. ", info.Cheque.Id) + model.Description;
                }
            }

            if (string.IsNullOrEmpty(model.Description))
            {
                model.Description = "Нет описания";
            }

            return model;
        }
    }
}
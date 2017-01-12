using System;
using System.Linq;
using Vtb24.Site.Services.Processing.Models;

namespace Vtb24.Arms.Security.Models.Users
{
    public class UserPointsOperationModel
    {
        public DateTime Date { get; set; }

        public decimal Amount { get; set; }

        public bool IsRecieved { get; set; }

        public string Description { get; set; }

        public static UserPointsOperationModel Map(ProcessingOperationInfo original)
        {
            var model = new UserPointsOperationModel
            {
                Date = original.ProcessingTime,
                Amount = Math.Abs(original.Sum),
                IsRecieved = original.Type == ProcessingOperationType.Deposit,
                Description = original.Desc
            };

            if (original.Cheque != null)
            {
                if (string.IsNullOrEmpty(model.Description))
                {
                    model.Description = string.Join("; ", original.Cheque.Items.Select(i => i.Name).ToArray());
                }

                if (original.Type == ProcessingOperationType.Withdraw)
                {
                    model.Description = string.Format("Заказ №{0}. ", original.Cheque.Id) + model.Description;
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
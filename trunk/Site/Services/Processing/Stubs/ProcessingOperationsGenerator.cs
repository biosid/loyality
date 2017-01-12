using System;
using System.Collections.Generic;
using Vtb24.Site.Services.Infrastructure;
using Vtb24.Site.Services.Processing.Models;

namespace Vtb24.Site.Services.Processing.Stubs
{
    public static class ProcessingOperationsGenerator
    {
        public static List<ProcessingOperationInfo> Generate(int count, DateTimeRange range)
        {
            if (count <= 0 && !range.HasBothValues)
                return null;

            var operations = new List<ProcessingOperationInfo>();

            var rnd = new Random();
            var span = range.GetActualEndValue() - range.GetActualStartValue();
            for (var i = 0; i < count; i++)
            {
                var date = range.GetActualStartValue().AddDays(rnd.Next(0, (int)span.TotalDays));

                operations.Add(GenerateInfo(i + 1, date, date.AddDays(1)));
            }

            return operations;
        }

        private static ProcessingOperationInfo GenerateInfo(int id, DateTime posDate, DateTime procDate)
        {
            var rnd = new Random();
            var amount = rnd.Next(300, 42930);

            var info = new ProcessingOperationInfo
            {
                Id = id,
                PartnerId = "10",
                PartnerName = "Авито",
                PosId = "324",
                Sum = rnd.Next(1, 10) % 3 == 0 ? amount * -1 : amount,
                PosTime = posDate,
                ProcessingTime = procDate
            };

            return info;
        }
    }
}

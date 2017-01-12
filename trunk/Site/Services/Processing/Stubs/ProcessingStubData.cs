using System;
using System.Collections.Generic;
using Vtb24.Site.Services.Infrastructure;
using Vtb24.Site.Services.Processing.Models;

namespace Vtb24.Site.Services.Processing.Stubs
{
    internal static class ProcessingStubData
    {
        static ProcessingStubData()
        {
            Operations = ProcessingOperationsGenerator.Generate(10, new DateTimeRange(DateTime.Now.AddMonths(-6), DateTime.Now));
        }

        public static readonly List<ProcessingOperationInfo> Operations;
    }
}

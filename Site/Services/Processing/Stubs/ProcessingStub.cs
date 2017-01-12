using System;
using System.Linq;
using Vtb24.Site.Services.Models;
using Vtb24.Site.Services.Processing.Models;
using Vtb24.Site.Services.Processing.Models.Inputs;

namespace Vtb24.Site.Services.Processing.Stubs
{
    public class ProcessingStub : IProcessing
    {
        public decimal GetBalance()
        {
            return 15500;
        }

        public ProcessingOperationsHistoryResult GetOperationsHistory(DateTime from, DateTime to, PagingSettings paging)
        {
            var operations = ProcessingStubData.Operations;

            var range = operations.Where(op => op.ProcessingTime.Date >= from.Date && op.ProcessingTime.Date <= to.Date).ToArray();
            var totalCredit = range.Where(op => op.Sum > 0).Sum(op => op.Sum);
            var totalDebit = range.Where(op => op.Sum < 0).Sum(op => op.Sum);

            var totalCount = range.Length;
            var selectedOp = range.Skip(paging.Skip).Take(paging.Take).ToArray();

            return new ProcessingOperationsHistoryResult(selectedOp, totalDebit, totalCredit, totalCount, paging);
        }

        public decimal GetBalance(GetBalanceParameters parameters)
        {
            parameters.ValidateAndThrow();
            return 15500;
        }

        public ProcessingOperationsHistoryResult GetOperationsHistory(GetOperationHistoryParameters parameters, PagingSettings paging)
        {
            parameters.ValidateAndThrow();

            var operations = ProcessingStubData.Operations;

            var range = operations.Where(op => op.ProcessingTime.Date >= parameters.From.Date && op.ProcessingTime.Date <= parameters.To.Date).OrderByDescending(op => op.ProcessingTime).ToArray();
            var totalCredit = range.Where(op => op.Sum > 0).Sum(op => op.Sum);
            var totalDebit = range.Where(op => op.Sum < 0).Sum(op => op.Sum);

            var totalCount = range.Length;
            var selectedOp = range.Skip(paging.Skip).Take(paging.Take).ToArray();

            return new ProcessingOperationsHistoryResult(selectedOp, totalDebit, totalCredit, totalCount, paging);
        }
    }
}
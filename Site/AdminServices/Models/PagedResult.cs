using System.Collections;
using System.Collections.Generic;

namespace Vtb24.Arms.AdminServices.Models
{
    public class PagedResult<T> : IEnumerable<T>
    {
        public PagedResult(IEnumerable<T> result, int totalCount, PagingSettings paging)
        {
            _result = result;
            TotalCount = totalCount;
            Paging = paging;
        }

        public PagedResult(IEnumerable<T> result)
        {
            _result = result;
        }

        private readonly IEnumerable<T> _result;

        public int TotalCount { get; protected set; }

        public PagingSettings Paging { get; protected set; }

        public int TotalPages
        {
            get { return Paging.Take > 0 ? 1 + (TotalCount - 1) / Paging.Take : 0; }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return (_result ?? new T[] { }).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

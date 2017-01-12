using System;

namespace Vtb24.Arms.Security.Models
{
    public static class PagingHelper
    {
        public static int GetSkip(int pageNum, int pageSize)
        {
            return pageSize * (pageNum - 1);
        }

        public static int GetPagesCount(int itemsCount, int pageSize)
        {
            return (int)Math.Ceiling((double)itemsCount / pageSize);
        }
    }
}
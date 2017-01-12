using System;

namespace Vtb24.Site.Services.Models
{
    public struct PagingSettings
    {
        public int Skip { get; set; }
        
        public int Take { get; set; }

        public int GetTotalPages(int totalCount)
        {
            return Take > 0 ? 1 + (totalCount - 1) / Take : 0;
        }

        public static PagingSettings ByPage(int pageNum, int pageSize)
        {
            #region �������� ����������

            if (pageNum <= 0)
            {
                throw new ArgumentException(string.Format("������������ �������� pageNum: {0}", pageNum), "pageNum");    
            }
            if (pageSize <= 0)
            {
                throw new ArgumentException(string.Format("������������ �������� pageSize: {0}", pageSize), "pageSize");
            }
            
            #endregion

            return new PagingSettings { Skip = pageSize * (pageNum - 1), Take = pageSize };
        }

        public static PagingSettings ByOffset(int skip, int top)
        {
            #region �������� ����������

            if (skip < 0)
            {
                throw new ArgumentException(string.Format("������������ �������� skip: {0}", skip), "skip");
            }
            if (top <= 0)
            {
                throw new ArgumentException(string.Format("������������ �������� top: {0}", top), "top");
            }

            #endregion

            return new PagingSettings { Skip = skip, Take = top };
        }
    }
}
namespace RapidSoft.Loaylty.PromoAction.Repositories
{
    using System.Collections.Generic;

    /// <summary>
    /// �������� ���������, ������������ ������ <see cref="IList{T}"/> ��� ����� ������ ���������, 
    /// ��� ��� ��������� �� �������� ���������� � ��� ��� � ��������� �� ��� ������.
    /// </summary>
    /// <typeparam name="TEntity">��� ��������</typeparam>
    public class Page<TEntity> : List<TEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Page{TEntity}"/> class.
        /// </summary>
        /// <param name="collection">
        /// ��������� ���������.
        /// </param>
        /// <param name="skipCount">
        /// ���-�� ����������� ��������� ��� ������������ ��������.
        /// </param>
        /// <param name="takeCount">
        /// ���-�� ���������� ��������� ��� ������������ ��������.
        /// </param>
        /// <param name="totalCount">
        /// ���-�� ��������� ��� ����� ��������� ����������� � �����������.
        /// </param>
        public Page(IEnumerable<TEntity> collection, int? skipCount = null, int? takeCount = null, int? totalCount = null)
            : base(collection)
        {
            this.SkipCount = skipCount ?? 0;
            this.TakeCount = takeCount;
            this.TotalCount = totalCount;
        }

        /// <summary>
        /// ���-�� ����������� ��������� ��� ������������ ��������.
        /// </summary>
        public int SkipCount { get; private set; }

        /// <summary>
        /// ���-�� ���������� ��������� ��� ������������ ��������.
        /// </summary>
        public int? TakeCount { get; private set; }

        /// <summary>
        /// ���-�� ��������� ��� ����� ��������� ����������� � �����������.
        /// </summary>
        public int? TotalCount { get; private set; }
    }
}
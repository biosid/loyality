namespace RapidSoft.GeoPoints.Entities
{
    /// <summary>
    /// �������� �������������� (��� ������)
    /// </summary>
    public enum GeoCodingAccuracy
    {
        /// <summary>
        /// �������� ����������
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// ���, ������
        /// </summary>
        House = 1,

        /// <summary>
        /// �����
        /// </summary>
        Street = 2,
        
        /// <summary>
        /// ����� ������
        /// </summary>
        District = 3,

        /// <summary>
        /// �����, ���������
        /// </summary>
        City = 4,

        /// <summary>
        /// ����� �������
        /// </summary>
        Area = 5,

        /// <summary>
        /// �������
        /// </summary>
        Province = 6,

        /// <summary>
        /// ������
        /// </summary>
        Country = 7
    }
}
namespace RapidSoft.Loaders.Geocoder.Entities.Yandex
{
    public enum GeocodingPrecision
    {
        /// <summary>
        /// ������ ������������
        /// </summary>
        Exact = 0,

        /// <summary>
        /// ��������� ������ ����� ����
        /// </summary>
        Number = 1,

        /// <summary>
        /// ������ ��� ���������� (��� ��� 18�16 &lt; 10)
        /// </summary>
        Near = 2,

        /// <summary>
        /// ������� ����� (��� ��� 18�4 &gt; 10)
        /// </summary>
        Street = 3,

        /// <summary>
        /// ����� �� �������, �� ������, ��������, ������, �����, � �. �.
        /// </summary>
        Other = 4
    }
}
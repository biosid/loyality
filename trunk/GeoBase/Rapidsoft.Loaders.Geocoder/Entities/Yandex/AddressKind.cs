namespace RapidSoft.Loaders.Geocoder.Entities.Yandex
{
    public enum AddressKind
    {
        /// <summary>
        /// ��������� ���
        /// </summary>
        House = 0,

        /// <summary>
        /// �����
        /// </summary>
        Street = 1,

        /// <summary>
        /// ������� �����
        /// </summary>
        Metro = 2,

        /// <summary>
        /// ����� ������
        /// </summary>
        District = 3,

        /// <summary>
        /// ��������� �����: �����/�������/�������/����/...
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
        Country = 7,

        /// <summary>
        /// ����,�����,�����,�������������...
        /// </summary>
        Hydro = 8,

        /// <summary>
        /// �/�. �������
        /// </summary>
        Railway = 9,

        /// <summary>
        /// ����� ����� / ����� / �.�. �����
        /// </summary>
        Route = 10,

        /// <summary>
        /// ���, ����...
        /// </summary>
        Vegetation = 11,

        /// <summary>
        /// ��������
        /// </summary>
        Cemetery = 12,

        /// <summary>
        /// ����
        /// </summary>
        Bridge = 13,

        /// <summary>
        /// �������� �����
        /// </summary>
        Km = 14,

        /// <summary>
        /// ������
        /// </summary>
        Other = 15
    }
}
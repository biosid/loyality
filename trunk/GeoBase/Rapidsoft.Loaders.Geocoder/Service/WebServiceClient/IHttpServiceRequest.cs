using System.Security.Cryptography.X509Certificates;

namespace RapidSoft.Loaders.Geocoder.Service.WebServiceClient
{
    public interface IHttpServiceRequest {
        /// <summary>
        /// URL �������
        /// </summary>
        string Url { get; }

        /// <summary>
        /// SSL ���������� ��� ���������� ��������
        /// </summary>
        X509Certificate2 Certificate { get; set; }

        /// <summary>
        /// ������� ���������� � �������������.
        /// �� ��������� 10000�� (10 ������)
        /// </summary>
        int Timeout { get; set; }

        /// <summary>
        /// ������ �������, ����-��������
        /// </summary>
        QueryString Content { get; }

        /// <summary>
        /// ��������� ������ ����� GET.
        /// ������ ������� ���������� ����� query string.
        /// </summary>
        IHttpServiceResponse Get();

        /// <summary>
        /// ��������� ������ ����� POST.
        /// ������ ������� ������������ ��� application/x-www-form-urlencoded.
        /// </summary>
        IHttpServiceResponse Post();
    }
}
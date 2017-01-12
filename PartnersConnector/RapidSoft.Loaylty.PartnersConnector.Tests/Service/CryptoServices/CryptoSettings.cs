namespace RapidSoft.Loaylty.PartnersConnector.Tests.Service.CryptoServices
{
    using System.Configuration;
    using System.Security.Cryptography.X509Certificates;

    public class CryptoSettings
    {
        public static string XmlPublicKey
        {
            get
            {
                return @"<RSAKeyValue><Modulus>vI57shcn9CdZpU5tG9SsB20shtQ+7wwtYqZfWMbhXFyLIP8HOCsL/5B+xr5byts8cMBCc7L9hx21j6/xJCY5o7EPO2Q4cyMhcik6/9s9dp1gSGNLP8biqHZHv8sk+z6o5nisoVm9zP0Kiu1OULUg5spZlCsj4wy4f8ULM7agr+c=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
            }
        }

        public static string PemPublicKey
        {
            get
            {
                return @"-----BEGIN PUBLIC KEY-----
MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQC8jnuyFyf0J1mlTm0b1KwHbSyG
1D7vDC1ipl9YxuFcXIsg/wc4Kwv/kH7GvlvK2zxwwEJzsv2HHbWPr/EkJjmjsQ87
ZDhzIyFyKTr/2z12nWBIY0s/xuKodke/yyT7PqjmeKyhWb3M/QqK7U5QtSDmylmU
KyPjDLh/xQsztqCv5wIDAQAB
-----END PUBLIC KEY-----";
            }
        }

        public static string XmlPrivateKey
        {
            get
            {
                return
                    @"<RSAKeyValue><Modulus>vI57shcn9CdZpU5tG9SsB20shtQ+7wwtYqZfWMbhXFyLIP8HOCsL/5B+xr5byts8cMBCc7L9hx21j6/xJCY5o7EPO2Q4cyMhcik6/9s9dp1gSGNLP8biqHZHv8sk+z6o5nisoVm9zP0Kiu1OULUg5spZlCsj4wy4f8ULM7agr+c=</Modulus><Exponent>AQAB</Exponent><P>6xnZ7HzXHvtJi+KxJkUBHJt+wSPgO1CiSAYzHBy16uajaN+XUhuWXcCCEmwyYUwH18mmH726c5ZPKqT0MJcliQ==</P><Q>zVFt/FuaZIV6D0qh/vN83b0APvf8pZlKESS8hwfGUl7s1RNOL391dAu15JEZxixzNSaOH3XniL6KYTnudzs97w==</Q><DP>wpQtwkX8wZ6W21ju506bQfMMMIwhzNXKyjOfX3f/tH/Y5TaRBhrhE4z92oOEGvUTVKyHeqPLyliwAwptND4UiQ==</DP><DQ>AZaerjQbNqndrt6Z8Dn7/k8nAFW0y6cq7oUFPFowC5UWafOTSETJKNOqXZFNzL2tSnz43n9wAhvPQD9Ne/imWw==</DQ><InverseQ>izdoQEbFzZpytICetjejkuOG7JAIsJHOjjzajb00GbQcQw5GOavhyQsddKT2qQSNDzm/NW/O3m/2jCDppvw4Ew==</InverseQ><D>SPFx5sZQfYJPisSZLwAKAOF6LTqkkgOK9zPXhNeDTSC77era1x7ICHjUonv3tLU1X4Tw8CNZMtEKcOimh86F0yQGAOZEkLw5A+4OWjccGzeuhnjpVwASRmuYvuoKrxdRX4VeXs3RPBtHg/6n/vPyKKPgxOvYSMNXWXGatEze//E=</D></RSAKeyValue>";
            }
        }

        public static string PemPrivateKey
        {
            get
            {
                return @"-----BEGIN RSA PRIVATE KEY-----
MIICXQIBAAKBgQC8jnuyFyf0J1mlTm0b1KwHbSyG1D7vDC1ipl9YxuFcXIsg/wc4
Kwv/kH7GvlvK2zxwwEJzsv2HHbWPr/EkJjmjsQ87ZDhzIyFyKTr/2z12nWBIY0s/
xuKodke/yyT7PqjmeKyhWb3M/QqK7U5QtSDmylmUKyPjDLh/xQsztqCv5wIDAQAB
AoGASPFx5sZQfYJPisSZLwAKAOF6LTqkkgOK9zPXhNeDTSC77era1x7ICHjUonv3
tLU1X4Tw8CNZMtEKcOimh86F0yQGAOZEkLw5A+4OWjccGzeuhnjpVwASRmuYvuoK
rxdRX4VeXs3RPBtHg/6n/vPyKKPgxOvYSMNXWXGatEze//ECQQDrGdnsfNce+0mL
4rEmRQEcm37BI+A7UKJIBjMcHLXq5qNo35dSG5ZdwIISbDJhTAfXyaYfvbpzlk8q
pPQwlyWJAkEAzVFt/FuaZIV6D0qh/vN83b0APvf8pZlKESS8hwfGUl7s1RNOL391
dAu15JEZxixzNSaOH3XniL6KYTnudzs97wJBAMKULcJF/MGelttY7udOm0HzDDCM
IczVysozn193/7R/2OU2kQYa4ROM/dqDhBr1E1Ssh3qjy8pYsAMKbTQ+FIkCQAGW
nq40Gzap3a7emfA5+/5PJwBVtMunKu6FBTxaMAuVFmnzk0hEySjTql2RTcy9rUp8
+N5/cAIbz0A/TXv4plsCQQCLN2hARsXNmnK0gJ62N6OS44bskAiwkc6OPNqNvTQZ
tBxDDkY5q+HJCx10pPapBI0POb81b87eb/aMIOmm/DgT
-----END RSA PRIVATE KEY-----";
            }
        }

        public static X509Certificate2 Cert
        {
            get
            {
                var certFileName = ConfigurationManager.AppSettings["TestPartnerCer"];
                var cert = new X509Certificate2(certFileName);
                return cert;
            }
        }
    }
}
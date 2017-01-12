#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#include "include\mespro.h"
#include "include\tinydir.h"
//------------------------------------------------------------------------------------------
void ClearAll();
int EncryptInit(char* certificate, void *&enc_ctx, void *&sgn_ctx);
int DecryptInit(char* certificate, void *&enc_ctx, void *&sgn_ctx);
//------------------------------------------------------------------------------------------
int main(int argc, char **argv)
{
	int i = 0;
	char pse_path[4096];
	char src_fullname[4096];
	char tmp_fullname[4096];
	char dst_fullname[4096];
	void *enc_ctx;
	void *sgn_ctx;

	if (argc <= 4)
	{
		fprintf(stdout, "\nEXAMPLE\nEncryption:\nfilepro.exe -e \"Path\\to\\open\\files\" \"Path\\to\\encrypted\\files\" \"Path\\to\\certificate\"\nDecryption:\nfilepro.exe -d \"Path\\to\\encrypted\\files\" \"Path\\to\\decrypted\\files\" \"Path\\to\\certificate\"");
		return 1;
	}

	strcpy_s(pse_path, 4096, argv[4]);

	if (argv[1][0] == '-' && argv[1][1] == 'e')
	{
		//Инициализация шифрования
		if (EncryptInit(pse_path, enc_ctx, sgn_ctx)!= 0) goto end;

		tinydir_dir dir;
		tinydir_open(&dir, argv[2]);

		while (dir.has_next)
		{
			tinydir_file file;
			tinydir_readfile(&dir, &file);

			if (!file.is_dir)
			{
				strcpy_s(src_fullname, 4096, argv[2]);
				strcat_s(src_fullname, 4096, "\\");
				strcat_s(src_fullname, 4096, file.name);

				strcpy_s(dst_fullname, 4096, argv[3]);
				strcat_s(dst_fullname, 4096, "\\");
				strcat_s(dst_fullname, 4096, file.name);

				strcpy_s(tmp_fullname, 4096, argv[3]);
				strcat_s(tmp_fullname, 4096, "\\");
				strcat_s(tmp_fullname, 4096, file.name);
				strcat_s(tmp_fullname, 4096, ".tmp");

				// Формирование подписи файла
				if((i = SignFile(sgn_ctx, src_fullname, tmp_fullname)) != 0)
				{
					remove(tmp_fullname);
					tinydir_close(&dir);

					goto end;
				}

				// Шифрование файла
				if((i = EncryptOneFile(enc_ctx, tmp_fullname, dst_fullname)) != 0)
				{
					remove(tmp_fullname);
					tinydir_close(&dir);

					goto end;
				}

				remove(tmp_fullname);
			}

			tinydir_next(&dir);
		}

		tinydir_close(&dir);
	}
	else if (argv[1][0] == '-' && argv[1][1] == 'd')
	{
		//Инициализация дешифрования
		if (DecryptInit(pse_path, enc_ctx, sgn_ctx)!= 0) goto end;

		tinydir_dir dir;
		tinydir_open(&dir, argv[2]);

		while (dir.has_next)
		{
			tinydir_file file;
			tinydir_readfile(&dir, &file);

			if (!file.is_dir)
			{
				strcpy_s(src_fullname, 4096, argv[2]);
				strcat_s(src_fullname, 4096, "\\");
				strcat_s(src_fullname, 4096, file.name);

				strcpy_s(dst_fullname, 4096, argv[3]);
				strcat_s(dst_fullname, 4096, "\\");
				strcat_s(dst_fullname, 4096, file.name);

				strcpy_s(tmp_fullname, 4096, argv[3]);
				strcat_s(tmp_fullname, 4096, "\\");
				strcat_s(tmp_fullname, 4096, file.name);
				strcat_s(tmp_fullname, 4096, ".tmp");

				// Дешифрование файла
				if((i = DecryptOneFile(src_fullname, tmp_fullname)) != 0)
				{
					remove(tmp_fullname);
					tinydir_close(&dir);

					goto end;
				}

				// Проверка подписи 
				if((i = CheckFileSign(sgn_ctx, tmp_fullname, dst_fullname, 1)) != 0)
				{
					remove(tmp_fullname);
					tinydir_close(&dir);

					goto end;
				}

				remove(tmp_fullname);
			}

			tinydir_next(&dir);
		}

		tinydir_close(&dir);
	}

end:
	// Освобождаем контекст шифрования.
	if(enc_ctx != NULL) FreeCipherCTX(enc_ctx);

	// Освобождаем контекст подписи.
	if (sgn_ctx != NULL)FreeSignCTX(sgn_ctx);

	PKCS7Final();

	return i;
}
//------------------------------------------------------------------------------------------
void ClearAll()
{
	ClearPrivateKeys();
	ClearCertificates();
	ClearCAs();
	ClearCRLs();
}

/*****************************************************************************/
/* Функция инициализации шифрования с использованием ключей СКЗИ.            */
/*****************************************************************************/
static int EncryptInit(char* certificate, void *&enc_ctx, void *&sgn_ctx)
{
	enc_ctx = NULL;
	sgn_ctx = NULL;

	int i = 0;
	char ca[4096];
	char receiver[4096];
	char signer[4096];
	char private_key[4096];

	strcpy_s(ca, 4096, certificate);
	strcat_s(ca, 4096, "\\ca\\vtb24.pem");

	strcpy_s(receiver, 4096, certificate);
	strcat_s(receiver, 4096, "\\cert\\receiver.pem");

	strcpy_s(signer, 4096, certificate);
	strcat_s(signer, 4096, "\\cert\\signer.pem");

	strcpy_s(private_key, 4096, certificate);
	strcat_s(private_key, 4096, "\\keys\\00000001.key");

	// Следующая функция вызывается перед началом использования всех функций
	// библиотеки
	ClearBuffer((unsigned char*)"F752AA41");
	if (i = PKCS7Init(certificate, 0) != 0) goto error;

	// Очищаем контекстную информацию в библиотеки шифрования
	ClearAll();

	// Считываем в память из заданного каталога сертификаты
	// Удостоверяющего центра (УЦ) для проверки сертификата абонента. 
	if((i = AddCA(ca)) != 0) goto error;

	// Создание контекста шифрования.
	if((enc_ctx = GetCipherCTX()) == NULL) goto error;

	// Добавление абонента в контекст шифрования (задаем файл его сертификата).
	if((i = AddRecipient(enc_ctx, BY_FILE, receiver, NULL)) != 0) goto error;

	// Считываем из СКЗИ секретный ключ автора подписи.
	if((i = AddPSEPrivateKeyEx(certificate, NULL, private_key, NULL)) != 0) goto error;

	// Создание контекста подписи.
	if((sgn_ctx = GetSignCTX()) == NULL) goto error;

	// Добавление автора подписи в контекст (задаем файл его сертификата).
	if((i = AddSigner(sgn_ctx, BY_FILE, signer, NULL)) != 0) goto error;

	return i;

error:
	// Освобождаем контекст шифрования.
	if(enc_ctx != NULL) FreeCipherCTX(enc_ctx);

	// Освобождаем контекст подписи.
	if (sgn_ctx != NULL)FreeSignCTX(sgn_ctx);

	return i;
}

/*****************************************************************************/
/*	Функция инициализации дешифрования с использованием ключей СКЗИ.         */
/*****************************************************************************/
static int DecryptInit(char* certificate, void *&enc_ctx, void *&sgn_ctx)
{
	enc_ctx = NULL;
	sgn_ctx = NULL;

	int i = 0;
	char ca[4096];
	char crl[4096];
	char receiver[4096];
	char signer[4096];
	char private_key[4096];

	strcpy_s(ca, 4096, certificate);
	strcat_s(ca, 4096, "\\ca\\vtb24.pem");

	strcpy_s(receiver, 4096, certificate);
	strcat_s(receiver, 4096, "\\cert\\receiver.pem");

	strcpy_s(signer, 4096, certificate);
	strcat_s(signer, 4096, "\\cert\\signer.pem");

	strcpy_s(private_key, 4096, certificate);
	strcat_s(private_key, 4096, "\\keys\\00000001.key");

	// Следующая функция вызывается перед началом использования всех функций
	// библиотеки
	ClearBuffer((unsigned char*)"F752AA41");
	if (i = PKCS7Init(certificate, 0) != 0) goto error;

	// Очищаем контекстную информацию в библиотеки шифрования
	ClearAll();

	// Считываем в память сертификаты Удостоверяющего центра
	// для проверки сертификата автора подписи и списков отозванных сертификатов.
	if((i = AddCAs(ca)) != 0) goto error;

	// Считываем в память из заданного каталога сертификаты получателя
	// зашифрованных данных.
	if((i = AddCertificate(receiver)) != 0) goto error;

	// Считываем из СКЗИ секретный ключ получателя.
	if((i = AddPSEPrivateKeyEx(certificate, NULL, private_key, NULL)) != 0) goto error;

	// Создание контекста дешифрования.
	if((sgn_ctx = GetSignCTX()) == NULL) goto error;

	// Добавление автора подписи в контекст (задаем файл его сертификата).
	if((i = AddSigner(sgn_ctx, BY_FILE, signer, NULL)) != 0) goto error;

	return i;

error:
	// Освобождаем контекст шифрования.
	if(enc_ctx != NULL) FreeCipherCTX(enc_ctx);

	// Освобождаем контекст подписи.
	if (sgn_ctx != NULL)FreeSignCTX(sgn_ctx);

	return i;
}
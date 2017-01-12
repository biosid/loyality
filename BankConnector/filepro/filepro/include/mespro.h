/*
* $Revision: 1.51.2.1 $
* $Date: 2012/10/25 07:36:16 $
*/
#ifndef _MESPRO_H
#define _MESPRO_H

#ifdef __BORLANDC__
 #ifdef _WIN32
  #define WIN32
 #endif
#endif

#ifdef WIN32
 #include <windows.h>
 #define MPAPI WINAPI
#else
 #define MPAPI
 #define CDECL
#endif

#ifdef MP_DLL
 #ifdef MP_EXPORT
  #define MPFUN __declspec(dllexport)
 #else
  #define MPFUN __declspec(dllimport)
 #endif
#else
 #define MPFUN
#endif

#ifdef UNDER_CE
#undef MPFUN
#define MPFUN
#endif	/*UNDER_CE*/

/* certificate setting means */
#define BY_FILE				0
#define BY_SUBJECT			1
#define BY_SERIAL			2
#define BY_COMPONENTS		3
#define BY_BUFFER			4

/* data encoding types */
#define FORMAT_UNDEF		0
#define FORMAT_ASN1			1		/* DER */
#define FORMAT_TEXT			2
#define FORMAT_PEM			3		/* PEM */
#define FORMAT_NETSCAPE		4
#define FORMAT_GNIVC_FNS	5

/* code page values */
#define X509_NAME_HEX       0
#define X509_NAME_ANSI      1
#define X509_NAME_KOI8      2
#define X509_NAME_OEM       3
#define X509_NAME_UNICODE   4
#define X509_NAME_UTF8      5

/* PKCS7 message format definitions */
#define PKCS7_UNPROTECTED					0
#define PKCS7_SIGNED_TYPE					1
#define PKCS7_ENVELOPED_TYPE				2
#define PKCS7_SIGNED_AND_ENVELOPED_TYPE		(PKCS7_SIGNED_TYPE | PKCS7_ENVELOPED_TYPE)

/* DISTINGUISHED_NAME member length */
#define COUNTRY_NAME_LN					2		/* equal */
#define STATE_OR_PROVINCE_NAME_LN		128		/* maximum */
#define LOCALITY_NAME_LN				128		/* maximum */
#define ORGANIZATION_NAME_LN			64		/* maximum */
#define ORGANIZATIONAL_UNIT_NAME_LN		64		/* maximum */
#define TITLE_LN						64		/* maximum */
#define COMMON_NAME_LN					64		/* maximum */
#define EMAIL_ADDRESS_LN				255		/* maximum */
#define SERIAL_NUMBER_LN				64		/* maximum */
#define PSEUDONYM_LN					128		/* maximum */
#define POSTAL_ADDRESS_LN				180		/* maximum */
#define UNSTRUCTURED_NAME_LN			255		/* maximum */

#define SURNAME_LN						64		/* maximum */
#define GIVENNAME_LN					64		/* maximum */
#define STREET_ADDRESS_LN				128		/* maximum */

#define INN_LN							12		/* maximum */
#define RNS_FSS_LN						10		/* maximum */
#define KP_FSS_LN						4		/* maximum */

#define OGRN_LN							13		/* maximum */
#define SNILS_LN						11		/* maximum */
#define OGRNIP_LN						15		/* maximum */

/*  RDN attribute Object Identifiers */
#define COUNTRY_NAME_OID				"2.5.4.6"
#define STATE_OR_PROVINCE_NAME_OID		"2.5.4.8"
#define LOCALITY_NAME_OID				"2.5.4.7"
#define ORGANIZATION_NAME_OID			"2.5.4.10"
#define ORGANIZATIONAL_UNIT_NAME_OID	"2.5.4.11"
#define TITLE_OID						"2.5.4.12"
#define COMMON_NAME_OID					"2.5.4.3"
#define EMAIL_ADDRESS_OID				"1.2.840.113549.1.9.1"
#define SERIAL_NUMBER_OID				"2.5.4.5"
#define PSEUDONYM_OID					"2.5.4.65"
#define POSTAL_ADDRESS_OID				"2.5.4.16"
#define UNSTRUCTURED_NAME_OID			"1.2.840.113549.1.9.2"

#define SURNAME_OID						"2.5.4.4"
#define GIVENNAME_OID					"2.5.4.42"
#define STREET_ADDRESS_OID				"2.5.4.9"

#define INN_OID							"1.2.643.3.131.1.1"
#define RNS_FSS_OID						"1.2.643.3.141.1.1"
#define KP_FSS_OID						"1.2.643.3.141.1.2"

#define OGRN_OID						"1.2.643.100.1"
#define SNILS_OID						"1.2.643.100.3"
#define OGRNIP_OID						"1.2.643.100.5"

/* callback function option definitions for MakeCertificateRequest */
#define REQ_KEY_DRIVE_REQUEST			1
#define SGN_KEY_DRIVE_REQUEST			2
#define REQ_PSE_DRIVE_REQUEST			3
#define SGN_PSE_DRIVE_REQUEST			4
#define REQ_FILE_DRIVE_REQUEST			5
#define SGN_CERT_DRIVE_REQUEST			6
#define REQ_PSE_KEY_DRIVE_REQUEST		7
#define SGN_PSE_KEY_DRIVE_REQUEST		8

#define PRINT_CERTIFICATE_INFO			1
#define PRINT_CERTIFICATE_REQ_INFO		2

/* Flags definitions for SetCertificateVerifyFlags, VerifyCertificate and VerifyCRL */
#define STRICT_CERT_VERIFICATION		0x00000000L
#define CERT_NOT_YET_VALID_IGNORE		0x00000001L
#define CERT_HAS_EXPIRED_IGNORE			0x00000002L
#define CERT_REVOKED_IGNORE				0x00000004L
#define ISSUER_CERT_NOT_FOUND_IGNORE	0x00000008L
#define DEPTH_ZERO_ROOT_IGNORE          0x00000010L
#define CRL_MUST_EXIST                  0x00000020L
#define FAIL_IF_CRL_NOT_YET_VALID		0x00000040L
#define FAIL_IF_CRL_HAS_EXPIRED			0x00000080L
#define CRL_NOT_YET_VALID_IGNORE		0x00000100L
#define CRL_HAS_EXPIRED_IGNORE			0x00000200L
#define CERT_KEY_USAGE_IGNORE			0x80000000L

/* Flags definitions for PSE_Generation and PSE_Copy */
#define PSE_FAIL_IF_EXISTS				0x00000001L

#define PSE31_FAIL_IF_EXISTS			PSE_FAIL_IF_EXISTS

/* Flags definitions for MakeCertificateRequest */
#define CERT_REQ_ENCODE_TYPE_DER        0x00000001L
#define CERT_REQ_DONT_PRINT_TEXT        0x00000002L
#define CERT_REQ_UTF8_NAME              0x00000004L
#define CERT_REQ_USER_RDN_ORDER         0x00000008L
#define CERT_REQ_CMC_FORMAT             0x00000010L
#define CERT_REQ_CMC_ADD_CERTIFICATE    0x00000020L
#define CERT_REQ_USE_HARDWARE_DIGEST    0x00000040L
#define CERT_REQ_ANSI_NAME              0x00000080L

/* Flags definitions for SetPKCS7Flags */
#define USE_MESPRO_KEY_AGREE_FORMAT		0x00000000L
#define USE_CAPI_KEY_AGREE_FORMAT		0x00000001L
#define USE_CAPI_KEY_TRANS_FORMAT		0x00000002L
#define USE_HARDWARE_DIGEST             0x00000004L

/* Operation type definitions for processing callback function */
#define MPO_BUFFER_ENCRYPTION			1
#define MPO_BUFFER_DECRYPTION			2
#define MPO_FILE_ENCRYPTION				3
#define MPO_FILE_DECRYPTION				4
#define MPO_BUFFER_SIGNING				5
#define MPO_BUFFER_SIGNATURE_CHECKING	6
#define MPO_FILE_SIGNING				7
#define MPO_FILE_SIGNATURE_CHECKING		8
#define MPO_BUFFER_SIGNING_ENCRYPTION	9
#define MPO_BUFFER_DECRYPTION_CHECKING	10
#define MPO_FILE_SIGNING_ENCRYPTION		11
#define MPO_FILE_DECRYPTION_CHECKING	12
#define MPO_PSE_COPING					13
#define MPO_PSE_ERASING					14

#define MPO_PSE31_COPING				MPO_PSE_COPING
#define MPO_PSE31_ERASING				MPO_PSE_ERASING

/* Object type definitions for ConvertEncodedObject */
#define MP_OBJ_CERT_REQUEST				1
#define MP_OBJ_CERTIFICATE				2
#define MP_OBJ_CRL						3

/* Flags definitions for ConvertEncodedObject */
#define MP_CON_OBJ_PRINT_TEXT			0x00000001L

/* Private key format definitions for ConvertPrivateKey */
#define MP_PKEY_FORMAT_EAY				1
#define MP_PKEY_FORMAT_PKCS8			2

/* Flags definitions for ConvertPrivateKey */
#define MP_CON_PKEY_DONT_ENC_BY_PSW		0x00000001L

/* Flags definitions for Key Usage Extension */
#define MP_KEY_USAGE_DIGITAL_SIGNATURE	0x0001
#define MP_KEY_USAGE_NON_REPUDIATION	0x0002
#define MP_KEY_USAGE_KEY_ENCIPHERMENT	0x0004
#define MP_KEY_USAGE_DATA_ENCIPHERMENT	0x0008
#define MP_KEY_USAGE_KEY_AGREEMENT		0x0010
#define MP_KEY_USAGE_KEY_CERT_SIGN		0x0020
#define MP_KEY_USAGE_CRL_SIGN			0x0040
#define MP_KEY_USAGE_ENCIPHER_ONLY		0x0080
#define MP_KEY_USAGE_DECIPHER_ONLY		0x0100

/* Parameter types of data to be retrieved by GetPKCS7Param */
#define MP_CMS_ENCODED_CONTENT_PARAM				1
#define MP_CMS_SIGNATURE_TIME_STAMP_DATA_PARAM		2
#define MP_CMS_SIGNATURE_TIME_STAMP_TOKEN_PARAM		3

/* Operation types to be performed by PKCS7Control */
#define MP_CMS_CTRL_ADD_SIGNATURE_TIME_STAMP_TOKEN	1

/* Parameter types of data to be retrieved by MP_GetCertificateContextParam */
#define MP_CERT_EXTENSION_OID_PARAM			1
#define MP_CERT_EXTENSION_CRITICAL_PARAM	2
#define MP_CERT_KEY_USAGE_PARAM				3
#define MP_CERT_POLICY_OID_PARAM			4
#define MP_CERT_BASIC_CONSTRAINTS_CA_PARAM	5
#define MP_CERT_EXT_KEY_USAGE_OID_PARAM		6
#define MP_CERT_SUBJECT_NAME_PARAM			7
#define MP_CERT_ISSUER_NAME_PARAM			8
#define MP_CERT_SUBJECT_SIGN_TOOL_PARAM		9
#define MP_CERT_ISSUER_SIGN_TOOL_PARAM		10
#define MP_CERT_ISSUER_CA_TOOL_PARAM		11
#define MP_CERT_ISSUER_SIGN_TOOL_CERT_PARAM	12
#define MP_CERT_ISSUER_CA_TOOL_CERT_PARAM	13

/* Parameter types of data to be retrieved by MP_GetRequestContextParam */
#define MP_REQ_EXTENSION_OID_PARAM			1
#define MP_REQ_EXTENSION_CRITICAL_PARAM		2
#define MP_REQ_KEY_USAGE_PARAM				3
#define MP_REQ_POLICY_OID_PARAM				4
#define MP_REQ_BASIC_CONSTRAINTS_CA_PARAM	5
#define MP_REQ_EXT_KEY_USAGE_OID_PARAM		6
#define MP_REQ_SUBJECT_NAME_PARAM			7
#define MP_REQ_SUBJECT_SIGN_TOOL_PARAM		8

/* Key algorithm alias definitions */
#define MP_KEY_ALG_NAME_ECR3410				"ECR3410"
#define MP_KEY_ALG_NAME_ECR3410_CP			"ECR3410-CP"	/* id-GostR3410-2001-CryptoPro-A-ParamSet */
#define MP_KEY_ALG_NAME_ECR3410_CP_B		"CryptoPro-GostR3410-2001-B-ParamSet"
#define MP_KEY_ALG_NAME_ECR3410_CP_C		"CryptoPro-GostR3410-2001-C-ParamSet"
#define MP_KEY_ALG_NAME_ECR3410_CP_EX_A		"CryptoPro-GostR3410-2001-XchA-ParamSet"
#define MP_KEY_ALG_NAME_ECR3410_CP_EX_B		"CryptoPro-GostR3410-2001-XchB-ParamSet"
#define MP_KEY_ALG_NAME_R3410				"R3410"
#define MP_KEY_ALG_NAME_R3410_CP			"R3410-CP"
#define MP_KEY_ALG_NAME_RSA					"RSA"
#define MP_KEY_ALG_NAME_DSA					"DSA"

/* Cipher algorithm alias definitions */
#define MP_CIPHER_ALG_NAME_RUS_GAMMAR		"RUS-GAMMAR"
#define MP_CIPHER_ALG_NAME_RUS_GAMMAR_CP	"RUS-GAMMAR-CP"
#define MP_CIPHER_ALG_NAME_DES_EDE3_CBC		"DES-EDE3-CBC"

/* Hash algorithm alias definitions */
#define MP_HASH_ALG_NAME_RUS_HASH			"RUS-HASH"
#define MP_HASH_ALG_NAME_RUS_HASH_CP		"RUS-HASH-CP"
#define MP_HASH_ALG_NAME_SHA1				"SHA1"
#define MP_HASH_ALG_NAME_MD5				"MD5"

/* Token information definitions */
#define MP_TOKEN_LABEL						1
#define MP_TOKEN_SERIAL_NUMBER				2
#define MP_TOKEN_SLOT_NAME					3

#ifdef __cplusplus
extern "C" {
#endif

#pragma pack (1)

typedef struct
{
  char *Country;				/* can be NULL */
  char *StateOrProvince;		/* can be NULL */
  char *Locality;				/* can be NULL */
  char *Organization;			/* can be NULL */
  char *OrganizationalUnit;		/* can be NULL */
  char *Title;					/* can be NULL */
  char *CommonName;				/* can be NULL */
  char *EmailAddress;			/* can be NULL */
} DISTINGUISHED_NAME;

typedef struct
{
  char *Version;
  char *SerialNumber;
  char *NotBefore;
  char *NotAfter;
  DISTINGUISHED_NAME Issuer;
  DISTINGUISHED_NAME Subject;
  char *PublicKey;
  char *X509v3Extensions;		/* can be NULL */
  char *Signature;
  char *Text;
} CERTIFICATE_INFO;

typedef struct
{
  char *Version;
  DISTINGUISHED_NAME Subject;
  char *PublicKey;
  char *Signature;
  char *Text;
} CERTIFICATE_REQ_INFO;

#pragma pack()

/* Random Number Generator Functions */
MPFUN void MPAPI SetRandInitCallbackFun(int (MPAPI *Func)(int, int, int, char *));
	
/* Initialization and Completion Functions */
MPFUN int MPAPI PKCS7Init(char *pse_path, int reserved);
MPFUN int MPAPI PKCS7Final(void);

/* Key and Certificate Request Generation Functions */
MPFUN int MPAPI PSE_Generation(char *pse_path, int reserv, char *passwd, unsigned long flags);
MPFUN int MPAPI PSE_Copy(char *pse_path, char *passwd, char *new_pse_path, unsigned long flags);
MPFUN int MPAPI PSE_Erase(char *pse_path);
MPFUN int MPAPI PSE_ChangePassword(char *pse_path, char *old_pass, char *new_pass);

MPFUN int MPAPI PSE31_Generation(char *pse_path, int reserv, char *passwd, unsigned long flags);
MPFUN int MPAPI Copy_PSE31(char *pse_path, char *passwd, char *new_pse_path,
						   unsigned long flags);
MPFUN int MPAPI Erase_PSE31(char *pse_path);

MPFUN int MPAPI SetNewKeysAlgorithm(char *algor);
MPFUN int MPAPI SetKeysLength(int bits);
MPFUN int MPAPI SetKeyGenerationCallbackFun(void (CDECL *Func)(int, int, void *));
MPFUN void MPAPI SetKeysNewPasswordCallbackFun(int (CDECL *Func)(char *, int, int, void *));

MPFUN int MPAPI SetMakeRequestCallbackFun(int (CDECL *Func)(int, char, void *), void *Param);
MPFUN int MPAPI SetCertificateRequestFlags(unsigned long flags);
MPFUN int MPAPI GetCertificateRequestFlags(unsigned long *flags);

MPFUN int MPAPI SetCountry(char *Country);
MPFUN int MPAPI SetStateOrProvince(char *StateOrProvince);
MPFUN int MPAPI SetLocality(char *Locality);
MPFUN int MPAPI SetOrganization(char *Organization);
MPFUN int MPAPI SetOrganizationalUnit(char *OrganizationalUnit);
MPFUN int MPAPI SetTitle(char *Title);
MPFUN int MPAPI SetCommonName(char *CommonName);
MPFUN int MPAPI SetEmailAddress(char *EmailAddress);
MPFUN int MPAPI SetDistinguishedNameAttribute(char *oid, char *attr);
MPFUN void MPAPI ResetDistinguishedName(void);

/* Certificate Request Extensions Functions */
MPFUN int MPAPI SetKeyUsage(int value, int critical);
MPFUN int MPAPI AddCertificatePolicy(char *oid, int critical);
MPFUN int MPAPI SetBasicConstraints(int ca, int critical);
MPFUN int MPAPI AddExtendedKeyUsage(char *oid, int critical);
MPFUN int MPAPI SetSubjectSignTool(void);
MPFUN void MPAPI ResetExtensions(void);

MPFUN int MPAPI NewKeysGeneration(char *keyfile, char *password, char *reqfile);	/* obsolete, only used for RSA and DSA algorithms */
MPFUN int MPAPI NewKeysGenerationEx(char *pse_path, char *pse_pass, char *keyfile,
									char *key_pass, char *reqfile);

MPFUN int MPAPI MakeCertificateRequest(char *ReqFile,
                                       char *ReqKeyFile, char *ReqKeyPsw,
                                       char *SgnKeyFile, char *SgnKeyPsw);			/* obsolete, only used for RSA and DSA algorithms */
MPFUN int MPAPI MakeCertificateRequestEx(char *ReqFile,
                                         char *ReqKeyFile, char *ReqKeyPsw,
                                         char *SgnKeyFile, char *SgnKeyPsw, 
										 int CertParamType, void *CertParam1,
										 void *CertParam2);							/* obsolete, only used for RSA and DSA algorithms */
MPFUN int MPAPI MakePSECertificateRequest(char *ReqFile, 
										  char *ReqKeyPSEDir, char *ReqKeyFile, 
										  char *SgnKeyPSEDir, char *SgnKeyFile);
MPFUN int MPAPI MakePSECertificateRequestEx(char *ReqFile, 
										    char *ReqKeyPSEDir, char *ReqKeyFile, 
										    char *SgnKeyPSEDir, char *SgnKeyFile,
											int CertParamType, void *CertParam1,
										    void *CertParam2);

/* Certificate Request Information Functions */
MPFUN char* MPAPI GetRequestFingerprint(char *reqfile);
MPFUN char* MPAPI GetRequestFingerprintBuffer(char *buf, int ln);
MPFUN int MPAPI GetRequestInfo(char *reqfile, CERTIFICATE_REQ_INFO *info);
MPFUN int MPAPI GetRequestInfoBuffer(char *buf, int ln, 
									 CERTIFICATE_REQ_INFO *info);
MPFUN void MPAPI FreeRequestInfo(CERTIFICATE_REQ_INFO *info);
MPFUN void* MPAPI MP_GetRequestContext(int type, void *param, int len);
MPFUN void MPAPI MP_FreeRequestContext(void *ctx);
MPFUN int MPAPI MP_GetRequestContextParam(void *ctx, int type, void *arg,
										  char **data, int *len);

/* Private Key Password Changing Functions */
MPFUN int MPAPI ChangePrivateKeyPassword(char *keyfile, char *old_psw,
                                         char *new_psw);							/* obsolete, only used for RSA and DSA algorithms */
MPFUN int MPAPI ChangePrivateKeyPasswordEx(char *pse_path, char *pse_pass,
										   char *key_path, char *old_pass,
										   char *new_pass);

/* Private Key Loading Functions */
MPFUN void MPAPI SetPSEPasswordCallbackFun(int (CDECL *Func)(char *, int, int, void *));
MPFUN void MPAPI SetKeysPasswordCallbackFun(int (CDECL *Func)(char *, int, int, void *));
MPFUN char* MPAPI GetKeyFileNameForPassword(void);
MPFUN void MPAPI SetPrivateKeyErrorCallbackFun(int (CDECL *Func)(int, char *));

MPFUN int MPAPI AddPrivateKey(char *keyfile, char *password);						/* obsolete, only used for RSA and DSA algorithms */
MPFUN int MPAPI AddPrivateKeyFromBuffer(char *buf, int ln, char *password);			/* obsolete, only used for RSA and DSA algorithms */
MPFUN int MPAPI AddPrivateKeys(char *keydir, char *password);						/* obsolete, only used for RSA and DSA algorithms */
MPFUN int MPAPI AddPSEPrivateKey(char *psedir, char *keyfile);
MPFUN int MPAPI AddPSEPrivateKeyEx(char *pse_path, char *pse_pass, char *key_path, char *key_pass);
MPFUN int MPAPI AddPSEPrivateKeyFromBufferEx(char *pse_path, char *pse_pass, char *buf,
											 int len, char *key_pass);
MPFUN int MPAPI AddPSEPrivateKeys(char *psedir, char *keydir);
MPFUN int MPAPI ClearPrivateKeys(void);

/* Private Key Information Functions */
MPFUN char* MPAPI GetPrivateKeyFingerprint(char *keyfile, char *password);			/* obsolete, only used for RSA and DSA algorithms */
MPFUN char* MPAPI GetPrivateKeyFingerprintBuffer(char *buf, int ln, char *password);/* obsolete, only used for RSA and DSA algorithms */
MPFUN char* MPAPI GetPrivateKeyAlgorithm(char *pse_path, char *pse_pass,
										 char *key_path, char *key_pass);

/* Private Key Conversion Functions */
MPFUN int MPAPI ConvertPrivateKey(char *pse_path, char *pse_pass,
								  void *in_buf, int in_len, char *pass,
								  int encode, int format,
								  char *new_pse_path, char *new_pse_pass,
								  void **out_buf, int *out_len, char *new_pass,
								  unsigned long flags);

/* User Certificate Loading Functions */
MPFUN void MPAPI SetCertificateErrorCallbackFun(void (CDECL *Func)(int, char *));
MPFUN int MPAPI AddCertificate(char *certfile);
MPFUN int MPAPI AddCertificateFromBuffer(char *buf, int ln);
MPFUN int MPAPI AddCertificates(char *certdir);
MPFUN int MPAPI ClearCertificates(void);

/* CA Certificate Loading Functions */
MPFUN int MPAPI AddCA(char *CAfile);
MPFUN int MPAPI AddCAFromBuffer(char *buf, int ln);
MPFUN int MPAPI AddCAs(char *CAdir);
MPFUN int MPAPI ClearCAs(void);

/* Certificate Verification Functions */
MPFUN int MPAPI SetCertificateVerifyFlags(unsigned long flags);
MPFUN int MPAPI GetCertificateVerifyFlags(unsigned long *flags);
MPFUN int MPAPI SetValidationDate(char *date);

MPFUN int MPAPI VerifyCertificate(char *certfile, char *capath, char *cafile, 
								  unsigned long flags, CERTIFICATE_INFO *info);
MPFUN int MPAPI VerifyCertificateEx(char *certfile, char *capath, char *cafile,
									char *crlpath, char *crlfile,
								    unsigned long flags, CERTIFICATE_INFO *info);
MPFUN int MPAPI VerifyCertificateBuffer(char *cert_buf, int cert_ln, char *capath, 
										char *ca_buf, int ca_ln, 
										unsigned long flags, 
										CERTIFICATE_INFO *info);
MPFUN int MPAPI VerifyCertificateBufferEx(char *cert_buf, int cert_ln, 
										  char *capath, char *ca_buf, int ca_ln, 
										  char *crlpath, char *crl_buf, 
										  int crl_ln, unsigned long flags, 
										  CERTIFICATE_INFO *info);

/* Certificate Information Functions */
MPFUN char* MPAPI GetCertificateSubject(char *certfile);
MPFUN char* MPAPI GetCertificateSubjectBuffer(char *buf, int ln);
MPFUN char* MPAPI GetCertificateIssuer(char *certfile);
MPFUN char* MPAPI GetCertificateIssuerBuffer(char *buf, int ln);
MPFUN char* MPAPI GetCertificateSerial(char *certfile);
MPFUN char* MPAPI GetCertificateSerialBuffer(char *buf, int ln);
MPFUN char* MPAPI GetCertificateNotBefore(char *certfile);
MPFUN char* MPAPI GetCertificateNotBeforeBuffer(char *buf, int ln);
MPFUN char* MPAPI GetCertificateNotAfter(char *certfile);
MPFUN char* MPAPI GetCertificateNotAfterBuffer(char *buf, int ln);
MPFUN char* MPAPI GetCertificateFingerprint(char *certfile);
MPFUN char* MPAPI GetCertFingerprintBuffer(char *buf, int ln);
MPFUN char* MPAPI GetCertificateInfo(char *certfile);
MPFUN char* MPAPI GetCertificateInfoBuffer(char *buf, int ln);
MPFUN int MPAPI GetCertificateInfoEx(char *certfile, CERTIFICATE_INFO *info);
MPFUN int MPAPI GetCertificateInfoBufferEx(char *buf, int ln, 
										   CERTIFICATE_INFO *info);
MPFUN void MPAPI FreeCertificateInfo(CERTIFICATE_INFO *info);
MPFUN char* MPAPI GetCertPublicKeyAlgorithm(char *cert_path);
MPFUN char* MPAPI GetCertPublicKeyAlgorithmBuffer(char *buf, int len);
MPFUN int MPAPI GetCertificateBySubject(char *subject, char **buf, int *ln);
MPFUN int MPAPI GetCertificateBySerial(char *serial, char **buf, int *ln);
MPFUN void* MPAPI MP_GetCertificateContext(int type, void *cert, int len);
MPFUN void MPAPI MP_FreeCertificateContext(void *ctx);
MPFUN int MPAPI MP_GetCertificateContextParam(void *ctx, int type, void *arg,
											  char **data, int *len);
MPFUN char* MPAPI GetX509NameAttribute(void *name, int len, char *oid);
MPFUN int MPAPI GetX509NameAttributeNumber(void *name, int len);
MPFUN char* MPAPI GetX509NameAttributeOID(void *name, int len, int ind);
MPFUN char* MPAPI GetX509NameAttributeValue(void *name, int len, int ind);

/* CRL Loading Functions */
MPFUN int MPAPI AddCRL(char *CRLfile);
MPFUN int MPAPI AddCRLFromBuffer(char *buf, int ln);
MPFUN int MPAPI AddCRLs(char *CRLdir);
MPFUN int MPAPI ClearCRLs(void);

/* CRL Verification Functions */
MPFUN int MPAPI VerifyCRL(char *crl_file, char *ca_path,
						  char *ca_file, unsigned long flags,
						  CERTIFICATE_INFO *info);
MPFUN int MPAPI VerifyCRL_Buffer(char *crl_buf, int crl_ln, char *ca_path,
								 char *ca_buf, int ca_ln, unsigned long flags,
								 CERTIFICATE_INFO *info);

/* CRL Information Functions */
MPFUN char* MPAPI GetCRLLastUpdate(char *crlfile);
MPFUN char* MPAPI GetCRLLastUpdateBuffer(char *buf, int ln);
MPFUN char* MPAPI GetCRLNextUpdate(char *crlfile);
MPFUN char* MPAPI GetCRLNextUpdateBuffer(char *buf, int ln);
MPFUN char* MPAPI GetCRLIssuer(char *crlfile);
MPFUN char* MPAPI GetCRLIssuerBuffer(char *buf, int ln);
MPFUN void* MPAPI GetCrlCTX(char *crlfile);
MPFUN void* MPAPI GetCrlBufferCTX(char *buf, int ln);
MPFUN void MPAPI FreeCrlCTX(void *ctx);
MPFUN int MPAPI GetCrlRevokedNumber(void *ctx);
MPFUN char* MPAPI GetCrlRevokedSerial(void *ctx, int ind);

/* Comparison Functions */
MPFUN int MPAPI CertAndPrivateKeyMatch(char *certfile, char *keyfile, 
									   char *password);								/* obsolete, only used for RSA and DSA algorithms */
MPFUN int MPAPI CertAndPrivateKeyMatchBuffer(char *certbuf, int certln, 
									         char *keybuf, int keyln, 
											 char *password);						/* obsolete, only used for RSA and DSA algorithms */

MPFUN int MPAPI CertAndPSEPrivateKeyMatch(char *certfile, char *psedir, 
										  char *keyfile);
MPFUN int MPAPI CertAndPSEPrivateKeyMatchBufferEx(char *cert_buf, int cert_len, 
												  char *pse_path, char *reserv,
												  char *key_buf, int key_len,
												  char *pass);

MPFUN int MPAPI CertAndPSEPrivateKeysMatch(char *certfile, char *psedir, 
										   char *keyfile);
MPFUN int MPAPI CertAndPSEPrivateKeysMatchBuffer(char *cert_buf, int cert_len,
												 char *pse_dir, char *key_file);

MPFUN int MPAPI CertAndRequestMatch(char *certfile, char *reqfile);
MPFUN int MPAPI CertAndRequestMatchBuffer(char *certbuf, int certln, 
									      char *reqbuf, int reqln);

/* Certificate Request, Certificate and CRL Conversion Functions */
MPFUN int MPAPI ConvertEncodedObject(int obj_type, void *in_buf, int in_len,
									 int encode, void **out_buf, int *out_len,
									 unsigned long flags);

MPFUN int MPAPI SetInputFormat(int form);
MPFUN int MPAPI SetOutputFormat(int form);

/* Data Encryption/Decryption Functions */
MPFUN int MPAPI SetPKCS7Flags(unsigned long flags);
MPFUN int MPAPI GetPKCS7Flags(unsigned long *flags);

MPFUN int MPAPI SetCipherAlgorithm(char *algor);

MPFUN void* MPAPI GetCipherCTX(void);
MPFUN int MPAPI AddRecipient(void *ctx, int type, void *param1, void *param2);
MPFUN void MPAPI FreeCipherCTX(void *ctx);

MPFUN int MPAPI EncryptBuffer(void *ctx, void *in_buf, int in_len,
                              void **out_buf, int *out_len);
MPFUN int MPAPI EncryptOneFile(void *ctx, char *in_file, char *out_file);

MPFUN int MPAPI DecryptBuffer(void *in_buf, int in_len,
                              void **out_buf, int *out_len);
MPFUN int MPAPI DecryptOneFile(char *in_file, char *out_file);

/* Gigital Signature Functions */
MPFUN int MPAPI SetDigestAlgorithm(char *algor);
MPFUN int MPAPI InsertCertificateToSign(int insert);
MPFUN int MPAPI InsertSigningTimeToSign(int insert);

MPFUN void* MPAPI GetSignCTX(void);
MPFUN int MPAPI AddSigner(void *ctx, int type, void *param1, void *param2);
MPFUN void MPAPI FreeSignCTX(void *ctx);

MPFUN int MPAPI SignBuffer(void *ctx, void *in_buf, int in_len,
                           void **out_buf, int *out_len);
MPFUN int MPAPI SignBufferEx(void *sign_ctx, void *in_buf, int in_len,
                             void **out_buf, int *out_len, int detach);
MPFUN int MPAPI SignFile(void *ctx, char *in_file, char *out_file);
MPFUN int MPAPI SignFileEx(void *ctx, char *in_file, char *out_file, int detach);

MPFUN int MPAPI CheckBufferSign(void *ctx, void *in_buf, int in_len,
                                void **out_buf, int *out_len, int sign_del);
MPFUN int MPAPI CheckBufferSignEx(void *sign_ctx, void *in_buf, int in_len,
                                  void **out_buf, int *out_len, int sign_del,
								  void *detach, int detach_ln);
MPFUN int MPAPI CheckFileSign(void *sign_ctx, char *in_file, char *out_file,
                              int sign_del);
MPFUN int MPAPI CheckFileSignEx(void *sign_ctx, char *in_file, char *out_file,
                                int sign_del, char *det_file);

MPFUN int MPAPI DeleteBufferSign(void *in_buf, int in_len,
                                 void **out_buf, int *out_len);
MPFUN int MPAPI DeleteFileSign(char *in_file, char *out_file);

MPFUN int MPAPI GetSignatureCount(void *ctx);
MPFUN char* MPAPI GetSignatureSubject(void *ctx, int ind);
MPFUN char* MPAPI GetSignatureIssuer(void *ctx, int ind);
MPFUN int MPAPI GetSignatureStatus(void *ctx, int ind);
MPFUN char* MPAPI GetSignatureTime(void *ctx, int ind);
MPFUN int MPAPI SignatureCertificateIsNew(void *sgn_ctx, int ind);
MPFUN int MPAPI GetSignatureCertInBuffer(void *ctx, int ind, char **buf, int *ln);

MPFUN int MPAPI VerifyDevelopersSign(char *file, unsigned char *sig, 
									 unsigned int siglen);

/* Combined Functions (obsolete) */ 
MPFUN int MPAPI SignAndEncryptBuffer(void *sign_ctx, void *enc_ctx,
                                     void *in_buf, int in_len,
                                     void **out_buf, int *out_len);
MPFUN int MPAPI SignAndEncryptFile(void *sign_ctx, void *enc_ctx,
                                   char *in_file, char *out_file);

MPFUN int MPAPI DecryptAndCheckBufferSign(void *ctx, void *in_buf, int in_len,
                                          void **out_buf, int *out_len);
MPFUN int MPAPI DecryptAndCheckFileSign(void *ctx, char *in_file, char *out_file);

/* PKCS7 Message Information Functions */
MPFUN int MPAPI GetPKCS7TypeBuffer(void *buf, int len, int *type);
MPFUN int MPAPI GetPKCS7TypeBufferEx(void *buf, int len, int *type, void **ctx);
MPFUN int MPAPI GetPKCS7TypeFile(char *file, int *type);
MPFUN int MPAPI GetPKCS7TypeFileEx(char *file, int *type, void **ctx);
MPFUN void MPAPI FreePKCS7CTX(void *ctx);
MPFUN int MPAPI GetPKCS7RecipientNumber(void *ctx);
MPFUN char* MPAPI GetPKCS7RecipientSerial(void *ctx, int ind);
MPFUN char* MPAPI GetPKCS7RecipientIssuer(void *ctx, int ind, DISTINGUISHED_NAME *name);
MPFUN int MPAPI GetPKCS7SignatureNumber(void *ctx);
MPFUN char* MPAPI GetPKCS7SignatureSerial(void *ctx, int ind);
MPFUN char* MPAPI GetPKCS7SignatureIssuer(void *ctx, int ind, DISTINGUISHED_NAME *name);
MPFUN char* MPAPI GetPKCS7SignatureTime(void *ctx, int ind);

MPFUN int MPAPI GetPKCS7Param(void *ctx, int type, int ind, void *arg, char **data, int *len);
MPFUN int MPAPI PKCS7Control(void *ctx, int type, int ind, void *reserv, char *data, int len);

/* Auxiliary Functions */
MPFUN void* MPAPI AllocBuffer(int size);
MPFUN void MPAPI FreeBuffer(void *ptr);
MPFUN void MPAPI BufferFree(void *ptr);

MPFUN int MPAPI FileExists(char *file);
MPFUN int MPAPI MP_CopyFile(char *in_file, char *out_file, int fail_if_exists);
MPFUN void MPAPI EraseAndRemoveFile(char *file);
MPFUN int MPAPI PrintDetailErrors(char *file);

MPFUN int MPAPI SetProcessingCallbackFun(int (MPAPI *Func)(int, unsigned long,
										 unsigned long, void *), void *Param);

MPFUN int MPAPI SetInputCodePage(int codepage);
MPFUN int MPAPI SetOutputCodePage(int codepage);

MPFUN int MPAPI CspContainerToPse(char *cont, int prov, char *pse_path, char *pse_pass,
								  char *keyfile, char *key_pass, unsigned long flags,
								  void *reserv);
MPFUN int MPAPI PseToCspContainer(char *pse_path, char *pse_pass, char *keyfile,
								  char *key_pass, char *cont, void *reserv);

/* Touch-Memory (iButton) Functions */
MPFUN void MPAPI SetTouchMemoryCallbackFun(int (CDECL *Func)(short, short, short));

/* eToken Functions */
MPFUN void MPAPI Set_eTokenPasswordCallbackFun(int (CDECL *Func)(char *, int, int, void *));
MPFUN int MPAPI eTokenInserted(char *path);

MPFUN void MPAPI Set_ruTokenPasswordCallbackFun(int (CDECL *Func)(char *, int, int, void *));
MPFUN int MPAPI ruTokenInserted(char *path);

MPFUN void MPAPI Set_PKCS11TokenPasswordCallbackFun(int (CDECL *Func)(char *, int, int, void *));
MPFUN int MPAPI PKCS11TokenInserted(char *path);

MPFUN void MPAPI Set_iKeyPasswordCallbackFun(int (CDECL *Func)(char *, int, int, void *));
MPFUN int MPAPI iKeyInserted(char *path);

MPFUN int MPAPI GetTokenInfo(char *path, int type, char *buf, int *len);
MPFUN int MPAPI GetLastTokenError(void);
MPFUN void MPAPI ClearLastTokenError(void);
MPFUN int MPAPI TokenPinRequired(char *path);

MPFUN void MPAPI CryptoMemCtrlOn(void);
MPFUN void MPAPI CryptoMemCtrlOff(void);
MPFUN void MPAPI CryptoMemLeaksPrint(char *file);
MPFUN void MPAPI ClearBuffer(unsigned char *buf);

/* Error Handling Functions */
MPFUN int MPAPI GetMessageProLastError(void);

#ifdef __cplusplus
}
#endif

#endif //  _MESPRO_H

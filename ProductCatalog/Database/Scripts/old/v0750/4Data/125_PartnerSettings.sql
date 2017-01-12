DECLARE @FixBasketItemPriceUrl nvarchar(512) = 'https://ogrishchenko-w7.rapidsoft.local:643/Actions/FixBasketItemPrice'
DECLARE @CheckUrl nvarchar(512) = 'https://ogrishchenko-w7.rapidsoft.local:643/Actions/CheckOrder'
DECLARE @ConfirmationUrl nvarchar(512) = 'https://ogrishchenko-w7.rapidsoft.local:643/Actions/ConfirmOrder'
DECLARE @BatchConfirmationUrl nvarchar(512) = 'https://ogrishchenko-w7.rapidsoft.local:643/Actions/BatchConfirmOrder'
DECLARE @CertificateThumbprint nvarchar(512) = '10 8e 80 42 da 05 48 78 80 c3 e5 b0 c1 a2 39 dc f8 ea 63 1b'

MERGE [prod].[PartnerSettings] as dst
USING (	VALUES 
			(1, 'FixBasketItemPrice', @FixBasketItemPriceUrl), 
			(1, 'Check', @CheckUrl), 
			(1, 'UseBatch', 'false'), 
			(1, 'Confirmation', @ConfirmationUrl), 
			(1, 'BatchConfirmation', @BatchConfirmationUrl), 
			(1, 'CertificateThumbprint', @CertificateThumbprint),
			
			(2, 'FixBasketItemPrice',@FixBasketItemPriceUrl), 
			(2, 'Check', @CheckUrl), 
			(2, 'UseBatch', 'true'), 
			(2, 'Confirmation', @ConfirmationUrl), 
			(2, 'BatchConfirmation', @BatchConfirmationUrl), 
			(2, 'CertificateThumbprint', @CertificateThumbprint),
			
			(5, 'PublicKey','<RSAKeyValue><Modulus>vI57shcn9CdZpU5tG9SsB20shtQ+7wwtYqZfWMbhXFyLIP8HOCsL/5B+xr5byts8cMBCc7L9hx21j6/xJCY5o7EPO2Q4cyMhcik6/9s9dp1gSGNLP8biqHZHv8sk+z6o5nisoVm9zP0Kiu1OULUg5spZlCsj4wy4f8ULM7agr+c=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>')
		)
		AS src ([partnerId], [key], [value])
ON dst.[PartnerId] = src.[partnerId] AND dst.[Key] = src.[key]
WHEN MATCHED THEN
	UPDATE SET Value = src.[value]
WHEN NOT MATCHED BY TARGET THEN
	INSERT ([PartnerId], [Key], [Value]) 
	VALUES ([partnerId], [key], [value]);
GO
	
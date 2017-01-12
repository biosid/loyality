IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[ClientForBankRegistrationResults]'))
	DROP VIEW [dbo].[ClientForBankRegistrationResults]
GO

CREATE VIEW [dbo].[ClientForBankRegistrationResults]
AS
	SELECT ISNULL(res.ClientId, (SELECT TOP 1 ires.ClientId
								  FROM [dbo].[ClientForBankRegistrationResponse] ires 
								 WHERE ires.SessionId = req.SessionId
								   AND ires.[Login] = req.MobilePhone)) AS ClientId,
		   ISNULL(res.[Login],req.[MobilePhone]) AS [Login],
			CASE
				WHEN res.[Status] IS NOT NULL THEN res.[Status]
				WHEN EXISTS (	SELECT TOP 1 1 
								  FROM [dbo].[ClientForBankRegistrationResponse] ires 
								 WHERE ires.SessionId = req.SessionId
								   AND ires.[Login] = req.MobilePhone
								   AND ires.[Status] = 1)
					THEN 2
				ELSE 3
			END AS [Status],
			req.[SessionId]
	FROM [dbo].[ClientForBankRegistration] req
	LEFT JOIN [dbo].[ClientForBankRegistrationResponse] res ON res.ClientId = req.ClientId AND res.SessionId = req.SessionId
GO



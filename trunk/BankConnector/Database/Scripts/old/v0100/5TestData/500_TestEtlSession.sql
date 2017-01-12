/* EtlSessions */
GO
DELETE FROM [etl].[EtlSessions] WHERE [EtlSessionId]=N'B1B96FC1-E344-4BD9-B10D-F5342C2F47D5';
GO
DELETE FROM [etl].[EtlSessions] WHERE [EtlSessionId]=N'8353F751-1519-4B3E-8714-8499A587A824';
GO
DELETE FROM [etl].[EtlSessions] WHERE [EtlSessionId]=N'F6C6D2A4-450C-4FF7-8C43-73991148C3ED';
GO
DELETE FROM [etl].[EtlSessions] WHERE [EtlSessionId]=N'CCBB2B68-CB0F-4281-807B-AC76974099EA';
GO
DELETE FROM [etl].[EtlSessions] WHERE [EtlSessionId]=N'c3a950d4-c39c-48ce-bfd5-47abc5d783f4';
GO
DELETE FROM [etl].[EtlSessions] WHERE [EtlSessionId]=N'ee85ead5-d591-4a12-8fd3-6cf78641980d';
GO
DELETE FROM [etl].[EtlSessions] WHERE [EtlSessionId]=N'71231619-C0C1-4836-8FE1-A0A6F95944F1';
GO

/* EtlSessions */
INSERT [etl].[EtlSessions] (
[EtlSessionId], 
[EtlPackageId], 
[EtlPackageName],
[StartDateTime],
[StartUtcDateTime],
[Status]) 
VALUES (
N'B1B96FC1-E344-4BD9-B10D-F5342C2F47D5', 
N'E23D8AF7-5916-4907-9D99-D69ED8E7D542', 
N'ReceiveActivateClients',
GETDATE(),
GETUTCDATE(),
0)
GO
/* EtlSessions */
INSERT [etl].[EtlSessions] (
[EtlSessionId], 
[EtlPackageId], 
[EtlPackageName],
[StartDateTime],
[StartUtcDateTime],
[Status]) 
VALUES (
N'8353F751-1519-4B3E-8714-8499A587A824', 
N'E23D8AF7-5916-4907-9D99-D69ED8E7D542', 
N'ReceiveActivateClients',
GETDATE(),
GETUTCDATE(),
0)

GO
/* EtlSessions */
INSERT [etl].[EtlSessions] (
[EtlSessionId], 
[EtlPackageId], 
[EtlPackageName],
[StartDateTime],
[StartUtcDateTime],
[Status]) 
VALUES (
N'F6C6D2A4-450C-4FF7-8C43-73991148C3ED', 
N'6DD71E6C-0FD6-4F27-BC3E-ABB94921B7CF', 
N'SendOrdersForPayment',
GETDATE(),
GETUTCDATE(),
0)

GO
/* EtlSessions */
INSERT [etl].[EtlSessions] (
[EtlSessionId], 
[EtlPackageId], 
[EtlPackageName],
[StartDateTime],
[StartUtcDateTime],
[Status]) 
VALUES (
N'CCBB2B68-CB0F-4281-807B-AC76974099EA', 
N'B8C07E39-37E9-45F1-8A79-2A99D86B9641', 
N'ReceiveOrderPaymentResponse',
GETDATE(),
GETUTCDATE(),
0)
go 

INSERT [etl].[EtlSessions] (
[EtlSessionId], 
[EtlPackageId], 
[EtlPackageName],
[StartDateTime],
[StartUtcDateTime],
[Status]) 
VALUES (
N'c3a950d4-c39c-48ce-bfd5-47abc5d783f4', 
N'64E1E608-C27F-43BB-88FA-4865E7178109', 
N'RegisterBankClients',
GETDATE(),
GETUTCDATE(),
0)
GO

INSERT [etl].[EtlSessions] (
[EtlSessionId], 
[EtlPackageId], 
[EtlPackageName],
[StartDateTime],
[StartUtcDateTime],
[Status]) 
VALUES (
N'ee85ead5-d591-4a12-8fd3-6cf78641980d', 
N'a50fdfdb-92bd-4b27-a9cb-4f80be7c5295', 
N'AssignClientRules',
GETDATE(),
GETUTCDATE(),
0)
GO

INSERT [etl].[EtlSessions] (
[EtlSessionId], 
[EtlPackageId], 
[EtlPackageName],
[StartDateTime],
[StartUtcDateTime],
[Status]) 
VALUES (
N'71231619-C0C1-4836-8FE1-A0A6F95944F1',
N'D83D02DF-98E0-4714-B06A-9F967930D051',  
N'ReceivePersonalMessages',
GETDATE(),
GETUTCDATE(),
0)
GO






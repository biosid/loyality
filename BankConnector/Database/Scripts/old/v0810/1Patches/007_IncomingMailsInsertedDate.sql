ALTER TABLE etl.EtlIncomingMails ADD
InsertedDate datetime NOT NULL 
CONSTRAINT DF_EtlIncomingMails_InsertedDate DEFAULT getdate()
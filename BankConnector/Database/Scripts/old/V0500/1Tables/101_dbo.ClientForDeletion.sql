IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ClientForDeletion_InsertEtlSessionId]') AND parent_object_id = OBJECT_ID(N'[dbo].[ClientForDeletion]'))
ALTER TABLE [dbo].[ClientForDeletion] DROP CONSTRAINT [FK_ClientForDeletion_InsertEtlSessionId]
GO



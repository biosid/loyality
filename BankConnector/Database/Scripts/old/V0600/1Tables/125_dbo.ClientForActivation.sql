IF NOT EXISTS(	SELECT * FROM sys.columns 
				WHERE Name = N'Segment' AND Object_ID = Object_ID(N'[dbo].[ClientForActivation]'))    
BEGIN
	PRINT '��������� ������� Segment � ������� [dbo].[ClientForActivation]'
    ALTER TABLE [dbo].[ClientForActivation]
	ADD [Segment] [int] NULL
END
GO
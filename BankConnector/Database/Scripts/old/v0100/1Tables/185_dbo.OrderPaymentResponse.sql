CREATE TABLE [dbo].[OrderPaymentResponse](
	[EtlSessionId] [uniqueidentifier] NOT NULL,
	[OrderId] [int] NOT NULL,	
	[Status] [int] NOT NULL,
	[Reason] [nvarchar](255) NULL,	
 CONSTRAINT [PK_OrderPaymentResponse] PRIMARY KEY CLUSTERED 
(
	[OrderId] ASC,
	[EtlSessionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
)
GO
ALTER TABLE [dbo].[OrderPaymentResponse]  WITH CHECK ADD  CONSTRAINT [FK_OrderPaymentResponse_EtlSessions] FOREIGN KEY([EtlSessionId])
REFERENCES [etl].[EtlSessions] ([EtlSessionId])
GO
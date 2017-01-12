CREATE TABLE [dbo].[OrderPaymentResponse2](
	[EtlSessionId] [uniqueidentifier] NOT NULL,
	[OrderId] [int] NOT NULL,
	[ItemPaymentStatus] [int] NOT NULL,
	[DeliveryPaymentStatus] [int] NOT NULL,
	[BonusGatewayStatus] [int] NULL
 CONSTRAINT [PK_OrderPaymentResponse2] PRIMARY KEY CLUSTERED 
(
	[OrderId] ASC,
	[EtlSessionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY])
GO

ALTER TABLE [dbo].[OrderPaymentResponse2]  WITH CHECK ADD  CONSTRAINT [FK_OrderPaymentResponse2_EtlSessions] FOREIGN KEY([EtlSessionId])
REFERENCES [etl].[EtlSessions] ([EtlSessionId])
GO
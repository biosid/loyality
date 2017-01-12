ALTER TABLE dbo.PromoAction
  ADD 
	Approved [bit] NOT NULL,
	ApproveDescription [nvarchar](255) NULL
	
ALTER TABLE dbo.PromoActionResponse
  ADD 
	Approved [bit] NOT NULL,
	ApproveDescription [nvarchar](255) NULL
  
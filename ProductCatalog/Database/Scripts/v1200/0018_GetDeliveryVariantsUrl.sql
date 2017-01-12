if not exists(select * from [prod].[PartnerSettings] where PartnerId=1 and [key]='GetDeliveryVariants')

INSERT INTO [prod].[PartnerSettings]
           ([PartnerId]
           ,[Key]
           ,[Value])
     VALUES
           (1
           ,'GetDeliveryVariants'
           ,'https://ogrishchenko-w7.rapidsoft.local:643/Actions/GetDeliveryVariants')
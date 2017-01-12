INSERT INTO [EtlPackages]
           ([Id]
           ,[Name]
           ,[Enabled]
            )
     VALUES
           ('0A82A1F6-748F-4725-88CB-22006E4E13BE'
            ,'test_package'
            ,1
           )

INSERT INTO [EtlSessions]
           ([EtlPackageId]
           ,[EtlSessionId]
           ,[StartDateTime]
           ,[StartUtcDateTime]
           ,[EndDateTime]
           ,[EndUtcDateTime]
           ,[Status])
     VALUES
           ('0A82A1F6-748F-4725-88CB-22006E4E13BE'
           ,'7E9236E6-BAFF-4753-8505-693BDBA3A349'
           ,dateadd(day, -1, getdate())
           ,dateadd(day, -1, getutcdate())
           ,dateadd(hour, -1, getdate())
           ,dateadd(hour, -1, getutcdate())
           ,1)



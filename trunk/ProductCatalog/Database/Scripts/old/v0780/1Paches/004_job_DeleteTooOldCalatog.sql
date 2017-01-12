DECLARE @jobId binary(16)
SELECT @jobId = job_id 
FROM msdb.dbo.sysjobs WHERE (name = N'DeleteTooOldCalatog')
IF (@jobId IS NOT NULL)
BEGIN
	print 'Job DeleteTooOldCalatog allready exists'
	return
END

EXEC  msdb.dbo.sp_add_job
    @job_name = N'DeleteTooOldCalatog',
    @description=N'Delete too old calatog tables', 
	@owner_login_name=N'LoyaltyDB';

EXEC msdb.dbo.sp_add_jobstep
    @job_name = N'DeleteTooOldCalatog',
    @step_name = N'exec store procedure [prod].[DeleteTooOldCatalog]',
    @subsystem = N'TSQL',
    @command = N'EXECUTE [prod].[DeleteTooOldCatalog]', 
    @database_name=N'ProductCatalogDB';

EXEC msdb.dbo.sp_add_schedule
    @schedule_name = N'DeleteTooOldCalatog',
    @enabled = 1,
	@freq_type = 4, --daily
	@freq_interval = 1, --every day
	@freq_subday_type = 0x8, --hours
	@freq_subday_interval = 1; --each hour

EXEC msdb.dbo.sp_attach_schedule
   @job_name = N'DeleteTooOldCalatog',
   @schedule_name = N'DeleteTooOldCalatog';

EXEC msdb.dbo.sp_add_jobserver
    @job_name = N'DeleteTooOldCalatog';
GO

--select s.* from  msdb..sysjobs s 
--left join master.sys.syslogins l on s.owner_sid = l.sid

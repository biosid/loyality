if not exists
(
    select 1
    from master.dbo.sysprocesses
    where program_name = N'SQLAgent - Generic Refresher'
)
begin
    print 'Sql agent is not running or not installed'
    return
end

begin transaction

declare @ReturnCode int = 0
declare @jobId binary(16)
declare @dbName nvarchar(max) = db_name()

-- удаление существующей задачи
declare @deleteJobId binary(16)
select @deleteJobId = job_id from msdb.dbo.sysjobs where (name = N'Start Optimize Catalog Population on ProductCatalogDB.ftcProductDB')
if (@deleteJobId is not null)
begin
    exec msdb.dbo.sp_delete_job @deleteJobId
end

-- добавление задачи
exec @ReturnCode = msdb.dbo.sp_add_job
     @job_name = 'Start Optimize Catalog Population on ProductCatalogDB.ftcProductDB',
     @enabled = 1,
     @description = N'Scheduled full-text optimize catalog population for full-text catalog ftcProductDB in database ProductCatalogDB. This job was created by the Full-Text Catalog Scheduling dialog or Full-Text Indexing Wizard.', 
     @category_name = N'Full-Text', 
     @owner_login_name = N'LoyaltyDB',
     @job_id = @jobId output
if (@@error <> 0 or @ReturnCode <> 0) goto QuitWithRollback

-- добавление шага задачи
exec @ReturnCode = msdb.dbo.sp_add_jobstep
     @job_id = @jobId,
     @step_name = 'Full-Text Indexing',
     @step_id = 1,
     @cmdexec_success_code = 0,
     @on_success_action = 1,
     @on_success_step_id = -1,
     @on_fail_action = 2,
     @on_fail_step_id = -1,
     @retry_attempts = 0,
     @retry_interval = 0,
     @os_run_priority = 0,
     @subsystem = 'TSQL',
     @command = 'IF NOT EXISTS (SELECT * FROM sysfulltextcatalogs ftc WHERE ftc.name = N''ftcProductDB'')
CREATE FULLTEXT CATALOG [ftcProductDB] WITH ACCENT_SENSITIVITY = ON AS DEFAULT

IF NOT EXISTS (SELECT * FROM sys.fulltext_indexes fti WHERE fti.object_id = OBJECT_ID(N''[prod].[ProductsFromAllPartners]''))
BEGIN
   CREATE FULLTEXT INDEX ON [prod].[ProductsFromAllPartners] KEY INDEX [PK_ProductsFromAllPartners] ON ([ftcProductDB]) WITH (CHANGE_TRACKING AUTO)
   ALTER FULLTEXT INDEX ON [prod].[ProductsFromAllPartners] ADD ([AgrDescription])
   ALTER FULLTEXT INDEX ON [prod].[ProductsFromAllPartners] ADD ([Description])
   ALTER FULLTEXT INDEX ON [prod].[ProductsFromAllPartners] ADD ([Name])
   ALTER FULLTEXT INDEX ON [prod].[ProductsFromAllPartners] ENABLE
END

ALTER FULLTEXT CATALOG [ftcProductDB] REORGANIZE',
     @database_name = @dbName,
     @flags = 0
if (@@error <> 0 or @ReturnCode <> 0) goto QuitWithRollback

exec @ReturnCode = msdb.dbo.sp_update_job
     @job_id = @jobId,
     @start_step_id = 1
if (@@error <> 0 or @ReturnCode <> 0) goto QuitWithRollback

-- добавление расписания для задачи
exec @ReturnCode = msdb.dbo.sp_add_jobschedule
     @job_id = @jobId,
     @name = 'ftcProductDB Full Text Catalog',
     @enabled = 1,
     @freq_type = 4,
     @freq_interval = 1,
     @freq_subday_type = 1,
     @freq_subday_interval = 0,
     @freq_relative_interval = 0,
     @freq_recurrence_factor = 1,
     @active_start_date = 20131201,
     @active_end_date = 99991231,
     @active_start_time = 20000,
     @active_end_time = 235959,
     @schedule_uid = '612025c3-8a49-41d1-bf20-2a8a85b51876'
if (@@error <> 0 or @ReturnCode <> 0) goto QuitWithRollback

declare @startOnceDt datetime = dateadd(second, 10, getdate())
declare @startOnceDate int = datepart(year, @startOnceDt) * 10000 + datepart(month, @startOnceDt) * 100 + datepart(day, @startOnceDt)
declare @startOnceTime int = datepart(hour, @startOnceDt) * 10000 + datepart(minute, @startOnceDt) * 100 + datepart(second, @startOnceDt)

exec @ReturnCode = msdb.dbo.sp_add_jobschedule
     @job_id = @jobId,
     @name = 'ftcProductDB Full Text Catalog once',
     @enabled = 1,
     @freq_type = 1,
     @freq_interval = 1,
     @freq_subday_type = 0,
     @freq_subday_interval = 0,
     @freq_relative_interval = 0,
     @freq_recurrence_factor = 1,
     @active_start_date = @startOnceDate,
     @active_end_date = 99991231,
     @active_start_time = @startOnceTime,
     @active_end_time = 235959,
     @schedule_uid = 'da5a2ca4-d525-4a59-bc11-b83f285b4a80'
if (@@error <> 0 or @ReturnCode <> 0) goto QuitWithRollback

-- добавляем сервер для задачи
exec @ReturnCode = msdb.dbo.sp_add_jobserver
     @job_id = @jobId,
     @server_name = N'(local)'
if (@@error <> 0 or @returncode <> 0) goto QuitWithRollback

commit transaction

goto EndSave

QuitWithRollback:
    if (@@trancount > 0) rollback transaction

EndSave:
go

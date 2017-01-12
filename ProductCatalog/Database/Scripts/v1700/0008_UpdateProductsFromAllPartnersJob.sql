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

-- проверка/создание категории задач
if not exists (select name from msdb.dbo.syscategories where name=N'[Uncategorized (Local)]' and category_class =  1)
begin
    exec @ReturnCode = msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=N'[Uncategorized (Local)]'
    if (@@error <> 0 or @ReturnCode <> 0) goto QuitWithRollback
end

-- удаление существующей задачи
declare @deleteJobId binary(16)
select @deleteJobId = job_id from msdb.dbo.sysjobs where (name = N'UpdateProductsFromAllPartners')
if (@deleteJobId is not null)
begin
    exec msdb.dbo.sp_delete_job @deleteJobId
end

-- добавление задачи
exec @ReturnCode = msdb.dbo.sp_add_job
     @job_name = 'UpdateProductsFromAllPartners',
     @enabled = 1, 
     @notify_level_eventlog = 0, 
     @notify_level_email = 0, 
     @notify_level_netsend = 0, 
     @notify_level_page = 0, 
     @delete_level = 0, 
     @description = N'No description available.', 
     @category_name = N'[Uncategorized (Local)]', 
     @owner_login_name = N'LoyaltyDB', @job_id = @jobId output
if (@@error <> 0 or @ReturnCode <> 0) goto QuitWithRollback

-- добавление шага задачи
exec @ReturnCode = msdb.dbo.sp_add_jobstep
     @job_id = @jobId,
     @step_name = 'UpdateProductsFromAllPartners',
     @step_id = 1,
     @cmdexec_success_code = 0,
     @on_success_action = 1,
     @on_success_step_id = 0,
     @on_fail_action = 2,
     @on_fail_step_id = 0,
     @retry_attempts = 0,
     @retry_interval = 0,
     @os_run_priority = 0,
     @subsystem = 'TSQL',
     @command = 'exec [prod].[UpdateProductsFromAllPartners]',
     @database_name = 'ProductCatalogDB',
     @flags = 0
if (@@error <> 0 or @ReturnCode <> 0) goto QuitWithRollback

exec @ReturnCode = msdb.dbo.sp_update_job
     @job_id = @jobId,
     @start_step_id = 1
if (@@error <> 0 or @ReturnCode <> 0) goto QuitWithRollback

-- добавление расписания для задачи
exec @ReturnCode = msdb.dbo.sp_add_jobschedule
     @job_id = @jobId,
     @name = 'every 10 min',
     @enabled = 1,
     @freq_type = 4,
     @freq_interval = 1,
     @freq_subday_type = 4,
     @freq_subday_interval = 10,
     @freq_relative_interval = 0,
     @freq_recurrence_factor = 0,
     @active_start_date = 20131201,
     @active_end_date = 99991231,
     @active_start_time = 0,
     @active_end_time = 235959,
     @schedule_uid = 'aa2a73fe-ea5b-4548-81a3-ff4c8b7c3f32'
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

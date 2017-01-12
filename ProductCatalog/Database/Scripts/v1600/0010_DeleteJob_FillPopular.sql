if not exists (select 1 from master.dbo.sysprocesses where program_name = N'SQLAgent - Generic Refresher')
begin
    print 'Sql agent is not running or not installed'
    return
end

declare @jobId binary(16)
select @jobId = job_id from msdb.dbo.sysjobs where (name = N'FillPopular')
if (@jobId is null)
begin
    print 'Job FillPopular is not exists'
    return
end

exec msdb.dbo.sp_delete_job @job_id=@jobId

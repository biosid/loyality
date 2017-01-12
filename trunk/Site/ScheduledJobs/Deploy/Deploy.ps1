Write-Host "Configure " $envConfigFrontJobs
.\.tools\ctt.exe s:"ScheduledJobs.exe.config" d:"ScheduledJobs.exe.config" t:"$envConfigFrontJobs"

write-host "Deploy Quartz..." -backgroundcolor green
.\deployquartz.ps1

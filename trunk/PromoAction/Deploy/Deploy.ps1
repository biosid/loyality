write-host "configure " $envconfig
.\config\singleconfig\singleconfig.exe "$envconfig"
write-host "done"

write-host "db update..."
Push-Location '.\database'
autopatch\autopatch.exe /patchtolatest
Pop-Location

Write-Host "DeployIIS..." -backgroundcolor green
.\DeployIIS.ps1 "RapidSoft.Loaylty.PromoAction" ":57451:"

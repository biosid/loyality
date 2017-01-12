write-host "configure " $envconfig
.\config\singleconfig\singleconfig.exe "$envconfig"

write-host "db update..."
Push-Location '.\database'
autopatch\autopatch.exe /patchtolatest
Pop-Location

Write-Host "DeployIIS..." -backgroundcolor green
.\DeployIIS.ps1 "RapidSoft.VTB24.BankConnector.WebServices" ":63910:"

write-host "deployquartz..." -backgroundcolor green
.\deployquartz.ps1